using CommunityToolkit.Maui.Views;
using MAUI_IOT.ViewModels;
using System.Diagnostics;

namespace MAUI_IOT.Views;

public partial class AnalyzePopup : Popup
{

    public AnalyzePopup(LessonnViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnButtonClicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as LessonnViewModel;
        if (viewModel != null)
        {
            viewModel.addData();  // Gọi hàm trong ViewModel
        }

    }


}