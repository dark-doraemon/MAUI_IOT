﻿using MAUI_IOT.ViewModels;
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
namespace MAUI_IOT.Views;

public partial class LessonView : ContentPage
{
    public LessonViewModel lessonViewModel;

    public ObservableCollection<double> a = new ObservableCollection<double>();
    public ObservableCollection<double> F = new ObservableCollection<double>();
    public ObservableCollection<TimeSpan> Duration = new ObservableCollection<TimeSpan>();

    private const string result_textcolor = "#FFFFFF";
    private const string backgroundColor_odd = "#2C3034";
    private const string backgroundColor_even = "#212529";

    private bool isEntryM = false;
    private bool isSave = false;
    private bool isStop = false;
    private bool isStart = false;

    private event EventHandler SetM;

    private string path;
    public LessonView(LessonViewModel lessonViewModel)
    {
        InitializeComponent();
        Tabinit();
        this.lessonViewModel = lessonViewModel;
        BindingContext = lessonViewModel;
        lessonViewModel.OnStart += Handle_Onstart;
        lessonViewModel.OnStop += Handle_Onstop;
        lessonViewModel.OnSave += Handle_OnSave;
        tab_View.PropertyChanged += OnTabView;
        lessonViewModel.FinishSelected += Handle_Selecting;
        lessonViewModel.isSelected = false;
    }

    private void OnClick(object sender, EventArgs e) {
        Debug.WriteLine(lessonViewModel.isSelected);
        lessonViewModel.isSelected = !lessonViewModel.isSelected;
        Debug.WriteLine($"Selected {lessonViewModel.isSelected}");
        if (lessonViewModel.isSelected)
        {
            selectChartAll.Text = "Selecting";
            Debug.WriteLine("Select True");
        }
        else
        {
            selectChartAll.Text = "Select Range";
            Debug.WriteLine("Select False");
        }
    }
    private async void Handle_OnSave(object sender, EventArgs e)
    {
        if (!isStop) {
            await DisplayAlert("Thông báo", "Chưa có dữ liệu", "OK");
            return;
        }

        if (await ReadData(lessonViewModel.path))
        {
            await DisplayAlert("Thông báo!", "Lưu dữ liệu thành công", "OK");
            isSave = true;
            try
            {
                //a = lessonViewModel.a;
                //F = lessonViewModel.F;
                //Duration = lessonViewModel.Duration;
                //await BindingData(table, a.Count, Duration, a, F);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ReadData: " + ex.ToString());
            }

            EnabledButton(false);
            tab_View.SelectedTab = table_results;
        }
        else
        {
            await DisplayAlert("Thông báo!", "Lưu dữ liệu thất bại", "OK");
        }
    }
    private async void Handle_Onstop(object sender, EventArgs e)
    {
        if (isStart)
        {
            isStop = true;
            a = lessonViewModel.a;
            F = lessonViewModel.F;
            Duration = lessonViewModel.Duration;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await BindingData(table, a.Count, Duration, a, F);
            });
            btn_Save.IsEnabled = true;
            btn_Start.IsEnabled = true;
            btn_Start.BackgroundColor = Color.FromHex("#052959");
            btn_Save.BackgroundColor = Color.FromHex("#052959");
        }
    }
    private void Handle_Onstart(object sender, EventArgs e)
    {

        if (isEntryM)
        {
            isStart = true;
            btn_Start.IsEnabled = false;
            btn_Save.IsEnabled = false;
            btn_Start.BackgroundColor = Color.FromHex("#5F6265");
            btn_Save.BackgroundColor = Color.FromHex("#5F6265");
        }
        else
        {
            DisplayAlert("Thông báo!", "Bạn phải nhập khối lượng!", "Xác nhận");
            tab_View.SelectedTab = inputParameters;
        }
    }

    // tạo ra dữ liệu cho bảng dữ liệu
    private async Task BindingData(Grid table, int rows, ObservableCollection<TimeSpan> Duration, ObservableCollection<double> a, ObservableCollection<double> F)
    {
        double tableWidth = DeviceDisplay.MainDisplayInfo.Width / 9;

        table.ColumnDefinitions[0].Width = tableWidth;
        table.ColumnDefinitions[1].Width = tableWidth;
        table.ColumnDefinitions[2].Width = tableWidth;

        for (int i = 1; i < rows; i++)
        {

            table.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }

        for (int i = 1; i < rows; i++)
        {
            var label1 = new Label
            {
                Text = $"{Duration[i].TotalSeconds:F2}" + " s",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex(result_textcolor),
                FontSize = 15
            };
            table.Children.Add(label1);
            table.SetRow(label1, i);
            table.SetColumn(label1, 0);

            var label2 = new Label
            {
                Text = $"{a[i]:F2}" + " m/s",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex(result_textcolor),
                FontSize = 15
            };
            table.Children.Add(label2);
            table.SetRow(label2, i);
            table.SetColumn(label2, 1);

            var label3 = new Label
            {
                Text = $"{F[i]:F2}" + " N",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex(result_textcolor),
                FontSize = 15
            };
            table.Children.Add(label3);
            table.SetRow(label3, i);
            table.SetColumn(label3, 2);

            //chọn màu cho từng ô
            if (i % 2 == 0)
            {
                label1.BackgroundColor = Color.FromHex(backgroundColor_even);
                label2.BackgroundColor = Color.FromHex(backgroundColor_even);
                label3.BackgroundColor = Color.FromHex(backgroundColor_even);
            }
            else
            {
                label1.BackgroundColor = Color.FromHex(backgroundColor_odd);
                label2.BackgroundColor = Color.FromHex(backgroundColor_odd);
                label3.BackgroundColor = Color.FromHex(backgroundColor_odd);
            }
        }

        table.RowDefinitions.Add(new RowDefinition());
        var label = new Label
        {
            Text = $"AVG F = {Math.Round(F.Average(), 3)} (N)",
            HorizontalTextAlignment = TextAlignment.Center,
            FontAttributes = FontAttributes.Bold,
            FontSize = 15
        };
        table.SetRow(label, rows);
        table.SetColumn(label, 2);
        table.Children.Add(label);
    }

    //đọc file sau khi lưu
    private async Task<bool> ReadData(string path)
    {
        try
        {

            var jsonData = await File.ReadAllTextAsync(path);
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);

            if (data != null)
            {
                var aList = JsonConvert.DeserializeObject<List<double>>(data["a"].ToString());
                var FList = JsonConvert.DeserializeObject<List<double>>(data["F"].ToString());
                var durationList = JsonConvert.DeserializeObject<List<TimeSpan>>(data["Duration"].ToString());
                var mData = data["m"];

                this.path = path;
                return true;
            }
            else
            {
                Debug.Write("Can't read this file");
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ReadFile Error: {ex.Message}");
            return false;
        }
    }

    //Kiểm tra phần nhập vào khố
    //i lượng
    private async void Handle_Set(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Xác nhận!", $"Khối lượng m = {weight_entry.Text} kg", "Lưu", "Nhập lại");

        if(weight_entry.Text == String.Empty)
        {
            await DisplayAlert("Thông báo", "Khối lượng không hợp lệ", "OK");
            return;
        }

        double temp = lessonViewModel.M;
        if (answer) {
            string entryAfterFormatted = weight_entry.Text.Replace(",", ".");
            if (double.TryParse(entryAfterFormatted, NumberStyles.Any, CultureInfo.InvariantCulture, out temp))
            {
                lessonViewModel.M = temp;
            }
            tab_View.SelectedTab = Experiment;
        }

        weight_entry.Text = temp.ToString();

        isEntryM = true;

    }

    //Kiểm tra đang ở tab nào
    public async void OnTabView(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //if ("Table".Equals(tab_View.SelectedTab.Title))
        //{
        //    if (!isSave)
        //    {
        //        await DisplayAlert("Thông báo!", "Chưa có dữ liệu", "OK");
        //        tab_View.SelectedTab = Experiment;
        //    }
        if ("Table".Equals(tab_View.SelectedTab.Title))
        {
            MainThread.BeginInvokeOnMainThread(async ()=> { table.IsVisible = false; table.IsVisible = true; });
        }

        if ("All".Equals(tab_View.SelectedTab.Title))
        {
            Debug.WriteLine("Tab All");
            if (!isStart)
            {
                if (!isStop)
                {
                    await DisplayAlert("Thông báo!", "Chưa bắt đầu thí nghiệm", "OK");
                    tab_View.SelectedTab = Experiment;
                }
            }
        }
    }

    private void EnabledButton(bool value)
    {
        if (value)
        {
            weight.IsEnabled = true;
            weight_entry.IsEnabled = true;
            btn_Stop.IsEnabled = true;
            btn_Save.IsEnabled = true;
            btn_Start.IsEnabled = true;
        }
        else
        {
            weight.IsEnabled = false;
            weight_entry.IsEnabled = false;
            btn_Save.IsEnabled = false;
            btn_Start.IsEnabled = false;
            btn_Stop.IsEnabled = false;
        }
    }

    private async void loadDataTable(string path)
    {
        if (!(await ReadData(path)))
        {
            isSave = true;

            await BindingData(table, a.Count, Duration, a, F);
            Debug.Write("loadData");
        }
    }

    private async Task BindingDataAfterSelected(Grid table, int rows, ObservableCollection<double> a, ObservableCollection<double> F)
    {

        Debug.WriteLine("Hello new Table");
        double tableWidth = DeviceDisplay.MainDisplayInfo.Width / 9;

        table.Clear();

        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        // Định nghĩa các hàng
        table.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        // Tạo các Label
        var timeLabel = new Label
        {
            Padding = new Thickness(5),
            FontAttributes = FontAttributes.Bold,
            FontSize = 25,
            HorizontalTextAlignment = TextAlignment.Center,
            Text = "Time",
            VerticalTextAlignment = TextAlignment.Center
        };
        Grid.SetRow(timeLabel, 0);
        Grid.SetColumn(timeLabel, 0);

        var velocityLabel = new Label
        {
            Padding = new Thickness(5),
            FontAttributes = FontAttributes.Bold,
            FontSize = 25,
            HorizontalTextAlignment = TextAlignment.Center,
            Text = "v(m/s^2)",
            VerticalTextAlignment = TextAlignment.Center
        };
        Grid.SetRow(velocityLabel, 0);
        Grid.SetColumn(velocityLabel, 1);

        var forceLabel = new Label
        {
            Padding = new Thickness(5),
            FontAttributes = FontAttributes.Bold,
            FontSize = 25,
            HorizontalTextAlignment = TextAlignment.Center,
            Text = "F (N)",
            VerticalTextAlignment = TextAlignment.Center
        };
        Grid.SetRow(forceLabel, 0);
        Grid.SetColumn(forceLabel, 2);

        table.Children.Add(timeLabel);
        table.Children.Add(velocityLabel);
        table.Children.Add(forceLabel);

        table.ColumnDefinitions[0].Width = tableWidth;
        table.ColumnDefinitions[1].Width = tableWidth;
        table.ColumnDefinitions[2].Width = tableWidth;

        for (int i = 1; i < rows; i++)
        {
            table.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }

        for (int i = 1; i < rows; i++)
        {
            var label1 = new Label
            {
                Text = $"" + "",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex(result_textcolor),
                FontSize = 15
            };
            table.Children.Add(label1);
            table.SetRow(label1, i);
            table.SetColumn(label1, 0);

            var label2 = new Label
            {
                Text = $"{a[i]:F2}" + " m/s",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex(result_textcolor),
                FontSize = 15
            };
            table.Children.Add(label2);
            table.SetRow(label2, i);
            table.SetColumn(label2, 1);

            var label3 = new Label
            {
                Text = $"{F[i]:F2}" + " N",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex(result_textcolor),
                FontSize = 15
            };
            table.Children.Add(label3);
            table.SetRow(label3, i);
            table.SetColumn(label3, 2);

            //chọn màu cho từng ô
            if (i % 2 == 0)
            {
                label1.BackgroundColor = Color.FromHex(backgroundColor_even);
                label2.BackgroundColor = Color.FromHex(backgroundColor_even);
                label3.BackgroundColor = Color.FromHex(backgroundColor_even);
            }
            else
            {
                label1.BackgroundColor = Color.FromHex(backgroundColor_odd);
                label2.BackgroundColor = Color.FromHex(backgroundColor_odd);
                label3.BackgroundColor = Color.FromHex(backgroundColor_odd);
            }
        }

        table.RowDefinitions.Add(new RowDefinition());
        var label = new Label
        {
            Text = $"AVG F = {Math.Round(F.Average(), 3)} (N)",
            HorizontalTextAlignment = TextAlignment.Center,
            FontAttributes = FontAttributes.Bold,
            FontSize = 15
        };
        table.SetRow(label, rows);
        table.SetColumn(label, 2);
        table.Children.Add(label);
        table.IsVisible = false;
    }

    private async void Handle_Selecting(object sender, EventArgs e)
    {
        selectChartAll.Text = "Select Range";
        lessonViewModel.isSelected = false;

        if (isStop)
        {
            Debug.WriteLine("Render table success");
            a = lessonViewModel.afterSelected_a;
            F = lessonViewModel.afterSelected_F;          
            await BindingDataAfterSelected(table, a.Count, a, F);
        }
    }

    private void Tabinit()
    {
        tab_View.SelectedTab = inputParameters;
    }
}