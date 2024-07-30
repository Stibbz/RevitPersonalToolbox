namespace RevitPersonalToolbox.CreateDirectFilter;

internal class DataModel()
{
    public string Name { get; set; }
    private dynamic SourceObject { get; set; }

    // CONSTRUCTORS
    public DataModel(Parameter parameter) : this()
    {
        Name = parameter.Definition.Name;
        SourceObject = parameter;
    }
    
    public DataModel(Category category) : this()
    {
        Name = category.Name;
        SourceObject = category;
    }

    // METHODS
    public dynamic GetSourceObject()
    {
        return SourceObject;
    }
}