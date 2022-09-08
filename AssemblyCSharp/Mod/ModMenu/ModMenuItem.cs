using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.ModMenu
{
    public class ModMenuItem
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool isDisabled { get; set; }

        public string DisabledReason { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title">Tiêu đề</param>
        /// <param name="description">Mô tả</param>
        /// <param name="isDisabled">Trạng thái vô hiệu hóa</param>
        /// <param name="disabledReason">Lý do bị vô hiệu hóa, được thông báo khi ModMenuItem được chọn đang bị vô hiệu hóa.</param>
        public ModMenuItem(string title, string description, bool isDisabled, string disabledReason)
        {
            Title = title;
            Description = description;
            this.isDisabled = isDisabled;
            DisabledReason = disabledReason;
        }

        public override bool Equals(object obj)
        {
            return obj is ModMenuItem item &&
                   Title == item.Title &&
                   Description == item.Description &&
                   isDisabled == item.isDisabled &&
                   DisabledReason == item.DisabledReason;
        }

        public override int GetHashCode()
        {
            int hashCode = -918142224;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + isDisabled.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisabledReason);
            return hashCode;
        }
    }
}
