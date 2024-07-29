//using Autodesk.Revit.UI;
//using RevitPersonalToolbox.Windows;

//namespace RevitPersonalToolbox.TemplateActivityChecker
//{
//    public class ViewModel(RevitUtils revitUtils, ExternalCommandData commandData) : PropertyChangedNotifier
//    {
//        private bool _dialogResult;

//        public object SelectedItem { get; set; }
//        // DialogResult is used to determine the result of the dialog.
//        // True when user pressed Apply, if null or false no selection was made.
//        public bool DialogResult
//        {
//            get => _dialogResult;
//            set
//            {
//                _dialogResult = value;
//                OnPropertyChanged(nameof(DialogResult));
//            }
//        }


//        public RevitViewDataModel RevitViewDataModel { get; } = new();


//        public void LoadData()
//        {
//            RevitViewDataModel.ViewTemplates = revitUtils.GetViewTemplates();
//            RevitViewDataModel.Views = revitUtils.GetViews();
//        }


//        public void ApplySelection()
//        {
//            DialogResult = true;
//            if (SelectedItem == null) return;

//            string selectedViewTemplateName = null;
//            if (SelectedItem is KeyValuePair<string, object> keyValuePair)
//            {
//                selectedViewTemplateName = keyValuePair.Key;
//            }

//            // Find View Template with the Selected View Template Name
//            View selectedViewTemplate = revitUtils.GetViewTemplates().FirstOrDefault(view => view.Name == selectedViewTemplateName);

//            string mainTitle = $"Template: \"{selectedViewTemplateName}\"";
//            const string subTitle = "Has been assigned to the listed View(s)";
//            List<View> assignedViews = revitUtils.CheckViewTemplateAssignment(selectedViewTemplate);
            
//            ViewModel resultViewModel = new(revitUtils, commandData);
//            foreach (View view in assignedViews)
//            {
//                resultViewModel.RevitViewDataModel.Properties.Add(view.Name, "secondProp");
//            }


//            SingleSelectionWindow resultWindow = new(mainTitle, subTitle, Utils.RevitWindow(commandData))
//            {
//                DataContext = resultViewModel
//            };

//            resultWindow.Show(); // Show the new window
//            // Make it so you can open the selected View(s)
//        }
//    }
//}