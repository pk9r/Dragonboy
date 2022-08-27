using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.ModMenu
{
    public class ModMenuItemBoolean
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool Value { get; set; }

        public string RMSName { get; set; }

        public bool isDisabled { get; set; }

        public string DisabledReason { get; set; }

        public ModMenuItemBoolean(string title, string description, bool value = false, string rmsName = "", bool isDisabled = false, string disabledReason = "")
        {
            Title = title;
            Description = description;
            Value = value;
            RMSName = rmsName;
            this.isDisabled = isDisabled;
            DisabledReason = disabledReason;
        }

        public override bool Equals(object obj)
        {
            if (obj is ModMenuItemBoolean modMenuItem)
            {
                return modMenuItem.Title == Title && modMenuItem.Description == Description && modMenuItem.Value == Value && modMenuItem.RMSName == RMSName && modMenuItem.isDisabled == isDisabled;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = -1012648466;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RMSName);
            return hashCode;
        }

        public void setValue(bool value)
        {
            Value = value;
            ModMenuPanel.onModMenuBoolsValueChanged();
        }
    }
}