using System.Data;
using System.Reflection;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox
{
    public class RevitUtils
    {
        private ExternalCommandData CommandData { get; set; }
        private UIDocument UiDocument { get; set; }
        private Document Document { get; set; }


        public RevitUtils(ExternalCommandData commandData)
        {
            CommandData = commandData;
            UiDocument = commandData.Application.ActiveUIDocument;
            Document = commandData.Application.ActiveUIDocument.Document;
        }


        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();      
    
            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }
    
            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }
        
                dataTable.Rows.Add(values);
            }
    
            return dataTable;
        }

        public IEnumerable<Element> SelectAllObservableElements()
        {
            // Select anything that is observable in Revit
            return new FilteredElementCollector(Document)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .Where(IsObservable);
        }

        private static bool IsObservable(Element e)
        {
            // Filter to exclude anything that is not observable in Revit
            if (e.Category == null) return false;
            if (e.ViewSpecific) return false;

            // exclude specific unwanted categories (i.e. Voids)
            if ((BuiltInCategory)e.Category.Id.IntegerValue == BuiltInCategory.OST_HVAC_Zones) return false;

            return e.Category.CategoryType == CategoryType.Model && e.Category.CanAddSubcategory;
        }

        public IEnumerable<Element> SelectRevitLinks()
        {
            return new FilteredElementCollector(Document, Document.ActiveView.Id)
                .OfCategory(BuiltInCategory.OST_RvtLinks)
                .OfClass(typeof(RevitLinkInstance))
                .ToElements();
        }

        public IEnumerable<Element> GetSelectedElements()
        {
            IEnumerable<ElementId> selection = UiDocument.Selection.GetElementIds();
            List<Element> selectedElements = selection.Select(id => Document.GetElement(id)).ToList();

            if (selectedElements.Any()) return selectedElements;

            TaskDialog.Show("Error", "Select items first.");
            return null;
        }

        public IEnumerable<View> GetViewTemplates()
        {
            return new FilteredElementCollector(Document)
                    .OfClass(typeof(View))
                    .Cast<View>()
                    .Where(v => v.IsTemplate)
                    .Where(v => v.ViewType != ViewType.Schedule);
        }

        public FilteredElementCollector GetFilterElements()
        {
            return new FilteredElementCollector(Document)
                .OfClass(typeof(FilterElement));
        }

        public FilteredElementCollector GetRevitLinks()
        {
            return new FilteredElementCollector(Document)
                .OfCategory(BuiltInCategory.OST_RvtLinks)
                .OfClass(typeof(RevitLinkType));
        }

        public IEnumerable<View> GetViews()
        {
            return new FilteredElementCollector(Document)
                .WhereElementIsNotElementType()
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v=> !v.IsTemplate)
                .Where(v=> v.CanUseTemporaryVisibilityModes());
        }

        public List<View> CheckViewTemplateAssignment(View selectedViewTemplate)
        {
            //Dictionary<string, dynamic> resultDictionary = new Dictionary<string, dynamic>();
            List<View> assignedViews = new List<View>();
            IEnumerable<View> views = GetViews();
            foreach (View view in views)
            {
                if (view.ViewTemplateId != selectedViewTemplate.Id) continue;
                assignedViews.Add(view);
            }

            return assignedViews;
        }
        
    }
}