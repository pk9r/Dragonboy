using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod;
public class ModMenu
{
    /// <summary>
    /// Thêm chức năng mod ở đây
    /// </summary>
    public static ModMenuItem[] modMenuItems = new ModMenuItem[]
    {
        new ModMenuItem("Auto 1", "abcdefflefiofqwhoqhoqeoqwwkndwnkd", "auto1"),
        new ModMenuItem("Auto 2", "regrhtrhrthffedg", "auto2"),
        new ModMenuItem("Auto 3", "ww lkn2jepqw eferdgreg rejyuliukyt", "auto3"),
        new ModMenuItem("Auto 4", "sfwfsdfsfdsdsfsdffvfdvfdfd", "auto4"),
        new ModMenuItem("Auto 5", "dfdsdfffdsggfhjhjkujhkedweq2w", "auto5"),
        new ModMenuItem("Auto 6", "auto", "auto6"),
        new ModMenuItem("Auto 7", "auto", "auto7"),
        new ModMenuItem("Auto 8", "eqwerewfewferfregr", "auto8"),
        new ModMenuItem("Auto 9", "a", "auto9"),
    };

    public static void SaveData()
    {
        foreach (ModMenuItem modMenuItem in modMenuItems) Rms.saveRMSBool(modMenuItem.RMSName, modMenuItem.Status);    
    }

    public static void LoadData()
    {
        try
        {
            foreach (ModMenuItem modMenuItem in modMenuItems) modMenuItem.Status = Rms.loadRMSBool(modMenuItem.RMSName);
        }
        catch { }
    }

    public static bool getStatus(string rmsName)
    {
        foreach (ModMenuItem modMenuItem in modMenuItems)
        {
            if (modMenuItem.RMSName == rmsName) return modMenuItem.Status;
        }
        throw new Exception("Not found any ModMenuItem with RMSName \"" + rmsName + "\"!");
    }

    public static bool getStatus(int index)
    {
        return modMenuItems[index].Status;
    }
}
