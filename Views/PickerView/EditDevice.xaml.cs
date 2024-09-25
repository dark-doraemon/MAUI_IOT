using MauiPopup;
using MauiPopup.Views;
using MAUI_IOT.ViewModels;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace MAUI_IOT.Views.PickerView
{
    [QueryProperty(nameof(QrCodeResult), "Thiết bị phát hiện ")]
    public partial class EditDevice : BasePopupPage
    {
        //private AnalyzePopup _analyzePopup;
        public EditDevice(LessonnViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            btnSelectDevice.ViewModel = viewModel;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupAction.ClosePopup();
        }
        public string QrCodeResult
        {
            set
            {
                CounterBtn.Text = $"Scanner result :{value}";
            }
        }
        private void OnCounterClicked(object sender, EventArgs e)
        {

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
        private async void ScanButtonClick(object sender, EventArgs e)
        {
            Routing.RegisterRoute(nameof(ScanPage), typeof(ScanPage));

            await Shell.Current.GoToAsync(nameof(ScanPage));
        }
    }
}