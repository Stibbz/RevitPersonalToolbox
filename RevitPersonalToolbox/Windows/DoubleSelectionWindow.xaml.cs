using System.Windows;

namespace RevitPersonalToolbox.Windows
{
    /// <summary>
    /// Interaction logic for DoubleSelectionWindow.xaml
    /// </summary>
    public partial class DoubleSelectionWindow : Window
    {
        public bool Cancelled { get; set; }
        public Dictionary<string, dynamic> Items { get; set; }
        public List<dynamic> SelectedItems {get;set;}

        public DoubleSelectionWindow(string mainTitle, string subTitle, Dictionary<string, dynamic> items, Window owner)
        {
            InitializeComponent();
            Items = Utils.SortDictionary(items);
            MainTitle.Content = mainTitle;
            SubTitle.Content = subTitle;
            Owner = Owner;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedItems = new List<dynamic>();
            this.Cancelled = false;
            //var selectedItems = ItemNamesListBox.SelectedItem;
            //foreach (string item in selectedItems)
            //{
            //    SelectedItem.Add(Items[item]);
            //}
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
