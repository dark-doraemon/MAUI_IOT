﻿using MAUI_IOT.ViewModels;
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
namespace MAUI_IOT.Views;

public partial class LessonView : ContentPage
{


    public LessonView(LessonnViewModel lessonnViewModel)
    {

        InitializeComponent();
        BindingContext = lessonnViewModel;
        Tabinit();

        //weight_entry.Focused += (sender, e) =>
        //{
        //    weight_entry.Text = "";
        //    weight_entry.Focus();
        //};

        //weight_entry.Unfocused += async (sender, e) =>
        //{
        //    if (string.IsNullOrEmpty(weight_entry.Text))
        //    {
        //        weight_entry.Text = "0";
        //        await DisplayAlert("Thông báo", "Vui lòng nhập giá trị", "Nhập lại");
        //        weight_entry.Focus();
        //    }
        //};

        //weight.Clicked += async (sender, e) => {
        //    if (string.IsNullOrEmpty(weight_entry.Text))
        //    {
        //        weight_entry.Text = "0";
        //        await DisplayAlert("Thông báo", "Vui lòng nhập giá trị", "Nhập lại");
        //        return;
        //    }
        //    if (await DisplayAlert("Thông báo", $"Xác nhận giá trị m = {weight_entry.Text.ToString()} kg", "Lưu", "Nhập lại"))
        //    {
        //        weight_entry.Unfocus();
        //        weight_entry.IsEnabled = false;
        //        weight_entry.IsEnabled = true;
        //        FormatWeightEntry();
        //        tab_View.SelectedTab = Experiment;
        //        lessonViewModel.IsValidEntryWeight = true;
        //    }
        //    else
        //    {
        //        weight_entry.Focus();
        //    }
        //};

        //btn_Start.Clicked += async (sender, e) =>
        //{
        //    if(string.IsNullOrEmpty(weight_entry.Text) || weight_entry.Text == "0")
        //    {
        //        await DisplayAlert("Thông báo!", "Vui lòng nhập khối lượng hợp lệ", "OK");
        //        lessonViewModel.IsValidEntryWeight = false;
        //        tab_View.SelectedTab = inputParameters;
        //    }
        //};

    }

    private void FormatWeightEntry()
    {
        double temp = 0;
        try
        {
            //string entryAfterFormatted = weight_entry.Text.Replace(",", ".");
            //if (double.TryParse(entryAfterFormatted, NumberStyles.Any, CultureInfo.InvariantCulture, out temp))
            //{
            //    lessonViewModel.M = temp;

            //    if (temp < 0)
            //    {
            //        lessonViewModel.M = 0;
            //        weight_entry.Text = "0";
            //    }
            //}
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