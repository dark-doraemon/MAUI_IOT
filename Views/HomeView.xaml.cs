using MAUI_IOT.ViewModels;

namespace MAUI_IOT.Views;


public partial class HomeView : ContentPage
{
    public HomeView(HomeViewModel vm)
	{
		InitializeComponent();
		BindingContext= (HomeViewModel)vm;
	}

   
}