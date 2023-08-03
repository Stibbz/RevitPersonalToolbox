using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;

namespace RevitPersonalToolbox.SelectByParameter
{
    internal class ViewModel : INotifyPropertyChanged
    {
        // Fields
        private readonly BusinessLogic _businessLogic;
        private readonly RevitUtils _revitUtils;
        private IOrderedEnumerable<ParameterModel> _parameterModel;
        
        
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
            List<Element> selectedElements = _revitUtils.GetSelectedElements().ToList();
            
            // Get Data from Revit to populate the ParameterModels
            ParameterModel = _businessLogic.GetParameterModelData(selectedElements);
        }

        // Method to save the data to the Business Logic Layer
        public void SaveData()
        {
            _businessLogic.SaveDataToModel(ParameterModel);
        }
        
        private void SchimmelPenninckIsZielig()
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
            // IOrderedEnumerable<ParameterModel> dataModelParameters = _businessLogic.GetParameterModelData(selectedElements);
            
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
        
        // Events
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
