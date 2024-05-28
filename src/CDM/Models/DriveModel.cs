using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDM.Models
{
    public class DriveModel : INotifyPropertyChanged
    {
        #region :: Properties ::
        public string DriveName { get; set; }
        public string DriveDescription { get; set; }
        public string Type { get; set; } = "Drive";

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
        #endregion
        #region :: Event Handler ::
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// This method will execute when property change
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
