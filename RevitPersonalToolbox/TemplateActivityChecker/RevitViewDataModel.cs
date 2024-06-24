using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitPersonalToolbox.TemplateActivityChecker
{
    public class RevitViewDataModel
    {
        public IEnumerable<View> ViewTemplates { get; set; }
        public IEnumerable<View> Views { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new();

    }
}
