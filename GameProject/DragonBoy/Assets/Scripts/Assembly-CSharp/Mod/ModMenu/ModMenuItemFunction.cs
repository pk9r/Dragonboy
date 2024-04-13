using System;

namespace Mod.ModMenu
{
    internal class ModMenuItemFunction : ModMenuItem
    {
        /// <summary>Hàm được gọi khi được chọn, không có đối số và không trả về giá trị.</summary>
        internal Action Action => _config.Action;

        ModMenuItemFunctionConfig _config;

        /// <summary>
        /// Constructor
        /// </summary>
        /// param name="config">Cấu hình <see cref="ModMenuItemFunction"/></param>
        internal ModMenuItemFunction(ModMenuItemFunctionConfig config) : base(config)
        {
            _config = config;
        }
    }
}