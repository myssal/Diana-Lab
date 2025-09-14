using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace DianaLab.GUI.Utils;

[SuppressUnmanagedCodeSecurity]
public static class ConsoleHelper
{
    private const int MF_BYCOMMAND = 0x00000000;
    private const int SC_CLOSE = 0xF060;

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    public static void Show()
    {
        AllocConsole();

        // Redirect Console.Out to the console window
        var consoleOutput = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
        Console.SetOut(consoleOutput);
        Console.SetError(consoleOutput);
    }
}