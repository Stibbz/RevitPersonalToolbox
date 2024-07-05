namespace RevitPersonalToolbox.SelectByParameter
{
    internal class BusinessLogic(Document document)
    {
        // Fields
        private readonly Document _document = document;
        
        
        // Constructors


        // Methods
        /// <summary>
        /// Get ParameterModel data from a collection of elements
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        internal IOrderedEnumerable<ParameterModel> GetParameterData(IEnumerable<Element> elements)
        {
            List<ParameterModel> dataModelParameters = new List<ParameterModel>();
            foreach (Element element in elements)
            {
                // Get Parameters from Element
                IEnumerable<Parameter> parameters = element.GetOrderedParameters();
                
                // Populate a RevitViewDataModel for each Parameter
                dataModelParameters.AddRange(parameters.Select(parameter => new ParameterModel
                {
                    Parameter = parameter, 
                    Name = parameter.Definition.Name, 
                    Value = GetParameterValue(parameter)
                }));
            }
            
            // Sort Models
            IOrderedEnumerable<ParameterModel> sortedDataModelParameters = dataModelParameters.OrderBy(x => x.Name);

            return sortedDataModelParameters;
        }

        /// <summary>
        /// Get Revit Parameter value as a string regardless of StorageType
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Iterate through the List to populate the DataTable
        /// </summary>
        /// <param name="dataModelParameters"></param>
        /// <returns></returns>
        internal Dictionary<string, List<string>> GetDistinctNames(IOrderedEnumerable<ParameterModel> dataModelParameters)
        {
            Dictionary<string, List<string>> distinctParameters = new Dictionary<string, List<string>>();
            foreach (ParameterModel parameter in dataModelParameters)
            {
                if (distinctParameters.TryGetValue(parameter.Name, out List<string> value))
                {
                    value.Add(parameter.Value);
                }
                else
                {
                    distinctParameters.Add(parameter.Name, new List<string> {parameter.Value});
                }
            }

            return distinctParameters;
        }
        
        
        
        public void SaveDataToModel(IOrderedEnumerable<ParameterModel> parameterModel)
        {

        }

        public IOrderedEnumerable<ParameterModel> GetDistinctParameters(List<Element> selectedElements)
        {
            // TODO: Return a list of only distinct parameters. Multiple values should display <varies>
            List<ParameterModel> distinctParameters = new List<ParameterModel>();



            IOrderedEnumerable<ParameterModel> distinctSortedParameters = distinctParameters.OrderBy(x => x.Name);
            return distinctSortedParameters;
        }
    }
}