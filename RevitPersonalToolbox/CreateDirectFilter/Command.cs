using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.CreateDirectFilter;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class Command : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        Document document = commandData.Application.ActiveUIDocument.Document;
        RevitUtils revitUtils = new(commandData);

        // List<FilterRule> filterRules = Utils.CreateFilterRules(document);
        Utils.CreateViewFilter(document, document.ActiveView);

        BusinessLogic businessLogic = new(document);
        ViewModel viewModel = new(businessLogic, revitUtils);
        ViewWindow viewWindow = new(viewModel, revitUtils);

        const string mainTitle = "Create Filter";
        const string subTitle = "Select parameter to base the filter on";

        viewWindow.ShowWindow(mainTitle, subTitle, Utils.RevitWindow(commandData));

        return Result.Succeeded;
    }
}
