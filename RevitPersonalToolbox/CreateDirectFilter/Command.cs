using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.CreateDirectFilter;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class Command : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        Document document = commandData.Application.ActiveUIDocument.Document;
        RevitUtils revitUtils = new RevitUtils(commandData);
        
        BusinessLogic businessLogic = new BusinessLogic(document);
        ViewModel viewModel = new ViewModel(businessLogic, revitUtils);
        ViewWindow viewWindow = new ViewWindow(viewModel, revitUtils);

        const string mainTitle = "Create Filter";
        const string subTitle = "Select parameter to base the filter on";

        viewWindow.ShowWindow(mainTitle, subTitle, Utils.RevitWindow(commandData));
            
        return Result.Succeeded;
    }
}