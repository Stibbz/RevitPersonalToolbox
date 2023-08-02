using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.Attributes;

namespace RevitPersonalToolbox.SelectByParameter
{
    internal class ViewModel : INotifyPropertyChanged
    {
        private readonly BusinessLogic _businessLogic;

        public ViewModel()
        {
            _businessLogic = new BusinessLogic();
        }

        // Property to hold the data from the Model
        private ParameterModel data;
        public ParameterModel Data
        {
            get { return data; }
            set
            {
                data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        // Method to load the data from the Business Logic Layer
        public void LoadData()
        {
            Data = _businessLogic.GetDataFromRevit();
        }

        // Method to save the data to the Business Logic Layer
        public void SaveData()
        {
            _businessLogic.SaveDataToModel(Data);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
