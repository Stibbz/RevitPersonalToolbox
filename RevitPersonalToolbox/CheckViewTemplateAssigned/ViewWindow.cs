using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.CheckViewTemplateAssigned
{
    internal class ViewWindow
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
            IEnumerable<View> viewTemplates = _revitUtils.GetViewTemplates();


            // Create an instance of your WPF window (ViewWindow)
            SelectFromList selectFromList = new SelectFromList
            {
                MainTitle =
                {
                    Content = "Pick View Template"
                },
                Subtitle =
                {
                    Content = "Mkay."
                },
                ListBoxSelection =
                {
                    DataContext = viewTemplates.Select(x => x.Name)
                }
            };

            selectFromList.Show();
        }
    }
}
