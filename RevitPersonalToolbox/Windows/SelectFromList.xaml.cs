using System.Windows;

namespace RevitPersonalToolbox.Windows
{
    /// <summary>
    /// Interaction logic for SelectFromList.xaml
    /// </summary>
    public partial class SelectFromList : Window
    {
        public SelectFromList()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void ShowWindow()
        {
            throw new System.NotImplementedException();
        }
    }
}
