using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace RevitPersonalToolbox.Windows
{
    public partial class SelectSingleList : Window
    {
        public bool Cancelled { get; set; }
        public Dictionary<string, dynamic> Items { get; set; }
        public List<dynamic> SelectedItems {get;set;}

        public SelectSingleList(string mainTitle, string subTitle, Dictionary<string, dynamic> items, Window owner)
        {
            InitializeComponent();
            Items = Utils.SortDictionary(items);
            MainTitle.Content = mainTitle;
            SubTitle.Content = subTitle;
            Owner = Owner;
            ListBoxSelection.DataContext = Items.Keys;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedItems = new List<dynamic>();
            Cancelled = false;
            IList selectedItems = ListBoxSelection.SelectedItems;
            foreach (string item in selectedItems)
            {
                SelectedItems.Add(Items[item]);
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
