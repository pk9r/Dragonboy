using Mod.Graphics;
using Mod.PickMob;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using UnityEngine;
using Vietpad.InputMethod;

namespace Mod.ModMenu
{
    public class ModMenuMain
    {
        /// <summary>
        /// Thêm bật/tắt chức năng mod ở đây
        /// </summary>
        public static ModMenuItemBoolean[] modMenuItemBools = new ModMenuItemBoolean[]
        {
            new ModMenuItemBoolean("Vsync", "Tắt Vsync nếu bạn muốn điều chỉnh FPS!", (bool value) => QualitySettings.vSyncCount = value ? 1 : 0, true, "isvsync"),
            new ModMenuItemBoolean("Hiện thông tin nhân vật", "Hiện gần chính xác thời gian NRD, khiên, khỉ, huýt sáo... của nhân vật đang focus", CharEffect.setState, true, "isshowinfochar"),
            new ModMenuItemBoolean("Tự đánh", "Bật/tắt tự đánh", AutoAttack.toggle),
            new ModMenuItemBoolean("Hiện danh sách nhân vật", "Hiện danh sách nhân vật trong map", ListCharsInMap.setState, false, "isshowlistchar"),
            new ModMenuItemBoolean("Hiện đệ tử trong danh sách", "Hiện đệ tử trong danh sách nhân vật trong map (đệ tử không có sư phụ trong map không được hiển thị)", ListCharsInMap.setStatePet, false, "isshowlistpet", true, "Bạn chưa bật chức năng \"Hiện danh sách nhân vật\"!"),
            new ModMenuItemBoolean("Auto up SS", "Auto up acc sơ sinh đến nhiệm vụ vào bang", AutoSS.setState, false, "", true, "Bạn đã qua nhiệm vụ sơ sinh!"),
            new ModMenuItemBoolean("Auto T77", "Auto up Tàu Pảy Pảy", AutoT77.setState, false, "", true, "Bạn không thể vào map Đông Karin!"),
            new ModMenuItemBoolean("Hiện khoảng cách bom", "Hiển thị người, quái, boss... trong tầm bom", SuicideRange.setState, false, "isshowsuiciderange"),
            new ModMenuItemBoolean("Nền tùy chỉnh", "Thay thế nền của game bằng ảnh trong danh sách (ảnh sẽ được tự động điều chỉnh cho vừa kích thước màn hình)", CustomBackground.setState, false, "iscustombackground", false, "Bạn cần tắt chức năng \"Giảm đồ họa\"!"),
            new ModMenuItemBoolean("Logo tùy chỉnh", "Bật/tắt hiển thị logo tùy chỉnh trên màn hình game", CustomLogo.setState, false, "isshowlogo"),

            new ModMenuItemBoolean("Tàn sát", "Bật/tắt tự động đánh quái", (bool value) => Pk9rPickMob.IsTanSat = value, false, "", false, "Bạn đang bật auto T77 hoặc auto up SS!"),
            new ModMenuItemBoolean("Né siêu quái khi tàn sát", "Tự động né siêu quái khi tàn sát", (bool value) => Pk9rPickMob.IsNeSieuQuai = value, true, "isnesieuquaits"),
            new ModMenuItemBoolean("Vượt địa hình khi tàn sát", "Bật/tắt tự động vượt địa hình khi đang tàn sát", (bool value) => Pk9rPickMob.IsVuotDiaHinh = value, true, "isvuotdiahinh"),
            new ModMenuItemBoolean("Tự động nhặt vật phẩm", "Bật/tắt tự động nhặt vật phẩm", (bool value) => Pk9rPickMob.IsAutoPickItems = value, true, "isautopick", false, "Bạn đang bật auto T77 hoặc auto up SS!"),
            new ModMenuItemBoolean("Không nhặt đồ của người khác", "Bật/tắt lọc không nhặt vật phẩm của người khác", (bool value) => Pk9rPickMob.IsItemMe = value, true, "ispickmyitemonly"),
            new ModMenuItemBoolean("Giới hạn số lần nhặt", "Bật/tắt giới hạn số lần tự động nhặt một vật phẩm", (bool value) => Pk9rPickMob.IsLimitTimesPickItem = value, true,"islimitpicktimes"),
        };

        /// <summary>
        /// Thêm điều chỉnh chỉ số chức năng mod ở đây
        /// </summary>
        public static ModMenuItemInt[] modMenuItemInts = new ModMenuItemInt[]
        {
            new ModMenuItemInt("FPS", null, "FPS mục tiêu (cần tắt Vsync để thay đổi có hiệu lực)", 60, delegate(int value)
            {
                if (value > 5 && value <= 60) Application.targetFrameRate = value;
                else throw new ArgumentException();
            }, "targetfps", false, "Bạn chưa tắt Vsync!"),
            new ModMenuItemInt("Giảm đồ họa", new string[]{"Đang tắt", "Đang bật mức 1", "Đang bật mức 2", "Đang bật mức 3"}, "", 0, null, "levelreducegraphics"),
            new ModMenuItemInt("Goback", new string[]{"Đang tắt", "Đang bật (goback tới chỗ cũ khi chết)", "Đang bật (goback tới map cố định)" }, "", 0, AutoGoback.setState),
            new ModMenuItemInt("Gõ tiếng Việt", new string[]{"Đang tắt", "Đang bật kiểu gõ TELEX", "Đang bật kiểu gõ VIQR", "Đang bật kiểu gõ VNI"}, "", 0, delegate(int value)
            {
            if (value == 0) VietKeyHandler.VietModeEnabled = false;
            else
            {
                VietKeyHandler.VietModeEnabled = true;
                VietKeyHandler.InputMethod = (InputMethods)(value - 1);
            }
            }, "vietmode", false, "Bạn không biết gõ tiếng Việt!"),
            new ModMenuItemInt("Auto up đệ tử", new string[]{"Đang tắt", "Đang bật up đệ thường", "Đang bật up đệ né siêu quái", "Đang bật up đệ kaioken"}, "", 0, AutoPet.setState, "", false, "Bạn không có đệ tử!"),
            new ModMenuItemInt("Đánh khi đệ cần", new string[]{"Đánh quái gần nhất", "Đánh đệ (tự động bật cờ xám)", "Đánh bản thân (tự động bật cờ xám)"}, "", 0, AutoPet.setAttackState, "modeautopet", true, "Bạn chưa bật chức năng \"Auto up đệ tử\"!"),
            new ModMenuItemInt("Thời gian đổi ảnh nền", null, "Điều chỉnh thời gian thay đổi ảnh nền (giây)", 30, CustomBackground.setState, "backgroundinveral", false),
            new ModMenuItemInt("Thời gian đổi logo", null, "Điều chỉnh thời gian thay đổi logo (giây)", 30, CustomLogo.setState, "logoinveral", false),
            new ModMenuItemInt("Chiều cao của logo", null, "Điều chỉnh chiều cao của logo", 80, CustomLogo.setLogoHeight, "logoheight"),
        };

        public static ModMenuItemFunction[] modMenuItemFunctions = new ModMenuItemFunction[]
        {
            new ModMenuItemFunction("Menu Xmap", "Mở menu Xmap (chat \"xmp\" hoặc bấm nút x)", XmapController.ShowXmapMenu),
            new ModMenuItemFunction("Menu PickMob", "Mở menu PickMob (chat \"pickmob\")", Pk9rPickMob.ShowMenu),
            new ModMenuItemFunction("Menu Teleport", "Mở menu dịch chuyển (chat \"tele\" hoặc bấm nút z)", TeleportMenu.TeleportMenu.ShowMenu),
            new ModMenuItemFunction("Menu Custom Background", "Mở menu ảnh nền tùy chỉnh", CustomBackground.ShowMenu),
            new ModMenuItemFunction("Menu Custom Logo", "Mở menu logo tùy chỉnh", CustomLogo.ShowMenu),
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