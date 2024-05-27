using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDM.Models
{
    internal class CDMModel
    {
        public string DriveName { get; set; }
        public string DriveDescription { get; set; }

        public string RecentFileName { get; set; }
        public string RecentFileDate { get; set; }
    }
}
