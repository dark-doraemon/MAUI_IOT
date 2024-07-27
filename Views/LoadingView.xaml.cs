using MAUI_IOT.Services.Implements;

namespace MAUI_IOT.Views;

public partial class LoadingView : ContentPage
{
    private readonly IAuthService authService;

    public LoadingView(IAuthService authService)
	{
		InitializeComponent();
        this.authService = authService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (await authService.IsAuthenticatedAsync())
        {
            //user logged in 
            //redirect to home view
            await Shell.Current.GoToAsync($"//{nameof(HomeView)}");
        }
        else
        {
            await Shell.Current.GoToAsync($"{nameof(LoginView)}");
        }
    }
}