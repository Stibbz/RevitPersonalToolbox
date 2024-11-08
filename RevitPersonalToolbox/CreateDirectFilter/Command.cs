using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.CreateDirectFilter.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class Command : IExternalCommand
{
    public static bool Cancelled { get; set; } = true;

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        Document document = commandData.Application.ActiveUIDocument.Document;
        UIDocument uiDocument = commandData.Application.ActiveUIDocument;

        RevitUtils revitUtils = new(document, uiDocument);
        ViewModel viewModel = new(revitUtils);
        viewModel.LoadData();

        // Keep Window on top of Revit
        Window windowOwner = Utils.GetRevitWindowOwner(commandData);

        // Create a new window that shows existing filters
        if (ShowDialog(new SelectionWindow(windowOwner, viewModel)) &&
            ShowDialog(new EnterFilterName(windowOwner, viewModel)) &&
            ShowDialog(new EnterFilterValues(windowOwner, viewModel)))
        {
            using Transaction t = new(document, "Created View Filter using Addin");
            t.Start();

            viewModel.ApplyUserInput();

            t.Commit();
        }

        return Cancelled ? Result.Cancelled : Result.Succeeded;
    }
    private static bool ShowDialog(Window window)
    {
        if (Cancelled) return !Cancelled;

        Cancelled = true; // reset cancelled status until code completes
        window.ShowDialog();

        return !Cancelled;
    }
}