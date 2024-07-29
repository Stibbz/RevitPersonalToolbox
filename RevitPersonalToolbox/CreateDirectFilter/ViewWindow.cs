using System.Windows;
using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter;

internal class ViewWindow(ViewModel viewModel, RevitUtils revitUtils)
{
    private readonly RevitUtils _revitUtils = revitUtils;

    public void ShowWindow(Window owner)
    {
        SingleSelectionWindow singleSelectionWindow = new(owner)
        {
            DataContext = viewModel
        };

        singleSelectionWindow.Show();
    }
}