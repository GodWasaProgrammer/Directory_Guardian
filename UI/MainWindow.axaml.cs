using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using DirectoryGuardian;
using System.Collections.Generic;
using UI.ViewModels;
namespace UI;

public partial class MainWindow : Window
{
    private MainViewModel viewModel;
    private DirGuard dirGuard;
    public MainWindow()
    {
        InitializeComponent();
        viewModel = new MainViewModel();
        DataContext = viewModel;
        Width = 800; Height = 450;
    }

    private async void OpenFolderButton_Clicked(object sender, RoutedEventArgs args)
    {
        // Get top level from the current control.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Open Folder",
            AllowMultiple = false
        });

        var setup = new Setup();
        if (folders.Count >= 1)
        {
            // Set the variable with the path to the selected folder.
            var selectedFolderPath = folders[0].Path;
            // You can now use selectedFolderPath as needed

            setup.AddDirectoryToSort(selectedFolderPath.LocalPath);
        }
        dirGuard = new DirGuard(setup);
        dirGuard.Directory_Guardian();
        DisplayExtensions(dirGuard.Extensions_List);
    }

    private void DisplayExtensions(List<string> extensions)
    {
        viewModel.Items.Clear();

        foreach (var extension in extensions)
        {
            viewModel.Items.Add(new ItemViewModel { Text = extension });
        }
    }

    private void PassChosenFiletypeToSortOnClick(object sender, RoutedEventArgs e)
    {
        var selectedItems = viewModel.GetSelectedItems();
        foreach (var item in selectedItems)
        {
            dirGuard.Setup.AddExtensionToSort(item.ToString());
        }
    }
}