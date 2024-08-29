﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using MAUI_IOT.Models.Data;
using MAUI_IOT.Services.Interfaces.MQTT;
using MAUI_IOT.Views;
using MQTTnet;
using MQTTnet.Client;
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
using System.Windows.Input;

namespace MAUI_IOT.ViewModels
{
    public partial class LessonnViewModel : ObservableObject
    {
        //Services
        private IConnect _connect;
        private IPublish _publisher;
        private ISubscribe _subscriber;
        private IDisconnect _disconnect;

        //Data
        private readonly List<double> _accX = new List<double>();
        private readonly List<double> _accY = new List<double>();
        private readonly List<double> _accZ = new List<double>();
        private readonly List<double> _force = new List<double>();

        [ObservableProperty]
        public ObservableCollection<Data> datas = new ObservableCollection<Data>();
        [ObservableProperty]
        public ObservableCollection<Data> selectedDatas = new ObservableCollection<Data>();

        //MQTT
        private MqttFactory mqttFactory;
        private IMqttClient _mqttClient;

        //Chart
        public Axis[] XAxes { get; set; }
        private readonly DateTimeAxis _customAxis;
        public object Sync { get; } = new object();
        public DrawMarginFrame Frame { get; set; } = new()
        {
            Fill = new SolidColorPaint(),
            Stroke = new SolidColorPaint
            {
                Color = SKColors.Gray,
                StrokeThickness = 1
            }
        };
        public ObservableCollection<ISeries> new_Series { get; set; }
        public ObservableCollection<ISeries> Series_X { get; set; }
        public ObservableCollection<ISeries> Series_Y { get; set; }
        public ObservableCollection<ISeries> Series_Z { get; set; }
        private DateTime startTime = new DateTime();

        //Weight (input)
        [ObservableProperty]
        private double m = 0;

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
        public RectangularSection[] Section { get; set; }
        private double Xi { get; set; } = -10;
        private double Xj { get; set; } = -10;

        //Colors
        public static Color InActive = Color.FromRgb(214, 214, 214);
        public static Color Active = Color.FromRgb(2, 126, 111);
        public static Color Default = Color.FromRgb(178, 215, 239);

        //State
        [ObservableProperty]
        private bool isEnableEntryWeight = true;
        [ObservableProperty]
        private bool isEnableButtonStop = false;
        [ObservableProperty]
        private bool isEnableButtonStart = true;
        [ObservableProperty]
        private bool isStartingButtonStart = false;
        [ObservableProperty]
        private bool isEnableTabAll = false;
        [ObservableProperty]
        private bool isNull = false;
        [ObservableProperty]
        private bool isValidEntryWeight = false;
        [ObservableProperty]
        private bool isButtonSelectActive = false;

        //Color
        [ObservableProperty]
        private Color colorButtonStart = Active;
        [ObservableProperty]
        private Color colorButtonSave = InActive;
        [ObservableProperty]
        private Color colorButtonStop = InActive;
        [ObservableProperty]
        private Color colorButtonSelectRange = Default;


        //Text
        [ObservableProperty]
        private string textSelectRangeButton = "Select Range";

        public LessonnViewModel() { }
        public LessonnViewModel(IConnect connect, IPublish publisher, ISubscribe subscriber, IDisconnect disconnect)
        {

            Debug.WriteLine("Hello hello");

            _connect = connect;
            _publisher = publisher;
            _subscriber = subscriber;
            _disconnect = disconnect;

            //Summarize chart
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

            //X
            Series_X = new ObservableCollection<ISeries>()
            {
                new LineSeries<double>
                {
                    Values = _accX,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke = new SolidColorPaint(SKColors.Green){StrokeThickness = 1 }

                },
            };

            //Y
            Series_Y = new ObservableCollection<ISeries>()
            {
                new LineSeries<double>
                {
                    Values = _accY,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke = new SolidColorPaint(SKColors.Yellow){StrokeThickness = 1 }

                },
            };

            //Z
            Series_Z = new ObservableCollection<ISeries>()
            {
                new LineSeries<double>
                {
                    Values = _accZ,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke = new SolidColorPaint(SKColors.Red){StrokeThickness = 1 }

                },
            };

            //XAxes
            _customAxis = new DateTimeAxis(TimeSpan.FromSeconds(1), Formatter)
            {
                CustomSeparators = GetSeparators(),
                AnimationsSpeed = TimeSpan.FromMilliseconds(0),
                SeparatorsPaint = new SolidColorPaint(SKColors.Black.WithAlpha(100))
            };

            XAxes = new Axis[] { _customAxis };

            //Sections
            Section = new RectangularSection[]
            {
                new RectangularSection
                {
                    Xi = 0,
                    Xj = 0,
                    Fill = new SolidColorPaint(new SKColor(83, 137, 71).WithAlpha(20))
                }
            };

            //MQTT connect
            mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();
        }
        private async Task Connect()
        {
            //Clean
            _accX.Clear();
            _accY.Clear();
            _accZ.Clear();
            _force.Clear();
            Datas.Clear();

            _mqttClient = mqttFactory.CreateMqttClient();
            _mqttClient = await _connect.IConnect(mqttFactory, "113.161.84.132", 8883, "iot", "iot@123456");
            _mqttClient = await _subscriber.ISubscriber(_mqttClient, "/ABCD/data");

            Config config = new Config(5000, 50);
            string config_json = JsonSerializer.Serialize(config);

            _mqttClient = await _publisher.IPublisher(_mqttClient, config_json, "ABCD/control/config/req");
            _mqttClient = await _publisher.IPublisher(_mqttClient, "start", "/ABCD/control/start/req");

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {

                var json = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                Packet packet = JsonSerializer.Deserialize<Packet>(json);

                if (packet != null)
                {
                    lock (Sync)
                    {
                        foreach (Data data in packet.data)
                        {
                            //chart
                            _accX.Add(data.accX);
                            _accY.Add(data.accY);
                            _accZ.Add(data.accZ);
                            _force.Add(data.force);

                            //table
                            Datas.Add(data);

                            if (_accX.Count > 250) _accX.RemoveAt(0);
                            if (_accY.Count > 250) _accY.RemoveAt(0);
                            if (_accZ.Count > 250) _accZ.RemoveAt(0);
                            if (_force.Count > 250) _force.RemoveAt(0);

                            _customAxis.CustomSeparators = GetSeparators();
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
        private async Task Disconnect()
        {
            await _publisher.IPublisher(_mqttClient, "stop", "/ABCD/control/stop/req");
            await _disconnect.IDisconnect(_mqttClient);
        }

        [RelayCommand]
        public async Task Zoom(ObservableCollection<ISeries> series)
        {
            Dictionary<string, object> paramaters = new Dictionary<string, object>
            {
                {"data",series },
            };
            try
            {
                await Shell.Current.GoToAsync(nameof(FullScreenChartView), paramaters);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        [ObservableProperty]
        private string textButtonSelect = "Select Range";

        [RelayCommand]
        private void OnStart()
        {
            Debug.Write("OnStart");
            if (!IsEnableButtonStart) return;

            if (!IsEnableEntryWeight) return;

            if (!IsValidEntryWeight) return;

            Debug.WriteLine("Start button was clicked");
            Task.Run(async () => { await Connect(); });

            ColorButtonStart = InActive;
            ColorButtonStop = Active;
            ColorButtonSave = InActive;

            IsEnableButtonStop = true;
            IsEnableButtonStart = false;
            IsEnableEntryWeight = false;
            IsStartingButtonStart = true;
        }

        [RelayCommand]
        private void OnStop()
        {
            Debug.Write("OnStop");

            if (!IsStartingButtonStart) return;

            Debug.WriteLine("Stop button was clicked");
            Task.Run(async () => await Disconnect());

            IsEnableButtonStart = true;
            IsEnableButtonStop = false;
            IsEnableTabAll = true;
            IsEnableEntryWeight = true;
            IsStartingButtonStart = false;

            ColorButtonStart = Active;
            ColorButtonStop = InActive;
        }

        [RelayCommand]
        private void OnSelectRangeButton()
        {
            IsButtonSelectActive = !IsButtonSelectActive;

            if (IsButtonSelectActive)
            {
                TextSelectRangeButton = "Selecting";
                ColorButtonSelectRange = Active;
            }
            else
            {
                TextSelectRangeButton = "Select Range";
                ColorButtonSelectRange = Default;
                //_customAxis.MaxLimit = null;
                //_customAxis.MinLimit = null;
            }
        }

        [RelayCommand]
        public void PointerDown(PointerCommandArgs args)
        {
            if (IsButtonSelectActive)
            {
                var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
                var scaledPoint = chart.ScalePixelsToData(args.PointerPosition);
                Section[0].Xi = Section[0].Xj = Xi = Xj = scaledPoint.X;
            }
        }

        [RelayCommand]
        public void PointerUp(PointerCommandArgs args)
        {
            if (IsButtonSelectActive)
            {
                Debug.WriteLine("Pointer Up");


                this.SelectedDatas.Clear();


                _customAxis.MinLimit = Xi;
                _customAxis.MaxLimit = Xj;
                // var xValues = ((LineSeries<ObservableValue>)new_Series[0]).Values.Select(x => x.Value).ToList().Skip((int)Section[0].Xi).Take((int)Section[0].Xj - (int)Section[0].Xi + 1).ToList();
                // var yValues = ((LineSeries<ObservableValue>)new_Series[1]).Values.Select(x => x.Value).ToList().Skip((int)Section[0].Xi).Take((int)Section[0].Xj - (int)Section[0].Xi + 1).ToList();
                // var zValues = ((LineSeries<ObservableValue>)new_Series[2]).Values.Select(x => x.Value).ToList().Skip((int)Section[0].Xi).Take((int)Section[0].Xj - (int)Section[0].Xi + 1).ToList();

            }
        }

        [RelayCommand]
        public void PointerMove(PointerCommandArgs args)
        {
            if (IsButtonSelectActive)
            {
                var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
                var scaledPoint = chart.ScalePixelsToData(args.PointerPosition);

                Section[0].Xj = Xj = scaledPoint.X;
                Debug.WriteLine("scaledPointMove" + scaledPoint.ToString());
            }
        }

        //private void getXYZ_range(List<double?> x, List<double?> y, List<double?> z)
        //{
        //    if (x.Count != y.Count || x.Count != z.Count) return;
        //    // Chuyển một list có thể có giá trị null sang một list không có giá trị null
        //    var nonNullxValue = x.Where(value => value.HasValue).Select(value => value.Value).ToList();
        //    var nonNullyValue = y.Where(value => value.HasValue).Select(value => value.Value).ToList();
        //    var nonNullzValue = z.Where(value => value.HasValue).Select(value => value.Value).ToList();

        //    this.Series_X = new ObservableCollection<double>(nonNullxValue);
        //    this.Series_Y = new ObservableCollection<double>(nonNullyValue);
        //    this.Series_Z = new ObservableCollection<double>(nonNullzValue);

        //    afterSelected_F = new ObservableCollection<double>();
        //    afterSelected_a = new ObservableCollection<double>();

        //    for (int i = 0; i < xValues.Count; i++)
        //    {
        //        afterSelected_a.Add(Math.Sqrt(xValues[i] * xValues[i] + yValues[i] * yValues[i] + zValues[i] * zValues[i]));
        //    }

        //    if (afterSelected_a.Count != xValues.Count) return;

        //    foreach (var value in afterSelected_a)
        //    {
        //        afterSelected_F.Add(this.M * value);
        //    }

        //    Debug.WriteLine("after select" + afterSelected_a.Count + " " + afterSelected_F.Count);

        //}
    }
}
