using System;
using System.IO;
using System.Linq;
using Mod.Graphics;
using Mod.R;
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

        static bool isRegistered = false;

        /// <summary>
        /// Kích hoạt khi người chơi chat.
        /// </summary>
        /// <param name="text">Nội dung chat.</param>
        /// <returns></returns>
        internal static bool onSendChat(string text)
        {
            bool result = true;
            if (text == "test")
                GameScr.info1.addInfo("Test OK", 0);
            else
                result = false;
            return result;
        }

        /// <summary>
        /// Kích hoạt sau khi game khởi động.
        /// </summary>
        internal static bool onGameStarted()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
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
            onCheckZoomLevel(Screen.width, Screen.height);
            GameEventHook.InstallAll();
            UIImage.OnStart();
            //TestHook.Install();
            if (onSetResolution())
                return true;
            return false;
        }

        /// <summary>
        /// Kích hoạt khi game đóng
        /// </summary>
        internal static void onGameClosing()
        {

        }

        internal static void onSaveRMSString(ref string filename, ref string data)
        {

        }


        internal static void onLoadLanguage(sbyte newLanguage)
        {
            Strings.LoadLanguage(newLanguage);
        }

        /// <summary>
        /// Kích hoạt khi cài đăt kích thước màn hình.
        /// </summary>
        /// <returns></returns>
        internal static bool onSetResolution()
        {
            return Utils.IsAndroidBuild();
        }

        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr) chưa được xử lý.
        /// </summary>
        internal static void onGameScrPressHotkeysUnassigned()
        {

        }

        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr).
        /// </summary>
        internal static void onGameScrPressHotkeys()
        {

        }

        /// <summary>
        /// Kích hoạt sau khi vẽ khung chat.
        /// </summary>
        internal static void onPaintChatTextField(ChatTextField instance, mGraphics g)
        {

        }

        /// <summary>
        /// Kích hoạt khi mở khung chat.
        /// </summary>
        internal static bool onStartChatTextField(ChatTextField sender, IChatable parentScreen)
        {
            return false;
        }

        internal static bool onGetRMSPath(out string result)
        {
            //result = $"{Application.persistentDataPath}\\{GameMidlet.IP}_{GameMidlet.PORT}_x{mGraphics.zoomLevel}\\";
            string subFolder = "TeaMobi";
            // check ip server lậu, lưu rms riêng
            // ...
            result = Application.persistentDataPath;
            if (Utils.IsLinuxBuild())
                result = Path.Combine(Application.persistentDataPath, Application.companyName, Application.productName);
            if (Utils.IsAndroidBuild())
                result = Application.persistentDataPath;
            result = Path.Combine(result, subFolder);
            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);
            return true;
        }

        internal static bool onTeleportUpdate(Teleport teleport)
        {
            return false;
        }

        /// <summary>
        /// Kích hoạt khi có ChatTextField update.
        /// </summary>
        internal static void onUpdateChatTextField(ChatTextField sender)
        {

        }

        internal static bool onClearAllRMS()
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
            foreach (DirectoryInfo directory in new DirectoryInfo(Rms.GetiPhoneDocumentsPath() + "/").EnumerateDirectories())
            {
                try
                {
                    directory.Delete(true);
                }
                catch { }
            }
            return true;
        }

        /// <summary>
        /// Kích hoạt khi GameScr.gI() update.
        /// </summary>
        internal static void onUpdateGameScr()
        {

        }

        /// <summary>
        /// Kích hoạt khi gửi yêu cầu đăng nhập.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <param name="type"></param>
        internal static void onLogin(ref string username, ref string pass, ref sbyte type)
        {

        }

        /// <summary>
        /// Kích hoạt sau khi màn hình chọn server được load.
        /// </summary>
        internal static void onServerListScreenLoaded()
        {
            isRegistered = !string.IsNullOrEmpty(Rms.loadRMSString("acc"));
        }

        /// <summary>
        /// Kích hoạt khi Session kết nối đến server.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        internal static void onSessionConnecting(ref string host, ref int port)
        {

        }

        internal static void onScreenDownloadDataShow()
        {

        }

        internal static bool onCheckZoomLevel(int w, int h)
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

        internal static bool onKeyPressedz(int keyCode, bool isFromSync)
        {
            return false;
        }

        internal static bool onKeyReleasedz(int keyCode, bool isFromAsync)
        {
            return false;
        }

        internal static bool onChatPopupMultiLine(string chat)
        {
            return false;
        }

        internal static bool onAddBigMessage(string chat, Npc npc)
        {
            return false;
        }

        internal static void onInfoMapLoaded()
        {

        }

        internal static void onPaintGameScr(mGraphics g)
        {
            //g.setColor(0xff0000);
            //Skill[] skills = Main.isPC ? GameScr.keySkill : GameScr.onScreenSkill;
            //int xSMax = int.MinValue;
            //int xSMin = int.MaxValue;
            //int ySMax = int.MinValue;
            //int ySMin = int.MaxValue;
            //for (int i = skills.Length - 1; i >= 0; i--)
            //{
            //    if (skills[i] != null)
            //    {
            //        xSMax = Math.Max(GameScr.xS[i], xSMax);
            //        xSMin = Math.Min(GameScr.xS[i], xSMin);
            //        ySMax = Math.Max(GameScr.yS[i], ySMax);
            //        ySMin = Math.Min(GameScr.yS[i], ySMin);
            //    }
            //}
            //g.drawRect(GameScr.xSkill, ySMin, xSMax - xSMin + GameScr.wSkill, ySMax - ySMin + GameScr.wSkill);
        }

        internal static bool onUseSkill(Char ch)
        {
            return false;
        }

        internal static void onFixedUpdateMain()
        {

        }

        internal static void onUpdateMain()
        {
            if (_previousWidth != Screen.width || _previousHeight != Screen.height)
            {
                _previousWidth = Screen.width;
                _previousHeight = Screen.height;
                ScaleGUI.initScaleGUI();
                GameCanvas.instance?.resetSize();
                ChatTextField.gI().ResetTextField();
                GameScr.gamePad?.SetGamePadZone();
                GameScr.loadCamera(false, -1, -1);
            }
        }

        internal static void onAddInfoMe(string str)
        {

        }

        internal static bool onUpdateTouchGameScr(GameScr instance)
        {
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
                    if (GameCanvas.isPointerHoldIn(GameScr.xSkill, ySMin, xSMax - xSMin + GameScr.wSkill, ySMax - ySMin + GameScr.wSkill))
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
            return false;
        }

        internal static void onUpdateTouchPanel()
        {

        }

        internal static void onSetPointItemMap(int xEnd, int yEnd)
        {

        }

        internal static bool onMenuStartAt(MyVector menuItems)
        {
            return false;
        }

        internal static void onAddInfoChar(Char c, string info)
        {

        }

        internal static bool onPaintBgGameScr(mGraphics g)
        {
            return false;
        }

        internal static void onMobStartDie(Mob instance)
        {

        }

        internal static void onUpdateMob(Mob instance)
        {

        }

        internal static bool onCreateImage(string filename, out Image image)
        {
            string streamingAssetsPath = Application.streamingAssetsPath;
            if (Utils.IsAndroidBuild())
                streamingAssetsPath = Path.Combine(Application.persistentDataPath, "StreamingAssets");
            string customAssetsPath = Path.Combine(streamingAssetsPath, "CustomAssets");
            image = new Image();
            Texture2D texture2D;
            if (!Utils.IsEditor() && !Directory.Exists(customAssetsPath))
                Directory.CreateDirectory(customAssetsPath);
            if (File.Exists(Path.Combine(customAssetsPath, filename.Replace('/', '\\') + ".png")))
            {
                texture2D = new Texture2D(1, 1);
                texture2D.LoadImage(File.ReadAllBytes(Path.Combine(customAssetsPath, filename.Replace('/', '\\') + ".png")));
            }
            else
                texture2D = Resources.Load<Texture2D>(filename);
            if (texture2D == null)
                throw new Exception("NULL POINTER EXCEPTION AT Image __createImage " + filename);
            image.texture = texture2D;
            image.w = image.texture.width;
            image.h = image.texture.height;
            image.texture.anisoLevel = 0;
            image.texture.filterMode = FilterMode.Point;
            image.texture.mipMapBias = 0f;
            image.texture.wrapMode = TextureWrapMode.Clamp;
            return true;
        }

        internal static void onChatVip(string chatVip)
        {

        }

        internal static void onUpdateScrollMousePanel(Panel instance, ref int pXYScrollMouse)
        {

        }

        internal static void onPanelHide(Panel instance)
        {

        }

        internal static void onUpdateKeyPanel(Panel instance)
        {

        }

        internal static bool onPaintTouchControl(GameScr instance, mGraphics g)
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

        internal static bool onPaintGamePad(mGraphics g)
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

        internal static bool onPanelFireOption(Panel panel)
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

        internal static bool onSoundMnGetStrOption()
        {
            static string status(bool value) => value ? mResources.ON : mResources.OFF;
            //string on = "[x]   ";
            //string off = "[  ]   ";
            Panel.strCauhinh = new string[]
            {
                //Char.isPaintAura ? (off + mResources.aura_off.Trim()) : (on + mResources.aura_off.Trim()),
                //Char.isPaintAura2 ? (off + mResources.aura_off_2.Trim()) : (on + mResources.aura_off_2.Trim()),
                //GameCanvas.isPlaySound ? (on + mResources.turnOffSound.Trim()) : (off + mResources.turnOffSound.Trim()),
                //GameScr.isAnalog == 0 ? (off + mResources.turnOnAnalog) : (on + mResources.turnOffAnalog),
                //GameCanvas.lowGraphic ? (on + mResources.cauhinhcao?.Trim()) : (off + mResources.cauhinhthap?.Trim()),
                //(mGraphics.zoomLevel <= 1) ? (off + mResources.x2Screen) : (on + mResources.x1Screen)
                mResources.aura_off?.Trim() + ": " + status(Char.isPaintAura),
                mResources.aura_off_2?.Trim() + ": " + status(Char.isPaintAura2),
                mResources.serverchat_off?.Trim() + ": " + status(GameScr.isPaintChatVip),
                mResources.turnOffSound?.Trim() + ": " + status(GameCanvas.isPlaySound),
                mResources.turnOnAnalog?.Trim() + ": " + status(GameScr.isAnalog != 0),
                (GameCanvas.lowGraphic ? mResources.cauhinhcao : mResources.cauhinhthap)?.Trim(),
                mGraphics.zoomLevel <= 1 ? mResources.x2Screen : mResources.x1Screen,
            };
            return true;
        }

        internal static bool onSetSkillBarPosition()
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

        internal static bool onGamepadPaint(GamePad instance, mGraphics g)
        {
            if (GameScr.isAnalog != 0)
            {
                g.drawImage(GameScr.imgAnalog1, instance.xC, instance.yC, mGraphics.HCENTER | mGraphics.VCENTER);
                g.drawImage(GameScr.imgAnalog2, instance.xM, instance.yM, mGraphics.HCENTER | mGraphics.VCENTER);
                return true;
            }
            return false;
        }

        internal static void onGameScrPaintSelectedSkill(GameScr instance, mGraphics g)
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
                g.setColor(11152401);
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

        internal static bool onPanelPaintToolInfo(mGraphics g)
        {
            mFont.tahoma_7b_white.drawString(g, Strings.communityMod + " " + GameMidlet.VERSION, 60, 4, mFont.LEFT, mFont.tahoma_7b_dark);
            mFont.tahoma_7_yellow.drawString(g, mResources.character + ": " + Char.myCharz().cName, 60, 16, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_yellow.drawString(g, mResources.account + " " + mResources.account_server.ToLower() + " " + ServerListScreen.nameServer[ServerListScreen.ipSelect], 60, 27, mFont.LEFT, mFont.tahoma_7_grey);
            mFont.tahoma_7_yellow.drawString(g, isRegistered ? Strings.registered : mResources.not_register_yet, 60, 39, mFont.LEFT, mFont.tahoma_7_grey);
            return true;
        }

        internal static bool onSkillPaint(Skill skill, int x, int y, mGraphics g)
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
    }
}
