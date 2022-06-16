using System.Collections.Generic;
using System.IO;

namespace Mod
{
    public class HistoryChat
    {
        #region Singleton

        static HistoryChat()
        {
        }

        private HistoryChat()
        {
        }

        public static HistoryChat gI { get; } = new HistoryChat();

        #endregion Singleton

        /// <summary>
        /// Chiều cao hiển thị mỗi gợi ý
        /// </summary>
        public const int HEIGHT_ITEM = 10;

        /// <summary>
		/// Số lượng tối da các gợi ý hiển thị
		/// </summary>
		public const int MAX_ITEM = 10;

        /// <summary>
        /// Chiều rộng cho phần text của commnad
        /// </summary>
        public const int WIDTH_COMMAND = 80;

        /// <summary>
        /// Chiều rộng cho phần text của info
        /// </summary>
        public const int WIDTH_INFO = 40;

        public int height;

        public List<string> hints;

        public List<string> histories;

        public int index;

        public bool isShow;

        public int lenghtShow;

        public int scrollValue;

        public int width;

        public int x;

        public int y;

        private string chatBack;

        private bool isFixDoubleEnter;

        /// <summary>
        /// Chiều rộng tối da của khung gợi ý
        /// </summary>
        public int MaxWidth => mGraphics.zoomLevel > 1 ? 280 : 350;

        public void append(string text)
        {
            try
            {
                histories = LitJson.JsonMapper.ToObject<List<string>>(
                        File.ReadAllText(Properties.Resources.PathChatHistory));
            }
            catch
            {
                histories = new List<string>();
            }

            histories.Remove(text);
            histories.Insert(0, text);
            File.WriteAllText(Properties.Resources.PathChatHistory,
                LitJson.JsonMapper.ToJson(histories));
        }

        public void paint(mGraphics g)
        {
            var chatTextField = ChatTextField.gI();

            // Số lượng gợi ý hiển thị
            lenghtShow = hints.Count > MAX_ITEM ? MAX_ITEM : hints.Count;

            // Kích thước bảng gợi ý
            height = (lenghtShow + 1) * HEIGHT_ITEM; // Chừa 1 chỗ cho title
            width = GameCanvas.w - 10 > MaxWidth ? MaxWidth : GameCanvas.w - 10;

            // Chiều dài của Scrollbar
            int lenghtScrollbar = lenghtShow * (height - HEIGHT_ITEM) / hints.Count;

            // Vị trí của bảng gợi ý
            x = (GameCanvas.w - width) / 2;
            y = chatTextField.tfChat.y - 40 - height;

            // Background
            g.setColor(0, 0.75f);
            g.fillRect(x, y, width, height);

            // Title
            g.setColor(0, 1f);
            g.fillRect(x, y, width, HEIGHT_ITEM);
            mFont.tahoma_7_white_pSmall.drawString(g, "Gần đây", x + 5, y, 0);
            if (this.chatBack != hints[index])
            {
                int x = this.x + this.width - mFont.tahoma_7_white_pSmall.getWidth("Nhấn Tab để lựa chọn") - 5;
                mFont.tahoma_7_white_pSmall.drawString(g, "Nhấn Tab để lựa chọn", x, y, 0);
            }

            // Đường ngăn cách title với lệnh gợi ý
            g.setColor(0xffffff, 0.5f);
            g.fillRect(x, y + HEIGHT_ITEM - 1, this.width, 1);

            // History đang chọn
            g.setColor(0x838383, 0.75f);
            g.fillRect(x, y + HEIGHT_ITEM + HEIGHT_ITEM * (index - scrollValue), width - 5, HEIGHT_ITEM);
            g.setColor(0xffffff, 0.75f);
            g.fillRect(x, y + HEIGHT_ITEM + HEIGHT_ITEM * (index - scrollValue), 2, HEIGHT_ITEM);

            // Đường ngăn cách danh sách gợi ý với Scrollbar
            g.setColor(0xffffff, 0.75f);
            g.fillRect(x + width - 5, y + HEIGHT_ITEM, 1, height - HEIGHT_ITEM);

            // Scrollbar
            g.setColor(0xffffff, 0.75f);
            g.fillRect(x + width - 3, y + HEIGHT_ITEM + scrollValue * (height - HEIGHT_ITEM) / hints.Count, 2, lenghtScrollbar);

            // Danh sách gợi ý
            for (int i = scrollValue; i < scrollValue + lenghtShow; i++)
            {
                mFont.tahoma_7_white_pSmall.drawString(g, hints[i], x + 5, y + HEIGHT_ITEM + HEIGHT_ITEM * (i - scrollValue), 0);
            }
        }

        public void show()
        {
            try
            {
                histories = LitJson.JsonMapper.ToObject<List<string>>(
                        File.ReadAllText(Properties.Resources.PathChatHistory));
            }
            catch
            {
                histories = new List<string>();
            }

            isShow = true;
            index = 0;
        }

        public void update()
        {
            var tfChat = ChatTextField.gI().tfChat;

            if (!ChatTextField.gI().isShow)
                isShow = false;

            if (!isShow)
                return;

            if (chatBack != tfChat.getText())
            {
                chatBack = tfChat.getText();
                index = 0;
                scrollValue = 0;
                loadHints();
            }

            var startStr = "";
            var endStr = chatBack;

            // "/hsme/ts/tdc10/h"
            //                ↑
            //     startStr   ]  endStr
            var indexLastChatCommand = chatBack.LastIndexOf('/');
            if (indexLastChatCommand != -1)
            {
                startStr = chatBack.Substring(0, indexLastChatCommand);
                endStr = chatBack.Substring(indexLastChatCommand);
            }

            // Down Arrow
            if (GameCanvas.keyPressed[22])
            {
                index++;

                if (index >= hints.Count)
                {
                    index = hints.Count - 1;
                }
                if (index >= scrollValue + MAX_ITEM)
                {
                    scrollValue = index - (MAX_ITEM - 1);
                }

                tfChat.setText(startStr + hints[index]);

                // Cập nhập chatBak tránh sự phát hiện thay đổi làm mất danh sách đã gợi ý
                chatBack = tfChat.getText();

                GameCanvas.keyPressed[22] = false;
                GameCanvas.clearKeyPressed();
                GameCanvas.clearKeyHold();
            }

            // Up Arrow
            if (GameCanvas.keyPressed[21])
            {
                index--;
                if (index <= 0)
                {
                    index = 0;
                }
                if (index < scrollValue)
                {
                    scrollValue = index;
                }

                tfChat.setText(startStr + hints[index]);

                // Cập nhập chatBack tránh sự phát hiện thay đổi làm mất danh sách đã gợi ý
                chatBack = ChatTextField.gI().tfChat.getText();

                GameCanvas.keyPressed[21] = false;
                GameCanvas.clearKeyPressed();
                GameCanvas.clearKeyHold();
            }

            // Tab
            if (GameCanvas.keyPressed[16])
            {
                if (endStr != hints[index])
                {
                    tfChat.setText(startStr + hints[index]);
                }

                GameCanvas.keyPressed[16] = false;
                GameCanvas.clearKeyPressed();
                GameCanvas.clearKeyHold();
            }
        }

        private void loadHints()
        {
            // Thêm chuỗi đang copy vào phần gợi ý
            var histories = new List<string>(this.histories);
            histories.Remove(UnityEngine.GUIUtility.systemCopyBuffer);
            histories.Insert(0, UnityEngine.GUIUtility.systemCopyBuffer);

            string endStr = chatBack;
            int indexLastCommandChat = chatBack.LastIndexOf('/');
            if (indexLastCommandChat != -1)
            {
                endStr = chatBack.Substring(indexLastCommandChat);
            }

            hints = histories.FindAll(x => x.StartsWith(endStr));
        }
    }
}