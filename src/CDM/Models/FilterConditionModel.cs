using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDM.Models
{
    public class FilterConditionModel : INotifyPropertyChanged
    {
        public string Code { get; set; }
        public string Name { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        static FilterConditionModel()
        {
            Types = new ObservableCollection<FilterConditionModel>();
            Types.Add(new FilterConditionModel { Code = "", Name = "All types" });
            Types.Add(new FilterConditionModel { Code = "File", Name = "Files" });
            Types.Add(new FilterConditionModel { Code = "Dir", Name = "Folders" });

            GlobalLocations = new ObservableCollection<FilterConditionModel>();
            GlobalLocations.Add(new FilterConditionModel { Code = "", Name = "All drives" });

            DirveLocations = new ObservableCollection<FilterConditionModel>();
            DirveLocations.Add(new FilterConditionModel { Code = "", Name = "All drives" });
            DirveLocations.Add(new FilterConditionModel { Code = "CurDrive", Name = "This drive" });

            DirectoryLocations = new ObservableCollection<FilterConditionModel>();
            DirectoryLocations.Add(new FilterConditionModel { Code = "", Name = "All drives" });
            DirectoryLocations.Add(new FilterConditionModel { Code = "CurDrive", Name = "This drive" });
            DirectoryLocations.Add(new FilterConditionModel { Code = "CurDir", Name = "This directory" });
        }

        public static ObservableCollection<FilterConditionModel> Types { get; set; }
        public static ObservableCollection<FilterConditionModel> GlobalLocations { get; set; }
        public static ObservableCollection<FilterConditionModel> DirveLocations { get; set; }
        public static ObservableCollection<FilterConditionModel> DirectoryLocations { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
