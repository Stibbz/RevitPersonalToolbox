using System.Data;
using System.Reflection;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox;

public class RevitUtils(ExternalCommandData commandData)
{
    private UIDocument UiDocument { get; } = commandData.Application.ActiveUIDocument;
    private Document Document { get; } = commandData.Application.ActiveUIDocument.Document;


    public static DataTable CreateDataTable<T>(IEnumerable<T> list)
    {
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();

        DataTable dataTable = new();
        dataTable.TableName = typeof(T).FullName;
        foreach (PropertyInfo info in properties)
        {
            dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
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
        return new FilteredElementCollector(Document)
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
        return new FilteredElementCollector(Document, Document.ActiveView.Id)
            .OfCategory(BuiltInCategory.OST_RvtLinks)
            .OfClass(typeof(RevitLinkInstance))
            .ToElements();
    }

    public ICollection<Element> GetAllSelectedElements()
    {
        IEnumerable<ElementId> selection = UiDocument.Selection.GetElementIds();
        List<Element> selectedElements = selection.Select(id => Document.GetElement(id)).ToList();

        if (selectedElements.Any()) return selectedElements;

        TaskDialog.Show("Error", "Select items first.");
        return null;
    }

    public IEnumerable<View> GetViewTemplates()
    {
        return new FilteredElementCollector(Document)
            .OfClass(typeof(View))
            .Cast<View>()
            .Where(v => v.IsTemplate)
            .Where(v => v.ViewType != ViewType.Schedule);
    }

    public FilteredElementCollector GetFilterElements()
    {
        return new FilteredElementCollector(Document)
            .OfClass(typeof(FilterElement));
    }

    public FilteredElementCollector GetRevitLinks()
    {
        return new FilteredElementCollector(Document)
            .OfCategory(BuiltInCategory.OST_RvtLinks)
            .OfClass(typeof(RevitLinkType));
    }

    public IEnumerable<View> GetViews()
    {
        return new FilteredElementCollector(Document)
            .WhereElementIsNotElementType()
            .OfClass(typeof(View))
            .Cast<View>()
            .Where(v => !v.IsTemplate)
            .Where(v => v.CanUseTemporaryVisibilityModes());
    }

    public Dictionary<string, dynamic> GetParameterData(ICollection<Element> allSelectedElements, ICollection<ElementId> parameters)
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

    public ParameterFilterElement CreateViewFilter(ICollection<Element> elements, ElementId parameters, string filterName, string value)
    {
        using Transaction t = new(Document, "Created Filter based on selection");
        t.Start();

        // Create filter rules (i.e. "length =< 100")
        List<FilterRule> filterRules = [];
        filterRules.Add(ParameterFilterRuleFactory.CreateGreaterOrEqualRule(parameters, value, false));
        ElementFilter elementFilter = CreateElementFilterFromFilterRules(filterRules);

        // Create filter using the filter rules
        ICollection<ElementId> categories = elements.Select(x => x.Category.Id).ToList();
        ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(Document, filterName, categories);
            
        parameterFilterElement.SetElementFilter(elementFilter);

        t.Commit();

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
        ICollection<ElementId> filterableParameterIds = ParameterFilterUtilities.GetFilterableParametersInCommon(Document, categories);
            
        return filterableParameterIds;
    }

    public void ApplyFilterToView(ParameterFilterElement viewFilter)
    {
        View view = Document.ActiveView;

        view.AddFilter(viewFilter.Id);
        view.SetFilterVisibility(viewFilter.Id, false);
    }
}