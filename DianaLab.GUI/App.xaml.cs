using System.Windows;
using System.Windows.Threading;
using DianaLab.GUI.Logging;
using Microsoft.Extensions.Logging;

namespace DianaLab.GUI;

// <summary>
// Interaction logic for App.xaml
// </summary>
public partial class App : Application
{
    private DispatcherTimer _consoleCheckTimer;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Log.LoggerFactory = LoggerFactory.Create(builder => builder.AddDebug());
        Utils.ConsoleHelper.Show();
        Console.WriteLine($"Diana's Lab");

        _consoleCheckTimer = new DispatcherTimer();
        _consoleCheckTimer.Interval = TimeSpan.FromMilliseconds(500);
        _consoleCheckTimer.Tick += ConsoleCheckTimer_Tick;
        _consoleCheckTimer.Start();
    }

    private void ConsoleCheckTimer_Tick(object sender, EventArgs e)
    {
        if (Utils.ConsoleHelper.GetConsoleWindow() == IntPtr.Zero)
        {
            _consoleCheckTimer.Tick -= ConsoleCheckTimer_Tick;
            Shutdown();
        }
    }
}