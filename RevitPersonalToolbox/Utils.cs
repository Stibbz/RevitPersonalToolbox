using System.Windows;
using System.Windows.Interop;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox
{
    public class Utils(ExternalCommandData commandData)
    {
        private ExternalCommandData CommandData { get; set; } = commandData;
        private UIDocument UiDocument { get; set; } = commandData.Application.ActiveUIDocument;
        private Document Document { get; set; } = commandData.Application.ActiveUIDocument.Document;

        public static Dictionary<string, dynamic> SortDictionary(Dictionary<string, dynamic> dictionary)
        {
            Dictionary<string, dynamic> result = new();
            IList<string> sortedKeys = dictionary.Keys.ToList().OrderBy(x => x).ToList();

            foreach (string key in sortedKeys)
            {
                result.Add(key, dictionary[key]);

            }

            return result;
        }

        public static Window RevitWindow(ExternalCommandData commandData)
        {
            IntPtr RevitWindowHandle = commandData.Application.MainWindowHandle;
            HwndSource hwndSource = HwndSource.FromHwnd(RevitWindowHandle);
            Window RevitWindow = hwndSource.RootVisual as Window;
            return RevitWindow;
        }

        public Parameter PickParameter(ICollection<Element> selectedElements)
        {
            if (selectedElements == null) return null;
            
            IEnumerable<Parameter> parameters = null;
            Parameter parameter = null;

            foreach (Element element in selectedElements)
            {
                // Get Parameters from SourceObject
                parameters = element.GetOrderedParameters();

                // Pick specific parameter for testing
                parameter = element.LookupParameter("Length");
            }

            return parameter;
        }

        private string GetParameterValue(Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    return parameter.AsValueString();
                case StorageType.ElementId:
                    return parameter.AsValueString();
                case StorageType.Integer:
                    return parameter.AsValueString();
                case StorageType.None:
                    return parameter.AsValueString();
                case StorageType.String:
                    return parameter.AsString();
                default:
                    return null;
            }
        }
        }
}