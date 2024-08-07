using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.CreateDirectFilter.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class Command : IExternalCommand
{
    public static bool Cancelled { get; set; }

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        RevitUtils revitUtils = new(commandData);
        ViewModel viewModel = new(revitUtils);
        viewModel.LoadParameterData();

        Window windowOwner = Utils.GetRevitWindowOwner(commandData);

        if (ShowDialog(new SingleSelectionWindow(windowOwner, viewModel)) &&
            ShowDialog(new PickFilterName(windowOwner, viewModel)) &&
            ShowDialog(new FilterInputWindow(windowOwner, viewModel)))
        {
            viewModel.ApplyInput();
        }

        return Cancelled ? Result.Cancelled : Result.Succeeded;
    }

    private bool ShowDialog(Window window)
    {
        if (!Cancelled)
        {
            window.ShowDialog();
        }

        return !Cancelled;
    }
}