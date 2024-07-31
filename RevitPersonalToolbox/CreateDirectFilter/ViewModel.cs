using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Autodesk.Revit.UI;
using RevitPersonalToolbox.Windows;

namespace RevitPersonalToolbox.CreateDirectFilter;

internal class ViewModel(ExternalCommandData commandData, BusinessLogic businessLogic, RevitUtils revitUtils)
{
    // Private fields
    private readonly ExternalCommandData _commandData = commandData;
    private readonly BusinessLogic _businessLogic = businessLogic;
    private readonly RevitUtils _revitUtils = revitUtils;


    //Properties
    public bool DialogResult { get; set; }
    public ICollection<Element> SelectedElements;
    public object SelectedItem { get; set; }
    public Dictionary<string, dynamic> ParameterDictionary { get; set; } = new();


    public void LoadData()
    {
        SelectedElements = _revitUtils.GetAllSelectedElements();
        ICollection<ElementId> applicableParameters = _revitUtils.GetApplicableParameters(SelectedElements);
        Dictionary<string, dynamic> parameters = _revitUtils.GetParameterData(SelectedElements, applicableParameters);
        ParameterDictionary = Utils.SortDictionary(parameters);
    }

    public void ApplySelection()
    {
        DialogResult = true;
        if (SelectedItem == null) return;

        KeyValuePair<string, dynamic> selectedItem = (KeyValuePair<string, dynamic>)SelectedItem;
        string parameterName = selectedItem.Key;
        ElementId parameter = selectedItem.Value;
        
        string mainTitle = "Create Filter";
        string subTitle = "Define rules for the filter";
        string selectedParameter = $"Parameter:\n{parameterName}";
        Utils.CallNewWindow(commandData, mainTitle, subTitle, selectedParameter);

        //TODO: Variables (temporary)
        string filterName = "filter name testing";
        string parameterValue = "100";




        // TODO: Second window prompting user to create the filter
        // enter "filter name" + "define equals / contains etc." + "parameter value"

        // Create filter
        ParameterFilterElement ViewFilter = revitUtils.CreateViewFilter(SelectedElements, parameter, filterName, parameterValue);

        // Apply filter
        revitUtils.ApplyFilterToView(ViewFilter);

    }
}