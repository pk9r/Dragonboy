using System;
using System.Collections.Generic;

namespace Mod.ModMenu
{
    public delegate bool GetBoolStatus();

    public class ModMenuItemBooleanConfig
    {
        /// <summary>Tiêu đề</summary>
        public string Title { get; set; }

        /// <summary>Mô tả</summary>
        public string Description { get; set; }

        /// <summary>Hàm được gọi để biết trạng thái hiện tại của menu, không có đối số và trả về một boolean.</summary>
        public GetBoolStatus GetStatus { get; set; }

        /// <summary>Hàm được gọi khi giá trị hiện tại thay đổi, có 1 đối số kiểu boolean và không trả về giá trị.</summary>
        public Action<bool> Action { get; set; }

        /// <summary>Giá trị hiện tại</summary>
        public bool DefaultValue { get; set; } = false;

        /// <summary>Tên tệp lưu dữ liệu</summary>
        public string RMSName { get; set; } = "";

        /// <summary>Trạng thái vô hiệu hóa</summary>
        public bool IsDisabled { get; set; } = false;

        /// <summary>Lý do bị vô hiệu hóa, được thông báo khi ModMenuItem được chọn đang bị vô hiệu hóa.</summary>
        public string DisabledReason { get; set; } = "";
    }

    public class ModMenuItemBoolean : ModMenuItem
    {
        public bool Value { 
            get { return GetStatus.Invoke(); }
            set { Action.Invoke(value); }
        }

        public string RMSName { get; private set; }

        public GetBoolStatus GetStatus { get; private set; }
        public Action<bool> Action { get; private set; }

        public ModMenuItemBoolean(ModMenuItemBooleanConfig config) : base(config.Title, config.Description, config.IsDisabled, config.DisabledReason)
        {
            Action = config.Action;
            GetStatus = config.GetStatus;
            RMSName = config.RMSName;
            Value = config.DefaultValue;
        }

        public void setValue(bool value)
        {
            Value = value;
            ModMenuMain.onModMenuValueChanged();
        }

        public override bool Equals(object obj)
        {
            return obj is ModMenuItemBoolean boolean &&
                   base.Equals(obj) &&
                   Title == boolean.Title &&
                   Description == boolean.Description &&
                   isDisabled == boolean.isDisabled &&
                   DisabledReason == boolean.DisabledReason &&
                   Value == boolean.Value &&
                   RMSName == boolean.RMSName;
        }

        public override int GetHashCode()
        {
            int hashCode = -1011152150;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + isDisabled.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisabledReason);
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RMSName);
            return hashCode;
        }
    }
}
