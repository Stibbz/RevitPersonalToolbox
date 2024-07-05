//using Nice3point.Revit.Toolkit.External;
//using RevitPersonalToolbox.Commands;

//namespace RevitPersonalToolbox
//{
//    /// <summary>
//    ///     Application entry point
//    /// </summary>
//    [UsedImplicitly]
//    public class Application : ExternalApplication
//    {
//        public override void OnStartup()
//        {
//            CreateRibbon();
//        }

//        private void CreateRibbon()
//        {
//            var panel = Application.CreatePanel("Commands", "MultiRevitToolbox");

//            panel.AddPushButton<StartupCommand>("Execute")
//                .SetImage("/MultiRevitToolbox;component/Resources/Icons/RibbonIcon16.png")
//                .SetLargeImage("/MultiRevitToolbox;component/Resources/Icons/RibbonIcon32.png");
//        }
//    }
//}