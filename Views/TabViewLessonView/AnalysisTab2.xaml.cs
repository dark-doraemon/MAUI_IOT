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

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Task.Delay(100);
            gridCollection.Children.Clear();
            gridCollection.ColumnDefinitions.Clear();
            //Content.Add(tableDetail(gridCollection));
            Content.SetRowSpan(tableDetail(gridCollection), 1);
            LessonnViewModel l = BindingContext as LessonnViewModel;
            Debug.WriteLine("Number of datas count: " + l.Datas_database.Count);
        });
    }

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
            view.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            var grid = generateColumn(temp.Datas_database[i]);
            view.Children.Add(grid);
            view.SetColumn(grid, i);
        }

        return view;
    }

    public IView generateColumn(ObservableCollection<Data> data) 
    {
        Grid columns = new Grid
        {
            ColumnDefinitions = {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            },
            RowDefinitions =
            {
                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
            }
        };
       
        var collectionView = new CollectionView
        {
            ItemsSource = data,
            ItemsUpdatingScrollMode = ItemsUpdatingScrollMode.KeepScrollOffset,
            ItemTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },

                    RowDefinitions = {
                        new RowDefinition { Height = GridLength.Auto }
                    }
                };

                var labelA = new Label
                {
                    BackgroundColor = Colors.Black,
                    TextColor = Colors.Wheat
                };
                labelA.SetBinding(Label.TextProperty, "a", stringFormat: "{0:F3}");

                var labelForce = new Label
                {
                    BackgroundColor = Colors.Red
                };
                labelForce.SetBinding(Label.TextProperty, "force", stringFormat: "{0:F3}");

                // Thêm Labels vào Grid
                grid.Children.Add(labelA);
                grid.Children.Add(labelForce);
                grid.SetColumn(labelA, 0);
                grid.SetColumn(labelForce, 1);
                return grid;
            })
        };

        columns.Children.Add(collectionView);
        columns.SetColumn(collectionView, 0);
        return columns;
    }

}