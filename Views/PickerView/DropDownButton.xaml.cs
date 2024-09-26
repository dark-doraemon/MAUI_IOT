using MauiPopup;
using System.Collections;
using System.Diagnostics;
using System.Windows.Input;
using MAUI_IOT.ViewModels;

namespace MAUI_IOT.Views.PickerView;

public partial class DropDownButton : Frame
{
    public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(
        propertyName: nameof(ViewModel),
        returnType: typeof(LessonnViewModel),
        declaringType: typeof(DropDownButton),
        defaultBindingMode: BindingMode.OneWay
    );

    public LessonnViewModel ViewModel
    {
        get => (LessonnViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(
            propertyName: nameof(ItemSource),
            returnType: typeof(IEnumerable),
            declaringType: typeof(DropDownButton),
            defaultBindingMode: BindingMode.OneWay
    );

    public IEnumerable ItemSource
    {
        get => (IEnumerable)GetValue(ItemSourceProperty);
        set => SetValue(ItemSourceProperty, value);
    }

    public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(
            propertyName: nameof(IsLoading),
            returnType: typeof(bool),
            declaringType: typeof(DropDownButton),
            defaultBindingMode: BindingMode.OneWay
    );

    public bool IsLoading
    {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public event EventHandler<EventArgs> OpenPickerEvent;
    public static readonly BindableProperty OpenPickerCommandProperty = BindableProperty.Create(
            propertyName: nameof(OpenPickerCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(DropDownButton),
            defaultBindingMode: BindingMode.OneWay
    );

    public ICommand OpenPickerCommand
    {
        get => (ICommand)GetValue(OpenPickerCommandProperty);
        set => SetValue(OpenPickerCommandProperty, value);
    }

    public static readonly BindableProperty IsDisplayPickerControlProperty = BindableProperty.Create(
            propertyName: nameof(IsDisplayPickerControlProperty),
            returnType: typeof(bool),
            declaringType: typeof(DropDownButton),
            propertyChanged: IsDisplayPickerControlPropertyChanged,
            defaultBindingMode: BindingMode.TwoWay
    );

    public static readonly BindableProperty CurrentItemProperty = BindableProperty.Create(
             propertyName: nameof(CurrentItems),
             returnType: typeof(object),
             declaringType: typeof(DropDownButton),
             propertyChanged: CurrentItemPropertyChanged,
             defaultBindingMode: BindingMode.OneWay
     );

    private static void CurrentItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (DropDownButton)bindable;

        if (newValue != null)
        {
            try
            {
                if (!string.IsNullOrEmpty(controls.DisplayMember))
                    controls.lblDropDown.Text = newValue.GetType().GetProperty("Name").GetValue(newValue, null).ToString();
            }
            catch (Exception ex) {
               Debug.WriteLine(ex.ToString());
            }
        }
    }

    public object CurrentItems
    {
        get => (object)GetValue(CurrentItemProperty);
        set => SetValue(CurrentItemProperty, value);
    }

    public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(
             propertyName: nameof(PlaceHolder),
             returnType: typeof(string),
             declaringType: typeof(DropDownButton),
             propertyChanged: PlaceHolderPropertyChanged,
             defaultBindingMode: BindingMode.OneWay
     );

    private static void PlaceHolderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (DropDownButton)bindable;
        if (control.CurrentItems == null)
        {
            if (newValue != null)
            {
                control.lblDropDown.Text = newValue.ToString();
            }
        }
    }

    public string PlaceHolder
    {
        get => (string)GetValue(PlaceHolderProperty);
        set => SetValue(PlaceHolderProperty, value);
    }

    private static async void IsDisplayPickerControlPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        try
        {
            var controls = (DropDownButton)bindable;
            if (newValue != null)
            {
                if ((bool)newValue)
                {
                    var respone = await PopupAction.DisplayPopup<object>(new PickerControl(controls.ItemSource, controls.ItemTemplate, controls.ViewModel, controls.PickerHeightRequest));
                    if (respone != null)
                    {
                        controls.CurrentItems = respone;
                    }
                    controls.IsDisplayPickerControl = false;
                }
            }
        }
        catch (Exception ex) { 
            Debug.WriteLine(ex.ToString());
        }
    }

    public bool IsDisplayPickerControl
    {
        get => (bool)GetValue(IsDisplayPickerControlProperty);
        set => SetValue(IsDisplayPickerControlProperty, value);
    }
    public DataTemplate ItemTemplate { get; set; }

    public double PickerHeightRequest { get; set; }
    public string DisplayMember { get; set; }
    public DropDownButton()
    {
        InitializeComponent();
        BindingContext = new LessonnViewModel();
    }

    private void TabGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {    
        
        OpenPickerCommand?.Execute(null);
       
        IsDisplayPickerControl = true;     
    }

}