using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.SelectByParameter;

namespace RevitPersonalToolbox.CheckViewTemplateAssigned
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CheckViewTemplateAssigned : IExternalCommand
    {
        /* Check for a specific View Template to which Views it has been assigned.
           Additionally, a way to show affected sheets + a button to filter which of these views have been placed on a sheet.
        */

        /*
          1. Click tool button
          2. Show form displaying all view templates, user selects a view template
          3. Show new form displaying all views (if any) that have the selected view template applied to them
             - Checkbox to filter Views that are placed on a sheet
             - Button to show additional form with all affected sheets
        */ 

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Where code comes in from Revit

            Document document = commandData.Application.ActiveUIDocument.Document;
            RevitUtils revitUtils = new RevitUtils(commandData);
            RevitExecutor revitExecutor = new RevitExecutor(document, revitUtils);
            
            BusinessLogic businessLogic = new BusinessLogic(document);
            ViewModel viewModel = new ViewModel(businessLogic, revitUtils);
            ViewWindow viewWindow = new ViewWindow(viewModel, revitUtils);
            
            viewWindow.ShowWindow();
            
            return Result.Succeeded;
        }
    }
}