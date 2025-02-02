﻿using Serilog;

namespace DirectoryGuardian;

public class DirGuard
{
    public DirGuard(Setup setup)
    {
        _setup = setup;
        _logger = new LoggerConfiguration()
           .WriteTo.File("logs/MainLog.txt")
           .CreateLogger();
        _Monitor = new Monitor(Logger, JobType.Initialize, this);
        _resetChanges = new ResetManager(this, _logger);
    }

    private readonly ILogger _logger;
    private readonly ResetManager _resetChanges;
    public ILogger Logger { get { return _logger; } }

    private readonly Setup _setup;
    public Setup Setup { get { return _setup; } }

    private string? pathToDir;

    private List<string> Extensions = [];
    public List<string> Extensions_List { get { return Extensions; } }

    private Monitor _Monitor;
    public Monitor Monitor { get { return _Monitor; } }

    public void StartGuardian()
    {
        List<string> dirPathToGuard;
        dirPathToGuard = _setup.FetchDirectoriesToSort();
        pathToDir = dirPathToGuard[0]; // assuming we only have one directory to guard
        _logger.Information($"Directory Guardian was set to work on: {pathToDir}");
        var dirinfo = Directory.GetFiles(pathToDir);
        Extensions = FetchExtensions(dirinfo);

        if (pathToDir is null)
        {
            _logger.Error("No directory to guard was found. Please ensure the path is correct and try again.");
        }
    }

    public void StopMonitor()
    {
        _logger.Information($"Stopping Monitoring on:{pathToDir}");
        _Monitor?.StopMonitoring();
    }

    public void MonitorByExtension(JobType jobType)
    {
        _logger.Information($"Monitoring Extension on:{pathToDir}");
        _Monitor = new Monitor(_logger, jobType, this);
        _Monitor.MonitorDirectory(pathToDir, jobType);
    }

    public void MonitorByType(JobType jobType)
    {
        _logger.Information($"Monitoring Type on:{pathToDir}");
        _Monitor = new Monitor(_logger, jobType, this);
        _Monitor.MonitorDirectory(pathToDir, jobType);
    }

    public static void Main()
    {
        // for compiler
    }

    public void Sort_By_Type()
    {
        _logger.Information($"Sorting types on {pathToDir}");
        if (pathToDir is null)
        {
            _logger.Error("No directory to guard was found. Please ensure the path is correct and try again.");
            return;
        }
        var files = Directory.GetFiles(pathToDir);

        if (_setup.TypesToSort is null)
        {
            _logger.Error("No types to sort were provided. Please ensure the list is not empty and try again.");
            return;
        }
        foreach (var type in _setup.TypesToSort)
        {
            if (!TypeLists.ExtensionsMap.ContainsKey(type))
                continue;

            var extensions = TypeLists.ExtensionsMap[type];
            var targetDirectory = type.ToString();

            var fullPath = Path.Combine(pathToDir, targetDirectory);

            Directory.CreateDirectory(fullPath);

            foreach (var file in files)
            {
                if (extensions.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    Record record = new Record();
                    record.OGpath = file;
                    record.NewPath = Path.Combine(fullPath, Path.GetFileName(file));
                    record.TimeOfChange = DateTime.Now;
                    _resetChanges.RecordChange(record);
                    File.Move(file, Path.Combine(fullPath, Path.GetFileName(file)));
                }
            }
        }
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

    public void Sort_By_Extension()
    {
        _logger.Information($"Sorting extensions on {pathToDir}");
        var listofDirs = Setup.FetchDirectoriesToSort();
        var singledir = listofDirs[0];
        var dirinfo = Directory.GetFiles(singledir);

        CreateSortingDirectory(singledir, _setup.ExtensionsToSort);
        foreach (var file in dirinfo.Where(file => _setup.ExtensionsToSort.Contains(Path.GetExtension(file)))
        // we will then sort the files of that type to the dir
        )
        {
            var destinationPath = Path.Combine(singledir, Path.GetExtension(file).Replace(".", ""), Path.GetFileName(file));
            if (!Monitor.IsFileLocked(file, _logger) && !File.Exists(destinationPath))
            {
                Record record = new Record();
                record.OGpath = file;
                record.NewPath = destinationPath;
                record.TimeOfChange = DateTime.Now;
                _resetChanges.RecordChange(record);
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
}