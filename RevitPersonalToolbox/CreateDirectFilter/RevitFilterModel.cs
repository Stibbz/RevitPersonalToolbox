namespace RevitPersonalToolbox.CreateDirectFilter;

internal class RevitFilterModel
{
    public IEnumerable<View> ViewTemplates { get; set; }
    public IEnumerable<View> Views { get; set; }
    public Dictionary<string, object> Properties { get; set; } = new();
}