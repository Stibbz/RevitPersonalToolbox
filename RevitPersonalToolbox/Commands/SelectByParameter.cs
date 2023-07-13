using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class SelectByParameter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = commandData.Application.ActiveUIDocument.Document;

            // Get current User Selection
            IEnumerable<ElementId> selection = uiDocument.Selection.GetElementIds();
            List<Element> selectedElements = selection.Select(elementId => document.GetElement(elementId)).ToList();
            
            // Get all distinct parameters in sorted (by name) order
            Dictionary<Parameter, string> dictionary = new Dictionary<Parameter, string>();
            foreach (Element element in selectedElements)
            {
                IList<Parameter> paramsPerElement = element.GetOrderedParameters();
                foreach (Parameter parameter in paramsPerElement)
                {
                    if (dictionary.ContainsValue(parameter.Definition.Name)) continue;
                    dictionary.Add(parameter, parameter.Definition.Name);
                }
            }
            IEnumerable<Parameter> allDistinctParams = dictionary.Keys.OrderBy(x => x.Definition.Name);

            // TODO: Remove (debugging only)
            int i = 1;
            foreach (Parameter para in allDistinctParams)
            {
                Debug.WriteLine($"Found Parameter {i}: {para.Definition.Name}");
                i++;
            }

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
