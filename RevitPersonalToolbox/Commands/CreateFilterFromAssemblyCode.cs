using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CreateFilterFromAssemblyCode : IExternalCommand
    {
        // Read Assembly code from selected element and create a filter to turn every matching element off
        // Note: Find a way to make this work on elements from Linked Models as well
        // Note: How to handle which categories the filters are to be set up with by default (probably simply using all categories, otherwise matching the current selection?)

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            
            
            return Result.Succeeded;
        }
    }
}