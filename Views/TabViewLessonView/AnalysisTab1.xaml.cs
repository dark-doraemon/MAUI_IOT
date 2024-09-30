using MAUI_IOT.ViewModels;
using Syncfusion.Maui.Core.Carousel;
using System.Diagnostics;

namespace MAUI_IOT.Views.TabViewLessonView;

public partial class AnalysisTab1 : ContentView
{
	public AnalysisTab1()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}