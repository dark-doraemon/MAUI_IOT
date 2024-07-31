using MAUI_IOT.ViewModels;

namespace MAUI_IOT.Views;

public partial class LessonView : ContentPage
{
    public LessonView(LessonViewModel lessonViewModel)
    {
        InitializeComponent();
        BindingContext = lessonViewModel;
    }

    //public LessonView()
    //{
    //    InitializeComponent();
    //}
}