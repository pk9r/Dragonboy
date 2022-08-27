using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.ModMenu
{
    public class ModMenuItemInt
    {
        public string Title { get; set; }

        public string[] Values { get; set; }

        public string Description { get; set; }

        public int SelectedValue { get; set; }

        public string RMSName { get; set; }

        public bool isDisabled { get; set; }

        public string DisabledReason { get; set; }

        public ModMenuItemInt(string title, string[] values = null, string description = "", int selectedValue = 0, string rmsName = "", bool isDisabled = false, string disabledReason = "")
        {
            if (values == null && string.IsNullOrEmpty(description)) throw new ArgumentException("Values and description cannot be null at the same time");
            Title = title;
            Values = values;
            Description = description;
            SelectedValue = selectedValue;
            RMSName = rmsName;
            this.isDisabled = isDisabled;
            DisabledReason = disabledReason;
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
            return Values[SelectedValue];
        }

        public void SwitchSelection()
        {
            if (Values != null)
            {
                SelectedValue++;
                if (SelectedValue > Values.Length - 1) SelectedValue = 0;
            }
            ModMenuPanel.onModMenuIntsValueChanged();
        }

        public void setValue(int value)
        {
            SelectedValue = value;
            ModMenuPanel.onModMenuIntsValueChanged();
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