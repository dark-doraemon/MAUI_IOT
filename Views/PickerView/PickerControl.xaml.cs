using MAUI_IOT.ViewModels;
using MauiPopup;
using MauiPopup.Views;
using System.Collections;
using System.Diagnostics;

namespace MAUI_IOT.Views.PickerView;

public partial class PickerControl : BasePopupPage
{
    private LessonnViewModel lessonnViewModel;
	public PickerControl(IEnumerable itemSource, DataTemplate itemTemplate, LessonnViewModel viewModel, double pickerControlHeight = 200)
    {
        InitializeComponent();

        clPickerView.ItemsSource = itemSource;
        clPickerView.ItemTemplate = itemTemplate;
        clPickerView.HeightRequest = pickerControlHeight;
        this.lessonnViewModel = viewModel;
    }

    private async void clPickerView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentItem = e.CurrentSelection.FirstOrDefault();
        try
        {
            
            Models.Device selectedDevice = currentItem as Models.Device;

           
            if (selectedDevice != null)
            {
                lessonnViewModel.Device = selectedDevice.Name;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }

    }
}