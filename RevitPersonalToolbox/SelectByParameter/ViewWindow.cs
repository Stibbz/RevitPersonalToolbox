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
        private readonly Utils _utils; 
        private readonly ViewModel _viewModel;

        public ViewWindow(Utils utils, ViewModel viewModel)
        {
            _utils = utils;
            _viewModel = viewModel;
        }

        public void ShowWindow()
        {
            // Get all selected elements
            List<Element> selectedElements = _utils.GetSelectedElements().ToList();
            if (!selectedElements.Any())
            {
                TaskDialog.Show("Error", "Select items first.");
                return;
            }

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
