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
            builder.Services.AddTransient<ILessionService,LessionService>();

            builder.Services.AddTransient<LoadingView>();

            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<LoginViewModel>();

            builder.Services.AddTransient<HomeView>();
            builder.Services.AddTransient<HomeViewModel>();

            builder.Services.AddTransient<ProfileView>();
            builder.Services.AddTransient<ProfileViewModel>();

            builder.Services.AddTransient<LessonView>();
            builder.Services.AddTransient<LessonViewModel>();
            //builder.Services.AddViewModel<LessonViewModel, LessonView>();

            return builder.Build();
        }

        private static void AddViewModel<TViewModel, TView>(this IServiceCollection services)
            where TViewModel : class
            where TView : ContentPage, new()
        {
            services.AddTransient<TViewModel>();
            services.AddTransient<TView>(s => new TView() { BindingContext = s.GetRequiredService<TViewModel>() });
        }
    }
}
