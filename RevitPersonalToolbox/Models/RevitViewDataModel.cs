using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace RevitPersonalToolbox.Models
{
    public class RevitViewDataModel
    {
        public IEnumerable<View> ViewTemplates { get; set; }
        public IEnumerable<View> Views { get; set; }
        public View SelectedViewTemplate { get; set; }
        public Dictionary<string, dynamic> ResultDictionary { get; set; }
        public List<View> ResultingViews { get; set; }
    }
}