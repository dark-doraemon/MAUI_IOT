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

        private Models.Axis aDXL345Model;

        public Models.Axis ADXL345Model
        {
            get => aDXL345Model;
            set
            {
                if (aDXL345Model != value)
                {
                    aDXL345Model = value;
                    OnPropertyChanged(nameof(ADXL345Model));
                }
            }
        }


        public ADXL345ViewModel()
        {
            xAxis = new ObservableCollection<ObservableValue>();
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

            aDXL345Model = new Models.Axis();

            ADXL345Sensor = new ADXL345Sensor();

            ADXL345Sensor.PropertyChanged += ADXL345Sensor_PropertyChanged;

        }

        public async Task Connect()
        {
            await ADXL345Sensor.ConnectAsync(new Uri("ws://113.161.84.132:8800/temp"));
        }

        private void ADXL345Sensor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ADXL345Sensor.ReceivedData))
            {
                ADXL345Model = JsonConvert.DeserializeObject<Models.Axis>(ADXL345Sensor.ReceivedData);
                AddItem(aDXL345Model.x, aDXL345Model.y, aDXL345Model.z);


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
