using System;
using Mod.R;

namespace Mod.ModMenu
{
    internal class ModMenuItemValues : ModMenuItem, IChatable
    {
        internal int SelectedValue
        {
            get => GetValueFunc();
            set => SetValueAction(value);
        }

        /// <summary>Danh sách giá trị để lựa chọn</summary>
        internal string[] Values => _config.Values;
        /// <summary>Tên tệp lưu dữ liệu</summary>
        internal string RMSName => _config.RMSName;
        /// <summary>Hàm được gọi để lấy giá trị hiện tại, không có đối số và trả về giá trị kiểu <see cref="int"/>.</summary>
        internal Action<int> SetValueAction => _config.SetValueAction;
        /// <summary>Hàm được gọi để gán giá trị mới, có 1 đối số kiểu <see cref="int"/> và không trả về giá trị.</summary>
        internal Func<int> GetValueFunc => _config.GetValueFunc;
        /// <summary>Tiêu đề của trường nhập liệu <see cref="ChatTextField"/></summary>
        internal string TextFieldName => _config.TextFieldTitle;
        /// <summary>Gợi ý cho trường nhập liệu <see cref="ChatTextField"/></summary>
        internal string TextFieldHint => _config.TextFieldHint;

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

        internal string getSelectedValue() => Values[SelectedValue];

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
        }

        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(text))
                return;
            if (to == TextFieldName)
            {
                if (int.TryParse(text, out int value))
                {
                    SelectedValue = value;
                    GameScr.info1.addInfo(string.Format(Strings.valueChanged, Title, SelectedValue) + '!', 0);
                }
                else 
                    GameScr.info1.addInfo(Strings.invalidValue + '!', 0);
            }
            else
                currentCTF.isShow = false;
            onCancelChat();
        }

        public void onCancelChat() => currentCTF.ResetTF();
    }
}