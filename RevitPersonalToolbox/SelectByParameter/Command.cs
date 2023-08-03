using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.SelectByParameter
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {
        /* Summary
        1. Get selected elements and parameter data 
        2. Display form showing distinct Parameters and their values (<varies> if varying values).
        3. User selects the desired parameter value to search for.
        4. Get every element in the project and match with user input
        5. Select every corresponding element
        6. (optional) Pop-up suggesting to isolate selection OR temporarily colorize for visibility
        */
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Where code comes in from Revit
            Document document = commandData.Application.ActiveUIDocument.Document;
            RevitUtils revitUtils = new RevitUtils(document);
            RevitExecutor revitExecutor = new RevitExecutor(document, revitUtils);

            Utils utils = new Utils(commandData);
            
            BusinessLogic businessLogic = new BusinessLogic(document);
            ViewModel viewModel = new ViewModel(businessLogic, utils);
            ViewWindow viewWindow = new ViewWindow(utils, viewModel);
            
            viewWindow.ShowWindow();
            
            return Result.Succeeded;
        }
    }
}