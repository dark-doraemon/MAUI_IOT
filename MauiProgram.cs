using MAUI_IOT.Services.Implements;
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

            builder.Services.AddTransient<LessonView>();
            builder.Services.AddTransient<LessonViewModel>();
            builder.Services.AddTransient<LessonnViewModel>();
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
            return builder;
        }
    }
}
