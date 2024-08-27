using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MAUI_IOT.Models.Data;
using MAUI_IOT.Services.Interfaces.MQTT;
using MQTTnet;
using MQTTnet.Internal;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAUI_IOT.ViewModels
{
    public partial class LessonnViewModel
    {
        //Services
        private IConnect _connect;
        private IPublish _publisher;
        private ISubscribe _subscriber;


        //Chart
        private readonly List<double> _accX = new List<double>();
        private readonly List<double> _accY = new List<double>();
        private readonly List<double> _accZ = new List<double>();
        private readonly List<double> _force = new List<double>();

        private MqttFactory mqttFactory;

        private readonly DateTimeAxis _newCustomAxis;

        public ObservableCollection<ISeries> new_Series { get; set; }
        public Axis[] newXAxes { get; set; }
        public object Sync { get; } = new object();

        public LessonnViewModel() { }

        public LessonnViewModel(IConnect connect, IPublish publisher, ISubscribe subscriber)
        {
            _connect = connect;
            _publisher = publisher;
            _subscriber = subscriber;

            new_Series = new ObservableCollection<ISeries>()
            {
                new LineSeries<double>
                {
                    Values = _accX,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke = new SolidColorPaint(SKColors.Green){StrokeThickness = 1 }
                },
                new LineSeries<double>
                {
                    Values = _accY,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke = new SolidColorPaint(SKColors.Yellow){StrokeThickness =1 }
                },
                new LineSeries<double>
                {
                    Values= _accZ,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke = new SolidColorPaint(SKColors.Red){StrokeThickness = 1 }
                },
                new LineSeries<double>
                {
                    Values = _force,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke = new SolidColorPaint(SKColors.Blue){StrokeThickness = 1},
                }
            };

            _newCustomAxis = new DateTimeAxis(TimeSpan.FromSeconds(1), Formatter)
            {
                CustomSeparators = GetSeparators(),
                AnimationsSpeed = TimeSpan.FromMilliseconds(0),
                SeparatorsPaint = new SolidColorPaint(SKColors.Tomato.WithAlpha(100))
            };

            newXAxes = new Axis[] { _newCustomAxis };

            mqttFactory = new MqttFactory();
        }

        private double[] GetSeparators()
        {
            var now = DateTime.Now;

            return new double[]
            {
            now.AddSeconds(-25).Ticks,
            now.AddSeconds(-20).Ticks,
            now.AddSeconds(-15).Ticks,
            now.AddSeconds(-10).Ticks,
            now.AddSeconds(-5).Ticks,
            now.Ticks
            };
        }

        private static string Formatter(DateTime date)
        {
            var secsAgo = (DateTime.Now - date).TotalSeconds;

            return secsAgo < 1
                ? "now"
                : $"{secsAgo:N0}s ago";
        }

        [RelayCommand]
        private void StartButton()
        {
            Debug.WriteLine("Start button was clicked");
            _ = Task.Run(async () => { await newStart(); });
        }


        private async Task newStart()
        {
            var mqttClient = mqttFactory.CreateMqttClient();
            mqttClient = await _connect.IConnect(mqttFactory, "113.161.84.132", 8883, "iot", "iot@123456");
            mqttClient = await _subscriber.ISubscriber(mqttClient, "/adxl345/data");

            mqttClient.ApplicationMessageReceivedAsync += async e =>
            {

                var josn = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                Packet packet = JsonSerializer.Deserialize<Packet>(josn);

                if (packet != null)
                {
                    lock (Sync)
                    {
                        foreach(Data data in packet.data)
                        {
                            _accX.Add(data.accX);
                            _accY.Add(data.accY);
                            _accZ.Add(data.accZ);
                            _force.Add(data.force);
                               
                            if (_accX.Count > 250) _accX.RemoveAt(0);
                            if (_accY.Count > 250) _accY.RemoveAt(0);
                            if (_accZ.Count > 250) _accZ.RemoveAt(0);
                            if (_force.Count > 250) _force.RemoveAt(0);

                            _newCustomAxis.CustomSeparators = GetSeparators();
                        }
                    }




                    Debug.WriteLine($"Name: {packet.name} \n Packet number: {packet.packetNumber} \n data: {packet.data}");
                    foreach (Data data in packet.data)
                    {
                        Debug.WriteLine($"timetamp: {data.timetamp}\naccX: {data.accX}\naccY: {data.accX}\naccZ: {data.accZ}");
                    }
                }

                else
                {
                    Debug.WriteLine($"Null");
                }
            };
            await Task.Delay(Timeout.Infinite);
        }
    }
}
