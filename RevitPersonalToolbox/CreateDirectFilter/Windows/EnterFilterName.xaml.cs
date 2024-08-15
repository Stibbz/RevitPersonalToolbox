using System.Windows;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.CreateDirectFilter.Windows;

public partial class EnterFilterName : Window
{
    public EnterFilterName(Window owner, ViewModel viewmodel)
    {
        InitializeComponent();
        Owner = owner;
        MainTitle.Text = "Filter Name";
        SubTitle.Text = "Enter desired name for the filter";
        DataContext = viewmodel;

        //TODO: Implement suggestions for filter naming. Maybe suggestions based on what you start typing?
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        ViewModel viewModel = DataContext as ViewModel;
        Command.Cancelled = false;

        foreach (string existingFilterName in viewModel.ExistingFilterNames)
        {
            if (InputFilterName.Text != existingFilterName) continue;
            TaskDialog.Show("Error", "A filter with that name already exists.\nPlease choose a different name.");
            return;
        }
        
        if (InputFilterName.Text.IsNullOrEmpty() || InputFilterName.Text.IsNullOrWhiteSpace()) return;
        viewModel.FilterName = InputFilterName.Text;
        Hide();
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}