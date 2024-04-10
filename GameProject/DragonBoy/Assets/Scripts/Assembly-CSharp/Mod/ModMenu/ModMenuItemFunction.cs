using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.ModMenu
{
    internal class ModMenuItemFunction : ModMenuItem
    {
        internal Action Callback { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="title">Tiêu đề</param>
        /// <param name="description">Mô tả</param>
        /// <param name="callback">Hàm được gọi khi được chọn, không có đối số và không trả về giá trị.</param>
        /// <param name="getIsDisabled">Hàm được gọi để lấy giá trị của trạng thái vô hiệu hóa, không có đối số và trả về giá trị kiểu <see cref="bool"/>.</param>
        /// <param name="disabledReasonCallback">Hàm trả về lý do bị vô hiệu hóa, được gọi khi ModMenuItem được chọn bị vô hiệu hóa.</param>
        internal ModMenuItemFunction(string id, string title, string description, Action callback, Func<bool> getIsDisabled = null, Func<string> disabledReasonCallback = null) : base(id, title, description, getIsDisabled, disabledReasonCallback)
        {
            Callback = callback;
        }

        public override bool Equals(object obj)
        {
            return obj is ModMenuItemFunction function &&
                   base.Equals(obj) &&
                   Callback.Method == function.Callback.Method;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), ID, Title, Description, GetIsDisabled, DisabledReasonCallback, Callback);
        }
    }
}