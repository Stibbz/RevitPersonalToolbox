using System.Windows;
using System.Windows.Controls;

namespace RevitPersonalToolbox.Playground.Windows
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class EnterParameter : Window
    {
        public EnterParameter(Window owner, ViewModel viewmodel)
        {
            InitializeComponent();
            Owner = owner;
            MainTitle.Text = "Enter parameter";
            SubTitle.Text = "Enter value to match";
            DataContext = viewmodel;
        }

        private void OnApplyButtonClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
