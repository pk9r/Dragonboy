using System;
using System.Collections.Generic;

namespace Mod.ModMenu
{
    internal class ModMenuItemValues : ModMenuItem
    {
        internal string[] Values { get; }
        internal string RMSName { get; }
        internal int SelectedValue
        {
            get => GetValueFunc();
            set => SetValueAction(value);
        }
        internal Action<int> SetValueAction { get; }
        internal Func<int> GetValueFunc { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="title">Tiêu đề</param>
        /// <param name="values">Danh sách giá trị để lựa chọn</param>
        /// <param name="description">Mô tả</param>
        /// <param name="getValue">Hàm được gọi để lấy giá trị hiện tại, không có đối số và trả về giá trị kiểu <see cref="int"/>.</param>
        /// <param name="setValue">Hàm được gọi để gán giá trị mới, có 1 đối số kiểu <see cref="int"/> và không trả về giá trị.</param>
        /// <param name="rmsName">Tên tệp lưu dữ liệu</param>
        /// <param name="getIsDisabled">Hàm được gọi để lấy giá trị của trạng thái vô hiệu hóa, không có đối số và trả về giá trị kiểu <see cref="bool"/>.</param>
        /// <param name="disabledReasonCallback">Hàm trả về lý do bị vô hiệu hóa, được gọi khi ModMenuItem được chọn bị vô hiệu hóa.</param>
        /// <exception cref="ArgumentException">Danh sách giá trị và mô tả đều bằng <see langword="null"/> hoặc rỗng.</exception>
        internal ModMenuItemValues(string id, string title, Func<int> getValue, Action<int> setValue, string[] values = null, string description = "", string rmsName = "", Func<bool> getIsDisabled = null, Func<string> disabledReasonCallback = null) : base(id, title, description, getIsDisabled, disabledReasonCallback)
        {
            if ((values == null || values.Length <= 0) && string.IsNullOrEmpty(description))
                throw new ArgumentException("Values and description cannot be null at the same time");
            Values = values;
            GetValueFunc = getValue;
            SetValueAction = setValue;
            SelectedValue = getValue();
            RMSName = rmsName;
        }

        internal string getSelectedValue() => Values[SelectedValue];

        internal void SwitchSelection()
        {
            if (Values != null)
            {
                if (SelectedValue < Values.Length - 1)
                    SelectedValue++;
                else 
                    SelectedValue = 0;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is ModMenuItemValues @int &&
                   base.Equals(obj) &&
                   EqualityComparer<string[]>.Default.Equals(Values, @int.Values) &&
                   GetValueFunc.Method == @int.GetValueFunc.Method &&
                   SetValueAction.Method == @int.SetValueAction.Method &&
                   RMSName == @int.RMSName;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(ID);
            hash.Add(Title);
            hash.Add(Description);
            hash.Add(GetIsDisabled);
            hash.Add(DisabledReasonCallback);
            hash.Add(Values);
            hash.Add(RMSName);
            hash.Add(SelectedValue);
            hash.Add(SetValueAction);
            hash.Add(GetValueFunc);
            return hash.ToHashCode();
        }
    }
}