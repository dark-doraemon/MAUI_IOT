using MAUI_IOT.ViewModels;

namespace MAUI_IOT.Views;

public partial class FullScreenChartView : ContentPage
{
	public FullScreenChartView(FullScreenChartViewModel fullScreenChartViewModel)
	{
		InitializeComponent();
		BindingContext = fullScreenChartViewModel;	
	}
}