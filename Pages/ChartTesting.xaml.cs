using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using ViewModelsSamples.Lines.AutoUpdate;

namespace MAUI_IOT.Pages;


[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ChartTesting : ContentPage
{
    private bool? isStreaming = false;
    public ChartTesting()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, System.EventArgs e)
    {
        var vm = (ViewModel)BindingContext;

        isStreaming = isStreaming is null ? true : !isStreaming;

        while (isStreaming.Value)
        {
            vm.RemoveItem();
            vm.AddItem();
            await Task.Delay(1000);
        }
    }

}