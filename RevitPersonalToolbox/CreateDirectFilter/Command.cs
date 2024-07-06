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
        // Get category from selected element
        ICollection<ElementId> selectedItems = commandData.Application.ActiveUIDocument.Selection.GetElementIds();
        IEnumerable<Element> selectedElements = selectedItems.Select(eId => document.GetElement(eId));
        ICollection<ElementId> categoryIds = selectedElements.Select(selectedElement => selectedElement.Category.Id).ToList();
        string parameterValue = "not implemented";

        Utils.CreateViewFilter(document, document.ActiveView, categoryIds, parameterValue);

        BusinessLogic businessLogic = new(document);
        ViewModel viewModel = new(businessLogic, revitUtils);
        ViewWindow viewWindow = new(viewModel, revitUtils);

        const string mainTitle = "Create Filter";
        const string subTitle = "Select parameter to base the filter on";

        viewWindow.ShowWindow(mainTitle, subTitle, Utils.RevitWindow(commandData));

        return Result.Succeeded;
    }
}
