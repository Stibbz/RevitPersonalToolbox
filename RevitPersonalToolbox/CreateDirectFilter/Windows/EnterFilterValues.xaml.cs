using System.Windows;
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
        IsGreaterThanOrEqualTo,
        IsLessThan,
        IsLessThanOrEqualTo,
        Contains,
        DoesNotContain,
        BeginsWith,
        DoesNotBeginWith,
        EndsWith,
        DoesNotEndWith,
        HasValue,
        HasNoValue
    }

    public EnterFilterValues(Window owner, ViewModel viewModel)
    {
        InitializeComponent();
        Owner = owner;
        MainTitle.Text = "Parameter Values";
        SubTitle.Text = "Provide a name for the filter";
        DataContext = viewModel;
        viewModel.LoadData();
        
        EquationSelect.ItemsSource = Enum.GetValues(typeof(ComparisonRule)).Cast<ComparisonRule>();
        EquationSelect.SelectedIndex = 0;

        InputParameterValue.ItemsSource = viewModel.ValueSuggestions;
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