using LitJson;
using QLTK.Models;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace QLTK
{
    public class Utilities
    {
        public static void UpdateStatus(Account account, string status)
        {
            account.status = status;
            RefreshAccounts();
        }

        public static async void RefreshAccounts()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                GetMainWindow().RefreshAccounts();
            });
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
    }
}
