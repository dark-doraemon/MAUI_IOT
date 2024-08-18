using MAUI_IOT.ViewModels;
using Microsoft.Maui.Layouts;
using System.Diagnostics;
using MAUI_IOT.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Globalization;
using UraniumUI.ViewExtensions;
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
    private event EventHandler SetM;
    public LessonView(LessonViewModel lessonViewModel)
    {
        InitializeComponent();

        this.lessonViewModel = lessonViewModel;
        BindingContext = lessonViewModel;

        lessonViewModel.OnStart += Handle_Onstart;
        lessonViewModel.OnStop += Handle_Onstop;
        lessonViewModel.OnSave += Handle_OnSave;
        tab_View.PropertyChanged += OnTabView;

        loadingPosition();

    }

    //căn chỉnh hoạt ảnh load dữ liệu
    private void loadingPosition()
    {
        var width_x = 300;
        var height_y = 300;

        loading_1.TranslationX = width_x / 10;
        loading_1.TranslationY = -height_y / 10;

        loading_2.TranslationX = width_x / 10;
        loading_2.TranslationY = -height_y / 10;

        loading_3.TranslationX = width_x / 10;
        loading_3.TranslationY = -height_y / 10;
    }

    private async void Handle_OnSave(object sender, EventArgs e)
    {
        if (await ReadData(lessonViewModel.path))
        {
            await DisplayAlert("Thông báo!", "Lưu dữ liệu thành công", "OK");
            isSave = true;
            try
            {
                a = lessonViewModel.a;
                F = lessonViewModel.F;
                Duration = lessonViewModel.Duration;
                await BindingData(table, a.Count, Duration, a, F);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ReadData: "+ex.ToString());
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
        chart_1.IsVisible = true;
        loading_1.IsVisible = false;
        chart_2.IsVisible = true;
        loading_2.IsVisible = false;
        chart_3.IsVisible = true;
        loading_3.IsVisible = false;

        loading_text_1.IsVisible = loading_text_2.IsVisible = loading_text_3.IsVisible = false;


      

        //BindingData(table, a.Count, Duration, a, F);
    }
    private void Handle_Onstart(object sender, EventArgs e)
    {

        if (isEntryM)
        {

            chart_1.IsVisible = false;
            loading_1.IsRunning = true;
            chart_2.IsVisible = false;
            loading_2.IsRunning = true;
            chart_3.IsVisible = false;
            loading_3.IsRunning = true;
            loading_text_1.IsVisible = loading_text_2.IsVisible = loading_text_3.IsVisible = true;
            Debug.Write("Start" + DateTime.Now.ToString());
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

    private async void Handle_Set(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Xác nhận!", $"Khối lượng m = {weight_entry.Text} kg", "Lưu", "Nhập lại");
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

    public async void OnTabView(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if ("Table".Equals(tab_View.SelectedTab.Title))
        {
            if (!isSave)
            {
                await DisplayAlert("Thông báo!", "Chưa có dữ liệu", "OK");
                tab_View.SelectedTab = Experiment;
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
}