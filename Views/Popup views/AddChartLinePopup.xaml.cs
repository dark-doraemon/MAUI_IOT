using CommunityToolkit.Maui.Views;
using MAUI_IOT.ViewModels;

namespace MAUI_IOT.Views;
public partial class AddChartLinePopup : Popup
{
    public AddChartLinePopup(LessonnViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

    }
}