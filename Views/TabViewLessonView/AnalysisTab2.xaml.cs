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
        Content.Children.Remove(gridCollection);
        tableDetail(gridCollection1);
        Content.Children.Add(gridCollection);

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
            var grid = generateColumn(temp.Datas_database[i], i);
            view.Children.Add(grid);
            view.SetColumn(grid, i);
        }

        return view;
    }
    public IView generateColumn(ObservableCollection<Data> data, int i)
    {
        Grid columns = new Grid();
        columns.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        columns.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

        // Remove fixed width for columns; let them adjust based on content
        columns.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        Grid headerGrid = new Grid
        {
            ColumnDefinitions = {
            new ColumnDefinition { Width = GridLength.Star }, // Flexible width for A
            new ColumnDefinition { Width = GridLength.Star }, // Flexible width for F
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
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 18,
            HorizontalTextAlignment = TextAlignment.Center,
            FontAttributes = FontAttributes.Bold,
            WidthRequest = 75
        };

        var Times = new Label
        {
            Text = $"Times {i} ",
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black,
            WidthRequest = 150, // Total width of two columns (75+75)
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
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            WidthRequest = 75

        };

        headerGrid.Children.Add(A);
        headerGrid.Children.Add(F);
        headerGrid.Children.Add(Times);

        headerGrid.SetColumn(A, 0);
        headerGrid.SetColumn(F, 1);
        headerGrid.SetColumn(Times, 0);
        headerGrid.SetRow(A, 1);
        headerGrid.SetRow(F, 1);
        headerGrid.SetRow(Times, 0);
        headerGrid.SetColumnSpan(Times, 2); // Span "Times" label across both columns

        // Add header to the columns grid
        columns.Children.Add(headerGrid);
        columns.SetRow(headerGrid, 0); // Place it in the first row

        // Create CollectionView for the rest of the data
        var collectionView = new CollectionView
        {
            ItemsSource = data,
            ItemsUpdatingScrollMode = ItemsUpdatingScrollMode.KeepScrollOffset,
            ItemTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {
                    ColumnDefinitions = {
                    new ColumnDefinition { Width = GridLength.Star }, // Let column widths adjust
                    new ColumnDefinition { Width = GridLength.Star }, // Let column widths adjust
                },
                    RowDefinitions = {
                    new RowDefinition { Height = GridLength.Auto },
                }
                };

                var labelA = new Label
                {
                    TextColor = Colors.Black,
                    WidthRequest = 75, // Set fixed width for label A
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                };
                labelA.SetBinding(Label.TextProperty, "a", stringFormat: "{0:F3}");

                var labelForce = new Label
                {
                    TextColor = Colors.Black,
                    BackgroundColor = Colors.LightSkyBlue,
                    WidthRequest = 75, // Set fixed width for label Force
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                };
                labelForce.SetBinding(Label.TextProperty, "force", stringFormat: "{0:F3}");
                grid.Children.Add(labelA);
                grid.Children.Add(labelForce);
                grid.SetColumn(labelA, 0);
                grid.SetColumn(labelForce, 1);

                return grid;
            })
        };

        // Add CollectionView to the columns grid
        columns.Children.Add(collectionView);
        columns.SetRow(collectionView, 1); // Place in the second row

        return columns;
    }

}