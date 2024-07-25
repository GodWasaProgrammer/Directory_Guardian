using Avalonia.Media;
using System;
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
            Items = new ObservableCollection<ItemViewModel>();
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
            DiodeColor = _isMonitorActive ? Brushes.Red : Brushes.Green;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();

        public void Execute(object? parameter) => _execute();

        public event EventHandler? CanExecuteChanged;
    }
}

