using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace RevitPersonalToolbox.CreateDirectFilter.Windows;

public partial class SelectionWindow : Window
{
    private readonly ViewModel _viewModel;
    public ObservableCollection<string> Parameters { get; set; }

    public SelectionWindow(Window owner, ViewModel viewModel)
    {
        InitializeComponent();
        Owner = owner;
        MainTitle.Text = "Parameters";
        SubTitle.Text = "Enter the parameter value to filter by";

        Parameters = new ObservableCollection<string>(viewModel.ParameterDictionary.Keys);
        DataContext = this;

        _viewModel = viewModel;

        // Since this is the main window and Cancelled is true by default:
        // If Parameters is not empty, this indicates successful loading, so set Cancelled to false.
        if (Parameters.Count == 0) return;
        Command.Cancelled = false;
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox box = (TextBox)sender;
        string searchString = box.Text;
        if(searchString == "")
        {
            try
            {
                SearchList("");
            }
            catch { }
        }
        else
        {
            SearchList(searchString);
        }
    }

    private void SearchList(string searchTerm)
    {
        IList<object> selectedItems = ListBoxSelection.SelectedItems.Cast<object>().ToList();

        Parameters.Clear();
        string[] searchTerms = searchTerm.Split(' ');
        foreach(string key in _viewModel.ParameterDictionary.Keys)
        {
            bool match = true;
            foreach(string searchString in searchTerms)
            {
                if(key.ToLower().Contains(searchString.ToLower())) continue;
                if(Regex.Match(key, searchTerm).Success)
                {
                    match = true;
                    break;
                }
                match = false;
            }

            if(!match) continue;

            Parameters.Add(key);
            if(selectedItems.Contains(key))
            {
                Parameters.Add(key);
            }
        }
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        Command.Cancelled = false;

        string selectedItem = ListBoxSelection.SelectedItem.ToString();
        _viewModel.SelectedParameter = _viewModel.ParameterDictionary[selectedItem];

        if(ListBoxSelection.SelectedItem != null) Hide();
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        Hide();
    }
}