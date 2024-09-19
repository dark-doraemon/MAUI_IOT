using MauiPopup;
using MauiPopup.Views;
using MAUI_IOT.ViewModels;

namespace MAUI_IOT.Views.PickerView;

public partial class EditDevice : BasePopupPage
{
	public EditDevice()
	{
		InitializeComponent();
        BindingContext = new LessonnViewModel();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await PopupAction.ClosePopup();
    }
}