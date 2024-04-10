using Mod.Auto;
using Mod.CustomPanel;
using Mod.Graphics;
using Mod.PickMob;
using Mod.R;
using Mod.Set;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mod.ModMenu
{
    internal class ModMenuMain : IChatable, IActionListener
    {
        static ModMenuMain instance = new ModMenuMain();
        internal static ModMenuMain gI() => instance;
        internal static Panel currentPanel
        {
            get => GameCanvas.panel2;
            set => GameCanvas.panel2 = value;
        }

        internal static ModMenuItemBoolean[] modMenuItemBools;
        internal static ModMenuItemValues[] modMenuItemValues;
        internal static ModMenuItemFunction[] modMenuItemFunctions;

        static sbyte lastLanguage = -1;

        internal static Dictionary<int, string[]> inputModMenuItemInts = new Dictionary<int, string[]>()
        {
            { 0, new string[]{ "Nhập mức FPS", "FPS" } },
            { 6, new string[]{ "Nhập thời gian thay đổi hình nền", "Thời gian (giây)" } },
            { 7, new string[]{ "Nhập thời gian thay đổi logo", "Thời gian (giây)" } },
            { 8, new string[]{ "Nhập chiều cao logo", "Chiều cao logo" } },
        };

        internal static Command cmdOpenModMenu;

        internal static void Initialize()
        {
            if (cmdOpenModMenu == null)
            {
                cmdOpenModMenu = new Command("", instance, 1, null);
                cmdOpenModMenu.img = new Image();
                cmdOpenModMenu.img.texture = CustomGraphics.FlipTextureHorizontally(GameScr.imgMenu.texture);
                cmdOpenModMenu.img.w = cmdOpenModMenu.img.texture.width;
                cmdOpenModMenu.img.h = cmdOpenModMenu.img.texture.height;
                cmdOpenModMenu.isPlaySoundButton = false;
                cmdOpenModMenu.w = cmdOpenModMenu.img.w / mGraphics.zoomLevel;
                cmdOpenModMenu.h = cmdOpenModMenu.img.h / mGraphics.zoomLevel;
                UpdatePosition();
                LoadData();
            }
        }

        internal static void UpdatePosition()
        {
            if (cmdOpenModMenu == null)
                return;
            cmdOpenModMenu.x = GameCanvas.w - cmdOpenModMenu.w;
            //cmdOpenModMenu.y = GameCanvas.h / 2 - cmdOpenModMenu.h / 2;
            cmdOpenModMenu.y = (int)(mGraphics.getImageHeight(GameScr.imgChat) * 1.5f);
            if (currentPanel != null && currentPanel == GameCanvas.panel2 && currentPanel.type == CustomPanelMenu.TYPE_CUSTOM_PANEL_MENU)
            {
                currentPanel.cmdClose.x = GameCanvas.w - currentPanel.cmdClose.img.getWidth() - 1;
                currentPanel.cmdClose.y = 1;
            }
        }

        internal static void UpdateLanguage(sbyte newLanguage)
        {
            if (newLanguage == lastLanguage)
                return;
            lastLanguage = newLanguage;
            modMenuItemBools = new ModMenuItemBoolean[]
            {
                new ModMenuItemBoolean(
                    "VSync_Toggle",
                    "VSync",
                    Strings.vSyncDescription,
                    () => QualitySettings.vSyncCount == 1,
                    value => QualitySettings.vSyncCount = value ? 1 : 0,
                    "isvsync"),
                new ModMenuItemBoolean(
                    "ShowTargetInfo_Toggle",
                    Strings.showTargetInfoTitle,
                    Strings.showTargetInfoDescription,
                    () => CharEffect.isEnabled,
                    CharEffect.setState,
                    "isshowinfochar"),
                new ModMenuItemBoolean(
                    "AutoSendAttack_Toggle",
                    Strings.autoAttack,
                    Strings.autoSendAttackDescription,
                    () => AutoSendAttack.gI.IsActing,
                    AutoSendAttack.toggle),
                new ModMenuItemBoolean(
                    "ShowCharList_Toggle",
                    Strings.showCharListTitle,
                    Strings.showCharListDescription,
                    () => ListCharsInMap.isEnabled,
                    ListCharsInMap.setState,
                    "isshowlistchar"),
                new ModMenuItemBoolean(
                    "ShowPetInCharList_Toogle",
                    Strings.showPetInCharListTitle,
                    Strings.showPetInCharListDescription,
                    () => ListCharsInMap.isShowPet,
                    ListCharsInMap.setStatePet,
                    "isshowlistpet",
                    () => !ListCharsInMap.isEnabled,
                    () => string.Format(Strings.functionShouldBeEnabled, Strings.showCharListTitle)),
                new ModMenuItemBoolean(
                    "AutoTrainForNewbie_Toggle",
                    Strings.autoTrainForNewbieTitle,
                    Strings.autoTrainForNewbieDescription,
                    () => AutoTrainNewAccount.isEnabled,
                    AutoTrainNewAccount.setState,
                    "",
                    () => Char.myCharz().taskMaint == null || Char.myCharz().taskMaint.taskId > 11,
                    () => Strings.noLongerNewAccount + '!'),
                //new ModMenuItemBoolean("Hiện khoảng cách bom", "Hiển thị người, quái, boss... trong tầm bom", SuicideRange.setState, false, "isshowsuiciderange")
                new ModMenuItemBoolean(
                    "CustomBg_Toggle",
                    Strings.customBackgroundTitle,
                    Strings.customBackgroundDescription,
                    () => CustomBackground.isEnabled,
                    CustomBackground.setState,
                    "iscustombackground",
                    () => GraphicsReducer.level > ReduceGraphicsLevel.None,
                    () => string.Format(Strings.functionShouldBeDisabled, Strings.setReduceGraphicsTitle)),
                //new ModMenuItemBoolean("Logo tùy chỉnh", "Bật/tắt hiển thị logo tùy chỉnh trên màn hình game", CustomLogo.setState, false, "isshowlogo"),
                new ModMenuItemBoolean(
                    "NotifyBoss_Toggle",
                    Strings.notifyBossTitle,
                    Strings.notifyBossDescription,
                    () => Boss.isEnabled,
                    Boss.setState,
                    "sanboss"),
                //new ModMenuItemBoolean("EHVN_CustomCursor_Toggle", "Con trỏ tùy chỉnh", "Thay con trỏ chuột mặc định thành con trỏ chuột tùy chỉnh", CustomCursor.setState, false, "customcusor"),
                // Slaughter
                new ModMenuItemBoolean(
                    "PickMob_Toggle",
                    Strings.pickMobTitle,
                    Strings.pickMobDescription,
                    () => Pk9rPickMob.IsTanSat,
                    value => Pk9rPickMob.IsTanSat = value,
                    "",
                    () => AutoTrainNewAccount.isEnabled,
                    () => string.Format(Strings.functionShouldBeDisabled, Strings.autoTrainForNewbieTitle)),
                new ModMenuItemBoolean(
                    "PickMob_AvoidSuperMob_Toogle",
                    Strings.avoidSuperMobTitle,
                    Strings.avoidSuperMobDescription,
                    () => Pk9rPickMob.IsNeSieuQuai,
                    value => Pk9rPickMob.IsNeSieuQuai = value,
                    "isnesieuquaits"),
                new ModMenuItemBoolean(
                    "PickMob_VDH_Toggle",
                    Strings.vdhTitle,
                    Strings.vdhDescription,
                    () => Pk9rPickMob.IsVuotDiaHinh,
                    value => Pk9rPickMob.IsVuotDiaHinh = value,
                    "isvuotdiahinh"),
                new ModMenuItemBoolean(
                    "PickMob_AutoPickItem_Toogle",
                    Strings.autoPickItemTitle,
                    Strings.autoPickItemDescription,
                    () => Pk9rPickMob.IsAutoPickItems,
                    value => Pk9rPickMob.IsAutoPickItems = value,
                    "isautopick",
                    () => AutoTrainNewAccount.isEnabled,
                    () => string.Format(Strings.functionShouldBeDisabled, Strings.autoTrainForNewbieTitle)),
                new ModMenuItemBoolean(
                    "PickMob_PickMyItemOnly_Toogle",
                    Strings.pickMyItemOnlyTitle,
                    Strings.pickMyItemOnlyDescription,
                    () => Pk9rPickMob.IsItemMe,
                    value => Pk9rPickMob.IsItemMe = value,
                    "ispickmyitemonly"),
                new ModMenuItemBoolean(
                    "PickMob_LimitPickTimes_Toogle",
                    Strings.limitPickTimesTitle,
                    Strings.limitPickTimesDescription,
                    () => Pk9rPickMob.IsLimitTimesPickItem,
                    value => Pk9rPickMob.IsLimitTimesPickItem = value,
                    "islimitpicktimes"),
                // Auto Pean
                new ModMenuItemBoolean(
                    "AutoAskForPeans_Toggle",
                    Strings.autoAskForPeansTitle,
                    Strings.autoAskForPeansDescription,
                    () => AutoPean.isAutoRequest,
                    value => AutoPean.isAutoRequest = value,
                    "autoaskforpeans",
                    () => Char.myCharz().clan == null,
                    () => Strings.youAreNotInAClan + '!'),
                new ModMenuItemBoolean(
                    "AutoDonatePeans_Toggle",
                    Strings.autoDonatePeansTitle,
                    Strings.autoDonatePeansDescription,
                    () => AutoPean.isAutoDonate,
                    value => AutoPean.isAutoDonate = value,
                    "autodonatepeans",
                    () => Char.myCharz().clan == null,
                    () => Strings.youAreNotInAClan + '!'),
                new ModMenuItemBoolean(
                    "AutoHarvestPeans_Toggle",
                    Strings.autoHarvestPeansTitle,
                    Strings.autoHarvestPeansDescription,
                    () => AutoPean.isAutoHarvest,
                    value => AutoPean.isAutoHarvest = value,
                    "autoharvestpeans")
            };
            modMenuItemValues = new ModMenuItemValues[]
            {
                new ModMenuItemValues(
                    "Set_FPS",
                    "FPS",
                    () => Application.targetFrameRate,
                    value =>
                    {
                        if (value > 5 && value <= Screen.currentResolution.refreshRateRatio.value)
                            Application.targetFrameRate = value;
                    },
                    null,
                    Strings.setFPSDescription,
                    "targetfps",
                    () => QualitySettings.vSyncCount == 1,
                    () => string.Format(Strings.functionShouldBeDisabled, "VSync")),
                new ModMenuItemValues(
                    "Set_ReduceGraphics",
                    Strings.setReduceGraphicsTitle,
                    () => (int)GraphicsReducer.level,
                    level => GraphicsReducer.level = (ReduceGraphicsLevel)level,
                    Strings.setReduceGraphicsChoices,
                    "",
                    "levelreducegraphics"),
                new ModMenuItemValues(
                    "Set_GoBack",
                    "GoBack",
                    () => (int)AutoGoback.mode,
                    AutoGoback.setState,
                    Strings.setGoBackChoices,
                    "",
                    "",
                    () => AutoTrainNewAccount.isEnabled,
                    () => string.Format(Strings.functionShouldBeDisabled, Strings.autoTrainForNewbieTitle)),
                new ModMenuItemValues(
                    "Set_AutoTrainPet",
                    Strings.setAutoTrainPetTitle,
                    () => (int)AutoPet.mode,
                    AutoPet.setState,
                    Strings.setAutoTrainPetChoices,
                    "",
                    "",
                    () => !Char.myCharz().havePet || AutoTrainNewAccount.isEnabled,
                    () =>
                    {
                        if (!Char.myCharz().havePet)
                            return Strings.youDontHaveDisciple + '!';
                        else if (AutoTrainNewAccount.isEnabled)
                            return string.Format(Strings.functionShouldBeDisabled, Strings.autoTrainForNewbieTitle);
                        else
                            return string.Empty;
                    }),
                new ModMenuItemValues(
                    "Set_AutoAttackWhenDiscipleNeed",
                    Strings.setAutoAttackWhenDiscipleNeededTitle,
                    () => (int)AutoPet.modeAttackWhenNeeded,
                    AutoPet.setAttackState,
                    Strings.setAutoAttackWhenDiscipleNeededChoices,
                    "",
                    "modeautopet",
                    () => AutoPet.mode <= AutoPet.AutoPetMode.Disabled,
                    () => string.Format(Strings.functionShouldBeEnabled, Strings.setAutoTrainPetTitle)),
                new ModMenuItemValues(
                    "Set_AutoRescue",
                        Strings.setAutoRescueTitle,
                        () => (int)AutoSkill.targetMode,
                        AutoSkill.setReviveTargetMode,
                        Strings.setAutoRescueChoices,
                        "",
                        "",
                        () => Char.myCharz().cgender != 1,
                        () => Strings.youAreNotNamekian + '!'),
                new ModMenuItemValues(
                    "Set_TimeChangeBg",
                    Strings.setTimeChangeCustomBgTitle,
                    () => CustomBackground.intervalChangeBackgroundWallpaper / 1000,
                    value => CustomBackground.intervalChangeBackgroundWallpaper = value * 1000,
                    null,
                    Strings.setTimeChangeCustomBgDescription,
                    "backgroundinveral"),
                //new ModMenuItemInt("Thời gian đổi logo", null, "Điều chỉnh thời gian thay đổi logo (giây)", 30, CustomLogo.setState, "logoinveral", false),
                //new ModMenuItemInt("Chiều cao của logo", null, "Điều chỉnh chiều cao của logo", 80, CustomLogo.setLogoHeight, "logoheight"),
            };
            modMenuItemFunctions = new ModMenuItemFunction[]
            {
                new ModMenuItemFunction("OpenXmapMenu", Strings.openXmapMenuTitle, Strings.openXmapMenuDescription, Pk9rXmap.showXmapMenu),
                new ModMenuItemFunction("OpenPickMobMenu", Strings.openPickMobMenuTitle, Strings.openPickMobMenuDescription, Pk9rPickMob.ShowMenu),
                //new ModMenuItemFunction("Menu AutoItem", "Mở menu AutoItem (lệnh \"item\" hoặc bấm nút I)", AutoItem.ShowMenu),
                new ModMenuItemFunction("OpenTeleportMenu", Strings.openTeleportMenuTitle, Strings.openTeleportMenuDescription, TeleportMenu.TeleportMenuMain.ShowMenu),
                new ModMenuItemFunction("OpenCustomBackgroundMenu", Strings.openCustomBackgroundMenuTitle, Strings.openCustomBackgroundMenuDescription, CustomBackground.ShowMenu),
                //new ModMenuItemFunction("Menu Custom Logo", "Mở menu logo tùy chỉnh", CustomLogo.ShowMenu),
                //new ModMenuItemFunction("Menu Custom Cursor", "Mở menu con trỏ tùy chỉnh", CustomCursor.ShowMenu),
                new ModMenuItemFunction("OpenSetsMenu", Strings.openSetsMenuTitle, Strings.openSetsMenuDescription, SetDo.ShowMenu),
            };
        }

        internal static void Paint(mGraphics g)
        {
            if (InfoDlg.isShow)
                return;
            if (Char.isLoadingMap)
                return;
            if (ChatTextField.gI().isShow)
                return;
            if (GameCanvas.menu.showMenu)
                return;
            if (GameCanvas.panel.isShow)
                return;
            if (GameCanvas.panel2 != null && GameCanvas.panel2.isShow)
                return;
            cmdOpenModMenu?.paint(g);
            if (cmdOpenModMenu != null && GameCanvas.isMouseFocus(cmdOpenModMenu.x, cmdOpenModMenu.y, cmdOpenModMenu.w, cmdOpenModMenu.h))
                g.drawImage(ItemMap.imageFlare, cmdOpenModMenu.x + 4, cmdOpenModMenu.y + 15, mGraphics.VCENTER | mGraphics.HCENTER);
        }

        internal static void UpdateTouch()
        {
            if (cmdOpenModMenu == null)
                return;
            if (GameCanvas.isPointerHoldIn(cmdOpenModMenu.x, cmdOpenModMenu.y, cmdOpenModMenu.w, cmdOpenModMenu.h))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                    cmdOpenModMenu.performAction();
                Char.myCharz().currentMovePoint = null;
                GameCanvas.clearAllPointerEvent();
                return;
            }
        }

        internal static void SetTabModMenu(Panel panel)
        {
            SetTabPanelTemplates.setTabListTemplate(panel, modMenuItemBools, modMenuItemValues, modMenuItemFunctions);
        }

        internal static void DoFireModMenu(Panel panel)
        {
            if (panel.currentTabIndex == 0)
                DoFireModMenuBools(panel);
            else if (panel.currentTabIndex == 1)
                DoFireModMenuValues(panel);
            else if (panel.currentTabIndex == 2)
                DoFireModMenuFunctions(panel);
            NotifySelectDisabledItem(panel);
        }

        static void DoFireModMenuFunctions(Panel panel)
        {
            panel.hideNow();
            if (modMenuItemFunctions[panel.selected].Callback != null)
                modMenuItemFunctions[panel.selected].Callback();
        }

        static void DoFireModMenuBools(Panel panel)
        {
            if (panel.selected < 0) 
                return;
            if (!modMenuItemBools[panel.selected].IsDisabled)
            {
                modMenuItemBools[panel.selected].SwitchSelection();
                GameScr.info1.addInfo(modMenuItemBools[panel.selected].Title + ": " + Strings.OnOffStatus(modMenuItemBools[panel.selected].Value), 0);
            }
        }

        static void DoFireModMenuValues(Panel panel)
        {
            if (panel.selected < 0) 
                return;
            int selected = panel.selected;
            if (modMenuItemValues[selected].IsDisabled) 
                return;
            if (modMenuItemValues[selected].Values != null)
                modMenuItemValues[selected].SwitchSelection();
            else
            {
                panel.chatTField = new ChatTextField();
                panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                panel.chatTField.initChatTextField();
                panel.chatTField.strChat = string.Empty;
                panel.chatTField.tfChat.name = inputModMenuItemInts[selected][1];
                panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                panel.chatTField.startChat2(instance, inputModMenuItemInts[selected][0]);
            }
        }

        static void NotifySelectDisabledItem(Panel panel)
        {
            int selected = panel.selected;
            if (selected == -1)
                return;
            if (panel.currentTabIndex == 0)
            {
                if (!modMenuItemBools[selected].IsDisabled) 
                    return;
                GameScr.info1.addInfo(modMenuItemBools[selected].DisabledReason, 0);
            }
            else if (panel.currentTabIndex == 1)
            {
                if (!modMenuItemValues[selected].IsDisabled) 
                    return;
                GameScr.info1.addInfo(modMenuItemValues[selected].DisabledReason, 0);
            }
            else if (panel.currentTabIndex == 2)
            {
                if (!modMenuItemFunctions[selected].IsDisabled) 
                    return;
                GameScr.info1.addInfo(modMenuItemFunctions[selected].DisabledReason, 0);
            }
        }

        internal static void PaintModMenu(Panel panel, mGraphics g)
        {
            if (panel.currentTabIndex == 0)
                PaintModMenuBools(panel, g);
            else if (panel.currentTabIndex == 1)
                PaintModMenuValues(panel, g);
            else if (panel.currentTabIndex == 2)
                PaintModMenuFunctions(panel, g);
        }

        static void PaintModMenuFunctions(Panel panel, mGraphics g)
        {
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            g.setColor(0);
            if (modMenuItemFunctions == null || modMenuItemFunctions.Length != panel.currentListLength) 
                return;
            bool isReset = true;
            string descriptionTextInfo = string.Empty;
            int x = 0, y = 0;
            for (int i = 0; i < panel.currentListLength; i++)
            {
                int num = panel.xScroll;
                int num2 = panel.yScroll + i * panel.ITEM_HEIGHT;
                int num3 = panel.wScroll;
                int num4 = panel.ITEM_HEIGHT - 1;
                ModMenuItemFunction modMenuItem = modMenuItemFunctions[i];
                if (!modMenuItem.IsDisabled)
                    g.setColor((i != panel.selected) ? 0xE7DFD2 : 0xF9FF4A);
                else 
                    g.setColor((i != panel.selected) ? 0xb7afa2 : 0xd0d73b);
                g.fillRect(num, num2, num3, num4);
                if (modMenuItem != null)
                {
                    mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, num + 5, num2, 0);
                    string description = Utils.TrimUntilFit(modMenuItem.Description, new GUIStyle() { font = mFont.tahoma_7_blue.myFont }, panel.wScroll - 5);
                    if (i == panel.selected && mFont.tahoma_7_blue.getWidth(modMenuItem.Description) > panel.wScroll - 5 && !panel.isClose)
                    {
                        isReset = false;
                        descriptionTextInfo = modMenuItem.Description;
                        x = num + 5;
                        y = num2 + 11;
                    }
                    else 
                        mFont.tahoma_7_blue.drawString(g, description, num + 5, num2 + 11, 0);
                }
            }
            if (isReset) 
                TextInfo.reset();
            else
            {
                TextInfo.paint(g, descriptionTextInfo, x, y, panel.wScroll - 5, 15, mFont.tahoma_7_blue);
                g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
                g.translate(0, -panel.cmy);
            }
            panel.paintScrollArrow(g);
        }

        static void PaintModMenuBools(Panel panel, mGraphics g)
        {
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            g.setColor(0);
            if (modMenuItemBools == null || modMenuItemBools.Length != panel.currentListLength) 
                return;
            bool isReset = true;
            string descriptionTextInfo = string.Empty;
            int x = 0, y = 0;
            for (int i = 0; i < panel.currentListLength; i++)
            {
                int xScroll = panel.xScroll;
                int yScroll = panel.yScroll + i * panel.ITEM_HEIGHT;
                int wScroll = panel.wScroll;
                int itemHeight = panel.ITEM_HEIGHT - 1;
                ModMenuItemBoolean modMenuItem = modMenuItemBools[i];
                if (!modMenuItem.IsDisabled) 
                    g.setColor((i != panel.selected) ? 0xE7DFD2 : 0xF9FF4A);
                else
                    g.setColor((i != panel.selected) ? 0xb7afa2 : 0xd0d73b);
                g.fillRect(xScroll, yScroll, wScroll, itemHeight);
                if (modMenuItem != null)
                {
                    mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, xScroll + 7, yScroll, 0);
                    string description = Utils.TrimUntilFit(modMenuItem.Description, new GUIStyle() { font = mFont.tahoma_7_blue.myFont }, panel.wScroll - 5);
                    if (i == panel.selected && mFont.tahoma_7_blue.getWidth(modMenuItem.Description) > panel.wScroll - 20 && !panel.isClose)
                    {
                        isReset = false;
                        descriptionTextInfo = modMenuItem.Description;
                        x = xScroll + 7;
                        y = yScroll + 11;
                    }
                    else
                        mFont.tahoma_7_blue.drawString(g, description, xScroll + 7, yScroll + 11, 0);
                    //mFont mf = mFont.tahoma_7_grey;
                    //if (modMenuItem.Value)
                        //mf = mFont.tahoma_7b_red;
                    //mf.drawString(g, status, num + num3 - 2, num2 + panel.ITEM_HEIGHT - 14, mFont.RIGHT);
                    if (modMenuItem.Value)
                        g.setColor(0x00b000);
                    else 
                        g.setColor(0xe00000);
                    g.fillRect(xScroll, yScroll, 2, itemHeight);
                }
            }
            if (isReset)
                TextInfo.reset();
            else
            {
                TextInfo.paint(g, descriptionTextInfo, x, y, panel.wScroll - 5, 15, mFont.tahoma_7_blue);
                g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
                g.translate(0, -panel.cmy);
            }
            panel.paintScrollArrow(g);
        }

        static void PaintModMenuValues(Panel panel, mGraphics g)
        {
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            g.setColor(0);
            if (modMenuItemValues == null || modMenuItemValues.Length != panel.currentListLength)
                return;
            bool isReset = true;
            string descriptionTextInfo = string.Empty;
            int x = 0, y = 0, currSelectedValue = 0;
            for (int i = 0; i < panel.currentListLength; i++)
            {
                int num = panel.xScroll;
                int num2 = panel.yScroll + i * panel.ITEM_HEIGHT;
                int num3 = panel.wScroll;
                int num4 = panel.ITEM_HEIGHT - 1;
                ModMenuItemValues modMenuItem = modMenuItemValues[i];
                if (!modMenuItem.IsDisabled)
                    g.setColor((i != panel.selected) ? 0xE7DFD2 : 0xF9FF4A);
                else
                    g.setColor((i != panel.selected) ? 0xb7afa2 : 0xd0d73b);
                g.fillRect(num, num2, num3, num4);
                if (modMenuItem != null)
                {
                    string description, str;
                    mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, num + 5, num2, 0);
                    if (modMenuItem.Values != null)
                    {
                        str = modMenuItem.getSelectedValue();
                        description = Utils.TrimUntilFit(str, new GUIStyle() { font = mFont.tahoma_7_blue.myFont }, panel.wScroll - 5);
                    }
                    else
                    {
                        str = modMenuItem.Description;
                        description = Utils.TrimUntilFit(str, new GUIStyle() { font = mFont.tahoma_7_blue.myFont }, panel.wScroll - 5 - mFont.tahoma_7_blue.getWidth(modMenuItem.SelectedValue.ToString()));
                        mFont.tahoma_7b_red.drawString(g, modMenuItem.SelectedValue.ToString(), num + num3 - 2, num2 + panel.ITEM_HEIGHT - 14, mFont.RIGHT);
                    }
                    if (i == panel.selected && mFont.tahoma_7_blue.getWidth(str) > panel.wScroll - 5 - mFont.tahoma_7_blue.getWidth(modMenuItem.SelectedValue.ToString()) && !panel.isClose)
                    {
                        isReset = false;
                        descriptionTextInfo = modMenuItem.Description;
                        currSelectedValue = modMenuItem.SelectedValue;
                        x = num + 5;
                        y = num2 + 11;
                    }
                    else
                        mFont.tahoma_7_blue.drawString(g, description, num + 5, num2 + 11, 0);
                }
            }
            if (isReset)
                TextInfo.reset();
            else
            {
                TextInfo.paint(g, descriptionTextInfo, x, y, panel.wScroll - 10 - mFont.tahoma_7b_red.getWidth(currSelectedValue.ToString()), 15, mFont.tahoma_7_blue);
                g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
                g.translate(0, -panel.cmy);
            }
            panel.paintScrollArrow(g);
        }

        internal static void SaveData()
        {
            foreach (ModMenuItemBoolean modMenuItem in modMenuItemBools) 
                if (!string.IsNullOrEmpty(modMenuItem.RMSName))
                    Utils.SaveData(modMenuItem.RMSName, modMenuItem.Value);
            foreach (ModMenuItemValues modMenuItem in modMenuItemValues)
                if (!string.IsNullOrEmpty(modMenuItem.RMSName))
                    Utils.SaveData(modMenuItem.RMSName, modMenuItem.SelectedValue);
        }

        internal static void LoadData()
        {
            foreach (ModMenuItemBoolean modMenuItem in modMenuItemBools)
            {
                if (!string.IsNullOrEmpty(modMenuItem.RMSName) && Utils.TryLoadDataBool(modMenuItem.RMSName, out bool value))
                    modMenuItem.Value = value;
            }
            foreach (ModMenuItemValues modMenuItem in modMenuItemValues)
            {
                if (!string.IsNullOrEmpty(modMenuItem.RMSName) && Utils.TryLoadDataInt(modMenuItem.RMSName, out int data))
                    modMenuItem.SelectedValue = data;
            }
        }

        internal static ModMenuItem GetModMenuItem(string id)
        {
            foreach (ModMenuItemBoolean item in modMenuItemBools)
            {
                if (item.ID == id)
                    return item;
            }
            foreach (ModMenuItemValues item in modMenuItemValues)
            {
                if (item.ID == id)
                    return item;
            }
            foreach (ModMenuItemFunction item in modMenuItemFunctions)
            {
                if (item.ID == id)
                    return item;
            }
            return null;
        }

        internal static T GetModMenuItem<T>(string id) where T : ModMenuItem
        {
            if (typeof(T) == typeof(ModMenuItemBoolean))
            {
                foreach (ModMenuItemBoolean item in modMenuItemBools)
                {
                    if (item.ID == id)
                        return item as T;
                }
            }
            if (typeof(T) == typeof(ModMenuItemValues))
            {
                foreach (ModMenuItemValues item in modMenuItemValues)
                {
                    if (item.ID == id)
                        return item as T;
                }
            }
            if (typeof(T) == typeof(ModMenuItemFunction))
                foreach (ModMenuItemFunction item in modMenuItemFunctions)
                {
                    if (item.ID == id)
                        return item as T;
                }
            return null;
        }

        public void onChatFromMe(string text, string to)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (to == inputModMenuItemInts[0][0])
                {
                    try
                    {
                        int value = int.Parse(text);
                        if (value > Screen.currentResolution.refreshRateRatio.value || value < 5) 
                            throw new Exception();
                        GetModMenuItem<ModMenuItemValues>("Set_FPS").SelectedValue = value;
                        GameScr.info1.addInfo("Đã thay đổi mức FPS!", 0);
                    }
                    catch 
                    {
                        GameCanvas.startOKDlg("Mức FPS không hợp lệ!");
                    }
                }
                else if (to == inputModMenuItemInts[6][0])
                {
                    try
                    {
                        int value = int.Parse(text);
                        if (value < 10) 
                            throw new Exception();
                        GetModMenuItem<ModMenuItemValues>("Set_TimeChangeBg").SelectedValue = value;
                        GameScr.info1.addInfo("Đã thay đổi thời gian đổi hình nền!", 0);
                    }
                    catch
                    {
                        GameCanvas.startOKDlg("Thời gian không hợp lệ!");
                    }
                }
                //else if (to == inputModMenuItemInts[7][0])
                //{
                //    try
                //    {
                //        int value = int.Parse(text);
                //        if (value < 10) throw new Exception();
                //        modMenuItemInts[7].setValue(value);
                //        GameScr.info1.addInfo("Đã thay đổi thời gian đổi logo!", 0);
                //    }
                //    catch
                //    {
                //        GameCanvas.startOKDlg("Thời gian không hợp lệ!");
                //    }
                //}
                //else if (to == inputModMenuItemInts[8][0])
                //{
                //    try
                //    {
                //        int value = int.Parse(text);
                //        if (value < 25 || value > Screen.height * 30 / 100) throw new Exception();
                //        modMenuItemInts[8].setValue(value);
                //        GameScr.info1.addInfo("Đã thay đổi chiều cao logo!", 0);
                //        CustomLogo.LoadData();
                //    }
                //    catch
                //    {
                //        GameCanvas.startOKDlg("Chiều cao không hợp lệ!");
                //    }
                //}
            }
            else
                currentPanel.chatTField.isShow = false;
            currentPanel.chatTField.ResetTF();
        }

        public void onCancelChat() => currentPanel.chatTField.ResetTF();

        public void perform(int idAction, object p)
        {
            if (idAction == 1)
            {
                if (currentPanel == null)
                    currentPanel = new Panel();
                CustomPanelMenu.show(SetTabModMenu, DoFireModMenu, null, PaintModMenu, currentPanel);
                currentPanel.cmdClose.x = GameCanvas.w - currentPanel.cmdClose.img.getWidth() - 1;
                currentPanel.cmdClose.y = 1;
            }
        }
    }
}
