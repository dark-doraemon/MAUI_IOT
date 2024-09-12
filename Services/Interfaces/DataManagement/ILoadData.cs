using MAUI_IOT.Models.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Services.Interfaces.DataManagement
{
    public interface ILoadData
    {
        public Task Load(string fileName, double m, ObservableCollection<Data> data);

    }
}