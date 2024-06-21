using System;

namespace Mod.ModMenu
{
    internal class ModMenuItemBooleanConfig : ModMenuItemConfig
    {
        /// <summary>Tên tệp lưu dữ liệu</summary>
        internal string RMSName { get; set; } = "";
        /// <summary>Hàm được gọi để lấy giá trị hiện tại, không có đối số và trả về giá trị kiểu <see cref="bool"/>.</summary>
        internal Func<bool> GetValueFunc { get; set; }
        /// <summary>Hàm được gọi để gán giá trị mới, có 1 đối số kiểu <see cref="bool"/> và không trả về giá trị.</summary>
        internal Action<bool> SetValueAction { get; set; }
    }
}
