using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.TemplateActivityChecker
{
    public class ViewModel : PropertyChangedNotifier
    {
        private readonly RevitUtils _revitUtils;
        private readonly ExternalCommandData _commandData;
        private bool _dialogResult;

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
        }

        public void ApplySelection()
        {
            DialogResult = true;
            if (SelectedItem == null) return;

            string mainTitle = $"Template: \"{SelectedItem.Name}\"";
            const string subTitle = "Has been assigned to the listed View(s)";

            GenericSelectionView resultWindow = new GenericSelectionView(mainTitle, subTitle, Utils.RevitWindow(_commandData))
            {
                DataContext = _revitUtils.CheckViewTemplateAssignment(SelectedItem),
                ListBoxSelection =
                {
                DisplayMemberPath = "Name"
                }
            };

            resultWindow.ShowDialog();
        }



    }
}