using QLTK.Models;
using QLTK.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QLTK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static object sizeData = null;

        public static List<Server> Servers = new List<Server>()
        {
            new Server() { name = "Vũ trụ 1", ip = "dragon1.teamobi.com", port = 14445, language = 0},
            new Server() { name = "Vũ trụ 2", ip = "dragon2.teamobi.com", port = 14445, language = 0 },
            new Server() { name = "Vũ trụ 3", ip = "dragon3.teamobi.com", port = 14445, language = 0 },
            new Server() { name = "Vũ trụ 4", ip = "dragon4.teamobi.com", port = 14445, language = 0 },
            new Server() { name = "Vũ trụ 5", ip = "dragon5.teamobi.com", port = 14445, language = 0 },
            new Server() { name = "Vũ trụ 6", ip = "dragon6.teamobi.com", port = 14445, language = 0 },
            new Server() { name = "Vũ trụ 7", ip = "dragon7.teamobi.com", port = 14445, language = 0 },
            new Server() { name = "Vũ trụ 8", ip = "dragon8.teamobi.com", port = 14445, language = 0 },
            new Server() { name = "Vũ trụ 9", ip = "dragon9.teamobi.com", port = 14445, language = 0 },
            new Server() { name = "Vũ trụ 10", ip = "dragon10.teamobi.com", port = 14445, language = 0 },
            new Server() { name = "Võ đài Liên Vũ Trụ", ip = "dragonwar.teamobi.com", port = 20000, language = 0 },
            new Server() { name = "Indonaga", ip = "dragon.indonaga.com", port = 14446, language = 2 },
            new Server() { name = "Universe 1", ip = "dragon.indonaga.com", port = 14445, language = 2 },
            //Server Blue không chạy được phiên bản 217
            //Các server lậu khác chưa test
            new Server("Blue 01", "103.48.194.146", 14445),
            new Server("Blue 02", "103.48.194.152", 14445),
            new Server("Blue 03", "45.119.81.28", 14445),
            new Server("Blue 04", "45.119.81.51", 14445),
            new Server("Blue 05", "103.48.194.173", 14445),
            new Server("Blue 06", "103.48.194.137", 14445),
            new Server("Blue 07", "103.48.194.159", 14445),
            new Server("Blue 08", "103.48.194.139", 14445),

            new Server("Green 01", "103.48.194.46", 14445),

            new Server("Dream 1", "14.225.198.30", 14446),
            new Server("Dream 2", "14.225.198.30", 14447),

            new Server("NroZ 1", "222.255.214.169", 14445),
            new Server("NroZ 2", "222.255.214.169", 14445),

            new Server("Vũ Trụ Kakarot", "103.90.224.247", 14445),

            new Server("NROLOVE 1", "103.200.22.220", 14446),
            new Server("NROLOVE 2", "103.27.236.54", 14446),

            new Server("Private 1", "222.255.214.140", 14445),

            new Server("SUPER 1", "103.90.224.245", 14446),
            new Server("SUPER 2", "103.90.224.245", 14447),
        };

        public MainWindow()
        {
            InitializeComponent();

            new Thread(() => AsynchronousSocketListener.StartListening())
            {
                IsBackground = true
            }.Start();

            ComboBoxServer.ItemsSource = Servers;
            ComboBoxServer.DisplayMemberPath = "name";
            ComboBoxServer.SelectedIndex = 0;

            LoadAccounts();
            LoadSizeSettings();
        }

        private void LoadSizeSettings()
        {
            var sizeSettings = new SizeSettings();
            try
            {
                sizeSettings = LitJson.JsonMapper.ToObject<SizeSettings>(
                    File.ReadAllText(Settings.Default.PathSizeSettings));
            }
            catch
            {
            }

            TextBoxSize.Text = sizeSettings.size;
            ComboBoxLowGraphic.SelectedIndex = sizeSettings.lowGraphic;
            ComboBoxTypeSize.SelectedIndex = sizeSettings.typeSize;
        }

        private void LoadAccounts()
        {
            try
            {
                DataGridAccount.ItemsSource = LitJson.JsonMapper.ToObject<List<Account>>(
                    Utilities.DecryptString(File.ReadAllText(Settings.Default.PathAccounts)));
            }
            catch
            {
                DataGridAccount.ItemsSource = new List<Account>();
                SaveAccounts();
            }
        }

        public void RefreshAccounts()
            => DataGridAccount.Items.Refresh();

        public List<Account> GetAccounts()
            => (List<Account>)DataGridAccount.ItemsSource;

        private List<Account> GetSelectedAccounts()
            => DataGridAccount.SelectedItems.Cast<Account>().ToList();

        private Account GetSelectedAccount()
            => (Account)DataGridAccount.SelectedItem;

        private Account GetInputAccount() => new Account()
        {
            username = TextBoxUsername.Text,
            password = PasswordBoxPassword.Password,
            indexServer = ComboBoxServer.SelectedIndex,
        };

        private void SaveAccounts()
        {
            try
            {
                File.WriteAllText(Settings.Default.PathAccounts,
                    Utilities.EncryptString(LitJson.JsonMapper.ToJson(DataGridAccount.ItemsSource)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        private void SaveSizeSettings()
        {
            try
            {
                var sizeSettings = new SizeSettings()
                {
                    size = TextBoxSize.Text,
                    lowGraphic = ComboBoxLowGraphic.SelectedIndex,
                    typeSize = ComboBoxTypeSize.SelectedIndex,
                };

                File.WriteAllText(Settings.Default.PathSizeSettings,
                    LitJson.JsonMapper.ToJson(sizeSettings));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private static async Task OpenGamesAsync(List<Account> accounts)
        {
            foreach (var account in accounts)
            {
                if (account.process == null || account.process.HasExited)
                    await OpenGameAsync(account);
            }
        }

        private static async Task OpenGameAsync(Account account)
        {
            account.status = "Đang khởi động...";
            Utilities.RefreshAccounts();

            AsynchronousSocketListener.waitingAccounts.Add(account);

            account.process = Process.Start(Settings.Default.PathGame,
                $"-port {Settings.Default.PortListener}");

            while (account.process.MainWindowHandle == IntPtr.Zero)
            {
                await Task.Delay(50);
            }

            Utilities.SetWindowText(
                hWnd: account.process.MainWindowHandle,
                text: account.username);

            Utilities.GetWindowRect(account.process.MainWindowHandle, out RECT rect);
            Utilities.MoveWindow(
                hWnd: account.process.MainWindowHandle,
                x: (int)SystemParameters.PrimaryScreenWidth, y: 0,
                width: rect.right - rect.left,
                height: rect.bottom - rect.top,
                bRepaint: true);
        }

        private static bool ExistedWindow(Account account, out IntPtr hWnd)
        {
            hWnd = IntPtr.Zero;
            if (account.process == null || account.process.HasExited)
            {
                return false;
            }

            hWnd = account.process.MainWindowHandle;
            return hWnd != IntPtr.Zero;
        }

        private async Task ShowWindowsAsync()
        {
            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                if (ExistedWindow(account, out IntPtr hWnd))
                {
                    Utilities.ShowWindowAsync(hWnd, 1);
                    Utilities.SetForegroundWindow(hWnd);
                }
            }
            await Task.Delay(1000);
        }

        private void ArrangeWindows()
        {
            var accounts = GetAccounts();

            var maxWidth = SystemParameters.PrimaryScreenWidth;
            var maxHeight = SystemParameters.PrimaryScreenHeight;

            int cx = 0, cy = 0;

            for (int i = 0; i < accounts.Count; i++)
            {
                if (!ExistedWindow(accounts[i], out IntPtr hWnd))
                    continue;

                if (!Utilities.GetWindowRect(hWnd, out RECT rect))
                    continue;

                int width = rect.right - rect.left;
                int height = rect.bottom - rect.top;

                Utilities.MoveWindow(hWnd, cx, cy, width, height, true);

                cx += width / 2;
                if (cx + (width / 2) > maxWidth)
                {
                    cx = 0;
                    cy += height - 5;
                }
                if (cy + height > maxHeight)
                {
                    cy = 0;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            foreach (var account in GetAccounts())
            {
                if (account.process?.HasExited == false)
                {
                    account.process.Kill();
                }
            }
            SaveAccounts();
            SaveSizeSettings();
        }

        private void ListViewAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridAccount.SelectedItem is Account account)
            {
                TextBoxUsername.Text = account.username;
                PasswordBoxPassword.Password = account.password;
                ComboBoxServer.SelectedIndex = account.indexServer;
            }
            GridControl.Focus();
        }

        private void ListViewAccount_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridAccount.SelectedItem is Account account)
            {
                if (ExistedWindow(account, out IntPtr hWnd))
                {
                    // Hiển thị cửa sổ
                    Utilities.ShowWindowAsync(hWnd, 1);
                    Utilities.SetForegroundWindow(hWnd);
                    Thread.Sleep(50);

                    Utilities.GetWindowRect(hWnd, out RECT rect);

                    int width = rect.right - rect.left;
                    int height = rect.bottom - rect.top;
                    Utilities.MoveWindow(hWnd,
                        x: (int)(SystemParameters.PrimaryScreenWidth / 2) - width / 2,
                        y: (int)(SystemParameters.PrimaryScreenHeight / 2) - height / 2,
                        width, height, true);
                }
            }
        }

        private void ButtonAddAccount_Click(object sender, RoutedEventArgs e)
        {
            GetAccounts().Add(GetInputAccount());
            DataGridAccount.Items.Refresh();
            SaveAccounts();
        }

        private void ButtonEditAccount_Click(object sender, RoutedEventArgs e)
        {
            var account = GetSelectedAccount();
            if (account == null)
            {
                return;
            }

            var inputAccount = GetInputAccount();
            account.username = inputAccount.username;
            account.password = inputAccount.password;
            account.indexServer = inputAccount.indexServer;

            DataGridAccount.Items.Refresh();
            SaveAccounts();
        }

        private void ButtonDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            var accounts = GetSelectedAccounts();
            foreach (var account in accounts)
                GetAccounts().Remove(account);

            SaveAccounts();
            DataGridAccount.Items.Refresh();
        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            var accounts = GetSelectedAccounts();
            if (accounts.Count() == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            string sizeStr = TextBoxSize.Text;
            var match = Regex.Match(sizeStr, @"^\s*(\d+)x(\d+)\s*$");
            if (!match.Success)
            {
                MessageBox.Show("Kích thước cửa sổ không hợp lệ");
                return;
            }

            int width = int.Parse(match.Groups[1].Value);
            int height = int.Parse(match.Groups[2].Value);

            sizeData = new
            {
                width,
                height,
                typeSize = ComboBoxTypeSize.SelectedIndex + 1,
                lowGraphic = ComboBoxLowGraphic.SelectedIndex
            };

            GridMain.IsEnabled = false;
            await OpenGamesAsync(accounts);
            GridMain.IsEnabled = true;
        }

        private async void ButtonLoginAll_Click(object sender, RoutedEventArgs e)
        {
            var accounts = GetAccounts();

            string sizeStr = TextBoxSize.Text;
            var match = Regex.Match(sizeStr, @"^\s*(\d+)x(\d+)\s*$");
            if (!match.Success)
            {
                MessageBox.Show("Kích thước cửa sổ không hợp lệ");
                return;
            }

            int width = int.Parse(match.Groups[1].Value);
            int height = int.Parse(match.Groups[2].Value);

            sizeData = new
            {
                width,
                height,
                typeSize = ComboBoxTypeSize.SelectedIndex + 1,
                lowGraphic = ComboBoxLowGraphic.SelectedIndex
            };

            GridMain.IsEnabled = false;
            await OpenGamesAsync(accounts);
            GridMain.IsEnabled = true;
        }

        private void ButtonKill_Click(object sender, RoutedEventArgs e)
        {
            var accounts = GetSelectedAccounts();
            if (accounts.Count() == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            foreach (var account in accounts)
            {
                if (account.process?.HasExited == false)
                {
                    account.process.Kill();
                }
            }
        }

        private async void ButtonArrangeWindows_Click(object sender, RoutedEventArgs e)
        {
            await ShowWindowsAsync();
            ArrangeWindows();
        }

        private void ListViewAccount_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            ((Account)e.Row.Item).number = e.Row.GetIndex();
        }

        private void ButtonChat_Click(object sender, RoutedEventArgs e)
        {
            var accounts = GetSelectedAccounts();
            if (accounts.Count() == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            foreach (var account in accounts)
            {
                if (account.workSocket?.Connected == true)
                {
                    AsynchronousSocketListener.sendMessage(account, new
                    {
                        action = "chat",
                        text = TextBoxChat.Text
                    });
                }
            }
        }

        private static int GetKeyCode(Button button)
        {
            int keyCode;
            switch (button.Content)
            {
                case "▲":
                    keyCode = -1;
                    break;
                case "▼":
                    keyCode = -2;
                    break;
                case "◀":
                    keyCode = -3;
                    break;
                case "▶":
                    keyCode = -4;
                    break;
                case "↲":
                    keyCode = -5;
                    break;
                case "F1":
                    keyCode = -21;
                    break;
                case "F2":
                    keyCode = -22;
                    break;
                default:
                    keyCode = ((string)button.Content)[0];
                    break;
            }
            return keyCode;
        }

        private static int GetKeyCode(KeyEventArgs e)
        {
            int keyCode;
            switch (e.Key)
            {
                case Key.Up:
                    keyCode = -1;
                    break;
                case Key.Down:
                    keyCode = -2;
                    break;
                case Key.Left:
                    keyCode = -3;
                    break;
                case Key.Right:
                    keyCode = -4;
                    break;
                case Key.Enter:
                    keyCode = -5;
                    break;
                case Key.F1:
                    keyCode = -21;
                    break;
                case Key.F2:
                    keyCode = -22;
                    break;
                case Key.Tab:
                    keyCode = -26;
                    break;
                case Key.Space:
                    keyCode = 32;
                    break;
                default:
                    keyCode = (int)e.Key;
                    if (keyCode >= 34 && keyCode <= 43)
                        keyCode += 14;
                    else if (keyCode >= 44 && keyCode <= 69)
                        keyCode += 53;
                    break;
            }
            return keyCode;
        }

        private void ButtonKeyPress_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var button = (Button)sender;
            int keyCode = GetKeyCode(button);

            if (keyCode == 0)
                return;

            var accounts = GetSelectedAccounts();
            if (accounts.Count() == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            foreach (var account in accounts)
            {
                if (account.workSocket?.Connected == true)
                {
                    AsynchronousSocketListener.sendMessage(account, new
                    {
                        action = "keyPress",
                        keyCode
                    });
                }
            }
        }

        private void ButtonKeyPress_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var button = (Button)sender;
            int keyCode = GetKeyCode(button);

            if (keyCode == 0)
                return;

            var accounts = GetSelectedAccounts();
            if (accounts.Count() == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            foreach (var account in accounts)
            {
                if (account.workSocket?.Connected == true)
                {
                    AsynchronousSocketListener.sendMessage(account, new
                    {
                        action = "keyRelease",
                        keyCode
                    });
                }
            }
        }

        private void GridControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int keyCode = GetKeyCode(e);
            e.Handled = true;

            if (keyCode == 0)
                return;

            var accounts = GetSelectedAccounts();
            if (accounts.Count() == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            foreach (var account in accounts)
            {
                if (account.workSocket?.Connected == true)
                {
                    AsynchronousSocketListener.sendMessage(account, new
                    {
                        action = "keyPress",
                        keyCode
                    });
                }
            }
        }

        private void GridControl_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            int keyCode = GetKeyCode(e);
            e.Handled = true;
            
            if (keyCode == 0)
                return;

            var accounts = GetSelectedAccounts();
            if (accounts.Count() == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            foreach (var account in accounts)
            {
                if (account.workSocket?.Connected == true)
                {
                    AsynchronousSocketListener.sendMessage(account, new
                    {
                        action = "keyRelease",
                        keyCode
                    });
                }
            }
        }

        private void ButtonControl_Click(object sender, RoutedEventArgs e)
        {
            GridControl.Focus();
        }

        private void ButtonSelecteAll_Click(object sender, RoutedEventArgs e)
        {
            DataGridAccount.SelectedItems.Clear();
            GetAccounts().ForEach(
                a => DataGridAccount.SelectedItems.Add(a));
        }
    }
}
