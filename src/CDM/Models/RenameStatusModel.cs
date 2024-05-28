using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDM.Models
{
    public class RenameStatusModel : INotifyPropertyChanged
    {
        #region :: Properties ::
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
