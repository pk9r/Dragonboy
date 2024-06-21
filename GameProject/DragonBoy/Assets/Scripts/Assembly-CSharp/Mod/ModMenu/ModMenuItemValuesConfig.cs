using System;

namespace Mod.ModMenu
{
    internal class ModMenuItemValuesConfig : ModMenuItemConfig
    {
        /// <summary>Danh sách giá trị để lựa chọn</summary>
        internal string[] Values { get; set; }
        /// <summary>Tên tệp lưu dữ liệu</summary>
        internal string RMSName { get; set; }
        /// <summary>Hàm được gọi để lấy giá trị hiện tại, không có đối số và trả về giá trị kiểu <see cref="int"/>.</summary>
        internal Func<int> GetValueFunc { get; set; }
        /// <summary>Hàm được gọi để gán giá trị mới, có 1 đối số kiểu <see cref="int"/> và không trả về giá trị.</summary>
        internal Action<int> SetValueAction { get; set; }
        /// <summary>Tiêu đề của trường nhập liệu <see cref="ChatTextField"/></summary>
        internal string TextFieldTitle { get; set; }
        /// <summary>Gợi ý cho trường nhập liệu <see cref="ChatTextField"/></summary>
        internal string TextFieldHint { get; set; }
        /// <summary>Giá trị tối thiểu của <see cref="ModMenuItemValues"/>, chỉ có hiệu lực khi nhập giá trị bằng <see cref="ChatTextField"/></summary>
        internal int MinValue { get; set; }
        /// <summary>Giá trị tối đa của <see cref="ModMenuItemValues"/>, chỉ có hiệu lực khi nhập giá trị bằng <see cref="ChatTextField"/></summary>
        internal int MaxValue { get; set; }
    }
}
