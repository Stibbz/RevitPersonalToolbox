﻿using System.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter.Windows;

public partial class FilterInputWindow : Window
{
    public bool Cancelled { get; set; }
        
    public FilterInputWindow(Window owner, ViewModel viewmodel)
    {
        InitializeComponent();
        Owner = owner;
        MainTitle.Text = "Parameters";
        SubTitle.Text = "Determine which parameter to base the filter on";
        DataContext = viewmodel;
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
        ViewModel viewModel = DataContext as ViewModel;

        viewModel.InputFilterName = FilterNameInput.Text;
        // TODO: implement dynamic equation using something like enums
        viewModel.InputFilterValue = ParameterValueInput.Text;
        Close();

        viewModel.ApplyInput();
    }

    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        Command.Cancelled = true;
        Close();
    }
}