using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.ModMenu
{
    public class ModMenuMain
    {
        public const int TYPE_MOD_MENU = 26;

        /// <summary>
        /// Thêm bật/tắt chức năng mod ở đây
        /// </summary>
        public static ModMenuItemBoolean[] modMenuItemBools = new ModMenuItemBoolean[]
        {
            new ModMenuItemBoolean("Vsync", "Tắt Vsync nếu bạn muốn điều chỉnh FPS!", true, "isvsync"),
            new ModMenuItemBoolean("Hiện thông tin nhân vật", "Hiện gần chính xác thời gian NRD, khiên, khỉ, huýt sáo... của nhân vật đang focus", true, "isshowinfochar"),
            new ModMenuItemBoolean("Tự đánh", "Bật/tắt tự đánh"),
            new ModMenuItemBoolean("Hiện danh sách nhân vật", "Hiện danh sách nhân vật trong map", false, "isshowlistchar"),
            new ModMenuItemBoolean("Hiện đệ tử trong danh sách", "Hiện đệ tử trong danh sách nhân vật trong map (đệ tử không có sư phụ trong map không được hiển thị)", false, "isshowlistpet", true, "Bạn chưa bật chức năng \"Hiện danh sách nhân vật\"!"),
            new ModMenuItemBoolean("Auto up SS", "Auto up acc sơ sinh đến nhiệm vụ vào bang", false, "", true, "Bạn đã qua nhiệm vụ sơ sinh!"),
            new ModMenuItemBoolean("Auto T77", "Auto up Tàu Pảy Pảy", false, "", true, "Bạn không thể vào map Đông Karin!"),
            new ModMenuItemBoolean("Hiện khoảng cách bom", "Hiển thị người, quái, boss... trong tầm bom", false, "isshowsuiciderange"),
            new ModMenuItemBoolean("Ảnh nền tùy chỉnh", "Thay thế nền của game bằng ảnh tùy chỉnh (ảnh sẽ được tự động điều chỉnh cho vừa kích thước màn hình)", false, "iscustombackground", false, "Bạn cần tắt chức năng \"Giảm đồ họa\"!"),
            new ModMenuItemBoolean("Logo tùy chỉnh", "Bật/tắt hiển thị logo tùy chỉnh trên màn hình game", false, "isshowlogo"),

            new ModMenuItemBoolean("Tàn sát", "Bật/tắt tự động đánh quái", false, "", false, "Bạn đang bật auto T77 hoặc auto up SS!"),
            new ModMenuItemBoolean("Né siêu quái khi tàn sát", "Tự động né siêu quái khi tàn sát", true, "isnesieuquaits"),
            new ModMenuItemBoolean("Vượt địa hình khi tàn sát", "Bật/tắt tự động vượt địa hình khi đang tàn sát", true, "isvuotdiahinh"),
            new ModMenuItemBoolean("Tự động nhặt vật phẩm", "Bật/tắt tự động nhặt vật phẩm", true, "isautopick", false, "Bạn đang bật auto T77 hoặc auto up SS!"),
            new ModMenuItemBoolean("Không nhặt đồ của người khác", "Bật/tắt lọc không nhặt vật phẩm của người khác", true, "ispickmyitemonly"),
            new ModMenuItemBoolean("Giới hạn số lần nhặt", "Bật/tắt giới hạn số lần tự động nhặt một vật phẩm", true,"islimitpicktimes"),
        };

        /// <summary>
        /// Thêm điều chỉnh chỉ số chức năng mod ở đây
        /// </summary>
        public static ModMenuItemInt[] modMenuItemInts = new ModMenuItemInt[]
        {
            new ModMenuItemInt("FPS", null, "FPS mục tiêu (cần tắt Vsync để thay đổi có hiệu lực)", 60, "targetfps", false, "Bạn chưa tắt Vsync!"),
            new ModMenuItemInt("Giảm đồ họa", new string[]{"Đang tắt", "Đang bật mức 1", "Đang bật mức 2", "Đang bật mức 3"},"", 0, "levelreducegraphics"),
            new ModMenuItemInt("Goback", new string[]{"Đang tắt", "Đang bật (goback tới chỗ cũ khi chết)", "Đang bật (goback tới map cố định)" }),
            new ModMenuItemInt("Gõ tiếng Việt", new string[]{"Đang tắt", "Đang bật kiểu gõ TELEX", "Đang bật kiểu gõ VIQR", "Đang bật kiểu gõ VNI"}, "",0, "vietmode", false, "Bạn không biết gõ tiếng Việt!"),
            new ModMenuItemInt("Auto up đệ tử", new string[]{"Đang tắt", "Đang bật up đệ thường", "Đang bật up đệ né siêu quái", "Đang bật up đệ kaioken"}, "", 0, "", false, "Bạn không có đệ tử!"),
            new ModMenuItemInt("Đánh khi đệ cần", new string[]{"Đánh quái gần nhất", "Đánh đệ (tự động bật cờ xám)", "Đánh bản thân (tự động bật cờ xám)"}, "", 0, "modeautopet", true, "Bạn chưa bật chức năng \"Auto up đệ tử\"!"),
            new ModMenuItemInt("Thời gian đổi ảnh nền", null, "Điều chỉnh thời gian thay đổi ảnh nền (giây)", 30, "backgroundinveral", false),
            new ModMenuItemInt("Thời gian đổi logo", null, "Điều chỉnh thời gian thay đổi logo (giây)", 30, "logoinveral", false),
            new ModMenuItemInt("Chiều cao của logo", null, "Điều chỉnh chiều cao của logo", 80, "logoheight"),
        };

        public static ModMenuItemFunction[] modMenuItemFunctions = new ModMenuItemFunction[]
        {
            new ModMenuItemFunction("Menu Xmap", "Mở menu Xmap (chat \"xmp\" hoặc bấm nút x)"),
            new ModMenuItemFunction("Menu PickMob", "Mở menu PickMob (chat \"pickmob\")"),
            new ModMenuItemFunction("Menu Teleport", "Mở menu dịch chuyển (chat \"tele\" hoặc bấm nút z)"),
            new ModMenuItemFunction("Menu Custom Background", "Mở menu ảnh nền tùy chỉnh"),
            new ModMenuItemFunction("Menu Custom Logo", "Mở menu logo tùy chỉnh"),
        };

        public static Dictionary<int, string[]> inputModMenuItemInts = new Dictionary<int, string[]>()
        {
            { 0, new string[]{"Nhập mức FPS", "FPS"} },
            { 6, new string[]{"Nhập thời gian thay đổi ảnh nền", "thời gian (giây)"} },
            { 7, new string[]{"Nhập thời gian thay đổi logo", "thời gian (giây)"} },
            { 8, new string[]{"Nhập chiều cao logo", "Chiều cao logo" } },
        };

        public static void SaveData()
        {
            foreach (ModMenuItemBoolean modMenuItem in modMenuItemBools) if (!string.IsNullOrEmpty(modMenuItem.RMSName)) Utilities.saveRMSBool(modMenuItem.RMSName, modMenuItem.Value);
            foreach (ModMenuItemInt modMenuItem in modMenuItemInts) if (!string.IsNullOrEmpty(modMenuItem.RMSName)) Utilities.saveRMSInt(modMenuItem.RMSName, modMenuItem.SelectedValue);
        }

        public static void LoadData()
        {
            foreach (ModMenuItemBoolean modMenuItem in modMenuItemBools)
            {
                try
                {
                    if (!string.IsNullOrEmpty(modMenuItem.RMSName)) modMenuItem.setValue(Utilities.loadRMSBool(modMenuItem.RMSName));
                }
                catch { }
            }
            foreach (ModMenuItemInt modMenuItem in modMenuItemInts)
            {
                try
                {
                    int data = Utilities.loadRMSInt(modMenuItem.RMSName);
                    modMenuItem.setValue(data == -1 ? 0 : data);
                }
                catch { }
            }
        }

        public static bool getStatusBool(string rmsName)
        {
            foreach (ModMenuItemBoolean modMenuItem in modMenuItemBools)
            {
                if (modMenuItem.RMSName == rmsName) return modMenuItem.Value;
            }
            throw new Exception("Not found any ModMenuItemBoolean with RMSName \"" + rmsName + "\"!");
        }

        public static bool getStatusBool(int index)
        {
            return modMenuItemBools[index].Value;
        }

        public static int getStatusInt(string rmsName)
        {
            foreach (ModMenuItemInt modMenuItem in modMenuItemInts)
            {
                if (modMenuItem.RMSName == rmsName) return modMenuItem.SelectedValue;
            }
            throw new Exception("Not found any ModMenuItemOther with RMSName \"" + rmsName + "\"!");
        }

        public static int getStatusInt(int index)
        {
            return modMenuItemInts[index].SelectedValue;
        }
    }
}