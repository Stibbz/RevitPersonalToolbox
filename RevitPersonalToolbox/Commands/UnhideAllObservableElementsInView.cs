using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class UnhideAllObservableElementsInView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            var allObservableElementIds = Utils.SelectAllObservableElements(doc)
                .Select(x => x.Id)
                .ToList();

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Unhide All Observable Elements in current View");
                doc.ActiveView.UnhideElements(allObservableElementIds);
                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}