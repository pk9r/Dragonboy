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
        internal static DevelopStatus currentDevelopStatus;

        internal static List<Server> LoadServersFromFile()
        {
            List<Server> servers = [];
            if (File.Exists("ModData\\Servers.txt"))
                foreach (string server in File.ReadAllLines("ModData\\Servers.txt"))
                {
                    try
                    {
                        string[] strings = server.Split(['|']);
                        servers.Add(new Server(strings[0], strings[1], int.Parse(strings[2]), int.Parse(strings[3])));
                    }
                    catch (Exception) { }
                }
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

                switch (currentDevelopStatus = await GetCurrentDevelopStatus())
                {
                    case DevelopStatus.None:
                        throw new NullReferenceException(nameof(currentDevelopStatus) + " is not initialized!");
                    case DevelopStatus.NormalUser:
                        break;
                    case DevelopStatus.Developing:
                        new Thread(() =>
                        MessageBoxNative(Process.GetCurrentProcess().MainWindowHandle, "Nếu bạn có ý tưởng hay chức năng mới, đừng ngại ngần mà hãy đóng góp cho Mod Cộng Đồng!", "Thông báo", 0x00000040 | 0x00040000)
                        ).Start();
                        break;
                    case DevelopStatus.OldVersion:
                        new Thread(() =>
                        {
                            if (MessageBoxNative(Process.GetCurrentProcess().MainWindowHandle, $"Đã có phiên bản mới!{Environment.NewLine}Bạn có muốn cập nhật không?", "Cập nhật", 0x00000004 | 0x00000040 | 0x00040000) == 6)
                                Process.Start("https://github.com/pk9r327/Dragonboy");
                        }).Start();
                        break;
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

        private static async Task<DevelopStatus> GetCurrentDevelopStatus()
        {
            try
            {
                using var client = new HttpClient();

                string linkHash = Settings.Default.LinkHash;
                string[] remoteInfo = (await client.GetStringAsync(linkHash)).Split('\n');

                string hashGameAssemblyLocal = BitConverter.ToString(MD5.HashData(File.ReadAllBytes(@"Game_Data\Managed\Assembly-CSharp.dll"))).Replace("-", "");
                string hashQLTKLocal = BitConverter.ToString(MD5.HashData(File.ReadAllBytes("QLTK.dll"))).Replace("-", "");

                string hashGameAssemblyRemote = remoteInfo[0].TrimStart('\ufeff');
                string hashQLTKRemote = remoteInfo[2];

                if (hashQLTKLocal != hashQLTKRemote || hashGameAssemblyLocal != hashGameAssemblyRemote)
                {
                    int timeStampGameAssemblyRemote = int.Parse(remoteInfo[1]);
                    int timeStampQLTKRemote = int.Parse(remoteInfo[3]);
                    int timeStampGameAssemblyLocal = BitConverter.ToInt32(File.ReadAllBytes(@"Game_Data\Managed\Assembly-CSharp.dll"), 0x00000088);
                    int timeStampQLTKLocal = BitConverter.ToInt32(File.ReadAllBytes(@"QLTK.dll"), 0x00000088);
                    if (timeStampGameAssemblyLocal >= timeStampGameAssemblyRemote || timeStampQLTKLocal >= timeStampQLTKRemote)
                        return DevelopStatus.Developing;
                    else if (timeStampGameAssemblyLocal < timeStampGameAssemblyRemote || timeStampQLTKLocal < timeStampQLTKRemote)
                        return DevelopStatus.OldVersion;
                }
                else
                    return DevelopStatus.NormalUser;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("Không thể kết nối đến máy chủ!" + Environment.NewLine + ex, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return DevelopStatus.NormalUser;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra:" + Environment.NewLine + ex, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return DevelopStatus.NormalUser;
            }
            return DevelopStatus.None;
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
            if (currentDevelopStatus == DevelopStatus.Developing)
            {
                richPresence.Assets.SmallImageKey = "icon_developing";
                richPresence.Assets.SmallImageText = "Đang phát triển";
            }
            Program.discordClient.SetPresence(richPresence);
        }

        internal static string GetWindowTitle()
        {
            string name = "QLTK - NRO";
            switch (currentDevelopStatus)
            {
                case DevelopStatus.None:
                case DevelopStatus.NormalUser:
                    break;
                case DevelopStatus.Developing:
                    name += " [Chế độ phát triển]";
                    break;
                case DevelopStatus.OldVersion:
                    name += " [Phiên bản cũ]";
                    break;
            }
            return name;
        }
    }

    enum DevelopStatus
    {
        None,
        NormalUser,
        Developing,
        OldVersion
    }
}
