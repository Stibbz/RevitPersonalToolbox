using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Autodesk.Revit.DB;

namespace RevitPersonalToolbox.SelectByParameter
{
    internal class RevitElement
    {
        public Element Element { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }

        public RevitElement(Element element)
        {
            Element = element;
            GetParameters(element);

            var orderedParams = element.GetOrderedParameters();
            foreach (Parameter parameter in orderedParams)
            {
                
            }


        }

        private void GetParameters(Element element)
        {
            IList<Parameter> paraList = element.GetOrderedParameters();
            foreach (Parameter parameter in paraList)
            {
                ParamName = parameter.Definition.Name;

                switch (parameter.StorageType)
                {
                    case StorageType.Double:
                        ParamValue = parameter.AsValueString();
                        break;
                    case StorageType.ElementId:
                        ParamValue = parameter.AsValueString();
                        break;
                    case StorageType.Integer:
                        ParamValue = parameter.AsValueString();
                        break;
                    case StorageType.None:
                        ParamValue = parameter.AsValueString();
                        break;
                    case StorageType.String:
                        ParamValue = parameter.AsString();
                        break;
                }

            }
        }

        // Get all values for each parameter (if different values display "<varies>")
    }
}
