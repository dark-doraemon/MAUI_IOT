using MAUI_IOT.Services.Implements;
using MAUI_IOT.Services.Implements.MQTT;
using MAUI_IOT.Services.Interfaces;
using MAUI_IOT.Services.Interfaces.MQTT;
using MAUI_IOT.ViewModels;
using MAUI_IOT.Views;
using MauiApp1.Services.Implements;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using UraniumUI;
using CommunityToolkit.Maui;
using MAUI_IOT.Services.Interfaces.DataManagement;
using MAUI_IOT.Services.Implements.DataManagement;
using MAUI_IOT.Services.Interfaces.SaveData;
using MAUI_IOT.Services.Implements.SaveData;
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
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font Awesome 6 Free-Solid-900", "FaBrands");
                })
                .RegisterServices();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<ILessionService, LessionService>();

            builder.Services.AddTransient<LoadingView>();

            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<LoginViewModel>();

            builder.Services.AddTransient<HomeView>();
            builder.Services.AddTransient<HomeViewModel>();

            builder.Services.AddTransient<ProfileView>();
            builder.Services.AddTransient<ProfileViewModel>();

            builder.Services.AddSingleton<LessonView>();
            builder.Services.AddSingleton<LessonnViewModel>();
            //builder.Services.AddViewModel<LessonViewModel, LessonView>();

            //builder.Services.AddViewModel<FullScreenChartView,FullScreenChartViewModel>();
            builder.Services.AddTransient<FullScreenChartView>();
            builder.Services.AddTransient<FullScreenChartViewModel>();

            return builder.Build();
        }

        private static void AddViewModel<TView, TViewModel>(this IServiceCollection services)
            where TView : ContentPage, new()
            where TViewModel : class
        {
            services.AddTransient<TView>(s => new TView() { BindingContext = s.GetRequiredService<TViewModel>() });
            services.AddTransient<TViewModel>();
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IConnect, Connect>();
            builder.Services.AddTransient<IPublish, Publisher>();
            builder.Services.AddTransient<ISubscribe, Subscriber>();
            builder.Services.AddTransient<IDisconnect, Disconnect>();
            builder.Services.AddTransient<ILoadData, LoadData>();
            builder.Services.AddTransient<ISaveData, SaveData>();
            return builder;
        }
    }
}
