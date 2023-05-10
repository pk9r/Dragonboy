using DiscordRPC;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace QLTK
{
    internal class Program
    {
        public static DiscordRpcClient discordClient;

        public static Timestamps timestampsStartQLTK = new Timestamps(DateTime.UtcNow);

        public static bool isDiscordRichPresenceDisabled;

        static Mutex mutex = new Mutex(true, "{b2dbc8db-7340-4a4a-8e63-f9ec86e5a4fd}");

        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                if (args.Any(s => s == "--disable-discord-rich-presence"))
                    isDiscordRichPresenceDisabled = true;
                Initialize();
                App.Main();
                if (!isDiscordRichPresenceDisabled)
                {
                    discordClient?.ClearPresence();
                    discordClient?.Dispose();
                }
                mutex.ReleaseMutex();
            }
            else
            {
                Process otherInstance = Process.GetProcessesByName(Assembly.GetEntryAssembly().GetName().Name).First(p => p.MainWindowHandle != IntPtr.Zero);
                Utilities.ShowWindowAsync(otherInstance.MainWindowHandle, 9);
                Utilities.SetForegroundWindow(otherInstance.MainWindowHandle);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!isDiscordRichPresenceDisabled)
                discordClient?.Dispose();
            MessageBox.Show(Application.Current.MainWindow, $"Có lỗi xảy ra:{Environment.NewLine}{e.ExceptionObject}", "QLTK", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        static void Initialize()
        {
            discordClient = new DiscordRpcClient("1055462814166294559")
            {
                ShutdownOnly = true
            };
            if (isDiscordRichPresenceDisabled)
                return;
            discordClient.Initialize();
        }
    }
}
