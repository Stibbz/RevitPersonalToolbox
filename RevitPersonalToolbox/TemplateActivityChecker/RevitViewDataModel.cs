namespace RevitPersonalToolbox.TemplateActivityChecker
{
    public class RevitViewDataModel
    {
        public IEnumerable<View> ViewTemplates { get; set; }
        public IEnumerable<View> Views { get; set; }
        public Dictionary<string, object> Properties { get; set; } = new();
    }
}
