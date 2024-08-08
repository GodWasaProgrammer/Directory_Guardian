using Avalonia.Media;
using DirectoryGuardian.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace UI.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private ObservableCollection<ItemViewModel>? _items;
    private IBrush? _diodeColor;
    private bool _isMonitorActive;

    private string _chosenFolder;
    public string ChosenFolder
    {
        get
        {
            if (_chosenFolder != null)
            {
                return _chosenFolder;
            }
            else
            {
                return "No Folder Selected";
            }
        }
        set
        {
            _chosenFolder = value;
            OnPropertyChanged(nameof(ChosenFolder));
        }
    }


    public ObservableCollection<ItemViewModel>? Items
    {
        get => _items;
        set
        {
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    public IBrush? DiodeColor
    {
        get => _diodeColor;
        set
        {
            _diodeColor = value;
            OnPropertyChanged(nameof(DiodeColor));
        }
    }

    public ICommand ToggleMonitorCommand { get; }
    public SortTypeViewModel SortTypeViewModelInstance { get; }

    public MainViewModel()
    {
        Items = [];
        DiodeColor = Brushes.Red;
        _isMonitorActive = false;
        SortTypeViewModelInstance = new SortTypeViewModel();
        _chosenFolder = "No Folder Selected";

        ToggleMonitorCommand = new RelayCommand(ToggleMonitor);
#if DEBUG
        ItemViewModel itemViewModel_For_Design1 = new()
        {
            Text = "ttf"
        };
        Items.Add(itemViewModel_For_Design1);
        ItemViewModel itemViewModel_For_Design2 = new()
        {
            Text = "pdf"
        };
        Items.Add(itemViewModel_For_Design2);
#endif
    }
    public List<string>? GetSelectedItems()
    {
        return Items?.Where(item => item.IsChecked).Select(item => item.Text!).ToList();
    }

    private void ToggleMonitor()
    {
        _isMonitorActive = !_isMonitorActive;
        DiodeColor = _isMonitorActive ? Brushes.Green : Brushes.Red;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}