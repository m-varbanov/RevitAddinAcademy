#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

#endregion

namespace RevitAddinAcademy
{
    [Transaction(TransactionMode.Manual)]
    public class cmdProjectSetup : IExternalCommand
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

            string excelPath = @"C:\Users\Mladen\Downloads\Session02_Challenge-220706-113155.xlsx";

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWb = excelApp.Workbooks.Open(excelPath);

            //Get the levels 
            Excel.Worksheet excelWs1 = excelApp.Worksheets.Item[1];
            Excel.Worksheet excelWs2 = excelApp.Worksheets.Item[2];

            Excel.Range excelRg = excelWs1.UsedRange;
            Excel.Range excelRg2 = excelWs2.UsedRange;



            int rowsCount1 = excelRg.Rows.Count;
            int rowsCount2 = excelRg2.Rows.Count;

            using(Transaction t = new Transaction(doc))
            {
                t.Start("Project Setup");

                //levels 
                //the loop should start at 2 because of the header in excel
                for (int i = 2; i <= rowsCount1; i++)
                {
                    //creating a loop specific for the column
                    //level name
                    Excel.Range levelData1 = excelWs1.Cells[i, 1];
                    //level elevation
                    Excel.Range levelData2 = excelWs1.Cells[i, 2];

                    string levelName = levelData1.Value.ToString();
                    double levelElev = levelData2.Value;

                    Level newLevel = Level.Create(doc, levelElev);
                    newLevel.Name = levelName;


                }

                //get title block 
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfCategory(BuiltInCategory.OST_TitleBlocks);
                collector.WhereElementIsElementType();


                //sheets
                for (int j = 2; j <= rowsCount2; j++)
                {
                    Excel.Range sheetData1 = excelWs2.Cells[j, 1];
                    Excel.Range sheetData2 = excelWs2.Cells[j, 2];

                    string sheetNum = sheetData1.Value.ToString();
                    string sheetName = sheetData2.Value.ToString();

                    ViewSheet newSheet = ViewSheet.Create(doc, collector.FirstElementId());

                    newSheet.SheetNumber = sheetNum;
                    newSheet.Name = sheetName;
                    

                }

                t.Commit();
                t.Dispose();
            }

            excelWb.Close();
            excelApp.Quit();


              
            return Result.Succeeded;
        }
    }
}
