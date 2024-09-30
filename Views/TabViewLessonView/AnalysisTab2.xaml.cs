using MAUI_IOT.ViewModels;
using System.Diagnostics;

namespace MAUI_IOT.Views.TabViewLessonView;

public partial class AnalysisTab2 : ContentView
{
	public AnalysisTab2()
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