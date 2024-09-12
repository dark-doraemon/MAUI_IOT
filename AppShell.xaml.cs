using MAUI_IOT.Views;

namespace MAUI_IOT
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoadingView), typeof(LoadingView));
            //Routing.RegisterRoute(nameof(HomeView), typeof(HomeView));
            Routing.RegisterRoute(nameof(ProfileView), typeof(ProfileView));
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(LessonView), typeof(LessonView));
            Routing.RegisterRoute(nameof(FullScreenChartView), typeof(FullScreenChartView));
        }
    }
}
