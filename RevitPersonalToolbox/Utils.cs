using System.Windows;
using System.Windows.Interop;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.CreateDirectFilter;

namespace RevitPersonalToolbox
{
    public class Utils(ExternalCommandData commandData)
    {
        private ExternalCommandData CommandData { get; set; } = commandData;
        private UIDocument UiDocument { get; set; } = commandData.Application.ActiveUIDocument;
        private Document Document { get; set; } = commandData.Application.ActiveUIDocument.Document;

        public static Dictionary<string, dynamic> SortDictionary(Dictionary<string, dynamic> dictionary)
        {
            Dictionary<string, dynamic> result = [];
            IList<string> sortedKeys = dictionary.Keys.ToList().OrderBy(x => x).ToList();

            foreach (string key in sortedKeys)
            {
                result.Add(key, dictionary[key]);
            }

            return result;
        }

        public static Window GetRevitWindowOwner(ExternalCommandData commandData)
        {
            IntPtr RevitWindowHandle = commandData.Application.MainWindowHandle;
            HwndSource hwndSource = HwndSource.FromHwnd(RevitWindowHandle);
            Window RevitWindow = hwndSource.RootVisual as Window;
            return RevitWindow;
        }

        //public static void CallInputWindow(ExternalCommandData commandData, string mainTitle, string subTitle, string selectedParameter)
        //{
        //    FilterInputWindow newWindow = new(GetRevitWindowOwner(commandData))
        //    {
        //        MainTitle = { Text = mainTitle },
        //        SubTitle = { Text = subTitle },
        //        ParameterName = { Text = selectedParameter },
        //    };
        //    newWindow.ShowDialog();
        //}

        public Parameter PickParameter(ICollection<Element> selectedElements)
        {
            if (selectedElements == null) return null;

            Parameter parameter = null;

            foreach (Element element in selectedElements)
            {
                // Get Parameters from SourceObject
                element.GetOrderedParameters();

                // Pick specific parameter for testing
                parameter = element.LookupParameter("Length");
            }

            return parameter;
        }

        private string GetParameterValue(Parameter parameter)
        {
            return parameter.StorageType switch
            {
                StorageType.Double => parameter.AsValueString(),
                StorageType.ElementId => parameter.AsValueString(),
                StorageType.Integer => parameter.AsValueString(),
                StorageType.None => parameter.AsValueString(),
                StorageType.String => parameter.AsString(),
                _ => null
            };
        }
    }
}