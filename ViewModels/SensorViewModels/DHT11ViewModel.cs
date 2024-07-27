using __XamlGeneratedCode__;
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

namespace MAUI_IOT.ViewModels.SensorViewModels
{
    public partial class DHT11ViewModel : ObservableObject
    {

        // Use ObservableCollections to let the chart listen for changes (or any INotifyCollectionChanged). 
        private readonly ObservableCollection<ObservableValue> _observableTemperature;
        private readonly ObservableCollection<ObservableValue> _observableHumidity;
        public ObservableCollection<ISeries> Series { get; set; }
        ESP32Sensor ESP32Sensor { get; set; }

        private Temp _temp;
        public Temp Temp
        {
            get => _temp;
            set
            {
                if (_temp != value)
                {
                    _temp = value;
                    OnPropertyChanged(nameof(Temp));
                }
            }
        }
        public DHT11ViewModel()
        {
            _observableTemperature = new ObservableCollection<ObservableValue>
            {
                //example data
                new ObservableValue(5),
                new(10),
                new(15),
                new(20),
                new(25),
                new(30),
                new(31),
                new(32),
                new(33),
                new(34),
                new(35),
                new(36),
                new(37)
            };

            _observableHumidity = new ObservableCollection<ObservableValue>
            {
                new ObservableValue(5),
                new(10),
                new(15),
                new(20),
                new(25),
                new(30),
                new(31),
                new(32),
                new(33),
                new(34),
                new(35),
                new(36),
                new(37)
            };

            Series = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = _observableTemperature,
                    Fill = null,
                    LineSmoothness = 0,

                },
                new LineSeries<ObservableValue>
                {
                    Values = _observableHumidity,
                    Fill = null,
                    LineSmoothness = 1,
                }
                
            };

            _temp = new Temp();

            ESP32Sensor = new ESP32Sensor();

            ESP32Sensor.PropertyChanged += ESP32Sensor_PropertyChanged;

        }

        public async Task Connect()
        {
            await ESP32Sensor.ConnectAsync(new Uri("ws://113.161.84.132:8800/temp"));
        }

        private void ESP32Sensor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //kiểm tra xem tên thuộc tính có phải là ReceivedData không vì ta đã set thuộc tính PropertyName là ReceivedData ở hàm Invoke
            if (e.PropertyName == nameof(ESP32Sensor.ReceivedData))
            {
                Temp = JsonConvert.DeserializeObject<Temp>(ESP32Sensor.ReceivedData);
                AddItem(Temp.Temperature,Temp.Humidity);
                //RemoveItem();
            }
        }

        //[RelayCommand] //automatically create ICommand
        public void AddItem(float temperature,float humidity)
        {
            _observableTemperature.Add(new ObservableValue(temperature));
            _observableHumidity.Add(new ObservableValue(humidity));
        }

        //[RelayCommand]
        public void RemoveItem()
        {
            if (_observableTemperature.Count == 0) return;
            _observableTemperature.RemoveAt(0);

            if (_observableHumidity.Count == 0) return;
            _observableHumidity.RemoveAt(0);

        }

    }
}
