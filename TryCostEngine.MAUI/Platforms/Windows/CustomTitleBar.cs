using Microsoft.Maui;
using Xaml = Microsoft.UI.Xaml;
using XamlControls = Microsoft.UI.Xaml.Controls;

namespace TryCostEngine.MAUI;

internal class CustomTitleBar : XamlControls.StackPanel
{
	public CustomTitleBar()
	{
        //StackPanel背景透明 Background="Transparent"
        //Windows.UI.Color.FromArgb(100, 255, 0, 0)
        Background = new Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);

        var grid = new XamlControls.Grid();

        //Grid 背景透明 
        grid.Background = new Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);

        grid.Name = "CustomTitleBar";

        //grid.RowDefinitions.Add(new XamlControls.RowDefinition()
        //{
        //    Height = new Xaml.GridLength(1, Xaml.GridUnitType.Auto)
        //});

        grid.RowDefinitions.Add(new XamlControls.RowDefinition()
        {
            Height = new Xaml.GridLength(1, Xaml.GridUnitType.Star)
        });


        grid.ColumnDefinitions.Add(new XamlControls.ColumnDefinition() 
        { 
            Width = new Xaml.GridLength(1, Xaml.GridUnitType.Star) 
        });

        var button = new XamlControls.Button()
        {
            Content = "This is a button"
        };

        grid.Children.Add(button);

        XamlControls.Grid.SetColumn(button, 0);
        XamlControls.Grid.SetRow(button, 0);

        var border = new XamlControls.Border();

        //Border 背景颜色
        //border.Background = new Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 1));
        border.Child = grid;

        Children.Add(border);
    }
}
