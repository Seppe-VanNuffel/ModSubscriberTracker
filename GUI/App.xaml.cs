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
    private SteamRefreshService _refreshService;
    
    private CancellationTokenSource _refreshCancellationTokenSource;
    private Task _refreshTask;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        _steamManager = new SteamManager(new ModsFileManager(".\\Files\\Mods.json"));

        CreateTrayIcon();

        _refreshService = new(_steamManager, TimeSpan.FromMinutes(1));

        _refreshService.DataUpdated += RefreshService_DataUpdated;
        
        _refreshCancellationTokenSource = new();
        
        _refreshTask = _refreshService.RunAsync(_refreshCancellationTokenSource.Token);

        UpdateTray();
        
        _mainWindow = new MainWindow(_steamManager, _refreshService);
        
        _mainWindow.Show();
    }

    private void CreateTrayIcon()
    {
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
        await _refreshService.RefreshAsync();
    }

    private void UpdateTray()
    {
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
    
    private void RefreshService_DataUpdated(object? sender, EventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            UpdateTray();
            _mainWindow.RefreshMods();
        });
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