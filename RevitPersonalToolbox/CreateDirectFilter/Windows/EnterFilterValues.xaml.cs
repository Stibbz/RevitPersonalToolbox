using System.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter.Windows;

public partial class EnterFilterValues : Window
{
    public bool Cancelled { get; set; }
        
    public EnterFilterValues(Window owner, ViewModel viewmodel)
    {
        InitializeComponent();
        Owner = owner;
        MainTitle.Text = "Parameter Values";
        SubTitle.Text = "Provide a name for the filter";
        DataContext = viewmodel;
        viewmodel.LoadData();

        //TODO: Tested this on EquationSelect, move this to InputParameterValue
        EquationSelect.ItemsSource = viewmodel.ValueSuggestions;
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        ViewModel viewModel = DataContext as ViewModel;

        // TODO: implement dynamic equation using something like enums
        viewModel.FilterValue = InputParameterValue.Text;
        Close();
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        Command.Cancelled = true;
        Close();
    }
}