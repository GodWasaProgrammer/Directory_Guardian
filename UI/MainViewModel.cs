using Avalonia.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ItemViewModel>? _items;
        private IBrush _diodeColor;
        private bool _isMonitorActive;

        public ObservableCollection<ItemViewModel>? Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public IBrush DiodeColor
        {
            get => _diodeColor;
            set
            {
                _diodeColor = value;
                OnPropertyChanged(nameof(DiodeColor));
            }
        }

        public ICommand ToggleMonitorCommand { get; }

        public MainViewModel()
        {
            Items = [];
            DiodeColor = Brushes.Red;
            _isMonitorActive = false;

            ToggleMonitorCommand = new RelayCommand(ToggleMonitor);
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
}

