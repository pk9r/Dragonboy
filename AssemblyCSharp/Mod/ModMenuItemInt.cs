using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod
{
    public class ModMenuItemInt
    {
        public string Title { get; set; }

        public string[] Values { get; set; }

        public int SelectedValue { get; set; }

        public string RMSName { get; set; }

        public string Description { get; set; }

        public ModMenuItemInt(string title, string[] values, int selectedValue, string rmsName)
        {
            Title = title;
            Values = values;
            SelectedValue = selectedValue;
            RMSName = rmsName;
            Description = null;
        }

        public ModMenuItemInt(string title, string[] values, string rmsName)
        {
            Title = title;
            Values = values;
            RMSName = rmsName;
            SelectedValue = 0;
            Description = null;
        }

        public ModMenuItemInt(string title, string description, int selectedValue, string rmsName)
        {
            Title = title;
            Values = null;
            SelectedValue = selectedValue;
            RMSName = rmsName;
            Description = description;
        }

        public ModMenuItemInt(string title, string description, string rmsName)
        {
            Title = title;
            Values = null;
            SelectedValue = 0;
            RMSName = rmsName;
            Description = description;
        }

        public override bool Equals(object obj)
        {
            if (obj is ModMenuItemInt modMenuItem)
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
            if (Values != null)
            {
                SelectedValue++;
                if (SelectedValue > Values.Length - 1) SelectedValue = 0;
            }
        }

        public void setValue(int value)
        {
            SelectedValue = value;
        }

        public override int GetHashCode()
        {
            int hashCode = -1820188900;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(Values);
            hashCode = hashCode * -1521134295 + SelectedValue.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RMSName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            return hashCode;
        }
    }
}