using Microsoft.Win32;
using QLTK.Models;
using QLTK.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QLTK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region winAPI
        [DllImport("user32.dll")]
        static extern int SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool bRepaint);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            ComboBoxServer.SelectedIndex = 0;

            LoadAccounts();
        }

        private void LoadAccounts()
        {
            try
            {
                this.ListViewAccount.ItemsSource = LitJson.JsonMapper.ToObject<List<Account>>(
                    File.ReadAllText(Settings.Default.PathAccounts));
            }
            catch
            {
                this.ListViewAccount.ItemsSource = new List<Account>();
                SaveAccounts();
            }
        }

        private List<Account> GetAccounts()
            => (List<Account>)this.ListViewAccount.ItemsSource;

        private IEnumerable<Account> GetSelectedAccounts()
            => this.ListViewAccount.SelectedItems.Cast<Account>();

        private Account GetSelectedAccount()
            => (Account)this.ListViewAccount.SelectedItem;

        private Account GetInputAccount() => new Account()
        {
            username = this.TextBoxUsername.Text,
            password = this.PasswordBoxPassword.Password,
            indexServer = this.ComboBoxServer.SelectedIndex,
        };

        private void SaveAccounts()
        {
            File.WriteAllText(Settings.Default.PathAccounts,
                LitJson.JsonMapper.ToJson(this.ListViewAccount.ItemsSource));
        }

        private void OpenGame(Account account)
        {
            account.process = Process.Start(
                fileName: Settings.Default.PathGame,
                arguments: Utilities.Base64StringEncode(
                    LitJson.JsonMapper.ToJson(new
                    {
                        account,
                    })));

            SpinWait.SpinUntil(()
                => account.process.MainWindowHandle != IntPtr.Zero);

            SetWindowText(account.process.MainWindowHandle, account.username);
            GetWindowRect(account.process.MainWindowHandle, out RECT rect);

            MoveWindow(
                hWnd: account.process.MainWindowHandle,
                x: (int)SystemParameters.PrimaryScreenWidth, y: 0,
                width: rect.right - rect.left,
                height: rect.bottom - rect.top,
                bRepaint: true);

            account.status = "Đang chạy";
        }

        private void ShowWindows()
        {
            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                if (ExistedWindow(account, out IntPtr hWnd))
                {
                    ShowWindowAsync(hWnd, 1);
                    SetForegroundWindow(hWnd);
                }
            }
            Thread.Sleep(50);
        }

        private void ArrangeWindows()
        {
            var accounts = this.GetAccounts();

            var maxWidth = SystemParameters.PrimaryScreenWidth;
            var maxHeight = SystemParameters.PrimaryScreenHeight;

            int cx = 0, cy = 0;

            for (int i = 0; i < accounts.Count; i++)
            {
                if (!ExistedWindow(accounts[i], out IntPtr hWnd))
                    continue;

                if (!GetWindowRect(hWnd, out RECT rect))
                    continue;

                int width = rect.right - rect.left;
                int height = rect.bottom - rect.top;

                MoveWindow(hWnd, cx, cy, width, height, true);

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
        }

        private void ListViewAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewAccount.SelectedItem is Account account)
            {
                TextBoxUsername.Text = account.username;
                PasswordBoxPassword.Password = account.password;
                ComboBoxServer.SelectedIndex = account.indexServer;
            }
        }

        private void ListViewAccount_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewAccount.SelectedItem is Account account)
            {
                if (ExistedWindow(account, out IntPtr hWnd))
                {
                    // Hiển thị cửa sổ
                    ShowWindowAsync(hWnd, 1);
                    SetForegroundWindow(hWnd);
                    Thread.Sleep(50);

                    GetWindowRect(hWnd, out RECT rect);

                    int width = rect.right - rect.left;
                    int height = rect.bottom - rect.top;
                    MoveWindow(hWnd,
                        x: (int)(SystemParameters.PrimaryScreenWidth / 2) - width / 2,
                        y: (int)(SystemParameters.PrimaryScreenHeight / 2) - height / 2,
                        width, height, true);
                }
            }
        }

        private void ButtonAddAccount_Click(object sender, RoutedEventArgs e)
        {
            GetAccounts().Add(GetInputAccount());
            ListViewAccount.Items.Refresh();
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

            ListViewAccount.Items.Refresh();
            SaveAccounts();
        }

        private void ButtonDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            var accounts = GetSelectedAccounts();
            foreach (var account in accounts)
                GetAccounts().Remove(account);

            SaveAccounts();
            ListViewAccount.Items.Refresh();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            var accounts = GetSelectedAccounts();
            if (accounts.Count() == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản");
                return;
            }

            foreach (var account in accounts)
            {
                if (account.process == null || account.process.HasExited)
                    OpenGame(account);
            }
            ListViewAccount.Items.Refresh();
        }

        private void ButtonLoginAll_Click(object sender, RoutedEventArgs e)
        {
            var accounts = GetAccounts();
            foreach (var account in accounts)
            {
                if (account.process == null || account.process.HasExited)
                    OpenGame(account);
            }
            ListViewAccount.Items.Refresh();
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

        private void ButtonArrangeWindows_Click(object sender, RoutedEventArgs e)
        {
            ShowWindows();
            ArrangeWindows();
        }
    }
}
