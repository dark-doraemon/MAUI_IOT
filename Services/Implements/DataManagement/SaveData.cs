using MAUI_IOT.Models.Data;
using MAUI_IOT.Services.Interfaces.SaveData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MAUI_IOT.Services.Implements.SaveData
{
    public class SaveData : ISaveData
    {
        public async Task SaveDataFile(string filename, double m, ObservableCollection<Data> data)
        {
            FileSave fileSave = new FileSave();
            fileSave.m = m;
            fileSave.datafile = data;
            string fileName = $"{filename}.json";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            string jsonData = System.Text.Json.JsonSerializer.Serialize<FileSave>(fileSave);
            await File.WriteAllTextAsync(filePath, jsonData);

        }

    }
}
