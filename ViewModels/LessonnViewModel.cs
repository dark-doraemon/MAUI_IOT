﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using MAUI_IOT.Models.Data;
using MAUI_IOT.Services.Implements;
using MAUI_IOT.Services.Interfaces.MQTT;
using MAUI_IOT.Services.Interfaces.LineChart;
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
using Newtonsoft.Json;
using System.IO;
using Microcharts;
using MAUI_IOT.Services.Implements.LineChart;
using MAUI_IOT.Services.Implements.DataManagement;
using MAUI_IOT.Models;
namespace MAUI_IOT.ViewModels
{
    public partial class LessonnViewModel : ObservableObject
    {
        private readonly string LessonName = "bai_1";
        [ObservableProperty]
        private int countNumberExperiment = 0;
        //Services
        private readonly IConnect _connect;
        private readonly IPublish _publisher;
        private readonly ISubscribe _subscriber;
        private readonly IDisconnect _disconnect;

        //Data when current
        [ObservableProperty]
        private ObservableCollection<ObservablePoint> _accX = new ObservableCollection<ObservablePoint>();
        [ObservableProperty]
        private ObservableCollection<ObservablePoint> _accY = new ObservableCollection<ObservablePoint>();
        [ObservableProperty]
        private ObservableCollection<ObservablePoint> _accZ = new ObservableCollection<ObservablePoint>();
        [ObservableProperty]
        private ObservableCollection<ObservablePoint> _accF = new ObservableCollection<ObservablePoint>();
        [ObservableProperty]
        private ObservableCollection<ObservablePoint> _accA = new ObservableCollection<ObservablePoint>();

        private ObservableCollection<double> _timetamp = new ObservableCollection<double>();


        [ObservableProperty]
        public ObservableCollection<Data> datas = new ObservableCollection<Data>();
        [ObservableProperty]
        public ObservableCollection<Data> selectedDatas = new ObservableCollection<Data>();

        //MQTT
        private MqttFactory mqttFactory;
        private IMqttClient _mqttClient;

        //Chart
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }
        public Axis[] XAxesSummarize { get; set; }
        public Axis[] YAxesSummarize { get; set; }
        public object Sync { get; } = new object();
        public RectangularSection[] Section { get; set; }
        private double Xi { get; set; } = -10;
        private double Xj { get; set; } = -10;
        public DrawMarginFrame Frame { get; set; } = new()
        {
            Fill = new SolidColorPaint(),
            Stroke = new SolidColorPaint
            {
                Color = SKColors.Gray,
                StrokeThickness = 1
            }
        };

        //Line in chart
        public ObservableCollection<ISeries> Series { get; set; }
        public ObservableCollection<ISeries> Series_X { get; set; }
        public ObservableCollection<ISeries> Series_Y { get; set; }
        public ObservableCollection<ISeries> Series_Z { get; set; }
        public ObservableCollection<ISeries> Series_Summarize { get; set; }

        [ObservableProperty]
        public ObservableCollection<ISeries> seriesRegression;

        private DateTime lastTimeTap = new DateTime();
        private const float StrokeThickness = 1.1f;
        private const float StrokeThickness_All = 1.5f;


        //Các thông số khi bắt đầu dữ liệu

        [ObservableProperty]
        private string device = "Chưa kết nối";

        [ObservableProperty]
        private double weight;

        [ObservableProperty]
        private int samplingDuration;

        [ObservableProperty]
        private int samplingRate;

        //file 
        [ObservableProperty]
        private int fileCount = Directory.GetFiles(FileSystem.AppDataDirectory).Length;
        Draw draw = new Draw();
        private double xi { get; set; } = -10;
        private double xj { get; set; } = -10;



        //IsViewVisible
        [ObservableProperty]
        private bool isCheckedDetail = false;
        [ObservableProperty]
        private bool isCheckedSummary = false;
        [ObservableProperty]
        private bool isCheckedChart = false;
        public ICommand ToggleVisibilityCommand { get; }
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
        private bool isEnableButtonSave = false;
        [ObservableProperty]
        private bool isStartingButtonStart = false;
        [ObservableProperty]
        private bool isCaculateRegression = false;
        [ObservableProperty]
        private bool isSaveData = false;
        [ObservableProperty]
        private bool isEnableTabAll = false;
        [ObservableProperty]
        private bool isNull = false;
        [ObservableProperty]
        private bool isValidEntryWeight = true;
        [ObservableProperty]
        private bool isButtonSelectActive = false;
        [ObservableProperty]
        private bool isStartingButtonSave = false;
        [ObservableProperty]
        private object zoomAndPanningMode = LiveChartsCore.Measure.ZoomAndPanMode.X;
        private bool isDoubleClickedChart = false;
        private bool OnPressed = false;
        private bool OnMoving = false;
        private bool OnReleased = false;


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

        private List<Packet> packetList = new List<Packet>();

        //Popup Items
        [ObservableProperty]
        public ObservableCollection<Models.Device> devices = new ObservableCollection<Models.Device>();

        [ObservableProperty]
        public bool isLoadingPopup;

        [ObservableProperty]
        public bool isActiveLoad;

        [ObservableProperty]
        public bool isDisplayPicker;

        [ObservableProperty]
        public Models.Device currentItems = new Models.Device() { Name = "ADXL345", Address = "ABCD/data" };

        [RelayCommand]
        public void OpenPicker()
        {
            IsLoadingPopup = true;
            IsActiveLoad = true;
            Devices.Clear();

            Devices.Add(new Models.Device() { Name = "ADXL345", Address = "ABCD/data" });
            Devices.Add(new Models.Device() { Name = "ESP32", Address = "ABCD/data" });
            Devices.Add(new Models.Device() { Name = "Humidity", Address = "ABCD/data" });
            Devices.Add(new Models.Device() { Name = "Temperature", Address = "ABCD/data" });

            IsActiveLoad = false;
            IsLoadingPopup = false;
        }


        //Summarize
        [ObservableProperty]
        public double standardDeviationA = 0;
        [ObservableProperty]
        public double avgA = 0;
        [ObservableProperty]
        public double standardDeviationF = 0;
        [ObservableProperty]
        public double avgF = 0;

        //Load data
        public ObservableCollection<Data> currentFileData { get; set; } = new ObservableCollection<Data>();
        public ObservableCollection<ExperimentConfig> currentFileExperimentConfig { get; set; } = new ObservableCollection<ExperimentConfig>();
        public ObservableCollection<DataSummarize> currentFileDataSummarize { get; set; } = new ObservableCollection<DataSummarize>();

        [ObservableProperty]
        public string primaryKey;

        [ObservableProperty]
        public string currentFileName = "NULL";

        [ObservableProperty]
        public string currentExperimentName = "NULL";

        [ObservableProperty]
        public bool isCheckLine_1 = false;

        [ObservableProperty]
        public bool isCheckLine_2 = false;

        [ObservableProperty]
        public bool isCheckLine_3 = false;

        [ObservableProperty]
        public bool isCheckLine_4 = false;

        private DatabaseHelper _database;

        private string _primaryKey = string.Empty;


        //Database
        [ObservableProperty]
        public ObservableCollection<ExperimentManager> experimentManagers = new ObservableCollection<ExperimentManager>();

        [ObservableProperty]
        private ObservableCollection<Experiment> experiments = new ObservableCollection<Experiment>();

        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Data>> datas_database = new ObservableCollection<ObservableCollection<Data>>();
        [ObservableProperty]
        private ObservableCollection<ExperimentConfig> experimentConfigs_database = new ObservableCollection<ExperimentConfig>();
        [ObservableProperty]
        private ObservableCollection<DataSummarize> dataSummarizes_database = new ObservableCollection<DataSummarize>();

        //Regression
        [ObservableProperty]
        public ObservableCollection<ObservablePoint> _fRegression = new ObservableCollection<ObservablePoint>();

        [ObservableProperty]
        public ObservableCollection<ObservablePoint> _aRegression = new ObservableCollection<ObservablePoint>();

        public LessonnViewModel() { }
        public LessonnViewModel(IConnect connect, IPublish publisher, ISubscribe subscriber, IDisconnect disconnect)
        {

            Debug.WriteLine("Hello hello");
            _connect = connect;
            _publisher = publisher;
            _subscriber = subscriber;
            _disconnect = disconnect;

            //Summarize chart
            Series = new ObservableCollection<ISeries>()
            {
                new LineSeries<ObservablePoint>
                {
                    Values = AccF,
                    Fill = null,
                    GeometryFill = null, // Màu cho điểm dữ liệu
                    GeometryStroke = null, // Đường viền cho điểm dữ liệu
                    Stroke = new SolidColorPaint(SKColors.Red) // Màu đường
                    {
                        StrokeThickness = StrokeThickness_All, // Độ dày đường
                    },

                },
                new LineSeries<ObservablePoint>
                {
                    Values = FRegression,
                    //Values = new ObservableCollection<ObservablePoint>(),
                    Fill = null,
                    GeometryFill = null, // Màu cho điểm dữ liệu
                    GeometryStroke = null, // Đường viền cho điểm dữ liệu
                    Stroke = new SolidColorPaint(SKColors.MediumPurple) // Màu đường
                    {
                        StrokeThickness = StrokeThickness*2f, // Độ dày đường
                    },
                },
            };

            //X
            Series_X = new ObservableCollection<ISeries>()
            {
                new LineSeries<ObservablePoint>
                {
                    Values = _accX,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke = new SolidColorPaint(SKColors.Red){StrokeThickness = StrokeThickness }

                },
            };

            //Y
            Series_Y = new ObservableCollection<ISeries>()
            {
                new LineSeries<ObservablePoint>
                {
                    Values = _accY,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke = new SolidColorPaint(SKColors.Black){StrokeThickness = StrokeThickness }

                },
            };

            //Z
            Series_Z = new ObservableCollection<ISeries>()
            {
                new LineSeries<ObservablePoint>
                {
                    Values = _accZ,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke = new SolidColorPaint(SKColors.Blue){StrokeThickness = StrokeThickness }
                },
            };

            //Summerize
            Series_Summarize = new ObservableCollection<ISeries>()
            {
                new LineSeries<ObservablePoint>
                {
                    Values = null,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke =  new SolidColorPaint(SKColors.Red){StrokeThickness = StrokeThickness }
                },
                new LineSeries<ObservablePoint>
                {
                    Values = null,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke =  new SolidColorPaint(SKColors.Yellow){StrokeThickness = StrokeThickness }
                },
                new LineSeries<ObservablePoint>
                {
                    Values = null,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke =  new SolidColorPaint(SKColors.Blue){StrokeThickness = StrokeThickness }
                },
                new LineSeries<ObservablePoint>
                {
                    Values = null,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke =  new SolidColorPaint(SKColors.Green){StrokeThickness = StrokeThickness }
                },
            };

            SeriesRegression = new ObservableCollection<ISeries>()
            {
                 new LineSeries<ObservablePoint>
                {
                    Values = null,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke =  new SolidColorPaint(SKColors.Red){StrokeThickness = StrokeThickness }
                },
                 new LineSeries<ObservablePoint>
                {
                    Values = null,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke =  new SolidColorPaint(SKColors.Blue){StrokeThickness = StrokeThickness }
                },
                  new LineSeries<ObservablePoint>
                {
                    Values = _accF,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke =  new SolidColorPaint(SKColors.Purple){StrokeThickness = StrokeThickness }
                },
                   new LineSeries<ObservablePoint>
                {
                    Values = AccA,
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Stroke =  new SolidColorPaint(SKColors.DarkGreen){StrokeThickness = StrokeThickness }
                },
            };

            XAxes = new[] {
                new Axis
                {
                    Labeler = value => (value).ToString("0.00 s"),
                    Name = "Time",
                    TextSize = 10,
                    SubseparatorsCount= 9,
                    NameTextSize = 10,
                    InLineNamePlacement = true,
                    LabelsPaint = new SolidColorPaint(SKColors.Gray),
                }
            };

            YAxes = new[] {
                new Axis
                {
                    //MinLimit= 0.5,
                    //MaxLimit= 1.5,
                    Name = "Value",
                    NameTextSize = 10,
                    InLineNamePlacement= true,
                    Labeler = value => value.ToString("0.00"),
                    NamePaint = new SolidColorPaint(SKColors.Gray),
                    TextSize = 10,
                    Padding = new Padding(5, 0, 15, 0),
                    LabelsPaint = new SolidColorPaint(SKColors.Gray),
                    //SeparatorsPaint = new SolidColorPaint
                    //{
                    //    Color = SKColors.Gray,
                    //    StrokeThickness = 1,
                    //    PathEffect = new DashEffect(new float[] { 3, 3 })
                    //},
                    //SubseparatorsPaint = new SolidColorPaint
                    //{
                    //    Color = SKColors.DarkGray,
                    //    StrokeThickness = 0.5f
                    //},
                    SubseparatorsCount = 9,
                    //ZeroPaint = new SolidColorPaint
                    //{
                    //    Color = SKColors.DarkSlateGray,
                    //    StrokeThickness = 2
                    //},
                    //TicksPaint = new SolidColorPaint
                    //{
                    //    Color = SKColors.Gray,
                    //    StrokeThickness = 1.5f
                    //},
                    //SubticksPaint = new SolidColorPaint
                    //{
                    //    Color = SKColors.Gray,
                    //    StrokeThickness = 1
                    //}
                }
            };

            XAxesSummarize = new[] {
                new Axis
                {
                    Labeler = value => (value).ToString("0.00"),
                    TextSize = 10,
                    SubseparatorsCount= 0,
                    NameTextSize = 10,
                    InLineNamePlacement = true,
                    LabelsPaint = new SolidColorPaint(SKColors.Gray),
                }
            };

            YAxesSummarize = new[] {
                new Axis
                {

                    //MinLimit=-0.05,
                    //MaxLimit=0.15,
                    NameTextSize = 5,
                    InLineNamePlacement= true,
                    Labeler = value => value.ToString("0.000"),
                    NamePaint = new SolidColorPaint(SKColors.Gray),
                    TextSize = 10,
                    LabelsPaint = new SolidColorPaint(SKColors.Gray),
                }
            };
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

            //File

            //Create a table managerment experiments

            try
            {
                DatabaseHelper.InitConnection(LessonName + ".db3");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            //init a experiment when start lesson, if don't save data, this file will be removed
            _primaryKey = generatePrimaryKey();
            DatabaseHelper.AddAsync(new ExperimentManager
            {
                ExperimentManagerId = _primaryKey,
                ExperimentManagerName = LessonName + " " + DateTime.Now.ToString("d/M/yyyy H:mm")
            });

            //loading files: data has been saved before
            Task.Run(async () =>
            {
                ExperimentManagers = await DatabaseHelper.GetExperimentManagersAsync();
            });

        }
        private async Task Connect()
        {
            //Clean
            AccX.Clear();
            AccY.Clear();
            AccZ.Clear();
            AccF.Clear();
            AccA.Clear();
            _timetamp.Clear();

            Datas.Clear();

            _mqttClient = mqttFactory.CreateMqttClient();
            // _mqttClient = await _connect.IConnect(mqttFactory, "test.mosquitto.org", 1883);
            _mqttClient = await _connect.IConnect(mqttFactory, "113.161.84.132", 8883, "iot", "iot@123456");
            _mqttClient = await _subscriber.ISubscriber(_mqttClient, "/ABCD1/data");

            Config config = new Config(5000, 50);
            string config_json = System.Text.Json.JsonSerializer.Serialize(config);

            _mqttClient = await _publisher.IPublisher(_mqttClient, config_json, "ABCD/control/config/req");
            _mqttClient = await _publisher.IPublisher(_mqttClient, "start", "/ABCD/control/start/req");
            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {

                var json = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                Packet packet = null;
                try
                {
                    packet = System.Text.Json.JsonSerializer.Deserialize<Packet>(json);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                if (packet != null)
                {
                    draw.DrawChart(packet, AccX, AccY, AccZ, AccF, AccF, AccA, Datas, Sync);
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
        private async Task OnStart()
        {
            Debug.Write("OnStart");
            if (IsSaveData || CountNumberExperiment < 1)
            {
                CountNumberExperiment++;
            }
            else
            {
                await Shell.Current.DisplayAlert("Thông báo!", "Vui lòng lưu dữ liệu trước", "Đồng ý");
                return;
            }

            if (CountNumberExperiment > 1)
            {
                bool answer = await Shell.Current.DisplayAlert("Thông báo", $"Tạo mới thí nghiệm thứ: {CountNumberExperiment}", "Đồng ý", "Hủy");
                if (!answer)
                {
                    CountNumberExperiment--;
                    return;
                }
            }

            Debug.WriteLine("Khối lượng" + Weight + "\n Tốc độ lấy mẫu: " + SamplingRate + "\n Thời gian lấy mẫu: " + SamplingDuration + ")");
            //if (!IsEnableButtonStart) return;

            //if (!IsEnableEntryWeight) return;

            //if (!IsValidEntryWeight) return;

            Debug.WriteLine("Start button was clicked");
            _ = Connect();
            //Task.Run(async () => { await Connect(); });
            ColorButtonStart = InActive;
            ColorButtonStop = Active;
            ColorButtonSave = InActive;

            IsEnableButtonStop = true;
            IsEnableButtonStart = false;
            IsEnableButtonSave = false;
            IsEnableEntryWeight = false;
            IsStartingButtonStart = true;
            IsEnableButtonSave = false;
            IsSaveData = false;
        }

        [RelayCommand]
        private void OnStop()
        {
            Debug.Write("OnStop");


            IsEnableButtonStart = true;
            IsEnableButtonStop = false;
            IsEnableButtonSave = true;
            IsEnableTabAll = true;
            IsEnableEntryWeight = true;
            IsStartingButtonStart = false;
            IsCaculateRegression = true;
            ColorButtonStart = Active;
            ColorButtonStop = InActive;
            ColorButtonSave = Active;

            if (Datas == null || Datas.Count < 1) return;

            Debug.WriteLine("Stop button was clicked");
            Task.Run(async () => await Disconnect());


            summarizeResult();


            try
            {
                ARegression = getLinearRegression(AccA);
                FRegression = getLinearRegression(AccF);
                SeriesRegression[0].Values = FRegression;
                SeriesRegression[1].Values = SeriesRegression;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            IsCaculateRegression = false;
            Debug.WriteLine("Onstop end");
            //SelectedDatas = new ObservableCollection<Data>(Datas);
        }

        [RelayCommand]
        public void PointerDown(PointerCommandArgs args)
        {
            checkDoubleClicked(); //true
            if (!isDoubleClickedChart)
            {
                ZoomAndPanningMode = LiveChartsCore.Measure.ZoomAndPanMode.X;
                return;
            }
            OnPressed = true;
            ZoomAndPanningMode = LiveChartsCore.Measure.ZoomAndPanMode.None;

            if (OnPressed)
            {
                var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
                var scaledPoint = chart.ScalePixelsToData(args.PointerPosition);
                Section[0].Xi = Section[0].Xj = Xi = Xj = scaledPoint.X;
                OnMoving = true;
            }
        }

        [RelayCommand]
        public void PointerMove(PointerCommandArgs args)
        {
            if (OnMoving && isDoubleClickedChart)
            {
                var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
                var scaledPoint = chart.ScalePixelsToData(args.PointerPosition);

                Section[0].Xj = Xj = scaledPoint.X;
                Debug.WriteLine("scaledPointMove" + scaledPoint.ToString());
                OnReleased = true;
            }
        }

        [RelayCommand]
        public void PointerUp(PointerCommandArgs args)
        {
            if (OnReleased && isDoubleClickedChart)
            {
                Debug.WriteLine("Pointer Up");
                Debug.WriteLine(Xi + " " + Xj);
                OnPressed = OnReleased = OnMoving = false;
                isDoubleClickedChart = false;
                if (Xi > Xj)
                {
                    double temp = Xi;
                    Xi = Xj;
                    Xj = temp;
                }
                XAxes[0].MinLimit = Xi;
                XAxes[0].MaxLimit = Xj;
                ZoomAndPanningMode = LiveChartsCore.Measure.ZoomAndPanMode.X;
                Debug.WriteLine("Datas: " + Datas.Count);
                GetData(Datas, Xi, Xj);
            }
        }

        private void checkDoubleClicked()
        {
            DateTime currentTime = DateTime.Now;
            Debug.WriteLine("currentTime: " + currentTime);
            Debug.WriteLine("lastTimeTap" + lastTimeTap);
            TimeSpan timeSinceLastTap = currentTime - lastTimeTap;
            Debug.WriteLine("timeSinceLastTap " + timeSinceLastTap.TotalMilliseconds);
            lastTimeTap = currentTime;
            if (timeSinceLastTap.TotalMilliseconds < 300)
            {
                Debug.WriteLine("Double click: true");
                isDoubleClickedChart = true;
                return;
            }
            Debug.WriteLine("Double click: false");
            isDoubleClickedChart = false;
        }

        public void GetData(ObservableCollection<Data> datas, double Xi, double Xj)
        {
            Debug.WriteLine("Datas: " + Datas.Count);
            double start = Xi * 1000; // Chỉ số bắt đầu
            double end = Xj * 1000;   // Chỉ số kết thúc
            SelectedDatas.Clear();
            Debug.WriteLine("Datas: " + Datas.Count);
            Debug.WriteLine("Before load data" + SelectedDatas.Count);
            foreach (Data i in datas)
            {
                if (i.timestamp > start && i.timestamp < end)
                {
                    SelectedDatas.Add(i);
                }
            }
            Debug.WriteLine("After load data" + SelectedDatas.Count);
        }

        //}
        [RelayCommand]
        public void addFile()
        {
            //fileCount++;
            //ExperimentInfo.Weight = 0;
            //datas = new ObservableCollection<Data>();
        }

        public static int countExperiment = 0;


        [RelayCommand]
        public async Task Save()
        {
            IsStartingButtonSave = true;
            IsEnableButtonStart = false;

            if (Datas.Count < 1 || Datas == null)
            {
                await Shell.Current.DisplayAlert("Thông báo", "Chưa có dữ liệu", "Hủy");
                IsEnableButtonStart = true;
                IsStartingButtonSave = false;
                IsSaveData = true;
                return;
            }

            Debug.WriteLine("Save file starting.....");
            try
            {
                var temp = DatabaseHelper.GetExperimentsByExperimentManagerId(_primaryKey);
                if (temp == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            PrimaryKey = generatePrimaryKey();
            countExperiment++;
            try
            {
                var experiment = new Experiment
                {
                    ExperimentId = PrimaryKey,
                    ExperimentName = LessonName + " " + CountNumberExperiment.ToString(),
                    ExperimentManagerId = _primaryKey,
                };
                await DatabaseHelper.AddAsync(experiment);

                var experimentConfig = new ExperimentConfig
                {
                    Device = CurrentItems.Name.ToString(),
                    SamplingDuration = SamplingDuration,
                    SamplingRate = SamplingRate,
                    Weight = Weight,
                    ExperimentId = PrimaryKey,
                };
                await DatabaseHelper.AddAsync(experimentConfig);

                var dataSummarize = new DataSummarize
                {
                    AvgA = AvgA,
                    AvgF = AvgF,
                    Std_A = StandardDeviationA,
                    Std_F = StandardDeviationF,
                    ExperimentId = PrimaryKey
                };
                await DatabaseHelper.AddAsync(dataSummarize);

                foreach (Data d in Datas)
                {
                    await DatabaseHelper.AddAsync(new Data
                    {
                        accX = d.accX,
                        accY = d.accY,
                        accZ = d.accZ,
                        force = d.force,
                        timestamp = d.timestamp,
                        ExperimentId = PrimaryKey
                    });
                }

                Datas_database.Add(Datas);
                ExperimentConfigs_database.Add(experimentConfig);
                DataSummarizes_database.Add(dataSummarize);

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Thông báo", "Lưu thất bại", "Đồng ý");
                Debug.WriteLine(ex.Message);
            }

            Debug.WriteLine("Save file successful");
            Experiments.Add(new Experiment
            {
                DateTime = DateTime.Now,
                ExperimentId = PrimaryKey,
                ExperimentManagerId = PrimaryKey,
                ExperimentName = LessonName + " lần " + countExperiment.ToString(),
            });

            IsEnableButtonStart = true;
            IsStartingButtonSave = false;
            IsSaveData = true;
            await Shell.Current.DisplayAlert("Thông báo", "Lưu thành công", "Đồng ý");
        }

        private async Task<string> LoadExperimentFromDatabase()
        {
            IsLoadingDataFromDataBase = true;
            ObservableCollection<ExperimentManager> Data = await DatabaseHelper.GetExperimentManagersAsync();
            if (Data.Count < 1 || Data == null)
            {
                Debug.Write("Database don't have data");
                IsLoadingDataFromDataBase = false;
                return string.Empty;
            }
            else
            {
                IsLoadingDataFromDataBase = false;
                return Data.FirstOrDefault().ExperimentManagerId.ToString() ?? string.Empty;
            }
        }

        [ObservableProperty]
        private bool isLoadingDataFromDataBase = false;

        [RelayCommand]
        public async Task LoadDataFromDatabase(string experimentManagerId)
        {
            Debug.WriteLine("Load data....");
            Debug.WriteLine("Path: " + FileSystem.AppDataDirectory.ToString());

            Datas_database.Clear();
            DataSummarizes_database.Clear();
            ExperimentConfigs_database.Clear();


            IsLoadingDataFromDataBase = true;

            if (string.IsNullOrEmpty(experimentManagerId))
            {
                Debug.WriteLine("Nothing in database");
                return;
            }

            //GEt list experiment for add data in dictionary
            ObservableCollection<Experiment> experiments = await DatabaseHelper.GetExperimentsByExperimentManagerId(experimentManagerId);
            foreach (Experiment experiment in experiments)
            {
                Datas_database.Add(await DatabaseHelper.GetDataByExperimentId(experiment.ExperimentId));
                ExperimentConfigs_database.Add(await DatabaseHelper.GetExperimentConfigByExperimentId(experiment.ExperimentId));
                DataSummarizes_database.Add(await DatabaseHelper.GetDataSummarizeByExperimentId(experiment.ExperimentId));
            }

            //Load data into collection view
            Experiments = await DatabaseHelper.GetExperimentsByExperimentManagerId(experimentManagerId);
            IsLoadingDataFromDataBase = false;
            Debug.WriteLine("Load data success");
        }

        private void getDataFromExperimentId(string id)
        {
            IsLoadingDataFromDataBase = true;

            if (currentFileData.Count < 1 || currentFileDataSummarize.Count < 1 || currentFileExperimentConfig.Count < 1)
                return;

            SelectedDatas = currentFileData;
            Device = currentFileExperimentConfig.First().Device;
            SamplingDuration = currentFileExperimentConfig.First().SamplingDuration;
            SamplingRate = currentFileExperimentConfig.First().SamplingRate;
            Weight = currentFileExperimentConfig.First().Weight;

            AccX.Clear();
            AccY.Clear();
            AccZ.Clear();

            foreach (Data data in SelectedDatas)
            {
                AccX.Add(new ObservablePoint(data.timestamp, data.accX));
                AccY.Add(new ObservablePoint(data.timestamp, data.accY));
                AccZ.Add(new ObservablePoint(data.timestamp, data.accZ));
            }

            AvgA = currentFileDataSummarize.First().AvgA;
            AvgF = currentFileDataSummarize.First().AvgF;
            StandardDeviationA = currentFileDataSummarize.First().Std_A;
            StandardDeviationF = currentFileDataSummarize.First().Std_F;
            IsLoadingDataFromDataBase = false;
        }


        public void addDataSummary()
        {
            //a = sqrt(x^2+y^2+z^2)
            if (Datas.Count > 0)
            {
                List<Double> listA = new List<double>();
                List<Double> listF = new List<double>();
                foreach (var item in Datas)
                {
                    listA.Add(Math.Sqrt(item.accX * item.accX + item.accY * item.accY + item.accZ + item.accZ));
                }
                listF = listA.Select(a => a * 5).ToList(); //5 là khối lượng quá nặng 

                AvgF = listF.Average();

                double sumOfSquaresF = listF.Select(x => Math.Pow(x - AvgF, 2)).Sum();
                StandardDeviationF = Math.Sqrt(sumOfSquaresF / listF.Count);
                AvgA = listA.Average();
                double sumOfSquares = 0;
                foreach (var item in listA)
                {
                    sumOfSquares += Math.Pow(item - AvgA, 2);
                }

            }
        }

        public void addDataDetail()
        {

            Debug.WriteLine("ADD data vào table ");
            //SelectedDatas = new ObservableCollection<Data>(Datas);
            foreach (var a in Datas)
            {
                SelectedDatas.Add(a);

            }

        }

        private string generatePrimaryKey()
        {
            return DateTime.Now.ToString("dMyyHmsmf");
        }
        private void summarizeResult()
        {
            AvgF = Datas.Average(value => value.force);
            AvgA = Datas.Average(value => value.a);
            StandardDeviationF = caculateStandardDeviation(Datas.Select(value => value.force).ToList());
            StandardDeviationA = caculateStandardDeviation(Datas.Select(value => value.a).ToList());
        }

        private double caculateStandardDeviation(List<double> datas)
        {
            double mean = datas.Average();
            double result = datas.Select(value => (value - mean) * (value - mean)).Sum() / (datas.Count - 1) * 1.0;
            return Math.Sqrt(result);
        }

        //Get experiments
        [RelayCommand]
        private void OnExperimentTapped(Experiment selectedExperiment)
        {
            IsLoadingDataFromDataBase = true;
            Debug.WriteLine("Data binding: .../");
            if (selectedExperiment != null)
            {
                Debug.WriteLine($"Selected Experiment: {selectedExperiment.ExperimentId}");
                getDataFromExperimentId(selectedExperiment.ExperimentId);
            }

            IsLoadingDataFromDataBase = false;
            Debug.WriteLine("Data binding: successful .../");
            CurrentExperimentName = selectedExperiment.ExperimentName ?? string.Empty;
        }

        //Get File
        [RelayCommand]
        private async Task OnExperimentManagerTapped(ExperimentManager selectedExperimentManager)
        {
            IsLoadingDataFromDataBase = true;
            Debug.WriteLine("Data binding: .../");
            if (selectedExperimentManager != null)
            {
                Debug.WriteLine($"Selected Experiment: {selectedExperimentManager.ExperimentManagerId}");
                Experiments.Clear();
                await LoadDataFromDatabase(selectedExperimentManager.ExperimentManagerId);
            }
            IsLoadingDataFromDataBase = false;
            CurrentFileName = selectedExperimentManager.ExperimentManagerName ?? string.Empty;
            Debug.WriteLine($"Data binding: {Experiments.Count} /");
        }

        [RelayCommand]
        private async Task DeleteSelected(ExperimentManager selectedExperimentManager)
        {
            try
            {
                if (selectedExperimentManager != null)
                {
                    ExperimentManagers.Remove(selectedExperimentManager);
                    await DatabaseHelper.DeleteExperimentManagerById(selectedExperimentManager.ExperimentManagerId);

                    ObservableCollection<Experiment> experimentId = await DatabaseHelper.GetExperimentsByExperimentManagerId(selectedExperimentManager.ExperimentManagerId);

                    if (experimentId.Count < 1 || experimentId == null) return;

                    foreach (Experiment experiment in experimentId)
                    {
                        await DatabaseHelper.DeleteByExperimentId(experiment.ExperimentId);
                    }

                    await DatabaseHelper.DeleteByExperimentId(selectedExperimentManager.ExperimentManagerId);

                    Debug.WriteLine("Delete file success: " + selectedExperimentManager.ExperimentManagerName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        partial void OnIsCheckLine_1Changed(bool value)
        {
            if (value)
                SeriesRegression[0].Values = FRegression;
            else
                SeriesRegression[0].Values = null;
        }

        partial void OnIsCheckLine_2Changed(bool value)
        {
            if (value)
                SeriesRegression[1].Values = ARegression;
            else
                SeriesRegression[1].Values = null;
        }

        partial void OnIsCheckLine_3Changed(bool value)
        {
            if (value)
                SeriesRegression[2].Values = AccF;
            else
                SeriesRegression[2].Values = null;
        }

        partial void OnIsCheckLine_4Changed(bool value)
        {
            if (value)
                SeriesRegression[3].Values = AccA;
            else
                SeriesRegression[3].Values = null;
        }

        private ObservableCollection<ObservablePoint> getLinearRegression(ObservableCollection<ObservablePoint> observablePoints)
        {
            (double, double) weight_bias = CaculateRegression.LinearRegressionFunction(observablePoints);
            ObservableCollection<ObservablePoint> linearRegression = new ObservableCollection<ObservablePoint>();

            foreach (var x in observablePoints)
            {
                if (x != null)
                    linearRegression.Add(new ObservablePoint(x.X, (double)(weight_bias.Item1 * x.Y + weight_bias.Item2)));
            }
            return linearRegression;
        }
    }
}