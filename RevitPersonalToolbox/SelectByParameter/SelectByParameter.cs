using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.SelectByParameter
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class SelectByParameter : IExternalCommand
    {
        /* Summary
            1. Get selected elements and their parameters + Values
            2. Show to user (in WPF form):
                - All distinct parameters that have been found in the Selected Elements
                - All values for each parameter (if different values display "<varies>")
            3. Have user select the desired parameter + value to search for
            4. Select every element that has the same parameter + value
        */

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Utils utils = new Utils(commandData);

            // Get all selected elements
            IEnumerable<Element> selectedElements = utils.GetSelectedElements();

            // TODO: Idea
            /* Create methods that gets all Parameters out of each Element,
                then call DataModelParameters so the DataModel results in a list of all Parameters. 
                -> Duplicate Parameters is fine here, since each Parameter has to have it's own value. NO FILTERING NEEDED YET */
            


            //// Get all distinct parameters that have been found in the selected elements
            //IEnumerable<Parameter> distinctParameters = SelectByParameterUtils.GetDistinctParameters(selectedElements);

            //// Get all values for each parameter (if different values display "<varies>")



            // 2. Show to user (in WPF form):
            SelectionWindow selectionWindow = new SelectionWindow();
            selectionWindow.TitleLabel.Name = "Select By Parameter";

            //List<string> paramNames = revitElements.Select(revitElement => revitElement.Name).ToList();
            selectionWindow.ListBoxParameterNames.ItemsSource = paramNames;

            //List<string> paramValues = revitElements.Select(revitElement => revitElement.Value).ToList();
            selectionWindow.ListBoxParameterValues.ItemsSource = paramValues;

            selectionWindow.ShowDialog();






            List<Element> filteredByParameter = new List<Element>();
            ICollection<ElementId> filteredElementIds = filteredByParameter.Select(element => element.Id).ToList();
            uiDocument.Selection.SetElementIds(filteredElementIds);

            return Result.Succeeded;
        }
    }

}
