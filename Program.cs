namespace DirectoryGuardian
{
    internal class Program
    {
        static void Main()
        {
            // We will guard a directory, sort files after file extensions or other variables such as name, size
            var dirPathToGuard = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
            var dirinfo = Directory.GetFiles(dirPathToGuard);

            // read our extensions
            var listOfExtensions = new List<string>();
            foreach (var file in dirinfo)
            {
                // we should only add the extension if its not already in the list
                if (!listOfExtensions.Contains(Path.GetExtension(file)))
                {
                    listOfExtensions.Add(Path.GetExtension(file));
                }
            }

            var listOfFileTypesToSort = new List<string>();
            listOfFileTypesToSort.Add(".jpg");
            // create our directories
            foreach (var file in listOfFileTypesToSort)
            {
                var dir = Path.Combine(dirPathToGuard, file).Replace(".", "");
                Directory.CreateDirectory(dir);
            }

            // we will then sort the files of that type to the dir

            foreach (var file in dirinfo)
            {
                if (listOfFileTypesToSort.Contains(Path.GetExtension(file)))
                {
                    var destinationPath = Path.Combine(dirPathToGuard, Path.GetExtension(file).Replace(".", ""), Path.GetFileName(file));
                    File.Move(file, destinationPath);
                }
            }
        }
    }
}