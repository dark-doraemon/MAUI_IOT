using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Models.Data
{
    public class FileSave
    {
        public double m { get; set; }
        public ObservableCollection<Data> datafile { get; set; }

    }
}
