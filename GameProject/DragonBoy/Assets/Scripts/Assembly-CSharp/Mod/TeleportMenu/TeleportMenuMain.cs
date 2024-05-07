using Mod.CustomPanel;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using Mod.R;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Mod.TeleportMenu
{
    internal class TeleportMenuMain
    {
        class TeleportMenuChatable : IChatable
        {
            public void onChatFromMe(string text, string to)
            {
                if (string.IsNullOrEmpty(text) || to != Strings.teleportMenuInputCharIDTextFieldName)
                {
                    onCancelChat();
                    return;
                }
                try
                {
                    int charId = int.Parse(text);
                    if (charId < 0)
                    {
                        GameCanvas.startOKDlg(string.Format(Strings.inputNumberMustBeBiggerThanOrEqual, 0) + '!');
                        return;
                    }
                    listTeleportChars.Add(new TeleportChar(charId));
                    SaveData();
                    GameScr.info1.addInfo(string.Format(Strings.teleportMenuAddedCharacterWithID, charId) + '!', 0);

                }
                catch
                {
                    GameCanvas.startOKDlg(Strings.invalidValue + '!');
                }
                ChatTextField.gI().ResetTF();
                SortList();
            }

            public void onCancelChat() => ChatTextField.gI().ResetTF();
        }

        internal static List<TeleportChar> listTeleportChars = new List<TeleportChar>();
        static List<TeleportChar> listTeleportChars_orderByName = new List<TeleportChar>();
        static TeleportStatus currentTeleportStatus;
        static bool isDataLoaded, isAutoTeleportTo;
        static TeleportChar charAutoTeleportTo;

        static long lastTimeAutoTeleportTo;
        static bool isChangeDisguise;
        static int previousDisguiseId = -1;

        [ChatCommand("tele"), HotkeyCommand('z')]
        internal static void ShowMenu()
        {
            var menuBuilder = new MenuBuilder();
            menuBuilder.addItem(listTeleportChars.Count > 0,
                    Strings.teleportMenuOpenSavedCharList, new MenuAction(() => ShowListChars(TeleportStatus.TeleportTo)));
            Char ch;
            Char focus = Char.myCharz().charFocus;
            if (focus != null && focus.IsNormalChar())
                ch = focus;
            else
                ch = Char.myCharz().ClosestChar(70, true);

            if (ch != null)
            {
                var teleportChar = new TeleportChar(ch);
                menuBuilder.addItem(!listTeleportChars.Contains(teleportChar),
                    $"{Strings.add}\n{teleportChar.Name}\n[{teleportChar.ID}]", new MenuAction(() =>
                    {
                        listTeleportChars.Insert(0, teleportChar);
                        SaveData();
                        GameScr.info1.addInfo(string.Format(Strings.teleportMenuCharacterAdded, teleportChar) + '!', 0);
                    }));
            }

            if (focus != null && focus.IsNormalChar())
            {
                var teleportChar = new TeleportChar(focus);
                menuBuilder.addItem(listTeleportChars.Contains(teleportChar) && ((isAutoTeleportTo && charAutoTeleportTo != teleportChar) || !isAutoTeleportTo),
                    $"{Strings.delete}\n{teleportChar.Name}\n[{teleportChar.ID}]", new MenuAction(() =>
                    {
                        if (!isAutoTeleportTo || teleportChar == charAutoTeleportTo)
                        {
                            GameScr.info1.addInfo(Strings.teleportMenuCantRemoveTargetChar + '!', 0);
                            return;
                        }

                        listTeleportChars.Remove(teleportChar);
                        SaveData();
                        GameScr.info1.addInfo(string.Format(Strings.teleportMenuCharacterRemoved, teleportChar) + '!', 0);
                    }));
            }
            menuBuilder.addItem(listTeleportChars.Count > 0, isAutoTeleportTo ? Strings.teleportMenuStopTeleporting : Strings.teleportMenuSelectTarget, new MenuAction(() =>
                {
                    if (!isAutoTeleportTo)
                    {
                        ShowListChars(TeleportStatus.AutoTeleportTo);
                        return;
                    }
                    isAutoTeleportTo = false;
                    GameScr.info1.addInfo(string.Format(Strings.teleportMenuStopTeleportToTarget, charAutoTeleportTo) + '!', 0);
                    charAutoTeleportTo = null;
                }))
            .addItem(Strings.teleportMenuAddCharacterByID, new MenuAction(() =>
            {
                ChatTextField.gI().strChat = "";
                ChatTextField.gI().tfChat.name = Strings.teleportMenuInputCharIDTextFieldHint;
                ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                ChatTextField.gI().startChat2(new TeleportMenuChatable(), Strings.teleportMenuInputCharIDTextFieldName);
            }))
            .addItem(GameScr.vCharInMap.size() > 1,
                Strings.teleportMenuAddEveryoneInZone, new MenuAction(() =>
                {
                    for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                    {
                        Char ch = (Char)GameScr.vCharInMap.elementAt(i);
                        if (ch.IsNormalChar(false, false))
                        {
                            TeleportChar teleportChar1 = new TeleportChar(ch);
                            if (!listTeleportChars.Contains(teleportChar1))
                                listTeleportChars.Add(teleportChar1);
                        }
                    }
                    SaveData();
                    GameScr.info1.addInfo(Strings.teleportMenuEveryoneAdded + '!', 0);
                }));
            if (listTeleportChars.Count > 0)
            {
                menuBuilder
                    .addItem(Strings.teleportMenuRemoveCharacter, new MenuAction(() => ShowListChars(TeleportStatus.Delete)))
                    .addItem(Strings.deleteAll, new MenuAction(() =>
                    {
                        for (int i = listTeleportChars.Count - 1; i >= 0; i--)
                        {
                            if (listTeleportChars[i] != charAutoTeleportTo)
                                listTeleportChars.RemoveAt(i);
                        }
                        SaveData();
                        GameScr.info1.addInfo(Strings.teleportMenuCleared + '!', 0);
                    }));
            }
            menuBuilder.start();
        }

        internal static void LoadData()
        {
            try
            {
                if (isDataLoaded)
                    return;
                foreach (string str in Utils.LoadDataString($"teleport_list_{GameMidlet.IP}_{GameMidlet.PORT}").Split('|'))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] s = str.Split(',');
                            TeleportChar teleportChar = new TeleportChar(s[0], int.Parse(s[1]), long.Parse(s[2]));
                            if (listTeleportChars.Contains(teleportChar)) 
                                continue;
                            listTeleportChars.Add(teleportChar);
                        }
                    }
                    catch { }
                }
                isDataLoaded = true;
            }
            catch (Exception) { }
        }

        internal static void SaveData()
        {
            string data = "";
            foreach (TeleportChar teleportChar in listTeleportChars)
                data += teleportChar.Name + "," + teleportChar.ID + "," + teleportChar.LastTimeTeleportTo + "|";
            Utils.SaveData($"teleport_list_{GameMidlet.IP}_{GameMidlet.PORT}", data);
        }

        static void ShowListChars(TeleportStatus status)
        {
            var menuBuilder = new MenuBuilder();
            int i = 0;
            int count = listTeleportChars.Count;
            while (menuBuilder.menuItems.Count < (count < 5 ? count : 5))
            {
                TeleportChar teleportChar = listTeleportChars.ElementAt(i);
                i++;
                if (status == TeleportStatus.Delete)
                {
                    if (teleportChar == charAutoTeleportTo)
                    {
                        count--;
                        continue;
                    }
                    menuBuilder.addItem($"{teleportChar.Name}\n[{teleportChar.ID}]", new MenuAction(() =>
                    {
                        listTeleportChars.Remove(teleportChar);
                        SaveData();
                        GameScr.info1.addInfo(string.Format(Strings.teleportMenuCharacterRemoved, teleportChar) + '!', 0);
                    }));
                }
                else
                {
                    menuBuilder.addItem($"{teleportChar.Name}\n[{teleportChar.ID}]", new MenuAction(() =>
                    {
                        GameScr.info1.addInfo(string.Format(Strings.teleportMenuTeleportingToCharacter, teleportChar) + "...", 0);
                        TeleportToPlayer(teleportChar.ID);
                        teleportChar.LastTimeTeleportTo = mSystem.currentTimeMillis();
                    }));
                }
            }
            menuBuilder.addItem(listTeleportChars.Count > 5,
                Strings.more + "...", new MenuAction(() =>
                {
                    currentTeleportStatus = status;
                    showTeleportCharListPanel();
                }));
            menuBuilder.start();
            if (menuBuilder.menuItems.Count <= 0)
                GameScr.info1.addInfo(Strings.teleportMenuNoRemovableChar + '!', 0);
        }

        static void showTeleportCharListPanel()
        {
            SortList();
            CustomPanelMenu.Show(new CustomPanelMenuConfig()
            {
                SetTabAction = SetTabTeleportListPanel, 
                DoFireItemAction = DoFireTeleportListPanel, 
                PaintTabHeaderAction = PaintTabHeader, 
                PaintAction = PaintTeleportListPanel
            });
        }

        internal static void Update()
        {
            if (GameCanvas.gameTick % (60 * Time.timeScale) == 0)
            {
                foreach (TeleportChar teleportChar in listTeleportChars.Where(tC => tC.Name == "no name"))
                {
                    Char c = GameScr.findCharInMap(teleportChar.ID);
                    if (c == null) 
                        continue;
                    teleportChar.Name = c.cName;
                }
            }
            if (isAutoTeleportTo)
            {
                bool isCharInMap = false;
                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    Char c = (Char)GameScr.vCharInMap.elementAt(i);
                    if (c.charID == charAutoTeleportTo.ID)
                    {
                        isCharInMap = true;
                        break;
                    }
                }
                if (GameCanvas.gameTick % 30 * Time.timeScale == 0)
                {
                    if (isCharInMap && previousDisguiseId != -1 && !isChangeDisguise)
                        new Thread(() =>
                        {
                            isChangeDisguise = true;
                            for (int j = 0; j < Char.myCharz().arrItemBag.Length; j++)
                            {
                                Item item = Char.myCharz().arrItemBag[j];
                                if (item != null && item.template.id == previousDisguiseId)
                                {
                                    do
                                    {
                                        Service.gI().getItem(4, (sbyte)j);
                                        Thread.Sleep(500);
                                    }
                                    while (Char.myCharz().arrItemBody[5].template.id != previousDisguiseId);
                                    break;
                                }
                            }
                            previousDisguiseId = -1;
                        }).Start();
                }
                if (!isCharInMap && isAutoTeleportTo && mSystem.currentTimeMillis() - lastTimeAutoTeleportTo >= 2000)
                {
                    lastTimeAutoTeleportTo = mSystem.currentTimeMillis();
                    if (previousDisguiseId == -1) 
                        new Thread(() => 
                    {
                        if (Char.myCharz().arrItemBody[5] == null || Char.myCharz().arrItemBody[5] != null && (Char.myCharz().arrItemBody[5].template.id < 592 || Char.myCharz().arrItemBody[5].template.id > 594))
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
                        TeleportToPlayer(charAutoTeleportTo.ID, false);
                    }).Start();
                    charAutoTeleportTo.LastTimeTeleportTo = mSystem.currentTimeMillis();
                }
            }
        }

        static void SortList()
        {
            listTeleportChars_orderByName = listTeleportChars.OrderBy(tC => tC.Name).ToList();
            listTeleportChars = listTeleportChars.OrderBy(tC => -tC.LastTimeTeleportTo).ToList();
        }

        internal static void SetTabTeleportListPanel(Panel panel)
        {
            SetTabPanelTemplates.setTabListTemplate(panel, listTeleportChars);
        }

        internal static void DoFireTeleportListPanel(Panel panel)
        {
            listTeleportChars_orderByName = listTeleportChars.OrderBy(tC => tC.Name).ToList();
            if (panel.selected < 0)
                return;
            string str = "";
            Action action = null;
            var teleportChar = listTeleportChars_orderByName[panel.selected];

            switch (currentTeleportStatus)
            {
                case TeleportStatus.TeleportTo:
                    str = Strings.goTo;
                    action = () =>
                    {
                        GameScr.info1.addInfo(string.Format(Strings.teleportMenuTeleportingToCharacter, teleportChar) + "...", 0);
                        TeleportToPlayer(teleportChar.ID);
                        teleportChar.LastTimeTeleportTo = mSystem.currentTimeMillis();
                    };
                    break;
                case TeleportStatus.Delete:
                    str = Strings.delete;
                    action = () =>
                    {
                        if (teleportChar != charAutoTeleportTo)
                        {
                            listTeleportChars.Remove(teleportChar);
                            SaveData();
                            GameScr.info1.addInfo(string.Format(Strings.teleportMenuCharacterRemoved, teleportChar) + '!', 0);
                            showTeleportCharListPanel();
                        }
                        else 
                            GameCanvas.startOKDlg(Strings.teleportMenuCantRemoveTargetChar + '!');
                    };
                    break;
                case TeleportStatus.AutoTeleportTo:
                    str = Strings.teleportMenuAutoTeleportTo;
                    action = () =>
                    {
                        currentTeleportStatus = TeleportStatus.TeleportTo;
                        showTeleportCharListPanel();
                    };
                    break;
            }

            new MenuBuilder()
                .addItem(str, new MenuAction(action))
                .setPos(panel.X, (panel.selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll)
                .start();

            panel.cp = new ChatPopup();
            panel.cp.isClip = false;
            panel.cp.sayWidth = 180;
            panel.cp.cx = 3 + panel.X - (panel.X != 0 ? Res.abs(panel.cp.sayWidth - panel.W) + 8 : 0);
            panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + listTeleportChars_orderByName[panel.selected].Name + "\n|6|ID: " + listTeleportChars_orderByName[panel.selected].ID, panel.cp.sayWidth - 10);
            panel.cp.delay = 10000000;
            panel.cp.c = null;
            panel.cp.sayRun = 7;
            panel.cp.ch = 15 - panel.cp.sayRun + panel.cp.says.Length * 12 + 10;
            if (panel.cp.ch > GameCanvas.h - 80)
            {
                panel.cp.ch = GameCanvas.h - 80;
                panel.cp.lim = panel.cp.says.Length * 12 - panel.cp.ch + 17;
                if (panel.cp.lim < 0)
                    panel.cp.lim = 0;
                ChatPopup.cmyText = 0;
                panel.cp.isClip = true;
            }
            panel.cp.cy = GameCanvas.menu.menuY - panel.cp.ch;
            while (panel.cp.cy < 10)
            {
                panel.cp.cy++;
                GameCanvas.menu.menuY++;
            }
            panel.cp.mH = 0;
            panel.cp.strY = 10;
        }

        static void PaintTabHeader(Panel panel, mGraphics g)
        {
            PaintPanelTemplates.PaintTabHeaderTemplate(panel, g, Strings.teleportMenuCharacterList);
        }

        internal static void PaintTeleportListPanel(Panel panel, mGraphics g)
        {
            PaintPanelTemplates.PaintCollectionCaptionAndDescriptionTemplate(panel, g, listTeleportChars_orderByName,
                c => c.Name, c => $"ID: {c.ID}");
        }

        static void TeleportToPlayer(int charId, bool isAutoUseYardrat = true)
        {
            GameEvents.OnGotoPlayer(charId, isAutoUseYardrat);
        }

        internal enum TeleportStatus
        {
            TeleportTo,
            Delete,
            AutoTeleportTo
        }
    }
}