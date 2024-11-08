using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.Playground.Windows;

namespace RevitPersonalToolbox.Playground;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class Command : IExternalCommand
{
    public static bool Cancelled { get; set; } = true;

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        Document document = commandData.Application.ActiveUIDocument.Document;
        UIDocument uiDocument = commandData.Application.ActiveUIDocument;

        RevitUtils revitUtils = new(document, uiDocument);
        ViewModel viewModel = new ViewModel(revitUtils);

        Window windowOwner = Utils.GetRevitWindowOwner(commandData);
        

        IEnumerable<Element> items = new FilteredElementCollector(document)
            .OfCategory(BuiltInCategory.OST_DetailComponents)
            .OfClass(typeof(FamilyInstance))
            .WhereElementIsNotElementType()
            .Cast<FamilyInstance>()
            .Where(fi => fi.Symbol.Family.Name == "00.00_DI_Afbreekmask" && fi.Symbol.Name == "Afbreek_Mask")
            .ToList();

        int counter = 0;
        List<Element> selected = [];

        using Transaction t = new Transaction(document, "Set Instance Parameters to False");
        t.Start();

        foreach (Element element in items)
        {
            if (element.GroupId != ElementId.InvalidElementId) continue;

            Parameter afbreektekst = element.LookupParameter("Afbreektekst");
            if (!afbreektekst.AsString().IsNullOrEmpty()) continue;

            Parameter tekstZichtbaar = element.LookupParameter("Tekst_Zichtbaar");
            tekstZichtbaar.Set(0);

            selected.Add(element);
            counter++;
        }

        uiDocument.Selection.SetElementIds(selected.Select(x => x.Id).ToList());

        t.Commit();
        TaskDialog.Show("info", $"{counter} Afbreeklijnen have been set to false).");


        #region Select all elements with parameter value

        //View currentView = uiDocument.ActiveView;
        //IEnumerable<Element> elementsWithParameterName = new FilteredElementCollector(document, currentView.Id)
        //    .WhereElementIsNotElementType();

        //string searchParameter = "Mark";
        //string parameterValue = "GX200/30";
        //List<Element> matchingElements = [];

        //foreach(Element element in elementsWithParameterName)
        //{
        //    Parameter parameter = element.LookupParameter(searchParameter);
        //    if (parameter == null) continue;
        //    if(parameter.AsString() == parameterValue)
        //    {
        //        matchingElements.Add(element);
        //    }
        //}

        //if (matchingElements.Count == 0)
        //{
        //    TaskDialog.Show("Nothing", "No matching elements found.");
        //}

        //uiDocument.Selection.SetElementIds(matchingElements.Select(x => x.Id).ToList());

        //EnterParameter window = new EnterParameter(windowOwner, viewModel);
        //window.Show();

        #endregion

        return Cancelled ? Result.Cancelled : Result.Succeeded;
    }
}