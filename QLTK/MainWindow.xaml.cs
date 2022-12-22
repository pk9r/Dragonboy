using QLTK.Models;
using QLTK.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
            new Server("Vũ trụ 1", "dragon1.teamobi.com", 14445),
            new Server("Vũ trụ 2", "dragon2.teamobi.com", 14445),
            new Server("Vũ trụ 3", "dragon3.teamobi.com", 14445),
            new Server("Vũ trụ 4", "dragon4.teamobi.com", 14445),
            new Server("Vũ trụ 5", "dragon5.teamobi.com", 14445),
            new Server("Vũ trụ 6", "dragon6.teamobi.com", 14445),
            new Server("Vũ trụ 7", "dragon7.teamobi.com", 14445),
            new Server("Vũ trụ 8", "dragon8.teamobi.com", 14445),
            new Server("Vũ trụ 9", "dragon9.teamobi.com", 14445),
            new Server("Vũ trụ 10", "dragon10.teamobi.com", 14445),
            new Server("Vũ trụ 11", "dragon11.teamobi.com", 14445),
            new Server("Võ đài Liên Vũ Trụ", "dragonwar.teamobi.com", 20000),
            new Server("Universe 1", "dragon.indonaga.com", 14445, 2),
            new Server("Indonaga", "dragon.indonaga.com", 14446, 2),
        };

        public static object settings;

        static int width, height;

        static bool isCheckedStateChangedByCode;
        static MainWindow()
        {
            Servers.AddRange(Utilities.LoadServersFromFile());
            Servers.Add(new Server("Local", "127.0.0.1", 14445));
        }

        public MainWindow()
        {
            this.InitializeComponent();

            new Thread(() => AsynchronousSocketListener.StartListening())
            {
                IsBackground = true
            }.Start();

            this.ServerComboBox.ItemsSource = Servers;
            this.ServerComboBox.DisplayMemberPath = "name";
            this.ServerComboBox.SelectedIndex = 0;

            this.LoadAccounts();
            this.LoadSaveSettings();
            if (SaveSettings.Instance.indexConnectToDiscordRPC >= 0 && SaveSettings.Instance.indexConnectToDiscordRPC < AccountsDataGrid.Items.Count)
                SaveSettings.accountConnectToDiscordRPC = AccountsDataGrid.Items[SaveSettings.Instance.indexConnectToDiscordRPC] as Account;
            Program.discordClient.SetPresence(new DiscordRPC.RichPresence()
            {
                State = "Chưa đăng nhập",
                Assets = new DiscordRPC.Assets()
                {
                    LargeImageKey = "icon_large",
                    LargeImageText = "Mod Cộng Đồng",
                }
            });
            if (WinVersion.GetVersion().Major < 8) return;
            using (WebClient client = new WebClient())
            {
                var data = client.DownloadData(Settings.Default.LinkNotification);
                var strings = Encoding.UTF8.GetString(data).Split('\n');
                if (SaveSettings.Instance.versionNotification != strings[0])
                {
                    for (int i = 1; i < strings.Length; i++)
                    {
                        strings[i] = strings[i].Trim();
                        if (strings[i] != "")
                        {
                            MessageBox.Show(strings[i], "Thông báo", MessageBoxButton.OK);
                        }
                    }
                    SaveSettings.Instance.versionNotification = strings[0];
                }
            }
        }

        private static bool ExistedWindow(Account account, out IntPtr hWnd)
        {
            hWnd = IntPtr.Zero;
            if (account.process == null || account.process.HasExited)
                return false;

            hWnd = account.process.MainWindowHandle;
            return hWnd != IntPtr.Zero;
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

        private static void SendMessageToAccounts(List<Account> accounts, object message)
        {
            var connectedAccounts = accounts.Where(account => account.workSocket?.Connected == true);
            foreach (var account in connectedAccounts)
                account.sendMessage(message);
        }

        private static void KillProcesses(List<Account> accounts)
        {
            var runningAccounts = accounts.Where(account => account.process?.HasExited == false);
            foreach (var account in runningAccounts)
                account.process.Kill();
        }

        private void ArrangeWindows(List<Account> accounts, int type)
        {
            var maxWidth = SystemParameters.PrimaryScreenWidth;
            var maxHeight = SystemParameters.PrimaryScreenHeight;

            int xBase = (int)this.ActualWidth;

            int cx = xBase, cy = 0;

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
                    cx = xBase;
                    cy += height - 5;
                }
                if (cy + height > maxHeight)
                {
                    cy = 0;
                }
            }
        }

        private bool UpdateSizeData()
        {
            var match = Regex.Match(this.TextBoxSize.Text, @"^\s*(\d+)x(\d+)\s*$");
            if (!match.Success)
                return false;
            width = int.Parse(match.Groups[1].Value);
            height = int.Parse(match.Groups[2].Value);
            sizeData = new
            {
                width,
                height,
                typeSize = this.ComboBoxTypeSize.SelectedIndex + 1,
                lowGraphic = this.ComboBoxLowGraphic.SelectedIndex
            };
            return true;
        }

        public List<Account> GetAllAccounts()
            => (List<Account>)this.AccountsDataGrid.ItemsSource;

        private Account GetSelectedAccount()
            => (Account)this.AccountsDataGrid.SelectedItem;

        private List<Account> GetSelectedAccounts()
            => this.AccountsDataGrid.SelectedItems.Cast<Account>().ToList();

        private Account GetInputAccount() => new Account()
        {
            username = this.UsernameTextBox.Text,
            password = this.PasswordPasswordBox.Password,
            indexServer = this.ServerComboBox.SelectedIndex,
        };

        #region Save
        private void DoSaveAccounts()
        {
            try
            {
                if (!Directory.Exists("ModData")) Directory.CreateDirectory("ModData");
                File.WriteAllText(Settings.Default.PathAccounts,
                    Utilities.EncryptString(LitJson.JsonMapper.ToJson(this.AccountsDataGrid.ItemsSource)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DoSaveSettings()
        {
            SaveSettings.Instance.size = this.TextBoxSize.Text;
            SaveSettings.Instance.lowGraphic = this.ComboBoxLowGraphic.SelectedIndex;
            SaveSettings.Instance.typeSize = this.ComboBoxTypeSize.SelectedIndex;
            SaveSettings.Instance.rowDetailsMode = this.RowDetailsModeComboBox.SelectedIndex;

            SaveSettings.Save();
        }
        #endregion

        #region Load
        private void LoadAccounts()
        {
            try
            {
                this.AccountsDataGrid.ItemsSource = LitJson.JsonMapper.ToObject<List<Account>>(
                    Utilities.DecryptString(File.ReadAllText(Settings.Default.PathAccounts)));
            }
            catch
            {
                this.AccountsDataGrid.ItemsSource = new List<Account>();
                this.DoSaveAccounts();
            }
        }

        private void LoadSaveSettings()
        {
            this.TextBoxSize.Text = SaveSettings.Instance.size;
            this.ComboBoxLowGraphic.SelectedIndex = SaveSettings.Instance.lowGraphic;
            this.ComboBoxTypeSize.SelectedIndex = SaveSettings.Instance.typeSize;
            this.RowDetailsModeComboBox.SelectedIndex = SaveSettings.Instance.rowDetailsMode;
        }
        #endregion

        #region Open Game
        private async Task LoginSelectedAccountsAsync()
        {
            if (!this.UpdateSizeData())
            {
                MessageBox.Show("Kích thước cửa sổ không hợp lệ");
                return;
            }

            var accounts = this.GetSelectedAccounts();
            if (accounts.Count() == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            this.MainGrid.IsEnabled = false;
            await this.OpenGamesAsync(accounts);
            this.MainGrid.IsEnabled = true;
        }

        private async Task LoginAllAccountsAsync()
        {
            if (!this.UpdateSizeData())
            {
                MessageBox.Show("Kích thước cửa sổ không hợp lệ");
                return;
            }

            var accounts = this.GetAllAccounts();

            this.MainGrid.IsEnabled = false;
            await this.OpenGamesAsync(accounts);
            this.MainGrid.IsEnabled = true;
        }

        private async Task OpenGamesAsync(List<Account> accounts)
        {
            foreach (var account in accounts)
            {
                await this.OpenGameAsync(account);
            }
        }

        private async Task OpenGameAsync(Account account)
        {
            if (account.process == null || account.process.HasExited)
            {
                account.status = "Đang khởi động...";
                this.AccountsDataGrid.Items.Refresh();

                AsynchronousSocketListener.waitingAccounts.Add(account);

                account.process = Process.Start(Settings.Default.PathGame,
                    $"-port {Settings.Default.PortListener} -screen-width {width} -screen-height {height}");

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
        private async Task ShowWindowAsync(IntPtr hWnd)
        {
            Utilities.ShowWindowAsync(hWnd, 1);
            Utilities.SetForegroundWindow(hWnd);
            await Task.Delay(100);

            Utilities.GetWindowRect(hWnd, out RECT rect);

            int xBase = (int)this.ActualWidth;

            double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
            double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;

            if (rect.left < xBase || rect.right > primaryScreenWidth ||
                rect.top < 0 || rect.bottom > primaryScreenHeight)
            {
                Utilities.MoveWindow(
                    hWnd: hWnd,
                    x: xBase, y: 0,
                    width: rect.right - rect.left,
                    height: rect.bottom - rect.top,
                    bRepaint: true);
            }
        }

        private async Task ShowAllWindowsAsync()
        {
            var accounts = this.GetAllAccounts();
            await ShowWindowsAsync(accounts);
        }

        private void ArrangeAllWindows(int type)
        {
            this.ArrangeWindows(this.GetAllAccounts(), type);
        }

        private async Task ShowAndArrangeWindows(int type)
        {
            var accounts = this.GetSelectedAccounts();
            if (accounts.Count <= 1)
                accounts = this.GetAllAccounts();

            await ShowWindowsAsync(accounts);
            this.ArrangeWindows(accounts, type);
        }

        private void KillSelectedProcesses()
        {
            var accounts = this.GetSelectedAccounts();
            if (accounts.Count() == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            KillProcesses(accounts);
        }

        private void KillAllProcesses()
        {
            foreach (var account in this.GetAllAccounts())
                if (account.process?.HasExited == false)
                    account.process.Kill();
        }
        #endregion

        #region Send
        private void SendChatToSelectedAccounts()
        {
            var accounts = this.GetSelectedAccounts();
            if (accounts.Count() == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            SendMessageToAccounts(accounts, new
            {
                action = "chat",
                text = this.TextBoxChat.Text
            });
        }

        private void SendKeyPressToSelectedAccounts(int keyCode)
        {
            SendMessageToAccounts(this.GetSelectedAccounts(), new
            {
                action = "keyPress",
                keyCode
            });
        }

        private void SendKeyReleaseToSelectedAccounts(int keyCode)
        {
            SendMessageToAccounts(this.GetSelectedAccounts(), new
            {
                action = "keyRelease",
                keyCode
            });
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

        private void KeyPressButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var button = (Button)sender;
            int keyCode = GetKeyCode(button);

            if (keyCode == 0)
                return;

            this.SendKeyPressToSelectedAccounts(keyCode);
        }

        private void KeyPressButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var button = (Button)sender;
            int keyCode = GetKeyCode(button);

            if (keyCode == 0)
                return;

            this.SendKeyReleaseToSelectedAccounts(keyCode);
        }

        private void Control_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int keyCode = GetKeyCode(e);
            e.Handled = true;

            if (keyCode == 0)
                return;

            this.SendKeyPressToSelectedAccounts(keyCode);
        }

        private void Control_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            int keyCode = GetKeyCode(e);
            e.Handled = true;

            if (keyCode == 0)
                return;

            this.SendKeyReleaseToSelectedAccounts(keyCode);
        }
        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            this.KillAllProcesses();

            this.DoSaveAccounts();
            this.DoSaveSettings();
        }

        private void AccountsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            ((Account)e.Row.Item).number = e.Row.GetIndex();
        }

        private void AccountsDataGird_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AccountsDataGrid.SelectedItem is Account account)
            {
                this.UsernameTextBox.Text = account.username;
                this.PasswordPasswordBox.Password = account.password;
                this.ServerComboBox.SelectedIndex = account.indexServer;
                isCheckedStateChangedByCode = true;
                if (account.Equals(SaveSettings.accountConnectToDiscordRPC))
                    IsDisplayInDiscordRichPresence.IsChecked = true;
                else
                    IsDisplayInDiscordRichPresence.IsChecked = false;
            }
        }

        private async void AccountsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.AccountsDataGrid.SelectedItem is Account account)
            {
                isCheckedStateChangedByCode = false;
                this.MainGrid.IsEnabled = false;
                
                if (ExistedWindow(account, out IntPtr hWnd))
                {
                    await this.ShowWindowAsync(hWnd);
                    this.MainGrid.IsEnabled = true;
                    return;
                }

                if (!this.UpdateSizeData())
                {
                    MessageBox.Show("Kích thước cửa sổ không hợp lệ");
                    this.MainGrid.IsEnabled = true;
                    return;
                }

                if (account.Equals(SaveSettings.accountConnectToDiscordRPC))
                    IsDisplayInDiscordRichPresence_Checked(sender, null);
                await this.OpenGameAsync(account);
                
                this.MainGrid.IsEnabled = true;
            }
        }

        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            this.AccountsDataGrid.SelectedItems.Clear();
            this.GetAllAccounts().ForEach(
                a => this.AccountsDataGrid.SelectedItems.Add(a));
        }

        private void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            this.GetAllAccounts().Add(this.GetInputAccount());
            this.AccountsDataGrid.Items.Refresh();
            this.DoSaveAccounts();
        }

        private void EditAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var account = this.GetSelectedAccount();
            if (account == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            var inputAccount = this.GetInputAccount();
            account.username = inputAccount.username;
            account.password = inputAccount.password;
            account.indexServer = inputAccount.indexServer;

            this.DoSaveAccounts();
            this.AccountsDataGrid.Items.Refresh();
        }

        private void DeleteAccountsButton_Click(object sender, RoutedEventArgs e)
        {
            var accounts = this.GetSelectedAccounts();
            foreach (var account in accounts)
            {
                if (account.Equals(SaveSettings.accountConnectToDiscordRPC))
                {
                    SaveSettings.accountConnectToDiscordRPC = null;
                    SaveSettings.Instance.indexConnectToDiscordRPC = -1;
                }
                this.GetAllAccounts().Remove(account);
            }

            this.DoSaveAccounts();
            this.AccountsDataGrid.Items.Refresh();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await this.LoginSelectedAccountsAsync();
        }

        private async void LoginAllButton_Click(object sender, RoutedEventArgs e)
        {
            await this.LoginAllAccountsAsync();
        }

        private void KillButton_Click(object sender, RoutedEventArgs e)
        {
            this.KillSelectedProcesses();
        }

        private async void ArrangeWindows1Button_Click(object sender, RoutedEventArgs e)
        {
            this.MainGrid.IsEnabled = false;
            await this.ShowAndArrangeWindows(1);
            this.MainGrid.IsEnabled = true;
        }

        private async void ArrangeWindows2Button_Click(object sender, RoutedEventArgs e)
        {
            this.MainGrid.IsEnabled = false;
            await this.ShowAndArrangeWindows(1);
            this.MainGrid.IsEnabled = true;
        }

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            this.SendChatToSelectedAccounts();
        }

        private void ChatTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.ChatButton_Click(sender, null);
                e.Handled = true;
            }
        }

        private void IsDisplayInDiscordRichPresence_Checked(object sender, RoutedEventArgs e)
        {
            if (isCheckedStateChangedByCode)
            {
                isCheckedStateChangedByCode = false;
                return;
            }
            SaveSettings.Instance.indexConnectToDiscordRPC = AccountsDataGrid.SelectedIndex;
            SaveSettings.accountConnectToDiscordRPC = AccountsDataGrid.SelectedItem as Account;
            Program.discordClient.SetPresence(new DiscordRPC.RichPresence()
            {
                State = "Chưa đăng nhập",
                Assets = new DiscordRPC.Assets()
                {
                    LargeImageKey = "icon_large",
                    LargeImageText = "Mod Cộng Đồng",
                }
            });
        }

        private void IsDisplayInDiscordRichPresence_Unchecked(object sender, RoutedEventArgs e)
        {
            if (isCheckedStateChangedByCode)
            {
                isCheckedStateChangedByCode = false;
                return;
            }
            SaveSettings.Instance.indexConnectToDiscordRPC = -1;
            SaveSettings.accountConnectToDiscordRPC = null;
            Program.discordClient.SetPresence(new DiscordRPC.RichPresence()
            {
                State = "Chưa đăng nhập",
                Assets = new DiscordRPC.Assets()
                {
                    LargeImageKey = "icon_large",
                    LargeImageText = "Mod Cộng Đồng",
                }
            });
        }

        private void AccountsDataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var args = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent
            };

            AccountsScrollViewer.RaiseEvent(args);
        }
    }
}
