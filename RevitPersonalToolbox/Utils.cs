using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitPersonalToolbox
{
    public static class Utils
    {
        internal static IEnumerable<Element> SelectAllObservableElements(Document doc)
        {
            // Select anything that is observable in Revit
            return new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .Where(IsObservable);
        }

        public static bool IsObservable(Element e)
        {
            // Filter to exclude anything that is not observable in Revit
            if (e.Category == null) return false;
            if (e.ViewSpecific) return false;

            // exclude specific unwanted categories (i.e. Voids)
            if ((BuiltInCategory)e.Category.Id.IntegerValue == BuiltInCategory.OST_HVAC_Zones) return false;

            return e.Category.CategoryType == CategoryType.Model && e.Category.CanAddSubcategory;
        }

        public static IEnumerable<Element> SelectRevitLinks (Document doc)
        {
            return new FilteredElementCollector(doc, doc.ActiveView.Id)
                .OfCategory(BuiltInCategory.OST_RvtLinks)
                .OfClass(typeof(RevitLinkInstance))
                .ToElements();
        }


        public static IEnumerable<View> GetViewTemplates(Document doc)
        {
            IEnumerable<View> colViewTemplates = new List<View>(new FilteredElementCollector(doc)
                    .OfClass(typeof(View))
                    .Cast<View>()
                    .Where(v => v.IsTemplate))
                .Where(v => v.ViewType != ViewType.Schedule);

            // // Sort colViewTemplates alphabetically by using their Name (Lambda).
            // // Making use of StringComparison.Ordinal to avoid problems when the code runs on computers with different culture settings.
            // // https://www.jetbrains.com/help/rider/StringCompareToIsCultureSpecific.html
            // colViewTemplates.Sort((v1, v2) => string.Compare(v1.Name, v2.Name, StringComparison.Ordinal));

            return colViewTemplates;
        }

        public static FilteredElementCollector GetFilterElements(Document doc)
        {
            FilteredElementCollector colFilters = new FilteredElementCollector(doc)
                .OfClass(typeof(FilterElement));

            return colFilters;
        }

        public static FilteredElementCollector GetRevitLinks(Document doc)
        {
            FilteredElementCollector colRevitLinks = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_RvtLinks)
                .OfClass(typeof(RevitLinkType));

            return colRevitLinks;
        }
    }
}