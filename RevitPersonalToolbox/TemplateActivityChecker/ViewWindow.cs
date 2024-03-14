namespace RevitPersonalToolbox.TemplateActivityChecker
{
    internal class ViewWindow
    {
        private readonly RevitUtils _revitUtils; 
        private readonly ViewModel _viewModel;

        public ViewWindow(ViewModel viewModel, RevitUtils revitUtils)
        {
            _revitUtils = revitUtils;
            _viewModel = viewModel;
        }
    }
}
