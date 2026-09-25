using System.Drawing;
using System.Windows;
using H.NotifyIcon;
using System.Windows.Controls;
using Data;
using Services;
using Services.Models;

namespace GUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private MainWindow? _mainWindow;
    private TaskbarIcon? _trayMenu;
    private SteamManager _steamManager;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        _trayMenu = new TaskbarIcon
        {
            ToolTipText = "Steam Workshop Monitor",
            Icon = new Icon(".\\Resources\\icon.ico")
        };
        
        _trayMenu.ForceCreate(false);

        var contextMenu = new ContextMenu();

        var openItem = new MenuItem
        {
            Header = "Open Dashboard"
        };

        openItem.Click += OpenDashboard_Click;

        var refreshItem = new MenuItem
        {
            Header = "Refresh Now"
        };

        refreshItem.Click += RefreshNow_Click;

        var exitItem = new MenuItem
        {
            Header = "Exit"
        };

        exitItem.Click += Exit_Click;

        contextMenu.Items.Add(openItem);
        contextMenu.Items.Add(refreshItem);
        contextMenu.Items.Add(new Separator());
        contextMenu.Items.Add(exitItem);

        _trayMenu.ContextMenu = contextMenu;

        _steamManager = new SteamManager(new ModsFileManager(".\\Files\\Mods.json"));

        _mainWindow = new MainWindow(_steamManager);

        await UpdateTray();
        
        _mainWindow.Show();
    }

    private void OpenDashboard_Click(object sender, RoutedEventArgs e)
    {
        if (_mainWindow == null)
            return;

        _mainWindow.Show();
        _mainWindow.WindowState = WindowState.Normal;
        _mainWindow.Activate();
    }

    private async void RefreshNow_Click(object sender, RoutedEventArgs e)
    {
        await UpdateTray();
    }

    private async Task UpdateTray()
    {
        await _steamManager.UpdateWorkshopItemData();
        UpdateTrayMenu(_steamManager.GetWorkshopItems());
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Shutdown();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _trayMenu?.Dispose();

        base.OnExit(e);
    }
    
    private void UpdateTrayMenu(IEnumerable<WorkshopMod> workshopItems)
    {
        if (_trayMenu == null)
            return;

        var contextMenu = new ContextMenu();
        
        foreach (var workshopItem in workshopItems)
        {
            var modItem = new MenuItem
            {
                Header = $"{workshopItem.Title}    {workshopItem.Subscribers}    {workshopItem.ComparisonText()}"
            };

            contextMenu.Items.Add(modItem);
        }

        contextMenu.Items.Add(new Separator());

        var openItem = new MenuItem
        {
            Header = "Open Dashboard"
        };

        openItem.Click += OpenDashboard_Click;

        contextMenu.Items.Add(openItem);

        var refreshItem = new MenuItem
        {
            Header = "Refresh Now"
        };

        refreshItem.Click += RefreshNow_Click;

        contextMenu.Items.Add(refreshItem);

        contextMenu.Items.Add(new Separator());

        var exitItem = new MenuItem
        {
            Header = "Exit"
        };

        exitItem.Click += Exit_Click;

        contextMenu.Items.Add(exitItem);

        _trayMenu.ContextMenu = contextMenu;
    }
}