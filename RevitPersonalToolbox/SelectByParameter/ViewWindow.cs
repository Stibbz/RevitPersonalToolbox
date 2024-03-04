using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.SelectByParameter
{
    internal class ViewWindow : IDisposable
    {
        private readonly RevitUtils _revitUtils; 
        private readonly ViewModel _viewModel;

        public ViewWindow(ViewModel viewModel, RevitUtils revitUtils)
        {
            _revitUtils = revitUtils;
            _viewModel = viewModel;
        }

        public void ShowWindow()
        {
            // Create an instance of your WPF window (ViewWindow)
            DataGridWindow dataGridWindow = new DataGridWindow
            {
                // Set the ViewModel as the DataContext for the ViewWindow
                DataContext = _viewModel
            };

            // Show the window
            dataGridWindow.Show();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
