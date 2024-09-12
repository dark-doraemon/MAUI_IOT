using MAUI_IOT.ViewModels;
//using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace MAUI_IOT.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel vm)
	{
		InitializeComponent();
        //Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
        //{
        //    h.PlatformView.BackgroundTintList =
        //    Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
        //});

        //BindingContext = vm;
    }

   
    
}