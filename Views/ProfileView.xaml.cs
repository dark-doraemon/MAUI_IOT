using MAUI_IOT.ViewModels;

namespace MAUI_IOT.Views;

public partial class ProfileView : ContentPage
{
	public ProfileView(ProfileViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;	
	}
}