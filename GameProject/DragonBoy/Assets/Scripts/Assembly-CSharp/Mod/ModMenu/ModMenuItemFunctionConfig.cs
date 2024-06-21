using System;

namespace Mod.ModMenu
{
    internal class ModMenuItemFunctionConfig : ModMenuItemConfig
    {
        /// <summary>Hàm được gọi khi được chọn, không có đối số và không trả về giá trị.</summary>
        internal Action Action { get; set; }
    }
}
