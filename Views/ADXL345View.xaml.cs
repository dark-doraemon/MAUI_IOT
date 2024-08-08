using MAUI_IOT.ViewModels.SensorViewModels;

namespace MAUI_IOT.Views;

public partial class ADXL345View : ContentPage
{
	ADXL345ViewModel ViewModel;
	public ADXL345View()
	{
		InitializeComponent();

        ViewModel = (ADXL345ViewModel)BindingContext;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await ViewModel.Connect();

    }
}