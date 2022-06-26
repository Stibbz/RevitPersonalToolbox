using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitPersonalToolbox;

public class RevitGlobalVariables
{
    // Fields
    private ControlledApplication _application { get; set; }
    private UIControlledApplication _uiApplication { get; set; }
    private static RevitGlobalVariables _current { get; set; }

    // Properties
    public ControlledApplication RevitApplication => _application;
    public static RevitGlobalVariables Current => _current;
    public UIControlledApplication RevitUiApplication => _uiApplication;
        
    //Constructors
    public RevitGlobalVariables(UIControlledApplication revitUiApplication)
    {
        this._uiApplication = revitUiApplication;
        this._application = revitUiApplication.ControlledApplication;
        _current = this;
    }
        
    // Methods
    public UIDocument GetCurrentUiDocument()
    {
        //return RevitUIApplication;
        return null;
    }
    public Document GetCurrentDocument()
    {
        //return this.RevitUIApplication.ActiveUIDocument.Document;
        return null;
    }
}