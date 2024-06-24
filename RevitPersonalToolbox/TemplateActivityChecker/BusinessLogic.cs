using Autodesk.Revit.DB;

namespace RevitPersonalToolbox.CheckViewTemplateAssigned
{
    public class BusinessLogic
    {
        private readonly Document _document;
        public BusinessLogic(Document document)
        {
            _document = document;
        }

    }
}