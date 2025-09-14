using System.Windows;
using System.Windows.Threading;
namespace DianaLab.GUI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private DispatcherTimer _consoleCheckTimer;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
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