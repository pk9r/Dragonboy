using System;

namespace Mod.ModMenu
{
    internal class ModMenuItemValuesConfig : ModMenuItemConfig
    {
        /// <summary>Danh sách giá trị để lựa chọn</summary>
        internal string[] Values { get; set; }
        /// <summary>Tên tệp lưu dữ liệu</summary>
        internal string RMSName { get; set; }
        /// <summary>Hàm được gọi để lấy giá trị hiện tại, không có đối số và trả về giá trị kiểu <see cref="double"/>.</summary>
        internal Func<double> GetValueFunc { get; set; }
        /// <summary>Hàm được gọi để gán giá trị mới, có 1 đối số kiểu <see cref="double"/> và không trả về giá trị.</summary>
        internal Action<double> SetValueAction { get; set; }
        /// <summary>Tiêu đề của trường nhập liệu <see cref="ChatTextField"/></summary>
        internal string TextFieldTitle { get; set; }
        /// <summary>Gợi ý cho trường nhập liệu <see cref="ChatTextField"/></summary>
        internal string TextFieldHint { get; set; }
        /// <summary>Giá trị tối thiểu của <see cref="ModMenuItemValues"/>, chỉ có hiệu lực khi nhập giá trị bằng <see cref="ChatTextField"/></summary>
        internal double MinValue { get; set; } = int.MinValue;
        /// <summary>Giá trị tối đa của <see cref="ModMenuItemValues"/>, chỉ có hiệu lực khi nhập giá trị bằng <see cref="ChatTextField"/></summary>
        internal double MaxValue { get; set; } = int.MaxValue;
        /// <summary> Quyết định giá trị của <see cref="ModMenuItemValues"/> có phải là số thực hay không. </summary>
        internal bool IsFloatingPoint { get; set; }
    }
}
