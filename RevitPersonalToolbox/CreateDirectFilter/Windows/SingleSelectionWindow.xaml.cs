using System.Windows;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.CreateDirectFilter.Windows;

public partial class SingleSelectionWindow : Window
{
    public SingleSelectionWindow(Window owner, ViewModel viewModel)
    {
        InitializeComponent();
        Owner = owner;
        MainTitle.Content = "Parameters";
        SubTitle.Content = "Determine which parameter to base the filter on";
        DataContext = viewModel;
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ViewModel viewModel) return;
        viewModel.SelectedItem = ListBoxSelection.SelectedItem;

        Close();

        //viewModel.ApplySelection();
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        Command.Cancelled = true;
        Close();
    }
}