#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
#endregion

namespace RevitAddinAcademy
{
    [Transaction(TransactionMode.Manual)]
    public class cmdDeleteBackupsDate : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            //set variables
            int counter = 0;
            string logPath = "";
            DateTime dateTime = DateTime.Now;

                
            //create a list for log file
            List<string> deletedFileLog = new List<string>();
            deletedFileLog.Add("The following backup files have been deleted:");

            FolderBrowserDialog selectFolder = new FolderBrowserDialog();
            selectFolder.ShowNewFolderButton = false;

            //open folder diaglog and only run code if a folder is selected 
            if (selectFolder.ShowDialog() == DialogResult.OK)
            {
                //get the selected folder path 
                string directory = selectFolder.SelectedPath;

                //get all folder files 
                string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

                //loop through files 
                foreach (string file in files)
                {
                    //check if file is a revit file 
                    if (Path.GetExtension(file) == ".rvt" || Path.GetExtension(file) == ".rfa")
                    {
                        //get the last 9 characters of file name to check if backup
                        string checkString = file.Substring(file.Length - 9, 9);
                        if (checkString.Contains(".0") == true)
                        {
                            //check when file was last write time
                            DateTime fileModified = File.GetLastWriteTime(file);

                            string user = System.IO.File.GetAccessControl(directory).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();

                            if (fileModified > DateTime.Now.AddMonths(-1))
                            {
                                //add file name to our list
                                deletedFileLog.Add(file + " - Last Modified: " + fileModified.ToString() + " - Created By: " + user);

                                File.Delete(file);

                                //increment counter 
                                counter++;

                            }

                        }

                    }
                }

                //output log files that were deleted 
                if (counter > 0)
                {
                    logPath = WriteListToExt(deletedFileLog, directory);
                }
            }

            //alert the user
            TaskDialog td = new TaskDialog("Complete");
            td.MainInstruction = "Deleted " + counter.ToString() + " backup files.";
            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Click to view log file.");
            td.CommonButtons = TaskDialogCommonButtons.Ok;

            TaskDialogResult result = td.Show();

            if (result == TaskDialogResult.CommandLink1)
            {
                Process.Start(logPath);
            }

            return Result.Succeeded;
        }

        internal string WriteListToExt(List<string> stringList, string filePath)
        {
            string fileName = "_Deleted Backup Files.txt";
            string fullPath = filePath + @"\" + fileName;

            File.WriteAllLines(fullPath, stringList); 
            return fullPath;
        }
    }
}
