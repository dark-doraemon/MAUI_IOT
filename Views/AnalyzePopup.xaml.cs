using CommunityToolkit.Maui.Views;
using MAUI_IOT.ViewModels;

namespace MAUI_IOT.Views;

public partial class AnalyzePopup : Popup
{

    public AnalyzePopup(LessonnViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}