using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public class RevitPersonalToolbox : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        UIDocument uiDoc = commandData.Application.ActiveUIDocument;
        Document doc = commandData.Application.ActiveUIDocument.Document;

        // Collect all lines in current view
        var currentView = doc.ActiveView;
        FilteredElementCollector allDetailLines =
            new FilteredElementCollector(doc, currentView.Id).OfCategory(BuiltInCategory.OST_Lines);

        double totalLength = 0;
        foreach (var line in allDetailLines)
        {
            // Check if lines are red
            var lineStyleParam = line.LookupParameter("Line Style");
            if (lineStyleParam == null) continue;
            string lineStyle = lineStyleParam.AsValueString();
            if (!lineStyle.Contains("Red")) continue;
            
            // Add lenght of each red line
            var lengthParam = line.LookupParameter("Length");
            if (lengthParam == null) continue;
            double length = double.Parse(lengthParam.AsValueString()) / 1000;
            
            totalLength += length;
        }

        TaskDialog.Show("Length", $@"Total length of all red lines in this view is: {totalLength}m");
        TaskDialog.Show("Area", $@"Total area (at 2.4m height) is: {totalLength * 2.4}m");
        
        return Result.Succeeded;
    }
}