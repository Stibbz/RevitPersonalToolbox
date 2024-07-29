//using System.Windows;
//using Autodesk.Revit.Attributes;
//using Autodesk.Revit.UI;
//using RevitPersonalToolbox.Windows;

//// MVVM pattern:
//// - Model: This represents the data and business logic. => "RevitUtils.cs & Utils.cs"
//// - View: This is the user interface or the presentation layer, it defines how the data is presented to the user. => "SingleSelectionWindow (XAML and code-behind)" 
//// - ViewModel: This acts as the intermediary between the View and the Model. => ViewModel.cs

//namespace RevitPersonalToolbox.TemplateActivityChecker;

//[Transaction(TransactionMode.Manual)]
//[Regeneration(RegenerationOption.Manual)]
//public class ViewTemplateAssignedChecker : IExternalCommand
//{
//    public Stack<Window> NavigationStack { get; } = new Stack<Window>();

//    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
//    {
//        RevitUtils revitUtils = new(commandData);
//        ViewModel viewModel = new(revitUtils, commandData);
//        viewModel.LoadData();

//        const string mainTitle = "Pick a View Template";
//        const string subTitle = "Check to which View(s) this Template has been assigned";
//        foreach (View view in viewModel.RevitViewDataModel.ViewTemplates)
//        {
//            //8 is a placeholder, replace with actual amount of views the template is active on
//            viewModel.RevitViewDataModel.Properties.Add(view.Name, 8);
//        }
//        viewModel.RevitViewDataModel.Properties = Utils.SortDictionary(viewModel.RevitViewDataModel.Properties);
            
//        //SingleSelectionWindow mainWindow = new(mainTitle, subTitle,Utils.RevitWindow(commandData))
//        //{
//        //    DataContext = viewModel
//        //};
//        //mainWindow.ShowDialog();
            

//        //return mainWindow.Cancelled ? Result.Cancelled : Result.Succeeded;
//    }
//}