using Mod.Auto;
using Mod.CustomPanel;
using Mod.Graphics;
using Mod.PickMob;
using Mod.R;
using Mod.Set;
using Mod.TeleportMenu;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mod.ModMenu
{
    internal static class ModMenuMain
    {
        class ModMenuMainActionListener : IActionListener
        {
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

        internal static Panel currentPanel
        {
            get => GameCanvas.panel2;
            set => GameCanvas.panel2 = value;
        }

        internal static ModMenuItemBoolean[] modMenuItemBools;
        internal static ModMenuItemValues[] modMenuItemValues;
        internal static ModMenuItemFunction[] modMenuItemFunctions;

        static ModMenuMainActionListener actionListener = new ModMenuMainActionListener();
        static sbyte lastLanguage = -1;
        static GUIStyle style = new GUIStyle() { font = Resources.Load<Font>($"FontSys/x{mGraphics.zoomLevel}/chelthm") };

        //internal static Dictionary<int, string[]> inputModMenuItemValues = new Dictionary<int, string[]>()
        //{
        //    { 7, new string[]{ "Nhập thời gian thay đổi logo", "Thời gian (giây)" } },
        //    { 8, new string[]{ "Nhập chiều cao logo", "Chiều cao logo" } },
        //};

        internal static Command cmdOpenModMenu;

        internal static void Initialize()
        {
            if (cmdOpenModMenu != null)
                return;
            cmdOpenModMenu = new Command("", actionListener, 1, null);
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
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig()
                {
                    ID = "VSync_Toggle",
                    Title = "VSync",
                    Description = Strings.vSyncDescription,
                    GetValueFunc = () => QualitySettings.vSyncCount == 1,
                    SetValueAction = value => QualitySettings.vSyncCount = value ? 1 : 0,
                    RMSName = "isvsync"
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig() 
                {
                    ID = "ShowTargetInfo_Toggle",
                    Title = Strings.showTargetInfoTitle,
                    Description = Strings.showTargetInfoDescription,
                    GetValueFunc = () => CharEffect.isEnabled,
                    SetValueAction = CharEffect.setState,
                    RMSName = "isshowinfochar"
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig()
                {
                    ID = "AutoSendAttack_Toggle",
                    Title = Strings.autoAttack,
                    Description = Strings.autoSendAttackDescription,
                    GetValueFunc = () => AutoSendAttack.gI.IsActing,
                    SetValueAction = AutoSendAttack.toggle
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig()
                {
                    ID = "ShowCharList_Toggle",
                    Title = Strings.showCharListTitle,
                    Description = Strings.showCharListDescription,
                    GetValueFunc =  () => ListCharsInMap.isEnabled,
                    SetValueAction = ListCharsInMap.setState,
                    RMSName = "isshowlistchar",
                    GetIsDisabled = () => true,
                    GetDisabledReason = () => "This feature is currently in development state"
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig()
                {
                    ID = "ShowPetInCharList_Toggle",
                    Title = Strings.showPetInCharListTitle,
                    Description = Strings.showPetInCharListDescription,
                    GetValueFunc = () => ListCharsInMap.isShowPet,
                    SetValueAction = ListCharsInMap.setStatePet,
                    RMSName = "isshowlistpet",
                    GetIsDisabled = () => !ListCharsInMap.isEnabled,
                    GetDisabledReason = () => string.Format(Strings.functionShouldBeEnabled, Strings.showCharListTitle)
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig()
                {
                    ID = "AutoTrainForNewbie_Toggle",
                    Title = Strings.autoTrainForNewbieTitle,
                    Description = Strings.autoTrainForNewbieDescription,
                    GetValueFunc = () => AutoTrainNewAccount.isEnabled,
                    SetValueAction = AutoTrainNewAccount.setState,
                    GetIsDisabled = () => Char.myCharz().taskMaint == null || Char.myCharz().taskMaint.taskId > 11,
                    GetDisabledReason = () => Strings.noLongerNewAccount + '!'
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig()
                {
                    ID = "CustomBg_Toggle",
                    Title = Strings.customBackgroundTitle,
                    Description = Strings.customBackgroundDescription,
                    GetValueFunc = () => CustomBackground.isEnabled,
                    SetValueAction = CustomBackground.SetState,
                    RMSName = "iscustombackground",
                    GetIsDisabled = () => GraphicsReducer.level > ReduceGraphicsLevel.None,
                    GetDisabledReason = () => string.Format(Strings.functionShouldBeDisabled, Strings.setReduceGraphicsTitle)
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig()
                {
                    ID = "SkipSpaceship_Toggle",
                    Title = Strings.skipSpaceshipTitle,
                    Description = Strings.skipSpaceshipDescription,
                    GetValueFunc = () => SpaceshipSkip.isEnabled,
                    SetValueAction = value => SpaceshipSkip.isEnabled = value,
                    RMSName = "isskipspaceship"
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig() 
                {
                    ID = "NotifyBoss_Toggle",
                    Title = Strings.notifyBossTitle,
                    Description = Strings.notifyBossDescription,
                    GetValueFunc = () => Boss.isEnabled,
                    SetValueAction = Boss.setState,
                    RMSName = "sanboss",
                    GetIsDisabled = () => true,
                    GetDisabledReason = () => "This feature is currently in development state"
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig() 
                {
                    ID = "PickMob_Toggle",
                    Title = Strings.pickMobTitle,
                    Description = Strings.pickMobDescription,
                    GetValueFunc = () => Pk9rPickMob.IsTanSat,
                    SetValueAction = Pk9rPickMob.SetSlaughter,
                    GetIsDisabled = () => AutoTrainNewAccount.isEnabled,
                    GetDisabledReason = () => string.Format(Strings.functionShouldBeDisabled, Strings.autoTrainForNewbieTitle)
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig() 
                {
                    ID = "PickMob_AvoidSuperMob_Toggle",
                    Title = Strings.avoidSuperMobTitle,
                    Description = Strings.avoidSuperMobDescription,
                    GetValueFunc = () => Pk9rPickMob.IsNeSieuQuai,
                    SetValueAction = Pk9rPickMob.SetAvoidSuperMonster,
                    RMSName = "isnesieuquaits"
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig() 
                {
                    ID = "PickMob_VDH_Toggle",
                    Title = Strings.vdhTitle,
                    Description = Strings.vdhDescription,
                    GetValueFunc = () => Pk9rPickMob.IsVuotDiaHinh,
                    SetValueAction = Pk9rPickMob.SetCrossTerrain,
                    RMSName = "isvuotdiahinh"
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig()
                {
                    ID = "PickMob_AutoPickItem_Toggle",
                    Title = Strings.autoPickItemTitle,
                    Description = Strings.autoPickItemDescription,
                    GetValueFunc = () => Pk9rPickMob.IsAutoPickItems,
                    SetValueAction = Pk9rPickMob.SetAutoPickItems,
                    RMSName = "isautopick",
                    GetIsDisabled = () => AutoTrainNewAccount.isEnabled,
                    GetDisabledReason = () => string.Format(Strings.functionShouldBeDisabled, Strings.autoTrainForNewbieTitle)
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig()
                {
                    ID = "PickMob_PickMyItemOnly_Toggle",
                    Title = Strings.pickMyItemOnlyTitle,
                    Description = Strings.pickMyItemOnlyDescription,
                    GetValueFunc = () => Pk9rPickMob.IsItemMe,
                    SetValueAction = Pk9rPickMob.SetAutoPickItemsFromOthers,
                    RMSName = "ispickmyitemonly"
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig()
                {
                    ID = "PickMob_LimitPickTimes_Toggle",
                    Title = Strings.limitPickTimesTitle,
                    Description = Strings.limitPickTimesDescription,
                    GetValueFunc = () => Pk9rPickMob.IsLimitTimesPickItem,
                    SetValueAction = Pk9rPickMob.SetPickUpLimited,
                    RMSName = "islimitpicktimes"
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig() 
                {
                    ID = "AutoAskForPeans_Toggle",
                    Title = Strings.autoAskForPeansTitle,
                    Description = Strings.autoAskForPeansDescription,
                    GetValueFunc = () => AutoPean.isAutoRequest,
                    SetValueAction = value => AutoPean.isAutoRequest = value,
                    RMSName = "autoaskforpeans",
                    GetIsDisabled = () => Char.myCharz().clan == null,
                    GetDisabledReason = () => Strings.youAreNotInAClan + '!'
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig()
                {
                    ID = "AutoDonatePeans_Toggle",
                    Title = Strings.autoDonatePeansTitle,
                    Description = Strings.autoDonatePeansDescription,
                    GetValueFunc = () => AutoPean.isAutoDonate,
                    SetValueAction = value => AutoPean.isAutoDonate = value,
                    RMSName = "autodonatepeans",
                    GetIsDisabled = () => Char.myCharz().clan == null,
                    GetDisabledReason = () => Strings.youAreNotInAClan + '!'
                }),
                new ModMenuItemBoolean(new ModMenuItemBooleanConfig() 
                {
                    ID = "AutoHarvestPeans_Toggle",
                    Title = Strings.autoHarvestPeansTitle,
                    Description = Strings.autoHarvestPeansDescription,
                    GetValueFunc = () => AutoPean.isAutoHarvest,
                    SetValueAction = value => AutoPean.isAutoHarvest = value,
                    RMSName = "autoharvestpeans"
                }),
                //new ModMenuItemBoolean("Hiện khoảng cách bom", "Hiển thị người, quái, boss... trong tầm bom", SuicideRange.setState, false, "isshowsuiciderange")
                //new ModMenuItemBoolean("Logo tùy chỉnh", "Bật/tắt hiển thị logo tùy chỉnh trên màn hình game", CustomLogo.setState, false, "isshowlogo"),
                //new ModMenuItemBoolean("EHVN_CustomCursor_Toggle", "Con trỏ tùy chỉnh", "Thay con trỏ chuột mặc định thành con trỏ chuột tùy chỉnh", CustomCursor.setState, false, "customcusor"),
            };
            modMenuItemValues = new ModMenuItemValues[]
            {
                new ModMenuItemValues(new ModMenuItemValuesConfig()
                {
                    ID = "Set_FPS",
                    Title = "FPS",
                    Description = Strings.setFPSDescription,
                    GetValueFunc = () => Application.targetFrameRate,
                    SetValueAction = value =>
                    {
                        if (value > 5 && value <= Screen.currentResolution.refreshRateRatio.value)
                            Application.targetFrameRate = value;
                    },
                    RMSName = "targetfps",
                    GetIsDisabled = () => QualitySettings.vSyncCount == 1,
                    GetDisabledReason = () => string.Format(Strings.functionShouldBeDisabled, "VSync"),
                    TextFieldTitle = Strings.inputFPS,
                    TextFieldHint = "FPS",
                }),
                new ModMenuItemValues(new ModMenuItemValuesConfig()
                { 
                    ID = "Set_ReduceGraphics",
                    Title = Strings.setReduceGraphicsTitle,
                    Values = Strings.setReduceGraphicsChoices,
                    GetValueFunc = () => (int)GraphicsReducer.level,
                    SetValueAction = level => GraphicsReducer.level = (ReduceGraphicsLevel)level,
                    RMSName = "levelreducegraphics",
                    GetIsDisabled = () => true,
                    GetDisabledReason = () => "This feature is currently in development state"
                }),
                new ModMenuItemValues(new ModMenuItemValuesConfig()
                {
                    ID = "Set_GoBack",
                    Title = "GoBack",
                    Values = Strings.setGoBackChoices,
                    GetValueFunc = () => (int)AutoGoback.mode,
                    SetValueAction = AutoGoback.setState,
                    //GetIsDisabled = () => AutoTrainNewAccount.isEnabled,
                    //GetDisabledReason = () => string.Format(Strings.functionShouldBeDisabled, Strings.autoTrainForNewbieTitle)
                    GetIsDisabled = () => true,
                    GetDisabledReason = () => "This feature is currently in development state"
                }),
                new ModMenuItemValues(new ModMenuItemValuesConfig()
                {
                    ID = "Set_AutoTrainPet",
                    Title = Strings.setAutoTrainPetTitle,
                    Values = Strings.setAutoTrainPetChoices,
                    GetValueFunc = () => (int)AutoPet.mode,
                    SetValueAction = AutoPet.setState,
                    GetIsDisabled = () => !Char.myCharz().havePet || AutoTrainNewAccount.isEnabled,
                    GetDisabledReason = () =>
                    {
                        if (!Char.myCharz().havePet)
                            return Strings.youDontHaveDisciple + '!';
                        else if (AutoTrainNewAccount.isEnabled)
                            return string.Format(Strings.functionShouldBeDisabled, Strings.autoTrainForNewbieTitle);
                        else
                            return string.Empty;
                    }
                }),
                new ModMenuItemValues(new ModMenuItemValuesConfig()
                {
                    ID = "Set_AutoAttackWhenDiscipleNeed",
                    Title = Strings.setAutoAttackWhenDiscipleNeededTitle,
                    Values = Strings.setAutoAttackWhenDiscipleNeededChoices,
                    GetValueFunc = () => (int)AutoPet.modeAttackWhenNeeded,
                    SetValueAction = AutoPet.setAttackState,
                    RMSName = "modeautopet",
                    GetIsDisabled = () => AutoPet.mode <= AutoPet.AutoPetMode.Disabled,
                    GetDisabledReason = () => string.Format(Strings.functionShouldBeEnabled, Strings.setAutoTrainPetTitle)
                }),
                new ModMenuItemValues(new ModMenuItemValuesConfig()
                {
                    ID = "Set_AutoRescue",
                    Title = Strings.setAutoRescueTitle,
                    Values = Strings.setAutoRescueChoices,
                    GetValueFunc = () => (int)AutoSkill.targetMode,
                    SetValueAction = AutoSkill.setReviveTargetMode,
                    GetIsDisabled = () => Char.myCharz().cgender != 1 || ((Skill)Char.myCharz().vSkillFight.elementAt(2)) != null && ((Skill)Char.myCharz().vSkillFight.elementAt(2)).template.isBuffToPlayer(),
                    GetDisabledReason = () => 
                    {
                        if (Char.myCharz().cgender != 1)
                            return Strings.youAreNotNamekian + '!';
                        Skill skill = (Skill)Char.myCharz().vSkillFight.elementAt(2);
                        if (skill == null)
                            return Strings.setAutoRescueSkill3Null + '!'; 
                        if (!skill.template.isBuffToPlayer())
                            return Strings.setAutoRescueSkill3BuffInvalid + '!';
                        return "";
                    }
                }),
                new ModMenuItemValues(new ModMenuItemValuesConfig()
                {
                    ID = "Set_TimeChangeBg",
                    Title = Strings.setTimeChangeCustomBgTitle,
                    Description = Strings.setTimeChangeCustomBgDescription,
                    GetValueFunc = () => CustomBackground.intervalChangeBackgroundWallpaper / 1000,
                    SetValueAction = value => CustomBackground.intervalChangeBackgroundWallpaper = value * 1000,
                    RMSName = "backgroundinveral",
                    TextFieldTitle = Strings.inputTimeChangeBg,
                    TextFieldHint = Strings.inputTimeChangeBgHint
                }),
                //new ModMenuItemInt("Thời gian đổi logo", null, "Điều chỉnh thời gian thay đổi logo (giây)", 30, CustomLogo.setState, "logoinveral", false),
                //new ModMenuItemInt("Chiều cao của logo", null, "Điều chỉnh chiều cao của logo", 80, CustomLogo.setLogoHeight, "logoheight"),
            };
            modMenuItemFunctions = new ModMenuItemFunction[]
            {
                new ModMenuItemFunction(new ModMenuItemFunctionConfig()
                {
                    ID = "OpenXmapMenu",
                    Title = Strings.openXmapMenuTitle,
                    Description = Strings.openXmapMenuDescription, 
                    Action = Pk9rXmap.showXmapMenu,
                    GetIsDisabled = () => true,
                    GetDisabledReason = () => "This feature is currently in development state"
                }),
                new ModMenuItemFunction(new ModMenuItemFunctionConfig()
                {
                    ID = "OpenPickMobMenu",
                    Title = Strings.openPickMobMenuTitle,
                    Description = Strings.openPickMobMenuDescription, 
                    Action = Pk9rPickMob.ShowMenu
                }),
                new ModMenuItemFunction(new ModMenuItemFunctionConfig()
                {
                    ID = "OpenTeleportMenu",
                    Title = Strings.openTeleportMenuTitle,
                    Description = Strings.openTeleportMenuDescription, 
                    Action = TeleportMenuMain.ShowMenu,
                    GetIsDisabled = () => true,
                    GetDisabledReason = () => "This feature is currently in development state"
                }),
                new ModMenuItemFunction(new ModMenuItemFunctionConfig()
                {
                    ID = "OpenCustomBackgroundMenu",
                    Title = Strings.openCustomBackgroundMenuTitle,
                    Description = Strings.openCustomBackgroundMenuDescription, 
                    Action = CustomBackground.ShowMenu
                }),
                new ModMenuItemFunction(new ModMenuItemFunctionConfig()
                {
                    ID = "OpenSetsMenu",
                    Title = Strings.openSetsMenuTitle,
                    Description = Strings.openSetsMenuDescription, 
                    Action = SetDo.ShowMenu,
                    GetIsDisabled = () => true,
                    GetDisabledReason = () => "This feature is currently in development state"
                }),
                //new ModMenuItemFunction("Menu AutoItem", "Mở menu AutoItem (lệnh \"item\" hoặc bấm nút I)", AutoItem.ShowMenu),
                //new ModMenuItemFunction("Menu Custom Logo", "Mở menu logo tùy chỉnh", CustomLogo.ShowMenu),
                //new ModMenuItemFunction("Menu Custom Cursor", "Mở menu con trỏ tùy chỉnh", CustomCursor.ShowMenu),
            };
        }

        internal static void Paint(mGraphics g)
        {
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
            if (cmdOpenModMenu != null && (GameCanvas.isMouseFocus(cmdOpenModMenu.x, cmdOpenModMenu.y, cmdOpenModMenu.w, cmdOpenModMenu.h) || (GameCanvas.isMouseFocus((int)(cmdOpenModMenu.x - cmdOpenModMenu.w * 1.5), cmdOpenModMenu.y, (int)(cmdOpenModMenu.w * 2.5), cmdOpenModMenu.h) && GameCanvas.isPointerDown)))
                g.drawImage(ItemMap.imageFlare, cmdOpenModMenu.x + 4, cmdOpenModMenu.y + 15, mGraphics.VCENTER | mGraphics.HCENTER);
        }

        internal static void UpdateTouch()
        {
            if (cmdOpenModMenu == null)
                return;
            if (GameCanvas.isPointerHoldIn((int)(cmdOpenModMenu.x - cmdOpenModMenu.w * 1.5), cmdOpenModMenu.y, (int)(cmdOpenModMenu.w * 2.5), cmdOpenModMenu.h) && GameCanvas.isPointerClick)
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
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
            if (!modMenuItemFunctions[panel.selected].IsDisabled)
            {
                panel.hide();
                modMenuItemFunctions[panel.selected].Action?.Invoke();
            }
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
                modMenuItemValues[selected].StartChat(currentPanel.chatTField = new ChatTextField());
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
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            if (panel.currentTabIndex == 0)
                PaintModMenuBools(panel, g);
            else if (panel.currentTabIndex == 1)
                PaintModMenuValues(panel, g);
            else if (panel.currentTabIndex == 2)
                PaintModMenuFunctions(panel, g);
        }

        static void PaintModMenuBools(Panel panel, mGraphics g)
        {
            if (modMenuItemBools == null || modMenuItemBools.Length != panel.currentListLength) 
                return;
            int offset = Math.Max(panel.cmy / panel.ITEM_HEIGHT, 0);
            bool isReset = true;
            string descriptionTextInfo = string.Empty;
            int x = 0, y = 0;
            for (int i = offset; i < Mathf.Clamp(offset + panel.hScroll / panel.ITEM_HEIGHT + 2, 0, panel.currentListLength); i++)
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
                mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, xScroll + 7, yScroll, 0);
                if (i == panel.selected && mFont.tahoma_7_blue.getWidth(modMenuItem.Description) > panel.wScroll - 20 && !panel.isClose)
                {
                    isReset = false;
                    descriptionTextInfo = modMenuItem.Description;
                    x = xScroll + 7;
                    y = yScroll + 11;
                }
                else
                    mFont.tahoma_7_blue.drawString(g, Utils.TrimUntilFit(modMenuItem.Description, style, panel.wScroll - 5), xScroll + 7, yScroll + 11, 0);
                if (modMenuItem.Value)
                    g.setColor(0x00b000);
                else 
                    g.setColor(0xe00000);
                g.fillRect(xScroll, yScroll, 2, itemHeight);
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
            if (modMenuItemValues == null || modMenuItemValues.Length != panel.currentListLength)
                return;
            int offset = Math.Max(panel.cmy / panel.ITEM_HEIGHT, 0);
            bool isReset = true;
            string descriptionTextInfo = string.Empty;
            int x = 0, y = 0, currSelectedValue = 0;
            for (int i = offset; i < Mathf.Clamp(offset + panel.hScroll / panel.ITEM_HEIGHT + 2, 0, panel.currentListLength); i++)
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
                string str;
                mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, num + 5, num2, 0);
                int descWidth;
                if (modMenuItem.Values != null)
                {
                    str = modMenuItem.getSelectedValue();
                    descWidth = panel.wScroll - 5;
                }
                else
                {
                    str = modMenuItem.Description;
                    descWidth = panel.wScroll - 5 - mFont.tahoma_7_blue.getWidth(modMenuItem.SelectedValue.ToString());
                    mFont.tahoma_7b_red.drawString(g, modMenuItem.SelectedValue.ToString(), num + num3 - 2, num2 + panel.ITEM_HEIGHT - 14, mFont.RIGHT);
                }
                if (i == panel.selected && mFont.tahoma_7_blue.getWidth(str) > descWidth && !panel.isClose)
                {
                    isReset = false;
                    descriptionTextInfo = str;
                    currSelectedValue = modMenuItem.SelectedValue;
                    x = num + 5;
                    y = num2 + 11;
                }
                else
                    mFont.tahoma_7_blue.drawString(g, Utils.TrimUntilFit(str, style, descWidth), num + 5, num2 + 11, 0);
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

        static void PaintModMenuFunctions(Panel panel, mGraphics g)
        {
            if (modMenuItemFunctions == null || modMenuItemFunctions.Length != panel.currentListLength) 
                return;
            int offset = Math.Max(panel.cmy / panel.ITEM_HEIGHT, 0);
            bool isReset = true;
            string descriptionTextInfo = string.Empty;
            int x = 0, y = 0;
            for (int i = offset; i < Mathf.Clamp(offset + panel.hScroll / panel.ITEM_HEIGHT + 2, 0, panel.currentListLength); i++)
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
                mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, num + 5, num2, 0);
                if (i == panel.selected && mFont.tahoma_7_blue.getWidth(modMenuItem.Description) > panel.wScroll - 5 && !panel.isClose)
                {
                    isReset = false;
                    descriptionTextInfo = modMenuItem.Description;
                    x = num + 5;
                    y = num2 + 11;
                }
                else 
                    mFont.tahoma_7_blue.drawString(g, Utils.TrimUntilFit(modMenuItem.Description, style, panel.wScroll - 5), num + 5, num2 + 11, 0);
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
    }
}
