using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDM.Models
{
    public class DragDropTaskModel : INotifyPropertyChanged
    {
        #region :: Properties ::
        private bool fromLocalToCDM;
        public bool FromLocalToCDM
        {
            get
            {
                return fromLocalToCDM;
            }
            set
            {
                fromLocalToCDM = value;
                OnPropertyChanged(nameof(FromLocalToCDM));
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

        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private long totalBytes;
        public long TotalBytes
        {
            get
            {
                return totalBytes;
            }
            set
            {
                totalBytes = value;
                OnPropertyChanged(nameof(TotalBytes));
            }
        }

        private long processedBytes;
        public long ProcessedBytes
        {
            get
            {
                return processedBytes;
            }
            set
            {
                processedBytes = value;
                OnPropertyChanged(nameof(ProcessedBytes));
            }
        }

        private decimal processedPercent100;
        public decimal ProcessedPercent100
        {
            get
            {
                return processedPercent100;
            }
            set
            {
                processedPercent100 = value;
                OnPropertyChanged(nameof(ProcessedPercent100));
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        #region :: Methods ::
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
