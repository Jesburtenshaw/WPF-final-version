using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDM.Models
{
    public class SearchStatusModel : INotifyPropertyChanged
    {
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string desc;
        public string Desc
        {
            get
            {
                return desc;
            }
            set
            {
                desc = value;
                OnPropertyChanged(nameof(Desc));
            }
        }

        private bool isError;
        public bool IsError
        {
            get
            {
                return isError;
            }
            set
            {
                isError = value;
                OnPropertyChanged(nameof(IsError));
            }
        }

        private bool searched;
        public bool Searched
        {
            get
            {
                return searched;
            }
            set
            {
                searched = value;
                OnPropertyChanged(nameof(Searched));
            }
        }

        private bool canSearch;
        public bool CanSearch
        {
            get
            {
                return canSearch;
            }
            set
            {
                canSearch = value;
                OnPropertyChanged(nameof(CanSearch));
            }
        }

        private bool isDoing;
        public bool IsDoing
        {
            get
            {
                return isDoing;
            }
            set
            {
                isDoing = value;
                OnPropertyChanged(nameof(IsDoing));
            }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private bool isLoadingDrives;
        public bool IsLoadingDrives
        {
            get
            {
                return isLoadingDrives;
            }
            set
            {
                isLoadingDrives = value;
                OnPropertyChanged(nameof(IsLoadingDrives));
            }
        }

        private bool isLoadingPinned;
        public bool IsLoadingPinned
        {
            get
            {
                return isLoadingPinned;
            }
            set
            {
                isLoadingPinned = value;
                OnPropertyChanged(nameof(IsLoadingPinned));
            }
        }

        private bool isLoadingRecent;
        public bool IsLoadingRecent
        {
            get
            {
                return isLoadingRecent;
            }
            set
            {
                isLoadingRecent = value;
                OnPropertyChanged(nameof(IsLoadingRecent));
            }
        }

        private bool isLoadingItems;
        public bool IsLoadingItems
        {
            get
            {
                return isLoadingItems;
            }
            set
            {
                isLoadingItems = value;
                OnPropertyChanged(nameof(IsLoadingItems));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
