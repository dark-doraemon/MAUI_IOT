using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using LiveChartsCore.Measure;
using MAUI_IOT.ViewModels;
using Microsoft.Maui.Layouts;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace MAUI_IOT.Views;

public partial class AnalyzePopup : Popup
{

    public AnalyzePopup(LessonnViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        GirdChoice.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });
        GirdChoice.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        int datacouter = 5;

        Grid choice = GenGrid();
        Grid data = GenDataGrid(datacouter);
        GirdChoice.Children.Add(choice);
        GirdChoice.Children.Add(data);
        GirdChoice.SetRow(choice, 0);
        GirdChoice.SetRow(data, 1);
    }




    Grid GenGrid()
    {


        Grid comparegird = new Grid();
        comparegird.BackgroundColor = Colors.Cyan;
        comparegird.ColumnDefinitions.Clear();
        comparegird.RowDefinitions.Clear();
        comparegird.WidthRequest = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density) * 1;
        comparegird.VerticalOptions = LayoutOptions.Start;
        comparegird.Margin = new Thickness(0, 10, 0, 0);
        comparegird.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        comparegird.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        // Tạo 6 cột cho bảng
        for (int i = 0; i < 6; i++)
        {
            if (i % 2 == 0)
                comparegird.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.7, GridUnitType.Star) });
            else
            {
                comparegird.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });

            }
        }

        Label lblTuyChon = new Label
        {
            Text = "Bảng So Sánh",
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold,
            FontSize = 30
        };

        Grid.SetRow(lblTuyChon, 0);
        Grid.SetColumn(lblTuyChon, 0);
        Grid.SetColumnSpan(lblTuyChon, 6);
        comparegird.Children.Add(lblTuyChon);

        Label lblTongHop = new Label
        {
            Text = "Tổng Hợp",
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(10, 10),
            FontAttributes = FontAttributes.Bold,
            FontSize = 15
        };
        CheckBox chkTongHop = new CheckBox
        {
            HorizontalOptions = LayoutOptions.Start,
        };
        chkTongHop.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsCheckedSummary", BindingMode.TwoWay));
        chkTongHop.CheckedChanged += OnButtonSummaryClicked;
        // Thêm Label và CheckBox "Tổng Hợp" vào hàng 1, cột 0 và cột 1
        Grid.SetRow(lblTongHop, 1);
        Grid.SetColumn(lblTongHop, 0);
        comparegird.Children.Add(lblTongHop);

        Grid.SetRow(chkTongHop, 1);
        Grid.SetColumn(chkTongHop, 1);
        comparegird.Children.Add(chkTongHop);

        // Label và Checkbox cho "Chi Tiết"
        Label lblChiTiet = new Label
        {
            Text = "Chi Tiết",
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(10, 10),
            FontAttributes = FontAttributes.Bold,
            FontSize = 15
        };
        CheckBox chkChiTiet = new CheckBox
        {
            HorizontalOptions = LayoutOptions.Start,
        };
        chkChiTiet.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsCheckedDetail", BindingMode.TwoWay));
        chkChiTiet.CheckedChanged += OnButtonDetaiClicked;
        // Thêm Label và CheckBox "Chi Tiết" vào hàng 1, cột 2 và cột 3
        Grid.SetRow(lblChiTiet, 1);
        Grid.SetColumn(lblChiTiet, 2);
        comparegird.Children.Add(lblChiTiet);

        Grid.SetRow(chkChiTiet, 1);
        Grid.SetColumn(chkChiTiet, 3);
        comparegird.Children.Add(chkChiTiet);

        // Label và Checkbox cho "Biểu Đồ"
        Label lblBieuDo = new Label
        {
            Text = "Biểu Đồ",
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(10, 10),
            FontAttributes = FontAttributes.Bold,
            FontSize = 15
        };
        CheckBox chkBieuDo = new CheckBox
        {
            HorizontalOptions = LayoutOptions.Start,
        };
        chkBieuDo.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsCheckedChart", BindingMode.TwoWay));
        // Thêm Label và CheckBox "Biểu Đồ" vào hàng 1, cột 4 và cột 5
        Grid.SetRow(lblBieuDo, 1);
        Grid.SetColumn(lblBieuDo, 4);
        comparegird.Children.Add(lblBieuDo);

        Grid.SetRow(chkBieuDo, 1);
        Grid.SetColumn(chkBieuDo, 5);
        comparegird.Children.Add(chkBieuDo);
        return comparegird;
    }



    Grid GenDataGrid(int n)
    {
        Grid grid = new Grid();
        grid.BackgroundColor = Colors.Red;
        grid.ColumnDefinitions.Clear();
        grid.RowDefinitions.Clear();
        grid.WidthRequest = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density) * 1;
        for (int i = 0; i < 6; i++)
        {
            if (i % 2 == 0)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.6, GridUnitType.Star) });
            else
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.4, GridUnitType.Star) });

            }
        }
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        Label lblTile = new Label
        {
            Text = "Bảng Dữ Liệu",
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold,
            FontSize = 30
        };
        grid.Children.Add(lblTile);
        grid.SetRow(lblTile, 0);
        grid.SetColumn(lblTile, 0);
        grid.SetColumnSpan(lblTile, 6);

        for (int i = 1; i <= n; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            Label lblName = new Label
            {
                Text = $"Thí Nghiệm Lần {i}",
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,

            };

            Label lblF = new Label
            {
                Text = $"F(l{i})",
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                Margin = new Thickness(5, 0, 0, 0)
            };
            CheckBox chkF = new CheckBox
            {
                HorizontalOptions = LayoutOptions.Start,
            };
            // chkChiTiet.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsCheckedDetail", BindingMode.TwoWay));
            Label lblA = new Label
            {
                Text = $"A(l{i})",
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 15
            };
            CheckBox chkA = new CheckBox
            {
                HorizontalOptions = LayoutOptions.Start,
            };
            // chkChiTiet.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsCheckedDetail", BindingMode.TwoWay));
            grid.Children.Add(lblName);
            grid.SetRow(lblName, i);
            grid.SetColumn(lblName, 0);
            grid.SetColumnSpan(lblName, 2);

            grid.Children.Add(lblF);
            grid.SetRow(lblF, i);
            grid.SetColumn(lblF, 2);

            grid.Children.Add(chkF);
            grid.SetRow(chkF, i);
            grid.SetColumn(chkF, 3);

            grid.Children.Add(lblA);
            grid.SetRow(lblA, i);
            grid.SetColumn(lblA, 4);

            grid.Children.Add(chkA);
            grid.SetRow(chkA, i);
            grid.SetColumn(chkA, 5);

            Debug.WriteLine("grid.SetRow(chkA, i)" + i);
        }
        return grid;

    }





    private void OnButtonDetaiClicked(object sender, CheckedChangedEventArgs e)
    {
        var viewModel = BindingContext as LessonnViewModel;
        if (viewModel != null)
        {
            viewModel.addDataDetail();  // Gọi hàm trong ViewModel
        }
    }

    private void OnButtonSummaryClicked(object sender, CheckedChangedEventArgs e)
    {
        var viewModel = BindingContext as LessonnViewModel;
        if (viewModel != null)
        {
            viewModel.addDataSummary();
        }

    }



    //public void onRadioBTNCheck(object sender, EventArgs e)
    //{
    //    RadioButton newRadioButton = new RadioButton
    //    {
    //        Content = $"Table {radioButtonCounter}",
    //        BindingContext = valueCounter
    //    };
    //    newRadioButton.CheckedChanged += OnRadioButtonCheckedChanged;

    //    RadioButtonContainer.Children.Add(newRadioButton);

    //    radioButtonCounter++;
    //    valueCounter++;
    //}

}