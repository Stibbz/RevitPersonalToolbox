namespace RevitPersonalToolbox.CreateDirectFilter;

internal class ViewModel(BusinessLogic businessLogic, RevitUtils revitUtils)
{
    // Fields
    private readonly BusinessLogic _businessLogic = businessLogic;
    private readonly RevitUtils _revitUtils = revitUtils;
    public Dictionary<string, dynamic> Dictionary { get; set; } = new();


    //Properties
    // DialogResult is used to determine the result of the dialog.
    // True when user pressed Apply, if null or false no selection was made.
    public bool DialogResult { get; set; }
    public object SelectedItem { get; set; }

    public void LoadData()
    {
        ICollection<Element> allSelectedElements = _revitUtils.GetAllSelectedElements();
        ICollection<ElementId> parameters = _revitUtils.CreateApplicableElementFilters(allSelectedElements);

        Dictionary<string, dynamic> parameterDictionary = new();
        foreach (ElementId parameter in parameters)
        {
            BuiltInParameter bip = (BuiltInParameter)parameter.IntegerValue;
            string label = LabelUtils.GetLabelFor(bip);
            parameterDictionary.Add(label, parameter);
        }
        Dictionary = Utils.SortDictionary(parameterDictionary);
    }

    public void ApplySelection()
    {
        DialogResult = true;
        if (SelectedItem == null) return;

        //TODO: Variables (temporary)
        string filterName = "filter name testing";
        string parameterValue = "100";

        // Hier zijn we gebleven met de dictionary als resultaat

        KeyValuePair<string, dynamic> kvp = (KeyValuePair<string, dynamic>)SelectedItem;
        string selectedParameterName = kvp.Key;
        ElementId selectedParameter = kvp.Value;


        //DataModel dataModel = (DataModel)SelectedItem;
        //Parameter selectedParameter = dataModel.GetSourceObject();

        //ICollection<Element> selectedElements = _revitUtils.GetAllSelectedElements();
        //revitUtils.CreateViewFilter(selectedElements, selectedParameter, filterName, parameterValue);
    }
}