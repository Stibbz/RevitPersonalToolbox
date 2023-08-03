using Autodesk.Revit.DB;

namespace RevitPersonalToolbox.SelectByParameter
{
    public class RevitUtils
    {
        private readonly Document _document;

        public RevitUtils(Document document)
        {
            _document = document;
        }
    }
}