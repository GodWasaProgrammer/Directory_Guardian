using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace DirectoryGuardian.ViewModels
{
    public class SortTypeViewModel : ReactiveObject
    {
        private ObservableCollection<SortTypes> _sortTypes;

        public SortTypeViewModel()
        {
            SortTypes = new ObservableCollection<SortTypes>((SortTypes[])Enum.GetValues(typeof(SortTypes)));
        }

        public ObservableCollection<SortTypes> SortTypes
        {
            get => _sortTypes;
            set => this.RaiseAndSetIfChanged(ref _sortTypes, value);
        }
    }
}