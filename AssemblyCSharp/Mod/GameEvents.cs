using Mod.Auto;
using Mod.Graphics;
using Mod.ModHelper;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModMenu;
using Mod.PickMob;
using Mod.Xmap;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;
using Vietpad.InputMethod;

namespace Mod
{
    /// <summary>
    /// Định nghĩa các sự kiện của game.
    /// </summary>
    /// <remarks>
    /// - Các hàm bool trả về true thì sự kiện game sẽ không được thực hiện, 
    /// trả về false thì sự kiện sẽ được kích hoạt như bình thường.<br/>
    /// - Các hàm void hỗ trợ thực hiện các lệnh cùng với sự kiện.
    /// </remarks>
    public static class GameEvents
    {
        private static bool isZoomLevelChecked;
        private static string currentDirectory = Environment.CurrentDirectory;

        /// <summary>
        /// Kích hoạt khi người chơi chat.
        /// </summary>
        /// <param name="text">Nội dung chat.</param>
        /// <returns></returns>
        public static bool onSendChat(string text)
        {
            HistoryChat.gI.append(text);
            ExtensionManager.Invoke(text);
            //if (text.StartsWith("/") && (Pk9rXmap.Chat(text.Remove(0, 1)) || Pk9rPickMob.Chat(text.Remove(0, 1))))
            //    return true;
            bool result = ChatCommandHandler.handleChatText(text);
            return result;
        }

        /// <summary>
        /// Kích hoạt sau khi game khởi động.
        /// </summary>
        public static void onGameStarted()
        {
            ChatCommandHandler.loadDefalut();
            HotkeyCommandHandler.loadDefalut();
            SocketClient.gI.initSender();
            ModMenuMain.LoadData();
            CustomBackground.LoadData();
            CustomLogo.LoadData();
            VietKeyHandler.SmartMark = true;
            ExtensionManager.LoadExtensions();
            ExtensionManager.Invoke();
        }

        /// <summary>
        /// Kích hoạt khi game đóng
        /// </summary>
        /// <returns></returns>
        public static bool onGameClosing()
        {
            SocketClient.gI.close();
            ModMenuMain.SaveData();
            TeleportMenu.TeleportMenu.SaveData();
            CustomBackground.SaveData();
            CustomLogo.SaveData();
            ExtensionManager.Invoke();
            return false;
        }

        public static void onSaveRMSString(ref string filename, ref string data)
        {
            if (filename == "acc" || filename == "pass")
                data = "pk9r327";
            ExtensionManager.Invoke(filename, data);
        }

        /// <summary>
        /// Kích hoạt sau khi load KeyMap.
        /// </summary>
        /// <param name="h"></param>
        public static void onKeyMapLoaded(Hashtable h)
        {
            ExtensionManager.Invoke(h);
        }

        /// <summary>
        /// Kích hoạt khi cài đăt kích thước màn hình.
        /// </summary>
        /// <returns></returns>
        public static bool onSetResolution()
        {
            ExtensionManager.Invoke();
            if (Utilities.sizeData != null)
            {
                int width = (int)Utilities.sizeData["width"];
                int height = (int)Utilities.sizeData["height"];
                bool fullScreen = (bool)Utilities.sizeData["fullScreen"];
                if (Screen.width != width || Screen.height != height)
                    Screen.SetResolution(width, height, fullScreen);
                new Thread(delegate ()
                {
                    Thread.Sleep(500);
                    while (Screen.fullScreen != fullScreen)
                    {
                        Screen.fullScreen = fullScreen;
                        Thread.Sleep(100);
                    }
                }).Start();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr) chưa được xử lý.
        /// </summary>
        public static void onGameScrPressHotkeysUnassigned()
        {
            HotkeyCommandHandler.handleHotkey(GameCanvas.keyAsciiPress);
            ExtensionManager.Invoke();
        }

        /// <summary>
        /// Kích hoạt sau khi vẽ khung chat.
        /// </summary>
        /// <param name="g"></param>
        public static void onPaintChatTextField(mGraphics g)
        {
            ExtensionManager.Invoke(g);
            if (ChatTextField.gI().strChat.Replace(" ", "") != "Chat" || ChatTextField.gI().tfChat.name != "chat") return;
            HistoryChat.gI.paint(g);
        }

        /// <summary>
        /// Kích hoạt khi mở khung chat.
        /// </summary>
        public static bool onStartChatTextField(ChatTextField sender)
        {
            if (ChatTextField.gI().strChat.Replace(" ", "") != "Chat" || ChatTextField.gI().tfChat.name != "chat") return false;
            if (sender == ChatTextField.gI())
            {
                HistoryChat.gI.show();
            }

            ExtensionManager.Invoke(sender);
            return false;
        }

        public static bool onLoadRMSInt(string file, out int result)
        {
            if (file == "lowGraphic" && Utilities.sizeData != null)
            {
                result = (int)Utilities.sizeData["lowGraphic"];
                ExtensionManager.Invoke(file, result);
                return true;
            }

            result = -1;
            ExtensionManager.Invoke(file, result);
            return false;
        }

        internal static bool onGetRMSPath(out string result)
        {
            if (Environment.CurrentDirectory != currentDirectory)
                Environment.CurrentDirectory = currentDirectory;
            if (Utilities.server != null)
            {
                GameMidlet.IP = (string)Utilities.server["ip"];
                GameMidlet.PORT = (int)Utilities.server["port"];
            }
            result = $"asset\\{GameMidlet.IP}_{GameMidlet.PORT}_x{mGraphics.zoomLevel}\\";
            if (!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }
            ExtensionManager.Invoke(result);
            return true;
        }

        public static bool onTeleportUpdate(Teleport teleport)
        {
            if (teleport.isMe)
            {
                if (teleport.type == 0)
                    Controller.isStopReadMessage = false;
                else
                    Char.myCharz().isTeleport = false;
            }
            else
            {
                var @char = GameScr.findCharInMap(teleport.id);
                if (@char != null)
                {
                    if (teleport.type == 0)
                        GameScr.vCharInMap.removeElement(@char);
                    else
                        @char.isTeleport = false;
                }
            }

            Teleport.vTeleport.removeElement(teleport);
            ExtensionManager.Invoke(teleport);
            return true;
        }

        /// <summary>
        /// Kích hoạt khi có ChatTextField update.
        /// </summary>
        public static void onUpdateChatTextField(ChatTextField sender)
        {
            if (!string.IsNullOrEmpty((string)typeof(TField).GetField("text", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sender.tfChat))) GameCanvas.keyPressed[14] = false;
            ExtensionManager.Invoke(sender);
        }

        public static bool onClearAllRMS()
        {
            FileInfo[] files = new DirectoryInfo(Rms.GetiPhoneDocumentsPath() + "/").GetFiles();
            foreach (FileInfo fileInfo in files)
                if (fileInfo.Name != "isPlaySound")
                    fileInfo.Delete();

            ExtensionManager.Invoke();
            return true;
        }

        /// <summary>
        /// Kích hoạt khi GameScr.gI() update.
        /// </summary>
        public static void onUpdateGameScr()
        {
            Char.myCharz().cspeed = Utilities.speedRun;
            CharEffect.Update();
            TeleportMenu.TeleportMenu.Update();
            if (GameCanvas.gameTick % (10 * Time.timeScale) == 0) Service.gI().petInfo();
            ListCharsInMap.update();
            AutoGoback.update();
            AutoSS.update();
            AutoT77.update();
            AutoPet.update();
            SuicideRange.update();
            if (!AutoSS.isAutoSS && !AutoT77.isAutoT77) Pk9rPickMob.Update();
            //NOTE onUpdateChatTextField không thể bấm tab.
            if (ChatTextField.gI().strChat.Replace(" ", "") != "Chat" || ChatTextField.gI().tfChat.name != "chat") return;
            HistoryChat.gI.update();
            ExtensionManager.Invoke();
        }

        /// <summary>
        /// Kích hoạt khi gửi yêu cầu đăng nhập.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <param name="type"></param>
        public static void onLogin(ref string username, ref string pass, ref sbyte type)
        {
            username = Utilities.username == "" ? username : Utilities.username;
            if (username.StartsWith("User"))
            {
                pass = string.Empty;
                type = 1;
            }
            else pass = Utilities.password == "" ? pass : Utilities.password;
            //Không thêm hàm này
        }

        /// <summary>
        /// Kích hoạt sau khi màn hình chọn server được load.
        /// </summary>
        public static void onServerListScreenLoaded()
        {
            if (GameCanvas.loginScr == null)
            {
                GameCanvas.loginScr = new LoginScr();
            }

            GameCanvas.loginScr.switchToMe();
            Service.gI().login("", "", GameMidlet.VERSION, 0);
            GameCanvas.startWaitDlg();
            TeleportMenu.TeleportMenu.LoadData();
            AutoPet.isFirstTimeCkeckPet = true;
            ExtensionManager.Invoke();
        }

        /// <summary>
        /// Kích hoạt khi Session kết nối đến server.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public static void onSessionConnecting(ref string host, ref int port)
        {
            if (Utilities.server != null)
            {
                host = (string)Utilities.server["ip"];
                port = (int)Utilities.server["port"];
            }
            ExtensionManager.Invoke(host, port);
        }

        public static void onSceenDownloadDataShow()
        {
            GameCanvas.serverScreen.perform(2, null);
            ExtensionManager.Invoke();
        }

        public static bool onCheckZoomLevel()
        {
            if (Utilities.sizeData != null)
            {
                mGraphics.zoomLevel = (int)Utilities.sizeData["typeSize"];
                ExtensionManager.Invoke();
                return true;
            }
            ExtensionManager.Invoke();
            return false;
        }

        public static bool onKeyPressedz(int keyCode, bool isFromSync)
        {
            if (Utilities.channelSyncKey != -1 && !isFromSync)
            {
                SocketClient.gI.sendMessage(new
                {
                    action = "syncKeyPressed",
                    keyCode,
                    Utilities.channelSyncKey
                });
            }
            ExtensionManager.Invoke(keyCode, isFromSync);
            return false;
        }

        public static bool onKeyReleasedz(int keyCode, bool isFromAsync)
        {
            if (Utilities.channelSyncKey != -1 && !isFromAsync)
            {
                SocketClient.gI.sendMessage(new
                {
                    action = "syncKeyReleased",
                    keyCode,
                    Utilities.channelSyncKey
                });
            }
            ExtensionManager.Invoke(keyCode, isFromAsync);
            return false;
        }

        public static bool onChatPopupMultiLine(string chat)
        {
            Pk9rXmap.Info(chat);
            if (chat.ToLower().Contains("chưa thể chuyển khu") || AutoSS.isAutoSS || AutoT77.isAutoT77)
            {
                GameScr.info1.addInfo(chat, 0);
                ExtensionManager.Invoke(chat);
                return true;
            }
            ExtensionManager.Invoke(chat);
            return false;
        }

        public static bool onAddBigMessage(string chat, Npc npc)
        {
            if (npc.avatar == 1139 || AutoSS.isAutoSS || AutoT77.isAutoT77)
            {
                if (!chat.Contains("NGOCRONGONLINE.COM") && !chat.Contains("Hack, Mod")) GameScr.info1.addInfo(chat, 0);
                ExtensionManager.Invoke(chat, npc);
                return true;
            }
            ExtensionManager.Invoke();
            return false;
        }

        public static void onInfoMapLoaded()
        {
            Utilities.updateWaypointChangeMap();
            ExtensionManager.Invoke();
        }

        public static void onPaintGameScr(mGraphics g)
        {
            ListCharsInMap.paint(g);
            CharEffect.Paint(g);
            SuicideRange.paint(g);
            Boss.Paint(g);
            //CustomGraphics.DrawCircle(g, Char.myCharz().cx, Char.myCharz().cy, 100, 2);
            //if (Char.myCharz().charFocus != null) mFont.tahoma_7_yellow.drawString(g, Extensions.getDistance(Char.myCharz(), Char.myCharz().charFocus).ToString(), GameCanvas.w / 2, 10, mFont.CENTER);
            ExtensionManager.Invoke(g);
        }

        public static bool onUseSkill(Skill skill)
        {
            CharEffect.AddEffectCreatedByMe(skill);
            ExtensionManager.Invoke(skill);
            return false;
        }

        public static void onFixedUpdateMain()
        {
            //Pk9rXmap.Update();
            CustomBackground.FixedUpdate();
            CustomLogo.update();
            ExtensionManager.Invoke();
        }

        public static void onAddInfoMe(string str)
        {
            Pk9rXmap.Info(str);
            if (str.StartsWith("Bạn vừa thu hoạch") && !AutoSS.isNeedMorePean) AutoSS.isHarvestingPean = false;
            if (str.ToLower().Contains("bạn vừa nhận thưởng bùa")) AutoSS.isNhanBua = true;
            ExtensionManager.Invoke(str);
        }

        public static void onUpdateKeyTouchControl()
        {
            ListCharsInMap.updateTouch();
            Boss.UpdateTouch();
            ExtensionManager.Invoke();
        }

        public static void onSetPointItemMap(int xEnd, int yEnd)
        {
            if (xEnd == Char.myCharz().cx && yEnd == Char.myCharz().cy - 10)
            {
                if (AutoSS.isAutoSS) AutoSS.isPicking = false;
                if (ModMenuMain.modMenuItemInts[4].SelectedValue != 0) AutoPet.isPicking = false;
            }
            ExtensionManager.Invoke(xEnd, yEnd);
        }

        public static bool onMenuStartAt(MyVector menuItems)
        {
            if (AutoSS.isAutoSS && menuItems.size() == 2 && ((Command)menuItems.elementAt(0)).caption == "Nhận quà" && ((Command)menuItems.elementAt(1)).caption == "Từ chối")
            {
                GameCanvas.menu.menuSelectedItem = 0;
                ((Command)menuItems.elementAt(0)).performAction();
                AutoSS.isNhapCodeTanThu = true;
                ExtensionManager.Invoke(menuItems);
                return true;
            }
            ExtensionManager.Invoke(menuItems);
            return false;
        }

        public static void onAddInfoChar(string info, Char c)
        {
            if (info.Contains("Sao sư phụ không đánh đi") && ModMenuMain.modMenuItemInts[4].SelectedValue > 0 && c.charID == -Char.myCharz().charID) AutoPet.isSaoMayLuoiThe = true;
            ExtensionManager.Invoke(info, c);
        }

        public static void onLoadImageGameCanvas()
        {
            if (!isZoomLevelChecked)
            {
                isZoomLevelChecked = true;
                onCheckZoomLevel();
            }
            ExtensionManager.Invoke();
        }

        public static bool onPaintBgGameScr(mGraphics g)
        {
            //UnityEngine.Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), BackgroundVideo.videoPlayer.texture);
            if (CustomBackground.isEnabled && CustomBackground.backgroundWallpapers.Count > 0 && !ModMenuMain.modMenuItemBools[8].isDisabled)
            {
                CustomBackground.paint(g);
                ExtensionManager.Invoke(g);
                return true;
            }
            ExtensionManager.Invoke(g);
            return false;
        }

        public static void onMobStartDie(Mob instance)
        {
            Pk9rPickMob.MobStartDie(instance);
            ExtensionManager.Invoke(instance);
        }

        public static void onUpdateMob(Mob instance)
        {
            Pk9rPickMob.UpdateCountDieMob(instance);
            ExtensionManager.Invoke(instance);
        }

        public static Image onCreateImage(string filename)
        {
            if (Environment.CurrentDirectory != currentDirectory)
                Environment.CurrentDirectory = currentDirectory;
            Image image = new Image();
            Texture2D texture2D = new Texture2D(1, 1);
            if (!Directory.Exists("Game_Data\\CustomAssets")) Directory.CreateDirectory("Game_Data\\CustomAssets");
            if (File.Exists("Game_Data\\CustomAssets\\" + filename.Replace('/', '\\') + ".png"))
            {
                texture2D.LoadImage(File.ReadAllBytes("Game_Data\\CustomAssets\\" + filename.Replace('/', '\\') + ".png"));
            }
            else texture2D = Resources.Load(filename) as Texture2D;
            image.texture = texture2D ?? throw new Exception("NULL POINTER EXCEPTION AT Image onCreateImage " + filename);
            image.w = image.texture.width;
            image.h = image.texture.height;
            image.texture.anisoLevel = 0;
            image.texture.filterMode = FilterMode.Point;
            image.texture.mipMapBias = 0f;
            image.texture.wrapMode = TextureWrapMode.Clamp;
            ExtensionManager.Invoke(filename);
            return image;
        }

        public static void onChatVip(string chatVip)
        {
            Boss.AddBoss(chatVip);
            ExtensionManager.Invoke(chatVip);
        }
    }
}