using Autodesk.Revit.UI;

namespace RevitPersonalToolbox.CreateDirectFilter;

public class ViewModel(RevitUtils revitUtils)
{
    //Properties
    public Dictionary<string, dynamic> ParameterDictionary { get; set; } = [];
    public ICollection<Element> SelectedElements ;
    public ElementId SelectedParameter { get; set; }
    public string InputFilterName { get; set; }
    public string InputFilterValue { get; set; }

    public bool LoadParameterData()
    {
        SelectedElements = revitUtils.GetAllSelectedElements();
        if(SelectedElements==null ) { return Command.Cancelled = true; }

        ICollection<ElementId> applicableParameters = revitUtils.GetApplicableParameters(SelectedElements);

        Dictionary<string, dynamic> parameters = revitUtils.GetParameterData(SelectedElements, applicableParameters);
        ParameterDictionary = Utils.SortDictionary(parameters);

        return Command.Cancelled = false;
    }

    public void ApplyUserInput()
    {
        ICollection<ParameterFilterElement> viewFilters = [];
        ParameterFilterElement viewFilter = revitUtils.CreateViewFilter(SelectedElements, SelectedParameter, InputFilterName, InputFilterValue);
        viewFilters.Add(viewFilter);

        revitUtils.ApplyFilterToView(viewFilters);
    }
}