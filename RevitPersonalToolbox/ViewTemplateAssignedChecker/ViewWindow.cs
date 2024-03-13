using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.ViewTemplateAssignedChecker
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
            viewTemplates.Select(x => x.Name);


        }
    }
}
