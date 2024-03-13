using System.Collections.Generic;
using System.Windows;

namespace RevitPersonalToolbox.Windows
{
    public partial class SelectSingleList : Window
    {
        public bool Cancelled { get; set; }
        public Dictionary<string, dynamic> Items { get; set; }
        public dynamic SelectedItem { get; set; }

        public SelectSingleList(string mainTitle, string subTitle, Dictionary<string, dynamic> items, Window owner)
        {
            InitializeComponent();
            
            MainTitle.Content = mainTitle;
            SubTitle.Content = subTitle;
            Items = Utils.SortDictionary(items);
            ListBoxSelection.DataContext = Items.Keys;
            Owner = owner;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Cancelled = false;
            string selectedItem = ListBoxSelection.SelectedItem.ToString();

            // Get View object from the name
            SelectedItem = Items[selectedItem];

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
