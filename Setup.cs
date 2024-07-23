namespace DirectoryGuardian;

public class Setup
{
    readonly List<string> DirectoriesToWatch = new List<string>();
    readonly List<string> DirToSort = new List<string>();

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

}
