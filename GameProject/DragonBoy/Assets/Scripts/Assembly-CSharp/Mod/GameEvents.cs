using System;
using System.IO;
using System.Linq;
using System.Threading;
using Mod.Auto;
using Mod.Auto.AutoChat;
using Mod.Background;
using Mod.CharEffect;
using Mod.CustomPanel;
using Mod.Graphics;
using Mod.ModHelper;
using Mod.ModMenu;
using Mod.PickMob;
using Mod.R;
using Mod.Set;
using Mod.TeleportMenu;
using Mod.Xmap;
using UnityEngine;

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
    internal static class GameEvents
    {
        static float _previousWidth = Screen.width;
        static float _previousHeight = Screen.height;
        static bool isHaveSelectSkill_old;
        static long lastTimeGamePause;
        static bool isFirstPause = true;
        static GUIStyle style;

        /// <summary>
        /// Kích hoạt sau khi game khởi động.
        /// </summary>
        internal static void OnGameStart()
        {
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
            QualitySettings.vSyncCount = 1;
            if (Utils.IsAndroidBuild())
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.orientation = ScreenOrientation.AutoRotation;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
            }
            OnCheckZoomLevel(Screen.width, Screen.height);
            GameEventHook.InstallAll();
            if (!Directory.Exists(Utils.dataPath))
                Directory.CreateDirectory(Utils.dataPath);
            CustomBackground.LoadData();
            CharEffectMain.Init();
            Setup.loadFile();
            //ChatCommandHandler.loadDefault();
            //HotkeyCommandHandler.loadDefault();
            //SocketClient.gI.initSender();
            CustomBackground.LoadData();
            //CustomLogo.LoadData();
            //CustomCursor.LoadData();
            SetDo.LoadData();
            GraphicsReducer.InitializeTileMap(true);
            //UIReportersManager.AddReporter(Boss.Paint);
            //UIReportersManager.AddReporter(ListCharsInMap.Paint);
            //ShareInfo.gI.toggle(true);
            OnSetResolution();
        }

        /// <summary>
        /// Kích hoạt khi người chơi chat.
        /// </summary>
        /// <param name="text">Nội dung chat.</param>
        /// <returns></returns>
        internal static bool OnSendChat(string text)
        {
            //HistoryChat.gI.append(text);
            //return ChatCommandHandler.handleChatText(text);
            return false;
        }

        /// <summary>
        /// Kích hoạt sau khi <see cref="MonoBehaviour"/> <see cref="Main"/> được kích hoạt.
        /// </summary>
        internal static void OnMainStart()
        {
            if (Main.started)
                return;
            if (Thread.CurrentThread.Name != "Main")
                Thread.CurrentThread.Name = "Main";
            Main.mainThreadName = Thread.CurrentThread.Name;
            Main.isPC = true;
            Main.started = true;
            UIImage.OnStart();
            if (Rms.loadRMSInt("svselect") == -1)
            {
                ServerListScreen.linkDefault = Strings.DEFAULT_IP_SERVERS;
                ServerListScreen.getServerList(Strings.DEFAULT_IP_SERVERS);
            }
        }

        internal static void OnGamePause(bool paused)
        {
            if (mSystem.currentTimeMillis() - lastTimeGamePause > 1000 && !isFirstPause)
            {
                ModMenuMain.SaveData();
                CustomBackground.SaveData();
            }
            lastTimeGamePause = mSystem.currentTimeMillis();
            if (isFirstPause)
                isFirstPause = false;
        }

        /// <summary>
        /// Kích hoạt khi game đóng
        /// </summary>
        internal static void OnGameClosing()
        {
            ModMenuMain.SaveData();
            CustomBackground.SaveData();

            Setup.clearStringTrash();
            //SocketClient.gI.close();
            TeleportMenuMain.SaveData();
            CustomBackground.SaveData();
            //CustomLogo.SaveData();
            //CustomCursor.SaveData();
            SetDo.SaveData();
            //UIReportersManager.ClearReporters();
        }

        internal static void OnFixedUpdateMain()
        {
            if (GameCanvas.currentScreen != null)
            {
                if (!GameCanvas.panel.isShow && GameCanvas.panel2 != null && GameCanvas.panel2.isShow)
                {
                    GameCanvas.isFocusPanel2 = true;
                    GameCanvas.panel2?.update();
                    if (GameCanvas.panel2?.chatTField != null && GameCanvas.panel2.chatTField.isShow)
                        GameCanvas.panel2?.chatTFUpdateKey();
                    else
                        GameCanvas.panel2?.updateKey();
                }
                if (!GameCanvas.panel.isShow && GameCanvas.panel2 != null && GameCanvas.panel2.isShow)
                    if (!GameCanvas.isPointer(GameCanvas.panel2.X, GameCanvas.panel2.Y, GameCanvas.panel2.W, GameCanvas.panel2.H) && GameCanvas.isPointerJustRelease && GameCanvas.panel2.isDoneCombine)
                        GameCanvas.panel2?.hide();
            }
            CustomBackground.Update();
            //CustomLogo.update();
        }

        internal static void OnUpdateMain()
        {
            if (!Main.started)
                return;
            if (_previousWidth != Screen.width || _previousHeight != Screen.height)
            {
                _previousWidth = Screen.width;
                _previousHeight = Screen.height;
                ScaleGUI.initScaleGUI();
                GameCanvas.instance?.ResetSize();
                Utils.ResetTextField(ChatTextField.gI());
                GameScr.gamePad?.SetGamePadZone();
                GameScr.loadCamera(false, -1, -1);
                if (GameCanvas.panel2 != null)
                    GameCanvas.panel2.EmulateSetTypePanel(1);
                ModMenuMain.UpdatePosition();
            }
#if UNITY_ANDROID && !UNITY_EDITOR
            EHVN.FileChooser.Update();
#endif
            MainThreadDispatcher.update();
            //CustomCursor.Update();
        }

        internal static void OnSaveRMSString(ref string filename, ref string data)
        {
            //if (filename == "acc" || filename == "pass")
            //    data = "pk9r327";
        }

        internal static void OnLoadLanguage(sbyte newLanguage)
        {
            Strings.LoadLanguage(newLanguage);
            ModMenuMain.UpdateLanguage(newLanguage);
        }

        /// <summary>
        /// Kích hoạt khi cài đăt kích thước màn hình.
        /// </summary>
        /// <returns></returns>
        internal static void OnSetResolution()
        {
            if (Utils.IsAndroidBuild())
                return;
            if (Utils.sizeData != null)
            {
                int width = (int)Utils.sizeData["width"];
                int height = (int)Utils.sizeData["height"];
                bool fullScreen = (bool)Utils.sizeData["fullScreen"];
                if (Screen.width != width || Screen.height != height)
                    Screen.SetResolution(width, height, fullScreen);
                new Thread(() =>
                {
                    while (Screen.fullScreen != fullScreen)
                    {
                        Screen.fullScreen = fullScreen;
                        Thread.Sleep(100);
                    }
                }).Start();
            }
        }

        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr) chưa được xử lý.
        /// </summary>
        internal static void OnGameScrPressHotkeysUnassigned()
        {
            //HotkeyCommandHandler.handleHotkey(GameCanvas.keyAsciiPress);
        }

        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr).
        /// </summary>
        internal static void OnGameScrPressHotkeys()
        {
            SetDo.UpdateKey();
        }

        /// <summary>
        /// Kích hoạt sau khi vẽ khung chat.
        /// </summary>
        internal static void OnPaintChatTextField(ChatTextField instance, mGraphics g)
        {
            //if (instance == ChatTextField.gI() && instance.strChat.Replace(" ", "") == "Chat" && instance.tfChat.name == "chat")
            //HistoryChat.gI.paint(g);
        }

        /// <summary>
        /// Kích hoạt khi mở khung chat.
        /// </summary>
        internal static bool OnStartChatTextField(ChatTextField sender, IChatable parentScreen)
        {
            sender.parentScreen = parentScreen;
            if (sender.strChat.Replace(" ", "") != "Chat" || sender.tfChat.name != "chat")
                return false;
            //if (sender == ChatTextField.gI())
            //HistoryChat.gI.show();
            return false;
        }

        internal static bool OnGetRMSPath(out string result)
        {
            //result = $"{Application.persistentDataPath}\\{GameMidlet.IP}_{GameMidlet.PORT}_x{mGraphics.zoomLevel}\\";
            string subFolder = $"TeaMobi";
            //string subFolder = $"TeaMobi{Path.DirectorySeparatorChar}Vietnam";

            //if (ServerListScreen.address[ServerListScreen.ipSelect] == "dragon.indonaga.com")
            //{
            //    switch (ServerListScreen.language[ServerListScreen.ipSelect])
            //    {
            //        case 1:
            //            subFolder = $"TeaMobi{Path.DirectorySeparatorChar}World";
            //            break;
            //        case 2:
            //            subFolder = $"TeaMobi{Path.DirectorySeparatorChar}Indonaga";
            //            break;
            //    }
            //}

            result = Utils.GetRootDataPath();
            // check ip server lậu, lưu rms riêng
            // ...
            result = Path.Combine(result, subFolder);
            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);
            return true;
        }

        internal static bool OnTeleportUpdate(Teleport teleport)
        {
            if (SpaceshipSkip.isEnabled)
            {
                SpaceshipSkip.Update(teleport);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Kích hoạt khi có ChatTextField update.
        /// </summary>
        internal static void OnUpdateChatTextField(ChatTextField sender)
        {
            if (!string.IsNullOrEmpty(sender.tfChat.getText()))
                GameCanvas.keyPressed[14] = false;
        }

        internal static bool OnClearAllRMS()
        {
            foreach (FileInfo file in new DirectoryInfo(Rms.GetiPhoneDocumentsPath() + "/").GetFiles().Where(f => f.Extension != ".log"))
            {
                try
                {
                    if (file.Name != "isPlaySound")
                        file.Delete();
                }
                catch { }
            }
            return true;
        }

        /// <summary>
        /// Kích hoạt khi <see cref="GameScr.update"/> được gọi.
        /// </summary>
        internal static void OnUpdateGameScr()
        {
            if (GameCanvas.gameTick % (10 * Time.timeScale) == 0)
            {
                //Service.gI().openUIZone();
                //Service.gI().petInfo();
            }
            Char.myCharz().cspeed = Utils.speedRun;

            CharEffectMain.Update();
            TeleportMenuMain.Update();
            //ListCharsInMap.Update();
            AutoGoback.update();
            AutoTrainNewAccount.Update();
            //AutoItem.update();
            AutoTrainPet.Update();
            //SuicideRange.update();
            //if (!AutoSS.isAutoSS && !AutoT77.isAutoT77)
            if (!AutoTrainNewAccount.isEnabled && !AutoGoback.isGoingBack)
            {
                if (Pk9rPickMob.IsTanSat)
                    GameScr.isAutoPlay = GameScr.canAutoPlay = false;
                Pk9rPickMob.Update();
            }
            Boss.Update();
            SetDo.Update();
            AutoPean.Update();
            AutoSkill.Update();
            //NOTE onUpdateChatTextField không thể bấm tab.
            if (ChatTextField.gI().strChat.Replace(" ", "") != "Chat" || ChatTextField.gI().tfChat.name != "chat")
                return;
            //HistoryChat.gI.update();
        }

        /// <summary>
        /// Kích hoạt khi gửi yêu cầu đăng nhập.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <param name="type"></param>
        internal static void OnLogin(ref string username, ref string pass, ref sbyte type)
        {
            username = Utils.username == "" ? username : Utils.username;
            if (username.StartsWith("User"))
            {
                pass = string.Empty;
                type = 1;
            }
            else
                pass = Utils.password == "" ? pass : Utils.password;
        }

        /// <summary>
        /// Kích hoạt sau khi màn hình chọn server được load.
        /// </summary>
        internal static void OnServerListScreenLoaded()
        {
            ModMenuMain.Initialize();

            //if (GameCanvas.loginScr == null)
            //    GameCanvas.loginScr = new LoginScr();
            //GameCanvas.loginScr.switchToMe();
            //Service.gI().login("", "", GameMidlet.VERSION, 0);
            //GameCanvas.startWaitDlg();
            TeleportMenuMain.LoadData();
            AutoTrainPet.isFirstTimeCheckPet = true;
        }

        /// <summary>
        /// Kích hoạt khi Session kết nối đến server.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        internal static void OnSessionConnecting(ref string host, ref int port)
        {
            if (Utils.server != null)
            {
                host = (string)Utils.server["ip"];
                port = (int)Utils.server["port"];
            }
        }

        internal static void OnScreenDownloadDataShow()
        {
            //GameCanvas.serverScreen.perform(2, null);
        }

        internal static bool OnCheckZoomLevel(int w, int h)
        {
            if (Utils.IsAndroidBuild())
            {
                if (w * h >= 2073600)
                    mGraphics.zoomLevel = 4;
                else if (w * h >= 691200)
                    mGraphics.zoomLevel = 3;
                else if (w * h > 153600)
                    mGraphics.zoomLevel = 2;
                else
                    mGraphics.zoomLevel = 1;
            }
            else
            {
                mGraphics.zoomLevel = 2;
                if (w * h < 480000)
                    mGraphics.zoomLevel = 1;
            }
            return true;
        }

        internal static bool OnKeyPressed(int keyCode, bool isFromSync)
        {
            if (Utils.channelSyncKey != -1 && !isFromSync)
            {
                //SocketClient.gI.sendMessage(new
                //{
                //    action = "syncKeyPressed",
                //    keyCode,
                //    Utils.channelSyncKey
                //});
            }
            return false;
        }

        internal static bool OnKeyReleased(int keyCode, bool isFromSync)
        {
            if (Utils.channelSyncKey != -1 && !isFromSync)
            {
                //SocketClient.gI.sendMessage(new
                //{
                //    action = "syncKeyReleased",
                //    keyCode,
                //    Utils.channelSyncKey
                //});
            }
            return false;
        }

        internal static bool OnChatPopupMultiLine(string chat)
        {
            if (chat.ToLower().Contains("chưa thể chuyển khu") || AutoTrainNewAccount.isEnabled)
            {
                GameScr.info1.addInfo(chat, 0);
                return true;
            }
            return false;
        }

        internal static bool OnAddBigMessage(string chat, Npc npc)
        {
            if (npc.avatar == 1139 || AutoTrainNewAccount.isEnabled)
            {
                if (new string[] { "NGOCRONGONLINE.COM", "Hack, Mod" }.Any(s => chat.Contains(s)))
                {
                    GameScr.info1.addInfo(chat, 0);
                    return true;
                }
            }
            return false;
        }

        internal static void OnInfoMapLoaded()
        {
            Utils.UpdateWaypointChangeMap();
        }

        internal static void OnPaintGameScr(mGraphics g)
        {
            ModMenuMain.Paint(g);
            CharEffectMain.Paint(g);
            //UIReportersManager.handlePaintGameScr(g);
        }

        internal static bool OnUseSkill(Char ch)
        {
            if (ch.me)
                CharEffectMain.AddEffectCreatedByMe(ch.myskill);
            return false;
        }

        internal static void OnAddInfoMe(string str)
        {
            Pk9rXmap.Info(str);
            if (str.StartsWith("Bạn vừa thu hoạch") && !AutoTrainNewAccount.isNeedMorePean)
                AutoTrainNewAccount.isHarvestingPean = false;
            if (str.ToLower().Contains("bạn vừa nhận thưởng bùa"))
                AutoTrainNewAccount.isNhanBua = true;
        }

        internal static bool OnUpdateTouchGameScr(GameScr instance)
        {
            ModMenuMain.UpdateTouch();
            if (GameCanvas.isTouchControl)
            {
                if (!TileMap.isOfflineMap())
                    return false;
                if (GameCanvas.isMouseFocus(GameScr.xC, GameScr.yC, 34, 34))
                    mScreen.keyMouse = 15;
                else if (GameCanvas.isMouseFocus(GameScr.xHP, GameScr.yHP, 40, 40))
                {
                    if (Char.myCharz().statusMe != 14)
                        mScreen.keyMouse = 10;
                }
                else if (GameCanvas.isMouseFocus(GameScr.xF, GameScr.yF, 40, 40))
                {
                    if (Char.myCharz().statusMe != 14)
                        mScreen.keyMouse = 5;
                }
                else if (instance.cmdMenu != null && GameCanvas.isMouseFocus(instance.cmdMenu.x, instance.cmdMenu.y, instance.cmdMenu.w / 2, instance.cmdMenu.h))
                    mScreen.keyMouse = 1;
                else
                    mScreen.keyMouse = -1;
                if (GameCanvas.isPointerHoldIn(GameScr.xC, GameScr.yC, 34, 34))
                {
                    mScreen.keyTouch = 15;
                    GameCanvas.isPointerJustDown = false;
                    instance.isPointerDowning = false;
                    if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                    {
                        ChatTextField.gI().startChat(instance, string.Empty);
                        SoundMn.gI().buttonClick();
                        Char.myCharz().currentMovePoint = null;
                        GameCanvas.clearAllPointerEvent();
                    }
                    return true;
                }
            }
            if (instance.mobCapcha != null)
                ;
            else if (GameScr.isHaveSelectSkill)
            {
                if (Char.myCharz().isCharDead())
                    return false;
                if (!instance.isCharging())
                {
                    Skill[] skills = Main.isPC ? GameScr.keySkill : GameScr.onScreenSkill;
                    int xSMax = int.MinValue;
                    int xSMin = int.MaxValue;
                    int ySMax = int.MinValue;
                    int ySMin = int.MaxValue;
                    for (int i = skills.Length - 1; i >= 0; i--)
                    {
                        if (skills[i] != null)
                        {
                            xSMax = Math.Max(GameScr.xS[i], xSMax);
                            xSMin = Math.Min(GameScr.xS[i], xSMin);
                            ySMax = Math.Max(GameScr.yS[i], ySMax);
                            ySMin = Math.Min(GameScr.yS[i], ySMin);
                        }
                    }
                    if (GameCanvas.isPointerHoldIn(GameScr.xSkill - 5, ySMin - 5, xSMax - xSMin + GameScr.wSkill, ySMax - ySMin + GameScr.wSkill))
                    {
                        for (int i = 0; i < GameScr.onScreenSkill.Length; i++)
                        {
                            if (GameCanvas.isPointerHoldIn(GameScr.xSkill + GameScr.xS[i], GameScr.yS[i], GameScr.wSkill, GameScr.wSkill))
                            {
                                GameCanvas.isPointerJustDown = false;
                                instance.isPointerDowning = false;
                                instance.keyTouchSkill = i;
                                if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                                {
                                    GameCanvas.isPointerClick = (GameCanvas.isPointerJustDown = (GameCanvas.isPointerJustRelease = false));
                                    instance.selectedIndexSkill = i;
                                    if (GameScr.indexSelect < 0)
                                        GameScr.indexSelect = 0;
                                    if (!Main.isPC)
                                    {
                                        if (instance.selectedIndexSkill > GameScr.onScreenSkill.Length - 1)
                                            instance.selectedIndexSkill = GameScr.onScreenSkill.Length - 1;
                                    }
                                    else if (instance.selectedIndexSkill > GameScr.keySkill.Length - 1)
                                        instance.selectedIndexSkill = GameScr.keySkill.Length - 1;
                                    Skill skill = Main.isPC ? GameScr.keySkill[instance.selectedIndexSkill] : GameScr.onScreenSkill[instance.selectedIndexSkill];
                                    if (skill != null)
                                        instance.doSelectSkill(skill, true);
                                    break;
                                }
                            }
                        }
                        return true;
                    }
                }
            }
            //ListCharsInMap.updateTouch();
            return false;
        }

        internal static void OnUpdateTouchPanel(Panel instance)
        {
            if (instance.type == CustomPanelMenu.TYPE_CUSTOM_PANEL_MENU)
                instance.updateKeyScrollView();
            SetDo.UpdateTouch(instance);
        }

        internal static void OnSetPointItemMap(int xEnd, int yEnd)
        {
            if (xEnd == Char.myCharz().cx && yEnd == Char.myCharz().cy - 10)
            {
                if (AutoTrainNewAccount.isEnabled)
                    AutoTrainNewAccount.isPicking = false;
                if (AutoTrainPet.Mode > AutoTrainPetMode.Disabled)
                    AutoTrainPet.isPicking = false;
            }
        }

        internal static bool OnMenuStartAt(MyVector menuItems)
        {
            if (AutoTrainNewAccount.isEnabled && menuItems.size() == 2)
            {
                Command command1 = (Command)menuItems.elementAt(0);
                Command command2 = (Command)menuItems.elementAt(1);
                if (command1.caption == "Nhận quà" && command2.caption == "Từ chối")
                {
                    GameCanvas.menu.menuSelectedItem = 0;
                    command1.performAction();
                    AutoTrainNewAccount.isNhapCodeTanThu = true;
                    return true;
                }
            }
            return false;
        }

        internal static void OnAddInfoChar(Char c, string info)
        {
            if (LocalizedString.saoMayLuoiThe.ContainsReversed(info.ToLower()) && AutoTrainPet.Mode > AutoTrainPetMode.Disabled && c.charID == -Char.myCharz().charID)
                AutoTrainPet.saoMayLuoiThe = true;
        }

        internal static bool OnPaintBgGameScr(mGraphics g)
        {
            bool result = false;
            if (GraphicsReducer.Level > ReduceGraphicsLevel.Off || (CustomBackground.isEnabled && CustomBackground.customBgs.Count > 0))
            {
                //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.blackTexture, ScaleMode.StretchToFill);
                g.setColor(0);
                g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
                result = true;
            }
            if (CustomBackground.isEnabled && CustomBackground.customBgs.Count > 0)
            {
                CustomBackground.Paint(g);
                result = true;
            }
            return result;
        }

        internal static void OnMobStartDie(Mob mob)
        {
            Pk9rPickMob.MobStartDie(mob);
        }

        internal static void OnUpdateMob(Mob mob)
        {
            Pk9rPickMob.UpdateCountDieMob(mob);
        }

        internal static bool OnCreateImage(string filename, out Image image)
        {
            string streamingAssetsPath = Application.streamingAssetsPath;
            if (Utils.IsAndroidBuild())
                streamingAssetsPath = Path.Combine(Application.persistentDataPath, "StreamingAssets");
            string customAssetsPath = Path.Combine(streamingAssetsPath, "CustomAssets");
            image = new Image();
            Texture2D texture2D;
            if (!Utils.IsEditor() && !Directory.Exists(customAssetsPath))
                Directory.CreateDirectory(customAssetsPath);
            string filePath = Path.Combine(customAssetsPath, filename.Replace('/', Path.DirectorySeparatorChar) + ".png");
            if (File.Exists(filePath))
            {
                texture2D = new Texture2D(1, 1);
                texture2D.LoadImage(File.ReadAllBytes(filePath));
            }
            else
                texture2D = Resources.Load<Texture2D>(filename);
            if (texture2D == null)
                throw new NullReferenceException(nameof(texture2D));
            image.texture = texture2D;
            image.w = image.texture.width;
            image.h = image.texture.height;
            image.texture.anisoLevel = 0;
            image.texture.filterMode = FilterMode.Point;
            image.texture.mipMapBias = 0f;
            image.texture.wrapMode = TextureWrapMode.Clamp;
            return true;
        }

        internal static void OnChatVip(string chatVip)
        {
            Boss.AddBoss(chatVip);
        }

        internal static bool OnUpdateScrollMousePanel(Panel panel, ref int pXYScrollMouse)
        {
            //if (GameCanvas.pxMouse > instance.X + instance.wScroll || GameCanvas.pxMouse < instance.X)
            //    return true;
            SetDo.UpdateScrollMouse(panel, ref pXYScrollMouse);
            return false;
        }

        internal static void OnPanelHide(Panel instance)
        {

        }

        internal static void OnUpdateKeyPanel(Panel instance)
        {

        }

        internal static void OnUpdateChar(Char ch)
        {
            CharEffectMain.UpdateChar(ch);
        }

        internal static void OnCharRemoveHoldEff(Char ch)
        {
            CharEffectMain.RemoveHold(ch);
        }

        internal static void OnCharSetHoldChar(Char ch, Char r)
        {
            CharEffectMain.AddCharHoldChar(ch, r);
        }

        internal static void OnCharSetHoldMob(Char ch)
        {
            CharEffectMain.AddCharHoldMob(ch);
        }

        internal static bool OnPaintTouchControl(GameScr instance, mGraphics g)
        {
            if (instance.isNotPaintTouchControl())
                return false;
            GameScr.resetTranslate(g);
            if (mScreen.keyTouch == 15 || (Utils.IsPC() && mScreen.keyMouse == 15))
                g.drawImage(Utils.IsPC() ? GameScr.imgChatsPC2 : GameScr.imgChat2, GameScr.xC + 17, GameScr.yC + 17 + mGraphics.addYWhenOpenKeyBoard, mGraphics.HCENTER | mGraphics.VCENTER);
            else
                g.drawImage(Utils.IsPC() ? GameScr.imgChatPC : GameScr.imgChat, GameScr.xC + 17, GameScr.yC + 17 + mGraphics.addYWhenOpenKeyBoard, mGraphics.HCENTER | mGraphics.VCENTER);
            return true;
        }

        internal static bool OnPaintGamePad(mGraphics g)
        {
            GameScr.isHaveSelectSkill = isHaveSelectSkill_old;
            if (GameScr.isAnalog != 0 && Char.myCharz().statusMe != 14)
            {
                g.drawImage(mScreen.keyTouch == 5 ? GameScr.imgFire1 : GameScr.imgFire0, GameScr.xF + 20, GameScr.yF + 20, mGraphics.HCENTER | mGraphics.VCENTER);
                GameScr.gamePad.paint(g);
                g.drawImage(mScreen.keyTouch != 13 ? GameScr.imgFocus : GameScr.imgFocus2, GameScr.xTG + 20, GameScr.yTG + 20, mGraphics.HCENTER | mGraphics.VCENTER);
            }
            return true;
        }

        internal static bool OnPanelFireOption(Panel panel)
        {
            if (panel.selected >= 0)
            {
                switch (panel.selected)
                {
                    case 0:
                        SoundMn.gI().AuraToolOption();
                        break;
                    case 1:
                        SoundMn.gI().AuraToolOption2();
                        break;
                    case 2:
                        SoundMn.gI().chatVipToolOption();
                        break;
                    case 3:
                        SoundMn.gI().soundToolOption();
                        break;
                    case 4:
                        SoundMn.gI().analogToolOption();
                        break;
                    case 5:
                        SoundMn.gI().CaseSizeScr();
                        break;
                    case 6:
                        GameCanvas.startYesNoDlg(mResources.changeSizeScreen, new Command(mResources.YES, panel, 170391, null), new Command(mResources.NO, panel, 4005, null));
                        break;
                }
            }
            return true;
        }

        internal static bool OnSoundMnGetStrOption()
        {
            Panel.strCauhinh = new string[]
            {
                mResources.aura_off?.Trim() + ": " + Strings.OnOffStatus(Char.isPaintAura),
                mResources.aura_off_2?.Trim() + ": " + Strings.OnOffStatus(Char.isPaintAura2),
                mResources.serverchat_off?.Trim() + ": " + Strings.OnOffStatus(GameScr.isPaintChatVip),
                mResources.turnOffSound?.Trim() + ": " + Strings.OnOffStatus(GameCanvas.isPlaySound),
                mResources.analog?.Trim() + ": " + Strings.OnOffStatus(GameScr.isAnalog != 0),
                (GameCanvas.lowGraphic ? mResources.cauhinhcao : mResources.cauhinhthap)?.Trim(),
                mGraphics.zoomLevel <= 1 ? mResources.x2Screen : mResources.x1Screen,
            };
            return true;
        }

        internal static bool OnSetSkillBarPosition()
        {
            Skill[] skills = GameCanvas.isTouch ? GameScr.onScreenSkill : GameScr.keySkill;
            GameScr.xS = new int[skills.Length];
            GameScr.yS = new int[skills.Length];
            if (GameCanvas.isTouchControlSmallScreen && GameScr.isUseTouch)
                GameScr.padSkill = 5;
            else
            {
                GameScr.wSkill = 30;
                if (GameCanvas.w <= 320)
                    GameScr.ySkill = GameScr.gH - GameScr.wSkill - 6;
                else
                    GameScr.wSkill = 40;
            }
            GameScr.xSkill = 17;
            GameScr.ySkill = GameCanvas.h - 40;
            if (GameScr.gamePad.isSmallGamePad && GameScr.isAnalog == 1)
            {
                GameScr.xHP = Math.Min(skills.Length, 5) * GameScr.wSkill;
                GameScr.yHP = GameScr.ySkill;
            }
            else
            {
                GameScr.xHP = GameCanvas.w - 45;
                GameScr.yHP = GameCanvas.h - 45;
            }
            //GameScr.setTouchBtn();
            if (GameScr.isAnalog != 0)
            {
                GameScr.xTG = GameScr.xF = GameCanvas.w - 45;
                if (GameScr.gamePad.isLargeGamePad)
                {
                    //GameScr.xSkill = GameScr.gamePad.wZone + 20;
                    int skillsCountNotNull = skills.Length;
                    for (int i = skills.Length - 1; i >= 0; i--)
                    {
                        if (skills[i] == null)
                            skillsCountNotNull--;
                        else
                            break;
                    }
                    GameScr.wSkill = 35;
                    GameScr.xSkill = Math.Max(GameScr.gamePad.wZone + 20, GameCanvas.hw - skillsCountNotNull * GameScr.wSkill / 2);
                    GameScr.xHP = GameScr.xF - 45;
                }
                else if (GameScr.gamePad.isMediumGamePad)
                    GameScr.xHP = GameScr.xF - 45;
                GameScr.yF = GameCanvas.h - 45;
                GameScr.yTG = GameScr.yF - 45;
            }
            if ((GameCanvas.isTouchControlSmallScreen && GameScr.isUseTouch) || (!GameScr.gamePad.isLargeGamePad && GameScr.isAnalog == 1))
            {
                for (int i = 0; i < GameScr.xS.Length; i++)
                {
                    GameScr.xS[i] = i * GameScr.wSkill;
                    GameScr.yS[i] = GameScr.ySkill;
                    if (GameScr.xS.Length > 5 && i >= GameScr.xS.Length / 2)
                    {
                        GameScr.xS[i] = (i - GameScr.xS.Length / 2) * GameScr.wSkill;
                        GameScr.yS[i] = GameScr.ySkill - 32;
                    }
                }
            }
            else
            {
                int lastJ = 0;
                for (int i = 0; i < GameScr.xS.Length; i++)
                {
                    GameScr.xS[i] = i * GameScr.wSkill;
                    GameScr.yS[i] = GameScr.ySkill;
                    if (lastJ == 0 && GameScr.xSkill + i * GameScr.wSkill > GameScr.xHP - 30)
                        lastJ = i;
                    if (GameScr.xS.Length > 5 && lastJ > 0 && i >= lastJ)
                    {
                        GameScr.xS[i] = (i - lastJ) * GameScr.wSkill;
                        GameScr.yS[i] = GameScr.ySkill - 32;
                    }
                }
            }
            return true;
        }

        internal static bool OnGamepadPaint(GamePad instance, mGraphics g)
        {
            if (GameScr.isAnalog != 0)
            {
                g.drawImage(GameScr.imgAnalog1, instance.xC, instance.yC, mGraphics.HCENTER | mGraphics.VCENTER);
                g.drawImage(GameScr.imgAnalog2, instance.xM, instance.yM, mGraphics.HCENTER | mGraphics.VCENTER);
                return true;
            }
            return false;
        }

        internal static void OnGameScrPaintSelectedSkill(GameScr instance, mGraphics g)
        {
            if (!GameScr.isHaveSelectSkill)
                return;
            isHaveSelectSkill_old = GameScr.isHaveSelectSkill;
            GameScr.isHaveSelectSkill = false;
            Skill[] array;
            if (Main.isPC)
                array = GameScr.keySkill;
            else if (GameCanvas.isTouch)
                array = GameScr.onScreenSkill;
            else
                array = GameScr.keySkill;
            if (!GameCanvas.isTouch)
            {
                g.setColor(0xAA2C11);
                g.fillRect(GameScr.xSkill + GameScr.xHP + 2, GameScr.yHP - 10 + 6, 20, 10);
                mFont.tahoma_7_white.drawString(g, "*", GameScr.xSkill + GameScr.xHP + 12, GameScr.yHP - 8 + 6, mFont.CENTER);
            }
            int num = instance.nSkill;
            if (Main.isPC || !GameCanvas.isTouch)
                num = array.Length;
            string[] array2 = TField.isQwerty ? new string[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" } : new string[5] { "7", "8", "9", "10", "11" };
            bool hasSkillsInTopRow = false;
            bool isStartHasSkill = false;
            for (int i = num - 1; i >= 0; i--)
            {
                if (array[i] != null)
                    isStartHasSkill = true;
                if (isStartHasSkill)
                {
                    if (GameScr.yS[i] == GameScr.ySkill - 32)
                    {
                        hasSkillsInTopRow = true;
                        break;
                    }
                }
            }
            isStartHasSkill = false;

            for (int i = num - 1; i >= 0; i--)
            {
                Skill skill = array[i];
                if (skill != null)
                {
                    isStartHasSkill = true;
                    if (skill != Char.myCharz().myskill)
                        g.drawImage(GameScr.imgSkill, GameScr.xSkill + GameScr.xS[i] - 1, GameScr.yS[i] - 1, 0);
                    else
                        g.drawImage(GameScr.imgSkill2, GameScr.xSkill + GameScr.xS[i] - 1, GameScr.yS[i] - 1, 0);
                }
                else
                {
                    if (isStartHasSkill)
                        g.drawImage(GameScr.imgSkill, GameScr.xSkill + GameScr.xS[i] - 1, GameScr.yS[i] - 1, 0);
                    continue;
                }
                if (Utils.IsPC())
                {
                    int num2 = 27;
                    if (hasSkillsInTopRow)
                    {
                        if (GameScr.yS[i] == GameScr.ySkill - 32)
                            num2 = -13;
                    }
                    else
                        num2 = -13;
                    mFont.tahoma_7b_white.drawString(g, array2[i], GameScr.xSkill + GameScr.xS[i] + 14, GameScr.yS[i] + num2 + 1, mFont.CENTER, mFont.tahoma_7b_dark);
                }
                skill.paint(GameScr.xSkill + GameScr.xS[i] + 13, GameScr.yS[i] + 13, g);
                if ((i == instance.selectedIndexSkill && !instance.isPaintUI() && GameCanvas.gameTick % 10 > 5) || i == instance.keyTouchSkill)
                    g.drawImage(ItemMap.imageFlare, GameScr.xSkill + GameScr.xS[i] + 13, GameScr.yS[i] + 14, 3);
            }
        }

        internal static bool OnPanelPaintToolInfo(mGraphics g)
        {
            mFont.tahoma_7b_white.drawString(g, Strings.communityMod, 60, 4, mFont.LEFT, mFont.tahoma_7b_dark);
            mFont.tahoma_7_yellow.drawString(g, Strings.gameVersion + ": v" + GameMidlet.VERSION, 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_yellow.drawString(g, mResources.character + ": " + Char.myCharz().cName, 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_yellow.drawString(g, mResources.account + " " + mResources.account_server.ToLower() + " " + ServerListScreen.nameServer[ServerListScreen.ipSelect], 60, 39, mFont.LEFT, mFont.tahoma_7_grey);
            return true;
        }

        internal static bool OnSkillPaint(Skill skill, int x, int y, mGraphics g)
        {
            SmallImage.drawSmallImage(g, skill.template.iconId, x, y, 0, StaticObj.VCENTER_HCENTER);
            long coolingDown = mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill;
            if (coolingDown < skill.coolDown)
            {
                float opacity = .6f;
                int realX = x - 11;
                int realY = y - 11;
                Color color = new Color(0, 0, 0, opacity);
                Color color2 = new Color(0, 0, 0, opacity / 2);
                g.setColor(color2);
                g.fillRect(realX, realY, 22, 22);
                float coolDownRatio = 1 - coolingDown / (float)skill.coolDown;
                CustomGraphics.drawCooldownRect(x, y, 22, 22, coolDownRatio, color);
                string cooldownStr = $"{(skill.coolDown - coolingDown) / 1000f:#.0}";
                if (cooldownStr.Length > 4)
                    cooldownStr = cooldownStr.Substring(0, cooldownStr.IndexOf(','));
                cooldownStr = cooldownStr.Replace(',', '.');
                mFont.tahoma_7_yellow.drawString(g, cooldownStr, x + 1, y - 12 + mFont.tahoma_7.getHeight() / 2, mFont.CENTER);
            }
            else
                skill.paintCanNotUseSkill = false;
            return true;
        }

        internal static bool OnGotoPlayer(int id, bool isAutoUseYardrat = true)
        {
            if (isAutoUseYardrat)
            {
                new Thread(delegate ()
                {
                    int previousDisguiseId = -1;
                    if (Char.myCharz().arrItemBody[5] == null || (Char.myCharz().arrItemBody[5] != null && (Char.myCharz().arrItemBody[5].template.id < 592 || Char.myCharz().arrItemBody[5].template.id > 594)))
                    {
                        if (Char.myCharz().arrItemBody[5] != null)
                            previousDisguiseId = Char.myCharz().arrItemBody[5].template.id;
                        for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
                        {
                            Item item = Char.myCharz().arrItemBag[i];
                            if (item != null && item.template.id >= 592 && item.template.id <= 594)
                            {
                                do
                                {
                                    Service.gI().getItem(4, (sbyte)i);
                                    Thread.Sleep(250);
                                }
                                while (Char.myCharz().arrItemBody[5].template.id < 592 || Char.myCharz().arrItemBody[5].template.id > 594);
                                break;
                            }
                        }
                    }
                    GameEventHook.Service_gotoPlayer_original(Service.gI(), id);
                    if (previousDisguiseId != -1)
                    {
                        Thread.Sleep(500);
                        for (int j = 0; j < Char.myCharz().arrItemBag.Length; j++)
                        {
                            Item item = Char.myCharz().arrItemBag[j];
                            if (item != null && item.template.id == previousDisguiseId)
                            {
                                do
                                {
                                    Service.gI().getItem(4, (sbyte)j);
                                    Thread.Sleep(250);
                                }
                                while (Char.myCharz().arrItemBody[5].template.id != previousDisguiseId);
                                break;
                            }
                        }
                    }
                }).Start();
                return true;
            }
            else
                return false;
        }

        internal static bool OnPaintPanel(Panel panel, mGraphics g)
        {
            if (panel.type != CustomPanelMenu.TYPE_CUSTOM_PANEL_MENU)
                return false;
            g.translate(-g.getTranslateX(), -g.getTranslateY());
            g.translate(-panel.cmx, 0);
            g.translate(panel.X, panel.Y);
            GameCanvas.paintz.paintFrameSimple(panel.X, panel.Y, panel.W, panel.H, g);
            g.setClip(panel.X + 1, panel.Y, panel.W - 2, panel.yScroll - 2);
            g.setColor(0x987B55);
            g.fillRect(panel.X, panel.Y, panel.W - 2, 50);
            //panel.paintCharInfo(g, Char.myCharz());
            CustomPanelMenu.PaintTopInfo(panel, g);
            panel.paintBottomMoneyInfo(g);
            if (!CustomPanelMenu.PaintTabHeader(panel, g))
                panel.paintTab(g);
            CustomPanelMenu.Paint(panel, g);
            GameScr.resetTranslate(g);
            panel.paintDetail(g);
            if (panel.cmx == panel.cmtoX)
                panel.cmdClose.paint(g);
            if (panel.tabIcon != null && panel.tabIcon.isShow)
                panel.tabIcon.paint(g);
            return true;
        }

        internal static void OnPaintGameCanvas(GameCanvas instance, mGraphics g)
        {
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.label)
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = (int)(8.5 * mGraphics.zoomLevel),
                };
                style.normal.textColor = style.hover.textColor = Color.yellow;
            }
            if (!GameCanvas.panel.isShow)
            {
                if (GameCanvas.panel2 != null)
                {
                    g.translate(-g.getTranslateX(), -g.getTranslateY());
                    g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
                    if (GameCanvas.panel2.isShow)
                        GameCanvas.panel2.paint(g);
                    if (GameCanvas.panel2.chatTField != null && GameCanvas.panel2.chatTField.isShow)
                        GameCanvas.panel2.chatTField.paint(g);
                }
            }
            g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.6f));
            double fps = Math.Round((double)(1f / Time.smoothDeltaTime * Time.timeScale), 1);
            string fpsStr = fps.ToString("F1").Replace(',', '.');
            g.fillRect(0, 0, mFont.tahoma_7b_red.getWidth(fpsStr) + 2, 12);
            mFont.tahoma_7b_red.drawString(g, fpsStr, 2, 0, 0);
        }

        internal static bool OnUpdatePanel(Panel instance)
        {
            if (instance.type == CustomPanelMenu.TYPE_CUSTOM_PANEL_MENU)
            {
                if ((instance.chatTField == null || !instance.chatTField.isShow) && !instance.isKiguiXu && !instance.isKiguiLuong && (instance.tabIcon == null || !instance.tabIcon.isShow) && instance.waitToPerform > 0)
                {
                    if (instance.waitToPerform - 1 == 0)
                    {
                        instance.waitToPerform--;
                        instance.lastSelect[instance.currentTabIndex] = instance.selected;
                        CustomPanelMenu.DoFire(instance);
                    }
                }
            }
            return false;
        }

        internal static bool OnPanelUpdateKeyInTabBar(Panel instance)
        {
            if (instance.type == CustomPanelMenu.TYPE_CUSTOM_PANEL_MENU)
            {
                if ((instance.scroll != null && instance.scroll.pointerIsDowning) || instance.pointerIsDowning)
                    return true;
                int num = instance.currentTabIndex;
                if (instance.isTabInven() && instance.isnewInventory)
                {
                    if (instance.selected == -1)
                    {
                        if (GameCanvas.keyPressed[6])
                        {
                            instance.currentTabIndex++;
                            if (instance.currentTabIndex >= instance.currentTabName.Length)
                            {
                                if (GameCanvas.panel2 != null)
                                {
                                    instance.currentTabIndex = instance.currentTabName.Length - 1;
                                    GameCanvas.isFocusPanel2 = true;
                                }
                                else
                                    instance.currentTabIndex = 0;
                            }
                            instance.selected = instance.lastSelect[instance.currentTabIndex];
                            instance.lastTabIndex[instance.type] = instance.currentTabIndex;
                        }
                        if (GameCanvas.keyPressed[4])
                        {
                            instance.currentTabIndex--;
                            if (instance.currentTabIndex < 0)
                                instance.currentTabIndex = instance.currentTabName.Length - 1;
                            if (GameCanvas.isFocusPanel2)
                                GameCanvas.isFocusPanel2 = false;
                            instance.selected = instance.lastSelect[instance.currentTabIndex];
                            instance.lastTabIndex[instance.type] = instance.currentTabIndex;
                        }
                    }
                    else if (instance.selected > 0)
                    {
                        if (GameCanvas.keyPressed[8])
                        {
                            if (instance.newSelected == 0)
                                instance.sellectInventory++;
                            else
                                instance.sellectInventory += 5;
                        }
                        else if (GameCanvas.keyPressed[2])
                        {
                            if (instance.newSelected == 0)
                                instance.sellectInventory--;
                            else
                                instance.sellectInventory -= 5;
                        }
                        else if (GameCanvas.keyPressed[4])
                        {
                            if (instance.newSelected == 0)
                                instance.sellectInventory -= 5;
                            else
                                instance.sellectInventory--;
                        }
                        else if (GameCanvas.keyPressed[6])
                        {
                            if (instance.newSelected == 0)
                                instance.sellectInventory += 5;
                            else
                                instance.sellectInventory++;
                        }
                    }
                    if (instance.sellectInventory == instance.nTableItem)
                        instance.sellectInventory = 0;
                }
                else if (!instance.IsTabOption())
                {
                    if (GameCanvas.keyPressed[(!Main.isPC) ? 6 : 24])
                    {
                        if (instance.isTabInven())
                        {
                            if (instance.selected >= 0)
                                instance.updateKeyInvenTab();
                            else
                            {
                                instance.currentTabIndex++;
                                if (instance.currentTabIndex >= instance.currentTabName.Length)
                                {
                                    if (GameCanvas.panel2 != null)
                                    {
                                        instance.currentTabIndex = instance.currentTabName.Length - 1;
                                        GameCanvas.isFocusPanel2 = true;
                                    }
                                    else
                                        instance.currentTabIndex = 0;
                                }
                                instance.selected = instance.lastSelect[instance.currentTabIndex];
                                instance.lastTabIndex[instance.type] = instance.currentTabIndex;
                            }
                        }
                        else
                        {
                            instance.currentTabIndex++;
                            if (instance.currentTabIndex >= instance.currentTabName.Length)
                            {
                                if (GameCanvas.panel2 != null)
                                {
                                    instance.currentTabIndex = instance.currentTabName.Length - 1;
                                    GameCanvas.isFocusPanel2 = true;
                                }
                                else
                                    instance.currentTabIndex = 0;
                            }
                            instance.selected = instance.lastSelect[instance.currentTabIndex];
                            instance.lastTabIndex[instance.type] = instance.currentTabIndex;
                        }
                    }
                    if (GameCanvas.keyPressed[(!Main.isPC) ? 4 : 23])
                    {
                        instance.currentTabIndex--;
                        if (instance.currentTabIndex < 0)
                            instance.currentTabIndex = instance.currentTabName.Length - 1;
                        if (GameCanvas.isFocusPanel2)
                            GameCanvas.isFocusPanel2 = false;
                        instance.selected = instance.lastSelect[instance.currentTabIndex];
                        instance.lastTabIndex[instance.type] = instance.currentTabIndex;
                    }
                }
                instance.keyTouchTab = -1;
                for (int i = 0; i < instance.currentTabName.Length; i++)
                {
                    if (!GameCanvas.isPointer(instance.startTabPos + i * instance.TAB_W, 52, instance.TAB_W - 1, 25))
                        continue;
                    instance.keyTouchTab = i;
                    if (GameCanvas.isPointerJustRelease)
                    {
                        instance.currentTabIndex = i;
                        instance.lastTabIndex[instance.type] = i;
                        GameCanvas.isPointerJustRelease = false;
                        instance.selected = instance.lastSelect[instance.currentTabIndex];
                        if (num == instance.currentTabIndex && instance.cmRun == 0)
                        {
                            instance.cmtoY = 0;
                            instance.selected = GameCanvas.isTouch ? -1 : 0;
                        }
                        break;
                    }
                }
                if (num == instance.currentTabIndex)
                    return true;
                instance.size_tab = 0;
                SoundMn.gI().panelClick();
                CustomPanelMenu.SetTab(instance);
                instance.selected = instance.lastSelect[instance.currentTabIndex];

                return true;
            }
            return false;
        }

        internal static void OnPaintImageBar(mGraphics g, bool isLeft, Char c)
        {
            if (!isLeft)
                return;
            if (c != Char.myCharz())
                return;
            int xHP = 85;
            int xMP = xHP;
            int yHP = 4;
            int yMP = 19;
            string cHP = Utils.FormatWithSIPrefix(Char.myCharz().cHP);
            string cMP = Utils.FormatWithSIPrefix(Char.myCharz().cMP);
            g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.6f));
            if (mGraphics.zoomLevel > 1)
            {
                style.fontSize = (int)(8.5 * mGraphics.zoomLevel);
                g.fillRect(xHP, yHP + 1, Utils.getWidth(style, cHP) + 1, Utils.getHeight(style, cHP) - 2);
                g.drawString(cHP, xHP, yHP, style);
                style.fontSize = 5 * mGraphics.zoomLevel;
                g.fillRect(xMP - 1, yMP + 1, Utils.getWidth(style, cMP) + 1, Utils.getHeight(style, cMP) - 2);
                g.drawString(cMP, xMP, yMP, style);
            }
            else
            {
                g.fillRect(xHP - 1, yHP + 1, mFont.tahoma_7b_yellow.getWidth(cHP), mFont.tahoma_7b_yellow.getHeight() - 2);
                mFont.tahoma_7b_yellow.drawString(g, cHP, xHP, yHP, mFont.LEFT);
                g.fillRect(xMP - 1, yMP + 1, mFont.tahoma_7_yellow.getWidth(cMP), mFont.tahoma_7_yellow.getHeight() - 2);
                mFont.tahoma_7_yellow.drawString(g, cMP, xMP, yMP, mFont.LEFT);
            }
        }

        internal static void OnLoadIP()
        {
            ServerListScreen.getServerList(Strings.DEFAULT_IP_SERVERS);
        }

        internal static void OnAfterPaintPanel(Panel panel, mGraphics g)
        {
            bool GetInventorySelect_isbody(int select, int subSelect, Item[] arrItem) => subSelect == 0 && select - 1 + subSelect * 20 < arrItem.Length;
            int GetInventorySelect_body(int select, int subSelect) => select - 1 + subSelect * 20;
            int GetInventorySelect_bag(int select, int subSelect, Item[] arrItem) => select - 1 + subSelect * 20 - arrItem.Length;
            sbyte GetColor_Item_Upgrade(int lv)
            {
                if (lv < 8)
                    return 0;
                if (lv == 9)
                    return 4;
                if (lv == 10)
                    return 1;
                if (lv == 11)
                    return 5;
                if (lv == 12)
                    return 3;
                if (lv == 13)
                    return 2;
                return 6;
            }
            int GetColor_ItemBg(int id)
            {
                switch (id)
                {
                    case 4:
                        return 1269146;
                    case 1:
                        return 2786816;
                    case 5:
                        return 13279744;
                    case 3:
                        return 12537346;
                    case 2:
                        return 7078041;
                    case 6:
                        return 11599872;
                    default:
                        return -1;
                }
            }

            if (GameCanvas.panel.combineSuccess != -1)
                return;
            g.translate(-(panel.cmx - panel.cmtoX), -panel.cmy);
            if (panel.type == 13)   //trade
            {
                bool? isMe = null;
                if (panel.currentTabIndex == 0 && panel != GameCanvas.panel)
                    isMe = false;
                if (panel.currentTabIndex == 2)
                    isMe = false;
                if (panel.currentTabIndex == 1)
                    isMe = true;
                if (isMe != null)
                {
                    MyVector myVector = isMe.Value ? panel.vMyGD : panel.vFriendGD;
                    if (myVector.size() <= 0)
                        return;
                    int offset = Math.Max(panel.cmy / panel.ITEM_HEIGHT, 0);
                    for (int i = offset; i < Mathf.Clamp(offset + panel.hScroll / panel.ITEM_HEIGHT + 2, 0, myVector.size()); i++)
                    {
                        Item item = (Item)myVector.elementAt(i);
                        if (item == null)
                            continue;
                        int y = panel.yScroll + i * panel.ITEM_HEIGHT;
                        if (item.itemOption != null)
                        {
                            ItemOption itemOption = item.GetBestItemOption();
                            if (itemOption == null)
                                goto Label;
                            int param = itemOption.param;
                            int id = itemOption.optionTemplate.id;
                            if (param > 7 || (id >= 127 && id <= 135))
                                param = 7;
                            if (id == 107)
                            {
                                if (param > 1)
                                    param = (int)Math.Ceiling((double)param / 2);
                                else if (param == 1)
                                    goto Label;
                            }
                            if (param <= 0)
                                goto Label;
                            g.setColor(i == panel.selected ? 0x919600 : 0x987B55);
                            for (int j = 0; j < item.itemOption.Length; j++)
                            {
                                if (item.itemOption[j].optionTemplate.id == 72 && item.itemOption[j].param > 0)
                                {
                                    byte id_ = (byte)GetColor_Item_Upgrade(item.itemOption[j].param);
                                    if (GetColor_ItemBg(id_) != -1)
                                        g.setColor(GetColor_ItemBg(id_));
                                }
                            }
                            g.fillRect(panel.xScroll, y, 34, panel.ITEM_HEIGHT - 1);
                            CustomGraphics.PaintItemEffectInPanel(g, panel.xScroll + 17, y + 11, 34, panel.ITEM_HEIGHT - 1, item);
                            SmallImage.drawSmallImage(g, item.template.iconID, panel.xScroll + 34 / 2, panel.yScroll + i * panel.ITEM_HEIGHT + (panel.ITEM_HEIGHT - 1) / 2, 0, 3);
                        }
                    Label:;
                        CustomGraphics.PaintItemOptions(g, panel, item, y);
                    }
                }
            }
            else if (panel.type == 1 || panel.type == 17)   //shop
            {
                if (panel.type == 1 && panel.currentTabIndex == panel.currentTabName.Length - 1 && GameCanvas.panel2 == null && panel.typeShop != 2)
                    return;
                if (panel.typeShop == 2 && panel == GameCanvas.panel)
                {
                    if (Char.myCharz().arrItemShop[panel.currentTabIndex].Length == 0 && panel.type != 17)
                        return;
                }
                Item[] array = Char.myCharz().arrItemShop[panel.currentTabIndex];
                if (panel.typeShop == 2 && (panel.currentTabIndex == 4 || panel.type == 17))
                {
                    array = Char.myCharz().arrItemShop[4];
                    if (array.Length == 0)
                        return;
                }
                for (int i = 0; i < array.Length; i++)
                {
                    int y = panel.yScroll + i * panel.ITEM_HEIGHT;
                    if (y - panel.cmy > panel.yScroll + panel.hScroll || y - panel.cmy < panel.yScroll - panel.ITEM_HEIGHT)
                        continue;
                    Item item = array[i];
                    if (item == null)
                        continue;
                    if (item.itemOption != null)
                    {
                        ItemOption itemOption = item.GetBestItemOption();
                        if (itemOption == null)
                            goto Label;
                        int param = itemOption.param;
                        int id = itemOption.optionTemplate.id;
                        if (param > 7 || (id >= 127 && id <= 135))
                            param = 7;
                        if (id == 107)
                        {
                            if (param > 1)
                                param = (int)Math.Ceiling((double)param / 2);
                            else if (param == 1)
                                goto Label;
                        }
                        if (param <= 0)
                            goto Label;
                        g.setColor(i == panel.selected ? 0x919600 : 0x987B55);
                        for (int j = 0; j < item.itemOption.Length; j++)
                        {
                            if (item.itemOption[j].optionTemplate.id == 72 && item.itemOption[j].param > 0)
                            {
                                byte id_ = (byte)GetColor_Item_Upgrade(item.itemOption[j].param);
                                if (GetColor_ItemBg(id_) != -1)
                                    g.setColor(GetColor_ItemBg(id_));
                            }
                        }
                        g.fillRect(panel.xScroll, y, 24, panel.ITEM_HEIGHT - 1);
                        CustomGraphics.PaintItemEffectInPanel(g, panel.xScroll + 12, y + 11, 24, panel.ITEM_HEIGHT - 1, item);
                        SmallImage.drawSmallImage(g, item.template.iconID, panel.xScroll + 24 / 2, panel.yScroll + i * panel.ITEM_HEIGHT + (panel.ITEM_HEIGHT - 1) / 2, 0, 3);
                    }
                Label:;
                    if (panel.type == Panel.TYPE_KIGUI)
                        CustomGraphics.PaintItemOptions(g, panel, item, y + mFont.tahoma_7b_blue.getHeight() + 2);
                    else if (panel.type == Panel.TYPE_SHOP)
                    {
                        if (!string.IsNullOrEmpty(item.nameNguoiKyGui))
                        {
                            if (GameCanvas.gameTick % 120 > 60 && (Utils.HasStarOption(item, out _, out _) || Utils.HasActivateOption(item)))
                            {
                                int w = mFont.tahoma_7b_green.getWidth(item.nameNguoiKyGui) + 5;
                                g.setColor(i != panel.selected ? 0xE7DFD2 : 0xF9FF4A);
                                g.fillRect(panel.X + Panel.WIDTH_PANEL - 2 - w, y + mFont.tahoma_7b_blue.getHeight() + 2, w, mFont.tahoma_7b_green.getHeight());
                                CustomGraphics.PaintItemOptions(g, panel, item, y + mFont.tahoma_7b_blue.getHeight() + 2);
                            }
                        }
                        else
                            CustomGraphics.PaintItemOptions(g, panel, item, y + mFont.tahoma_7b_blue.getHeight() + 2);
                    }
                    else
                        CustomGraphics.PaintItemOptions(g, panel, item, y);
                }
            }
            else if (panel.type == 21 && panel.currentTabIndex == 0)    //pet inventory
            {
                Item[] arrItemBody = Char.myPetz().arrItemBody;
                for (int i = 0; i < arrItemBody.Length; i++)
                {
                    int y = panel.yScroll + i * panel.ITEM_HEIGHT;
                    if (y - panel.cmy > panel.yScroll + panel.hScroll || y - panel.cmy < panel.yScroll - panel.ITEM_HEIGHT)
                        continue;
                    Item item = arrItemBody[i];
                    if (item == null)
                        continue;
                    if (item.itemOption != null)
                    {
                        ItemOption itemOption = item.GetBestItemOption();
                        if (itemOption == null)
                            goto Label;
                        int param = itemOption.param;
                        int id = itemOption.optionTemplate.id;
                        if (param > 7 || (id >= 127 && id <= 135))
                            param = 7;
                        if (id == 107)
                        {
                            if (param > 1)
                                param = (int)Math.Ceiling((double)param / 2);
                            else if (param == 1)
                                goto Label;
                        }
                        if (param <= 0)
                            goto Label;
                        g.setColor(i == panel.selected ? 0x919600 : 0x987B55);
                        for (int j = 0; j < item.itemOption.Length; j++)
                        {
                            if (item.itemOption[j].optionTemplate.id == 72 && item.itemOption[j].param > 0)
                            {
                                byte id_ = (byte)GetColor_Item_Upgrade(item.itemOption[j].param);
                                if (GetColor_ItemBg(id_) != -1)
                                    g.setColor(GetColor_ItemBg(id_));
                            }
                        }
                        g.fillRect(panel.xScroll, y, 34, panel.ITEM_HEIGHT - 1);
                        CustomGraphics.PaintItemEffectInPanel(g, panel.xScroll + 17, y + 14, 34, panel.ITEM_HEIGHT - 1, item);
                        SmallImage.drawSmallImage(g, item.template.iconID, panel.xScroll + 34 / 2, panel.yScroll + i * panel.ITEM_HEIGHT + (panel.ITEM_HEIGHT - 1) / 2, 0, 3);
                    }
                Label:;
                    CustomGraphics.PaintItemOptions(g, panel, item, y);
                }
            }
            else if (panel.type == 2 && panel.currentTabIndex == 0) //box
            {
                Item[] arrItemBox = Char.myCharz().arrItemBox;
                int offset = Math.Max(panel.cmy / panel.ITEM_HEIGHT, 0);
                for (int i = offset; i < Mathf.Clamp(offset + panel.hScroll / panel.ITEM_HEIGHT + 2, 0, arrItemBox.Length); i++)
                {
                    int y = panel.yScroll + (i + 1) * panel.ITEM_HEIGHT;
                    if (y - panel.cmy > panel.yScroll + panel.hScroll || y - panel.cmy < panel.yScroll - panel.ITEM_HEIGHT)
                        continue;
                    if (i == 0)
                        continue;
                    Item item = arrItemBox[i];
                    if (item == null)
                        continue;
                    if (item.itemOption != null)
                    {
                        ItemOption itemOption = item.GetBestItemOption();
                        if (itemOption == null)
                            goto Label;
                        int param = itemOption.param;
                        int id = itemOption.optionTemplate.id;
                        if (param > 7 || (id >= 127 && id <= 135))
                            param = 7;
                        if (id == 107)
                        {
                            if (param > 1)
                                param = (int)Math.Ceiling((double)param / 2);
                            else if (param == 1)
                                goto Label;
                        }
                        if (param <= 0)
                            goto Label;
                        g.setColor(i == panel.selected ? 0x919600 : 0x987B55);
                        for (int j = 0; j < item.itemOption.Length; j++)
                        {
                            if (item.itemOption[j].optionTemplate.id == 72 && item.itemOption[j].param > 0)
                            {
                                byte id_ = (byte)GetColor_Item_Upgrade(item.itemOption[j].param);
                                if (GetColor_ItemBg(id_) != -1)
                                    g.setColor(GetColor_ItemBg(id_));
                            }
                        }
                        g.fillRect(panel.xScroll, y, 34, panel.ITEM_HEIGHT - 1);
                        CustomGraphics.PaintItemEffectInPanel(g, panel.xScroll + 17, y + 11, 34, panel.ITEM_HEIGHT - 1, item);
                        SmallImage.drawSmallImage(g, item.template.iconID, panel.xScroll + 34 / 2, y + (panel.ITEM_HEIGHT - 1) / 2, 0, 3);
                    }
                Label:;
                    CustomGraphics.PaintItemOptions(g, panel, item, y);
                }
            }
            else if (panel.type == 12 && panel.currentTabIndex == 0)    //combine
            {
                if (panel.vItemCombine.size() == 0)
                    return;
                int offset = Math.Max(panel.cmy / panel.ITEM_HEIGHT, 0);
                for (int i = offset; i < Mathf.Clamp(offset + panel.hScroll / panel.ITEM_HEIGHT + 2, 0, panel.vItemCombine.size() + 1); i++)
                {
                    int y = panel.yScroll + i * panel.ITEM_HEIGHT;
                    if (y - panel.cmy > panel.yScroll + panel.hScroll || y - panel.cmy < panel.yScroll - panel.ITEM_HEIGHT)
                        continue;
                    if (i == panel.vItemCombine.size())
                        continue;
                    Item item = (Item)panel.vItemCombine.elementAt(i);
                    if (item == null)
                        continue;
                    if (item.itemOption != null)
                    {
                        ItemOption itemOption = item.GetBestItemOption();
                        if (itemOption == null)
                            goto Label;
                        int param = itemOption.param;
                        int id = itemOption.optionTemplate.id;
                        if (param > 7 || (id >= 127 && id <= 135))
                            param = 7;
                        if (id == 107)
                        {
                            if (param > 1)
                                param = (int)Math.Ceiling((double)param / 2);
                            else if (param == 1)
                                goto Label;
                        }
                        if (param <= 0)
                            goto Label;
                        g.setColor(i == panel.selected ? 0x919600 : 0x987B55);
                        for (int j = 0; j < item.itemOption.Length; j++)
                        {
                            if (item.itemOption[j].optionTemplate.id == 72 && item.itemOption[j].param > 0)
                            {
                                byte id_ = (byte)GetColor_Item_Upgrade(item.itemOption[j].param);
                                if (GetColor_ItemBg(id_) != -1)
                                    g.setColor(GetColor_ItemBg(id_));
                            }
                        }
                        g.fillRect(panel.xScroll, y, 34, panel.ITEM_HEIGHT - 1);
                        CustomGraphics.PaintItemEffectInPanel(g, panel.xScroll + 17, y + 11, 34, panel.ITEM_HEIGHT - 1, item);
                        SmallImage.drawSmallImage(g, item.template.iconID, panel.xScroll + 34 / 2, panel.yScroll + i * panel.ITEM_HEIGHT + (panel.ITEM_HEIGHT - 1) / 2, 0, 3);
                    }
                Label:;
                    CustomGraphics.PaintItemOptions(g, panel, item, y);
                }
            }
            else if ((panel.type == 21 && panel.currentTabIndex == 2) ||
                (panel.type == 0 && panel.currentTabIndex == 1) ||
                (panel.type == 2 && panel.currentTabIndex == 1) ||
                panel.type == 7 ||
                (panel.type == 12 && panel.currentTabIndex == 1) ||
                (panel.type == 13 && panel.currentTabIndex == 0 && panel == GameCanvas.panel) ||
                (panel.type == 1 && panel.currentTabIndex == panel.currentTabName.Length - 1 && GameCanvas.panel2 == null && panel.typeShop != 2))  //my inventory
            {
                Item[] arrItemBody = Char.myCharz().arrItemBody;
                Item[] arrItemBag = Char.myCharz().arrItemBag;
                int offset = Math.Max(panel.cmy / panel.ITEM_HEIGHT, 1);
                for (int i = offset; i < Mathf.Clamp(offset + (panel.hScroll - 21) / panel.ITEM_HEIGHT + 2, 0, panel.currentListLength); i++)
                {
                    int y = panel.yScroll + i * panel.ITEM_HEIGHT;
                    if (y - panel.cmy > panel.yScroll + panel.hScroll || y - panel.cmy < panel.yScroll - panel.ITEM_HEIGHT)
                        continue;
                    bool inventorySelect_isbody = GetInventorySelect_isbody(i, panel.newSelected, arrItemBody);
                    int inventorySelect_body = GetInventorySelect_body(i, panel.newSelected);
                    int inventorySelect_bag = GetInventorySelect_bag(i, panel.newSelected, arrItemBody);
                    Item item = (!inventorySelect_isbody) ? arrItemBag[inventorySelect_bag] : arrItemBody[inventorySelect_body];
                    if (item == null)
                        continue;
                    if (item.itemOption != null)
                    {
                        ItemOption itemOption = item.GetBestItemOption();
                        if (itemOption == null)
                            goto Label;
                        int param = itemOption.param;
                        int id = itemOption.optionTemplate.id;
                        if (param > 7 || (id >= 127 && id <= 135))
                            param = 7;
                        if (id == 107)
                        {
                            if (param > 1)
                                param = (int)Math.Ceiling((double)param / 2);
                            else if (param == 1)
                                goto Label;
                        }
                        if (param <= 0)
                            goto Label;
                        if (i == panel.selected)
                            g.setColor(0x919600);
                        else if (inventorySelect_isbody)
                            g.setColor(0x987B55);
                        else
                            g.setColor(0xB49F84);
                        for (int j = 0; j < item.itemOption.Length; j++)
                        {
                            if (item.itemOption[j].optionTemplate.id == 72 && item.itemOption[j].param > 0)
                            {
                                byte id_ = (byte)GetColor_Item_Upgrade(item.itemOption[j].param);
                                if (GetColor_ItemBg(id_) != -1)
                                    g.setColor(GetColor_ItemBg(id_));
                            }
                        }
                        g.fillRect(panel.xScroll, y, 34, panel.ITEM_HEIGHT - 1);
                        CustomGraphics.PaintItemEffectInPanel(g, panel.xScroll + 17 + (panel == GameCanvas.panel2 ? 2 : 0), y + 11, 34, panel.ITEM_HEIGHT - 1, item);
                        SmallImage.drawSmallImage(g, item.template.iconID, panel.xScroll + 34 / 2, panel.yScroll + i * panel.ITEM_HEIGHT + (panel.ITEM_HEIGHT - 1) / 2, 0, 3);
                    }
                Label:;
                    CustomGraphics.PaintItemOptions(g, panel, item, y);
                }
            }
        }
    }
}
