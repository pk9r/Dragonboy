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

        static Mutex mutex = new Mutex(true, "{b2dbc8db-7340-4a4a-8e63-f9ec86e5a4fd}");

        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Initialize();
                App.Main();
                discordClient?.ClearPresence();
                discordClient?.Dispose();
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
            discordClient?.Dispose();
            MessageBox.Show(Application.Current.MainWindow, $"Có lỗi xảy ra:{Environment.NewLine}{e.ExceptionObject}", "QLTK", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //Called when your application first starts.
        //For example, just before your main loop, on OnEnable for unity.
        static void Initialize()
        {
            /*
            Create a Discord client
            NOTE: 	If you are using Unity3D, you must use the full constructor and define
                     the pipe connection.
            */
            discordClient = new DiscordRpcClient("1055462814166294559")
            {
                ShutdownOnly = true
            };
            //Set the logger
            //client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            //Subscribe to events
            //client.OnReady += (sender, e) =>
            //{
            //    Console.WriteLine("Received Ready from user {0}", e.User.Username);
            //};

            //client.OnPresenceUpdate += (sender, e) =>
            //{
            //    Console.WriteLine("Received Update! {0}", e.Presence);
            //};
            //Connect to the RPC
            discordClient.Initialize();
        }
    }
}
