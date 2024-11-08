using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class TotalDetailLinesLength : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // Collect all lines in current view
            View currentView = doc.ActiveView;
            FilteredElementCollector allDetailLines =
                new FilteredElementCollector(doc, currentView.Id).OfCategory(BuiltInCategory.OST_Lines);

            double totalLength = 0;
            foreach (Element line in allDetailLines)
            {
                // TODO: Dynamically ask user what LineType(s) should be measured.
                // Check if lines are red
                Parameter lineStyleParam = line.LookupParameter("Line Style");
                if (lineStyleParam == null) continue;
                string lineStyle = lineStyleParam.AsValueString();
                if (!lineStyle.Contains("Rood")) continue;

                // Add length of each red line
                Parameter lengthParam = line.LookupParameter("Length");
                if (lengthParam == null) continue;
                double length = double.Parse(lengthParam.AsValueString()) / 1000;

                totalLength += length;
            }

            TaskDialog.Show("Length", $@"Total length of all red lines in this view is: {totalLength}m");
            TaskDialog.Show("Area", $@"Total area (at 2.6m height) is: {totalLength * 2.6}m");

            return Result.Succeeded;
        }
    }
}