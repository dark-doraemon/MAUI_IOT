using MAUI_IOT.ViewModels;

namespace MAUI_IOT.Views;


[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ESP32View : ContentPage
{
    ESP32ViewModel vm;
    public ESP32View(ESP32ViewModel eSP32ViewModel)
    {
        InitializeComponent();

        vm = (ESP32ViewModel)BindingContext;
        //vm = eSP32ViewModel;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await vm.Connect();

    }
}