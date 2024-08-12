using System.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter.Windows;

public partial class SingleSelectionWindow : Window
{
    public SingleSelectionWindow(Window owner, ViewModel viewModel)
    {
        InitializeComponent();
        Owner = owner;
        MainTitle.Text = "Parameters";
        SubTitle.Text = "Determine which parameter to base the filter on";
        DataContext = viewModel;
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ViewModel viewModel) return;
        viewModel.SelectedItem = ListBoxSelection.SelectedItem;

        Hide();
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        Command.Cancelled = true;
        Hide();
    }
}