namespace DirectoryGuardian;

public class DirGuard
{
    private readonly Setup set_up;
    private List<string> Extensions;
    public List<string> Extensions_List { get { return Extensions; } }
    public DirGuard(Setup setup)
    {
        set_up = setup;
    }

    public static void Main()
    {
        // Just for compiler
    }

    public void Directory_Guardian()
    {
        // We will guard a directory, sort files after file extensions or other variables such as name, size
        var dirPathToGuard = set_up.FetchDirectoriesToSort();

        // just shenanigans to pull the first dir
        var singledirpath = dirPathToGuard[0];

        var dirinfo = Directory.GetFiles(singledirpath);

        // fetch our extensions, this will be displayed in UI to select which to sort
        Extensions = FetchExtensions(dirinfo);

        // just hard coded for test purposes
        var listOfFileTypesToSort = new List<string>();
        listOfFileTypesToSort.Add(".jpg");

        SortExtensionType(singledirpath, dirinfo, listOfFileTypesToSort);
    }

    public List<string> FetchExtensions(string[] directoryInfo)
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

    private static void SortExtensionType(string singledirpath, string[] dirinfo, List<string> listOfFileTypesToSort)
    {
        CreateSortingDirectory(singledirpath, listOfFileTypesToSort);
        // we will then sort the files of that type to the dir
        foreach (var file in dirinfo)
        {
            if (listOfFileTypesToSort.Contains(Path.GetExtension(file)))
            {
                var destinationPath = Path.Combine(singledirpath, Path.GetExtension(file).Replace(".", ""), Path.GetFileName(file));
                File.Move(file, destinationPath);
            }
        }
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
        var Watcher = new FileSystemWatcher(dir);
        Watcher.EnableRaisingEvents = true;
    }
}