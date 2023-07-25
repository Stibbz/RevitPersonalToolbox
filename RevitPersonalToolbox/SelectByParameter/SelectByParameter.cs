using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.SelectByParameter
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class SelectByParameter : IExternalCommand
    {
        /* Summary
            1. Get selected elements and parameter data 
            2. Display form showing distinct Parameters and their values (<varies> if varying values).
            3. User selects the desired parameter value to search for.
            4. Get every element in the project and match with user input
            5. Select every corresponding element
            6. (optional) Pop-up suggesting to isolate selection OR temporarily colorize for visibility
        */

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Utils utils = new Utils(commandData);

            // Get all selected elements
            List<Element> selectedElements = utils.GetSelectedElements().ToList();
            if (!selectedElements.Any())
            {
                TaskDialog.Show("Error", "Select items first.");
                return Result.Failed;
            }

            // Create WPF Form with DataGrid
            DataGridWindow dataGridWindow = new DataGridWindow { TitleLabel = { Content = "Select by Parameter" } };

            // Get all Parameters and their Values from every selected Element
            IOrderedEnumerable<DataModelParameter> dataModelParameters = SelectByParameterUtils.GetParametersAndValues(selectedElements);
            
            // Get only distinct Parameters and their values (<varies> if varying values).
            Dictionary<string, List<string>> distinctNamesAndValues = SelectByParameterUtils.GetDistinctNamesAndValues(dataModelParameters);
            
            // Initialize Datatable and populate with distinct Parameters and values
            DataTable dataTable = SelectByParameterUtils.CreateDataTable();
            SelectByParameterUtils.PopulateDataTable(distinctNamesAndValues, dataTable);

            // Bind DataTable to WPF Form by name.
            // REVIEW: Understand better how the DataGrid is binding to the DataTable (in .XAML look for: "ItemsSource="{Binding }" )
            dataGridWindow.DataContext = dataTable.DefaultView;
            dataGridWindow.ShowDialog();
            
            

            return Result.Succeeded;
        }
    }

}
