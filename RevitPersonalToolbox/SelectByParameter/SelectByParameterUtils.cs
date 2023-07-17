using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitPersonalToolbox.SelectByParameter
{
    internal abstract class SelectByParameterUtils
    {
        /// <summary>
        /// Get all distinct Parameters from a selection of Elements.
        /// </summary>
        /// <param name="selectedElements"></param>
        /// <returns></returns>
        internal static IEnumerable<Parameter> GetDistinctParameters(IEnumerable<Element> selectedElements)
        {
            Dictionary<Parameter, string> dictionary = new Dictionary<Parameter, string>();
            foreach (Element element in selectedElements)
            {
                IList<Parameter> paramsPerElement = element.GetOrderedParameters();
                foreach (Parameter parameter in paramsPerElement)
                {
                    if (dictionary.ContainsValue(parameter.Definition.Name)) continue;
                    dictionary.Add(parameter, parameter.Definition.Name);
                }
            }
            IEnumerable<Parameter> allDistinctParams = dictionary.Keys.OrderBy(x => x.Definition.Name);

            return allDistinctParams;
        }
    }
}
