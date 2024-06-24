using Autodesk.Revit.DB;

namespace RevitPersonalToolbox.CreateDirectFilter;

internal class BusinessLogic
{
    // Fields
    private readonly Document _document;


    // Constructors
    public BusinessLogic(Document document)
    {
        _document = document;
    }
}