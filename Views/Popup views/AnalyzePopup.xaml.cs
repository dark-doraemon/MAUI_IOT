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



    private void OnButtonDetaiClicked(object sender, CheckedChangedEventArgs e)
    {
        var viewModel = BindingContext as LessonnViewModel;
        if (viewModel != null)
        {
            viewModel.addData();  // Gọi hàm trong ViewModel
        }

    }
    private void OnButtonSummaryClicked(object sender, CheckedChangedEventArgs e)
    {
        var viewModel = BindingContext as LessonnViewModel;
        if (viewModel != null)
        {
            viewModel.addDataSummary();  // Gọi hàm trong ViewModel
        }

    }
}