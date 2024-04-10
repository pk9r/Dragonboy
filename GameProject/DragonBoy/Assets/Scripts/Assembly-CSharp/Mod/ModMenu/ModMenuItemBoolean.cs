using System;

namespace Mod.ModMenu
{
    internal class ModMenuItemBoolean : ModMenuItem
    {
        internal bool Value
        {
            get => GetValueFunc();
            set => SetValueAction(value);
        }
        internal string RMSName { get; }
        internal Action<bool> SetValueAction { get; }
        internal Func<bool> GetValueFunc { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="title">Tiêu đề</param>
        /// <param name="description">Mô tả</param>
        /// <param name="getValue">Hàm được gọi để lấy giá trị hiện tại, không có đối số và trả về giá trị kiểu <see cref="bool"/>.</param>
        /// <param name="setValue">Hàm được gọi để gán giá trị mới, có 1 đối số kiểu <see cref="bool"/> và không trả về giá trị.</param>
        /// <param name="rmsName">Tên tệp lưu dữ liệu</param>
        /// <param name="getIsDisabled">Hàm được gọi để lấy giá trị của trạng thái vô hiệu hóa, không có đối số và trả về giá trị kiểu <see cref="bool"/>.</param>
        /// <param name="disabledReasonCallback">Hàm trả về lý do bị vô hiệu hóa, được gọi khi ModMenuItem được chọn bị vô hiệu hóa.</param>
        internal ModMenuItemBoolean(string id, string title, string description, Func<bool> getValue, Action<bool> setValue, string rmsName = "", Func<bool> getIsDisabled = null, Func<string> disabledReasonCallback = null) : base(id, title, description, getIsDisabled, disabledReasonCallback)
        {
            SetValueAction = setValue;
            GetValueFunc = getValue;
            RMSName = rmsName;
        }

        internal void SwitchSelection() => SetValueAction(!GetValueFunc());

        public override bool Equals(object obj)
        {
            return obj is ModMenuItemBoolean boolean &&
                   base.Equals(obj) &&
                   SetValueAction.Method == boolean.SetValueAction.Method &&
                   GetValueFunc.Method == boolean.GetValueFunc.Method &&
                   RMSName == boolean.RMSName;
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
            hash.Add(Value);
            hash.Add(RMSName);
            hash.Add(SetValueAction);
            return hash.ToHashCode();
        }
    }

}
