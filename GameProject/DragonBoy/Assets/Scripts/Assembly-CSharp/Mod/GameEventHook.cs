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
    /// <remarks>
    /// Vấn đề đã biết: trên Android scripting backend Mono, 1 số hàm thỉnh thoảng sẽ không hook được do vị trí của các hàm trong bộ nhớ sau khi JIT quá xa.
    /// </remarks>
    public static class GameEventHook
    {
        static List<MethodHook> _hooks = new List<MethodHook>();
        static HookObj _ = new HookObj();

        /// <summary>
        /// Cài đặt tất cả hook.
        /// </summary>
        public static void InstallAll()
        {
            MotherCanvas motherCanvas = new MotherCanvas(_);
            ChatTextField chatTextField = new ChatTextField(_);
            Teleport teleport = new Teleport(_);
            ServerListScreen serverListScreen = new ServerListScreen(_);
            GameCanvas gameCanvas = new GameCanvas(_);
            GameScr gameScr = new GameScr(_);
            Panel panel = new Panel(_);
            Char @char = new Char();
            ItemMap itemMap = new ItemMap(_);
            Menu menu = new Menu();
            Mob mob = new Mob();
            SoundMn soundMn = new SoundMn();
            GamePad gamePad = new GamePad(_);

            TryInstallHook<Action<int, int>, Action<MotherCanvas, int, int>>(motherCanvas.checkZoomLevel, MotherCanvas_checkZoomLevel_hook, MotherCanvas_checkZoomLevel_original);
            TryInstallHook<Func<string, Image>, Func<string, Image>>(Image.createImage, Image_createImage_hook, Image_createImage_original);
            TryInstallHook<Func<string>, Func<string>>(Rms.GetiPhoneDocumentsPath, Rms_GetiPhoneDocumentsPath_hook, Rms_GetiPhoneDocumentsPath_original);
            TryInstallHook<Func<string, int>, Func<string, int>>(Rms.loadRMSInt, Rms_loadRMSInt_hook, Rms_loadRMSInt_original);
            TryInstallHook<Action<string, string>, Action<string, string>>(Rms.saveRMSString, Rms_saveRMSString_hook, Rms_saveRMSString_original);
            TryInstallHook<Action<string>, Action<Service, string>>(Service.gI().chat, Service_chat_hook, Service_chat_original);
            TryInstallHook<Action, Action<GameScr>>(gameScr.updateKey, GameScr_updateKey_hook, GameScr_updateKey_original);
            TryInstallHook<Action<mGraphics>, Action<ChatTextField, mGraphics>>(chatTextField.paint, ChatTextField_paint_hook, ChatTextField_paint_original);
            TryInstallHook<Action<int, IChatable, string>, Action<ChatTextField, int, IChatable, string>>(chatTextField.startChat, ChatTextField_startChat_hook_1, ChatTextField_startChat_original_1);
            TryInstallHook<Action<IChatable, string>, Action<ChatTextField, IChatable, string>>(chatTextField.startChat, ChatTextField_startChat_hook_2, ChatTextField_startChat_original_2);
            TryInstallHook<Action, Action<Teleport>>(teleport.update, Teleport_update_hook, Teleport_update_original);
            TryInstallHook<Action, Action<ChatTextField>>(chatTextField.update, ChatTextField_update_hook, ChatTextField_update_original);
            TryInstallHook<Action, Action>(Rms.clearAll, Rms_clearAll_hook, Rms_clearAll_original);
            TryInstallHook<Action, Action<GameScr>>(gameScr.update, GameScr_update_hook, GameScr_update_original);
            TryInstallHook<Action<string, string, string, sbyte>, Action<Service, string, string, string, sbyte>>(Service.gI().login, Service_login_hook, Service_login_original);
            TryInstallHook<Action, Action<ServerListScreen>>(serverListScreen.switchToMe, ServerListScreen_switchToMe_hook, ServerListScreen_switchToMe_original);
            TryInstallHook<Action<string, int>, Action<Session_ME, string, int>>(Session_ME.gI().connect, Session_ME_connect_hook, Session_ME_connect_original);
            TryInstallHook<Action, Action<ServerListScreen>>(serverListScreen.show2, ServerListScreen_show2_hook, ServerListScreen_show2_original);
            TryInstallHook<Action<int>, Action<GameCanvas, int>>(gameCanvas.keyPressedz, GameCanvas_keyPressedz_hook, GameCanvas_keyPressedz_original);
            TryInstallHook<Action<int>, Action<GameCanvas, int>>(gameCanvas.keyReleasedz, GameCanvas_keyReleasedz_hook, GameCanvas_keyReleasedz_original);
            TryInstallHook<Action<string, int, Npc>, Action<string, int, Npc>>(ChatPopup.addChatPopupMultiLine, ChatPopup_addChatPopupMultiLine_hook, ChatPopup_addChatPopupMultiLine_original);
            TryInstallHook<Action<string, int, Npc>, Action<string, int, Npc>>(ChatPopup.addBigMessage, ChatPopup_addBigMessage_hook, ChatPopup_addBigMessage_original);
            TryInstallHook<Action<Message>, Action<Controller, Message>>(Controller.gI().loadInfoMap, Controller_loadInfoMap_hook, Controller_loadInfoMap_original);
            TryInstallHook<Action<mGraphics>, Action<GameScr, mGraphics>>(gameScr.paint, GameScr_paint_hook, GameScr_paint_original);
            TryInstallHook<Action<SkillPaint, int>, Action<Char, SkillPaint, int>>(@char.setSkillPaint, Char_setSkillPaint_hook, Char_setSkillPaint_original);
            TryInstallHook<Action<string, int>, Action<InfoMe, string, int>>(InfoMe.gI().addInfo, InfoMe_addInfo_hook, InfoMe_addInfo_original);
            TryInstallHook<Action, Action<Panel>>(panel.updateKey, Panel_updateKey_hook, Panel_updateKey_original);
            TryInstallHook<Action<int, int>, Action<ItemMap, int, int>>(itemMap.setPoint, ItemMap_setPoint_hook, ItemMap_setPoint_original);
            TryInstallHook<Action<MyVector, int>, Action<Menu, MyVector, int>>(menu.startAt, Menu_startAt_hook, Menu_startAt_original);
            TryInstallHook<Action<string>, Action<Char, string>>(@char.addInfo, Char_addInfo_hook, Char_addInfo_original);
            TryInstallHook<Action<mGraphics>, Action<mGraphics>>(GameCanvas.paintBGGameScr, GameCanvas_paintBGGameScr_hook, GameCanvas_paintBGGameScr_original);
            TryInstallHook<Action, Action<Mob>>(mob.startDie, Mob_startDie_hook, Mob_startDie_original);
            TryInstallHook<Action, Action<Mob>>(mob.update, Mob_update_hook, Mob_update_original);
            TryInstallHook<Action<string>, Action<GameScr, string>>(gameScr.chatVip, GameScr_chatVip_hook, GameScr_chatVip_original);
            TryInstallHook<Action<int>, Action<Panel, int>>(panel.updateScroolMouse, Panel_updateScroolMouse_hook, Panel_updateScroolMouse_original);
            TryInstallHook<Action, Action<Panel>>(panel.hide, Panel_hide_hook, Panel_hide_original);
            TryInstallHook<Action, Action<Panel>>(panel.hideNow, Panel_hideNow_hook, Panel_hideNow_original);
            TryInstallHook<Action<mGraphics>, Action<GameScr, mGraphics>>(gameScr.paintTouchControl, GameScr_paintTouchControl_hook, GameScr_paintTouchControl_original);
            TryInstallHook<Action<mGraphics>, Action<GameScr, mGraphics>>(gameScr.paintGamePad, GameScr_paintGamePad_hook, GameScr_paintGamePad_original);
            TryInstallHook<Action, Action<Panel>>(panel.doFireOption, Panel_doFireOption_hook, Panel_doFireOption_original);
            TryInstallHook<Action, Action<SoundMn>>(soundMn.getStrOption, SoundMn_getStrOption_hook, SoundMn_getStrOption_original);
            TryInstallHook<Action, Action>(GameScr.setSkillBarPosition, GameScr_setSkillBarPosition_hook, GameScr_setSkillBarPosition_original);
            TryInstallHook(typeof(GamePad).GetConstructors()[0], new Action<GamePad>(GamePad__ctor_hook).Method, new Action<GamePad>(GamePad__ctor_original).Method);
            TryInstallHook<Action<mGraphics>, Action<GamePad, mGraphics>>(gamePad.paint, GamePad_paint_hook, GamePad_paint_original);
            TryInstallHook<Action<mGraphics>, Action<GameScr, mGraphics>>(gameScr.paintSelectedSkill, GameScr_paintSelectedSkill_hook, GameScr_paintSelectedSkill_original);
            TryInstallHook<Action<mGraphics>, Action<Panel, mGraphics>>(panel.paintToolInfo, Panel_paintToolInfo_hook, Panel_paintToolInfo_original);
            TryInstallHook<Action<sbyte>, Action<sbyte>>(mResources.loadLanguague, mResources_loadLanguague_hook, mResources_loadLanguague_original);

            //TryInstallHook<Action, Action, Action>(, _hook, _original);
        }

        #region Hook methods
        /// <summary>
        /// Thử cài đặt 1 hook.
        /// </summary>
        /// <typeparam name="T1">Loại <see cref="Delegate"/> của <paramref name="hookTargetMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <typeparam name="T2">Loại <see cref="Delegate"/> của <paramref name="hookMethod"/> và <paramref name="originalProxyMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <param name="hookTargetMethod">Hàm để hook vào.</param>
        /// <param name="hookMethod">Hàm mới được gọi thay thế hàm bị hook.</param>
        /// <param name="originalProxyMethod">Hàm có cùng signature với hàm gốc, có chức năng làm hàm thay thế để gọi đến hàm gốc mà không bị thay thế (hàm trampoline). Hàm này phải có attribute <code>[MethodImpl(MethodImplOptions.NoOptimization)]</code> để tránh bị tối ưu hóa mất khi biên dịch.</param>
        static void TryInstallHook<T1, T2>(T1 hookTargetMethod, T2 hookMethod, T2 originalProxyMethod) where T1 : Delegate where T2 : Delegate => TryInstallHook<T1, T2, T2>(hookTargetMethod, hookMethod, originalProxyMethod);

        /// <summary>
        /// Thử cài đặt 1 hook.
        /// </summary>
        /// <typeparam name="T1">Loại <see cref="Delegate"/> của <paramref name="hookTargetMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <typeparam name="T2">Loại <see cref="Delegate"/> của <paramref name="hookMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <typeparam name="T3">Loại <see cref="Delegate"/> của <paramref name="originalProxyMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <param name="hookTargetMethod">Hàm để hook vào.</param>
        /// <param name="hookMethod">Hàm mới được gọi thay thế hàm bị hook.</param>
        /// <param name="originalProxyMethod">Hàm có cùng signature với hàm gốc, có chức năng làm hàm thay thế để gọi đến hàm gốc mà không bị thay thế (hàm trampoline). Hàm này phải có attribute <code>[MethodImpl(MethodImplOptions.NoOptimization)]</code> để tránh bị tối ưu hóa mất khi biên dịch.</param>
        static void TryInstallHook<T1, T2, T3>(T1 hookTargetMethod, T2 hookMethod, T3 originalProxyMethod) where T1 : Delegate where T2 : Delegate where T3 : Delegate => TryInstallHook(hookTargetMethod.Method, hookMethod.Method, originalProxyMethod.Method);

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

        /// <summary>
        /// Thử cài đặt 1 hook.
        /// </summary>
        /// <param name="hookTargetMethod">Hàm để hook vào.</param>
        /// <param name="hookMethod">Hàm mới được gọi thay thế hàm bị hook.</param>
        /// <param name="originalProxyMethod">Hàm có cùng signature với hàm gốc, có chức năng làm hàm thay thế để gọi đến hàm gốc mà không bị thay thế (hàm trampoline). Hàm này phải có attribute <code>[MethodImpl(MethodImplOptions.NoOptimization)]</code> để tránh bị tối ưu hóa mất khi biên dịch.</param>
        static void TryInstallHook(MethodBase hookTargetMethod, MethodBase hookMethod, MethodBase originalProxyMethod)
        {
            try
            {
                InstallHook(hookTargetMethod, hookMethod, originalProxyMethod);
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        /// <summary>
        /// Cài đặt 1 hook.
        /// </summary>
        /// <param name="hookTargetMethod">Hàm để hook vào.</param>
        /// <param name="hookMethod">Hàm mới được gọi thay thế hàm bị hook.</param>
        /// <param name="originalProxyMethod">Hàm có cùng signature với hàm gốc, có chức năng làm hàm thay thế để gọi đến hàm gốc mà không bị thay thế (hàm trampoline). Hàm này phải có attribute <code>[MethodImpl(MethodImplOptions.NoOptimization)]</code> để tránh bị tối ưu hóa mất khi biên dịch.</param>
        static void InstallHook(MethodBase hookTargetMethod, MethodBase hookMethod, MethodBase originalProxyMethod)
        {
            if (_hooks.Any(mH => mH.targetMethod == hookTargetMethod))
                throw new Exception("Hook already installed");
            Debug.Log($"Hooking {hookTargetMethod.Name} to {hookMethod.Name}...");
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
        #endregion

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
                    {
                        if (GameEvents.onUpdateTouchGameScr(_this))
                            return;
                    }
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
            if (!GameEvents.onClearAllRMS())
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
            GameEvents.onScreenDownloadDataShow();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ServerListScreen_show2_original(ServerListScreen _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void MotherCanvas_checkZoomLevel_hook(MotherCanvas _this, int w, int h)
        {
            if (!GameEvents.onCheckZoomLevel(w, h))
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
            if (GameEvents.onCreateImage(filename, out Image image))
                return image;
            return Image_createImage_original(filename);
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

        static void GameScr_paintTouchControl_hook(GameScr _this, mGraphics g)
        {
            if (!GameEvents.onPaintTouchControl(_this, g))
                GameScr_paintTouchControl_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_paintTouchControl_original(GameScr _this, mGraphics g)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void GameScr_paintGamePad_hook(GameScr _this, mGraphics g)
        {
            if (!GameEvents.onPaintGamePad(g))
                GameScr_paintGamePad_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_paintGamePad_original(GameScr _this, mGraphics g)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void SoundMn_getStrOption_hook(SoundMn _this)
        {
            if (!GameEvents.onSoundMnGetStrOption())
                SoundMn_getStrOption_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void SoundMn_getStrOption_original(SoundMn _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Panel_doFireOption_hook(Panel _this)
        {
            if (!GameEvents.onPanelFireOption(_this))
                Panel_doFireOption_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_doFireOption_original(Panel _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void GamePad_paint_hook(GamePad _this, mGraphics g)
        {
            if (!GameEvents.onGamepadPaint(_this, g))
                GamePad_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GamePad_paint_original(GamePad _this, mGraphics g)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void GamePad__ctor_hook(GamePad _this)
        {
            GamePad__ctor_original(_this);
            _this.SetGamePadZone();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GamePad__ctor_original(GamePad _this)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void GameScr_setSkillBarPosition_hook()
        {
            if (!GameEvents.onSetSkillBarPosition())
                GameScr_setSkillBarPosition_original();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_setSkillBarPosition_original()
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void GameScr_paintSelectedSkill_hook(GameScr _this, mGraphics g)
        {
            GameEvents.onGameScrPaintSelectedSkill(_this, g);
            GameScr_paintSelectedSkill_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_paintSelectedSkill_original(GameScr _this, mGraphics g)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }
        #endregion

        #region Hooks 2
        static void Panel_paintToolInfo_hook(Panel _this, mGraphics g)
        {
            if (!GameEvents.onPanelPaintToolInfo(g))
                Panel_paintToolInfo_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_paintToolInfo_original(Panel _this, mGraphics g)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void mResources_loadLanguague_hook(sbyte newLanguage)
        {
            mResources_loadLanguague_original(newLanguage);
            GameEvents.onLoadLanguage(newLanguage);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void mResources_loadLanguague_original(sbyte newLanguage)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }
        #endregion
    }
}
internal class HookObj { }