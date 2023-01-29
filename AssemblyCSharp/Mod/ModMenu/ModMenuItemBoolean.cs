using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.ModMenu
{
    public class ModMenuItemBoolean : ModMenuItem
    {
        public bool Value { get; set; }

        public string RMSName { get; private set; }

        public Action<bool> Action { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title">Tiêu đề</param>
        /// <param name="description">Mô tả</param>
        /// <param name="action">Hàm được gọi khi giá trị hiện tại thay đổi, có 1 đối số kiểu boolean và không trả về giá trị.</param>
        /// <param name="value">Giá trị hiện tại</param>
        /// <param name="rmsName">Tên tệp lưu dữ liệu</param>
        /// <param name="isDisabled">Trạng thái vô hiệu hóa</param>
        /// <param name="disabledReason">Lý do bị vô hiệu hóa, được thông báo khi ModMenuItem được chọn đang bị vô hiệu hóa.</param>
        public ModMenuItemBoolean(string title, string description, Action<bool> action, bool value = false, string rmsName = "", bool isDisabled = false, string disabledReason = "") : base(title, description, isDisabled, disabledReason)
        {
            Action = action;
            Value = value;
            RMSName = rmsName;
        }

        public void setValue(bool value)
        {
            Value = value;
            if (Action != null) Action(value);
            ModMenuMain.onModMenuValueChanged();
        }

        public override bool Equals(object obj)
        {
            return obj is ModMenuItemBoolean boolean &&
                   base.Equals(obj) &&
                   Title == boolean.Title &&
                   Description == boolean.Description &&
                   isDisabled == boolean.isDisabled &&
                   DisabledReason == boolean.DisabledReason &&
                   Value == boolean.Value &&
                   RMSName == boolean.RMSName;
        }

        public override int GetHashCode()
        {
            int hashCode = -1011152150;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + isDisabled.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisabledReason);
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RMSName);
            return hashCode;
        }
    }
}