using System;
using Mod.R;

namespace Mod.ModMenu
{
    internal class ModMenuItemValues : ModMenuItem, IChatable
    {
        internal double SelectedValue
        {
            get => GetValueFunc();
            set
            {
                if (value < MinValue || value > MaxValue)
                    return;
                SetValueAction(value);
            }
        }

        /// <summary>Danh sách giá trị để lựa chọn</summary>
        internal string[] Values => _config.Values;
        /// <summary>Tên tệp lưu dữ liệu</summary>
        internal string RMSName => _config.RMSName;
        /// <summary>Hàm được gọi để lấy giá trị hiện tại, không có đối số và trả về giá trị kiểu <see cref="double"/>.</summary>
        internal Action<double> SetValueAction => _config.SetValueAction;
        /// <summary>Hàm được gọi để gán giá trị mới, có 1 đối số kiểu <see cref="double"/> và không trả về giá trị.</summary>
        internal Func<double> GetValueFunc => _config.GetValueFunc;
        /// <summary>Tiêu đề của trường nhập liệu <see cref="ChatTextField"/></summary>
        internal string TextFieldName => _config.TextFieldTitle;
        /// <summary>Gợi ý cho trường nhập liệu <see cref="ChatTextField"/></summary>
        internal string TextFieldHint => _config.TextFieldHint;
        /// <summary>Giá trị tối thiểu của <see cref="ModMenuItemValues"/>, chỉ có hiệu lực khi nhập giá trị bằng <see cref="ChatTextField"/></summary>
        internal double MinValue => _config.MinValue;
        /// <summary>Giá trị tối đa của <see cref="ModMenuItemValues"/>, chỉ có hiệu lực khi nhập giá trị bằng <see cref="ChatTextField"/></summary>
        internal double MaxValue => _config.MaxValue;
        /// <summary> Quyết định giá trị của <see cref="ModMenuItemValues"/> có phải là số thực hay không. </summary>
        internal bool IsFloatingPoint => _config.IsFloatingPoint;

        ModMenuItemValuesConfig _config;
        ChatTextField currentCTF;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Cấu hình <see cref="ModMenuItemValues"/></param>
        /// <exception cref="ArgumentException">Danh sách giá trị và mô tả đều bằng <see langword="null"/> hoặc rỗng.</exception>
        internal ModMenuItemValues(ModMenuItemValuesConfig config) : base(config)
        {
            if ((config.Values == null || config.Values.Length <= 0) && string.IsNullOrEmpty(config.Description))
                throw new ArgumentException("Values and description cannot be null at the same time");
            _config = config;
        }

        internal string getSelectedValue() => Values[(int)SelectedValue];

        internal void SwitchSelection()
        {
            if (Values != null)
            {
                if (SelectedValue < Values.Length - 1)
                    SelectedValue++;
                else 
                    SelectedValue = 0;
            }
        }

        internal void StartChat(ChatTextField textField)
        {
            currentCTF = textField;
            textField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
            textField.initChatTextField();
            textField.strChat = string.Empty;
            textField.tfChat.name = TextFieldHint;
            textField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
            textField.startChat2(this, TextFieldName);
            textField.tfChat.setText(SelectedValue.ToString());
        }

        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(text) || to != TextFieldName)
            {
                onCancelChat();
                return;
            }
            text = text.Replace('.', ',');
            double value = 0;
            bool isNumber = double.TryParse(text, out value);
            if (isNumber)
            {
                int value2 = 0;
                isNumber = IsFloatingPoint || int.TryParse(text, out value2);
                if (!IsFloatingPoint && isNumber)
                    value = value2;
            }
            if (isNumber)
            {
                if (MinValue != MaxValue && (value < MinValue || value > MaxValue))
                    GameCanvas.startOKDlg(string.Format(Strings.inputNumberOutOfRange, MinValue, MaxValue) + '!');
                else
                {
                    SelectedValue = value;
                    GameScr.info1.addInfo(string.Format(Strings.valueChanged, Title, SelectedValue) + '!', 0);
                }
            }
            else
                GameCanvas.startOKDlg(Strings.invalidValue + '!');
            onCancelChat();
        }

        public void onCancelChat() => currentCTF.ResetTF();
    }
}