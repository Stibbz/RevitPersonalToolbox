namespace RevitPersonalToolbox.SelectByParameter
{
    public class RevitExecutor
    {
        private readonly Document _document;
        private readonly RevitUtils _revitUtils;

        public RevitExecutor(Document document, RevitUtils revitUtils)
        {
            _document = document;
            _revitUtils = revitUtils;
        }
    }
}