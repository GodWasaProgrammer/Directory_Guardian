using Serilog;

namespace DirectoryGuardian;

public class Monitor
{
    private FileSystemWatcher? _watcher;
    private readonly ILogger _Logger;
    private readonly DirGuard Guard;
    public bool IsActive { get; private set; }

    // private field for logic in the delegates for the watcher
    private JobType _Monitor_Job;
    public Monitor(ILogger logger, JobType jobType, DirGuard Guard)
    {
        _watcher = new FileSystemWatcher();
        _Logger = logger;
        _Monitor_Job = jobType;
        this.Guard = Guard;
    }

    public void MonitorDirectory(string dir, JobType jobType)
    {
        _watcher?.Dispose();

        _Monitor_Job = jobType;
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

        // Do the initial sorting
        if (jobType is JobType.MonitorByType)
        {
            Guard.Sort_By_Type();
        }
        if (jobType is JobType.MonitorByExtension)
        {
            Guard.Sort_By_Extension();
        }
        IsActive = true;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        _Logger.Information($"File: {e.FullPath} {e.ChangeType}");

        if (_Monitor_Job is JobType.MonitorByExtension)
        {
            if (!Guard.Setup.ExtensionsToSort.Contains(Path.GetExtension(e.FullPath), StringComparer.OrdinalIgnoreCase))
            {
                _Logger.Information($"File: {e.FullPath} is not a type to watch, ignoring.");
                return;
            }

            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                HandleNewFile(e.FullPath);
            }
        }

        if (_Monitor_Job == JobType.MonitorByType)
        {
            bool matched = false;

            foreach (var type in Guard.Setup.TypesToSort)
            {
                // Fetch the extensions related to the type
                if (TypeLists.ExtensionsMap.TryGetValue(type, out var extensions))
                    continue;
                // Check if the file extension matches any in the list
                if (extensions.Any(extension => extension.Equals(Path.GetExtension(e.FullPath), StringComparison.OrdinalIgnoreCase)))
                {
                    if (e.ChangeType == WatcherChangeTypes.Created)
                    {
                        HandleNewFile(e.FullPath);
                    }
                    matched = true; // Mark as matched
                }
            }

            // If no match was found, log a message
            if (!matched)
            {
                _Logger.Information($"File: {e.FullPath} is not a type to watch, ignoring.");
            }
        }
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        _Logger.Information($"File: {e.OldFullPath} renamed to {e.FullPath}");

        if (_Monitor_Job is JobType.MonitorByExtension)
        {
            if (!Guard.Setup.ExtensionsToSort.Contains(Path.GetExtension(e.FullPath), StringComparer.OrdinalIgnoreCase))
            {
                _Logger.Information($"File: {e.FullPath} is not a type to watch, ignoring.");
                return;
            }

            if (!IsFileLocked(e.FullPath, _Logger))
            {
                _Logger.Information($"File: {e.FullPath} is not in use, can be moved.");
                Thread.Sleep(3000);
                Guard.Sort_By_Extension();
            }
            else
            {
                _Logger.Information($"File: {e.FullPath} is in use, cannot move.");
            }
        }
        if (_Monitor_Job == JobType.MonitorByType)
        {
            bool matched = false;

            if (Guard.Setup.TypesToSort is not null)
            {
                foreach (var type in Guard.Setup.TypesToSort)
                {
                    // Fetch the extensions related to the type
                    if (!TypeLists.ExtensionsMap.TryGetValue(type, out var extensions))
                        continue;

                    // Check if the file extension matches any in the list
                    if (extensions.Any(extension => extension.Equals(Path.GetExtension(e.FullPath), StringComparison.OrdinalIgnoreCase)))
                    {
                        if (e.ChangeType == WatcherChangeTypes.Created && !IsFileLocked(e.FullPath, _Logger))
                        {
                            _Logger.Information($"File: {e.FullPath} is not in use, can be moved.");
                            Thread.Sleep(3000);
                            Guard.Sort_By_Type();
                        }
                        matched = true; // Mark as matched
                    }
                }
            }

            // If no match was found, log a message
            if (!matched)
            {
                _Logger.Information($"File: {e.FullPath} is not a type to watch, ignoring.");
            }
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
        IsActive = false;
    }

    private void HandleNewFile(string filePath)
    {
        // wait a little bit
        Thread.Sleep(1000);

        if (!IsFileLocked(filePath, _Logger))
        {
            // moving logic here
            _Logger.Information($"File: {filePath} is not in use, can be moved.");
            // sort the files
            if (_Monitor_Job is JobType.MonitorByExtension)
            {
                Guard.Sort_By_Extension();
            }
            if (_Monitor_Job is JobType.MonitorByType)
            {
                Guard.Sort_By_Type();
            }
        }
        else
        {
            _Logger.Information($"File: {filePath} is in use, cannot move.");
        }
    }

    internal static bool IsFileLocked(string filePath, ILogger logger)
    {
        FileStream? stream = null;

        try
        {
            stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            logger.Information($"File: {filePath} is not in use, can be moved");
            return false;
        }
        catch (IOException)
        {
            logger.Warning($"File: {filePath} is locked, cannot move.");
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            logger.Warning($"File: {filePath} Not authorized, cannot move.");
            return true;
        }
        finally
        {
            stream?.Close();
        }
    }
}
