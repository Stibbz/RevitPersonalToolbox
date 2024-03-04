using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;


namespace RevitPersonalToolbox.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class FlipEachElementMidpoint
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = commandData.Application.ActiveUIDocument.Document;

            IList<Reference> references = uiDocument.Selection.PickObjects(ObjectType.Element);
            List<Element> selectedElements = references.Select(reference => document.GetElement(reference)).ToList();

            
            using (Transaction t = new Transaction(document))
            {
                t.Start("Flip Selected Elements");




                t.Commit();
            }
            return Result.Succeeded;
        }
    }
}

