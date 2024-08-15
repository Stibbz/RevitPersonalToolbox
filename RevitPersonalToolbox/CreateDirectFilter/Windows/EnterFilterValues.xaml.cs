using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace RevitPersonalToolbox.CreateDirectFilter.Windows;

public partial class EnterFilterValues : Window
{
    public enum ComparisonRule
    {
        Equals,
        DoesNotEqual,
        IsGreaterThan,
        IsLessThan,
        Contains,
        DoesNotContain
    }

    public EnterFilterValues(Window owner, ViewModel viewmodel)
    {
        InitializeComponent();
        Owner = owner;
        MainTitle.Text = "Parameter Values";
        SubTitle.Text = "Provide a name for the filter";
        DataContext = viewmodel;
        viewmodel.LoadData();

        EquationSelect.ItemsSource = Enum.GetValues(typeof(ComparisonRule)).Cast<ComparisonRule>();
        EquationSelect.SelectedIndex = 0;

        InputParameterValue.ItemsSource = viewmodel.ValueSuggestions;
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        ViewModel viewModel = DataContext as ViewModel;
        Command.Cancelled = false;

        viewModel.ComparisonRule = (ComparisonRule)EquationSelect.SelectedItem;
        viewModel.FilterValue = InputParameterValue.Text;
        Close();
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    { 
        Close();
    }

    private void InputParameterValue_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        ResetTextStyle();
    }

    private void InputParameterValue_OnDropDownOpened(object sender, EventArgs e)
    {
        ResetTextStyle();
    }

    private void ResetTextStyle()
    {
        InputParameterValue.Foreground = new SolidColorBrush(Colors.Black);
        InputParameterValue.FontStyle = FontStyles.Normal;
    }
}