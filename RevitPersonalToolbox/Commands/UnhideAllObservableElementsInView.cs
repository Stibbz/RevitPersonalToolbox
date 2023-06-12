using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;

namespace RevitPersonalToolbox.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class UnhideAllElementsInView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            View active = doc.ActiveView;
            List<ElementId> hiddenPhysicalElements = Utils.SelectAllObservableElements(doc)
                .Where(x => x.IsHidden(doc.ActiveView))
                .Select(x => x.Id)
                .ToList();

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Un-hide All Observable Elements in current View");
                if (hiddenPhysicalElements.Count == 0)
                {
                    TaskDialog.Show("info", "0 hidden elements found.");
                    return Result.Failed;
                }

                doc.ActiveView.UnhideElements(hiddenPhysicalElements);
                tx.Commit();
                TaskDialog.Show("Success", $"{hiddenPhysicalElements.Count} hidden elements have been unhidden.");
            }

            return Result.Succeeded;
        }
    }
}