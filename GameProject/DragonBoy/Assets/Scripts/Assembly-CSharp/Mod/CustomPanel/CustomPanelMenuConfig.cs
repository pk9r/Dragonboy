using System;

namespace Mod.CustomPanel
{
    internal class CustomPanelMenuConfig
    {
        /// <summary>
        /// Hàm đặt các thông số mặc định của panel
        /// </summary>
        internal Action<Panel> SetTabAction { get; set; }
        /// <summary>
        /// Hàm được gọi khi người dùng chọn 1 item
        /// </summary>
        internal Action<Panel> DoFireItemAction { get; set; }
        /// <summary>
        /// Hàm vẽ header tab (tham khảo <see cref="Panel.paintTab(mGraphics)"/>
        /// </summary>
        internal Action<Panel, mGraphics> PaintTabHeaderAction { get; set; }
        /// <summary>
        /// Hàm vẽ phần trên của panel, mặc định sẽ vẽ thông tin bản mod, tên nhân vật và máy chủ
        /// </summary>
        internal Action<Panel, mGraphics> PaintTopInfoAction { get; set; }
        /// <summary>
        /// Hàm vẽ panel
        /// </summary>
        internal Action<Panel, mGraphics> PaintAction { get; set; }
    }
}