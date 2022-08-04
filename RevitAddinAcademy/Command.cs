#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Forms = System.Windows.Forms;

#endregion

namespace RevitAddinAcademy
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
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

            Forms.OpenFileDialog dialog = new Forms.OpenFileDialog();
            dialog.InitialDirectory = @"C:\";
            dialog.Multiselect = true;
            dialog.Filter = "Revit files | *.rvt; *.rfa";

            //for selecting single files
            string filePath = "";

            //for selecting multiple files
            string[] filePaths;

            //we are opening the file selected above
            if(dialog.ShowDialog()==Forms.DialogResult.OK)
            {
               //filePath = dialog.FileName;
               
                filePaths = dialog.FileNames;
            }


            //we repeat the above but with different fuction
            Forms.FolderBrowserDialog folderDialog = new Forms.FolderBrowserDialog();

            string folderPath = "";

            if (folderDialog.ShowDialog() == Forms.DialogResult.OK)
            {
                folderPath = folderDialog.SelectedPath;
            }

            //have a look at the method below
            TestStruct struct1;
            struct1.Name = "Mladen";
            struct1.Value = 100;
            struct1.Value2 = 88.3;

            List<TestStruct> structList = new List<TestStruct>();
            structList.Add(struct1);

            //we are repeating the above but with 1 line of code
            TestStruct struct2 = new TestStruct("structure 1", 23, 22.3);


            

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(ViewFamilyType));

            ViewFamilyType curVFT = null;
            ViewFamilyType curRCPVFT = null;

            foreach(ViewFamilyType curElem in collector)
            {
                if(curElem.ViewFamily == ViewFamily.FloorPlan)
                {
                    curVFT = curElem;
                }

                else if (curElem.ViewFamily == ViewFamily.CeilingPlan)
                {
                    curRCPVFT = curElem;
                }
            }

            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            collector2.OfCategory(BuiltInCategory.OST_TitleBlocks);
            collector2.WhereElementIsElementType();

            XYZ point = new XYZ(0, 0, 0);




            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create Plan Views");

                Level newLevel = Level.Create(doc, 100);
                ViewPlan curPlan = ViewPlan.Create(doc, curVFT.Id, newLevel.Id);
                ViewPlan curRCP = ViewPlan.Create(doc, curRCPVFT.Id, newLevel.Id);
                curRCP.Name = curRCP.Name + " RCP";

                //check the method at the end GetViewByName

                View exisitngView = GetViewByName(doc, "Level 1");

                ViewSheet newSheet = ViewSheet.Create(doc, collector2.FirstElementId());

                if(exisitngView != null)
                {
                    Viewport newVP = Viewport.Create(doc, newSheet.Id, curPlan.Id, point);
                }
                else
                {
                    TaskDialog.Show("Error", "Could not find view with that Name");
                }
                
                newSheet.Name = "Test Sheet";
                newSheet.SheetNumber = "A0101";


                string paramValue = "";
                foreach(Parameter curParam in newSheet.Parameters)
                {
                    if(curParam.Definition.Name == "Drawn By")
                    {
                        curParam.Set("MK");
                        
                    }
                }

                t.Commit();
                t.Dispose();
            }


            return Result.Succeeded;
        }


        //the same approach as the Parameters logic above. This time we are using a method
        //below you provide the doc and view name and it will return the view name 
        internal View GetViewByName(Document doc, string viewName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(View));

            foreach(View curView in collector)
            {
                if(curView.Name == viewName)
                {
                    return curView;
                }
            }
            return null;
        }

        //creating method to store multiple data formats 
        internal struct TestStruct 
        {
            public string Name;
            public int Value;
            public double Value2;

            //below we create a constructor; it is a method inside the structure and should have the same name
            public TestStruct(string _name, int _value, double _value2)
            {
                Name = _name;
                Value = _value;
                Value2 = _value2;
            }
        }


    }
}
