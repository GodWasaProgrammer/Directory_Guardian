using Serilog;

namespace DirectoryGuardian;

public class DirGuard
{
    public DirGuard(Setup setup)
    {
        set_up = setup;
        _logger = new LoggerConfiguration()
           .WriteTo.File("logs/MainLog.txt")
           .CreateLogger();
    }

    private readonly ILogger _logger;
    public ILogger Logger { get { return _logger; } }

    private readonly Setup set_up;
    public Setup Setup { get { return set_up; } }

    private string? pathToDir;

    private List<string> Extensions = [];
    public List<string> Extensions_List { get { return Extensions; } }

    private FileSystemWatcher? _watcher;

    public void Directory_Guardian(JobType jobType)
    {
        List<string> dirPathToGuard;
        if (jobType is JobType.Initialize)
        {
            dirPathToGuard = set_up.FetchDirectoriesToSort();
            pathToDir = dirPathToGuard[0]; // assuming we only have one directory to guard
            _logger.Information($"Directory Guardian was set to work on: {pathToDir}");
            var dirinfo = Directory.GetFiles(pathToDir);
            Extensions = FetchExtensions(dirinfo);
        }

        // fetch our extensions, this will be displayed in UI to select which to sort
        if (jobType is JobType.Sort)
        {
            SortExtensionType();
        }

        if (jobType is JobType.Monitor && pathToDir is not null)
        {
            MonitorDirectory(pathToDir);
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

    private void SortExtensionType()
    {
        var listofDirs = Setup.FetchDirectoriesToSort();
        var singledir = listofDirs[0];
        var dirinfo = Directory.GetFiles(singledir);


        CreateSortingDirectory(singledir, set_up.ExtensionsToSort);
        foreach (var file in dirinfo.Where(file => set_up.ExtensionsToSort.Contains(Path.GetExtension(file)))
        // we will then sort the files of that type to the dir
        )
        {
            var destinationPath = Path.Combine(singledir, Path.GetExtension(file).Replace(".", ""), Path.GetFileName(file));
            if (!IsFileLocked(file, _logger) && !File.Exists(destinationPath))
            {
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

    public void MonitorDirectory(string dir)
    {
        _watcher?.Dispose();

        _watcher = new FileSystemWatcher(dir)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size,
            Filter = "*",
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
        SortExtensionType();
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        Logger.Information($"File: {e.FullPath} {e.ChangeType}");

        if (!set_up.ExtensionsToSort.Contains(Path.GetExtension(e.FullPath), StringComparer.OrdinalIgnoreCase))
        {
            Logger.Information($"File: {e.FullPath} is not a type to watch, ignoring.");
            return;
        }

        if (e.ChangeType == WatcherChangeTypes.Created)
        {
            HandleNewFile(e.FullPath);
        }
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        _logger.Information($"File: {e.OldFullPath} renamed to {e.FullPath}");

        if (!set_up.ExtensionsToSort.Contains(Path.GetExtension(e.FullPath), StringComparer.OrdinalIgnoreCase))
        {
            _logger.Information($"File: {e.FullPath} is not a type to watch, ignoring.");
            return;
        }

        if (!IsFileLocked(e.FullPath, _logger))
        {
            _logger.Information($"File: {e.FullPath} is not in use, can be moved.");
            Thread.Sleep(3000);
            SortExtensionType();
        }
        else
        {
            _logger.Information($"File: {e.FullPath} is in use, cannot move.");
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
        // wait a little bit
        Thread.Sleep(1000);

        if (!IsFileLocked(filePath, _logger))
        {
            // moving logic here
            _logger.Information($"File: {filePath} is not in use, can be moved.");
            // sort the files
            SortExtensionType();
        }
        else
        {
            _logger.Information($"File: {filePath} is in use, cannot move.");
        }
    }

    private static bool IsFileLocked(string filePath, ILogger logger)
    {
        FileStream? stream = null;

        try
        {
            stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            logger.Information($"File: {filePath} is not in use, can be moved");
            // Om vi kommer hit, är filen inte låst.
            return false;
        }
        catch (IOException)
        {
            // Fångar specifikt undantag för I/O-problem (t.ex. filen är låst).
            logger.Warning($"File: {filePath} is locked, cannot move.");
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            // Hanterar fall där vi inte har behörighet att komma åt filen.
            logger.Warning($"File: {filePath} Not authorized, cannot move.");
            return true;
        }
        finally
        {
            // Säkerställer att strömmen stängs om den öppnades.
            stream?.Close();
        }
    }
}