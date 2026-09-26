using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Services;
using Services.Models;

namespace GUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private SteamManager _steamManager;
    private SteamRefreshService  _refreshService;
    
    public MainWindow(SteamManager steamManager, SteamRefreshService refreshService)
    {
        InitializeComponent();
        _steamManager = steamManager;
        _refreshService = refreshService;

        RefreshMods();
    }

    public void RefreshMods()
    {
        ClearModsDisplay();
        
        foreach (var workshopMod in _steamManager.GetWorkshopItems())
        {
            AddModToDisplay(workshopMod);
        }
    }

    private void ClearModsDisplay()
    {
        StackPanel.Children.Clear();
    }

    private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }

    private async void AddMod_Click(object sender, RoutedEventArgs e)
    {
        ClearModsDisplay();
        
        _steamManager.AddWorkshopItem(WorkshopIdTextBox.Text);
        
        WorkshopIdTextBox.Text = string.Empty;
        
        await _refreshService.RefreshAsync();
        
        RefreshMods();
    }

    private async void RefreshNow_Click(object sender, RoutedEventArgs e)
    {
        await _refreshService.RefreshAsync();
    }
    
    private void DeleteMod_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        if (button.Tag is not WorkshopMod mod)
            return;

        _steamManager.RemoveWorkshopItem(mod);

        RefreshMods();
    }
    
    private void AddModToDisplay(WorkshopMod mod)
    {
        var row = new Grid
        {
            Margin = new Thickness(0, 5, 0, 5)
        };

        row.ColumnDefinitions.Add(
            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        row.ColumnDefinitions.Add(
            new ColumnDefinition { Width = new GridLength(80) });

        row.ColumnDefinitions.Add(
            new ColumnDefinition { Width = new GridLength(100) });
        
        row.ColumnDefinitions.Add(
            new ColumnDefinition { Width = new GridLength(40) });

        var title = new TextBlock
        {
            Text = mod.Title,
            FontSize = 16,
            VerticalAlignment = VerticalAlignment.Center
        };

        Grid.SetColumn(title, 0);

        var subscriptions = new TextBlock
        {
            Text = mod.Subscribers.ToString(),
            FontSize = 16,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };

        Grid.SetColumn(subscriptions, 1);

        var change = new TextBlock
        {
            Text = mod.ComparisonText(),
            FontSize = 16,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        
        Grid.SetColumn(change, 2);
        
        var deleteButton = new Button
        {
            Content = "🗑",
            Width = 30,
            Height = 30,
            Padding = new Thickness(0),
            Tag = mod
        };

        deleteButton.Click += DeleteMod_Click;

        Grid.SetColumn(deleteButton, 3);

        row.Children.Add(title);
        row.Children.Add(subscriptions);
        row.Children.Add(change);
        row.Children.Add(deleteButton);

        StackPanel.Children.Add(row);
    }
}