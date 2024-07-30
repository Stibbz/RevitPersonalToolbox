using System.Windows;
using System.Windows.Controls;
using RevitPersonalToolbox.CreateDirectFilter;

namespace RevitPersonalToolbox.Windows
{
    public partial class SingleSelectionWindow : Window
    {
        public bool Cancelled { get; set; }
        
        public SingleSelectionWindow(Window owner)
        {
            InitializeComponent();
            Owner = owner;

        }

        private void OnApplyButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ViewModel viewModel) return;
            viewModel.SelectedItem = ListBoxSelection.SelectedItem;

            Close();

            viewModel.ApplySelection();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ViewModel) return;
            
        }
    }
}
