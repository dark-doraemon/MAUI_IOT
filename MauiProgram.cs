using MAUI_IOT.Services.Implements;
using MAUI_IOT.Services.Interfaces;
using MAUI_IOT.ViewModels;
using MAUI_IOT.ViewModels.SensorViewModels;
using MAUI_IOT.Views;
using Microcharts.Maui;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting; 
namespace MAUI_IOT
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp(true)
                .UseMicrocharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<IAuthService,AuthService>();

            builder.Services.AddTransient<LoadingView>();

            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<LoginViewModel>();

            builder.Services.AddTransient<HomeView>();
            builder.Services.AddTransient<HomeViewModel>();

            builder.Services.AddTransient<ProfileView>();
            builder.Services.AddTransient<ProfileViewModel>();

            return builder.Build();
        }
    }
}
