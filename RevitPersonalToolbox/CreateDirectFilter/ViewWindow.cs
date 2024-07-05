using System.Windows;
using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter;

internal class ViewWindow(ViewModel viewModel, RevitUtils revitUtils)
{
    private readonly RevitUtils _revitUtils = revitUtils;

    public void ShowWindow(string mainTitle, string subTitle, Window owner)
    {
        // Create an instance of your WPF window (ViewWindow)
        SingleSelectionWindow singleSelectionWindow = new SingleSelectionWindow(mainTitle, subTitle, owner)
        {
            // Set the ViewModel as the DataContext for the ViewWindow
            DataContext = viewModel
        };

        // Show the window
        singleSelectionWindow.Show();
    }
}