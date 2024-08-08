using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using DirectoryGuardian;
using DirectoryGuardian.ViewModels;
using System.Collections.Generic;
using System.Linq;
using UI.ViewModels;
namespace UI;

public partial class MainWindow : Window
{
    private readonly MainViewModel viewModel;
    private readonly SortTypeViewModel sortTypeViewModel;
    private DirGuard? dirGuard;

    public MainWindow()
    {
        InitializeComponent();
        viewModel = new MainViewModel();
        sortTypeViewModel = viewModel.SortTypeViewModelInstance;
        DataContext = viewModel;
        Width = 800; Height = 450;
    }

    private async void OpenFolderButton_Clicked(object sender, RoutedEventArgs args)
    {
        // Get top level from the current control.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.

        var setup = new Setup();
        if (topLevel is not null)
        {

            var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Open Folder",
                AllowMultiple = false
            });

            if (folders.Count >= 1)
            {
                // Set the variable with the path to the selected folder.
                var selectedFolderPath = folders[0].Path;
                // You can now use selectedFolderPath as needed

                setup.AddDirectoryToSort(selectedFolderPath.LocalPath);
                viewModel.ChosenFolder = selectedFolderPath.LocalPath;
            }
        }
        dirGuard = new DirGuard(setup);
        dirGuard.Directory_Guardian(JobType.Initialize);
        DisplayExtensions(dirGuard.Extensions_List);

    }

    private void SortTypes(object sender, RoutedEventArgs e)
    {
        var chosenTypes = sortTypeViewModel.GetSelectedSortTypes();

        if (dirGuard is not null)
        {
            if (chosenTypes is null)
            {
                dirGuard.Logger.Error("No sort type selected");
                return;
            }
            dirGuard.Setup.TypesToSort = chosenTypes;
            dirGuard.Directory_Guardian(JobType.SortByType);
        }
    }

    private void SortExtensions(object sender, RoutedEventArgs e)
    {
        Pass_Chosen_Extensions_ToSort_On_Click();
        dirGuard?.Directory_Guardian(JobType.SortByExtension);
        var selectedItems = viewModel.GetSelectedItems();

        if (selectedItems is null || viewModel.Items is null) return;

        foreach (var item in selectedItems)
        {
            var itemsToRemove = viewModel.Items.Where(i => i.Text == item).ToList();
            foreach (var itemToRemove in itemsToRemove)
            {
                viewModel.Items.Remove(itemToRemove);
            }
        }
    }

    private void DisplayExtensions(List<string> extensions)
    {
        viewModel?.Items?.Clear();

        foreach (var extension in extensions)
        {
            viewModel?.Items?.Add(new ItemViewModel { Text = extension });
        }
    }

    private void Pass_Chosen_Extensions_ToSort_On_Click()
    {
        var selectedItems = viewModel.GetSelectedItems();
        if (selectedItems is not null)
        {
            foreach (var item in selectedItems)
            {
                dirGuard?.Setup.AddExtensionToSort(item.ToString());
            }
        }
    }

    private void Monitor_By_Ext(object sender, RoutedEventArgs e)
    {
        Pass_Chosen_Extensions_ToSort_On_Click();
        dirGuard?.Directory_Guardian(JobType.MonitorByExtension);
        viewModel.ToggleMonitorCommand.Execute(null);
    }

    private void Monitor_By_Type(object sender, RoutedEventArgs e)
    {
        if (dirGuard is not null)
        {
            dirGuard.Setup.TypesToSort = sortTypeViewModel.GetSelectedSortTypes();
            dirGuard.Directory_Guardian(JobType.MonitorByType);
        }
    }
}