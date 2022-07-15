using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod;
public class ModMenuItemBoolean
{
    public string Title { get; set; }

    public string Description { get; set; }

    public bool Value { get; set; }

    public string RMSName { get; set; }

    public ModMenuItemBoolean(string title, string description, bool value, string rmsName)
    {
        Title = title;
        Description = description;
        Value = value;
        RMSName = rmsName;
    }
    public ModMenuItemBoolean(string title, string description, string rmsName)
    {
        Title = title;
        Description = description;
        Value = false;
        RMSName = rmsName;
    }

    public override bool Equals(object obj)
    {
        if (obj is ModMenuItemBoolean modMenuItem)
        {
            return modMenuItem.Title == Title && modMenuItem.Description == Description && modMenuItem.Value == Value && modMenuItem.RMSName == RMSName;
        }
        return false;
    }
}
