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

        /// <summary>Tên tệp lưu dữ liệu</summary>
        internal string RMSName => _config.RMSName;
        /// <summary>Hàm được gọi để lấy giá trị hiện tại, không có đối số và trả về giá trị kiểu <see cref="bool"/>.</summary>
        internal Func<bool> GetValueFunc => _config.GetValueFunc;
        /// <summary>Hàm được gọi để gán giá trị mới, có 1 đối số kiểu <see cref="bool"/> và không trả về giá trị.</summary>
        internal Action<bool> SetValueAction => _config.SetValueAction;

        ModMenuItemBooleanConfig _config;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Cấu hình <see cref="ModMenuItemBoolean"/></param>
        internal ModMenuItemBoolean(ModMenuItemBooleanConfig config) : base(config)
        {
            _config = config;
        }

        internal void SwitchSelection() => SetValueAction(!GetValueFunc());
    }
}
