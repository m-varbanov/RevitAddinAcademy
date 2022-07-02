#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


#endregion

namespace RevitAddinAcademy
{
    [Transaction(TransactionMode.Manual)]
    public class CreateMultipleNotesChallenge : IExternalCommand
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

           
            string text = "Revit Add-in Academy";
            string fileName = doc.PathName;

            double offsest = 0.05;
            double offsetCalc = offsest * doc.ActiveView.Scale;


            //Revit specific
            XYZ curPoints = new XYZ(0,0,0);
            XYZ offsetPoint = new XYZ(0,offsetCalc,0);

            int range = 100;
            int a = 3;
            int b = 5;

             double num1 = checkDivisibility(a, b);
            

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(TextNoteType));


            Transaction newTransaction = new Transaction(doc, "Create Text Note");
            newTransaction.Start();

            for (int i = 1; i < range; i++)
            {
                if (num1 == 3)
                {
                  TextNote curNote = TextNote.Create(doc, doc.ActiveView.Id, curPoints, "FIZZ " + i.ToString(), collector.FirstElementId());
                }
                else if (num1 == 5)
                {
                  TextNote curNote = TextNote.Create(doc, doc.ActiveView.Id, curPoints, "BUZZ " + i.ToString(), collector.FirstElementId());
                }
                else if (num1 == 0 && num1 == 5)
                {
                    TextNote curNote = TextNote.Create(doc, doc.ActiveView.Id, curPoints, "FIZZBUSS " + i.ToString(), collector.FirstElementId());
                }
                else
                {
                    TextNote curNote = TextNote.Create(doc, doc.ActiveView.Id, curPoints, i.ToString(), collector.FirstElementId());
                    
                }


                curPoints = curPoints.Subtract(offsetPoint);
            }


            newTransaction.Commit();
            newTransaction.Dispose();

            return Result.Succeeded;
        }

        internal double checkDivisibility(double a, double b)
        {
            double c = a % b;

            Debug.Print("Got here" + c.ToString());

            return c;
        }   


        //internal TextNote result(Document doc, ElementId elementId, XYZ curPoints, String resultMessage, FilteredElementCollector collector)
        //{
        //    TextNote curNote = TextNote.Create(doc, doc.ActiveView.Id, curPoints, "BUZZ " + i.ToString(), collector.FirstElementId());
        //    return curNote;
        //}
      
    }
}
