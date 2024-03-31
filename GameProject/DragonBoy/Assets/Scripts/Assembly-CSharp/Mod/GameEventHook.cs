using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using MonoHook;
using UnityEngine;

namespace Mod
{
    /// <summary>
    /// Hook vào các hàm của game để gọi các hàm trong <see cref="GameEvents"/>.
    /// </summary>
    public static class GameEventHook
    {
        static List<MethodHook> _hooks = new List<MethodHook>();

        /// <summary>
        /// Cài đặt tất cả hook.
        /// </summary>
        public static void InstallAll()
        {
            InstallHook<Action<string>, Action<Service, string>, Action<Service, string>>(Service.gI().chat, Service_chat_hook, Service_chat_original);
            InstallHook(typeof(Main).GetMethod("OnApplicationQuit", BindingFlags.NonPublic | BindingFlags.Instance), typeof(GameEventHook).GetMethod(nameof(Main_OnApplicationQuit_hook), BindingFlags.NonPublic | BindingFlags.Static), typeof(GameEventHook).GetMethod(nameof(Main_OnApplicationQuit_original), BindingFlags.NonPublic | BindingFlags.Static));
            InstallHook<Action<string, string>, Action<string, string>, Action<string, string>>(Rms.saveRMSString, Rms_saveRMSString_hook, Rms_saveRMSString_original);
            InstallHook<Action, Action<GameScr>, Action<GameScr>>(GameScr.gI().updateKey, GameScr_updateKey_hook, GameScr_updateKey_original);
            InstallHook<Action<mGraphics>, Action<ChatTextField, mGraphics>, Action<ChatTextField, mGraphics>>(ChatTextField.gI().paint, ChatTextField_paint_hook, ChatTextField_paint_original);
            InstallHook<Action<int, IChatable, string>, Action<ChatTextField, int, IChatable, string>, Action<ChatTextField, int, IChatable, string>>(ChatTextField.gI().startChat, ChatTextField_startChat_hook_1, ChatTextField_startChat_original_1);
            InstallHook<Action<IChatable, string>, Action<ChatTextField, IChatable, string>, Action<ChatTextField, IChatable, string>>(ChatTextField.gI().startChat, ChatTextField_startChat_hook_2, ChatTextField_startChat_original_2);
            InstallHook<Func<string, int>, Func<string, int>, Func<string, int>>(Rms.loadRMSInt, Rms_loadRMSInt_hook, Rms_loadRMSInt_original);
            InstallHook<Func<string>, Func<string>, Func<string>>(Rms.GetiPhoneDocumentsPath, Rms_GetiPhoneDocumentsPath_hook, Rms_GetiPhoneDocumentsPath_original);
            InstallHook<Action, Action<Teleport>, Action<Teleport>>(new Teleport().update, Teleport_update_hook, Teleport_update_original);
            //...
        }

        /// <summary>
        /// Cài đặt 1 hook.
        /// </summary>
        /// <typeparam name="T1">Loại <see cref="Delegate"/> của <paramref name="hookTargetMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <typeparam name="T2">Loại <see cref="Delegate"/> của <paramref name="hookMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <typeparam name="T3">Loại <see cref="Delegate"/> của <paramref name="originalProxyMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <param name="hookTargetMethod">Hàm để hook vào.</param>
        /// <param name="hookMethod">Hàm mới được gọi thay thế hàm bị hook.</param>
        /// <param name="originalProxyMethod">Hàm có cùng signature với hàm gốc, có chức năng làm hàm thay thế để gọi đến hàm gốc mà không bị thay thế. Hàm này phải có attribute <code>[MethodImpl(MethodImplOptions.NoOptimization)]</code> để tránh bị tối ưu hóa mất khi biên dịch.</param>
        static void InstallHook<T1, T2, T3>(T1 hookTargetMethod, T2 hookMethod, T3 originalProxyMethod) where T1 : Delegate where T2 : Delegate where T3 : Delegate => InstallHook(hookTargetMethod.Method, hookMethod.Method, originalProxyMethod.Method);

        /// <param name="hookTargetMethod">Hàm để hook vào.</param>
        /// <param name="hookMethod">Hàm mới được gọi thay thế hàm bị hook.</param>
        /// <param name="originalProxyMethod">Hàm có cùng signature với hàm gốc, có chức năng làm hàm thay thế để gọi đến hàm gốc mà không bị thay thế. Hàm này phải có attribute <code>[MethodImpl(MethodImplOptions.NoOptimization)]</code> để tránh bị tối ưu hóa mất khi biên dịch.</param>
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
        /// 
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
        static void Service_chat_hook(Service instance, string text)
        {
            if (!GameEvents.onSendChat(text))
                Service_chat_original(instance, text);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Service_chat_original(Service instance, string text)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void Main_OnApplicationQuit_hook(Main instance)
        {
            GameEvents.onGameClosing();
            Main_OnApplicationQuit_original(instance);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Main_OnApplicationQuit_original(Main instance)
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

        static void GameScr_updateKey_hook(GameScr instance)
        {
            if ((!ChatTextField.gI().isShow || GameCanvas.keyAsciiPress == 0) && !instance.isLockKey && !GameCanvas.menu.showMenu && !instance.isOpenUI() && !Char.isLockKey && GameCanvas.keyAsciiPress != 0 && instance.mobCapcha == null && TField.isQwerty)
            {
                GameEvents.onGameScrPressHotkeys();
                if (!GameCanvas.keyPressed[1] && !GameCanvas.keyPressed[2] && !GameCanvas.keyPressed[3] && !GameCanvas.keyPressed[4] && !GameCanvas.keyPressed[5] && !GameCanvas.keyPressed[6] && !GameCanvas.keyPressed[7] && !GameCanvas.keyPressed[8] && !GameCanvas.keyPressed[9] && !GameCanvas.keyPressed[0] && GameCanvas.keyAsciiPress != 114 && GameCanvas.keyAsciiPress != 47)
                {
                    GameEvents.onGameScrPressHotkeysUnassigned();
                }
            }
            GameScr_updateKey_original(instance);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_updateKey_original(GameScr instance)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void ChatTextField_paint_hook(ChatTextField instance, mGraphics g)
        {
            GameEvents.onPaintChatTextField(instance, g);
            ChatTextField_paint_original(instance, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatTextField_paint_original(ChatTextField instance, mGraphics g)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void ChatTextField_startChat_hook_1(ChatTextField instance, int firstCharacter, IChatable parentScreen, string to)
        {
            if (!GameEvents.onStartChatTextField(instance, parentScreen))
                ChatTextField_startChat_original_1(instance, firstCharacter, parentScreen, to);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatTextField_startChat_original_1(ChatTextField instance, int firstCharacter, IChatable parentScreen, string to)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }

        static void ChatTextField_startChat_hook_2(ChatTextField instance, IChatable parentScreen, string to)
        {
            if (!GameEvents.onStartChatTextField(instance, parentScreen))
                ChatTextField_startChat_original_2(instance, parentScreen, to);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatTextField_startChat_original_2(ChatTextField instance, IChatable parentScreen, string to)
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

        static void Teleport_update_hook(Teleport instance)
        {
            GameEvents.onTeleportUpdate(instance);
            Teleport_update_original(instance);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Teleport_update_original(Teleport instance)
        {
            Debug.Log("Gọi hàm này để gọi đến hàm gốc vì hàm gốc đã bị hook sang hàm khác.");
        }
        #endregion
    }
}