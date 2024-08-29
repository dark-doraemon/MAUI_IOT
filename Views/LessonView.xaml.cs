using MAUI_IOT.ViewModels;
using Microsoft.Maui.Layouts;
using System.Diagnostics;
using MAUI_IOT.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Globalization;
using UraniumUI.ViewExtensions;
using SkiaSharp.Views.Maui;
using Microsoft.Maui.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.Drawing;
using SkiaSharp;
using MAUI_IOT.Services.Interfaces;
using CommunityToolkit.Maui;
namespace MAUI_IOT.Views;

public partial class LessonView : ContentPage
{
    private LessonnViewModel _lessonnViewModel;

    public LessonView(LessonnViewModel lessonnViewModel)
    {

        InitializeComponent();
        this._lessonnViewModel = lessonnViewModel;
        BindingContext = lessonnViewModel;
        Tabinit();

        weight_entry.Focused += (sender, e) =>
        {
            weight_entry.Text = "";
            weight_entry.Focus();
        };





        var experiments = new List<string> { "Thí nghiệm 1", "Thí nghiệm 2", "Thí nghiệm 3" };
        foreach (var experiment in experiments)
        {
            var toolbarItem = new ToolbarItem
            {
                Text = experiment,
                Order = ToolbarItemOrder.Secondary
            };

            toolbarItem.Clicked += (sender, e) => HandleExperimentClicked(experiment);

            ToolbarItems.Add(toolbarItem);
        }











        weight_entry.Unfocused += async (sender, e) =>
        {
            if (string.IsNullOrEmpty(weight_entry.Text))
            {
                weight_entry.Text = "0";
                await DisplayAlert("Thông báo", "Vui lòng nhập giá trị", "Nhập lại");
                weight_entry.Focus();
            }
        };

        weight.Clicked += async (sender, e) =>
        {
            if (string.IsNullOrEmpty(weight_entry.Text))
            {
                weight_entry.Text = "0";
                await DisplayAlert("Thông báo", "Vui lòng nhập giá trị", "Nhập lại");
                return;
            }
            if (await DisplayAlert("Thông báo", $"Xác nhận giá trị m = {weight_entry.Text.ToString()} kg", "Lưu", "Nhập lại"))
            {
                double weight = double.Parse(weight_entry.Text);
                if (string.IsNullOrEmpty(weight_entry.Text) || weight <= 0)
                {
                    await DisplayAlert("Thông báo!", "Vui lòng nhập khối lượng hợp lệ", "OK");
                    lessonnViewModel.IsValidEntryWeight = false;
                    tab_View.SelectedTab = inputParameters;
                    return;
                }
                weight_entry.Unfocus();
                weight_entry.IsEnabled = false;
                weight_entry.IsEnabled = true;
                FormatWeightEntry();
                tab_View.SelectedTab = Experiment;
                lessonnViewModel.IsValidEntryWeight = true;
            }
            else
            {
                weight_entry.Focus();
            }
        };

        btn_Start.Clicked += async (sender, e) =>
        {
            double weight = double.Parse(weight_entry.Text);
            if (string.IsNullOrEmpty(weight_entry.Text) || weight <= 0)
            {
                await DisplayAlert("Thông báo!", "Vui lòng nhập khối lượng hợp lệ", "OK");
                lessonnViewModel.IsValidEntryWeight = false;
                tab_View.SelectedTab = inputParameters;
            }
        };

    }


    private void HandleExperimentClicked(string experimentName)
    {
        // Xử lý sự kiện khi một ToolbarItem được nhấp vào
        DisplayAlert("Thông báo", $"Bạn vừa nhấp vào thí nghiệm: {experimentName}", "OK");
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        DisplayAlert("test", "đây là thông báo  ", "Thoát ");
    }
    private void OnAddClicked1(object sender, EventArgs e)
    {
        DisplayAlert("test", "đây là thông báo 1   ", "Thoát ");
    }


    private void FormatWeightEntry()
    {
        double temp = 0;
        try
        {
            string entryAfterFormatted = weight_entry.Text.Replace(",", ".");
            if (double.TryParse(entryAfterFormatted, NumberStyles.Any, CultureInfo.InvariantCulture, out temp))
            {
                _lessonnViewModel.M = temp;

                if (temp < 0)
                {
                    _lessonnViewModel.M = 0;
                    weight_entry.Text = "0";
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString() + "Format entry");
        }
    }

    private void Tabinit()
    {
        tab_View.SelectedTab = inputParameters;
    }
}