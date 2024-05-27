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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
