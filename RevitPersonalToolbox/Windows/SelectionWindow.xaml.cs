using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RevitPersonalToolbox.Windows
{
    /// <summary>
    /// Interaction logic for SelectionWindow.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        public bool Cancelled { get; set; }
        public Dictionary<string, dynamic> Items { get; set; }
        public List<dynamic> SelectedItems {get;set;}

        public SelectionWindow(string mainTitle, string subTitle, Dictionary<string, dynamic> items, Window owner)
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
            //var selectedItems = ItemNamesListBox.SelectedItems;
            //foreach (string item in selectedItems)
            //{
            //    SelectedItems.Add(Items[item]);
            //}
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
