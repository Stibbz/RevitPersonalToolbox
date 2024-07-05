using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.TemplateActivityChecker
{
    internal class SelectionViewModel
    {
        // Fields
        private readonly BusinessLogic _businessLogic;
        private readonly RevitUtils _revitUtils;

        // Properties
        

        // Constructors
        public SelectionViewModel(BusinessLogic businessLogic, RevitUtils revitUtils)
        {
            _businessLogic = businessLogic;
            _revitUtils = revitUtils;

            GetViewTemplates();
        }

        // Methods
        public object GetViewTemplates()
        {
            IEnumerable<string> viewTemplates = _revitUtils.GetViewTemplates().Select(x => x.Name);
            
            return viewTemplates;
        }

        public object GetViewsByViewTemplate(View viewTemplate)
        {
            TaskDialog.Show("info", "ok nerd");
            
            return viewTemplate;
        }
    }
}
