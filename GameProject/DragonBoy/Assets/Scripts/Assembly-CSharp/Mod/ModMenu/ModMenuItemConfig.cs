using System;

namespace Mod.ModMenu
{
    internal class ModMenuItemConfig
    {
        /// <summary>ID</summary>
        internal string ID { get; set; } = "";
        /// <summary>Tiêu đề</summary>
        internal string Title { get; set; } = "";
        /// <summary>Mô tả</summary>
        internal string Description { get; set; } = "";
        /// <summary>Hàm được gọi để lấy giá trị của trạng thái vô hiệu hóa, không có đối số và trả về giá trị kiểu <see cref="bool"/>.</summary>
        internal Func<bool> GetIsDisabled { get; set; }
        /// <summary>Hàm trả về lý do bị vô hiệu hóa, được gọi khi ModMenuItem được chọn bị vô hiệu hóa.</summary>
        internal Func<string> GetDisabledReason { get; set; }
    }
}
