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
    public class excelPlugIn : IExternalCommand
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

            string excelFile = @"C:\Users\Mladen\Desktop\New Microsoft Excel Worksheet.xlsx";

            //creating a variable for an instance of excel
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWb = excelApp.Workbooks.Open(excelFile);
            //we specify the first item
            Excel.Worksheet excelWs = excelWb.Worksheets.Item[1];

            //we scan the data in the document and get all used cells 
            Excel.Range excelRg = excelWs.UsedRange;
            int rowCount = excelRg.Rows.Count;

            //do some stuff in Excel 

            List<string[]> dataList = new List<string[]>();

            for     (int i = 1; i < rowCount; i++)
            {
                Excel.Range cell1 = excelRg.Cells[i, 1];
                Excel.Range cell2 = excelRg.Cells[i, 2];

                string data1 = cell1.Value.ToString();
                string data2 = cell2.Value.ToString();

                string[] dataArray = new string[2];

                dataArray[0] = data1;
                dataArray[1] = data2;

                dataList.Add(dataArray);

            }

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create some Revit stuff");
                Level curLevel = Level.Create(doc, 80);

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfCategory(BuiltInCategory.OST_TitleBlocks);
                collector.WhereElementIsElementType();

                ViewSheet curSheet = ViewSheet.Create(doc, collector.FirstElementId());
                curSheet.SheetNumber = "101011";
                curSheet.Name = "test";

                t.Commit();
                t.Dispose();

            }

            excelWb.Close();
            excelApp.Quit();

            

            return Result.Succeeded;
        }
    }
}
