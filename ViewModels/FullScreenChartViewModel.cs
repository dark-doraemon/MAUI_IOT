using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Collections.ObjectModel;
using SkiaSharp;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Windows.Input;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.Defaults;

namespace MAUI_IOT.ViewModels
{
    [QueryProperty(nameof(Series), "data")]
    public partial class FullScreenChartViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<ISeries> series;

        [ObservableProperty]
        private bool isButtonEnabled = true;

        [ObservableProperty]
        private string buttonText = "Select range";

        [ObservableProperty]
        private List<ObservableValue> selectedValue = new List<ObservableValue>();

        double x1 = -10;
        double x2 = -10;

        public ICommand PointerPressedCommand { get; }

        private static readonly SKColor s_gray = new(195, 195, 195);
        private static readonly SKColor s_gray1 = new(160, 160, 160);
        private static readonly SKColor s_gray2 = new(90, 90, 90);
        private static readonly SKColor s_dark3 = new(60, 60, 60);

        private bool isSelectingRange = false;

        public FullScreenChartViewModel()
        {
            PointerPressedCommand = new RelayCommand<PointerCommandArgs>(OnPointerPressed);

        }
        
        private void OnPointerPressed(PointerCommandArgs e)
        {

            if(isSelectingRange == true)
            {
                var chart = (ICartesianChartView<SkiaSharpDrawingContext>)e.Chart;
                var scaledPoint = chart.ScalePixelsToData(e.PointerPosition);

                if(x1 == -10)
                {
                    x1 = Math.Floor(scaledPoint.X);
                    if(x1 < 0)
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

                if(x2 > x1)
                {
                    this.SelectedValue.Clear();
                    foreach (var seriesItem in series)
                    {
                        if (seriesItem is LineSeries<ObservableValue> lineSeries)
                        {
                            var values = lineSeries.Values.OfType<ObservableValue>().ToList();

                            // Lấy giá trị trong khoảng từ sampleIndex1 đến sampleIndex2
                            selectedValue = lineSeries.Values.OfType<ObservableValue>().ToList()
                                .Skip((int)x1).Take((int)x2 - (int)x1 + 1).ToList();

                            foreach (var v in selectedValue)
                            {
                                Console.WriteLine($"Value: {v.Value}");
                            }
                        }
                    }
                    x1 = -10;
                    x2 = -10;
                }

                isSelectingRange = false;
                this.ButtonText = "Select range";
                this.IsButtonEnabled = true;
            }

        }

        


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
                }
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
        public void SelectRange()
        {
            if (series.Count < 1)
            {
                Debug.WriteLine("Không có dữ liệu");
            }

            else
            {
                this.isSelectingRange = true;
                this.IsButtonEnabled = false;
                this.ButtonText = "Selecting";
            }
        }


    }
}
