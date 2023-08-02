using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Input;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.SelectByParameter
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {
        /* Summary
            1. Get selected elements and parameter data 
            2. Display form showing distinct Parameters and their values (<varies> if varying values).
            3. User selects the desired parameter value to search for.
            4. Get every element in the project and match with user input
            5. Select every corresponding element
            6. (optional) Pop-up suggesting to isolate selection OR temporarily colorize for visibility
        */

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public Command(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }




        public Command ButtonClickCommand { get; }

        public Command()
        {
            ButtonClickCommand = new Command(OnButtonClick);
        }

        private void OnButtonClick(object parameter)
        {
            TaskDialog.Show("info", "SHOW EM MY WATCH MAMA");
        }

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
            IOrderedEnumerable<ParameterModel> dataModelParameters = SelectByParameterUtils.GetParametersAndValues(selectedElements);

            // Get only distinct Parameters and their values (<varies> if varying values).
            Dictionary<string, List<string>> distinctNamesAndValues = SelectByParameterUtils.GetDistinctNamesAndValues(dataModelParameters);

            // Initialize Datatable and populate with distinct Parameters and values
            DataTable dataTable = SelectByParameterUtils.CreateDataTable();
            SelectByParameterUtils.PopulateDataTable(distinctNamesAndValues, dataTable);

            // Bind DataTable to WPF Form by name.
            // REVIEW: Understand better how the DataGrid is binding to the DataTable (in .XAML look for: "ItemsSource="{Binding }" )
            dataGridWindow.DataContext = dataTable;
            dataGridWindow.ShowDialog();




            return Result.Succeeded;
        }
    }
}