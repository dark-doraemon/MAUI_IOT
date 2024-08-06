using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAUI_IOT.ViewModels.SensorViewModels
{
    public class ADXL345ViewModel : ObservableObject
    {
        private readonly ObservableCollection<ObservableValue> xAxis;
        private readonly ObservableCollection<ObservableValue> yAxis;
        private readonly ObservableCollection<ObservableValue> zAxis;

        public ObservableCollection<ISeries> Series1 { get; set; }
        public ObservableCollection<ISeries> Series2 { get; set; }
        public ObservableCollection<ISeries> Series3 { get; set; }

        ADXL345Sensor ADXL345Sensor { get; set; }

        private Models.CustomAxis aDXL345Axis;

        public Models.CustomAxis ADXL345Axis
        {
            get => aDXL345Axis;
            set
            {
                if (aDXL345Axis != value)
                {
                    aDXL345Axis = value;
                    OnPropertyChanged(nameof(ADXL345Axis));
                }
            }
        }


        public ADXL345ViewModel()
        {
            xAxis = new ObservableCollection<ObservableValue>
            {
                 new ObservableValue(5),
                new(10),
                new(15),
                new(20),
                new(25),
                new(30),
            };
            yAxis = new ObservableCollection<ObservableValue>();
            zAxis = new ObservableCollection<ObservableValue>();

            Series1 = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = xAxis,
                    Fill = null,
                    LineSmoothness = 0
                },
                new LineSeries<ObservableValue>
                {
                    Values = yAxis,
                    Fill = null,
                    LineSmoothness = 0
                },
                new LineSeries<ObservableValue>
                {
                    Values = zAxis,
                    Fill = null,
                    LineSmoothness = 0
                },
            };

            aDXL345Axis = new Models.CustomAxis();
            ADXL345Sensor = new ADXL345Sensor();
            ADXL345Sensor.PropertyChanged += ADXL345Sensor_PropertyChanged;

        }

        public async Task Connect()
        {
            await ADXL345Sensor.ConnectAsync(new Uri("ws://113.161.84.132:8800/api/adxl345"));
        }

        private void ADXL345Sensor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ADXL345Sensor.ReceivedData))
            {
                ADXL345Axis = JsonConvert.DeserializeObject<Models.CustomAxis>(ADXL345Sensor.ReceivedData);
                AddItem(aDXL345Axis.x, aDXL345Axis.y, aDXL345Axis.z);


                RemoveItem();
            }
        }

        public void AddItem(float x, float y, float z)
        {
            xAxis.Add(new ObservableValue(x));
            yAxis.Add(new ObservableValue(y));
            zAxis.Add(new ObservableValue(z));
        }

        public void RemoveItem()
        {
            if (xAxis.Count >= 30)
            {
                if (xAxis.Count == 0) return;
                xAxis.RemoveAt(0);

                if (yAxis.Count == 0) return;
                yAxis.RemoveAt(0);

                if (zAxis.Count == 0) return;
                zAxis.RemoveAt(0);
            }
        }
    }
}
