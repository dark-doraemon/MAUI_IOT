using LiveChartsCore.Defaults;
using MAUI_IOT.Models.Data;
using MAUI_IOT.Services.Interfaces.DataManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Services.Implements.DataManagement
{
    public class LoadData : ILoadData
    {
        public async Task Load(string fileName, double m, ObservableCollection<Data> data)
        {

            FileSave fileSave = new FileSave();
            var filePath = Path.Combine(FileSystem.AppDataDirectory, $"{fileName}.json");
            if (File.Exists(filePath))
            {
                var jsonData = await File.ReadAllTextAsync(filePath);
                FileSave file = System.Text.Json.JsonSerializer.Deserialize<FileSave>(jsonData);
                m = file.m;
                data = file.datafile;
                Debug.WriteLine($"========giá trị của  M :{m} ");
            }
            else
            {
                //gui thong baio cho view la khong co du lieu 
                m = 0;


            }

        }

    }
}
