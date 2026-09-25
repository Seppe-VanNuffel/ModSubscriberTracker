using System.Drawing;
using System.Windows;
using H.NotifyIcon;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private MainWindow? _mainWindow;
    private TaskbarIcon? _trayIcon;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        _trayIcon = new TaskbarIcon
        {
            ToolTipText = "Steam Workshop Monitor",
            Icon = new Icon("./Resources/icon.ico")
        };
        
        _trayIcon.ForceCreate(false);

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

        _trayIcon.ContextMenu = contextMenu;

        _mainWindow = new MainWindow();
    }

    private void OpenDashboard_Click(object sender, RoutedEventArgs e)
    {
        if (_mainWindow == null)
            return;

        _mainWindow.Show();
        _mainWindow.WindowState = WindowState.Normal;
        _mainWindow.Activate();
    }

    private void RefreshNow_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Refresh clicked!");
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Shutdown();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _trayIcon?.Dispose();

        base.OnExit(e);
    }
}