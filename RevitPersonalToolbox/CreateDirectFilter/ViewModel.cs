using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace RevitPersonalToolbox.CreateDirectFilter;

internal class ViewModel(BusinessLogic businessLogic, RevitUtils revitUtils) : INotifyCollectionChanged
{
    // Fields
    private readonly BusinessLogic _businessLogic = businessLogic;
    private readonly RevitUtils _revitUtils = revitUtils;
    private ObservableCollection<string> _items;

    //Properties
    // DialogResult is used to determine the result of the dialog.
    // True when user pressed Apply, if null or false no selection was made.
    public bool DialogResult { get; set; }
    public object ActiveView { get; set; }
    public object SelectedItems { get; set; }
    public ObservableCollection<string> Items
    {
        get => _items;
        set
        {
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void ApplySelection()
    {
        DialogResult = true;
        if (SelectedItems == null) return;

        ICollection<Element> selectedElements = (ICollection<Element>)SelectedItems;


        //Variables (temporary)
        string filterName = "filter name testing";
        string parameterValue = "100";

        revitUtils.CreateViewFilter(selectedElements, filterName, parameterValue);
    }

    public void LoadData()
    {
        ICollection<Element> selectedElements = _revitUtils.GetSelectedElements();

        IEnumerable<Parameter> parameters = Utils.GetParametersFromElements(selectedElements);
        Dictionary<string, dynamic> paramDict = new();
        foreach (Parameter parameter in parameters)
        {
            paramDict.Add(parameter.Definition.Name, parameter);
        }

        paramDict = Utils.SortDictionary(paramDict);
        Items = new ObservableCollection<string>(paramDict.Keys);
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged;
}