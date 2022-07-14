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
    public class ComExcelPlugInmand : IExternalCommand
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




            excelWb.Close();
            excelApp.Quit();




            return Result.Succeeded;
        }
    }
}
