#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace RevitAddinAcademy
{
    [Transaction(TransactionMode.Manual)]
    public class cmdWallsFromLines : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            List<string> wallTypes = GetAllWallTypeNames(doc);
            List<string> lineStypes = GetAllLineStyles(doc);

            return Result.Succeeded;
        }

        private List<string> GetAllLineStyles(Document doc)
        {
            List<string> results = new List<string>();

            //we will be more specific here and convert only the lines that are visible in the view 

            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
            collector.OfClass(typeof(CurveElement));

            foreach(CurveElement element in collector)
            {
                GraphicsStyle curGS = element.LineStyle as GraphicsStyle;

                //below we filter the unique instances of the line styles
                if(results.Contains(curGS.Name) == false)
                {
                    results.Add(curGS.Name);
                }
            }

            return results;
            
        }

        private List<string> GetAllWallTypeNames(Document doc)
        {


            
        }
    }
}
