using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using MonoHook;
using UnityEngine;
using UnityEngine.Scripting;

[assembly: Preserve]
namespace Mod
{
    /// <summary>
    /// Hook vào các hàm của game để gọi các hàm trong <see cref="GameEvents"/>.
    /// </summary>
    public static class GameEventHook
    {
        static List<MethodHook> _hooks = new List<MethodHook>();
        static HookObj _ = new HookObj();

        /// <summary>
        /// Cài đặt tất cả hook.
        /// </summary>
        public static void InstallAll()
        {
            InstallHook<Action<string>, Action<Service, string>, Action<Service, string>>(Service.gI().chat, Service_chat_hook, Service_chat_original);
            InstallHook<Action<string, string>, Action<string, string>, Action<string, string>>(Rms.saveRMSString, Rms_saveRMSString_hook, Rms_saveRMSString_original);
            InstallHook<Action, Action<GameScr>, Action<GameScr>>(new GameScr(_).updateKey, GameScr_updateKey_hook, GameScr_updateKey_original);
            InstallHook<Action<mGraphics>, Action<ChatTextField, mGraphics>, Action<ChatTextField, mGraphics>>(new ChatTextField(_).paint, ChatTextField_paint_hook, ChatTextField_paint_original);
            InstallHook<Action<int, IChatable, string>, Action<ChatTextField, int, IChatable, string>, Action<ChatTextField, int, IChatable, string>>(new ChatTextField(_).startChat, ChatTextField_startChat_hook_1, ChatTextField_startChat_original_1);
            InstallHook<Action<IChatable, string>, Action<ChatTextField, IChatable, string>, Action<ChatTextField, IChatable, string>>(new ChatTextField(_).startChat, ChatTextField_startChat_hook_2, ChatTextField_startChat_original_2);
            InstallHook<Func<string, int>, Func<string, int>, Func<string, int>>(Rms.loadRMSInt, Rms_loadRMSInt_hook, Rms_loadRMSInt_original);
            InstallHook<Func<string>, Func<string>, Func<string>>(Rms.GetiPhoneDocumentsPath, Rms_GetiPhoneDocumentsPath_hook, Rms_GetiPhoneDocumentsPath_original);
            InstallHook<Action, Action<Teleport>, Action<Teleport>>(new Teleport(_).update, Teleport_update_hook, Teleport_update_original);
            InstallHook<Action, Action<ChatTextField>, Action<ChatTextField>>(new ChatTextField(_).update, ChatTextField_update_hook, ChatTextField_update_original);
            InstallHook<Action, Action, Action>(Rms.clearAll, Rms_clearAll_hook, Rms_clearAll_original);
            InstallHook<Action, Action<GameScr>, Action<GameScr>>(new GameScr(_).update, GameScr_update_hook, GameScr_update_original);
            InstallHook<Action<string, string, string, sbyte>, Action<Service, string, string, string, sbyte>, Action<Service, string, string, string, sbyte>>(Service.gI().login, Service_login_hook, Service_login_original);
            InstallHook<Action, Action<ServerListScreen>, Action<ServerListScreen>>(new ServerListScreen(_).switchToMe, ServerListScreen_switchToMe_hook, ServerListScreen_switchToMe_original);
            InstallHook<Action<string, int>, Action<Session_ME, string, int>, Action<Session_ME, string, int>>(Session_ME.gI().connect, Session_ME_connect_hook, Session_ME_connect_original);
            InstallHook<Action, Action<ServerListScreen>, Action<ServerListScreen>>(new ServerListScreen(_).show2, ServerListScreen_show2_hook, ServerListScreen_show2_original);
            InstallHook<Action<int, int>, Action<MotherCanvas, int, int>, Action<MotherCanvas, int, int>>(new MotherCanvas(_).checkZoomLevel, MotherCanvas_checkZoomLevel_hook, MotherCanvas_checkZoomLevel_original);
            InstallHook<Action<int>, Action<GameCanvas, int>, Action<GameCanvas, int>>(new GameCanvas(_).keyPressedz, GameCanvas_keyPressedz_hook, GameCanvas_keyPressedz_original);
            InstallHook<Action<int>, Action<GameCanvas, int>, Action<GameCanvas, int>>(new GameCanvas(_).keyReleasedz, GameCanvas_keyReleasedz_hook, GameCanvas_keyReleasedz_original);
            InstallHook<Action<string, int, Npc>, Action<string, int, Npc>, Action<string, int, Npc>>(ChatPopup.addChatPopupMultiLine, ChatPopup_addChatPopupMultiLine_hook, ChatPopup_addChatPopupMultiLine_original);
            InstallHook<Action<string, int, Npc>, Action<string, int, Npc>, Action<string, int, Npc>>(ChatPopup.addBigMessage, ChatPopup_addBigMessage_hook, ChatPopup_addBigMessage_original);
            InstallHook<Action<Message>, Action<Controller, Message>, Action<Controller, Message>>(Controller.gI().loadInfoMap, Controller_loadInfoMap_hook, Controller_loadInfoMap_original);
            InstallHook<Action<mGraphics>, Action<GameScr, mGraphics>, Action<GameScr, mGraphics>>(new GameScr(_).paint, GameScr_paint_hook, GameScr_paint_original);
            InstallHook<Action<SkillPaint, int>, Action<Char, SkillPaint, int>, Action<Char, SkillPaint, int>>(new Char().setSkillPaint, Char_setSkillPaint_hook, Char_setSkillPaint_original);
            InstallHook<Action<string, int>, Action<InfoMe, string, int>, Action<InfoMe, string, int>>(InfoMe.gI().addInfo, InfoMe_addInfo_hook, InfoMe_addInfo_original);
            InstallHook<Action, Action<Panel>, Action<Panel>>(new Panel(_).updateKey, Panel_updateKey_hook, Panel_updateKey_original);
            InstallHook<Action<int, int>, Action<ItemMap, int, int>, Action<ItemMap, int, int>>(new ItemMap(_).setPoint, ItemMap_setPoint_hook, ItemMap_setPoint_original);
            InstallHook<Action<MyVector, int>, Action<Menu, MyVector, int>, Action<Menu, MyVector, int>>(new Menu().startAt, Menu_startAt_hook, Menu_startAt_original);
            InstallHook<Action<string>, Action<Char, string>, Action<Char, string>>(new Char().addInfo, Char_addInfo_hook, Char_addInfo_original);
            InstallHook<Func<string, Image>, Func<string, Image>, Func<string, Image>>(GameCanvas.loadImage, GameCanvas_loadImage_hook, GameCanvas_loadImage_original);
            InstallHook<Action<mGraphics>, Action<mGraphics>, Action<mGraphics>>(GameCanvas.paintBGGameScr, GameCanvas_paintBGGameScr_hook, GameCanvas_paintBGGameScr_original);
            InstallHook<Action, Action<Mob>, Action<Mob>>(new Mob().startDie, Mob_startDie_hook, Mob_startDie_original);
            InstallHook<Action, Action<Mob>, Action<Mob>>(new Mob().update, Mob_update_hook, Mob_update_original);

            InstallHook<Func<string, Image>, Func<string, Image>, Func<string, Image>>(Image.createImage, Image_createImage_hook, Image_createImage_original);
            InstallHook<Action<string>, Action<GameScr, string>, Action<GameScr, string>>(new GameScr(_).chatVip, GameScr_chatVip_hook, GameScr_chatVip_original);
            InstallHook<Action<int>, Action<Panel, int>, Action<Panel, int>>(new Panel(_).updateScroolMouse, Panel_updateScroolMouse_hook, Panel_updateScroolMouse_original);
            InstallHook<Action, Action<Panel>, Action<Panel>>(new Panel(_).hide, Panel_hide_hook, Panel_hide_original);
            InstallHook<Action, Action<Panel>, Action<Panel>>(new Panel(_).hideNow, Panel_hideNow_hook, Panel_hideNow_original);

            //InstallHook<Action, Action, Action>(, _hook, _original);
        }

        /// <summary>
        /// Cài đặt 1 hook.
        /// </summary>
        /// <typeparam name="T1">Loại <see cref="Delegate"/> của <paramref name="hookTargetMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <typeparam name="T2">Loại <see cref="Delegate"/> của <paramref name="hookMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <typeparam name="T3">Loại <see cref="Delegate"/> của <paramref name="originalProxyMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <param name="hookTargetMethod">Hàm để hook vào.</param>
        /// <param name="hookMethod">Hàm mới được gọi thay thế hàm bị hook.</param>
        /// <param name="originalProxyMethod">Hàm có cùng signature với hàm gốc, có chức năng làm hàm thay thế để gọi đến hàm gốc mà không bị thay thế (hàm trampoline). Hàm này phải có attribute <code>[MethodImpl(MethodImplOptions.NoOptimization)]</code> để tránh bị tối ưu hóa mất khi biên dịch.</param>
        static void InstallHook<T1, T2, T3>(T1 hookTargetMethod, T2 hookMethod, T3 originalProxyMethod) where T1 : Delegate where T2 : Delegate where T3 : Delegate => InstallHook(hookTargetMethod.Method, hookMethod.Method, originalProxyMethod.Method);

        /// <param name="hookTargetMethod">Hàm để hook vào.</param>
        /// <param name="hookMethod">Hàm mới được gọi thay thế hàm bị hook.</param>
        /// <param name="originalProxyMethod">Hàm có cùng signature với hàm gốc, có chức năng làm hàm thay thế để gọi đến hàm gốc mà không bị thay thế (hàm trampoline). Hàm này phải có attribute <code>[MethodImpl(MethodImplOptions.NoOptimization)]</code> để tránh bị tối ưu hóa mất khi biên dịch.</param>
        static void InstallHook(MethodInfo hookTargetMethod, MethodInfo hookMethod, MethodInfo originalProxyMethod)
        {
            if (_hooks.Any(mH => mH.targetMethod == hookTargetMethod))
                throw new Exception("Hook already installed");
            MethodHook hook = new MethodHook(hookTargetMethod, hookMethod, originalProxyMethod);
            _hooks.Add(hook);
            hook.Install();
        }

        /// <summary>
        /// Gỡ bỏ tất cả hook.
        /// </summary>
        public static void UninstallAll()
        {
            foreach (MethodHook hook in _hooks)
                hook.Uninstall();
            _hooks.Clear();
        }

        /// <summary>
        /// Gỡ bỏ 1 hook.
        /// </summary>
        /// <typeparam name="T1">Loại <see cref="Delegate"/> của <paramref name="hookTargetMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <param name="hookTargetMethod">Hàm cần gỡ bỏ hook.</param>
        static void UninstallHook<T1>(T1 hookTargetMethod) where T1 : Delegate => UninstallHook(hookTargetMethod.Method);

        /// <summary>
        /// Gỡ bỏ 1 hook.
        /// </summary>
        /// <param name="hookTargetMethod">Hàm cần gỡ bỏ hook.</param>
        static void UninstallHook(MethodInfo hookTargetMethod)
        {
            MethodHook hook = _hooks.Find(mH => mH.targetMethod == hookTargetMethod);
            if (hook != null)
            {
                hook.Uninstall();
                _hooks.Remove(hook);
            }
        }

        #region Hooks
        static void Service_chat_hook(Service _this, string text)
        {
            if (!GameEvents.onSendChat(text))
                Service_chat_original(_this, text);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Service_chat_original(Service _this, string text)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Rms_saveRMSString_hook(string filename, string data)
        {
            GameEvents.onSaveRMSString(ref filename, ref data);
            Rms_saveRMSString_original(filename, data);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Rms_saveRMSString_original(string filename, string data)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void GameScr_updateKey_hook(GameScr _this)
        {
            if (!Controller.isStopReadMessage && !Char.myCharz().isTeleport && !Char.myCharz().isPaintNewSkill && !InfoDlg.isLock)
            {
                if (GameCanvas.isTouch && !ChatTextField.gI().isShow && !GameCanvas.menu.showMenu)
                {
                    //GameScr.updateKeyTouchControl()
                    if (!_this.isNotPaintTouchControl())
                        GameEvents.onUpdateTouchGameScr();
                }
                if ((!ChatTextField.gI().isShow || GameCanvas.keyAsciiPress == 0) && !_this.isLockKey && !GameCanvas.menu.showMenu && !_this.isOpenUI() && !Char.isLockKey && Char.myCharz().skillPaint == null && GameCanvas.keyAsciiPress != 0 && _this.mobCapcha == null && TField.isQwerty)
                {
                    GameEvents.onGameScrPressHotkeys();
                    if (!GameCanvas.keyPressed[1] && !GameCanvas.keyPressed[2] && !GameCanvas.keyPressed[3] && !GameCanvas.keyPressed[4] && !GameCanvas.keyPressed[5] && !GameCanvas.keyPressed[6] && !GameCanvas.keyPressed[7] && !GameCanvas.keyPressed[8] && !GameCanvas.keyPressed[9] && !GameCanvas.keyPressed[0] && GameCanvas.keyAsciiPress != 114 && GameCanvas.keyAsciiPress != 47)
                        GameEvents.onGameScrPressHotkeysUnassigned();
                }
            }
            GameScr_updateKey_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_updateKey_original(GameScr _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void ChatTextField_paint_hook(ChatTextField _this, mGraphics g)
        {
            GameEvents.onPaintChatTextField(_this, g);
            ChatTextField_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatTextField_paint_original(ChatTextField _this, mGraphics g)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void ChatTextField_startChat_hook_1(ChatTextField _this, int firstCharacter, IChatable parentScreen, string to)
        {
            if (!GameEvents.onStartChatTextField(_this, parentScreen))
                ChatTextField_startChat_original_1(_this, firstCharacter, parentScreen, to);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatTextField_startChat_original_1(ChatTextField _this, int firstCharacter, IChatable parentScreen, string to)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void ChatTextField_startChat_hook_2(ChatTextField _this, IChatable parentScreen, string to)
        {
            if (!GameEvents.onStartChatTextField(_this, parentScreen))
                ChatTextField_startChat_original_2(_this, parentScreen, to);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatTextField_startChat_original_2(ChatTextField _this, IChatable parentScreen, string to)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static int Rms_loadRMSInt_hook(string file)
        {
            if (GameEvents.onLoadRMSInt(file, out int result))
                return result;
            return Rms_loadRMSInt_original(file);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static int Rms_loadRMSInt_original(string file)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
            return 0;
        }

        static string Rms_GetiPhoneDocumentsPath_hook()
        {
            if (GameEvents.onGetRMSPath(out string result))
                return result;
            return Rms_GetiPhoneDocumentsPath_original();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static string Rms_GetiPhoneDocumentsPath_original()
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
            return null;
        }

        static void Teleport_update_hook(Teleport _this)
        {
            GameEvents.onTeleportUpdate(_this);
            Teleport_update_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Teleport_update_original(Teleport _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void ChatTextField_update_hook(ChatTextField _this)
        {
            if (!_this.isShow)
                GameEvents.onUpdateChatTextField(_this);
            ChatTextField_update_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatTextField_update_original(ChatTextField _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Rms_clearAll_hook()
        {
            GameEvents.onClearAllRMS();
            Rms_clearAll_original();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Rms_clearAll_original()
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void GameScr_update_hook(GameScr _this)
        {
            GameEvents.onUpdateGameScr();
            GameScr_update_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_update_original(GameScr _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Service_login_hook(Service _this, string username, string pass, string version, sbyte type)
        {
            GameEvents.onLogin(ref username, ref pass, ref type);
            Service_login_original(_this, username, pass, version, type);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Service_login_original(Service _this, string username, string pass, string version, sbyte type)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void ServerListScreen_switchToMe_hook(ServerListScreen _this)
        {
            ServerListScreen_switchToMe_original(_this);
            GameEvents.onServerListScreenLoaded();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ServerListScreen_switchToMe_original(ServerListScreen _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Session_ME_connect_hook(Session_ME _this, string host, int port)
        {
            GameEvents.onSessionConnecting(ref host, ref port);
            Session_ME_connect_original(_this, host, port);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Session_ME_connect_original(Session_ME _this, string host, int port)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void ServerListScreen_show2_hook(ServerListScreen _this)
        {
            ServerListScreen_show2_original(_this);
            GameEvents.onSceenDownloadDataShow();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ServerListScreen_show2_original(ServerListScreen _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void MotherCanvas_checkZoomLevel_hook(MotherCanvas _this, int w, int h)
        {
            if (!GameEvents.onCheckZoomLevel())
                MotherCanvas_checkZoomLevel_original(_this, w, h);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void MotherCanvas_checkZoomLevel_original(MotherCanvas _this, int w, int h)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void GameCanvas_keyPressedz_hook(GameCanvas _this, int keyCode)
        {
            //if (!GameEvents.onKeyPressedz(keyCode, isFromSync))  
            if (!GameEvents.onKeyPressedz(keyCode, false))
                GameCanvas_keyPressedz_original(_this, keyCode);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameCanvas_keyPressedz_original(GameCanvas _this, int keyCode)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void GameCanvas_keyReleasedz_hook(GameCanvas _this, int keyCode)
        {
            //if (!GameEvents.onKeyReleasedz(keyCode, isFromSync))  
            if (!GameEvents.onKeyReleasedz(keyCode, false))
                GameCanvas_keyReleasedz_original(_this, keyCode);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameCanvas_keyReleasedz_original(GameCanvas _this, int keyCode)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void ChatPopup_addChatPopupMultiLine_hook(string chat, int howLong, Npc c)
        {
            if (!GameEvents.onChatPopupMultiLine(chat))
                ChatPopup_addChatPopupMultiLine_original(chat, howLong, c);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatPopup_addChatPopupMultiLine_original(string chat, int howLong, Npc c)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }
        
        static void ChatPopup_addBigMessage_hook(string chat, int howLong, Npc c)
        {
            if (!GameEvents.onAddBigMessage(chat, c))
                ChatPopup_addChatPopupMultiLine_original(chat, howLong, c);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatPopup_addBigMessage_original(string chat, int howLong, Npc c)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Controller_loadInfoMap_hook(Controller _this, Message msg)
        {
            Controller_loadInfoMap_original(_this, msg);
            GameEvents.onInfoMapLoaded();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Controller_loadInfoMap_original(Controller _this, Message msg)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void GameScr_paint_hook(GameScr _this, mGraphics g)
        {
            GameScr_paint_original(_this, g);
            GameEvents.onPaintGameScr(g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_paint_original(GameScr _this, mGraphics g)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Char_setSkillPaint_hook(Char _this, SkillPaint skillPaint, int sType)
        {
            if (!GameEvents.onUseSkill(_this))
                Char_setSkillPaint_original(_this, skillPaint, sType);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_setSkillPaint_original(Char _this, SkillPaint skillPaint, int sType)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void InfoMe_addInfo_hook(InfoMe _this, string s, int Type)
        {
            InfoMe_addInfo_original(_this, s, Type);
            GameEvents.onAddInfoMe(s);   
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void InfoMe_addInfo_original(InfoMe _this, string s, int Type)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Panel_updateKey_hook(Panel _this)
        {
            if ((_this.chatTField == null || !_this.chatTField.isShow) && GameCanvas.panel.isDoneCombine && !InfoDlg.isShow)
                GameEvents.onUpdateKeyPanel(_this);
            if ((_this.tabIcon == null || !_this.tabIcon.isShow) && !_this.isClose && _this.isShow && !_this.cmdClose.isPointerPressInside())
                GameEvents.onUpdateTouchPanel();
            Panel_updateKey_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_updateKey_original(Panel _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void ItemMap_setPoint_hook(ItemMap _this, int xEnd, int yEnd)
        {
            GameEvents.onSetPointItemMap(xEnd, yEnd);
            ItemMap_setPoint_original(_this, xEnd, yEnd);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ItemMap_setPoint_original(ItemMap _this, int xEnd, int yEnd)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Menu_startAt_hook(Menu _this, MyVector menuItems, int pos)
        {
            if (!GameEvents.onMenuStartAt(menuItems))
                Menu_startAt_original(_this, menuItems, pos);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Menu_startAt_original(Menu _this, MyVector menuItems, int pos)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Char_addInfo_hook(Char _this, string info)
        {
            GameEvents.onAddInfoChar(_this, info);
            Char_addInfo_original(_this, info);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_addInfo_original(Char _this, string info)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static Image GameCanvas_loadImage_hook(string path)
        {
            GameEvents.onLoadImageGameCanvas();
            return GameCanvas_loadImage_original(path);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static Image GameCanvas_loadImage_original(string path)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
            return null;
        }

        static void GameCanvas_paintBGGameScr_hook(mGraphics g)
        {
            if (!GameEvents.onPaintBgGameScr(g))
                GameCanvas_paintBGGameScr_original(g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameCanvas_paintBGGameScr_original(mGraphics g)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Mob_startDie_hook(Mob _this)
        {
            GameEvents.onMobStartDie(_this);
            Mob_startDie_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Mob_startDie_original(Mob _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Mob_update_hook(Mob _this)
        {
            GameEvents.onUpdateMob(_this);
            Mob_update_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Mob_update_original(Mob _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static Image Image_createImage_hook(string filename)
        {
            return GameEvents.onCreateImage(filename);
            //return Image_createImage_original(filename);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static Image Image_createImage_original(string filename)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
            return null;
        }

        static void GameScr_chatVip_hook(GameScr _this, string chatVip)
        {
            GameEvents.onChatVip(chatVip);
            GameScr_chatVip_original(_this, chatVip);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_chatVip_original(GameScr _this, string chatVip)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Panel_updateScroolMouse_hook(Panel _this, int a)
        {
            GameEvents.onUpdateScrollMousePanel(_this, ref a);
            Panel_updateScroolMouse_original(_this, a);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_updateScroolMouse_original(Panel _this, int a)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Panel_hide_hook(Panel _this)
        {
            if (_this.timeShow <= 0)
                GameEvents.onPanelHide(_this);
            Panel_hide_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_hide_original(Panel _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Panel_hideNow_hook(Panel _this)
        {
            if (_this.timeShow <= 0)
                GameEvents.onPanelHide(_this);
            Panel_hideNow_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_hideNow_original(Panel _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }
        #endregion
    }
}
internal class HookObj { }