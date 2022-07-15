using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod;
public class ModMenu
{
    /// <summary>
    /// Thêm bật/tắt chức năng mod ở đây
    /// </summary>
    public static ModMenuItemBoolean[] modMenuItemBools = new ModMenuItemBoolean[]
    {
        new ModMenuItemBoolean("Auto thở", "Nếu không thở, bạn sẽ chết vì thiếu oxi!", true, "autobreathe"),
    };

    /// <summary>
    /// Thêm điều chỉnh chỉ số chức năng mod ở đây
    /// </summary>
    public static ModMenuItemConfig[] modMenuItemConfigs = new ModMenuItemConfig[]
    {
        new ModMenuItemConfig("Xóa địa hình", new string[]{ "Tắt", "Mức 1", "Mức 2", "Mức 3" }, "xdhLevel"),
    };

    public static void SaveData()
    {
        foreach (ModMenuItemBoolean modMenuItem in modMenuItemBools) Rms.saveRMSBool(modMenuItem.RMSName, modMenuItem.Value);    
        foreach (ModMenuItemConfig modMenuItem in modMenuItemConfigs) Rms.saveRMSInt(modMenuItem.RMSName, modMenuItem.SelectedValue);    
    }

    public static void LoadData()
    {
        try
        {
            foreach (ModMenuItemBoolean modMenuItem in modMenuItemBools) modMenuItem.Value = Rms.loadRMSBool(modMenuItem.RMSName);
            foreach (ModMenuItemConfig modMenuItem in modMenuItemConfigs)
            {
                int data = Rms.loadRMSInt(modMenuItem.RMSName);
                modMenuItem.SelectedValue = data == -1 ? 0 : data;
            }
        }
        catch { }
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
        foreach (ModMenuItemConfig modMenuItem in modMenuItemConfigs)
        {
            if (modMenuItem.RMSName == rmsName) return modMenuItem.SelectedValue;
        }
        throw new Exception("Not found any ModMenuItemOther with RMSName \"" + rmsName + "\"!");
    }

    public static int getStatusInt(int index)
    {
        return modMenuItemConfigs[index].SelectedValue;
    }
}
