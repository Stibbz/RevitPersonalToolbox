using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class Command : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        Document document = commandData.Application.ActiveUIDocument.Document;

        RevitUtils revitUtils = new(commandData);
        BusinessLogic businessLogic = new(document);

        ViewModel viewModel = new(commandData, businessLogic, revitUtils);
        viewModel.LoadData();

        SingleSelectionWindow mainWindow = new(Utils.GetRevitWindowOwner(commandData))
        {
            MainTitle = { Content = "Parameters" },
            SubTitle = { Content = "Determine which parameter to base the filter on" },
            DataContext = viewModel
        };
        mainWindow.ShowDialog();

        return Result.Succeeded;
    }
}
