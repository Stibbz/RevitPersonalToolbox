using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitPersonalToolbox.SelectByParameter
{
    internal class SelectByParameterUtils
    {
        /// <summary>
        /// Populate DataTable using Dictionary
        /// </summary>
        /// <param name="distinctNamesAndValues"></param>
        /// <param name="dataTable"></param>
        internal void PopulateDataTable(Dictionary<string, List<string>> distinctNamesAndValues, DataTable dataTable)
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

        internal DataTable CreateDataTable()
        {
            // Initialize DataTable
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));
            return dataTable;
        }


    }
}