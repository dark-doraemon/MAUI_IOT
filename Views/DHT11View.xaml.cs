using MAUI_IOT.ViewModels.SensorViewModels;

namespace MAUI_IOT.Views;


[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ESP32View : ContentPage
{
    DHT11ViewModel vm;
    public ESP32View()
    {
        InitializeComponent();

        vm = (DHT11ViewModel)BindingContext;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await vm.Connect();

    }
}