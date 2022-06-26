using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using System;

namespace RevitPersonalToolbox
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RibbonApplication : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            throw new NotImplementedException();
        }

        public Result OnStartup(UIControlledApplication application)
        {
            new RevitGlobalVariables(application);
            return Result.Succeeded;
        }
    }
}
