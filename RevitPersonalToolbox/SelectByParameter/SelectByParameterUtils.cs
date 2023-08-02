using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitPersonalToolbox.SelectByParameter
{
    internal static class SelectByParameterUtils
    {
        /// <summary>
        /// Populate DataTable using Dictionary
        /// </summary>
        /// <param name="distinctNamesAndValues"></param>
        /// <param name="dataTable"></param>
        internal static void PopulateDataTable(Dictionary<string, List<string>> distinctNamesAndValues, DataTable dataTable)
        {
            foreach (KeyValuePair<string, List<string>> keyValuePair in distinctNamesAndValues)
            {
                string name = keyValuePair.Key;
                List<string> values = keyValuePair.Value;

                // Determine if a key has single or multiple distinct values
                List<string> distinctValues = values.Distinct().ToList();
                string displayValue = (distinctValues.Count > 1) ? "<varies>" : values[0];

                dataTable.Rows.Add(name, displayValue);
            }
        }

        /// <summary>
        /// Iterate through the List to populate the DataTable
        /// </summary>
        /// <param name="dataModelParameters"></param>
        /// <returns></returns>
        internal static Dictionary<string, List<string>> GetDistinctNamesAndValues(IOrderedEnumerable<ParameterModel> dataModelParameters)
        {
            Dictionary<string, List<string>> distinctNamesAndValues = new Dictionary<string, List<string>>();
            foreach (ParameterModel parameter in dataModelParameters)
            {
                if (distinctNamesAndValues.ContainsKey(parameter.Name))
                {
                    distinctNamesAndValues[parameter.Name].Add(parameter.Value);
                }
                else
                {
                    distinctNamesAndValues.Add(parameter.Name, new List<string> {parameter.Value});
                }
            }

            return distinctNamesAndValues;
        }

        internal static DataTable CreateDataTable()
        {
            // Initialize DataTable
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));
            return dataTable;
        }

        internal static IOrderedEnumerable<ParameterModel> GetParametersAndValues(IEnumerable<Element> selectedElements)
        {
            List<ParameterModel> dataModelParameters = new List<ParameterModel>();
            foreach (Element selectedElement in selectedElements)
            {
                IEnumerable<Parameter> parameters = selectedElement.GetOrderedParameters();
                dataModelParameters.AddRange(parameters.Select(parameter => new ParameterModel()));
            }

            IOrderedEnumerable<ParameterModel> sortedDataModelParameters = dataModelParameters.OrderBy(x => x.Name);

            return sortedDataModelParameters;
        }
    }
}