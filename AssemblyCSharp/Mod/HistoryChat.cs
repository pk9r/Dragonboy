using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

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
        /// Chiều cao hiển thị một gợi ý.
        /// </summary>
        public const int HEIGHT_HINT_ITEM = 10;

        /// <summary>
		/// Số lượng tối da các gợi ý hiển thị.
		/// </summary>
		public const int MAX_HINTS_ITEM = 10;

        /// <summary>
        /// Danh sách gợi ý.
        /// </summary>
        public List<string> hints = new List<string>();

        /// <summary>
        /// Thứ tự lệnh gợi ý lựa chọn.
        /// </summary>
        public int selectedIndex;

        /// <summary>
        /// Trạng thái hiển thị.
        /// </summary>
        public bool isShow;

        /// <summary>
        /// Số lượng gợi ý hiển thị
        /// </summary>
        public int lenghtHintsShow;

        /// <summary>
        /// Giá trị thanh cuộn.
        /// </summary>
        public int scrollValue;

        /// <summary>
        /// Chiều rộng khung gợi ý.
        /// </summary>
        public int width;

        /// <summary>
        /// Chiều cao khung gợi ý.
        /// </summary>
        public int height;

        /// <summary>
        /// Toạ độ x khung gợi ý.
        /// </summary>
        public int x;

        /// <summary>
        /// Toạ độ y khung gợi ý.
        /// </summary>
        public int y;

        /// <summary>
        /// Lưu chuỗi chat cũ.
        /// </summary>
        private string chatBack;

        /// <summary>
        /// Chiều rộng tối da của khung gợi ý.
        /// </summary>
        public int maxWidth => mGraphics.zoomLevel > 1 ? 280 : 350;

        public void append(string text)
        {
            var histories = new List<string>();
            try
            {
                histories = LitJson.JsonMapper.ToObject<List<string>>(
                        File.ReadAllText(Utilities.PathChatHistory));
            }
            catch { }
            if (histories == null)
                histories = new List<string>();
            histories.Remove(text);
            histories.Insert(0, text);

            try
            {
                File.WriteAllText(Utilities.PathChatHistory,
                    LitJson.JsonMapper.ToJson(histories));
            }
            catch { }
        }

        public void paint(mGraphics g)
        {
            if (hints.Count > 0)
            {
                var chatTextField = ChatTextField.gI();

                // Số lượng gợi ý hiển thị
                lenghtHintsShow = hints.Count < MAX_HINTS_ITEM ? hints.Count : MAX_HINTS_ITEM;

                // Kích thước bảng gợi ý
                height = (lenghtHintsShow + 1) * HEIGHT_HINT_ITEM; // Chừa 1 chỗ cho title
                width = GameCanvas.w - 10 > maxWidth ? maxWidth : GameCanvas.w - 10;

                // Chiều dài của Scrollbar
                int lenghtScrollbar = lenghtHintsShow * (height - HEIGHT_HINT_ITEM) / hints.Count;

                // Vị trí của bảng gợi ý
                x = (GameCanvas.w - width) / 2;
                y = chatTextField.tfChat.y - 40 - height;

                // Background
                g.setColor(0, 0.75f);
                g.fillRect(x, y, width, height);

                // Title
                g.setColor(0, 1f);
                g.fillRect(x, y, width, HEIGHT_HINT_ITEM);
                mFont.tahoma_7_white_pSmall.drawString(g, "Gần đây", x + 5, y, 0);
                if (this.chatBack != hints[selectedIndex])
                {
                    int x = this.x + this.width - mFont.tahoma_7_white_pSmall.getWidth("Nhấn Tab để lựa chọn") - 5;
                    mFont.tahoma_7_white_pSmall.drawString(g, "Nhấn Tab để lựa chọn", x, y, 0);
                }

                // Đường ngăn cách title với lệnh gợi ý
                g.setColor(0xffffff, 0.5f);
                g.fillRect(x, y + HEIGHT_HINT_ITEM - 1, this.width, 1);

                // History đang chọn
                g.setColor(0x838383, 0.75f);
                g.fillRect(x, y + HEIGHT_HINT_ITEM + HEIGHT_HINT_ITEM * (selectedIndex - scrollValue), width - 5, HEIGHT_HINT_ITEM);
                g.setColor(0xffffff, 0.75f);
                g.fillRect(x, y + HEIGHT_HINT_ITEM + HEIGHT_HINT_ITEM * (selectedIndex - scrollValue), 2, HEIGHT_HINT_ITEM);

                // Đường ngăn cách danh sách gợi ý với Scrollbar
                g.setColor(0xffffff, 0.75f);
                g.fillRect(x + width - 5, y + HEIGHT_HINT_ITEM, 1, height - HEIGHT_HINT_ITEM);

                // Scrollbar
                g.setColor(0xffffff, 0.75f);
                g.fillRect(x + width - 3, y + HEIGHT_HINT_ITEM + scrollValue * (height - HEIGHT_HINT_ITEM) / hints.Count, 2, lenghtScrollbar);

                // Danh sách gợi ý
                for (int i = scrollValue; i < scrollValue + lenghtHintsShow; i++)
                {
                    mFont.tahoma_7_white_pSmall.drawString(g, hints[i], x + 5, y + HEIGHT_HINT_ITEM + HEIGHT_HINT_ITEM * (i - scrollValue), 0);
                }
            }
        }

        public void show()
        {
            isShow = true;
            selectedIndex = 0;
            loadHints();
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
                selectedIndex = 0;
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
                selectedIndex++;

                if (selectedIndex >= hints.Count)
                {
                    selectedIndex = hints.Count - 1;
                }
                if (selectedIndex >= scrollValue + MAX_HINTS_ITEM)
                {
                    scrollValue = selectedIndex - (MAX_HINTS_ITEM - 1);
                }

                tfChat.setText(startStr + hints[selectedIndex]);

                // Cập nhập chatBak tránh sự phát hiện thay đổi làm mất danh sách đã gợi ý
                chatBack = tfChat.getText();

                GameCanvas.keyPressed[22] = false;
                GameCanvas.clearKeyPressed();
                GameCanvas.clearKeyHold();
            }

            // Up Arrow
            if (GameCanvas.keyPressed[21])
            {
                selectedIndex--;
                if (selectedIndex <= 0)
                {
                    selectedIndex = 0;
                }
                if (selectedIndex < scrollValue)
                {
                    scrollValue = selectedIndex;
                }

                tfChat.setText(startStr + hints[selectedIndex]);

                // Cập nhập chatBack tránh sự phát hiện thay đổi làm mất danh sách đã gợi ý
                chatBack = ChatTextField.gI().tfChat.getText();

                GameCanvas.keyPressed[21] = false;
                GameCanvas.clearKeyPressed();
                GameCanvas.clearKeyHold();
            }

            // Tab
            if (GameCanvas.keyPressed[16])
            {
                try
                {
                    if (endStr != hints[selectedIndex])
                        tfChat.setText(startStr + hints[selectedIndex]);
                }
                catch (Exception) { }
                GameCanvas.keyPressed[16] = false;
                GameCanvas.clearKeyPressed();
                GameCanvas.clearKeyHold();
            }
        }

        private void loadHints()
        {
            var histories = new List<string>();

            try
            {
                histories = LitJson.JsonMapper.ToObject<List<string>>(
                        File.ReadAllText(Utilities.PathChatHistory));
            }
            catch { }

            // Thêm đoạn copy vào lịch sử chat.
            int copyIndex = histories.IndexOf(UnityEngine.GUIUtility.systemCopyBuffer);
            if (copyIndex == -1)
            {
                histories.Insert(0, UnityEngine.GUIUtility.systemCopyBuffer);
            }
            else if (copyIndex > 10)
            {
                histories.RemoveAt(copyIndex);
                histories.Insert(0, UnityEngine.GUIUtility.systemCopyBuffer);
            }

            var tfChat = ChatTextField.gI().tfChat;

            string endStr = tfChat.getText();
            int indexLastCommandChat = endStr.LastIndexOf('/');
            if (indexLastCommandChat != -1)
                endStr = endStr.Substring(indexLastCommandChat);

            hints = histories.FindAll(x => x.StartsWith(endStr));
        }


        /// <summary>
        /// <param name="pattern"> sử dụng biểu thức chính quy để tìm và loại bỏ các chuỗi auto chat </param>
        /// <param name=",\s* ">tìm dấu phẩy và khoảng trắng trước chuỗi gaXX</param>
        /// <param name="\[\d{2}\]">tìm số XX trong dấu ngoặc vuông []</param>
        /// <param name=":\s*"> tìm dấu hai chấm và khoảng trắng sau chuỗi gaXX</param>
        /// <param name="[^""]*">  tìm bất kỳ ký tự nào không phải dấu ngoặc kép "</param>
        /// <param name="""[^""]*"""> tìm chuỗi kí tự trong dấu ngoặc kép ""</param>
        /// </summary>
        public static void clearStringTrash()
        {
            // Đọc nội dung file text
            string content = File.ReadAllText(Utilities.PathChatHistory);

            string pattern = @",?\s*\ga\d{2}\:\s*[^""]*""[^""]*""";

            string output = Regex.Replace(content, pattern, "");

            // Ghi nội dung vào file output
            File.WriteAllText(Utilities.PathChatHistory, output);
        }
    }
}