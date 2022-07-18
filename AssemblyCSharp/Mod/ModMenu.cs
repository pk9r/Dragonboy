using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod;
public class ModMenu
{
    public const int TYPE_MOD_MENU = 26;

    /// <summary>
    /// Thêm bật/tắt chức năng mod ở đây
    /// </summary>
    public static ModMenuItemBoolean[] modMenuItemBools = new ModMenuItemBoolean[]
    {
        new ModMenuItemBoolean("Vsync", "Tắt Vsync nếu bạn muốn điều chỉnh FPS!", true, "isvsync"),
    };

    /// <summary>
    /// Thêm điều chỉnh chỉ số chức năng mod ở đây
    /// </summary>
    public static ModMenuItemInt[] modMenuItemInts = new ModMenuItemInt[]
    {
        new ModMenuItemInt("FPS", "FPS mục tiêu (cần tắt Vsync để thay đổi có hiệu lực)", 60, "targetfps"),
        new ModMenuItemInt("Test", new string[]{"Đang tắt", "Đang bật mức 1", "Đang bật mức 2", "Đang bật mức 3"}, 0, "test")
    };

    public static string[][] inputModMenuItemInts = new string[][]
    {
        new string[]{"Nhập mức FPS", "FPS"},
    };

    public static void SaveData()
    {
        foreach (ModMenuItemBoolean modMenuItem in modMenuItemBools) Rms.saveRMSBool(modMenuItem.RMSName, modMenuItem.Value);    
        foreach (ModMenuItemInt modMenuItem in modMenuItemInts) Rms.saveRMSInt2(modMenuItem.RMSName, modMenuItem.SelectedValue);    
    }

    public static void LoadData()
    {
        foreach (ModMenuItemBoolean modMenuItem in modMenuItemBools) 
        { 
            try
            {
                modMenuItem.Value = Rms.loadRMSBool(modMenuItem.RMSName);
            }
            catch { }
        }
        QualitySettings.vSyncCount = modMenuItemBools[0].Value ? 1 : 0;
        foreach (ModMenuItemInt modMenuItem in modMenuItemInts)
        {
            try
            {
                int data = Rms.loadRMSInt2(modMenuItem.RMSName);
                modMenuItem.SelectedValue = data == -1 ? 0 : data;
            }
            catch { }
        }
        if (modMenuItemInts[0].SelectedValue < 5 || modMenuItemInts[0].SelectedValue > 60) modMenuItemInts[0].SelectedValue = 60;
        Application.targetFrameRate = modMenuItemInts[0].SelectedValue;
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
