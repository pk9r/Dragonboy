using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mod
{
    public static class Utils
    {
        /// <summary>
        /// Kiểm tra xem game đang chạy trên Android hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên Android, ngược lại trả về false.</returns>
        public static bool IsAndroidBuild() => Application.platform == RuntimePlatform.Android;

        /// <summary>
        /// Kiểm tra xem game đang chạy trên Linux hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên Linux, ngược lại trả về false.</returns>
        public static bool IsLinuxBuild() => Application.platform == RuntimePlatform.LinuxPlayer;

        /// <summary>
        /// Kiểm tra xem game đang chạy trên Windows hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên Windows, ngược lại trả về false.</returns>
        public static bool IsWindowsBuild() => Application.platform == RuntimePlatform.WindowsPlayer;

        /// <summary>
        /// Kiểm tra xem game có đang chạy trên Unity Editor hay không.
        /// </summary>
        /// <returns>Trả về true nếu game đang chạy trên Editor, ngược lại trả về false.</returns>
        public static bool IsEditor() => Application.isEditor;

        /// <summary>
        /// Kiểm tra xem game đang chạy trên điện thoại hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên điện thoại, ngược lại trả về false.</returns>
        public static bool IsMobile() => IsAndroidBuild() || Application.platform == RuntimePlatform.IPhonePlayer;

        /// <summary>
        /// Kiểm tra xem game đang chạy trên PC hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên PC, ngược lại trả về false.</returns>
        public static bool IsPC() => !IsMobile();

        internal static void CheckBackButtonPress()
        {
            if (GameCanvas.panel != null || GameCanvas.panel2 != null)
            {
                if (GameCanvas.panel != null && GameCanvas.panel.isShow)
                {
                    GameCanvas.panel.hide();
                    return;
                }
                if (GameCanvas.panel2 != null && GameCanvas.panel2.isShow)
                {
                    GameCanvas.panel2.hide();
                    return;
                }
            }
            if (InfoDlg.isShow)
                return;
            if (GameCanvas.currentDialog != null && GameCanvas.currentDialog is MsgDlg)
            {
                GameCanvas.endDlg();
                return;
            }
            if (ChatTextField.gI().isShow)
            {
                ChatTextField.gI().close();
                return;
            }
            if (GameCanvas.menu.showMenu)
            {
                GameCanvas.menu.closeMenu();
                return;
            }
            GameCanvas.checkBackButton();
        }

        internal static void resetSize(this GameCanvas gameCanvas)
        {
            GameCanvas.w = MotherCanvas.instance.getWidthz();
            GameCanvas.h = MotherCanvas.instance.getHeightz();
            GameCanvas.hw = GameCanvas.w / 2;
            GameCanvas.hh = GameCanvas.h / 2;
            GameCanvas.wd3 = GameCanvas.w / 3;
            GameCanvas.hd3 = GameCanvas.h / 3;
            GameCanvas.w2d3 = 2 * GameCanvas.w / 3;
            GameCanvas.h2d3 = 2 * GameCanvas.h / 3;
            GameCanvas.w3d4 = 3 * GameCanvas.w / 4;
            GameCanvas.h3d4 = 3 * GameCanvas.h / 4;
            GameCanvas.wd6 = GameCanvas.w / 6;
            GameCanvas.hd6 = GameCanvas.h / 6;
            GameCanvas.isTouch = true;
            if (GameCanvas.w >= 240)
                GameCanvas.isTouchControl = true;
            if (GameCanvas.w < 320)
            {
                GameCanvas.isTouchControlSmallScreen = true;
                GameCanvas.isTouchControlLargeScreen = false;
            }
            if (GameCanvas.w >= 320)
            {
                GameCanvas.isTouchControlSmallScreen = false;
                GameCanvas.isTouchControlLargeScreen = true;
            }
            if (GameCanvas.h <= 160)
            {
                Paint.hTab = 15;
                mScreen.cmdH = 17;
            }
            GameScr.d = ((GameCanvas.w <= GameCanvas.h) ? GameCanvas.h : GameCanvas.w) + 20;
            Panel.WIDTH_PANEL = 176;
            if (Panel.WIDTH_PANEL > GameCanvas.w)
                Panel.WIDTH_PANEL = GameCanvas.w;
            GameCanvas.panel?.chatTField?.ResetTextField();
            GameCanvas.panel2?.chatTField?.ResetTextField();
        }

        internal static void ResetTextField(this ChatTextField chatTextField)
        {
            chatTextField.left = new Command(mResources.OK, chatTextField, 8000, null, 1, GameCanvas.h - mScreen.cmdH + 1);
            chatTextField.right = new Command(mResources.DELETE, chatTextField, 8001, null, GameCanvas.w - 70, GameCanvas.h - mScreen.cmdH + 1);
            chatTextField.center = null;
            chatTextField.w = chatTextField.tfChat.width + 20;
            chatTextField.h = chatTextField.tfChat.height + 26;
            chatTextField.x = GameCanvas.w / 2 - chatTextField.w / 2;
            chatTextField.tfChat.y = GameCanvas.h - 40 - chatTextField.tfChat.height;
            chatTextField.y = chatTextField.tfChat.y - 18;
            if (Main.isPC && chatTextField.w > 320)
                chatTextField.w = 320;
            chatTextField.left.x = chatTextField.x;
            chatTextField.right.x = chatTextField.x + chatTextField.w - 68;
            if (GameCanvas.isTouch)
            {
                //tfChat.y -= 5;
                chatTextField.y -= 15;
                chatTextField.h += 30;
                chatTextField.left.x = GameCanvas.w / 2 - 68 - 5;
                chatTextField.right.x = GameCanvas.w / 2 + 5;
                chatTextField.left.y = GameCanvas.h - 30;
                chatTextField.right.y = GameCanvas.h - 30;
            }
            chatTextField.yBegin = chatTextField.tfChat.y;
            chatTextField.yUp = GameCanvas.h / 2 - 2 * chatTextField.tfChat.height;
            if (Main.isWindowsPhone)
                chatTextField.tfChat.showSubTextField = false;
            if (Main.isIPhone)
                chatTextField.tfChat.isPaintMouse = false;
            chatTextField.tfChat.name = "chat";
            if (Main.isWindowsPhone)
                chatTextField.tfChat.strInfo = chatTextField.tfChat.name;
            chatTextField.tfChat.width = GameCanvas.w - 6;
            if (Main.isPC && chatTextField.tfChat.width > 250)
                chatTextField.tfChat.width = 250;
            chatTextField.tfChat.height = mScreen.ITEM_HEIGHT + 2;
            chatTextField.tfChat.x = GameCanvas.w / 2 - chatTextField.tfChat.width / 2;
            chatTextField.tfChat.isFocus = true;
            chatTextField.tfChat.setMaxTextLenght(80);
        }

        internal static void SetGamePadZone(this GamePad gamePad)
        {
            gamePad.isSmallGamePad = GameCanvas.w < 320;
            gamePad.isMediumGamePad = GameCanvas.w >= 320 && GameCanvas.w <= 400;
            gamePad.isLargeGamePad = GameCanvas.w > 400;
            gamePad.xZone = 0;
            if (!gamePad.isLargeGamePad)
            {
                gamePad.wZone = GameCanvas.hw / 3 * 2;
                gamePad.yZone = GameCanvas.h / 2;
                gamePad.hZone = GameCanvas.h - 80;
            }
            else
            {
                gamePad.wZone = GameCanvas.hw / 3 * 1;
                gamePad.yZone = GameCanvas.hh / 2;
                gamePad.hZone = GameCanvas.h;
            }
        }
    }
}