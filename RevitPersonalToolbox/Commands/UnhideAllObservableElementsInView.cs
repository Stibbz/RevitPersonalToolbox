using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Selection;

namespace RevitPersonalToolbox.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class UnhideAllObservableElementsInView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            View active = doc.ActiveView;
            List<ElementId> hiddenPhysicalElements = Utils.SelectAllObservableElements(doc)
                .Where(x => x.IsHidden(doc.ActiveView))
                .Select(x => x.Id)
                .ToList();

            //Application app = commandData.Application.Application;
            //List<Document> linkedDocs = new List<Document>();
            //foreach (Document d in app.Documents)
            //{
            //    if (d.IsLinked)
            //    {
            //        linkedDocs.Add(d);
            //    }
            //}

            //List<ElementId> allLinkedElements = new List<ElementId>();
            //List<ElementId> allLinkedElementsUnhide = new List<ElementId>();
            //foreach (Document d in linkedDocs)
            //{
            //    allLinkedElements.AddRange(new FilteredElementCollector(d).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElementIds());
            //}

            //IList<Element> instances =
            //    new FilteredElementCollector(doc).OfClass((typeof(RevitLinkInstance))).ToElements();

            //Element linkInstance = new FilteredElementCollector(doc).OfClass((typeof(RevitLinkInstance))).ToElements()
            //    .FirstOrDefault();
            //foreach (ElementId element in allLinkedElements)
            //{
            //    foreach (Element lInstance in instances)
            //    {
            //        LinkElementId linkId = new LinkElementId(lInstance.Id, element);
            //        if (linkId.LinkedElementId != ElementId.InvalidElementId && linkId.HostElementId != ElementId.InvalidElementId)
            //        {
            //            allLinkedElementsUnhide.Add(linkId.HostElementId);
            //            allLinkedElementsUnhide.Add(linkId.LinkedElementId);
            //            allLinkedElementsUnhide.Add(linkId.LinkInstanceId);
            //        }
            //    }
            //}

            //doc.ActiveView.UnhideElements(allLinkedElementsUnhide);


            //// Collect all elements belonging to (visible) Linked Models for un-hiding
            //IList<ElementId> elementsFromLinkedModels = new List<ElementId>();
            //IEnumerable<Element> revitLinks = Utils.SelectRevitLinks(doc);
            //foreach (Element revitLink in revitLinks)
            //{
            //    RevitLinkInstance docLinkedAsElement = (RevitLinkInstance)doc.GetElement(revitLink.Id);

            //    Document docLinked = docLinkedAsElement.Document;
            //    string title = docLinked.Title;
            //    var collected = new FilteredElementCollector(docLinked, doc.ActiveView.Id)
            //        .WhereElementIsNotElementType().ToElements();

            //    foreach (var c in collected)
            //    {
            //        elementsFromLinkedModels.Add(c.Id);
            //    }
            //}


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

    //[Transaction(TransactionMode.Manual)]
    //[Regeneration(RegenerationOption.Manual)]
    //public class UnhideAllObservableElementsInViewv2 : IExternalCommand
    //{
    //    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    //    {
    //        UIDocument uidoc = commandData.Application.ActiveUIDocument;
    //        Document doc = commandData.Application.ActiveUIDocument.Document;
    //        View active = doc.ActiveView;
    //        List<ElementId> hiddenPhysicalElements = Utils.SelectAllObservableElements(doc)
    //            .Where(x => x.IsHidden(doc.ActiveView))
    //            .Select(x => x.Id)
    //            .ToList();
    //        using (Transaction t = new Transaction(doc, "Set View"))
    //        {
    //            t.Start();

    //            doc.ActiveView.EnableRevealHiddenMode();
    //            t.Commit();

    //        }
    //        Reference r = uidoc.Selection.PickObject(ObjectType.LinkedElement);
    //        using (Transaction t = new Transaction(doc, "Set View"))
    //        {
    //            t.Start();
    //            doc.ActiveView.DisableTemporaryViewMode(TemporaryViewMode.RevealHiddenElements);
                
    //            doc.ActiveView.UnhideElements(new List<ElementId>() { r.ElementId });
    //            t.Commit();

    //        }
    //        //Application app = commandData.Application.Application;
    //        //List<Document> linkedDocs = new List<Document>();
    //        //foreach (Document d in app.Documents)
    //        //{
    //        //    if (d.IsLinked)
    //        //    {
    //        //        linkedDocs.Add(d);
    //        //    }
    //        //}

    //        //List<ElementId> allLinkedElements = new List<ElementId>();
    //        //List<ElementId> allLinkedElementsUnhide = new List<ElementId>();
    //        //foreach (Document d in linkedDocs)
    //        //{
    //        //    allLinkedElements.AddRange(new FilteredElementCollector(d).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElementIds());
    //        //}

    //        //IList<Element> instances =
    //        //    new FilteredElementCollector(doc).OfClass((typeof(RevitLinkInstance))).ToElements();

    //        //Element linkInstance = new FilteredElementCollector(doc).OfClass((typeof(RevitLinkInstance))).ToElements()
    //        //    .FirstOrDefault();
    //        //foreach (ElementId element in allLinkedElements)
    //        //{
    //        //    foreach (Element lInstance in instances)
    //        //    {
    //        //        LinkElementId linkId = new LinkElementId(lInstance.Id, element);
    //        //        if (linkId.LinkedElementId != ElementId.InvalidElementId && linkId.HostElementId != ElementId.InvalidElementId)
    //        //        {
    //        //            allLinkedElementsUnhide.Add(linkId.HostElementId);
    //        //            allLinkedElementsUnhide.Add(linkId.LinkedElementId);
    //        //            allLinkedElementsUnhide.Add(linkId.LinkInstanceId);
    //        //        }
    //        //    }
    //        //}

    //        //doc.ActiveView.UnhideElements(allLinkedElementsUnhide);
            
    //        return Result.Succeeded;
    //    }
    //}
}