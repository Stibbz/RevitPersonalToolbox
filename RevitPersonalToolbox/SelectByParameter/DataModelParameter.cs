namespace RevitPersonalToolbox.SelectByParameter
{
    internal class DataModelParameter
    {
        public Parameter Parameter { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public DataModelParameter(Parameter parameter)
        {
            Parameter = parameter;
            Name = parameter.Definition.Name;
            Value = GetParameterValue(parameter);
        }

        private string GetParameterValue(Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    Value = parameter.AsValueString();
                    break;
                case StorageType.ElementId:
                    Value = parameter.AsValueString();
                    break;
                case StorageType.Integer:
                    Value = parameter.AsValueString();
                    break;
                case StorageType.None:
                    Value = parameter.AsValueString();
                    break;
                case StorageType.String:
                    Value = parameter.AsString();
                    break;
            }
            return Value;
        }

        // Get all values for each parameter (if different values display "<varies>")
    }
}
