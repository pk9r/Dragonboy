using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod;
public class ModMenuItemConfig
{
    public string Title { get; set; }

    public string[] Values { get; set; }

    public int SelectedValue { get; set; }

    public string RMSName { get; set; }

    public ModMenuItemConfig(string title, string[] values, int selectedValue, string rmsName)
    {
        Title = title;
        Values = values;
        SelectedValue = selectedValue;
        RMSName = rmsName;
    }

    public ModMenuItemConfig(string title, string[] values, string rmsName)
    {
        Title = title;
        Values = values;
        RMSName = rmsName;
        SelectedValue = 0;
    }

    public override bool Equals(object obj)
    {
        if (obj is ModMenuItemConfig modMenuItem)
        {
            return modMenuItem.Title == Title && modMenuItem.Values == Values && modMenuItem.SelectedValue == SelectedValue && modMenuItem.RMSName == RMSName;
        }
        return false;
    }

    public string getSelectedValue()
    {
        return Values[SelectedValue].ToLower();
    }

    public void SwitchSelection()
    {
        SelectedValue++;
        if (SelectedValue > Values.Length - 1) SelectedValue = 0;
    }
}
