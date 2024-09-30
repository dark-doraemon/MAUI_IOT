
using MAUI_IOT.ViewModels;
using System.Diagnostics;
using LiveChartsCore.SkiaSharpView.Maui;
using CommunityToolkit.Maui.Views;

namespace MAUI_IOT.Views;

public partial class LessonView : ContentPage
{
    private LessonnViewModel _lessonnViewModel;
    private List<String> Series = new List<string>() { "Series_X", "Series_Y" };

    public LessonView(LessonnViewModel lessonnViewModel)
    {
        InitializeComponent();
        this._lessonnViewModel = lessonnViewModel;
        this.BindingContext = lessonnViewModel;

        GenarateGridWithCharts(Series, Series.Count, true);

        List<string> packetName = new List<string>();
        editBtn.ViewModel = lessonnViewModel;

        listData.IsVisible = false;
        listAllFile.IsVisible = false;
    }

    private void GenarateGridWithCharts(List<string> listSeries, int rowNumber, bool isMaximize)
    {
        //Restart grid
        charts.Clear();

        //Create a row in grid
        for (int i = 0; i < listSeries.Count; i++)
        {
            charts.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        StackLayout stack = new StackLayout
        {
            Spacing = 30,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End,
            Orientation = StackOrientation.Horizontal,
        };

        Button buttonStart = new Button
        {
            Text = "Start",
        };

        Button buttonStop = new Button
        {
            Text = "Stop",
        };

        charts.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
        //Binding
        var IsEnableStart = new Binding
        {
            Path = "IsEnableButtonStart",
            Mode = BindingMode.OneWay,
        };
        buttonStart.SetBinding(Button.IsEnabledProperty, IsEnableStart);

        var BackgroundColorStart = new Binding
        {
            Path = "ColorButtonStart",
            Mode = BindingMode.OneWay,
        };
        buttonStart.SetBinding(Button.BackgroundColorProperty, BackgroundColorStart);

        var StartCommand = new Binding
        {
            Path = "StartCommand",
            Mode = BindingMode.OneWay,
        };
        buttonStart.SetBinding(Button.CommandProperty, StartCommand);

        var IsEnableStop = new Binding
        {
            Path = "IsEnableButtonStop",
            Mode = BindingMode.OneWay,
        };
        buttonStop.SetBinding(Button.IsEnabledProperty, IsEnableStop);

        var BackgroundColorStop = new Binding
        {
            Path = "ColorButtonStop",
            Mode = BindingMode.OneWay,
        };
        buttonStop.SetBinding(Button.BackgroundColorProperty, BackgroundColorStop);

        var StopCommand = new Binding
        {
            Path = "StopCommand",
            Mode = BindingMode.OneWay,
        };
        buttonStop.SetBinding(Button.CommandProperty, StopCommand);

        stack.Children.Add(buttonStart);
        stack.Children.Add(buttonStop);


        //Add chart and button in grid
        for (int i = 0; i < listSeries.Count; i++)
        {
            var chartGrid = GenerateChart(listSeries, listSeries.Count, i, isMaximize); // Tạo một Grid chứa Chart và nút

            charts.Children.Add(chartGrid);

            charts.SetRow(chartGrid, i);
        }

        charts.Padding = new Thickness(0, 10, 0, 0);
        charts.Children.Add(stack);
        charts.SetRow(stack, rowNumber);
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        listAllFile.IsVisible =  !listAllFile.IsVisible;
        listData.IsVisible = !listAllFile.IsVisible;
    }

    private Grid GenerateChart(List<string> listSeries, int rowNumber, int chartIndex, bool isMaximize)
    {

        Grid grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(10) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            },
        };

        //Create a button above the chart
        Button button;

        //Toggle maximize and minimize
        if (isMaximize)
        {
            button = new Button
            {
                FontFamily = "FaBrands",
                TextColor = Colors.Black,
                Text = Models.FaBrandIcon.Maximize,
                FontSize = 20,
                Padding = 0,
                Margin = new Thickness(40, 0, 0, 0),
                HeightRequest = 20,
                WidthRequest = 20,
                HorizontalOptions = LayoutOptions.Start,
                BackgroundColor = Colors.Transparent,
                IsEnabled = true,
                IsVisible = true
            };
            button.Clicked += (sender, e) =>
            {
                var newListSeries = new List<string>()
                {
                    listSeries[chartIndex],
                };
                GenarateGridWithCharts(newListSeries, newListSeries.Count, false);
                Debug.WriteLine("Seriex" + Series.Count);
            };
        }
        else
        {
            button = new Button
            {
                FontFamily = "FaBrands",
                TextColor = Colors.Black,
                Text = Models.FaBrandIcon.Minimize,
                FontSize = 20,
                Padding = 0,
                Margin = new Thickness(40, 0, 0, 0),
                HeightRequest = 20,
                WidthRequest = 20,
                HorizontalOptions = LayoutOptions.Start,
                BackgroundColor = Colors.Transparent
            };
            button.Clicked += (sender, e) =>
            {
                GenarateGridWithCharts(Series, Series.Count, true);
                Debug.WriteLine("Serie: " + Series.Count);
            };
        }
        //Button close chart
        Button xmarks = new Button
        {
            FontFamily = "FaBrands",
            TextColor = Colors.Black,
            Text = Models.FaBrandIcon.Xmark,
            FontSize = 25,
            Padding = 0,
            Margin = new Thickness(40, 0, 0, 0),
            HeightRequest = 20,
            WidthRequest = 20,
            HorizontalOptions = LayoutOptions.End,
            BackgroundColor = Colors.Transparent,
        };

        //Chart
        CartesianChart cartesianChart = new CartesianChart
        {
            Padding = new Thickness(0, 0, 0, 0),
            Margin = new Thickness(0, 0, 0, 0),
            ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.None,
            MinimumHeightRequest = (DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density) * 0.7 / (rowNumber),
        };

        // Set up binding for chart
        cartesianChart.SetBinding(CartesianChart.SeriesProperty, new Binding { Path = listSeries[chartIndex], Mode = BindingMode.OneWay });
        cartesianChart.SetBinding(CartesianChart.SyncContextProperty, new Binding { Path = "Sync", Mode = BindingMode.OneWay });
        cartesianChart.SetBinding(CartesianChart.XAxesProperty, new Binding { Path = "XAxes", Mode = BindingMode.OneWay });
        cartesianChart.SetBinding(CartesianChart.YAxesProperty, new Binding { Path = "YAxes", Mode = BindingMode.OneWay });
        cartesianChart.SetBinding(CartesianChart.DrawMarginFrameProperty, new Binding { Path = "Frame", Mode = BindingMode.OneWay });

        //Set up binding for button
        //maximize.SetBinding(Button.CommandProperty, new Binding { Path = "ZoomCommand", Mode = BindingMode.OneWay });
        //maximize.SetBinding(Button.CommandParameterProperty, new Binding { Path = listSeries[chartIndex], Mode = BindingMode.OneWay 

        //Handel event
        xmarks.Clicked += (sender, e) =>
        {
            listSeries.RemoveAt(chartIndex);
            GenarateGridWithCharts(listSeries, rowNumber, true);
        };

        // Add element into grid
        grid.Children.Add(button);
        grid.SetRow(button, 0);
        grid.SetColumn(button, 0);

        grid.Children.Add(xmarks);
        grid.SetRow(xmarks, 0);
        grid.SetColumn(xmarks, 1);

        grid.Children.Add(cartesianChart);
        grid.SetRow(cartesianChart, 1);
        grid.SetColumnSpan(cartesianChart, 2);
        grid.SetColumn(cartesianChart, 0);

        return grid;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        listData.IsVisible = !listData.IsVisible;
        listAllFile.IsVisible = !listData.IsVisible;
    }
}