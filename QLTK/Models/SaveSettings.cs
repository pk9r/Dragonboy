using QLTK.Properties;
using System;
using System.IO;
using System.Windows;

namespace QLTK.Models
{
    public class SaveSettings
    {
        [LitJSON.JsonSkip]
        public static SaveSettings Instance { get; } = LoadSaveSettings();

        public string versionNotification;

        public string size = "1024x600";
        public int lowGraphic = 1;
        public int typeSize = 2;
        public int rowDetailsMode = 0;

        public int indexConnectToDiscordRPC = -1;

        [LitJSON.JsonSkip]
        public static Account accountConnectToDiscordRPC;
        private static SaveSettings LoadSaveSettings()
        {
            try
            {
                return LitJson.JsonMapper.ToObject<SaveSettings>(
                    File.ReadAllText(Settings.Default.PathSettings));
            }
            catch (Exception e)
            {
                var r = MessageBox.Show(
                    "Không tìm thấy dữ liệu cài đặt, bạn có muốn tạo dữ liệu mới?\n" + e.ToString(),
                    "Lỗi tải dữ liệu", MessageBoxButton.YesNo);

                if (r == MessageBoxResult.Yes)
                {
                    return new SaveSettings();
                }

                Application.Current.Shutdown();
                return null;
            }
        }

        public static void Save()
        {
            try
            {
                File.WriteAllText(Settings.Default.PathSettings,
                    LitJson.JsonMapper.ToJson(Instance));
            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi lưu dữ liệu\n" + e.ToString());
            }
        }
    }
}
