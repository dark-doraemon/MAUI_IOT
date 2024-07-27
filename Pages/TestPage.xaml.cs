using MAUI_IOT.Hubs;
using MAUI_IOT.Models;
using Newtonsoft.Json;
using System.Data;
using SkiaSharp;
using Microcharts.Maui;
using Microcharts;
using System.Collections.ObjectModel;
using LiveChartsCore.Defaults;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
namespace MAUI_IOT.Pages;

public partial class TestPage : ContentPage
{
   
    ESP32Sensor esp32Sensor;
    string uriString = "ws://192.168.1.125:1880/test2";
    Uri uri;

    public TestPage()
    {
        InitializeComponent();

        esp32Sensor = new ESP32Sensor();

        uri = new Uri(uriString);
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //đăng kí hàm sẽ gọi khi dữ liệu thay đổi 
        esp32Sensor.PropertyChanged += Sensor_PropertyChanged;

        //đăng kí hàm sẽ gọi khi kết nối wifi thay đổi
        Connectivity.Current.ConnectivityChanged += Connectivity_ConnectivityChanged;
        await esp32Sensor.ConnectAsync(uri);
    }

    private async void Sensor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //kiểm tra xem tên thuộc tính có phải là ReceivedData không vì ta đã set thuộc tính PropertyName là ReceivedData ở hàm Invoke
        if (e.PropertyName == nameof(ESP32Sensor.ReceivedData))
        {
            Temp eSP32Model = JsonConvert.DeserializeObject<Temp>(esp32Sensor.ReceivedData);

            // Update UI khi có dữ liệu mới
            this.lbl_temperature.Text = eSP32Model.Temperature.ToString();
            this.lbl_humidity.Text = eSP32Model.Humidity.ToString();
        }
    }

    async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.None)
        {
            esp32Sensor.Close();
            DisplayAlert("Message", "Mất kết nối", "OK");
            this.lbl_humidity.Text = "";
            this.lbl_temperature.Text = "";
        }
        else
        {
            //DisplayAlert("Message", "Đã kết nối", "Ok");
            await esp32Sensor.ConnectAsync(uri);
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.None)
        {
            DisplayAlert("Message", "Bạn chưa kết nối internet", "OK");
        }
        else
        {
            //await esp32Sensor.ConnectAsync(uri);
        }
    }

}


