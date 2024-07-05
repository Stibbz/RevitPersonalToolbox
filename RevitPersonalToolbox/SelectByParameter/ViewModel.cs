using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

namespace RevitPersonalToolbox.SelectByParameter
{
    internal class ViewModel : INotifyPropertyChanged
    {
        // Fields
        private readonly BusinessLogic _businessLogic;
        private readonly RevitUtils _revitUtils;
        private IOrderedEnumerable<ParameterModel> _parameterModel;
        private ParameterModel _distinctParameterModel;
        
        
        // Properties
        public IOrderedEnumerable<ParameterModel> ParameterModel
        {
            get => _parameterModel;
            set
            {
                _parameterModel = value;
                OnPropertyChanged(nameof(ParameterModel));
            }
        }

        public ParameterModel DistinctParameterModel
        {
            get => _distinctParameterModel;
            set
            {
                _distinctParameterModel = value;
                OnPropertyChanged(nameof(DistinctParameterModel));
            }
        }
        
        // Constructors
        public ViewModel(BusinessLogic businessLogic, RevitUtils revitUtils)
        {
            _businessLogic = businessLogic;
            _revitUtils = revitUtils;

            LoadData();
        }


        // Methods
        public void LoadData()
        {
            // Get all selected elements
            List<Element> selectedElements = _revitUtils.GetSelectedElements()?.ToList() ?? new List<Element>();
            

            // Refactor this stuff so we actually end up with distinct Parameters only...

            // Get Data from Revit to populate the ParameterModels
            // This should probably not become the ParameterModel yet??
            ParameterModel = _businessLogic.GetParameterData(selectedElements);

            List<ParameterModel> test = ParameterModel.ToList();
            test.RemoveRange(4, ParameterModel.Count() - 4);
            //DistinctParameterModel = _businessLogic.GetDistinctParameters(selectedElements);
            DistinctParameterModel = test[0];

            //DataTable dataTable = CreateDataTable();
            //PopulateDataTable(distinctNamesAndValues, dataTable);

        }

        // Method to save the data to the Business Logic Layer
        public void SaveData()
        {
            _businessLogic.SaveDataToModel(ParameterModel);
        }
        
        private void UnstructuredWorkingVersion()
        {
            // // Get all selected elements
            // List<Element> selectedElements = _revitUtils.GetSelectedElements().ToList();
            // if (!selectedElements.Any())
            // {
            //     TaskDialog.Show("Error", "Select items first.");
            // }
            //
            // Create WPF Form with DataGrid
            // DataGridWindow dataGridWindow = new DataGridWindow { TitleLabel = { Content = "Select by Parameter" } };
            
            // // Get all Parameters and their Values from every selected Element
            // IOrderedEnumerable<ParameterModel> dataModelParameters = _businessLogic.GetParameterData(selectedElements);
            
            // // Get only distinct Parameters and their values (<varies> if varying values).
            // Dictionary<string, List<string>> distinctNamesAndValues = _businessLogic.GetDistinctNames(dataModelParameters);

            // Initialize Datatable and populate with distinct Parameters and values
            // DataTable dataTable = _businessLogic.CreateDataTable();
            // _businessLogic.PopulateDataTable(distinctNamesAndValues, dataTable);

            // Bind DataTable to WPF Form by name.
            // REVIEW: Understand better how the DataGrid is binding to the DataTable (in .XAML look for: "ItemsSource="{Binding }" )
            // dataGridWindow.DataContext = dataTable;
            //dataGridWindow.ShowDialog();
        }

        /// <summary>
        /// Populate DataTable using Dictionary
        /// </summary>
        /// <param name="distinctNamesAndValues"></param>
        /// <param name="dataTable"></param>
        private void PopulateDataTable(Dictionary<string, List<string>> distinctNamesAndValues, DataTable dataTable)
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
        
        // Events
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
