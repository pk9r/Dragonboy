using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mod.Graphics;
using MonoHook;
using UnityEngine;
using Mod.R;

#if UNITY_EDITOR
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;
using System.Reflection.Emit;
using OpCodes = Mono.Cecil.Cil.OpCodes;
using ROpCodes = System.Reflection.Emit.OpCodes;
#endif

namespace Mod
{
    /// <summary>
    /// Hook vào các hàm của game để gọi các hàm trong <see cref="GameEvents"/>.
    /// </summary>
    /// <remarks>
    /// Vấn đề đã biết: trên Android scripting backend Mono, 1 số hàm thỉnh thoảng sẽ không hook được do vị trí của các hàm trong bộ nhớ sau khi JIT quá xa.
    /// </remarks>
    internal static class GameEventHook
    {
        /// <summary>
        /// Cài đặt tất cả hook.
        /// </summary>
        static void InstallAll()
        {
            //All instances can be null because this method will be rebuilt at build time or runtime (Editor play mode). Take a look at the generated IL code yourself, see the CreateDynamicInstallAllMethod method below, or see the PipelineBuild class.
            #region Instances
            MotherCanvas motherCanvas = null;
            ChatTextField chatTextField = null;
            Teleport teleport = null;
            ServerListScreen serverListScreen = null;
            GameCanvas gameCanvas = null;
            GameScr gameScr = null;
            Panel panel = null;
            Char ch = null;
            ItemMap itemMap = null;
            Menu menu = null;
            Mob mob = null;
            SoundMn soundMn = null;
            GamePad gamePad = null;
            LoginScr loginScr = null;
            Skill skill = null;
            Service service = null;
            Session_ME session_ME = null;
            Controller controller = null;
            InfoMe infoMe = null;
            BgItem bgItem = null;
            Effect effect = null;
            mGraphics g = null;
            MagicTree magicTree = null;
            Npc npc = null;
            ServerEffect serverEffect = null;
            #endregion

            TryInstallHook<Action<int, int>, Action<MotherCanvas, int, int>>(motherCanvas.checkZoomLevel, MotherCanvas_checkZoomLevel_hook, MotherCanvas_checkZoomLevel_original);
            TryInstallHook<Func<string, Image>, Func<string, Image>>(Image.createImage, Image_createImage_hook, Image_createImage_original);
            TryInstallHook<Func<string>, Func<string>>(Rms.GetiPhoneDocumentsPath, Rms_GetiPhoneDocumentsPath_hook, Rms_GetiPhoneDocumentsPath_original);
            TryInstallHook<Action<string, string>, Action<string, string>>(Rms.saveRMSString, Rms_saveRMSString_hook, Rms_saveRMSString_original);
            TryInstallHook<Action<string, sbyte[]>, Action<string, sbyte[]>>(Rms.saveRMS, Rms_saveRMS_hook);
            TryInstallHook<Func<string, sbyte[]>, Func<string, sbyte[]>>(Rms.loadRMS, Rms_loadRMS_hook);
            TryInstallHook<Action, Action>(ServerListScreen.saveIP, ServerListScreen_saveIP_hook);
            TryInstallHook<Action, Action>(ServerListScreen.loadIP, ServerListScreen_loadIP_hook);
            TryInstallHook<Action<string>, Action<string>>(ServerListScreen.getServerList, ServerListScreen_getServerList_hook, ServerListScreen_getServerList_original);
            TryInstallHook(typeof(Panel).GetConstructor(new Type[0]), new Action<Panel>(Panel_ctor_hook).Method, new Action<Panel>(Panel__ctor_original).Method);

            TryInstallHook<Action, Action<GameScr>>(gameScr.updateKey, GameScr_updateKey_hook, GameScr_updateKey_original);
            TryInstallHook<Action<mGraphics>, Action<ChatTextField, mGraphics>>(chatTextField.paint, ChatTextField_paint_hook, ChatTextField_paint_original);
            TryInstallHook<Action<int, IChatable, string>, Action<ChatTextField, int, IChatable, string>>(chatTextField.startChat, ChatTextField_startChat_hook_1, ChatTextField_startChat_original_1);
            TryInstallHook<Action<IChatable, string>, Action<ChatTextField, IChatable, string>>(chatTextField.startChat, ChatTextField_startChat_hook_2, ChatTextField_startChat_original_2);
            TryInstallHook<Action, Action<Teleport>>(teleport.update, Teleport_update_hook, Teleport_update_original);
            TryInstallHook<Action, Action<ChatTextField>>(chatTextField.update, ChatTextField_update_hook, ChatTextField_update_original);
            TryInstallHook<Action, Action>(Rms.clearAll, Rms_clearAll_hook, Rms_clearAll_original);
            TryInstallHook<Action, Action<GameScr>>(gameScr.update, GameScr_update_hook, GameScr_update_original);
            TryInstallHook<Action<string, string, string, sbyte>, Action<Service, string, string, string, sbyte>>(service.login, Service_login_hook, Service_login_original);
            TryInstallHook<Action, Action<ServerListScreen>>(serverListScreen.switchToMe, ServerListScreen_switchToMe_hook, ServerListScreen_switchToMe_original);
            TryInstallHook<Action<string, int>, Action<Session_ME, string, int>>(session_ME.connect, Session_ME_connect_hook, Session_ME_connect_original);
            TryInstallHook<Action, Action<ServerListScreen>>(serverListScreen.show2, ServerListScreen_show2_hook, ServerListScreen_show2_original);
            TryInstallHook<Action<int>, Action<GameCanvas, int>>(gameCanvas.keyPressedz, GameCanvas_keyPressedz_hook, GameCanvas_keyPressedz_original);
            TryInstallHook<Action<int>, Action<GameCanvas, int>>(gameCanvas.keyReleasedz, GameCanvas_keyReleasedz_hook, GameCanvas_keyReleasedz_original);
            TryInstallHook<Action<string, int, Npc>, Action<string, int, Npc>>(ChatPopup.addChatPopupMultiLine, ChatPopup_addChatPopupMultiLine_hook, ChatPopup_addChatPopupMultiLine_original);
            TryInstallHook<Action<string, int, Npc>, Action<string, int, Npc>>(ChatPopup.addBigMessage, ChatPopup_addBigMessage_hook, ChatPopup_addBigMessage_original);
            TryInstallHook<Action<Message>, Action<Controller, Message>>(controller.loadInfoMap, Controller_loadInfoMap_hook, Controller_loadInfoMap_original);
            TryInstallHook<Action<mGraphics>, Action<GameScr, mGraphics>>(gameScr.paint, GameScr_paint_hook, GameScr_paint_original);
            TryInstallHook<Action<SkillPaint, int>, Action<Char, SkillPaint, int>>(ch.setSkillPaint, Char_setSkillPaint_hook, Char_setSkillPaint_original);
            TryInstallHook<Action<string, int>, Action<InfoMe, string, int>>(infoMe.addInfo, InfoMe_addInfo_hook, InfoMe_addInfo_original);
            TryInstallHook<Action, Action<Panel>>(panel.updateKey, Panel_updateKey_hook, Panel_updateKey_original);
            TryInstallHook<Action<int, int>, Action<ItemMap, int, int>>(itemMap.setPoint, ItemMap_setPoint_hook, ItemMap_setPoint_original);
            TryInstallHook<Action<MyVector, int>, Action<Menu, MyVector, int>>(menu.startAt, Menu_startAt_hook, Menu_startAt_original);
            TryInstallHook<Action<string>, Action<Char, string>>(ch.addInfo, Char_addInfo_hook, Char_addInfo_original);
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
            TryInstallHook<Action, Action<LoginScr>>(loginScr.switchToMe, LoginScr_switchToMe_hook, LoginScr_switchToMe_original);
            TryInstallHook<Action<int, int, mGraphics>, Action<Skill, int, int, mGraphics>>(skill.paint, Skill_paint_hook, Skill_paint_original);
            TryInstallHook<Action<string>, Action<Service, string>>(service.chat, Service_chat_hook, Service_chat_original);
            TryInstallHook<Action<int>, Action<Service, int>>(service.gotoPlayer, Service_gotoPlayer_hook, Service_gotoPlayer_original);
            TryInstallHook<Action, Action<Panel>>(panel.updateKeyInTabBar, Panel_updateKeyInTabBar_hook, Panel_updateKeyInTabBar_original);
            TryInstallHook<Action<mGraphics>, Action<Panel, mGraphics>>(panel.paint, Panel_paint_hook, Panel_paint_original);
            TryInstallHook<Action, Action<Panel>>(panel.update, Panel_update_hook, Panel_update_original);
            TryInstallHook<Action<mGraphics>, Action<GameCanvas, mGraphics>>(gameCanvas.paint, GameCanvas_paint_hook, GameCanvas_paint_original);
            TryInstallHook<Action, Action<Char>>(ch.update, Char_update_hook, Char_update_original);
            TryInstallHook<Action, Action<Char>>(ch.removeHoleEff, Char_removeHoleEff_hook, Char_removeHoleEff_original);
            TryInstallHook<Action<Char>, Action<Char, Char>>(ch.setHoldChar, Char_setHoldChar_hook, Char_setHoldChar_original);
            TryInstallHook<Action<Mob>, Action<Char, Mob>>(ch.setHoldMob, Char_setHoldMob_hook, Char_setHoldMob_original);
            TryInstallHook<Action<mGraphics, bool, Char>, Action<GameScr, mGraphics, bool, Char>>(gameScr.paintImageBar, GameScr_paintImageBar_hook, GameScr_paintImageBar_original);
            TryInstallHook(typeof(BackgroudEffect).GetConstructors()[0], new Action<BackgroudEffect, int>(BackgroundEffect__ctor_hook).Method, new Action<BackgroudEffect, int>(BackgroundEffect__ctor_original).Method);
            TryInstallHook<Action, Action>(BackgroudEffect.initCloud, BackgroudEffect_initCloud_hook, BackgroudEffect_initCloud_original);
            TryInstallHook<Action, Action>(BackgroudEffect.updateCloud2, BackgroudEffect_updateCloud2_hook, BackgroudEffect_updateCloud2_original);
            TryInstallHook<Action, Action>(BackgroudEffect.updateFog, BackgroudEffect_updateFog_hook, BackgroudEffect_updateFog_original);
            TryInstallHook<Action<mGraphics>, Action<mGraphics>>(BackgroudEffect.paintCloud2, BackgroudEffect_paintCloud2_hook, BackgroudEffect_paintCloud2_original);
            TryInstallHook<Action<mGraphics>, Action<mGraphics>>(BackgroudEffect.paintFog, BackgroudEffect_paintFog_hook, BackgroudEffect_paintFog_original);
            TryInstallHook<Action<int>, Action<int>>(BackgroudEffect.addEffect, BackgroudEffect_addEffect_hook, BackgroudEffect_addEffect_original);
            TryInstallHook<Action<mGraphics>, Action<BgItem, mGraphics>>(bgItem.paint, BgItem_paint_hook, BgItem_paint_original);
            TryInstallHook<Action, Action<Char>>(ch.updateSuperEff, Char_updateSuperEff_hook, Char_updateSuperEff_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paint, Char_paint_hook, Char_paint_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paintMount1, Char_paintMount1_hook, Char_paintMount1_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paintMount2, Char_paintMount2_hook, Char_paintMount2_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paintSuperEffFront, Char_paintSuperEffFront_hook, Char_paintSuperEffFront_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paintSuperEffBehind, Char_paintSuperEffBehind_hook, Char_paintSuperEffBehind_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paintAuraFront, Char_paintAuraFront_hook, Char_paintAuraFront_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paintAuraBehind, Char_paintAuraBehind_hook, Char_paintAuraBehind_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paintEffFront, Char_paintEffFront_hook, Char_paintEffFront_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paintEffBehind, Char_paintEffBehind_hook, Char_paintEffBehind_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paintEff_Lvup_front, Char_paintEff_Lvup_front_hook, Char_paintEff_Lvup_front_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paintEff_Lvup_behind, Char_paintEff_Lvup_behind_hook, Char_paintEff_Lvup_behind_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paintEff_Pet, Char_paintEff_Pet_hook, Char_paintEff_Pet_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paintEffect, Char_paintEffect_hook, Char_paintEffect_original);
            TryInstallHook<Action<mGraphics>, Action<Char, mGraphics>>(ch.paint_map_line, Char_paint_map_line_hook, Char_paint_map_line_original);
            TryInstallHook<Action<mGraphics, int, int, int, int, bool>, Action<Char, mGraphics, int, int, int, int, bool>>(ch.paintCharBody, Char_paintCharBody_hook, Char_paintCharBody_original);
            TryInstallHook<Action<mGraphics>, Action<Effect, mGraphics>>(effect.paint, Effect_paint_hook, Effect_paint_original);
            TryInstallHook<Action<mGraphics>, Action<GameScr, mGraphics>>(gameScr.paintEffect, GameScr_paintEffect_hook, GameScr_paintEffect_original);
            TryInstallHook<Action<mGraphics, int>, Action<GameScr, mGraphics, int>>(gameScr.paintBgItem, GameScr_paintBgItem_hook, GameScr_paintBgItem_original);
            TryInstallHook<Action<Image, int, int, int>, Action<mGraphics, Image, int, int, int>>(g.drawImage, mGraphics_drawImage_hook, mGraphics_drawImage_original);
            TryInstallHook<Action<mGraphics>, Action<InfoMe, mGraphics>>(infoMe.paint, InfoMe_paint_hook, InfoMe_paint_original);
            TryInstallHook<Action<mGraphics>, Action<ItemMap, mGraphics>>(itemMap.paint, ItemMap_paint_hook, ItemMap_paint_original);
            TryInstallHook<Action<mGraphics>, Action<MagicTree, mGraphics>>(magicTree.paint, MagicTree_paint_hook, MagicTree_paint_original);
            TryInstallHook<Action<mGraphics>, Action<Mob, mGraphics>>(mob.paint, Mob_paint_hook, Mob_paint_original);
            TryInstallHook<Action<mGraphics>, Action<mGraphics>>(TileMap.paintTilemap, TileMap_paintTilemap_hook, TileMap_paintTilemap_original);
            TryInstallHook<Action<mGraphics>, Action<mGraphics>>(TileMap.paintOutTilemap, TileMap_paintOutTilemap_hook, TileMap_paintOutTilemap_original);
            TryInstallHook<Action<mGraphics>, Action<Npc, mGraphics>>(npc.paint, Npc_paint_hook, Npc_paint_original);
            TryInstallHook<Action<mGraphics>, Action<ServerEffect, mGraphics>>(serverEffect.paint, ServerEffect_paint_hook, ServerEffect_paint_original);
            TryInstallHook<Action, Action<ServerListScreen>>(serverListScreen.initCommand, ServerListScreen_initCommand_hook, ServerListScreen_initCommand_original);
            TryInstallHook<Action, Action<Panel>>(panel.doFireTool, Panel_doFireTool_hook, Panel_doFireTool_original);
            TryInstallHook<Action, Action<SoundMn>>(soundMn.getSoundOption, SoundMn_getSoundOption_hook, SoundMn_getSoundOption_original);
            TryInstallHook<Action, Action<Panel>>(panel.doFirePet, Panel_doFirePet_hook, Panel_doFirePet_original);
            TryInstallHook<Action<Message>, Action<GameScr, Message>>(gameScr.openUIZone, GameScr_openUIZone_hook, GameScr_openUIZone_original);
            TryInstallHook<Action<string>, Action<string>>(GameCanvas.startOKDlg, GameCanvas_startOKDlg_hook, GameCanvas_startOKDlg_original);
            TryInstallHook<Action, Action<Service>>(service.requestChangeMap, Service_requestChangeMap_hook, Service_requestChangeMap_original);
            TryInstallHook<Action, Action<Service>>(service.getMapOffline, Service_getMapOffline_hook, Service_getMapOffline_original);

            //TryInstallHook<Action, Action>(, _hook, _original);
        }

        #region Hooks
        static void Service_getMapOffline_hook(Service _this)
        {
            if (!GameEvents.OnGetMapOffline())
                Service_getMapOffline_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Service_getMapOffline_original(Service _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Service_requestChangeMap_hook(Service _this)
        {
            if (!GameEvents.OnRequestChangeMap())
                Service_requestChangeMap_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Service_requestChangeMap_original(Service _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameCanvas_startOKDlg_hook(string info)
        {
            if (!GameEvents.OnStartOKDlg(info))
                GameCanvas_startOKDlg_original(info);

        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameCanvas_startOKDlg_original(string info)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameScr_openUIZone_hook(GameScr _this, Message message)
        {
            if (!GameEvents.OnOpenUIZone(_this, message))
                GameScr_openUIZone_original(_this, message);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_openUIZone_original(GameScr _this, Message message)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Panel_doFirePet_hook(Panel _this)
        {
            Panel_doFirePet_original(_this);
            if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
            {
                GameCanvas.panel2 = new Panel();
                GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
                GameCanvas.panel2.setTypeBodyOnly();
                GameCanvas.panel2.show();
                GameCanvas.panel.setTypePetMain();
                GameCanvas.panel.show();
            }
            else
            {
                GameCanvas.panel.tabName[21] = mResources.petMainTab;
                GameCanvas.panel.setTypePetMain();
                GameCanvas.panel.show();
            }
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_doFirePet_original(Panel _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void SoundMn_getSoundOption_hook(SoundMn _this)
        {
            if (!GameEvents.OnGetSoundOption())
                SoundMn_getSoundOption_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void SoundMn_getSoundOption_original(SoundMn _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Panel_doFireTool_hook(Panel _this)
        {
            if (!GameEvents.OnPanelFireTool(_this))
                Panel_doFireTool_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_doFireTool_original(Panel _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ServerListScreen_initCommand_hook(ServerListScreen _this)
        {
            if (!GameEvents.OnServerListScreenInitCommand(_this))
                ServerListScreen_initCommand_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ServerListScreen_initCommand_original(ServerListScreen _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ServerEffect_paint_hook(ServerEffect _this, mGraphics g)
        {
            if (!GraphicsReducer.OnServerEffectPaint())
                ServerEffect_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ServerEffect_paint_original(ServerEffect _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Npc_paint_hook(Npc _this, mGraphics g)
        {
            if (!GraphicsReducer.OnNpcPaint(_this, g))
                Npc_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Npc_paint_original(Npc _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void TileMap_paintOutTilemap_hook(mGraphics g)
        {
            if (!GraphicsReducer.OnTileMapPaintOutTile())
                TileMap_paintOutTilemap_original(g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void TileMap_paintOutTilemap_original(mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void TileMap_paintTilemap_hook(mGraphics g)
        {
            if (!GraphicsReducer.OnTileMapPaintTile(g))
                TileMap_paintTilemap_original(g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void TileMap_paintTilemap_original(mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Mob_paint_hook(Mob _this, mGraphics g)
        {
            if (!GraphicsReducer.OnMobPaint(_this, g))
                Mob_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Mob_paint_original(Mob _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void MagicTree_paint_hook(MagicTree _this, mGraphics g)
        {
            if (!GraphicsReducer.OnMagicTreePaint(_this, g))
                MagicTree_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void MagicTree_paint_original(MagicTree _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ItemMap_paint_hook(ItemMap _this, mGraphics g)
        {
            if (!GraphicsReducer.OnItemMapPaint(_this, g))
                ItemMap_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ItemMap_paint_original(ItemMap _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void InfoMe_paint_hook(InfoMe _this, mGraphics g)
        {
            if (!GraphicsReducer.OnInfoMePaint(_this, g))
                InfoMe_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void InfoMe_paint_original(InfoMe _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void mGraphics_drawImage_hook(mGraphics g, Image image, int x, int y, int anchor)
        {
            if (!GameEvents.OnmGraphicsDrawImage(image))
                mGraphics_drawImage_original(g, image, x, y, anchor);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void mGraphics_drawImage_original(mGraphics g, Image image, int x, int y, int anchor)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameScr_paintBgItem_hook(GameScr _this, mGraphics g, int layer)
        {
            if (!GraphicsReducer.OnGameScrPaintBgItem())
                GameScr_paintBgItem_original(_this, g, layer);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_paintBgItem_original(GameScr _this, mGraphics g, int layer)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameScr_paintEffect_hook(GameScr _this, mGraphics g)
        {
            if (!GraphicsReducer.OnGameScrPaintEffect())
                GameScr_paintEffect_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_paintEffect_original(GameScr _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Effect_paint_hook(Effect _this, mGraphics g)
        {
            if (!GraphicsReducer.OnEffectPaint())
                Effect_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Effect_paint_original(Effect _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintCharBody_hook(Char _this, mGraphics g, int cx, int cy, int cdir, int cf, bool isPaintBag)
        {
            if (!GraphicsReducer.OnCharPaintCharBody(_this, g, cx, cy, cdir, isPaintBag))
                Char_paintCharBody_original(_this, g, cx, cy, cdir, cf, isPaintBag);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintCharBody_original(Char _this, mGraphics g, int cx, int cy, int cdir, int cf, bool isPaintBag)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paint_map_line_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintMapLine())
                Char_paint_map_line_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paint_map_line_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintEffect_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintEffect())
                Char_paintEffect_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintEffect_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintEff_Pet_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintEff_Pet())
                Char_paintEff_Pet_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintEff_Pet_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintEff_Lvup_front_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintEff_LvUp_Front())
                Char_paintEff_Lvup_front_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintEff_Lvup_front_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintEff_Lvup_behind_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintEff_LvUp_Behind())
                Char_paintEff_Lvup_behind_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintEff_Lvup_behind_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintEffFront_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintEffFront())
                Char_paintEffFront_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintEffFront_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintEffBehind_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintEffBehind())
                Char_paintEffBehind_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintEffBehind_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintAuraFront_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintAuraFront())
                Char_paintAuraFront_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintAuraFront_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintAuraBehind_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintAuraBehind())
                Char_paintAuraBehind_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintAuraBehind_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintSuperEffFront_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintSuperEffFront())
                Char_paintSuperEffFront_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintSuperEffFront_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintSuperEffBehind_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintSuperEffBehind())
                Char_paintSuperEffBehind_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintSuperEffBehind_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintMount2_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintMount2())
                Char_paintMount2_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintMount2_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paintMount1_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaintMount1(_this, g))
                Char_paintMount1_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paintMount1_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_paint_hook(Char _this, mGraphics g)
        {
            if (!GraphicsReducer.OnCharPaint())
                Char_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_paint_original(Char _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_updateSuperEff_hook(Char _this)
        {
            if (!GraphicsReducer.OnCharUpdateSuperEff())
                Char_updateSuperEff_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_updateSuperEff_original(Char _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void BgItem_paint_hook(BgItem _this, mGraphics g)
        {
            if (!GraphicsReducer.OnBgItemPaint())
                BgItem_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void BgItem_paint_original(BgItem _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void BackgroudEffect_addEffect_hook(int id)
        {
            if (!GraphicsReducer.OnBackgroundEffectAddEffect())
                BackgroudEffect_addEffect_original(id);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void BackgroudEffect_addEffect_original(int id)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void BackgroudEffect_paintFog_hook(mGraphics g)
        {
            if (!GraphicsReducer.OnBackgroundEffectPaintFog())
                BackgroudEffect_paintFog_original(g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void BackgroudEffect_paintFog_original(mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void BackgroudEffect_paintCloud2_hook(mGraphics g)
        {
            if (!GraphicsReducer.OnBackgroundEffectPaintCloud2())
                BackgroudEffect_paintCloud2_original(g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void BackgroudEffect_paintCloud2_original(mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void BackgroudEffect_updateFog_hook()
        {
            if (!GraphicsReducer.OnBackgroundEffectUpdateFog())
                BackgroudEffect_updateFog_original();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void BackgroudEffect_updateFog_original()
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void BackgroudEffect_updateCloud2_hook()
        {
            if (!GraphicsReducer.OnBackgroundEffectUpdateCloud2())
                BackgroudEffect_updateCloud2_original();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void BackgroudEffect_updateCloud2_original()
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void BackgroudEffect_initCloud_hook()
        {
            if (!GraphicsReducer.OnBackgroundEffectInitCloud())
                BackgroudEffect_initCloud_original();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void BackgroudEffect_initCloud_original()
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Service_chat_hook(Service _this, string text)
        {
            if (!GameEvents.OnSendChat(text))
                Service_chat_original(_this, text);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Service_chat_original(Service _this, string text)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Rms_saveRMSString_hook(string filename, string data)
        {
            GameEvents.OnSaveRMSString(ref filename, ref data);
            Rms_saveRMSString_original(filename, data);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Rms_saveRMSString_original(string filename, string data)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static sbyte[] Rms_loadRMS_hook(string filename)
        {
            return Rms.__loadRMS(filename);
        }

        static void Rms_saveRMS_hook(string filename, sbyte[] data)
        {
            Rms.__saveRMS(filename, data);
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
                        if (GameEvents.OnUpdateTouchGameScr(_this))
                            return;
                    }
                }
                if ((!ChatTextField.gI().isShow || GameCanvas.keyAsciiPress == 0) && !_this.isLockKey && !GameCanvas.menu.showMenu && !_this.isOpenUI() && !Char.isLockKey && Char.myCharz().skillPaint == null && GameCanvas.keyAsciiPress != 0 && _this.mobCapcha == null && TField.isQwerty)
                {
                    GameEvents.OnGameScrPressHotkeys();
                    if (!GameCanvas.keyPressed[1] && !GameCanvas.keyPressed[2] && !GameCanvas.keyPressed[3] && !GameCanvas.keyPressed[4] && !GameCanvas.keyPressed[5] && !GameCanvas.keyPressed[6] && !GameCanvas.keyPressed[7] && !GameCanvas.keyPressed[8] && !GameCanvas.keyPressed[9] && !GameCanvas.keyPressed[0] && GameCanvas.keyAsciiPress != 114 && GameCanvas.keyAsciiPress != 47)
                        GameEvents.OnGameScrPressHotkeysUnassigned();
                }
            }
            GameScr_updateKey_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_updateKey_original(GameScr _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ChatTextField_paint_hook(ChatTextField _this, mGraphics g)
        {
            GameEvents.OnPaintChatTextField(_this, g);
            ChatTextField_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatTextField_paint_original(ChatTextField _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ChatTextField_startChat_hook_1(ChatTextField _this, int firstCharacter, IChatable parentScreen, string to)
        {
            if (!GameEvents.OnStartChatTextField(_this, parentScreen))
                ChatTextField_startChat_original_1(_this, firstCharacter, parentScreen, to);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatTextField_startChat_original_1(ChatTextField _this, int firstCharacter, IChatable parentScreen, string to)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ChatTextField_startChat_hook_2(ChatTextField _this, IChatable parentScreen, string to)
        {
            if (!GameEvents.OnStartChatTextField(_this, parentScreen))
                ChatTextField_startChat_original_2(_this, parentScreen, to);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatTextField_startChat_original_2(ChatTextField _this, IChatable parentScreen, string to)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static string Rms_GetiPhoneDocumentsPath_hook()
        {
            if (GameEvents.OnGetRMSPath(out string result))
                return result;
            return Rms_GetiPhoneDocumentsPath_original();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static string Rms_GetiPhoneDocumentsPath_original()
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
            return null;
        }

        static void Teleport_update_hook(Teleport _this)
        {
            GameEvents.OnTeleportUpdate(_this);
            Teleport_update_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Teleport_update_original(Teleport _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ChatTextField_update_hook(ChatTextField _this)
        {
            if (!_this.isShow)
                GameEvents.OnUpdateChatTextField(_this);
            ChatTextField_update_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatTextField_update_original(ChatTextField _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Rms_clearAll_hook()
        {
            if (!GameEvents.OnClearAllRMS())
                Rms_clearAll_original();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Rms_clearAll_original()
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameScr_update_hook(GameScr _this)
        {
            GameEvents.OnUpdateGameScr();
            GameScr_update_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_update_original(GameScr _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Service_login_hook(Service _this, string username, string pass, string version, sbyte type)
        {
            GameEvents.OnLogin(ref username, ref pass, ref type);
            Service_login_original(_this, username, pass, version, type);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Service_login_original(Service _this, string username, string pass, string version, sbyte type)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ServerListScreen_switchToMe_hook(ServerListScreen _this)
        {
            ServerListScreen_switchToMe_original(_this);
            _this.cmd[1 + _this.nCmdPlay].caption = Strings.accounts;
            GameEvents.OnServerListScreenLoaded(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ServerListScreen_switchToMe_original(ServerListScreen _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Session_ME_connect_hook(Session_ME _this, string host, int port)
        {
            GameEvents.OnSessionConnecting(ref host, ref port);
            Session_ME_connect_original(_this, host, port);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Session_ME_connect_original(Session_ME _this, string host, int port)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ServerListScreen_show2_hook(ServerListScreen _this)
        {
            ServerListScreen_show2_original(_this);
            GameEvents.OnScreenDownloadDataShow();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ServerListScreen_show2_original(ServerListScreen _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void MotherCanvas_checkZoomLevel_hook(MotherCanvas _this, int w, int h)
        {
            if (!GameEvents.OnCheckZoomLevel(w, h))
                MotherCanvas_checkZoomLevel_original(_this, w, h);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void MotherCanvas_checkZoomLevel_original(MotherCanvas _this, int w, int h)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameCanvas_keyPressedz_hook(GameCanvas _this, int keyCode)
        {
            if (!GameEvents.OnKeyPressed(keyCode, false))
                GameCanvas_keyPressedz_original(_this, keyCode);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameCanvas_keyPressedz_original(GameCanvas _this, int keyCode)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameCanvas_keyReleasedz_hook(GameCanvas _this, int keyCode)
        {
            if (!GameEvents.OnKeyReleased(keyCode, false))
                GameCanvas_keyReleasedz_original(_this, keyCode);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameCanvas_keyReleasedz_original(GameCanvas _this, int keyCode)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ChatPopup_addChatPopupMultiLine_hook(string chat, int howLong, Npc c)
        {
            if (!GameEvents.OnChatPopupMultiLine(chat))
                ChatPopup_addChatPopupMultiLine_original(chat, howLong, c);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatPopup_addChatPopupMultiLine_original(string chat, int howLong, Npc c)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ChatPopup_addBigMessage_hook(string chat, int howLong, Npc c)
        {
            if (!GameEvents.OnAddBigMessage(chat, c))
                ChatPopup_addChatPopupMultiLine_original(chat, howLong, c);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ChatPopup_addBigMessage_original(string chat, int howLong, Npc c)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Controller_loadInfoMap_hook(Controller _this, Message msg)
        {
            Controller_loadInfoMap_original(_this, msg);
            GameEvents.OnInfoMapLoaded();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Controller_loadInfoMap_original(Controller _this, Message msg)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameScr_paint_hook(GameScr _this, mGraphics g)
        {
            GameScr_paint_original(_this, g);
            GameEvents.OnPaintGameScr(g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_paint_original(GameScr _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_setSkillPaint_hook(Char _this, SkillPaint skillPaint, int sType)
        {
            if (!GameEvents.OnUseSkill(_this))
                Char_setSkillPaint_original(_this, skillPaint, sType);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_setSkillPaint_original(Char _this, SkillPaint skillPaint, int sType)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void InfoMe_addInfo_hook(InfoMe _this, string s, int Type)
        {
            InfoMe_addInfo_original(_this, s, Type);
            GameEvents.OnAddInfoMe(s);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void InfoMe_addInfo_original(InfoMe _this, string s, int Type)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Panel_updateKey_hook(Panel _this)
        {
            if ((_this.chatTField == null || !_this.chatTField.isShow) && GameCanvas.panel.isDoneCombine && !InfoDlg.isShow)
                GameEvents.OnUpdateKeyPanel(_this);
            if ((_this.tabIcon == null || !_this.tabIcon.isShow) && !_this.isClose && _this.isShow && !_this.cmdClose.isPointerPressInside())
                GameEvents.OnUpdateTouchPanel(_this);
            Panel_updateKey_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_updateKey_original(Panel _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ItemMap_setPoint_hook(ItemMap _this, int xEnd, int yEnd)
        {
            GameEvents.OnSetPointItemMap(xEnd, yEnd);
            ItemMap_setPoint_original(_this, xEnd, yEnd);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ItemMap_setPoint_original(ItemMap _this, int xEnd, int yEnd)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Menu_startAt_hook(Menu _this, MyVector menuItems, int pos)
        {
            if (!GameEvents.OnMenuStartAt(menuItems))
                Menu_startAt_original(_this, menuItems, pos);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Menu_startAt_original(Menu _this, MyVector menuItems, int pos)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_addInfo_hook(Char _this, string info)
        {
            GameEvents.OnAddInfoChar(_this, info);
            Char_addInfo_original(_this, info);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_addInfo_original(Char _this, string info)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameCanvas_paintBGGameScr_hook(mGraphics g)
        {
            if (!GameEvents.OnPaintBgGameScr(g))
                GameCanvas_paintBGGameScr_original(g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameCanvas_paintBGGameScr_original(mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Mob_startDie_hook(Mob _this)
        {
            GameEvents.OnMobStartDie(_this);
            Mob_startDie_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Mob_startDie_original(Mob _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Mob_update_hook(Mob _this)
        {
            GameEvents.OnUpdateMob(_this);
            Mob_update_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Mob_update_original(Mob _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static Image Image_createImage_hook(string filename)
        {
            if (GameEvents.OnCreateImage(filename, out Image image))
                return image;
            return Image_createImage_original(filename);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static Image Image_createImage_original(string filename)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
            return null;
        }

        static void GameScr_chatVip_hook(GameScr _this, string chatVip)
        {
            GameEvents.OnChatVip(chatVip);
            GameScr_chatVip_original(_this, chatVip);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_chatVip_original(GameScr _this, string chatVip)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Panel_updateScroolMouse_hook(Panel _this, int a)
        {
            if (!GameEvents.OnUpdateScrollMousePanel(_this, ref a)) ;
            Panel_updateScroolMouse_original(_this, a);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_updateScroolMouse_original(Panel _this, int a)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Panel_hide_hook(Panel _this)
        {
            if (_this.timeShow <= 0)
                GameEvents.OnPanelHide(_this);
            Panel_hide_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_hide_original(Panel _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Panel_hideNow_hook(Panel _this)
        {
            if (_this.timeShow <= 0)
                GameEvents.OnPanelHide(_this);
            Panel_hideNow_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_hideNow_original(Panel _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameScr_paintTouchControl_hook(GameScr _this, mGraphics g)
        {
            if (!GameEvents.OnPaintTouchControl(_this, g))
                GameScr_paintTouchControl_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_paintTouchControl_original(GameScr _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameScr_paintGamePad_hook(GameScr _this, mGraphics g)
        {
            if (!GameEvents.OnPaintGamePad(g))
                GameScr_paintGamePad_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_paintGamePad_original(GameScr _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void SoundMn_getStrOption_hook(SoundMn _this)
        {
            if (!GameEvents.OnSoundMnGetStrOption())
                SoundMn_getStrOption_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void SoundMn_getStrOption_original(SoundMn _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Panel_doFireOption_hook(Panel _this)
        {
            if (!GameEvents.OnPanelFireOption(_this))
                Panel_doFireOption_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_doFireOption_original(Panel _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GamePad_paint_hook(GamePad _this, mGraphics g)
        {
            if (!GameEvents.OnGamepadPaint(_this, g))
                GamePad_paint_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GamePad_paint_original(GamePad _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GamePad__ctor_hook(GamePad _this)
        {
            GamePad__ctor_original(_this);
            _this.SetGamePadZone();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GamePad__ctor_original(GamePad _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameScr_setSkillBarPosition_hook()
        {
            if (!GameEvents.OnSetSkillBarPosition())
                GameScr_setSkillBarPosition_original();
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_setSkillBarPosition_original()
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameScr_paintSelectedSkill_hook(GameScr _this, mGraphics g)
        {
            if (Char.myCharz().IsCharDead())
                return;
            GameEvents.OnGameScrPaintSelectedSkill(_this, g);
            GameScr_paintSelectedSkill_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_paintSelectedSkill_original(GameScr _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Panel_paintToolInfo_hook(Panel _this, mGraphics g)
        {
            if (!GameEvents.OnPanelPaintToolInfo(g))
                Panel_paintToolInfo_original(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_paintToolInfo_original(Panel _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void mResources_loadLanguague_hook(sbyte newLanguage)
        {
            mResources_loadLanguague_original(newLanguage);
            GameEvents.OnLoadLanguage(newLanguage);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void mResources_loadLanguague_original(sbyte newLanguage)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void ServerListScreen_saveIP_hook()
        {
            //don't save IP
            if (Rms.loadRMSInt("svselect") == -1)
                Rms.saveRMSInt("svselect", ServerListScreen.ipSelect);
            SplashScr.loadIP();
        }

        static void ServerListScreen_loadIP_hook()
        {
            GameEvents.OnLoadIP();
        }

        static void ServerListScreen_getServerList_hook(string obj)
        {
            ServerListScreen_getServerList_original(Strings.DEFAULT_IP_SERVERS);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void ServerListScreen_getServerList_original(string obj)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void LoginScr_switchToMe_hook(LoginScr _this)
        {
            LoginScr_switchToMe_original(_this);
            _this.tfUser.setText(Rms.loadRMSString("acc"));
            _this.tfPass.setText(Rms.loadRMSString("pass"));
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void LoginScr_switchToMe_original(LoginScr _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Skill_paint_hook(Skill _this, int x, int y, mGraphics g)
        {
            if (!GameEvents.OnSkillPaint(_this, x, y, g))
                Skill_paint_original(_this, x, y, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Skill_paint_original(Skill _this, int x, int y, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Service_gotoPlayer_hook(Service _this, int id)
        {
            if (!GameEvents.OnGotoPlayer(id))
                Service_gotoPlayer_original(_this, id);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        internal static void Service_gotoPlayer_original(Service _this, int id)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Panel_updateKeyInTabBar_hook(Panel _this)
        {
            if (!GameEvents.OnPanelUpdateKeyInTabBar(_this))
                Panel_updateKeyInTabBar_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_updateKeyInTabBar_original(Panel _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Panel_paint_hook(Panel _this, mGraphics g)
        {
            if (!GameEvents.OnPaintPanel(_this, g))
            {
                Panel_paint_original(_this, g);
                GameEvents.OnAfterPaintPanel(_this, g);
                GameScr.resetTranslate(g);
                _this.paintDetail(g);
                if (_this.cmx == _this.cmtoX && !GameCanvas.menu.showMenu)
                    _this.cmdClose.paint(g);
                if (_this.tabIcon != null && _this.tabIcon.isShow)
                    _this.tabIcon.paint(g);
                g.translate(-g.getTranslateX(), -g.getTranslateY());
                g.translate(_this.X, _this.Y);
                g.translate(-_this.cmx, 0);
            }
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_paint_original(Panel _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Panel_update_hook(Panel _this)
        {
            if (!GameEvents.OnUpdatePanel(_this))
                Panel_update_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel_update_original(Panel _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameCanvas_paint_hook(GameCanvas _this, mGraphics g)
        {
            GameCanvas_paint_original(_this, g);
            GameEvents.OnPaintGameCanvas(_this, g);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameCanvas_paint_original(GameCanvas _this, mGraphics g)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_setHoldMob_hook(Char _this, Mob r)
        {
            Char_setHoldMob_original(_this, r);
            GameEvents.OnCharSetHoldMob(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_setHoldMob_original(Char _this, Mob r)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_setHoldChar_hook(Char _this, Char r)
        {
            Char_setHoldChar_original(_this, r);
            GameEvents.OnCharSetHoldChar(_this, r);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_setHoldChar_original(Char _this, Char r)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_removeHoleEff_hook(Char _this)
        {
            Char_removeHoleEff_original(_this);
            GameEvents.OnCharRemoveHoldEff(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_removeHoleEff_original(Char _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Char_update_hook(Char _this)
        {
            GameEvents.OnUpdateChar(_this);
            Char_update_original(_this);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Char_update_original(Char _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void GameScr_paintImageBar_hook(GameScr _this, mGraphics g, bool isLeft, Char c)
        {
            GameScr_paintImageBar_original(_this, g, isLeft, c);
            GameEvents.OnPaintImageBar(g, isLeft, c);
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void GameScr_paintImageBar_original(GameScr _this, mGraphics g, bool isLeft, Char c)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void Panel_ctor_hook(Panel _this)
        {
            Panel__ctor_original(_this);
            _this.tabName = new string[28][][]
            {
                null,
                null,
                new string[2][]
                {
                    mResources.chestt,
                    mResources.inventory
                },
                new string[1][] { mResources.zonee },
                new string[1][] { mResources.mapp },
                null,
                null,
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                new string[2][]
                {
                    mResources.combine,
                    mResources.inventory
                },
                new string[3][]
                {
                    mResources.inventory,
                    mResources.item_give,
                    mResources.item_receive
                },
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                mResources.petMainTab,
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                new string[1][] { new string[1] { string.Empty } },
                // thêm 1 phần tử cho CustomPanelMenu
                new string[1][] { new string[1] { string.Empty } },
            };
            _this.lastTabIndex = new int[_this.tabName.Length];
            for (int i = 0; i < _this.lastTabIndex.Length; i++)
                _this.lastTabIndex[i] = -1;
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void Panel__ctor_original(Panel _this)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }

        static void BackgroundEffect__ctor_hook(BackgroudEffect _this, int typeS)
        {
            BackgroundEffect__ctor_original(_this, typeS);
            if (typeS == 0 || typeS == 12)
            {
                _this.sum = Res.random(10, 20); //limit rain effect
                _this.x = new int[_this.sum];
                _this.y = new int[_this.sum];
                _this.vx = new int[_this.sum];
                _this.vy = new int[_this.sum];
                _this.type = new int[_this.sum];
                _this.t = new int[_this.sum];
                _this.frame = new int[_this.sum];
                _this.isRainEffect = new bool[_this.sum];
                _this.activeEff = new bool[_this.sum];
                for (int k = 0; k < _this.sum; k++)
                {
                    _this.y[k] = Res.random(-10, GameCanvas.h + 100) + GameScr.cmy;
                    _this.x[k] = Res.random(-10, GameCanvas.w + 300) + GameScr.cmx;
                    _this.t[k] = Res.random(0, 1);
                    _this.vx[k] = -12;
                    _this.vy[k] = 12;
                    _this.type[k] = Res.random(1, 3);
                    _this.isRainEffect[k] = false;
                    if (_this.type[k] == 2 && k % 2 == 0)
                        _this.isRainEffect[k] = true;
                    _this.activeEff[k] = false;
                    _this.frame[k] = Res.random(1, 2);
                }
            }
        }
        [MethodImpl(MethodImplOptions.NoOptimization)]
        static void BackgroundEffect__ctor_original(BackgroudEffect _this, int typeS)
        {
            Debug.LogError("If you see this line of text in your log file, it means your hook is not installed, cannot be installed, or is installed incorrectly!");
        }
        #endregion

        internal static void InstallAllHooks()
        {
#if UNITY_EDITOR 
            Debug.Log("Assembly-CSharp location: " + typeof(GameEventHook).Assembly.Location);
            CreateDynamicInstallAllMethod().CreateDelegate(typeof(Action)).DynamicInvoke();
#else
            InstallAll();
#endif
        }

        #region Hook methods
#if UNITY_EDITOR
        /// <summary>
        /// Tạo 1 hàm InstallAllDynamic động từ hàm <see cref="InstallAll"/> gốc với mã IL đã tối ưu.
        /// </summary>
        /// <returns><see cref="DynamicMethod"/> đã được tạo.</returns>
        /// <remarks>
        /// Mã IL được tạo sẽ tương tự như sau:
        /// <code>
        /// ldtoken     [target method]
        /// call        class System.Reflection.MethodBase System.Reflection.MethodBase::GetMethodFromHandle(valuetype System.RuntimeMethodHandle)
        /// ldtoken     [hook method]
        /// call        class System.Reflection.MethodBase System.Reflection.MethodBase::GetMethodFromHandle(valuetype System.RuntimeMethodHandle)
        /// ldtoken     [trampoline method]
        /// call        class System.Reflection.MethodBase System.Reflection.MethodBase::GetMethodFromHandle(valuetype System.RuntimeMethodHandle)
        /// call        void Mod.GameEventHook::TryInstallHook(class System.Reflection.MethodBase, class System.Reflection.MethodBase, class System.Reflection.MethodBase)
        /// ...
        /// ret
        /// </code>
        /// </remarks>
        static DynamicMethod CreateDynamicInstallAllMethod()
        {
            AssemblyDefinition assemblyCSharp_d = AssemblyDefinition.ReadAssembly(typeof(GameEventHook).Assembly.Location);
            TypeDefinition gameEventHook_d = assemblyCSharp_d.MainModule.GetType(typeof(GameEventHook).FullName);
            MethodDefinition installAll_d = gameEventHook_d.Methods.First(m => m.Name == nameof(InstallAll));

            //TryInstallHook<T1, T2>(T1 hookTargetMethod, T2 hookMethod, T2 originalProxyMethod)
            MethodDefinition tryInstallHookGeneric_withTrampoline_d = gameEventHook_d.Methods.First(m => m.Name == nameof(TryInstallHook) && m.GenericParameters.Count == 2 && m.Parameters.Count == 3);
            //TryInstallHook<T1, T2>(T1 hookTargetMethod, T2 hookMethod)
            MethodDefinition tryInstallHookGeneric_d = gameEventHook_d.Methods.First(m => m.Name == nameof(TryInstallHook) && m.GenericParameters.Count == 2 && m.Parameters.Count == 2);

            //TryInstallHook(MethodBase hookTargetMethod, MethodBase hookMethod, MethodBase originalProxyMethod)
            MethodDefinition tryInstallHook_d = gameEventHook_d.Methods.First(m => m.Name == nameof(TryInstallHook) && m.GenericParameters.Count == 0);

            //----------------------------------------------------------------------------------------------

            //TryInstallHook(MethodBase hookTargetMethod, MethodBase hookMethod, MethodBase originalProxyMethod)
            MethodInfo tryInstallHook = new Action<MethodBase, MethodBase, MethodBase>(TryInstallHook).Method;

            Module assemblyCSharp = typeof(GameEventHook).Module;
            MethodInfo getMethodFromHandle = new Func<RuntimeMethodHandle, MethodBase>(MethodBase.GetMethodFromHandle).Method;
            DynamicMethod installAllDynamic = new DynamicMethod("InstallAllDynamic", typeof(void), new Type[0], typeof(GameEventHook), true);
            ILGenerator il = installAllDynamic.GetILGenerator();

            int paramCount = 0;
            for (int i = 0; i < installAll_d.Body.Instructions.Count; i++)
            {
                Instruction instruction = installAll_d.Body.Instructions[i];
                if (instruction.OpCode == OpCodes.Ldftn)
                {
                    //ldftn     <target/hook/trampoline method>
                    if (instruction.Operand is MethodDefinition methodDef)
                    {
                        MethodInfo method = (MethodInfo)assemblyCSharp.ResolveMethod(methodDef.MetadataToken.ToInt32());
                        il.Emit(ROpCodes.Ldtoken, method);
                        il.Emit(ROpCodes.Call, getMethodFromHandle);
                        //MethodBase.GetMethodFromHandle(methodof(<some method here>).MethodHandle
                        paramCount++;
                    }
                }
                else if (instruction.OpCode == OpCodes.Ldvirtftn)
                {
                    //ldvirtftn     <target method (virtual)>
                    if (i >= 2 && instruction.Operand is MethodDefinition methodDef)
                    {
                        Instruction instruction1 = installAll_d.Body.Instructions[i - 1];
                        Instruction instruction2 = installAll_d.Body.Instructions[i - 2];
                        if (instruction1.OpCode == OpCodes.Dup && instruction2.IsLdloc())
                        {
                            VariableDefinition local = installAll_d.Body.Variables[instruction2.GetLdlocIndex()];
                            MethodInfo method = (MethodInfo)assemblyCSharp.ResolveMethod(local.VariableType.Resolve().Methods.First(m => m.Name == methodDef.Name).MetadataToken.ToInt32());
                            il.Emit(ROpCodes.Ldtoken, method);
                            il.Emit(ROpCodes.Call, getMethodFromHandle);
                            //MethodBase.GetMethodFromHandle(methodof(<some method here>).MethodHandle
                            paramCount++;
                        }
                    }
                }
                else if (instruction.OpCode == OpCodes.Ldtoken)
                {
                    //ldtoken       <some type>
                    if (i + 4 < installAll_d.Body.Instructions.Count && instruction.Operand is TypeDefinition typeToGetConstructor)
                    {
                        Instruction instruction2 = installAll_d.Body.Instructions[i + 1];
                        Instruction instruction3 = installAll_d.Body.Instructions[i + 2];
                        Instruction instruction4 = installAll_d.Body.Instructions[i + 3];
                        Instruction instruction5 = installAll_d.Body.Instructions[i + 4];
                        if (instruction2.OpCode == OpCodes.Call && instruction2.Operand is MethodReference getTypeFromHandle && getTypeFromHandle.Name == nameof(Type.GetTypeFromHandle))
                        {
                            //typeof(<some type>).GetConstructor(new Type[<some number>] {typeof(...), ... })
                            if (instruction3.IsLdcI4() &&
                                instruction4.OpCode == OpCodes.Newarr && instruction4.Operand is TypeReference typeReference2 && typeReference2.Name == nameof(Type))
                            {
                                //ldc.i4     <number of types>
                                //newarr     [System.Type]
                                //...
                                //dup
                                //ldc.i4     <index>
                                //ldtoken    <some type>
                                //call       class System.Type System.Type::GetTypeFromHandle(valuetype System.RuntimeTypeHandle)
                                //stelem.ref
                                //...
                                //call       class System.Reflection.ConstructorInfo System.Type::GetConstructors(class System.Type[])
                                int typesCount = instruction3.GetLdcI4Value();
                                TypeReference[] paramTypes = new TypeReference[typesCount];
                                for (int j = 0; j < typesCount; j++)
                                {
                                    int index = installAll_d.Body.Instructions[i + 5 * (j + 1)].GetLdcI4Value();
                                    Instruction ldTokenTypeParam = installAll_d.Body.Instructions[i + 5 * (j + 1) + 1];
                                    if (ldTokenTypeParam.Operand is TypeReference typeRefParam)
                                        paramTypes[index] = typeRefParam;
                                }
                                ConstructorInfo _ctor = (ConstructorInfo)assemblyCSharp.ResolveMethod(typeToGetConstructor.Methods.First(m => m.IsConstructor && m.Parameters.Select(p => p.ParameterType.FullName).SequenceEqual(paramTypes.Select(p => p.FullName))).MetadataToken.ToInt32());
                                il.Emit(ROpCodes.Ldtoken, _ctor);
                                il.Emit(ROpCodes.Call, getMethodFromHandle);
                                //MethodBase.GetMethodFromHandle(methodof(<constructor (.ctor) with x parameters>).MethodHandle
                                paramCount++;
                            }
                            //typeof(<some type>).GetConstructors()[0]
                            else if (instruction3.OpCode == OpCodes.Call && instruction3.Operand is MethodReference getConstructors && getConstructors.Name == nameof(Type.GetConstructors) &&
                                instruction4.OpCode == OpCodes.Ldc_I4_0 &&
                                instruction5.OpCode == OpCodes.Ldelem_Ref)
                            {
                                ConstructorInfo _ctor = (ConstructorInfo)assemblyCSharp.ResolveMethod(typeToGetConstructor.Methods.First(m => m.IsConstructor).MetadataToken.ToInt32());
                                il.Emit(ROpCodes.Ldtoken, _ctor);
                                il.Emit(ROpCodes.Call, getMethodFromHandle);
                                //MethodBase.GetMethodFromHandle(methodof(<constructor (.ctor)>).MethodHandle
                                paramCount++;
                            }
                        }
                    }
                }
                else if (instruction.OpCode == OpCodes.Call)
                {
                    if (instruction.Operand is MethodSpecification methodSpec)
                    {
                        //call      TryInstallHook<T1, T2>([target method], [hook method], [trampoline method])
                        if (methodSpec.Resolve() == tryInstallHookGeneric_withTrampoline_d && paramCount == 3)
                        {
                            il.Emit(ROpCodes.Call, tryInstallHook);
                            paramCount = 0;
                        }
                        //call      TryInstallHook<T1, T2>([target method], [hook method])
                        else if (methodSpec.Resolve() == tryInstallHookGeneric_d && paramCount == 2)
                        {
                            while (paramCount < 3)
                            {
                                il.Emit(ROpCodes.Ldnull);
                                paramCount++;
                            }
                            il.Emit(ROpCodes.Call, tryInstallHook);
                            paramCount = 0;
                        }
                    }
                    //call      TryInstallHook(.....)
                    else if (instruction.Operand is MethodDefinition methodDefinition && methodDefinition == tryInstallHook_d)
                    {
                        //trampoline method can be null
                        while (paramCount < 3)
                        {
                            il.Emit(ROpCodes.Ldnull);
                            paramCount++;
                        }
                        il.Emit(ROpCodes.Call, tryInstallHook);
                        paramCount = 0;
                    }
                }
                //The final result will be:
                //GameEventHook.TryInstallHook(MethodBase.GetMethodFromHandle(methodof(<some method/constructor (target)>).MethodHandle), MethodBase.GetMethodFromHandle(methodof(<some method/constructor (hook)>).MethodHandle), MethodBase.GetMethodFromHandle(methodof(<some method/constructor (trampoline)>).MethodHandle));
            }
            il.Emit(ROpCodes.Ret);
            return installAllDynamic;
        }
#endif

        /// <summary>
        /// Thử cài đặt 1 hook.
        /// </summary>
        /// <typeparam name="T1">Loại <see cref="Delegate"/> của <paramref name="hookTargetMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <typeparam name="T2">Loại <see cref="Delegate"/> của <paramref name="hookMethod"/> và <paramref name="originalProxyMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <param name="hookTargetMethod">Hàm để hook vào.</param>
        /// <param name="hookMethod">Hàm mới được gọi thay thế hàm bị hook.</param>
        static void TryInstallHook<T1, T2>(T1 hookTargetMethod, T2 hookMethod) where T1 : Delegate where T2 : Delegate => TryInstallHook(hookTargetMethod.Method, hookMethod.Method, null);

        /// <summary>
        /// Thử cài đặt 1 hook.
        /// </summary>
        /// <typeparam name="T1">Loại <see cref="Delegate"/> của <paramref name="hookTargetMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <typeparam name="T2">Loại <see cref="Delegate"/> của <paramref name="hookMethod"/> và <paramref name="originalProxyMethod"/>, là <see cref="Action"/> nếu hàm không trả về giá trị và <see cref="Func{TResult}"/> nếu hàm trả về giá trị.</typeparam>
        /// <param name="hookTargetMethod">Hàm để hook vào.</param>
        /// <param name="hookMethod">Hàm mới được gọi thay thế hàm bị hook.</param>
        /// <param name="originalProxyMethod">Hàm có cùng signature với hàm gốc, có chức năng làm hàm thay thế để gọi đến hàm gốc mà không bị thay thế (hàm trampoline). Hàm này phải có attribute <code>[MethodImpl(MethodImplOptions.NoOptimization)]</code> để tránh bị tối ưu hóa mất khi biên dịch.</param>
        static void TryInstallHook<T1, T2>(T1 hookTargetMethod, T2 hookMethod, T2 originalProxyMethod) where T1 : Delegate where T2 : Delegate => TryInstallHook(hookTargetMethod.Method, hookMethod.Method, originalProxyMethod.Method);

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
            MethodHook methodHook = HookPool.GetHook(hookTargetMethod);
            if (methodHook != null)
                throw new Exception($"Hook already installed: [{methodHook.targetMethod}, {methodHook.replacementMethod}, {methodHook.proxyMethod}] >< [{hookTargetMethod}, {hookMethod}, {originalProxyMethod}]");
            Debug.Log($"Hooking {hookTargetMethod.Name} to {hookMethod.Name}...");
            MethodHook hook = new MethodHook(hookTargetMethod, hookMethod, originalProxyMethod);
            hook.Install();
        }

        /// <summary>
        /// Gỡ bỏ tất cả hook.
        /// </summary>
        internal static void UninstallAll()
        {
            HookPool.UninstallAll();
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
            MethodHook hook = HookPool.GetHook(hookTargetMethod);
            hook?.Uninstall();
        }
        #endregion
    }
}