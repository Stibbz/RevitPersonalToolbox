using System.Net;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.CreateDirectFilter.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter;

public class ViewModel(RevitUtils revitUtils)
{
    //Properties
    public Dictionary<string, dynamic> ParameterDictionary { get; set; } = [];
    public ICollection<Element> SelectedElements ;
    public ElementId SelectedParameter { get; set; }
    public IEnumerable<string> ExistingFilterNames { get; set; }
    public string FilterName { get; set; }
    public string FilterValue { get; set; }
    public IEnumerable<string> ValueSuggestions { get; set; }
    public EnterFilterValues.ComparisonRule ComparisonRule { get; set; }

    public void LoadData()
    {
        SelectedElements = revitUtils.GetAllSelectedElements();
        if (SelectedElements == null) return;

        ICollection<ElementId> applicableParameters = revitUtils.GetApplicableParameters(SelectedElements);
        Dictionary<string, dynamic> parameterDictionary = revitUtils.GetParameterData(SelectedElements, applicableParameters);
        ParameterDictionary = Utils.SortDictionary(parameterDictionary);

        ExistingFilterNames = revitUtils.GetFilterElements().ToElements().Select(x => x.Name);

        if (SelectedParameter != null)
        {
            ValueSuggestions = revitUtils.GetParameterValues(SelectedElements, SelectedParameter);
        }
    }

    public void ApplyUserInput()
    {
        ICollection<ParameterFilterElement> viewFilters = [];
        ParameterFilterElement viewFilter = revitUtils.CreateViewFilter(SelectedElements, SelectedParameter, FilterName, FilterValue);
        viewFilters.Add(viewFilter);

        revitUtils.ApplyFilterToView(viewFilters);
    }
}