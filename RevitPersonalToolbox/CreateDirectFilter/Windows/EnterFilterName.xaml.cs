using System.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter.Windows;

public partial class EnterFilterName : Window
{
    public bool Cancelled { get; set; }
        
    public EnterFilterName(Window owner, ViewModel viewmodel)
    {
        InitializeComponent();
        Owner = owner;
        MainTitle.Text = "Filter Name";
        SubTitle.Text = "Enter desired name for the filter";
        DataContext = viewmodel;
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        ViewModel viewModel = DataContext as ViewModel;

        if (InputFilterName.Text.IsNullOrEmpty() || InputFilterName.Text.IsNullOrWhiteSpace()) return;
        viewModel.InputFilterName = InputFilterName.Text;
        Hide();

        //TODO: Check current filter names against what's been entered to avoid failure due to duplicate naming
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        Command.Cancelled = true;
        Close();
    }
}