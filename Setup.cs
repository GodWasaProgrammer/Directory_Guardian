namespace DirectoryGuardian;

public class Setup
{
    readonly List<string> DirectoriesToWatch = [];
    readonly List<string> DirToSort = [];
    private readonly List<string> _extensionsToSort = [];
    public List<string> ExtensionsToSort { get { return _extensionsToSort; } }

    private List<SortTypes>? _TypesToSort;
    public List<SortTypes>? TypesToSort { get { return _TypesToSort; } set { _TypesToSort = value; } }


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
