using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.SelectByParameter
{
    internal class ViewWindow(ViewModel viewModel, RevitUtils revitUtils) : IDisposable
    {
        private readonly RevitUtils _revitUtils = revitUtils;

        public void ShowWindow()
        {
            // Create an instance of your WPF window (ViewWindow)
            DataGridWindow dataGridWindow = new DataGridWindow
            {
                // Set the ViewModel as the DataContext for the ViewWindow
                DataContext = viewModel
            };

            // Show the window
            dataGridWindow.Show();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
