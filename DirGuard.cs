﻿namespace DirectoryGuardian;

public class DirGuard(Setup setup)
{
    private readonly Setup set_up = setup;
    public Setup Setup { get { return set_up; } }

    private List<string> Extensions = [];
    public List<string> Extensions_List { get { return Extensions; } }

    public void Directory_Guardian(JobType jobType)
    {
        // We will guard a directory, sort files after file extensions or other variables such as name, size
        List<string> dirPathToGuard;
        //if (jobType is JobType.Initialize)
        //{
        dirPathToGuard = set_up.FetchDirectoriesToSort();
        //}

        var dirinfo = Directory.GetFiles(dirPathToGuard[0]);

        // fetch our extensions, this will be displayed in UI to select which to sort

        Extensions = FetchExtensions(dirinfo);
        if (jobType is JobType.Sort)
        {
            SortExtensionType(dirPathToGuard[0], dirinfo, set_up.ExtensionsToSort);
        }
    }

    public static void Main()
    {
        // for compiler
    }

    public static List<string> FetchExtensions(string[] directoryInfo)
    {
        // read our extensions
        var listOfExtensions = new List<string>();
        foreach (var file in directoryInfo)
        {
            // we should only add the extension if its not already in the list
            if (!listOfExtensions.Contains(Path.GetExtension(file)))
            {
                listOfExtensions.Add(Path.GetExtension(file));
            }
        }
        return listOfExtensions;
    }

    private void SortExtensionType(string singledirpath, string[] dirinfo, List<string> listOfFileTypesToSort)
    {
        CreateSortingDirectory(singledirpath, listOfFileTypesToSort);
        foreach (var file in dirinfo.Where(file => listOfFileTypesToSort.Contains(Path.GetExtension(file)))
        // we will then sort the files of that type to the dir
        )
        {
            var destinationPath = Path.Combine(singledirpath, Path.GetExtension(file).Replace(".", ""), Path.GetFileName(file));
            File.Move(file, destinationPath);
        }

        foreach (var extension in listOfFileTypesToSort)
        {
            Extensions.Remove(extension);
        }
        set_up.ExtensionsToSort.Clear();


    }

    private static void CreateSortingDirectory(string singledirpath, List<string> listOfFileTypesToSort)
    {
        // create our directories
        foreach (var file in listOfFileTypesToSort)
        {
            var dir = Path.Combine(singledirpath, file).Replace(".", "");
            Directory.CreateDirectory(dir);
        }
    }

    public static void GuardDir(string dir)
    {
        var Watcher = new FileSystemWatcher(dir)
        {
            EnableRaisingEvents = true
        };
    }
}