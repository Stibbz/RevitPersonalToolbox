using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.TemplateActivityChecker
{
    public class ResultViewModel
    {
        // Fields
        private readonly BusinessLogic _businessLogic;
        private readonly RevitUtils _revitUtils;

        // Properties
        

        // Constructors
        public ResultViewModel(BusinessLogic businessLogic, RevitUtils revitUtils)
        {
            _businessLogic = businessLogic;
            _revitUtils = revitUtils;
        }

        // Methods
        public object GetViewsByViewTemplate(View viewTemplate)
        {
            TaskDialog.Show("info", "ok nerd");
            
            return viewTemplate;
        }
    }
}