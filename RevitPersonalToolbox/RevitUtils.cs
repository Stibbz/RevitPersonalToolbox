using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Autodesk.Revit.UI;
using Nice3point.Revit.Extensions;

namespace RevitPersonalToolbox
{
    public class RevitUtils
    {
        private ExternalCommandData CommandData { get; set; }
        private UIDocument UiDocument { get; set; }
        private Document Document { get; set; }


        public RevitUtils(ExternalCommandData commandData)
        {
            CommandData = commandData;
            UiDocument = commandData.Application.ActiveUIDocument;
            Document = commandData.Application.ActiveUIDocument.Document;
        }


        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            DataTable dataTable = new DataTable();
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

        public IEnumerable<Parameter> GetAllParameters()
        {
            return new FilteredElementCollector(Document)
                .OfClass(typeof(Parameter))
                .Cast<Parameter>();
        }

        public List<View> CheckViewTemplateAssignment(View selectedViewTemplate)
        {
            //Dictionary<string, dynamic> resultDictionary = new Dictionary<string, dynamic>();
            List<View> assignedViews = new List<View>();
            IEnumerable<View> views = GetViews();
            foreach (View view in views)
            {
                if (view.ViewTemplateId != selectedViewTemplate.Id) continue;
                assignedViews.Add(view);
            }

            return assignedViews;
        }

        /// <summary>
        /// Creates a new view filter matching multiple criteria.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="view"></param>
        /// <param name="filterName"></param>
        /// <param name="selectedElements"></param>
        /// <param name="parameterValue"></param>
        public void CreateViewFilter(ICollection<Element> selectedElements, Parameter selectedParameter, string filterName, string parameterValue)
        {
            using Transaction t = new(Document, "Add view filter");
            t.Start();

            Parameter parameter = selectedParameter;

            // Create filter rules (i.e. "length =< 100")
            List<FilterRule> filterRules = [];
            filterRules.Add(ParameterFilterRuleFactory.CreateGreaterOrEqualRule(parameter.Id, parameterValue, false));
            ElementFilter elementFilter = CreateElementFilterFromFilterRules(filterRules);

            // Create filter using the filter rules
            // TODO: Add fallback mechanism when name is already in use
            ICollection<ElementId> categories = selectedElements.Select(x => x.Category.Id).ToList();
            ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(Document, filterName, categories);
            ParameterFilterElement.AllRuleParametersApplicable(Document, categories, elementFilter);

            //ParameterFilterElement.AllRuleParametersApplicable(Document, categories, elementFilter);
            //ParameterFilterElement.AllRuleParametersApplicable(elementFilter);


            parameterFilterElement.SetElementFilter(elementFilter);

            //// Apply filter to view
            //view.AddFilter(parameterFilterElement.Id);
            //view.SetFilterVisibility(parameterFilterElement.Id, false);
            t.Commit();
        }

        /// <summary>
        /// Create an ElementFilter representing a conjunction ("ANDing together") of FilterRules.
        /// </summary>
        /// <param name="filterRules">A list of FilterRules</param>
        /// <returns>The ElementFilter.</returns>
        internal static ElementFilter CreateElementFilterFromFilterRules(IList<FilterRule> filterRules)
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

        public ICollection<ElementId> CreateApplicableElementFilters(ICollection<Element> allSelectedElements)
        {
            if (allSelectedElements == null) return null;
            ICollection<ElementId> categories = allSelectedElements.Select(x => x.Category.Id).ToList();

            string parameterValue = "to be replaced";

            ICollection<ElementId> filterableParameterIds = ParameterFilterUtilities.GetFilterableParametersInCommon(Document, categories);
            ICollection<Parameter> filterableParameters = new List<Parameter>();

            // This whole foreach loop was created before finding out about GetFilterableParametersInCommon(). Therefore it's most probably obsolete now, since every parameter should always pass anyways.
            foreach (ElementId parameter in filterableParameterIds)
            {
                BuiltInParameter bip = (BuiltInParameter)parameter.IntegerValue;
                string label = LabelUtils.GetLabelFor(bip);

                //filterableParameters.Add(allSelectedElements.First().LookupParameter(label));
                Element element = allSelectedElements.First();
                filterableParameters.Add(element.FindParameter(bip));
            }

            return filterableParameterIds;
        }
    }
}