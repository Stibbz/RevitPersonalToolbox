using System.Windows;
using System.Windows.Interop;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox
{
    public class Utils(ExternalCommandData commandData)
    {
        private ExternalCommandData CommandData { get; set; } = commandData;
        private UIDocument UiDocument { get; set; } = commandData.Application.ActiveUIDocument;
        private Document Document { get; set; } = commandData.Application.ActiveUIDocument.Document;

        public static Dictionary<string, dynamic> SortDictionary(Dictionary<string, dynamic> dictionary)
        {
            Dictionary<string, dynamic> result = new();
            IList<string> sortedKeys = dictionary.Keys.ToList().OrderBy(x => x).ToList();

            foreach (string key in sortedKeys)
            {
                result.Add(key, dictionary[key]);

            }

            return result;
        }

        public static Window RevitWindow(ExternalCommandData commandData)
        {
            IntPtr RevitWindowHandle = commandData.Application.MainWindowHandle;
            HwndSource hwndSource = HwndSource.FromHwnd(RevitWindowHandle);
            Window RevitWindow = hwndSource.RootVisual as Window;
            return RevitWindow;
        }

        /// <summary>
        /// Creates a new view filter matching multiple criteria.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="view"></param>
        /// <param name="categories"></param>
        /// <param name="parameterValue"></param>
        public void CreateViewFilter(Document doc, View view, string filterName, IEnumerable<Element> selectedElements, string parameterValue)
        {
            List<FilterRule> filterRules = [];

            using (Transaction t = new(doc, "Add view filter"))
            {
                t.Start();

                // Create filter element using the provided categories
                List<ElementId> categories = selectedElements.Select(x => x.Category.Id).ToList();
                ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(doc, filterName, categories);

                IEnumerable<Parameter> parameters = null;
                Parameter testParam = null;

                foreach (Element element in selectedElements)
                {
                    // Get Parameters from Element
                    parameters = element.GetOrderedParameters();

                    // Pick specific parameter for testing
                    testParam = element.LookupParameter("Length");
                }

                
                var value = GetParameterValue(testParam);


                //// Criterion 1 - wall type Function is "Exterior"
                //ElementId exteriorParamId = new(BuiltInParameter.FUNCTION_PARAM);
                //filterRules.Add(ParameterFilterRuleFactory.CreateEqualsRule(exteriorParamId, (int)WallFunction.Exterior));

                filterRules.Add(ParameterFilterRuleFactory.CreateGreaterOrEqualRule(testParam.Id, parameterValue));
                ElementFilter elementFilter = CreateElementFilterFromFilterRules(filterRules);
                parameterFilterElement.SetElementFilter(elementFilter);

                // Apply filter to view
                view.AddFilter(parameterFilterElement.Id);
                view.SetFilterVisibility(parameterFilterElement.Id, false);
                t.Commit();
            }
        }
        
        private string GetParameterValue(Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    return parameter.AsValueString();
                case StorageType.ElementId:
                    return parameter.AsValueString();
                case StorageType.Integer:
                    return parameter.AsValueString();
                case StorageType.None:
                    return parameter.AsValueString();
                case StorageType.String:
                    return parameter.AsString();
                default:
                    return null;
            }
        }
        
        /// <summary>
        /// Create an ElementFilter representing a conjunction ("ANDing together") of FilterRules.
        /// </summary>
        /// <param name="filterRules">A list of FilterRules</param>
        /// <returns>The ElementFilter.</returns>
        private static ElementFilter CreateElementFilterFromFilterRules(IList<FilterRule> filterRules)
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
    }
}