using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod.ModMenu
{
    public class ModMenuItemInt : ModMenuItem
    {
        public string[] Values { get; private set; }

        public int SelectedValue { get; set; }

        public string RMSName { get; private set; }

        public Action<int> Action { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title">Tiêu đề</param>
        /// <param name="values">Danh sách giá trị để lựa chọn</param>
        /// <param name="description">Mô tả</param>
        /// <param name="selectedValue">Giá trị được chọn</param>
        /// <param name="action">Hàm được gọi khi giá trị được chọn thay đổi, có 1 đối số kiểu int32 và không trả về giá trị.</param>
        /// <param name="rmsName">Tên tệp lưu dữ liệu</param>
        /// <param name="isDisabled">Trạng thái vô hiệu hóa</param>
        /// <param name="disabledReason">Lý do bị vô hiệu hóa, được thông báo khi ModMenuItem được chọn đang bị vô hiệu hóa.</param>
        /// <exception cref="ArgumentException">Danh sách giá trị và mô tả đều bằng <see langword="null"/> hoặc rỗng.</exception>
        public ModMenuItemInt(string title, string[] values = null, string description = "", int selectedValue = 0, Action<int> action = null, string rmsName = "", bool isDisabled = false, string disabledReason = "") : base(title, description, isDisabled, disabledReason)
        {
            if ((values == null || values.Length <= 0) && string.IsNullOrEmpty(description)) throw new ArgumentException("Values and description cannot be null at the same time");
            Values = values;
            SelectedValue = selectedValue;
            Action = action;
            RMSName = rmsName;
        }

        public string getSelectedValue()
        {
            return Values[SelectedValue];
        }

        public void SwitchSelection()
        {
            if (Values != null)
            {
                if (SelectedValue < Values.Length - 1) setValue(SelectedValue + 1);
                else setValue(0);
            }
            ModMenuPanel.onModMenuIntsValueChanged();
        }

        public void setValue(int value)
        {
            SelectedValue = value;
            Action?.Invoke(value);
            ModMenuPanel.onModMenuIntsValueChanged();
        }

        public override bool Equals(object obj)
        {
            return obj is ModMenuItemInt @int &&
                   base.Equals(obj) &&
                   Title == @int.Title &&
                   Description == @int.Description &&
                   isDisabled == @int.isDisabled &&
                   DisabledReason == @int.DisabledReason &&
                   EqualityComparer<string[]>.Default.Equals(Values, @int.Values) &&
                   SelectedValue == @int.SelectedValue &&
                   RMSName == @int.RMSName;
        }

        public override int GetHashCode()
        {
            int hashCode = -1244315040;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + isDisabled.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisabledReason);
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(Values);
            hashCode = hashCode * -1521134295 + SelectedValue.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RMSName);
            return hashCode;
        }
    }
}