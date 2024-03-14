using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.Models;
using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.TemplateActivityChecker
{
    public class ViewModel : PropertyChangedNotifier
    {
        private readonly RevitUtils _revitUtils;
        private readonly ExternalCommandData _commandData;
        private bool _dialogResult;

        public RevitViewDataModel RevitViewDataModel { get; }
        public View SelectedItem { get; set; }
        public bool DialogResult
        {
            get => _dialogResult;
            set
            {
                _dialogResult = value;
                OnPropertyChanged(nameof(DialogResult));
            }
        }

        public ViewModel(RevitUtils revitUtils, ExternalCommandData commandData)
        {
            _revitUtils = revitUtils;
            _commandData = commandData;
            RevitViewDataModel = new RevitViewDataModel();
        }
        
        public void LoadData()
        {
            RevitViewDataModel.ViewTemplates = _revitUtils.GetViewTemplates();
            RevitViewDataModel.Views = _revitUtils.GetViews();
        }

        public void ApplySelection()
        {
            DialogResult = true;
            if (SelectedItem == null) return;
            RevitViewDataModel.ResultingViews = _revitUtils.CheckViewTemplateAssignment(SelectedItem);

            string mainTitle = $"Template: \"{SelectedItem.Name}\"";
            const string subTitle = "Has been assigned to the listed View(s)";

            GenericSelectionView resultWindow = new GenericSelectionView(mainTitle, subTitle, Utils.RevitWindow(_commandData))
            {
                //DataContext = RevitViewDataModel.ResultDictionary,
                DataContext = RevitViewDataModel.ResultingViews,
                ListBoxSelection =
                {
                DisplayMemberPath = "Name"
                }
            };

            resultWindow.ShowDialog();
        }



    }
}