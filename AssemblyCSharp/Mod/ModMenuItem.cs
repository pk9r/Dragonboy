using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod;
public class ModMenuItem
{
    public string Title { get; set; }

    public string Description { get; set; }

    public bool Status { get; set; }

    public string RMSName { get; set; }

    public ModMenuItem(string title, string description, bool status, string rmsName)
    {
        Title = title;
        Description = description;
        Status = status;
        RMSName = rmsName;
    }
    public ModMenuItem(string title, string description, string rmsName)
    {
        Title = title;
        Description = description;
        Status = false;
        RMSName = rmsName;
    }

    public override bool Equals(object obj)
    {
        if (obj is ModMenuItem modMenuItem)
        {
            return modMenuItem.Title == Title && modMenuItem.Description == Description && modMenuItem.Status == Status && modMenuItem.RMSName == RMSName;
        }
        return false;
    }
}
