using System.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter.Windows;

public partial class PickFilterName : Window
{
    public bool Cancelled { get; set; }
        
    public PickFilterName(Window owner, ViewModel viewmodel)
    {
        InitializeComponent();
        Owner = owner;
        DataContext = viewmodel;
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        ViewModel viewModel = DataContext as ViewModel;
        if (!InputFilterName.Text.IsNullOrEmpty() || !InputFilterName.Text.IsNullOrWhiteSpace())
        {
            if (viewModel != null) viewModel.InputFilterName = InputFilterName.Text;
            Close();
        };
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        Command.Cancelled = true;
        Close();
    }
}