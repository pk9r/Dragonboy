using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.ModMenu
{
    internal class ModMenuItem
    {
        internal string ID { get; }
        internal string Title { get; set; }
        internal string Description { get; set; }
        internal Func<bool> GetIsDisabled { get; set; }
        internal Func<string> DisabledReasonCallback { get; set; }
        internal bool IsDisabled => GetIsDisabled != null && GetIsDisabled();
        internal string DisabledReason => DisabledReasonCallback == null ? "" : DisabledReasonCallback();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="title">Tiêu đề</param>
        /// <param name="description">Mô tả</param>
        /// <param name="getIsDisabled">Hàm được gọi để lấy giá trị của trạng thái vô hiệu hóa, không có đối số và trả về giá trị kiểu <see cref="bool"/>.</param>
        /// <param name="disabledReasonCallback">Hàm trả về lý do bị vô hiệu hóa, được gọi khi ModMenuItem được chọn bị vô hiệu hóa.</param>
        internal ModMenuItem(string id, string title, string description, Func<bool> getIsDisabled, Func<string> disabledReasonCallback)
        {
            ID = id;
            Title = title;
            Description = description;
            GetIsDisabled = getIsDisabled;
            DisabledReasonCallback = disabledReasonCallback;
        }

        public override bool Equals(object obj)
        {
            return obj is ModMenuItem item &&
                   Title == item.Title &&
                   Description == item.Description &&
                   GetIsDisabled.Method == item.GetIsDisabled.Method &&
                   DisabledReasonCallback.Method == item.DisabledReasonCallback.Method;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Title, Description, GetIsDisabled, DisabledReasonCallback);
        }
    }
}
