using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
            bool result = ChatCommandHandler.checkAndExecuteChatCommand(text);

            return result;
        }

        /// <summary>
        /// Kích hoạt sau khi game khởi động thành công.
        /// </summary>
        public static void onGameStarted()
        {
            ChatCommandHandler.loadDefalutCommands();
        }

        /// <summary>
        /// Kích hoạt sau khi load xong KeyMap
        /// </summary>
        /// <param name="h"></param>
        public static void onKeyMapLoaded(Hashtable h)
        {
            Utilities.AddKeyMap(h);
        }

        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr) chưa được xử lý.
        /// </summary>
        public static void onGameScrPressHotkeysUnassigned()
        {
            Utilities.AddHotkeys();
        }
    }
}
