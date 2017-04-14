using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ishoukeikaku_3dmax_tool
{
    class OldModel
    {

        private void LogProjectInfo(string testFolder, string logName)
        {
            // vars
            string[] copyTypes = new string[3];
            copyTypes[0] = "jpg";
            copyTypes[1] = "png";
            copyTypes[2] = "gif";
            string[] maxTypes = new string[1];
            maxTypes[0] = "max";

            // list all folders in main testing folder            
            string[] subFolders = Directory.GetDirectories(testFolder);

            // log file
            string tempFolder = @"\\IMPORT-3\share1\00_KwaTest\temp\";
            string logFile = Path.Combine(tempFolder, logName);
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            };

            foreach (string sub in subFolders)
            {
                // local log file
                string localLogFile = Path.Combine(sub, "project.txt");
                if (File.Exists(localLogFile))
                {
                    File.Delete(localLogFile);
                };

                // list all projects in subfolder
                string subName = sub.Substring(sub.LastIndexOf(@"\"), sub.Length - sub.LastIndexOf(@"\")).Trim().Remove(0, 1);
                Console.WriteLine("Project Name: {0}", subName);
                Console.WriteLine("Project Path: {0}", sub);

                // list files in subfolder
                DirectoryInfo subDI = new DirectoryInfo(sub);
                List<string> maxFiles = CopyDir.ListFiles(subDI, copyTypes);

                // find max file in subfolder
                string maxFile = "none";
                List<string> maxAppFiles = CopyDir.ListFiles(subDI, maxTypes);
                if (maxAppFiles.Count > 0)
                {
                    maxFile = maxAppFiles[0];
                };

                // log project name, location, logfile, max file, and all img files to subfolder
                using (var sw = new StreamWriter(localLogFile, true))
                {
                    sw.WriteLine(subName);
                    sw.WriteLine(sub);
                    sw.WriteLine(logFile);
                    sw.WriteLine(maxFile);
                    foreach (string mf in maxFiles) sw.WriteLine(mf);
                    sw.Close();
                };

                // log project name, location, max file, and all img files to main project log
                using (var sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(subName);
                    sw.WriteLine(sub);
                    foreach (string mf in maxFiles) sw.WriteLine(mf);
                    sw.Close();
                };

            };  // end "list all projects in testing folder"
        }

        private void FolderFix_W_AUTOMAP()
        {
            /*
            V 1.0
            Created an automap folder and redirects everything to that AUTOMAP folder
            */

            // list all folders in main testing folder
            string testFolder = @"\\IMPORT-3\share1\00_KwaTest\01_Object\家具_Furniture\ベッド_bed\";
            string tempFolder = @"\\IMPORT-3\share1\00_KwaTest\temp\";
            string[] subFolders = Directory.GetDirectories(testFolder);

            foreach (string sub in subFolders)
            {
                // list all projects in subfolder
                string subName = sub.Substring(sub.LastIndexOf(@"\"), sub.Length - sub.LastIndexOf(@"\")).Trim().Remove(0, 1);
                Console.WriteLine("SUBFOLDER: {0}, ({1})", subName, sub);
                string[] projects = Directory.GetDirectories(sub);
                foreach (string project in projects)
                {
                    // search for a AUTOMAP folder in project
                    string projectName = project.Substring(sub.LastIndexOf(@"\"), project.Length - project.LastIndexOf(@"\")).Trim().Remove(0, 1);
                    string tempAutoFolder = Path.Combine(tempFolder, projectName, "AUTOMAP");
                    string[] projectFullFolders = Directory.GetDirectories(project);
                    List<string> projectFolders = new List<string>();
                    foreach (string s in projectFullFolders)
                    {
                        projectFolders.Add(s.Replace(project, "").Trim().Remove(0, 1));
                    };
                    bool hasMapFolder = (projectFolders.Contains("AUTOMAP")) ? true : false;

                    // vars
                    string[] copyTypes = new string[4];
                    copyTypes[0] = "jpg";
                    copyTypes[1] = "png";
                    copyTypes[2] = "gif";
                    copyTypes[3] = "max";

                    // create merge folder if not exists
                    if (!hasMapFolder)
                    {
                        //check for 新しいフォルダー folder
                        bool hasNewFolder = (projectFolders.Contains("新しいフォルダー")) ? true : false;
                        string sourceDir;
                        if (hasNewFolder)
                        {
                            Console.WriteLine("PROJECT:{0} NO map folder, CREATE", project);
                            sourceDir = project + @"\新しいフォルダー";
                        }
                        else {
                            Console.WriteLine("PROJECT:{0} NO map NO new, PRODUCE", project);
                            sourceDir = project + @"\";
                        };
                        string targetDir = project + @"\AUTOMAP";
                        CopyDir.CopyFlatten(sourceDir, tempAutoFolder, copyTypes);
                        Directory.Move(tempAutoFolder, targetDir);
                        var deleteDir = new DirectoryInfo(Path.Combine(tempFolder, projectName));
                        deleteDir.Delete(true);
                    }
                    else {
                        Console.WriteLine("PROJECT:{0} HAS automap folder", project);
                        //string mergeDir = project + @"\mergeMe";
                        //var deleteDir = new DirectoryInfo(mergeDir);
                        //deleteDir.Delete(true);
                    };

                    // check AUTOMAP folder for project.txt
                    string mapDir = project + @"\AUTOMAP";
                    string[] projectFiles = Directory.GetFiles(mapDir);
                    bool hasMapFile = (projectFiles.Contains("project.txt")) ? true : false;
                    if (!hasMapFile)
                    {
                        Console.WriteLine("PROJECT:{0} has no project.txt FILE - MAKE", project);
                        string path = @"E:\AppServ\Example.txt";
                        if (!File.Exists(path))
                        {
                            File.Create(path);
                            TextWriter tw = new StreamWriter(path);
                            tw.WriteLine("The very first line!");
                            tw.Close();
                        }
                        else if (File.Exists(path))
                        {
                            using (var tw = new StreamWriter(path, true))
                            {
                                tw.WriteLine("The next line!");
                                tw.Close();
                            }
                        }

                    };

                };  // end "list all projects in subfolder"
            };  // end "list all projects in testing folder"
        }
    }
}
