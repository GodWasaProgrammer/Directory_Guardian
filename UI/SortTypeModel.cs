using System.ComponentModel;

namespace DirectoryGuardian.ViewModels
{
    public class SortTypeModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private SortTypes _sortType;
        public SortTypes SortType
        {
            get => _sortType;
            set
            {
                _sortType = value;
                OnPropertyChanged(nameof(SortType));
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        public SortTypeModel(SortTypes type, bool isChecked)
        {
            SortType = type;
            IsChecked = isChecked;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}