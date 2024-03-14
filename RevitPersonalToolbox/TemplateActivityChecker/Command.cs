using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.Windows;

// MVVM pattern:
// - Model: This represents the data and business logic. => "RevitUtils.cs & Utils.cs"
// - View: This is the user interface or the presentation layer, it defines how the data is presented to the user. => "SingleSelectionWindow (XAML and code-behind)" 
// - ViewModel: This acts as the intermediary between the View and the Model. => ViewModel.cs

namespace RevitPersonalToolbox.TemplateActivityChecker
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ViewTemplateAssignedChecker : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RevitUtils revitUtils = new RevitUtils(commandData);
            ViewModel viewModel = new ViewModel(revitUtils, commandData);


            const string mainTitle = "Pick a View Template";
            const string subTitle = "Check to which View(s) this Template has been assigned";
            SingleSelectionWindow mainWindow = new SingleSelectionWindow(mainTitle, subTitle,Utils.RevitWindow(commandData))
            {
                DataContext = viewModel,
                ListBoxSelection =
                {
                    ItemsSource = revitUtils.GetViewTemplates(),
                    DisplayMemberPath = "Name"
                }
            };

            mainWindow.ShowDialog();

            return mainWindow.Cancelled ? Result.Cancelled : Result.Succeeded;
        }
    }
}