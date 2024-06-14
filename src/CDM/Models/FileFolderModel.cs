using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CDM.Models
{
    public class FileFolderModel : INotifyPropertyChanged
    {
        #region :: Properties ::
        public string Path { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public BitmapSource IconSource { get; set; }
        public string OriginalPath { get; set; }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string type;
        /// <summary>
        /// Dir,File
        /// </summary>
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        private bool isPined;
        public bool IsPined
        {
            get
            {
                return isPined;
            }
            set
            {
                isPined = value;
                OnPropertyChanged(nameof(IsPined));
            }
        }

        private bool isDefault;
        public bool IsDefault
        {
            get
            {
                return isDefault;
            }
            set
            {
                isDefault = value;
                OnPropertyChanged(nameof(isDefault));
            }
        }

        private bool _isDrive = false;
        public bool IsDrive
        {
            get
            {
                return _isDrive;
            }
            set
            {
                _isDrive = value;
                OnPropertyChanged(nameof(IsDrive));
            }
        }

        #endregion
        #region :: Event Handler ::
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// This method will execute when poperty change
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
