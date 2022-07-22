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
            dialog.Filter = "Revit Files | *.rvt; *.rfa";

            string filePath = "";

            //when we set the multiselect to true
            string[] filePaths;

            if(dialog.ShowDialog() == Forms.DialogResult.OK)
            {
                //filePath = dialog.FileName;
                filePaths = dialog.FileNames;

                
            }

            Forms.FolderBrowserDialog folderDialog = new Forms.FolderBrowserDialog();

            string folderPath = "";

            if(folderDialog.ShowDialog() == Forms.DialogResult.OK)
            {
                folderPath = folderDialog.SelectedPath;
            }

            Tuple<string, int> t1 = new Tuple<string, int>("string1", 55);
            Tuple<string, int> t2 = new Tuple<string, int>("string2", 155);


            TestStruct struct1;
            struct1.Name = "Structure 1";
            struct1.Value = 123;
            struct1.Value2 = 345.2;

            TestStruct struct2 = new TestStruct("Strructure 1", 10, 22.3);

            List<TestStruct> structList = new List<TestStruct>();
            structList.Add(struct1);
           
            

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(ViewFamilyType));

            ViewFamilyType curVFT = null;
            ViewFamilyType curCPVFT = null;

            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            collector2.OfCategory(BuiltInCategory.OST_TitleBlocks);
            collector2.WhereElementIsElementType();


            //the loop is going through all elements in the collector and looking for a match(floor plan)
            foreach(ViewFamilyType curElem in collector)
            {
                if (curElem.ViewFamily == ViewFamily.FloorPlan)
                {
                    curVFT = curElem;
                }

                else if(curElem.ViewFamily == ViewFamily.CeilingPlan)
                {
                    curCPVFT = curElem;
                }
            }

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create Views");

                //creating plan views
                Level newLevel = Level.Create(doc, 100);

                ViewPlan curPlan = ViewPlan.Create(doc, curVFT.Id, newLevel.Id);
                ViewPlan curRCP = ViewPlan.Create(doc, curCPVFT.Id, newLevel.Id);
                curRCP.Name = curRCP.Name + " RCP";



                View existingView = GetViewByName(doc, "Level 1");

                ViewSheet newSheet = ViewSheet.Create(doc, collector2.FirstElementId());

                if(existingView != null)
                {
                    Viewport newVP = Viewport.Create(doc, newSheet.Id, curPlan.Id, new XYZ(0, 0, 0));
                }
                else
                {
                    TaskDialog.Show("Error", "Could not find view item");
                }
                

                newSheet.Name = "TEST SHEET";
                newSheet.SheetNumber = "A10101";
                

                foreach(Parameter curParam in newSheet.Parameters)
                {
                    if(curParam.Definition.Name == "Drawn By")
                    {
                        curParam.Set("MV");
                    }
                }



                t.Commit();
                t.Dispose();
            }

         



            return Result.Succeeded;
        }

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

        internal struct TestStruct
        {
            public string Name;
            public int Value;
            public double Value2;

            public TestStruct(string _name, int _value, double _value2)
            {
                Name = _name;
                Value = _value;
                Value2 = _value2;

            }

        }
    }
}
