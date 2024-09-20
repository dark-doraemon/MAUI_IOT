using CommunityToolkit.Maui.Views;
using MAUI_IOT.ViewModels;
using System.Diagnostics;

namespace MAUI_IOT.Views;

public partial class AnalyzePopup : Popup
{
    private int radioButtonCounter = 0;
    private int valueCounter = 0;
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
    public void onRadioBTNCheck(object sender, EventArgs e)
    {
        RadioButton newRadioButton = new RadioButton
        {
            Content = $"Table {radioButtonCounter}",
            BindingContext = valueCounter
        };
        newRadioButton.CheckedChanged += OnRadioButtonCheckedChanged;

        RadioButtonContainer.Children.Add(newRadioButton);

        radioButtonCounter++;
        valueCounter++;
    }

    private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            DataDisplayLabel.Text = $"Selected Value: {radioButton.BindingContext}";
        }
    }
}
