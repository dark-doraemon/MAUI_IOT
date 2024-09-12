using LiveChartsCore.Kernel;
using MAUI_IOT.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace MAUI_IOT.Views;

public partial class FullScreenChartView : ContentPage
{
    private readonly FullScreenChartViewModel fullScreenChartViewModel;
    //private SKCanvasView canvasView;

    public FullScreenChartView(FullScreenChartViewModel fullScreenChartViewModel)
    {   
        InitializeComponent();
        BindingContext = fullScreenChartViewModel;

    }

  


}