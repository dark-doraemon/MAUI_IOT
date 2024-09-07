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
namespace MAUI_IOT.Views;

public partial class LessonView : ContentPage
{
    private LessonnViewModel _lessonnViewModel;
    private List<String> Series = new List<string>() { "Series_X", "Series_Y", "Series_Z" };

    public LessonView(LessonnViewModel lessonnViewModel)
    {
        InitializeComponent();
        this._lessonnViewModel = lessonnViewModel;
        BindingContext = lessonnViewModel;
        //   Tabinit();
        weight_entry.Text = lessonnViewModel.M.ToString();
        weight_entry.Focused += (sender, e) =>
        {
            weight_entry.Text = "";
            weight_entry.Focus();
        };

        weight_entry.Unfocused += async (sender, e) =>
        {
            if (string.IsNullOrEmpty(weight_entry.Text))
            {
                weight_entry.Text = "0";
                await DisplayAlert("Thông báo", "Vui lòng nhập giá trị", "Nhập lại");
                weight_entry.Focus();
            }
        };
        weight.Clicked += async (sender, e) =>
        {
            if (string.IsNullOrEmpty(weight_entry.Text))
            {
                weight_entry.Text = "0";
                await DisplayAlert("Thông báo", "Vui lòng nhập giá trị", "Nhập lại");
                return;
            }
            if (await DisplayAlert("Thông báo", $"Xác nhận giá trị m = {weight_entry.Text.ToString()} kg", "Lưu", "Nhập lại"))
            {
                double weight = double.Parse(weight_entry.Text);
                if (string.IsNullOrEmpty(weight_entry.Text) || weight <= 0)
                {
                    await DisplayAlert("Thông báo!", "Vui lòng nhập khối lượng hợp lệ", "OK");
                    lessonnViewModel.IsValidEntryWeight = false;
                    tab_View.SelectedTab = Config;
                    return;
                }
                weight_entry.Unfocus();
                weight_entry.IsEnabled = false;
                weight_entry.IsEnabled = true;
                //   FormatWeightEntry();
                //    tab_View.SelectedTab = Experiment;
                lessonnViewModel.IsValidEntryWeight = true;
            }
            else
            {
                weight_entry.Focus();
            }
        };
        GenarateGridWithCharts(Series, Series.Count, true);

        Picker myPicker = this.FindByName<Picker>("myPicker");

        if (myPicker != null)
        {
            myPicker.Title = "Chọn 1 mục ";
        }
        List<string> packetName = new List<string>();
        for (int i = 0; i < lessonnViewModel.FileCount; i++)
        {
            packetName.Add($"Experiment{i}");
        }
        myPicker.ItemsSource = packetName;
        myPicker.SelectedIndexChanged += (sender, e) =>
        {
            var selectedItem = (sender as Picker)?.SelectedItem;
            getdata(selectedItem);
        };
        Button myButton = this.FindByName<Button>("myButton");
        if (myButton != null)
        {
            myButton.Clicked += MyButton_Clicked;
        }
    }


    private void MyButton_Clicked(object sender, EventArgs e)
    {
        var selectedItem = myPicker.SelectedItem;
        if (selectedItem != null)
        {
            _lessonnViewModel.Save(selectedItem.ToString());

        }

    }

    private void addPickerItem(object sender, EventArgs e)
    {
        List<string> packetName = new List<string>();
        for (int i = 0; i < _lessonnViewModel.FileCount; i++)
        {
            packetName.Add($"Experiment{i}");
        }
        myPicker.ItemsSource = packetName;

    }



    private void createToolbar(int n)
    {
        if (n > 10)
        {
            DisplayAlert("Warming", "Đã đạt giới hạn", "Thoát");
            return;
        }
        var toolbarItem = new ToolbarItem
        {
            Text = $" save  {n}",
            Order = ToolbarItemOrder.Secondary,
            Command = ((LessonnViewModel)BindingContext).SaveCommand, // Gán Command
            CommandParameter = n,
        };

        ToolbarItems.Add(toolbarItem);
    }
    private void FormatWeightEntry()
    {
        double temp = 0;
        try
        {
            string entryAfterFormatted = weight_entry.Text.Replace(",", ".");
            if (double.TryParse(entryAfterFormatted, NumberStyles.Any, CultureInfo.InvariantCulture, out temp))
            {
                _lessonnViewModel.M = temp;

                if (temp < 0)
                {
                    _lessonnViewModel.M = 0;
                    weight_entry.Text = "0";
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString() + "Format entry");
        }
    }

    private void Tabinit()
    {
        // tab_View.SelectedTab = Config;
    }


    //load dữ liệu 

    private async Task getdata(object selectedItem)
    {
        try
        {
            await _lessonnViewModel.Load(selectedItem.ToString());
            if (_lessonnViewModel.FileContent.Length != 0)
            {
                //     await DisplayAlert("Tiêu đề", _lessonnViewModel.FileContent, "Thoát");
                weight_entry.Text = _lessonnViewModel.M.ToString();
            }
            else
            {
                //     await DisplayAlert("Tiêu đề", $"file chưa lưu  ", "thoát");

            }
        }
        catch (Exception ex)
        {

            Debug.WriteLine("======================================Try catch Lessonview.xaml.cs===================================================");
            Debug.WriteLine(ex.Message.ToString());
            Debug.WriteLine("=======================================================================================================");


        }



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

    private void swapAxes()
    {

    }
}