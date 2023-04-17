using DiscordRPC;
using HardwareId;
using QLTK.Models;
using QLTK.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;

namespace QLTK
{
    public class Utilities
    {
        internal static DevelopStatus currentDevelopStatus;

        [DllImport("user32.dll", EntryPoint = "MessageBox", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int MessageBoxNative(IntPtr hWnd, string text, string caption, int type);

        internal static List<Server> LoadServersFromFile()
        {
            List<Server> servers = new List<Server>();
            if (File.Exists("ModData\\Servers.txt"))
                foreach(string server in File.ReadAllLines("ModData\\Servers.txt"))
                {
                    try
                    {
                        string[] strings = server.Split(new char[] { '|' });
                        servers.Add(new Server(strings[0], strings[1], int.Parse(strings[2]), int.Parse(strings[3])));
                    }
                    catch (Exception) { }
                }
            return servers;
        }

        public static string EncryptString(string data)
        {
            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                KeySize = 128,
                BlockSize = 64,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                Key = Key,
                IV = IV
            };
            ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length));
        }

        public static string DecryptString(string data)
        {
            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                KeySize = 128,
                BlockSize = 64,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                Key = Key,
                IV = IV
            };
            ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor(tripleDESCryptoServiceProvider.Key, tripleDESCryptoServiceProvider.IV);
            byte[] array = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(cryptoTransform.TransformFinalBlock(array, 0, array.Length));
        }

        static byte[] Key
        {
            get
            {
                return new MD5CryptoServiceProvider()
                    .ComputeHash(Encoding.UTF8.GetBytes(
                        Convert.ToBase64String(
                            Encoding.UTF8.GetBytes(
                                HWID.getHWID(true, false, true, true))) +
                                "6VRRnrPsZfd6FtAqlqNUixYO7spOLu8P"));
            }
        }

        static byte[] IV
        {
            get
            {
                byte[] bytes = new byte[8];
                byte[] bytes1 = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(HWID.getHWID(true, false, true, true))) + "zXIDQhn6sgNm8l1ArxvOmcoCVK0OuGr6"));
                for (int i = 0; i < bytes1.Length; i++) bytes1[i] ^= 113;
                Random random = new Random(BitConverter.ToInt32(bytes1, 0));
                random.NextBytes(bytes);
                return bytes;
            }
        }

        public static MainWindow GetMainWindow()
        {
            return (MainWindow)Application.Current.MainWindow;
        }

        #region winAPI
        [DllImport("user32.dll")]
        public static extern int SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool bRepaint);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
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

        internal static void CheckUpdateAndNotification()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string[] notifications = Encoding.UTF8.GetString(client.DownloadData(Settings.Default.LinkNotification)).Split('\n');
                    if (SaveSettings.Instance.versionNotification != notifications[0])
                    {
                        for (int i = 1; i < notifications.Length; i++)
                        {
                            notifications[i] = notifications[i].Trim();
                            if (notifications[i] != "")
                            {
                                MessageBox.Show(notifications[i], "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        SaveSettings.Instance.versionNotification = notifications[0];
                    }
                }
                switch (currentDevelopStatus = GetCurrentDevelopStatus())
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
            catch (WebException ex)
            {
                MessageBox.Show("Không thể kết nối đến máy chủ!" + Environment.NewLine + ex, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra:" + Environment.NewLine + ex, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static DevelopStatus GetCurrentDevelopStatus()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
                    string[] remoteInfo = Encoding.UTF8.GetString(client.DownloadData(Settings.Default.LinkHash)).Split('\n');

                    string hashGameAssemblyLocal = BitConverter.ToString(md5CryptoServiceProvider.ComputeHash(File.ReadAllBytes(@"Game_Data\Managed\Assembly-CSharp.dll"))).Replace("-", "");
                    string hashQLTKLocal = BitConverter.ToString(md5CryptoServiceProvider.ComputeHash(File.ReadAllBytes("QLTK.exe"))).Replace("-", "");

                    string hashGameAssemblyRemote = remoteInfo[0].TrimStart('\ufeff');
                    string hashQLTKRemote = remoteInfo[2];

                    if (hashQLTKLocal != hashQLTKRemote || hashGameAssemblyLocal != hashGameAssemblyRemote)
                    {
                        int timeStampGameAssemblyRemote = int.Parse(remoteInfo[1]);
                        int timeStampQLTKRemote = int.Parse(remoteInfo[3]);
                        int timeStampGameAssemblyLocal = BitConverter.ToInt32(File.ReadAllBytes(@"Game_Data\Managed\Assembly-CSharp.dll"), 0x00000088);
                        int timeStampQLTKLocal = BitConverter.ToInt32(File.ReadAllBytes(@"QLTK.exe"), 0x00000088);
                        if (timeStampGameAssemblyLocal >= timeStampGameAssemblyRemote || timeStampQLTKLocal >= timeStampQLTKRemote)
                            return DevelopStatus.Developing;
                        else if (timeStampGameAssemblyLocal < timeStampGameAssemblyRemote || timeStampQLTKLocal < timeStampQLTKRemote)
                            return DevelopStatus.OldVersion;
                    }
                    else
                        return DevelopStatus.NormalUser;
                }
            }
            catch (WebException ex)
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
