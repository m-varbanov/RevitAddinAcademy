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
    public class cmdSelectElements : IExternalCommand
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

            IList<Element> pickList = uidoc.Selection.PickElementsByRectangle("Select some elements:");
            List<CurveElement> curveList = new List<CurveElement>();

            WallType curWallType = GetWallTypeByName(doc, "Wall-Ext_102Bwk-75Ins-100LBlk-12P");
            Level curLevel = GetLevelByName(doc, "Level 1");

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create a Wall");
                foreach (Element element in pickList)
                {
                    //asking if the element of that class(Walls, Doors etc.)
                    if (element is CurveElement)
                    {
                        //we are checking if the element is that element, but cant directly access its properties
                        //we need to cast it first 

                        CurveElement curve = (CurveElement)element;
                        CurveElement curve2 = element as CurveElement;

                        curveList.Add(curve);

                        GraphicsStyle curGS = curve.LineStyle as GraphicsStyle;

                        Curve curCurve = curve.GeometryCurve;
                        XYZ startPoint = curCurve.GetEndPoint(0);
                        XYZ endPoint = curCurve.GetEndPoint(1);

                        Wall newWall = Wall.Create(doc, curCurve, curWallType.Id, curLevel.Id, 15, 0, false, false);

                        Debug.Print(curGS.Name);
                    }
                }
                t.Commit();
                t.Dispose();

            }
                

            TaskDialog.Show("Complete", curveList.Count.ToString());
            
            return Result.Succeeded;
        }

        private WallType GetWallTypeByName (Document doc, string wallTypeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(WallType));

            foreach(Element curElem in collector)
            {
                WallType wallType = (WallType)curElem;

                if(wallType.Name == wallTypeName)
                {
                    return wallType;    
                }
                //return null;
            }
            return null;
        }
        private Level GetLevelByName(Document doc, string levelName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Level));

            foreach (Element curElem in collector)
            {
                Level level = (Level)curElem;

                if (level.Name == levelName)
                {
                    return level;
                }
                //return null;

            }
            return null;
        }
    }
}
