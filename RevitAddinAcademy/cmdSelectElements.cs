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


            foreach(Element element in pickList)
            {
                //asking if the element of that class(Walls, Doors etc.)
                if(element is CurveElement)
                {
                    //we are checking if the element is that element, but cant directly access its properties
                    //we need to cast it first 
                    
                    CurveElement curve = (CurveElement)element;
                    CurveElement curve2 = element as CurveElement;

                    curveList.Add(curve);

                }
            }

            TaskDialog.Show("Complete", curveList.Count.ToString());
            
            return Result.Succeeded;
        }
    }
}
