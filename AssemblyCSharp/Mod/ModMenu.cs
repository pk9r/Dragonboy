﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod
{
    public class ModMenu
    {
        public const int TYPE_MOD_MENU = 26;

        /// <summary>
        /// Thêm bật/tắt chức năng mod ở đây
        /// </summary>
        public static ModMenuItemBoolean[] modMenuItemBools = new ModMenuItemBoolean[]
        {
        new ModMenuItemBoolean("Vsync", "Tắt Vsync nếu bạn muốn điều chỉnh FPS!", true, "isvsync"),
        new ModMenuItemBoolean("Hiện thông tin nhân vật", "Hiện gần chính xác thời gian NRD, khiên, khỉ, huýt sáo... của nhân vật đang focus", true, "isshowinfochar"),
        new ModMenuItemBoolean("Tự đánh", "Bật/tắt tự đánh", ""),
        new ModMenuItemBoolean("Hiện danh sách nhân vật", "Hiện danh sách nhân vật trong map", "isshowlistchar"),
        new ModMenuItemBoolean("Hiện đệ tử trong danh sách", "Hiện đệ tử trong danh sách nhân vật trong map (đệ tử không có sư phụ trong map không được hiển thị)", "isshowlistpet", true),
        new ModMenuItemBoolean("Auto up SS", "Auto up acc sơ sinh đến nhiệm vụ vào bang", "", true),
        new ModMenuItemBoolean("Auto T77", "Auto up Tàu Pảy Pảy", "", true),
        };

        /// <summary>
        /// Thêm điều chỉnh chỉ số chức năng mod ở đây
        /// </summary>
        public static ModMenuItemInt[] modMenuItemInts = new ModMenuItemInt[]
        {
        new ModMenuItemInt("FPS", "FPS mục tiêu (cần tắt Vsync để thay đổi có hiệu lực)", 60, "targetfps"),
        new ModMenuItemInt("Giảm đồ họa", new string[]{"Đang tắt", "Đang bật mức 1", "Đang bật mức 2", "Đang bật mức 3"}, 0, "levelreducegraphics"),
        new ModMenuItemInt("Goback", new string[]{"Đang tắt", "Đang bật (goback tới chỗ cũ khi chết)", "Đang bật (goback tới map cố định)" }, 0, ""),
        new ModMenuItemInt("Gõ tiếng Việt", new string[]{"Đang tắt", "Đang bật kiểu gõ TELEX", "Đang bật kiểu gõ VIQR", "Đang bật kiểu gõ VNI"}, "vietmode"),
        new ModMenuItemInt("Auto up đệ tử", new string[]{"Đang tắt", "Đang bật up đệ thường", "Đang bật up đệ né siêu quái", "Đang bật up đệ kaioken"}, ""),
        new ModMenuItemInt("Đánh khi đệ cần", new string[]{"Đánh quái gần nhất", "Đánh đệ (yêu cầu bật cờ xám)", "Đánh bản thân (yêu cầu bật cờ xám)"}, "modeautopet", true),
        };

        public static ModMenuItemFunction[] modMenuItemFunctions = new ModMenuItemFunction[]
        {
        new ModMenuItemFunction("Xmap", "Mở menu Xmap (chat \"xmp\" hoặc bấm nút x)"),
        new ModMenuItemFunction("Menu teleport", "Mở menu dịch chuyển (chat \"tele\" hoặc bấm nút z)"),
            
        };

        public static string[][] inputModMenuItemInts = new string[][]
        {
        new string[]{"Nhập mức FPS", "FPS"},
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