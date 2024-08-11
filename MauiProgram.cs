using MAUI_IOT.Services.Implements;
using MAUI_IOT.Services.Interfaces;
using MAUI_IOT.ViewModels;
using MAUI_IOT.Views;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using UraniumUI;
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
    }
}
