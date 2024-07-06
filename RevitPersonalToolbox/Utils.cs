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
        public static void CreateViewFilter(Document doc, View view, ICollection<ElementId> categories, string parameterValue)
        {
            List<FilterRule> filterRules = [];

            using (Transaction t = new(doc, "Add view filter"))
            {
                t.Start();

                // Create filter element associated to the input categories

                ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(doc, "Example view filter", categories);

                ElementId familyNameId = new(BuiltInParameter.ALL_MODEL_FAMILY_NAME);
                filterRules.Add(ParameterFilterRuleFactory.CreateContainsRule(familyNameId, parameterValue));



                //// Criterion 1 - wall type Function is "Exterior"
                //ElementId exteriorParamId = new(BuiltInParameter.FUNCTION_PARAM);
                //filterRules.Add(ParameterFilterRuleFactory.CreateEqualsRule(exteriorParamId, (int)WallFunction.Exterior));

                //// Criterion 2 - wall height > some number
                //ElementId lengthId = new(BuiltInParameter.CURVE_ELEM_LENGTH);
                //filterRules.Add(ParameterFilterRuleFactory.CreateGreaterOrEqualRule(lengthId, 28.0, 0.0001));

                ElementFilter elementFilter = CreateElementFilterFromFilterRules(filterRules);
                parameterFilterElement.SetElementFilter(elementFilter);

                // Apply filter to view
                view.AddFilter(parameterFilterElement.Id);
                view.SetFilterVisibility(parameterFilterElement.Id, false);
                t.Commit();
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