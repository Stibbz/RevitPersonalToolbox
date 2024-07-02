using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitPersonalToolbox.CreateDirectFilter
{
    internal class RevitFilterModel
    {
        public IEnumerable<View> ViewTemplates { get; set; }
        public IEnumerable<View> Views { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new();
    }
}
