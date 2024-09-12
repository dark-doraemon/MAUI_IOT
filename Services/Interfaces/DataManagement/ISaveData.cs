using MAUI_IOT.Models.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Services.Interfaces.SaveData
{
    public interface ISaveData
    {
        public Task SaveDataFile(string filename, double m, ObservableCollection<Data> data);
    }
}
