using System.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter.Windows
{
    public partial class FilterInputWindow : Window
    {
        public bool Cancelled { get; set; }
        
        public FilterInputWindow(Window owner)
        {
            InitializeComponent();
            Owner = owner;
        }

        private void OnApplyButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ViewModel viewModel) return;
            //viewModel.SelectedItem = ListBoxSelection.SelectedItem;

            Close();

            viewModel.ApplySelection();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ViewModel) return;
            
        }
    }
}