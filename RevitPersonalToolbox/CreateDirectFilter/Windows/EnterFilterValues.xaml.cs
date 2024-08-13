using System.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter.Windows;

public partial class EnterFilterValues : Window
{
    public bool Cancelled { get; set; }
        
    public EnterFilterValues(Window owner, ViewModel viewmodel)
    {
        InitializeComponent();
        Owner = owner;
        MainTitle.Text = "AllParameters";
        SubTitle.Text = "Determine which parameter to base the filter on";
        DataContext = viewmodel;
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        ViewModel viewModel = DataContext as ViewModel;

        // TODO: implement dynamic equation using something like enums
        viewModel.InputFilterValue = InputParameterValue.Text;
        Close();
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        Command.Cancelled = true;
        Close();
    }
}