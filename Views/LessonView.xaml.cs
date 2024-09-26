
using MAUI_IOT.ViewModels;
using Microsoft.Maui.Layouts;
using System.Diagnostics;
using MAUI_IOT.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Globalization;
using UraniumUI.ViewExtensions;
using SkiaSharp.Views.Maui;
using Microsoft.Maui.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.Drawing;
using SkiaSharp;
using MAUI_IOT.Services.Interfaces;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using LiveChartsCore.SkiaSharpView;
using CommunityToolkit.Maui.Views;
using LiveChartsCore.SkiaSharpView.SKCharts;
using Microcharts;
using MAUI_IOT.Models.Data;
using Syncfusion.Maui.DataSource.Extensions;
namespace MAUI_IOT.Views;

public partial class LessonView : ContentPage
{
    private LessonnViewModel _lessonnViewModel;
    private List<String> Series = new List<string>() { "Series_X", "Series_Y" };

    public LessonView(LessonnViewModel lessonnViewModel)
    {
        InitializeComponent();
        this._lessonnViewModel = lessonnViewModel;
        BindingContext = lessonnViewModel;

        table_data1.HeightRequest = (DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density) * 0.5;
        TongHop.WidthRequest = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density) * 1;
        charts2.WidthRequest = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density) * 1;
        Chitiet.WidthRequest = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density) * 1;

        // chart table 
        GenarateAnalyzeChart();
        GenarateGridWithCharts(Series, Series.Count, true);

        List<string> packetName = new List<string>();
        editBtn.ViewModel = lessonnViewModel;

        listData.IsVisible = false;

    }





    private void Tabinit()
    {
        // tab_View.SelectedTab = Config;
    }

    private void OnShowPopupClicked(object sender, EventArgs e)
    {
        var popup = new AnalyzePopup(_lessonnViewModel);
        this.ShowPopup(popup);

    }
    private void GenarateAnalyzeChart()
    {
        charts2.Clear();
        CartesianChart cartesianChart2 = new CartesianChart
        {
            Padding = new Thickness(0, 0, 0, 0),
            Margin = new Thickness(0, 0, 0, 0),
            ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.None,
            MinimumHeightRequest = (DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density) * 0.7
        };
        //  charts2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        Button popupButton = new Button
        {
            FontFamily = "FaBrands",
            TextColor = Colors.Black,
            Text = Models.FaBrandIcon.ChartLine,
            FontSize = 40,
            Padding = 0,
            Margin = new Thickness(0, 20, 20, 0),
            HeightRequest = 50,
            WidthRequest = 50,
            HorizontalOptions = LayoutOptions.End,
            BackgroundColor = Colors.Transparent,

        };

        Grid grid2 = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(30,GridUnitType.Absolute) },
                new RowDefinition { Height = new GridLength(30,GridUnitType.Absolute) },
                new RowDefinition { Height = new GridLength(30,GridUnitType.Absolute) },
                new RowDefinition { Height = new GridLength(30,GridUnitType.Absolute) },
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) }
            },
            HorizontalOptions = LayoutOptions.End,
            Margin = new Thickness(0, 20, 20, 0),
            IsVisible = false,
            BackgroundColor = Colors.Cyan,
            HeightRequest = 150,
            VerticalOptions = LayoutOptions.Start,
            Padding = 10

        };  // grid chứa tuỳ chọn line 
        bool isClicked = false;
        popupButton.Clicked += (sender, e) =>
        {
            if (isClicked == true)
            {
                popupButton.BackgroundColor = Colors.Gray;
                grid2.IsVisible = true;
            }
            else
            {
                popupButton.BackgroundColor = Colors.Transparent;
                grid2.IsVisible = false;

            }
            isClicked = !isClicked;
        };
       
        CheckBox checkBox1 = new CheckBox
        {
            IsChecked = false,
        };
        CheckBox checkBox2 = new CheckBox
        {
            IsChecked = false,
        };
        CheckBox checkBox3 = new CheckBox
        {
            IsChecked = false,
        };
        CheckBox checkBox4 = new CheckBox
        {
            IsChecked = false,
        };

        Label dothi1 = new Label()
        {
            Text = "Đồ Thị 1",
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold,
            FontSize = 10
        };
        Label dothi2 = new Label()
        {
            Text = "Đồ Thị 2",
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold,
            FontSize = 10
        };
        Label dothi3 = new Label()
        {
            Text = "Đồ Thị 3",
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold,
            FontSize = 10
        };
        Label dothi4 = new Label()
        {
            Text = "Đồ Thị 4",
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold,
            FontSize = 10
        };
        grid2.Children.Add(dothi1);
        grid2.Children.Add(dothi2);
        grid2.Children.Add(dothi3);
        grid2.Children.Add(dothi4);
        grid2.Children.Add(checkBox1);
        grid2.Children.Add(checkBox2);
        grid2.Children.Add(checkBox3);
        grid2.Children.Add(checkBox4);
        grid2.SetRow(checkBox1, 0);
        grid2.SetRow(checkBox2, 1);
        grid2.SetRow(checkBox3, 2);
        grid2.SetRow(checkBox4, 3);
        grid2.SetColumn(checkBox1, 0);
        grid2.SetColumn(checkBox2, 0);
        grid2.SetColumn(checkBox3, 0);
        grid2.SetColumn(checkBox4, 0);
        grid2.SetColumn(dothi1, 1);
        grid2.SetColumn(dothi2, 1);
        grid2.SetColumn(dothi3, 1);
        grid2.SetColumn(dothi4, 1);
        grid2.SetRow(dothi1, 0);
        grid2.SetRow(dothi2, 1);
        grid2.SetRow(dothi3, 2);
        grid2.SetRow(dothi4, 3);

        // Set up binding for chart
        cartesianChart2.SetBinding(CartesianChart.SeriesProperty, new Binding { Path = "Series_Summarize", Mode = BindingMode.OneWay });
        cartesianChart2.SetBinding(CartesianChart.SyncContextProperty, new Binding { Path = "Sync", Mode = BindingMode.OneWay });
        cartesianChart2.SetBinding(CartesianChart.XAxesProperty, new Binding { Path = "XAxes", Mode = BindingMode.OneWay });
        cartesianChart2.SetBinding(CartesianChart.YAxesProperty, new Binding { Path = "YAxes", Mode = BindingMode.OneWay });
        cartesianChart2.SetBinding(CartesianChart.DrawMarginFrameProperty, new Binding { Path = "Frame", Mode = BindingMode.OneWay });

        checkBox1.SetBinding(CheckBox.IsCheckedProperty, new Binding { Path = "IsCheckLine_1", Mode = BindingMode.TwoWay });
        checkBox2.SetBinding(CheckBox.IsCheckedProperty, new Binding { Path = "IsCheckLine_2", Mode = BindingMode.TwoWay });
        checkBox3.SetBinding(CheckBox.IsCheckedProperty, new Binding { Path = "IsCheckLine_3", Mode = BindingMode.TwoWay });
        checkBox4.SetBinding(CheckBox.IsCheckedProperty, new Binding { Path = "IsCheckLine_4", Mode = BindingMode.TwoWay });

        charts2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        charts2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
        charts2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        charts2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        charts2.Children.Add(popupButton);
        charts2.Children.Add(cartesianChart2);
        charts2.Children.Add(grid2);
        charts2.SetRow(cartesianChart2, 1);
        charts2.SetColumn(cartesianChart2, 0);
        charts2.SetColumnSpan(cartesianChart2, 2);
        charts2.SetColumnSpan(grid2, 2);
        charts2.SetColumn(grid2, 0);
        charts2.SetRow(grid2, 1);
        charts2.SetRow(popupButton, 0);
        charts2.SetColumn(popupButton, 1);

        checkBox1.PropertyChanged += (s, e) =>
        {
            Debug.WriteLine("State of checkBox_1: " + checkBox1.IsChecked);
        };
    }


    //private void OnShowAddChartLinePopupClicked(object sender, EventArgs e)
    //{
    //    var popup = new AddChartLinePopup(_lessonnViewModel);
    //    this.ShowPopup(popup);
    //}

    //load dữ liệu 
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
        Debug.WriteLine("Data list clicked " + _lessonnViewModel.Experiment_database.Count);
        CollectionView.ItemsSource = new ObservableCollection<Experiment>();
        try
        {
            CollectionView.ItemsSource = _lessonnViewModel.Experiment_database;
            Debug.WriteLine("Data list clicked " + _lessonnViewModel.Experiment_database.Count);
            foreach (Experiment exp in CollectionView.ItemsSource.ToList<Experiment>()) {
                Debug.WriteLine("Data: " + exp.ExperimentName);
            }
        }
        catch (Exception ex) { 
            Debug.WriteLine("Error when loading data" + ex.Message.ToString());
        }
    }
}