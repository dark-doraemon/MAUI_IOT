using MauiPopup;
using MauiPopup.Views;
using MAUI_IOT.ViewModels;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace MAUI_IOT.Views.PickerView;

public partial class EditDevice : BasePopupPage
{
    //private AnalyzePopup _analyzePopup;
    public EditDevice(LessonnViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
        private async void Button_Clicked(object sender, EventArgs e)
    {
        await PopupAction.ClosePopup();
    }
}