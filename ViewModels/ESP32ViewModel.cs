using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using MAUI_IOT.Hubs;
using MAUI_IOT.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.ViewModels
{
    public partial class ESP32ViewModel : ObservableObject
    {

        // Use ObservableCollections to let the chart listen for changes (or any INotifyCollectionChanged). 
        private readonly ObservableCollection<ObservableValue> _observableValues;
        public ObservableCollection<ISeries> Series { get; set; }
        ESP32Sensor ESP32Sensor { get; set; }

        private ESP32Model eSP32Model;
        public ESP32Model ESP32Model
        {
            get => eSP32Model;
            set
            {
                if (eSP32Model != value)
                {
                    eSP32Model = value;
                    OnPropertyChanged(nameof(ESP32Model));
                }
            }
        }
        public ESP32ViewModel()
        {
            _observableValues = new ObservableCollection<ObservableValue>
            {
                //example data
                new ObservableValue(2),
                new(5),
                new(4),
                new(5),
                new(2),
                new(6),
                new(6),
                new(6),
                new(4),
                new(2),
                new(3),
                new(4),
                new(3)
            };

            Series = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = _observableValues,
                    Fill = null
                }
            };
            eSP32Model = new ESP32Model();

            ESP32Sensor = new ESP32Sensor();

            ESP32Sensor.PropertyChanged += ESP32Sensor_PropertyChanged;

           
        }

        public async Task Connect()
        {
            await ESP32Sensor.ConnectAsync(new Uri("ws://192.168.1.125:1880/test2"));
        }

        private void ESP32Sensor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //kiểm tra xem tên thuộc tính có phải là ReceivedData không vì ta đã set thuộc tính PropertyName là ReceivedData ở hàm Invoke
            if (e.PropertyName == nameof(ESP32Sensor.ReceivedData))
            {
                ESP32Model = JsonConvert.DeserializeObject<ESP32Model>(ESP32Sensor.ReceivedData);
                AddItem(ESP32Model.Temperature);
                RemoveItem();
            }
        }

        //[RelayCommand] //automatically create ICommand
        public void AddItem(float temperature)
        {
            _observableValues.Add(new ObservableValue(temperature));
        }

        //[RelayCommand]
        public void RemoveItem()
        {
            if (_observableValues.Count == 0) return;
            _observableValues.RemoveAt(0);
        }

    }
}
