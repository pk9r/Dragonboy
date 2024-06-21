using DiscordRPC;
using HardwareId;
using QLTK.Models;
using QLTK.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace QLTK
{
    public partial class Utilities
    {
        internal static List<Server> LoadServersFromFile()
        {
            List<Server> servers = [];
            if (File.Exists("ModData\\Servers.txt"))
                foreach (string server in File.ReadAllLines("ModData\\Servers.txt"))
                {
                    try
                    {
                        if (server.StartsWith('#'))
                            continue;
                        string[] strings = server.Split('|');
                        servers.Add(new Server(strings[0], strings[1], int.Parse(strings[2]), int.Parse(strings[3])));
                    }
                    catch (Exception) { }
                }
            else 
                File.WriteAllText("ModData\\Servers.txt", "# tên server|ip server|port|ngôn ngữ (0: tiếng Việt, 1: tiếng Anh, 2: tiếng Indonesia");
            return servers;
        }

        public static string EncryptString(string data)
        {
            //using var tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
            //{
            //    KeySize = 128,
            //    BlockSize = 64,
            //    Padding = PaddingMode.PKCS7,
            //    Mode = CipherMode.CBC,
            //    Key = Key,
            //    IV = IV
            //};
            using var des = TripleDES.Create();
            des.KeySize = 128;
            des.BlockSize = 64;
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.CBC;
            des.Key = Key;
            des.IV = IV;

            using var cryptoTransform = des.CreateEncryptor();
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            var final = cryptoTransform.TransformFinalBlock(buffer, 0, buffer.Length);
            return Convert.ToBase64String(final);
        }

        public static string DecryptString(string data)
        {
            using var des = TripleDES.Create();
            des.KeySize = 128;
            des.BlockSize = 64;
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.CBC;
            des.Key = Key;
            des.IV = IV;

            using var cryptoTransform = des.CreateDecryptor(Key, IV);

            var buffer = Convert.FromBase64String(data);
            var final = cryptoTransform.TransformFinalBlock(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(final);
        }

        static byte[] Key
        {
            get
            {
                var hwid = HWID.getHWID(true, false, true, true);
                var hwidBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(hwid));
                var s = hwidBase64 + "6VRRnrPsZfd6FtAqlqNUixYO7spOLu8P";
                return MD5.HashData(Encoding.UTF8.GetBytes(s));
            }
        }

        static byte[] IV
        {
            get
            {
                byte[] buffer = new byte[8];

                var hwid = HWID.getHWID(true, false, true, true);
                var hwidBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(hwid));
                var s = hwidBase64 + "zXIDQhn6sgNm8l1ArxvOmcoCVK0OuGr6";

                var hash = MD5.HashData(Encoding.UTF8.GetBytes(s));
                for (int i = 0; i < hash.Length; i++)
                    hash[i] ^= 113;

                Random random = new Random(BitConverter.ToInt32(hash, 0));
                random.NextBytes(buffer);

                return buffer;
            }
        }

        public static MainWindow GetMainWindow()
        {
            return (MainWindow)Application.Current.MainWindow;
        }

        #region winAPI
        [LibraryImport("user32.dll", EntryPoint = "SetWindowTextW", StringMarshalling = StringMarshalling.Utf16)]
        public static partial int SetWindowText(IntPtr hWnd, string text);


        [LibraryImport("user32.dll", EntryPoint = "GetWindowRect")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetWindowRect(IntPtr hWnd, out RECT lpRect);


        [LibraryImport("user32.dll", EntryPoint = "MoveWindow", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, [MarshalAs(UnmanagedType.Bool)] bool bRepaint);


        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetForegroundWindow(IntPtr hWnd);


        [LibraryImport("user32.dll", EntryPoint = "ShowWindowAsync")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);


        [LibraryImport("user32.dll", EntryPoint = "MessageBoxW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        public static partial int MessageBoxNative(IntPtr hWnd, string text, string caption, int type);
        #endregion

        public static string Base64StringEncode(string originalString)
        {
            var bytes = Encoding.UTF8.GetBytes(originalString);

            var encodedString = Convert.ToBase64String(bytes);

            return encodedString;
        }

        public static string Base64StringDecode(string encodedString)
        {
            var bytes = Convert.FromBase64String(encodedString);

            var decodedString = Encoding.UTF8.GetString(bytes);

            return decodedString;
        }

        internal static async Task CheckUpdateAndNotification()
        {
            try
            {
                using var client = new HttpClient();

                string linkNotification = Settings.Default.LinkNotification;
                string[] notifications = (await client.GetStringAsync(linkNotification)).Split('\n');

                if (SaveSettings.Instance.versionNotification != notifications[0])
                {
                    for (int i = 1; i < notifications.Length; i++)
                    {
                        notifications[i] = notifications[i].Trim();
                        if (!string.IsNullOrWhiteSpace(notifications[i]))
                        {
                            MessageBox.Show(
                                messageBoxText: notifications[i],
                                caption: "Thông báo",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    SaveSettings.Instance.versionNotification = notifications[0];
                }

            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("Không thể kết nối đến máy chủ!" + Environment.NewLine + ex, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra:" + Environment.NewLine + ex, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        internal static void SetPresence(string state = "Chưa đăng nhập", string details = "", Timestamps timeStamps = null)
        {
            if (Program.isDiscordRichPresenceDisabled)
                return;
            string name = "Mod Cộng Đồng";
            RichPresence richPresence = new RichPresence()
            {
                State = state,
                Assets = new Assets()
                {
                    LargeImageKey = "icon_large",
                    LargeImageText = name,
                }
            };
            if (timeStamps != null)
                richPresence.Timestamps = timeStamps;
            if (!string.IsNullOrEmpty(details))
                richPresence.Details = details;
            Program.discordClient.SetPresence(richPresence);
        }
    }
}
