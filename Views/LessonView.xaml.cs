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
using UraniumUI.Controls;
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
        weight_entry.Text = lessonnViewModel.M.ToString();
        weight_entry.Focused += (sender, e) =>
        {
            weight_entry.Text = "";
            weight_entry.Focus();
        };

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
                    tab_View.SelectedTab = Config;
                    return;
                }
                weight_entry.Unfocus();
                weight_entry.IsEnabled = false;
                weight_entry.IsEnabled = true;
                FormatWeightEntry();
                //    tab_View.SelectedTab = Experiment;
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
                tab_View.SelectedTab = Config;
            }
        };

        Button myButton = this.FindByName<Button>("myButton");
        if (myButton != null)
        {
            myButton.Clicked += MyButton_Clicked;
        }




        Picker myPicker = this.FindByName<Picker>("myPicker");
        if (myPicker != null)
        {
            myPicker.Title = "Chọn 1 mục ";
        }
        List<string> packetName = new List<string>();
        for (int i = 0; i < lessonnViewModel.FileCount; i++)
        {
            packetName.Add($"Experiment{i}");
        }
        myPicker.ItemsSource = packetName;
        myPicker.SelectedIndexChanged += (sender, e) =>
        {
            var selectedItem = (sender as Picker)?.SelectedItem;
            getdata(selectedItem);
        };
    }

    private void MyButton_Clicked(object sender, EventArgs e)
    {
        var selectedItem = myPicker.SelectedItem;
        if (selectedItem != null)
        {
            _lessonnViewModel.Save(selectedItem.ToString());

        }

    }

    private void addPickerItem(object sender, EventArgs e)
    {
        List<string> packetName = new List<string>();
        for (int i = 0; i < _lessonnViewModel.FileCount; i++)
        {
            packetName.Add($"Experiment{i}");
        }
        myPicker.ItemsSource = packetName;

    }



    private void createToolbar(int n)
    {
        if (n > 10)
        {
            DisplayAlert("Warming", "Đã đạt giới hạn", "Thoát");
            return;
        }
        var toolbarItem = new ToolbarItem
        {
            Text = $" save  {n}",
            Order = ToolbarItemOrder.Secondary,
            Command = ((LessonnViewModel)BindingContext).SaveCommand, // Gán Command
            CommandParameter = n,
        };

        ToolbarItems.Add(toolbarItem);
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
        tab_View.SelectedTab = Config;
    }


    //load dữ liệu 

    private async Task getdata(object selectedItem)
    {
        try
        {
            await _lessonnViewModel.Load(selectedItem.ToString());
            if (_lessonnViewModel.FileContent.Length != 0)
            {
                //     await DisplayAlert("Tiêu đề", _lessonnViewModel.FileContent, "Thoát");
                weight_entry.Text = _lessonnViewModel.M.ToString();
            }
            else
            {
                //     await DisplayAlert("Tiêu đề", $"file chưa lưu  ", "thoát");

            }
        }
        catch (Exception ex)
        {

            Debug.WriteLine("======================================Try catch Lessonview.xaml.cs===================================================");
            Debug.WriteLine(ex.Message.ToString());
            Debug.WriteLine("=======================================================================================================");


        }



    }








}