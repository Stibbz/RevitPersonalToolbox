using System.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter.Windows;

public partial class SelectionWindow : Window
{
    public SelectionWindow(Window owner, ViewModel viewModel)
    {
        InitializeComponent();
        Owner = owner;
        MainTitle.Text = "AllParameters";
        SubTitle.Text = "Determine which parameter to base the filter on";
        DataContext = viewModel;
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        ViewModel viewModel = DataContext as ViewModel;

        KeyValuePair<string, dynamic> kvp = (KeyValuePair<string, dynamic>)ListBoxSelection.SelectedItem;
        ElementId parameter = kvp.Value;

        viewModel.SelectedParameter = parameter;

        if(ListBoxSelection.SelectedItem != null) Hide();
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        Command.Cancelled = true;
        Hide();
    }
}