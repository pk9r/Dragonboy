using DiscordRPC.Logging;
using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLTK
{
    internal class Program
    {
        public static DiscordRpcClient discordClient;

        public static Timestamps timestampsStartQLTK = new Timestamps(DateTime.UtcNow);
            
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Initialize();
            App.Main();
            discordClient.Dispose();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            discordClient.Dispose();
            MessageBox.Show($"Có lỗi xảy ra:{Environment.NewLine}{e.ExceptionObject}", "QLTK", MessageBoxButton.OK, MessageBoxImage.Error);
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
            discordClient = new DiscordRpcClient("1055462814166294559");

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
