using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Mod.Dialogs
{
    public class FileDialog
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class OpenFileName
        {
            public int lStructSize = 0;
            //The length, in bytes, of the structure. Use sizeof (OPENFILENAME) for this parameter.
            public IntPtr hwndOwner = IntPtr.Zero;
            //A handle to the window that owns the dialog box. This member can be any valid window handle, or it can be NULL if the dialog box has no owner.
            public IntPtr hInstance = IntPtr.Zero;
            public IntPtr lpstrFilter = IntPtr.Zero;
            //A buffer containing pairs of null-terminated filter strings. The last string in the buffer must be terminated by two NULL characters.
            public string lpstrCustomFilter = null;
            public int nMaxCustFilter = 0;
            public int nFilterIndex = 0;
            public IntPtr lpstrFile = IntPtr.Zero;
            public int nMaxFile = 0;
            public string lpstrFileTitle = null;
            public int nMaxFileTitle = 0;
            public string lpstrInitialDir = null;
            public string lpstrTitle = null;
            public int Flags = 0;
            public short nFileOffset = 0;
            public short nFileExtension = 0;
            public string lpstrDefExt = null;
            public IntPtr lCustData = IntPtr.Zero;
            public IntPtr lpfnHook = IntPtr.Zero;
            public string lpTemplateName = null;
            public IntPtr reservedPtr = IntPtr.Zero;
            public int reservedInt = 0;
            public int FlagsEx = 0;
        }

        public OpenFileName openFileNameInstance = new OpenFileName();

        FileDialog(string title, string filter, string extension)
        {
            openFileNameInstance.hwndOwner = GetForegroundWindow();
            openFileNameInstance.lStructSize = Marshal.SizeOf(openFileNameInstance);
            openFileNameInstance.lpstrFile = Marshal.AllocHGlobal(1024 * sizeof(short));
            Marshal.Copy(new short[1024], 0, openFileNameInstance.lpstrFile, 1024);
            openFileNameInstance.nMaxFile = 256;
            openFileNameInstance.lpstrFileTitle = new string(new char[64]);
            openFileNameInstance.nMaxFileTitle = openFileNameInstance.lpstrFileTitle.Length;
            openFileNameInstance.lpstrInitialDir = Application.dataPath;
            openFileNameInstance.lpstrTitle = title;
            openFileNameInstance.lpstrFilter = Marshal.AllocHGlobal((filter.Length + 2) * sizeof(short));
            StringBuilder stringBuilder = new StringBuilder(filter);
            char[] chars = new char[filter.Length + 2];
            stringBuilder.Replace('|', '\0').CopyTo(0, chars, 0, filter.Length);
            Marshal.Copy(chars, 0, openFileNameInstance.lpstrFilter, chars.Length);
            openFileNameInstance.lpstrDefExt = extension;
            openFileNameInstance.Flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
        }

        FileDialog(string title, string filter, string extension, string initialPath) : this(title, filter, extension)
        {
            openFileNameInstance.lpstrInitialDir = initialPath;
        }

        FileDialog(string title, string filter, string extension, int flags) : this(title, filter, extension)
        {
            openFileNameInstance.Flags = flags;
        }

        FileDialog(string title, string filter, string extension, string initialPath, int flags) : this(title, filter, extension, initialPath)
        {
            openFileNameInstance.Flags = flags;
        }

        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Mở hộp thoại chọn tệp. Được phép chọn nhiều tệp cùng một lúc.
        /// </summary>
        /// <param name="title">Tiêu đề cửa sổ của hộp thoại chọn tệp.</param>
        /// <param name="filter">Bộ lọc tệp, phân cách nhau bởi dấu chấm phẩy (;). Nội dung và phần mở rộng phân cách nhau bởi dấu gạch đứng (|).</param>
        /// <param name="missingExtension">Phần mở rộng tệp (không bao gồm dấu chấm) được tự động thêm nếu người dùng nhập thiếu. Có thể là một xâu rỗng nếu không cần sử dụng.</param>
        /// <returns>Mảng xâu chứa đường dẫn tới các tệp đã được chọn.</returns>
        public static string[] OpenFileDialog(string title, string filter, string missingExtension)
        {
            FileDialog openFileDialog = new FileDialog(title, filter, missingExtension);
            //filter = "Picture files (*.png)|*.png";
            //title = "Upload Image";
            //defExt = "PNG";
            string str = string.Empty;
            if (GetOpenFileName(openFileDialog.openFileNameInstance))
            {
                char[] chars = new char[1024];
                Marshal.Copy(openFileDialog.openFileNameInstance.lpstrFile, chars, 0, 1024);
                str = new StringBuilder().Append(chars).Replace('\0', '|').ToString();
                str = str.Substring(0, str.IndexOf("||"));
            }
            else return null;
            Marshal.FreeHGlobal(openFileDialog.openFileNameInstance.lpstrFile);
            Marshal.FreeHGlobal(openFileDialog.openFileNameInstance.lpstrFilter);
            openFileDialog.openFileNameInstance.lpstrFile = IntPtr.Zero;
            openFileDialog.openFileNameInstance.lpstrFilter = IntPtr.Zero;
            int length = str.Split('|').Length;
            string currentFolderOrSelectedFile = str.Split('|')[0];
            string[] result = new string[1] {currentFolderOrSelectedFile};
            if (length > 1)
            {
                result = new string[str.Split('|').Length - 1];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = currentFolderOrSelectedFile + "\\" + str.Split('|')[i + 1];
                }
            }
            return result;
        }
    }
}