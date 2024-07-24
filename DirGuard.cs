namespace DirectoryGuardian;

public class DirGuard(Setup setup)
{
    private readonly Setup set_up = setup;
    public Setup Setup { get { return set_up; } }

    private List<string> Extensions = [];
    public List<string> Extensions_List { get { return Extensions; } }

    private FileSystemWatcher? _watcher;

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

        if (jobType is JobType.Monitor)
        {
            // monitor our directory for changes and apply our sorting logic
            MonitorDirectory(dirPathToGuard[0]);
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

        //foreach (var extension in listOfFileTypesToSort)
        //{
        //    Extensions.Remove(extension);
        //}
        //set_up.ExtensionsToSort.Clear();
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

    public void MonitorDirectory(string dir)
    {
        if (_watcher != null)
        {
            _watcher.Dispose();
        }

        _watcher = new FileSystemWatcher(dir)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size,
            Filter = "*.*",
            EnableRaisingEvents = true
        };

        _watcher.Created += OnChanged;
        _watcher.Changed += OnChanged;
        _watcher.Deleted += OnChanged;
        _watcher.Renamed += OnRenamed;

        //unregister the watcher
        _watcher.Disposed += (sender, e) =>
        {
            _watcher.Created -= OnChanged;
            _watcher.Changed -= OnChanged;
            _watcher.Deleted -= OnChanged;
            _watcher.Renamed -= OnRenamed;
        };

        // do the initial sorting
        var dirinfo = Directory.GetFiles(dir);
        SortExtensionType(dir, dirinfo, set_up.ExtensionsToSort);
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        // Logga eller hantera filändringar
        Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");

        if (!set_up.ExtensionsToSort.Contains(Path.GetExtension(e.FullPath), StringComparer.OrdinalIgnoreCase))
        {
            Console.WriteLine($"File: {e.FullPath} is not a type to watch, ignoring.");
            return;
        }

        // Ignorera temporära filer
        if (Path.GetExtension(e.FullPath).Equals(".tmp", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"File: {e.FullPath} is a temporary file, ignoring.");
            return;
        }

        if (e.ChangeType == WatcherChangeTypes.Created)
        {
            HandleNewFile(e.FullPath);
        }
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");

        if (!set_up.ExtensionsToSort.Contains(Path.GetExtension(e.FullPath), StringComparer.OrdinalIgnoreCase))
        {
            Console.WriteLine($"File: {e.FullPath} is not a type to watch, ignoring.");
            return;
        }

        // Ignorera temporära filer
        if (Path.GetExtension(e.FullPath).Equals(".tmp", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"File: {e.FullPath} is a temporary file, ignoring.");
            return;
        }

        if (!IsFileLocked(e.FullPath))
        {
            // moving logic here
            Console.WriteLine($"File: {e.FullPath} is not in use, can be moved.");
            // sort the files
            var listofDirs = Setup.FetchDirectoriesToSort();
            var singledir = listofDirs[0];
            var dirinfo = Directory.GetFiles(singledir);
            SortExtensionType(singledir, dirinfo, set_up.ExtensionsToSort);
        }
        else
        {
            Console.WriteLine($"File: {e.FullPath} is in use, cannot move.");
        }
    }

    public void StopMonitoring()
    {
        if (_watcher != null)
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
            _watcher = null;
        }
    }

    private void HandleNewFile(string filePath)
    {
        // this is probably not a great solution :D
        // wait a little bit
        Thread.Sleep(1000);

        if (!IsFileLocked(filePath))
        {
            // moving logic here
            Console.WriteLine($"File: {filePath} is not in use, can be moved.");
            // sort the files
            var listofDirs = Setup.FetchDirectoriesToSort();
            var singledir = listofDirs[0];
            var dirinfo = Directory.GetFiles(filePath);
            SortExtensionType(singledir, dirinfo, set_up.ExtensionsToSort);
        }
        else
        {
            Console.WriteLine($"File: {filePath} is in use, cannot move.");
        }
    }

    private bool IsFileLocked(string filePath)
    {
        try
        {
            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                return false;
            }
        }
        catch (IOException)
        {
            return true;
        }
    }
}