namespace DirectoryGuardian;

public class Setup
{
    readonly List<string> DirectoriesToWatch = new List<string>();
    readonly List<string> DirToSort = new List<string>();
    private List<string> _extensionsToSort = [];
    public List<string> extensionsToSort { get { return _extensionsToSort; } }
    public void AddDirectoryToWatch(string directory)
    {
        DirectoriesToWatch.Add(directory);
    }

    public void AddDirectoryToSort(string directory)
    {
        DirToSort.Add(directory);
    }

    public void ClearLists()
    {
        DirectoriesToWatch.Clear();
        DirToSort.Clear();
    }

    public List<string> FetchDirectoriesToSort() { return DirToSort; }

    public void AddExtensionToSort(string extension)
    {
        _extensionsToSort.Add(extension);
    }

}
