using MauiPopup;
using MauiPopup.Views;
using System.Collections;
using System.Diagnostics;

namespace MAUI_IOT.Views.PickerView;

public partial class PickerControl : BasePopupPage
{
	public PickerControl(IEnumerable itemSource, DataTemplate itemTemplate, double pickerControlHeight = 200)
    {
        InitializeComponent();

        clPickerView.ItemsSource = itemSource;
        clPickerView.ItemTemplate = itemTemplate;
        clPickerView.HeightRequest = pickerControlHeight;
    }

    private async void clPickerView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            var currentItem = e.CurrentSelection.FirstOrDefault();

            await PopupAction.ClosePopup(currentItem);
        }
        catch (Exception ex) {
            Debug.WriteLine(ex.ToString());
        }
    }
}