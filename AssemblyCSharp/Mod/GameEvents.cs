using System.Collections;

namespace Mod
{
    public class GameEvents
    {
        /// <summary>
        /// Kích hoạt khi người chơi chat.
        /// </summary>
        /// <param name="text">Nội dung chat.</param>
        /// <returns>true nếu huỷ bỏ nội dung chat.</returns>
        public static bool onSendChat(string text)
        {
            HistoryChat.gI.append(text);
            bool result = ChatCommandHandler.handleChatText(text);

            return result;
        }

        /// <summary>
        /// Kích hoạt sau khi game khởi động thành công.
        /// </summary>
        public static void onGameStarted()
        {
            ChatCommandHandler.loadDefalut();
            HotkeyCommandHandler.loadDefalut();
        }

        /// <summary>
        /// Kích hoạt sau khi load xong KeyMap.
        /// </summary>
        /// <param name="h"></param>
        public static void onKeyMapLoaded(Hashtable h)
        {
        }

        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr) chưa được xử lý.
        /// </summary>
        public static void onGameScrPressHotkeysUnassigned()
        {
            HotkeyCommandHandler.handleHotkey(GameCanvas.keyAsciiPress);
        }

        /// <summary>
        /// Kích hoạt trong khi vẽ khung chat.
        /// </summary>
        /// <param name="g"></param>
        public static void onPaintChatTextField(mGraphics g)
        {
            HistoryChat.gI.paint(g);
        }

        /// <summary>
        /// Kích hoạt khi bắt đầu chat.
        /// </summary>
        public static void onStartChatTextField()
        {
            HistoryChat.gI.show();
        }

        /// <summary>
        /// Kích hoạt khi có ChatTextField update.
        /// </summary>
        public static void onUpdateChatTextField()
        {
        }

        /// <summary>
        /// Kích hoạt khi GameScr update.
        /// </summary>
        public static void onUpdateGameScr()
        {
            Char.myCharz().cspeed = Utilities.speedRun;
            
            HistoryChat.gI.update();
        }
    }
}