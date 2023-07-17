using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.SelectByParameter
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class SelectByParameter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = commandData.Application.ActiveUIDocument.Document;
            Utils utils = new Utils(commandData);

            // Get current User Selection
            IEnumerable<Element> selectedElements = utils.GetSelectedElements();
            
            // Get all distinct parameters in sorted (by name) order
            IEnumerable<Parameter> distinctParameters = SelectByParameterUtils.GetDistinctParameters(selectedElements);


            /* TODO: Future implementations
             1. Create WPF form to display:
                - All parameters that have been found
                - All values for each parameter (if different values display "<varies>")
             2. Have user select the desired parameter and value to search by
             3. Select every element that has the desired parameter and value
            */


            

            List<Element> filteredByParameter = new List<Element>();
            ICollection<ElementId> filteredElementIds = filteredByParameter.Select(element => element.Id).ToList();
            uiDocument.Selection.SetElementIds(filteredElementIds);

            return Result.Succeeded;
        }
    }
}
