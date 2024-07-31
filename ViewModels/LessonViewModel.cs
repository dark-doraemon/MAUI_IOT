using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.Defaults;
using LiveChartsCore;
using MAUI_IOT.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore.SkiaSharpView;
using Newtonsoft.Json;
using CommunityToolkit.Mvvm.Input;

namespace MAUI_IOT.ViewModels
{
    [QueryProperty(nameof(Lesson), "data")]
    public partial class LessonViewModel : ObservableObject
    {
        [ObservableProperty]
        public Lesson lesson;

        private readonly ObservableCollection<ObservableValue> xAxis;
        private readonly ObservableCollection<ObservableValue> yAxis;
        private readonly ObservableCollection<ObservableValue> zAxis;
        
        public ObservableCollection<ISeries> Series1 { get; set; }
        public ObservableCollection<ISeries> Series2 { get; set; }
        public ObservableCollection<ISeries> Series3 { get; set; }

        ADXL345Sensor ADXL345Sensor { get; set; }
        
        private Models.Axis aDXL345Axis;

        public Models.Axis ADXL345Axis
        {
            get => aDXL345Axis;
            set
            {
                aDXL345Axis = value;
                OnPropertyChanged(nameof(ADXL345Axis));
            }
        }

        public ObservableCollection<ISeries> Series { get; set; }

        public LessonViewModel()
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

            Series = new ObservableCollection<ISeries>
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

            aDXL345Axis = new Models.Axis();
            ADXL345Sensor = new ADXL345Sensor();
            ADXL345Sensor.PropertyChanged += ADXL345Sensor_PropertyChanged;
        }

        private async void ADXL345Sensor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ADXL345Sensor.ReceivedData))
            {
                ADXL345Axis = JsonConvert.DeserializeObject<Models.Axis>(ADXL345Sensor.ReceivedData);
                AddItem(ADXL345Axis.x, ADXL345Axis.y, ADXL345Axis.z);


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

        [RelayCommand]
        async void Start()
        {
            await ADXL345Sensor.ConnectAsync(new Uri("ws://113.161.84.132:8800/api/adxl345"));
        }

        [RelayCommand]
        void Stop()
        {

        }

        [RelayCommand]
        void Save()
        {

        }
    }
}
