using System.Windows;
using RevitPersonalToolbox.TemplateActivityChecker;

namespace RevitPersonalToolbox.Windows
{
    public partial class SingleSelectionWindow : Window
    {
        public bool Cancelled { get; set; }
        
        public SingleSelectionWindow(string mainTitle, string subTitle, Window owner)
        {
            InitializeComponent();
            MainTitle.Content = mainTitle;
            SubTitle.Content = subTitle;
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
