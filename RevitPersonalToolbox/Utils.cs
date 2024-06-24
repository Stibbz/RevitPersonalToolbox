using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows;
using System.Windows.Interop;


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
    }
}