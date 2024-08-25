using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.Defaults;
using LiveChartsCore;
using MAUI_IOT.Models;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView;
using Newtonsoft.Json;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using MAUI_IOT.Views;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using Microsoft.Maui.Storage;
using System.Reflection.Metadata;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.Drawing;


namespace MAUI_IOT.ViewModels
{
    [QueryProperty(nameof(Lesson), "data")]
    public partial class LessonViewModel : ObservableObject
    {
        [ObservableProperty]
        public Lesson lesson;

        private static readonly SKColor s_gray = new(195, 195, 195);
        private static readonly SKColor s_gray1 = new(160, 160, 160);
        private static readonly SKColor s_gray2 = new(90, 90, 90);
        private static readonly SKColor s_dark3 = new(60, 60, 60);

        private readonly DateTimeAxis _customAxis;

        private readonly ObservableCollection<ObservableValue> xAxis;
        private readonly ObservableCollection<ObservableValue> yAxis;
        private readonly ObservableCollection<ObservableValue> zAxis;
        private readonly IServiceProvider serviceProvider;
        private readonly FullScreenChartViewModel fullScreenChartViewModel;

        public ObservableCollection<ISeries> SeriesX { get; set; }
        public ObservableCollection<ISeries> SeriesY { get; set; }
        public ObservableCollection<ISeries> SeriesZ { get; set; }

        public ObservableCollection<double> a { get; set; }
        public ObservableCollection<double> F { get; set; }
        public ObservableCollection<DateTime> Time { get; set; }
        public ObservableCollection<TimeSpan> Duration { get; set; }

        //dữ liệu khối lượng
        [ObservableProperty]
        private double m;

        [ObservableProperty]
        private double avgF;

        public event EventHandler OnStop;
        public event EventHandler OnSave;
        public event EventHandler OnStart;

        public bool isSelected;
        [ObservableProperty]
        private List<ObservableValue> selectedValue = new List<ObservableValue>();

        // điểm đầu điểm cuối
        private double x1 = -10;
        private double x2 = -10;

        //Lưu các giá trị trên biểu đồ
        public ObservableCollection<double> xValues;
        public ObservableCollection<double> yValues;
        public ObservableCollection<double> zValues;

        ADXL345Sensor ADXL345Sensor { get; set; }

        private CustomAxis aDXL345Axis;

        public CustomAxis ADXL345Axis
        {
            get => aDXL345Axis;
            set
            {
                aDXL345Axis = value;
                OnPropertyChanged(nameof(ADXL345Axis));
            }
        }

        public ObservableCollection<ISeries> Series { get; set; }


        public Axis[] XAxes { get; set; } =
        {
            new Axis
            {
                Name = "X axis",
                NamePaint = new SolidColorPaint(s_gray1),
                TextSize = 18,
                Padding = new Padding(5, 15, 5, 5),
                LabelsPaint = new SolidColorPaint(s_gray),
                SeparatorsPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1,
                    PathEffect = new DashEffect(new float[] { 3, 3 })
                },
                SubseparatorsPaint = new SolidColorPaint
                {
                    Color = s_gray2,
                    StrokeThickness = 0.5f
                },
                SubseparatorsCount = 9,
                ZeroPaint = new SolidColorPaint
                {
                    Color = s_gray1,
                    StrokeThickness = 2
                },
                TicksPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1.5f
                },
                SubticksPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1
                },
            }
        };


        public Axis[] YAxes { get; set; } =
        {
            new Axis
            {
                Name = "Y axis",
                NamePaint = new SolidColorPaint(s_gray1),
                TextSize = 18,
                Padding = new Padding(5, 0, 15, 0),
                LabelsPaint = new SolidColorPaint(s_gray),
                SeparatorsPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1,
                    PathEffect = new DashEffect(new float[] { 3, 3 })
                },
                SubseparatorsPaint = new SolidColorPaint
                {
                    Color = s_gray2,
                    StrokeThickness = 0.5f
                },
                SubseparatorsCount = 9,
                ZeroPaint = new SolidColorPaint
                {
                    Color = s_gray1,
                    StrokeThickness = 2
                },
                TicksPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1.5f
                },
                SubticksPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1
                }
            }
        };

        public RectangularSection[] Sections { get; set; } = new RectangularSection[]
                   {
                        new RectangularSection {
                            Xi = 0,
                            Xj = 0,
                            Fill = new SolidColorPaint(new SKColor(255, 255, 255))
                        },
                   };

        public LessonViewModel()
        {

        }

        public LessonViewModel(IServiceProvider serviceProvider, FullScreenChartViewModel fullScreenChartViewModel)
        {
            xAxis = new ObservableCollection<ObservableValue>
            {
                // new ObservableValue(5),
                //new(10),
                //new(15),
                //new(20),
                //new(25),
                //new(30),
            };
            yAxis = new ObservableCollection<ObservableValue>();
            zAxis = new ObservableCollection<ObservableValue>();

            Series = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = xAxis,
                    Fill = null,
                    LineSmoothness = 0,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = 1 },
                },
                new LineSeries<ObservableValue>
                {
                    Values = yAxis,
                    Fill = null,
                    LineSmoothness = 0,
                    GeometrySize = 0,
                     Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 1 },
                },
                new LineSeries<ObservableValue>
                {
                    Values = zAxis,
                    Fill = null,
                    LineSmoothness = 0,
                    GeometrySize = 0,
                    Stroke = new SolidColorPaint(SKColors.Yellow) { StrokeThickness = 1 },
                },
            };


            SeriesX = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = xAxis,
                    Fill = null,
                    LineSmoothness = 0,
                    Stroke = new SolidColorPaint(SKColors.White),
                    GeometrySize = 2
                }
            };

            SeriesY = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = yAxis,
                    Fill = null,
                    LineSmoothness = 0,
                    Stroke = new SolidColorPaint(SKColors.White),
                    GeometrySize = 0
                }
            };

            SeriesZ = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservableValue>
                {
                    Values = zAxis,
                    Fill = null,
                    LineSmoothness = 0,
                    Stroke = new SolidColorPaint(SKColors.White),
                    GeometrySize = 0
                }
            };


            aDXL345Axis = new Models.CustomAxis();
            ADXL345Sensor = new ADXL345Sensor();
            ADXL345Sensor.PropertyChanged += ADXL345Sensor_PropertyChanged;
            this.serviceProvider = serviceProvider;
            this.fullScreenChartViewModel = fullScreenChartViewModel;

            //khởi tạo các biến lưu giá trị a, F, Time, Duration ( rút gọn của thời gian)
            a = new ObservableCollection<double>();
            F = new ObservableCollection<double>();
            Time = new ObservableCollection<DateTime>();
            Duration = new ObservableCollection<TimeSpan>();
        }

        private async void ADXL345Sensor_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ADXL345Sensor.ReceivedData))
            {
                ADXL345Axis = JsonConvert.DeserializeObject<Models.CustomAxis>(ADXL345Sensor.ReceivedData);
                await AddItem(ADXL345Axis.x, ADXL345Axis.y, ADXL345Axis.z);
                caculateA(ADXL345Axis.x, ADXL345Axis.y, ADXL345Axis.z, ADXL345Axis.TimeStamp);
                RemoveItem();
            }
        }

        public async Task AddItem(float x, float y, float z)
        {
            xAxis.Add(new ObservableValue(x));
            yAxis.Add(new ObservableValue(y));
            zAxis.Add(new ObservableValue(z));
        }

        public void RemoveItem()
        {
            if (xAxis.Count >= 100)
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
        async Task Start()
        {
            OnStart?.Invoke(this, new EventArgs());
            if (m == 0) return;
            await ADXL345Sensor.ConnectAsync(new Uri("ws://113.161.84.132:8800/api/adxl345"));
        }

        [RelayCommand]
        async Task Stop()
        {
            await ADXL345Sensor.CloseAsync();
            await DataBinding();
            OnStop?.Invoke(this, new EventArgs());
        }

        [RelayCommand]
        async Task Save()
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "data_table.json");
            await SaveFile(filePath);
            OnSave?.Invoke(this, new EventArgs());
        }

        public DrawMarginFrame Frame { get; set; } = new()
        {
            Fill = new SolidColorPaint(s_dark3),
            Stroke = new SolidColorPaint
            {
                Color = s_gray,
                StrokeThickness = 1
            }
        };

        [RelayCommand]
        public async Task Zoom(ObservableCollection<ISeries> series)
        {
            Dictionary<string, object> paramaters = new Dictionary<string, object>
            {
                {"data",series },
            };
            await Shell.Current.GoToAsync(nameof(FullScreenChartView), paramaters);
        }

        public async Task caculateA(double x, double y, double z, DateTime time)
        {
            await Task.Run(() =>
            {
                double result = Math.Sqrt(x * x + y * y + z * z);
                MainThread.BeginInvokeOnMainThread(() => { a.Add(result); F.Add(result * m); Time.Add(time); });
            });
        }

        //dữ liệu cho bảng
        private async Task DataBinding()
        {
            if (a.Count == 0 || Time.Count == 0 || a.Count != F.Count || a.Count != Time.Count)
            {
                Debug.WriteLine("Dữ liệu không hợp lệ.");
                return;
            }

            int n = a.Count;
            if (n <= 1) return;

            TimeSpan diff = Time[n - 1] - Time[0];

            // Số mẫu lấy trong 100ms
            int duration = (int)(n * 100 / diff.TotalMilliseconds);

            if (duration <= 0)
            {
                Debug.WriteLine("Duration phải lớn hơn 0.");
                return;
            }

            // Tạo biến hứng giá trị
            ObservableCollection<double> _a = new ObservableCollection<double>();
            ObservableCollection<double> _F = new ObservableCollection<double>();
            ObservableCollection<DateTime> _time = new ObservableCollection<DateTime>();

            for (int i = 0; i < n; i += duration)
            {
                if (i < a.Count && i < F.Count && i < Time.Count)
                {
                    _a.Add(a[i]);
                    _F.Add(F[i]);
                    _time.Add(Time[i]);
                }
            }

            a = _a;
            F = _F;
            Time = _time;

            Duration.Clear();

            for (int i = 0; i < Time.Count; i++)
                Duration.Add(Time[i] - Time[0]);

            avgF = F.Any() ? F.Average() : 0;
        }



        //lưu dữ liệu cho bảng
        public async Task SaveFile(string path)
        {
            if (Duration.Count < Time.Count)
            {
                Duration.Add(Time[Time.Count - 1] - Time[0]);
            }
            var dataSave = new
            {
                a = a.ToArray(),
                F = F.ToArray(),
                Time = Time.Select(dt => dt.ToString("o")).ToArray(),
                Duration = Duration.Select(ts => ts.ToString()).ToArray(),
                m = M
            };

            var jsonData = JsonConvert.SerializeObject(dataSave, Formatting.Indented);

            try
            {
                await File.WriteAllTextAsync(path, jsonData);
                this.path = path;
                Debug.Write(path + "save file success");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SaveFile: " + ex.ToString());
            }
        }

        public string path { get; set; }

        [RelayCommand]
        public void OnPointerPressed(PointerCommandArgs e)
        {

            if (isSelected == true)
            {
                var chart = (ICartesianChartView<SkiaSharpDrawingContext>)e.Chart;
                var scaledPoint = chart.ScalePixelsToData(e.PointerPosition);

                if (x1 == -10)
                {
                    x1 = Math.Floor(scaledPoint.X);
                    if (x1 < 0)
                    {
                        x1 = 0;
                    }
                    return;
                }

                if (x2 == -10)
                {
                    x2 = Math.Ceiling(scaledPoint.X);
                    if (x2 < 0)
                    {
                        x2 = 0;
                    }
                }

                if (x2 > x1)
                {
                    this.SelectedValue.Clear();

                    var xValues = ((LineSeries<ObservableValue>)Series[0]).Values.Select(x => x.Value).ToList().Skip((int)x1).Take((int)x2 - (int)x1 + 1).ToList();
                    var yValues = ((LineSeries<ObservableValue>)Series[1]).Values.Select(x => x.Value).ToList().Skip((int)x1).Take((int)x2 - (int)x1 + 1).ToList();
                    var zValues = ((LineSeries<ObservableValue>)Series[2]).Values.Select(x => x.Value).ToList().Skip((int)x1).Take((int)x2 - (int)x1 + 1).ToList();


                    getXYZ_range(xValues, yValues, zValues);
                }

                isSelected = false;


                Debug.Write("Selected strat x1: " + x1 + "\n");
                Debug.Write("Selected stop x2: " + x2);
                try
                {
                    Sections[0].Xi = x1;
                    Sections[0].Xj = x2;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Selected error: " + ex.ToString());
                }
                FinishSelected?.Invoke(this, new EventArgs());
            }

        }
        public event EventHandler FinishSelected;


        public ObservableCollection<double> afterSelected_a { get; set; }
        public ObservableCollection<double> afterSelected_F { get; set; }

        private void getXYZ_range(List<double?> x, List<double?> y, List<double?> z)
        {
            if (x.Count != y.Count || x.Count != z.Count) return;
            // Chuyển một list có thể có giá trị null sang một list không có giá trị null
            var nonNullxValue = x.Where(value => value.HasValue).Select(value => value.Value).ToList();
            var nonNullyValue = y.Where(value => value.HasValue).Select(value => value.Value).ToList();
            var nonNullzValue = z.Where(value => value.HasValue).Select(value => value.Value).ToList();

            this.xValues = new ObservableCollection<double>(nonNullxValue);
            this.yValues = new ObservableCollection<double>(nonNullyValue);
            this.zValues = new ObservableCollection<double>(nonNullzValue);

            afterSelected_F = new ObservableCollection<double>();
            afterSelected_a = new ObservableCollection<double>();

            for (int i = 0; i < xValues.Count; i++)
            {
                afterSelected_a.Add(Math.Sqrt(xValues[i] * xValues[i] + yValues[i] * yValues[i] + zValues[i] * zValues[i]));
            }

            if (afterSelected_a.Count != xValues.Count) return;

            foreach (var value in afterSelected_a)
            {
                afterSelected_F.Add(this.M * value);
            }

            Debug.WriteLine("after select" + afterSelected_a.Count + " " + afterSelected_F.Count);

        }
    }
}
