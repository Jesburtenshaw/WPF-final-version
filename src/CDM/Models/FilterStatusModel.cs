using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDM.Models
{
    public class FilterStatusModel : INotifyPropertyChanged
    {
        #region :: Properties ::
        private bool showDrives;
        public bool ShowDrives
        {
            get
            {
                return showDrives;
            }
            set
            {
                showDrives = value;
                OnPropertyChanged(nameof(ShowDrives));
            }
        }

        private bool showTypes;
        public bool ShowTypes
        {
            get
            {
                return showTypes;
            }
            set
            {
                showTypes = value;
                OnPropertyChanged(nameof(ShowTypes));
            }
        }

        private bool showLocations;
        public bool ShowLocations
        {
            get
            {
                return showLocations;
            }
            set
            {
                showLocations = value;
                OnPropertyChanged(nameof(ShowLocations));
            }
        }

        private string curDrives;
        public string CurDrives
        {
            get
            {
                if (string.IsNullOrEmpty(curDrives))
                {
                    curDrives = "Drive";
                }
                return curDrives;
            }
            set
            {
                curDrives = value;
                OnPropertyChanged(nameof(CurDrives));
            }
        }

        private string curType;
        public string CurType
        {
            get
            {
                if (string.IsNullOrEmpty(curType))
                {
                    curType = "Type";
                }
                return curType;
            }
            set
            {
                curType = value;
                OnPropertyChanged(nameof(CurType));
            }
        }

        private bool isFiltering;
        public bool IsFiltering
        {
            get
            {
                return isFiltering;
            }
            set
            {
                isFiltering = value;
                OnPropertyChanged(nameof(IsFiltering));
            }
        }

        private int recentCount;
        public int RecentCount
        {
            get
            {
                return recentCount;
            }
            set
            {
                recentCount = value;
                OnPropertyChanged(nameof(RecentCount));
            }
        }

        private int pinnedCount;
        public int PinnedCount
        {
            get
            {
                return pinnedCount;
            }
            set
            {
                pinnedCount = value;
                OnPropertyChanged(nameof(PinnedCount));
            }
        }

        private int itemsCount;
        public int ItemsCount
        {
            get
            {
                return itemsCount;
            }
            set
            {
                itemsCount = value;
                OnPropertyChanged(nameof(ItemsCount));
            }
        }

        private int drivesCount;
        public int DrivesCount
        {
            get
            {
                return drivesCount;
            }
            set
            {
                drivesCount = value;
                OnPropertyChanged(nameof(DrivesCount));
            }
        }
        #endregion
        #region :: Event Handler ::
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// This method will execute when property changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
