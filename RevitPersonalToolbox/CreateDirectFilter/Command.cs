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
        Document document = commandData.Application.ActiveUIDocument.Document;
        RevitUtils revitUtils = new(commandData);
        ViewModel viewModel = new(revitUtils);
        viewModel.LoadParameterData();

        Window windowOwner = Utils.GetRevitWindowOwner(commandData);

        if (ShowDialog(new SelectionWindow(windowOwner, viewModel)) &&
            ShowDialog(new EnterFilterName(windowOwner, viewModel)) &&
            ShowDialog(new EnterFilterValues(windowOwner, viewModel)))
        {
            using Transaction t = new(document, "Created Filter based on selection");
            t.Start();

            viewModel.ApplyUserInput();

            t.Commit();
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