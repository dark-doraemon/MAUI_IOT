using MAUI_IOT.Models.Data;
using MAUI_IOT.ViewModels;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MAUI_IOT.Views.TabViewLessonView;

public partial class AnalysisTab2 : ContentView
{
    public AnalysisTab2()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        gridCollection1.Children.Clear();
        gridCollection1.ColumnDefinitions.Clear();

        tableDetail(gridCollection1);

        this.InvalidateMeasure();

        LessonnViewModel l = BindingContext as LessonnViewModel;
    }

    //private void Button_Clicked(object sender, EventArgs e)
    //{
    //    gridCollection1.Children.Clear();

    //    Content.Children.Remove(gridCollection);

    //    tableDetail(gridCollection1);

    //    Content.Children.Add(gridCollection);

    //    LessonnViewModel l = BindingContext as LessonnViewModel;
    //    Debug.WriteLine("Number of datas count: " + l.Datas_database.Count);
    //}

    public Grid tableDetail(Grid view)
    {
        LessonnViewModel temp = BindingContext as LessonnViewModel;
        if (temp == null || temp.Datas_database == null)
            return view;

        int columnCount = temp.Datas_database.Count;

        view.ColumnDefinitions.Clear();
        view.Children.Clear();

        for (int i = 0; i < columnCount; i++)
        {
            view.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(500, GridUnitType.Absolute) });
            var grid = generateColumn(temp.Datas_database[i], i);
            view.Children.Add(grid);
            view.SetColumn(grid, i);
        }

        return view;
    }

    public IView generateColumn(ObservableCollection<Data> data, int ExperimentsTimes) // Thay YourDataType bằng kiểu dữ liệu thực tế
    {
        Grid columns = new Grid();
        columns.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        columns.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
        columns.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        Grid headerGrid = new Grid
        {
            ColumnDefinitions = {
            new ColumnDefinition { Width = GridLength.Auto },
            new ColumnDefinition { Width = GridLength.Auto },
        },
            RowDefinitions = {
            new RowDefinition { Height = GridLength.Auto },
            new RowDefinition { Height = GridLength.Auto }
        }
        };
        var A = new Label
        {
            Text = "A",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black,
            WidthRequest = 75,
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 18,
            HorizontalTextAlignment = TextAlignment.Center,
            FontAttributes = FontAttributes.Bold,


        };
        var Times = new Label
        {
            Text = $"Times {ExperimentsTimes} ",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black,
            WidthRequest = 150,
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,

        };
        var F = new Label
        {
            Text = "F",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black,
            WidthRequest = 75,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,


        };

        headerGrid.Children.Add(A);
        headerGrid.Children.Add(F);
        headerGrid.Children.Add(Times);
        headerGrid.SetColumn(F, 1);
        headerGrid.SetColumn(A, 0);
        headerGrid.SetColumn(Times, 0);
        headerGrid.SetRow(A, 1);
        headerGrid.SetRow(F, 1);
        headerGrid.SetRow(Times, 0);
        headerGrid.SetColumnSpan(Times, 2);
        // Thêm header vào cột
        columns.Children.Add(headerGrid);
        Grid.SetRow(headerGrid, 0); // Đặt ở hàng đầu tiên

        // Tạo CollectionView cho phần còn lại
        var collectionView = new CollectionView
        {
            ItemsSource = data,
            ItemsUpdatingScrollMode = ItemsUpdatingScrollMode.KeepScrollOffset,
            ItemTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {
                    ColumnDefinitions = {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                    RowDefinitions = {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                }
                };

                var labelA = new Label
                {
                    TextColor = Colors.Black,
                    WidthRequest = 75,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,


                };
                labelA.SetBinding(Label.TextProperty, "a", stringFormat: "{0:F3}");

                var labelForce = new Label
                {
                    TextColor = Colors.Black,
                    BackgroundColor = Colors.LightSkyBlue,
                    WidthRequest = 75,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,

                };
                labelForce.SetBinding(Label.TextProperty, "force", stringFormat: "{0:F3}");

                // Thêm Labels vào Grid
                grid.Children.Add(labelA);
                grid.Children.Add(labelForce);
                grid.SetColumn(labelA, 0);
                grid.SetColumn(labelForce, 1);
                grid.SetRow(labelA, 0);
                grid.SetRow(labelForce, 0);

                return grid;
            })
        };
        // Thêm CollectionView vào cột
        columns.Children.Add(collectionView);
        Grid.SetRow(collectionView, 1); // Đặt ở hàng thứ hai
        return columns;
    }

}
