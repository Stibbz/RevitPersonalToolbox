using System.Data;
using System.Reflection;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.CreateDirectFilter.Windows;

namespace RevitPersonalToolbox;

public class RevitUtils(Document document, UIDocument uiDocument)
{
    //private UIDocument UiDocument { get; } = commandData.Application.ActiveUIDocument;
    //private Document Document { get; } = commandData.Application.ActiveUIDocument.Document;

    public static DataTable CreateDataTable<T>(IEnumerable<T> list)
    {
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();

        DataTable dataTable = new();
        dataTable.TableName = typeof(T).FullName;
        foreach (PropertyInfo info in properties)
        {
            dataTable.Columns.Add(new DataColumn(info.Name,
                Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
        }

        foreach (T entity in list)
        {
            object[] values = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(entity);
            }

            dataTable.Rows.Add(values);
        }

        return dataTable;
    }

    public IEnumerable<Element> SelectAllObservableElements()
    {
        // Select anything that is observable in Revit
        return new FilteredElementCollector(document)
            .WhereElementIsNotElementType()
            .WhereElementIsViewIndependent()
            .Where(IsObservable);
    }
    
    private static bool IsObservable(Element e)
    {
        // Filter to exclude anything that is not observable in Revit
        if (e.Category == null) return false;
        if (e.ViewSpecific) return false;

        // exclude specific unwanted categories (i.e. Voids)
        if ((BuiltInCategory)e.Category.Id.IntegerValue == BuiltInCategory.OST_HVAC_Zones) return false;

        return e.Category.CategoryType == CategoryType.Model && e.Category.CanAddSubcategory;
    }

    public IEnumerable<Element> SelectRevitLinks()
    {
        return new FilteredElementCollector(document, document.ActiveView.Id)
            .OfCategory(BuiltInCategory.OST_RvtLinks)
            .OfClass(typeof(RevitLinkInstance))
            .ToElements();
    }

    /// <summary>
    /// Retrieves all selected elements from the current selection in the UI document.
    /// </summary>
    /// <returns>A collection of selected elements.</returns>
    public ICollection<Element> GetAllSelectedElements()
    {
        IEnumerable<ElementId> selection = uiDocument.Selection.GetElementIds();
        List<Element> selectedElements = selection.Select(document.GetElement).ToList();

        if(selectedElements.Any()) return selectedElements;
        TaskDialog.Show("Error", "Select items first.");
        return null;
    }

    /// <summary>
    /// Retrieves all selected elements from the current selection in the UI document.
    /// Provide a list of type names to invalidate the result.
    /// </summary>
    /// <param name="invalidTypeNames">A list of type names that will invalidate the element retrieval.</param>
    /// <returns>A collection of selected elements. Returns null if an invalid family type is included in the selection.</returns>
    public ICollection<Element> GetAllSelectedElements(List<string> invalidTypeNames)
    {
        IEnumerable<ElementId> selection = uiDocument.Selection.GetElementIds();
        List<Element> selectedElements = selection.Select(document.GetElement).ToList();

        foreach (string typeName in invalidTypeNames)
        {
            if (selectedElements.All(element => element.GetType().Name != typeName)) continue;
            TaskDialog.Show("Error", $"Element of type \"{typeName}\" is not valid!\nPlease try again without this element selected.");
            return null;
        }

        if(selectedElements.Any()) return selectedElements;
        TaskDialog.Show("Error", "Select items first.");
        return null;
    }

    public IEnumerable<View> GetViewTemplates()
    {
        return new FilteredElementCollector(document)
            .OfClass(typeof(View))
            .Cast<View>()
            .Where(v => v.IsTemplate)
            .Where(v => v.ViewType != ViewType.Schedule);
    }

    public FilteredElementCollector GetFilterElements()
    {
        return new FilteredElementCollector(document)
            .OfClass(typeof(FilterElement));
    }

    public FilteredElementCollector GetRevitLinks()
    {
        return new FilteredElementCollector(document)
            .OfCategory(BuiltInCategory.OST_RvtLinks)
            .OfClass(typeof(RevitLinkType));
    }

    public IEnumerable<View> GetViews()
    {
        return new FilteredElementCollector(document)
            .WhereElementIsNotElementType()
            .OfClass(typeof(View))
            .Cast<View>()
            .Where(v => !v.IsTemplate)
            .Where(v => v.CanUseTemporaryVisibilityModes());
    }

    public Dictionary<string, dynamic> GetParameterData(ICollection<Element> allSelectedElements,
        ICollection<ElementId> parameters)
    {
        Dictionary<string, dynamic> parameterDictionary = [];
        foreach (Element element in allSelectedElements)
        {
            foreach (ElementId parameter in parameters)
            {
                BuiltInParameter bip = (BuiltInParameter)parameter.IntegerValue;
                string name = GetParameterName(bip, element);

                if (string.IsNullOrEmpty(name)) continue;
                if (!parameterDictionary.ContainsKey(name))
                {
                    parameterDictionary.Add(name, parameter);
                }
            }
        }

        return parameterDictionary;
    }

    public string GetParameterName(BuiltInParameter bip, Element element)
    {
        try
        {
            return LabelUtils.GetLabelFor(bip);
        }
        catch (Autodesk.Revit.Exceptions.InvalidOperationException)
        {
            try
            {
                return element.get_Parameter(bip).Definition.Name;
            }
            catch
            {
                return null;
            }
        }
    }

    public List<View> CheckViewTemplateAssignment(View selectedViewTemplate)
    {
        //ParameterDictionary<string, dynamic> resultDictionary = new ParameterDictionary<string, dynamic>();
        List<View> assignedViews = [];
        IEnumerable<View> views = GetViews();
        foreach (View view in views)
        {
            if (view.ViewTemplateId != selectedViewTemplate.Id) continue;
            assignedViews.Add(view);
        }

        return assignedViews;
    }

    public ParameterFilterElement CreateViewFilter(ICollection<Element> elements, ElementId parameter,
        string filterName, EnterFilterValues.ComparisonRule comparisonRule, string value)
    {
        List<FilterRule> filterRules = [];

        // Define a dictionary to map ComparisonRule to the corresponding method
        Dictionary<EnterFilterValues.ComparisonRule, Func<ElementId, string, bool, FilterRule>> ruleFactoryMethods = new()
        {
            { EnterFilterValues.ComparisonRule.Equals, ParameterFilterRuleFactory.CreateEqualsRule },
            { EnterFilterValues.ComparisonRule.DoesNotEqual, ParameterFilterRuleFactory.CreateNotEqualsRule },
            { EnterFilterValues.ComparisonRule.IsGreaterThan, ParameterFilterRuleFactory.CreateGreaterRule },
            { EnterFilterValues.ComparisonRule.IsGreaterThanOrEqualTo, ParameterFilterRuleFactory.CreateGreaterOrEqualRule },
            { EnterFilterValues.ComparisonRule.IsLessThan, ParameterFilterRuleFactory.CreateLessRule },
            { EnterFilterValues.ComparisonRule.IsLessThanOrEqualTo, ParameterFilterRuleFactory.CreateLessOrEqualRule },
            { EnterFilterValues.ComparisonRule.Contains, ParameterFilterRuleFactory.CreateContainsRule },
            { EnterFilterValues.ComparisonRule.DoesNotContain, ParameterFilterRuleFactory.CreateNotContainsRule },
            { EnterFilterValues.ComparisonRule.BeginsWith, ParameterFilterRuleFactory.CreateBeginsWithRule },
            { EnterFilterValues.ComparisonRule.DoesNotBeginWith, ParameterFilterRuleFactory.CreateNotBeginsWithRule },
            { EnterFilterValues.ComparisonRule.EndsWith, ParameterFilterRuleFactory.CreateEndsWithRule },
            { EnterFilterValues.ComparisonRule.DoesNotEndWith, ParameterFilterRuleFactory.CreateNotEndsWithRule }
        };
       
        // Get the appropriate method based on the comparisonRule
        if (ruleFactoryMethods.TryGetValue(comparisonRule, out Func<ElementId, string, bool, FilterRule> createRuleMethod))
        {
            filterRules.Add(createRuleMethod(parameter, value, false));
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(comparisonRule), comparisonRule, null);
        }

        ElementFilter elementFilter = CreateElementFilterFromFilterRules(filterRules);

        // Create filter using the filter rules
        ICollection<ElementId> categories = elements.Select(x => x.Category.Id).ToList();

        ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(document, filterName, categories);
        parameterFilterElement.SetElementFilter(elementFilter);

        return parameterFilterElement;
    }

    public static ElementFilter CreateElementFilterFromFilterRules(IList<FilterRule> filterRules)
    {
        // We use a LogicalAndFilter containing one ElementParameterFilter for each FilterRule.
        // We could alternatively create a single ElementParameterFilter containing the entire list of FilterRules.
        IList<ElementFilter> elementFilters = new List<ElementFilter>();
        foreach (FilterRule filterRule in filterRules)
        {
            ElementParameterFilter elementParameterFilter = new(filterRule);
            elementFilters.Add(elementParameterFilter);
        }

        LogicalAndFilter elemFilter = new(elementFilters);

        return elemFilter;
    }

    public ICollection<ElementId> GetApplicableParameters(ICollection<Element> allSelectedElements)
    {
        if (allSelectedElements == null) return null;

        ICollection<ElementId> categories = allSelectedElements.Select(x => x.Category.Id).ToList();
        ICollection<ElementId> filterableParameterIds =
            ParameterFilterUtilities.GetFilterableParametersInCommon(document, categories);

        return filterableParameterIds;
    }

    public void ApplyFilterToView(ICollection<ParameterFilterElement> viewFilters)
    {
        View view = document.ActiveView;
        if (view.ViewTemplateId != null)
        {
            ElementId viewTemplateId = view.ViewTemplateId;
            if (viewTemplateId == ElementId.InvalidElementId)
            {
                viewTemplateId = view.Id;
            }

            View viewTemplate = (View)document.GetElement(viewTemplateId);
            foreach (ParameterFilterElement viewFilter in viewFilters)
            {
                viewTemplate.AddFilter(viewFilter.Id);
                viewTemplate.SetFilterVisibility(viewFilter.Id, false);
            }
        }
        else
        {
            foreach (ParameterFilterElement viewFilter in viewFilters)
            {
                view.AddFilter(viewFilter.Id);
                view.SetFilterVisibility(viewFilter.Id, false);
            }
        }
    }

    public List<string> GetParameterValues(ICollection<Element> selectedElements, ElementId selectedParameter)
    {
        List<string> parameterValuesInSelection = new();
        foreach (Element element in selectedElements)
        {
            BuiltInParameter bip = (BuiltInParameter)selectedParameter.IntegerValue;
            string parameterName = GetParameterName(bip, element);
            Parameter parameter = element.LookupParameter(parameterName);
            if (parameter?.AsValueString() == null) return null;
            parameterValuesInSelection.Add(parameter.AsValueString());
        }

        return parameterValuesInSelection;
    }
}