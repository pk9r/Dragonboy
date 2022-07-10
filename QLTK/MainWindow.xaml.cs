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
            new Server() { name = "Võ đài Liên Trụ", ip = "dragonwar.teamobi.com", port = 20000, language = 0 },
            new Server() { name = "Naga", ip = "dragon.indonaga.com", port = 14446, language = 2 },
            new Server() { name = "Universe 1", ip = "dragon.indonaga.com", port = 14445, language = 2 },
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

        private static async Task ShowWindowAsync(IntPtr hWnd)
        {
            Utilities.ShowWindowAsync(hWnd, 1);
            Utilities.SetForegroundWindow(hWnd);
            await Task.Delay(100);

            Utilities.GetWindowRect(hWnd, out RECT rect);

            double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
            double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;

            if (rect.left < 0 || rect.right > primaryScreenWidth ||
                rect.top < 0 || rect.bottom > primaryScreenHeight)
            {
                Utilities.MoveWindow(
                    hWnd: hWnd,
                    x: 0, y: 0,
                    width: rect.right - rect.left,
                    height: rect.bottom - rect.top,
                    bRepaint: true);
            }
        }

        private static async Task ShowWindowsAsync(List<Account> accounts)
        {
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

        private static void ArrangeWindows(List<Account> accounts, int type)
        {
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

                cx += width / type;
                if (cx + width / type > maxWidth)
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
                this.DataGridAccount.ItemsSource = LitJson.JsonMapper.ToObject<List<Account>>(
                    File.ReadAllText(Settings.Default.PathAccounts));
            }
            catch
            {
                this.DataGridAccount.ItemsSource = new List<Account>();
                SaveAccounts();
            }
        }

        private bool UpdateSizeData()
        {
            var match = Regex.Match(TextBoxSize.Text, @"^\s*(\d+)x(\d+)\s*$");
            if (!match.Success)
                return false;

            sizeData = new
            {
                width = int.Parse(match.Groups[1].Value),
                height = int.Parse(match.Groups[2].Value),
                typeSize = ComboBoxTypeSize.SelectedIndex + 1,
                lowGraphic = ComboBoxLowGraphic.SelectedIndex
            };
            return true;
        }

        public void RefreshAccounts()
            => this.DataGridAccount.Items.Refresh();

        public List<Account> GetAllAccounts()
            => (List<Account>)this.DataGridAccount.ItemsSource;

        private Account GetSelectedAccount()
            => (Account)this.DataGridAccount.SelectedItem;

        private List<Account> GetSelectedAccounts()
            => this.DataGridAccount.SelectedItems.Cast<Account>().ToList();

        private Account GetInputAccount() => new Account()
        {
            username = this.TextBoxUsername.Text,
            password = this.PasswordBoxPassword.Password,
            indexServer = this.ComboBoxServer.SelectedIndex,
        };

        #region Save
        private void SaveAccounts()
        {
            try
            {
                File.WriteAllText(Settings.Default.PathAccounts,
                    LitJson.JsonMapper.ToJson(this.DataGridAccount.ItemsSource));
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
                    size = this.TextBoxSize.Text,
                    lowGraphic = this.ComboBoxLowGraphic.SelectedIndex,
                    typeSize = this.ComboBoxTypeSize.SelectedIndex,
                };

                File.WriteAllText(Settings.Default.PathSizeSettings,
                    LitJson.JsonMapper.ToJson(sizeSettings));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region Open Game
        private async Task LoginSelectedAccountsAsync()
        {
            if (!UpdateSizeData())
            {
                MessageBox.Show("Kích thước cửa sổ không hợp lệ");
                return;
            }

            var accounts = GetSelectedAccounts();
            if (accounts.Count() == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            GridMain.IsEnabled = false;
            await OpenGamesAsync(accounts);
            GridMain.IsEnabled = true;
        }

        private async Task LoginAllAccountsAsync()
        {
            if (!UpdateSizeData())
            {
                MessageBox.Show("Kích thước cửa sổ không hợp lệ");
                return;
            }

            var accounts = GetAllAccounts();

            GridMain.IsEnabled = false;
            await OpenGamesAsync(accounts);
            GridMain.IsEnabled = true;
        }

        private async Task OpenGamesAsync(List<Account> accounts)
        {
            foreach (var account in accounts)
            {
                await OpenGameAsync(account);
            }
        }

        private async Task OpenGameAsync(Account account)
        {
            if (account.process == null || account.process.HasExited)
            {
                account.status = "Đang khởi động...";
                RefreshAccounts();

                AsynchronousSocketListener.waitingAccounts.Add(account);

                account.process = Process.Start(Settings.Default.PathGame,
                    $"-port {Settings.Default.PortListener}");

                while (account.process.MainWindowHandle == IntPtr.Zero)
                {
                    await Task.Delay(50);
                }

                var hWnd = account.process.MainWindowHandle;
                Utilities.SetWindowText(hWnd, account.username);

                Utilities.GetWindowRect(hWnd, out RECT rect);
                Utilities.MoveWindow(
                    hWnd, x: rect.left - rect.right, y: 0,
                    width: rect.right - rect.left,
                    height: rect.bottom - rect.top,
                    bRepaint: true);
            }
        }
        #endregion

        #region Processes
        private async Task ShowAllWindowsAsync()
        {
            var accounts = this.GetAllAccounts();
            await ShowWindowsAsync(accounts);
        }

        private void ArrangeAllWindows(int type)
        {
            var accounts = this.GetAllAccounts();

            ArrangeWindows(accounts, type);
        }

        private async Task ShowAndArrangeWindows(int type)
        {
            var accounts = GetSelectedAccounts();
            if (accounts.Count <= 1)
            {
                await ShowAllWindowsAsync();
                this.ArrangeAllWindows(type);
                return;
            }

            await ShowWindowsAsync(accounts);
            ArrangeWindows(accounts, type);
        }

        private void KillSelectedProcesses()
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

        private void KillAllProcesses()
        {
            foreach (var account in GetAllAccounts())
            {
                if (account.process?.HasExited == false)
                {
                    account.process.Kill();
                }
            }
        }
        #endregion

        #region Send
        private void SendChatToSelectedAccounts()
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
                    account.sendMessage(new
                    {
                        action = "chat",
                        text = TextBoxChat.Text
                    });
                }
            }
        }

        private void SendKeyPressToSelectedAccounts(int keyCode)
        {
            var accounts = GetSelectedAccounts();

            foreach (var account in accounts)
            {
                if (account.workSocket?.Connected == true)
                {
                    account.sendMessage(new
                    {
                        action = "keyPress",
                        keyCode
                    });
                }
            }
        }

        private void SendKeyReleaseToSelectedAccounts(int keyCode)
        {
            var accounts = GetSelectedAccounts();

            foreach (var account in accounts)
            {
                if (account.workSocket?.Connected == true)
                {
                    account.sendMessage(new
                    {
                        action = "keyRelease",
                        keyCode
                    });
                }
            }
        }
        #endregion

        #region KeyPress
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
                case Key.Back:
                    keyCode = -8;
                    break;
                case Key.Oem2:
                    keyCode = 47;
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

            SendKeyPressToSelectedAccounts(keyCode);
        }

        private void ButtonKeyPress_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var button = (Button)sender;
            int keyCode = GetKeyCode(button);

            if (keyCode == 0)
                return;

            SendKeyReleaseToSelectedAccounts(keyCode);
        }

        private void Control_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int keyCode = GetKeyCode(e);
            e.Handled = true;

            if (keyCode == 0)
                return;

            SendKeyPressToSelectedAccounts(keyCode);
        }

        private void Control_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            int keyCode = GetKeyCode(e);
            e.Handled = true;

            if (keyCode == 0)
                return;

            SendKeyReleaseToSelectedAccounts(keyCode);
        }
        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            KillAllProcesses();

            SaveAccounts();
            SaveSizeSettings();
        }

        private void DataGridAccount_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            ((Account)e.Row.Item).number = e.Row.GetIndex();
        }

        private void DataGirdAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridAccount.SelectedItem is Account account)
            {
                TextBoxUsername.Text = account.username;
                PasswordBoxPassword.Password = account.password;
                ComboBoxServer.SelectedIndex = account.indexServer;
            }
        }

        private async void DataGridAccount_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridAccount.SelectedItem is Account account)
            {
                GridMain.IsEnabled = false;

                if (ExistedWindow(account, out IntPtr hWnd))
                {
                    await ShowWindowAsync(hWnd);
                    GridMain.IsEnabled = true;
                    return;
                }

                if (!UpdateSizeData())
                {
                    MessageBox.Show("Kích thước cửa sổ không hợp lệ");
                    GridMain.IsEnabled = true;
                    return;
                }

                await OpenGameAsync(account);
                GridMain.IsEnabled = true;
            }
        }

        private void ButtonSelecteAll_Click(object sender, RoutedEventArgs e)
        {
            DataGridAccount.SelectedItems.Clear();
            this.GetAllAccounts().ForEach(
                a => DataGridAccount.SelectedItems.Add(a));
        }

        private void ButtonAddAccount_Click(object sender, RoutedEventArgs e)
        {
            GetAllAccounts().Add(GetInputAccount());
            DataGridAccount.Items.Refresh();
            SaveAccounts();
        }

        private void ButtonEditAccount_Click(object sender, RoutedEventArgs e)
        {
            var account = GetSelectedAccount();
            if (account == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
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
                GetAllAccounts().Remove(account);

            SaveAccounts();
            DataGridAccount.Items.Refresh();
        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            await LoginSelectedAccountsAsync();
        }

        private async void ButtonLoginAll_Click(object sender, RoutedEventArgs e)
        {
            await LoginAllAccountsAsync();
        }

        private void ButtonKill_Click(object sender, RoutedEventArgs e)
        {
            KillSelectedProcesses();
        }


        private async void ButtonArrangeWindows1_Click(object sender, RoutedEventArgs e)
        {
            GridMain.IsEnabled = false;
            await ShowAndArrangeWindows(1);
            GridMain.IsEnabled = true;
        }

        private async void ButtonArrangeWindows2_Click(object sender, RoutedEventArgs e)
        {
            GridMain.IsEnabled = false;
            await ShowAndArrangeWindows(1);
            GridMain.IsEnabled = true;
        }

        private void ButtonChat_Click(object sender, RoutedEventArgs e)
        {
            SendChatToSelectedAccounts();
        }

        private void TextBoxChat_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ButtonChat_Click(sender, null);
                e.Handled = true;
            }
        }
    }
}
