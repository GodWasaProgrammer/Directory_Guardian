using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DirectoryGuardian.ViewModels
{
    public class SortTypeViewModel : ReactiveObject
    {
        private ObservableCollection<SortTypeModel>? _sortTypes = new();

        public SortTypeViewModel()
        {
            SortTypes = [];
            foreach (SortTypes type in Enum.GetValues(typeof(SortTypes)))
            {
                SortTypes.Add(new SortTypeModel(type, false));
            }
        }

        public List<SortTypes> GetSelectedSortTypes()
        {
            if (_sortTypes is null)
            {
                // this should never happen
                return new();
            }
            var fetchedList = new ObservableCollection<SortTypeModel>(_sortTypes.Where(st => st.IsChecked));

            var ListToSort = new List<SortTypes>();
            foreach (var type in fetchedList)
            {
                ListToSort.Add(type.SortType);
            }

            return ListToSort;
        }

        public ObservableCollection<SortTypeModel> SortTypes
        {
            get
            {
                if (_sortTypes is null)
                {
                    return new();
                }
                return _sortTypes;
            }
            set => this.RaiseAndSetIfChanged(ref _sortTypes, value);
        }
    }
}