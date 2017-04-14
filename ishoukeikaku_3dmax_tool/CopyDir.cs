using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class CopyDir
{
    public static void Copy(string sourceDirectory, string targetDirectory)
    {
        DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
        DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

        CopyAll(diSource, diTarget);
    }

    public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        Directory.CreateDirectory(target.FullName);

        // Copy each file into the new directory.
        foreach (FileInfo fi in source.GetFiles())
        {
            //Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
            fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }

    }

    public static void CopyFlatten(string sourceDirectory, string targetDirectory, string[] copyTypes)
    {
        // Copies and flattens all folders
        DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
        DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

        CopyFlattenAll(diSource, diTarget, copyTypes);
    }

    public static void CopyFlattenAll(DirectoryInfo source, DirectoryInfo target, string[] copyTypes)
    {
        Directory.CreateDirectory(target.FullName);

        // Copy each file into the new directory.
        foreach (FileInfo fi in source.GetFiles()) {
            string ext = "";
            try {
                ext = fi.Extension.Remove(0, 1);
            } catch { };
            if (copyTypes.Contains(ext)) fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo sub1 in source.GetDirectories()) {
            CopyFlattenAll(sub1, target, copyTypes);
            //foreach (FileInfo f1 in sub1.GetFiles()) f1.CopyTo(Path.Combine(target.FullName, f1.name), true);
            //foreach (DirectoryInfo sub2 in source.GetDirectories()) {
            //};
        }

    }

    public static List<string> ListFiles(DirectoryInfo source, string[] copyTypes)
    {
        List<string> requestedFiles = new List<string>();

        // Get each file into the new directory.
        foreach (FileInfo fi in source.GetFiles()) {
            string ext = "";
            try {
                ext = fi.Extension.Remove(0, 1);
            } catch { };
            if (copyTypes.Contains(ext)) requestedFiles.Add(fi.FullName);
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo sub1 in source.GetDirectories()) {
            List<string> moreFiles = ListFiles(sub1, copyTypes);
            foreach (string f in moreFiles) requestedFiles.Add(f);
        }

        return requestedFiles;
    }
}
