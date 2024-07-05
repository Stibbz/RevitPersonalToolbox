using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace RevitPersonalToolbox.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class UnjoinFromEverything : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = commandData.Application.ActiveUIDocument.Document;

            Reference reference = uiDocument.Selection.PickObject(ObjectType.Element);
            Element selected = document.GetElement(reference);

            //FilteredElementCollector beam = new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_StructuralFraming);
            ICollection<ElementId> allJoined = JoinGeometryUtils.GetJoinedElements(document, selected);
            using (Transaction t = new Transaction(document))
            {
                t.Start("Unjoin From Everything");

                foreach (ElementId joined in allJoined)
                {
                    Element e = document.GetElement(joined);
                    JoinGeometryUtils.UnjoinGeometry(document, selected, e);
                }


                t.Commit();
            }
            return Result.Succeeded;
        }
    }
}
