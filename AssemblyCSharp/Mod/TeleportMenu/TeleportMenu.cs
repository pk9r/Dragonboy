using Mod.ModHelper.Menu;
using Mod.ModMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Mod.TeleportMenu
{
    public class TeleportMenu : IChatable
    {
        public static List<TeleportChar> listTeleportChars = new List<TeleportChar>();
        private static TeleportMenu _Instance;
        private static string[] inputCharID = new string[2] { "Nhập CharID", "CharID" };
        private static TeleportStatus currentTeleportStatus;
        private static bool isDataLoaded, isAutoTeleportTo;
        private static TeleportChar charAutoTeleportTo;

        private static long lastTimeAutoTeleportTo;
        private static bool isChangeDisguise;
        private static int previousDisguiseId = -1;

        public static TeleportMenu getInstance()
        {
            if (_Instance == null) _Instance = new TeleportMenu();
            return _Instance;
        }

        [ChatCommand("tele"), HotkeyCommand('z')]
        public static void ShowMenu()
        {
            OpenMenu.start(new(menuItems =>
            {
                if (listTeleportChars.Count > 0)
                    menuItems.Add(new("Danh sách\nnhân vật\nđã lưu", new(() => ShowListChars(TeleportStatus.TeleportTo))));
                Char c;
                Char charFocus = Char.myCharz().charFocus;
                if (charFocus != null && charFocus.isNormalChar())
                    c = charFocus;
                else
                    c = CharExtensions.ClosestChar(maxDistance: 70, isNormalCharOnly: true);

                if (c != null)
                {
                    var teleportChar = new TeleportChar(c);
                    if (!listTeleportChars.Contains(teleportChar))
                        menuItems.Add(new($"Thêm\n{teleportChar.cName}\n[{teleportChar.charID}]", new(() =>
                        {
                            listTeleportChars.Insert(0, teleportChar);
                            SaveData();
                            GameScr.info1.addInfo($"Đã thêm nhân vật {teleportChar}!", 0);
                        })));
                }

                if (charFocus != null && charFocus.isNormalChar())
                {
                    var teleportChar = new TeleportChar(charFocus);
                    if (listTeleportChars.Contains(teleportChar) && ((isAutoTeleportTo && charAutoTeleportTo != teleportChar) || !isAutoTeleportTo))
                        menuItems.Add(new($"Xóa\n{teleportChar.cName}\n[{teleportChar.charID}]", new(() =>
                        {
                            if (isAutoTeleportTo && teleportChar != charAutoTeleportTo)
                            {
                                listTeleportChars.Remove(teleportChar);
                                SaveData();
                                GameScr.info1.addInfo($"Đã xóa nhân vật {teleportChar}!", 0);
                            }
                            else GameScr.info1.addInfo($"Không thể xóa nhân vật đang auto dịch chuyển!", 0);
                        })));
                }
                if (listTeleportChars.Count > 0)
                    menuItems.Add(new(isAutoTeleportTo ? "Dừng auto\ndịch chuyển" : "Auto dịch\nchuyển đến\nnhân vật", new(() =>
                    {
                        if (!isAutoTeleportTo) ShowListChars(TeleportStatus.AutoTeleportTo);
                        else
                        {
                            isAutoTeleportTo = false;
                            charAutoTeleportTo = null;
                            GameScr.info1.addInfo($"Đã dừng auto dịch chuyển đến nhân vật {charAutoTeleportTo.cName}!", 0);
                        }
                    })));
                menuItems.Add(new("Thêm nhân\nvật bằng\ncharID", new(() =>
                {
                    ChatTextField.gI().strChat = inputCharID[0];
                    ChatTextField.gI().tfChat.name = inputCharID[1];
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                    ChatTextField.gI().startChat2(getInstance(), string.Empty);
                })));
                if (GameScr.vCharInMap.size() > 1)
                    menuItems.Add(new("Thêm tất\ncả người\ntrong map", new(() =>
                    {
                        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                        {
                            Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                            if (CharExtensions.isNormalChar(@char, false, false))
                            {
                                TeleportChar teleportChar1 = new TeleportChar(@char);
                                if (!listTeleportChars.Contains(teleportChar1)) listTeleportChars.Add(teleportChar1);
                            }
                        }
                        SaveData();
                        GameScr.info1.addInfo("Đã thêm toàn bộ nhân vật trong map!", 0);
                    })));
                if (listTeleportChars.Count > 0)
                {
                    menuItems.Add(new("Xóa\nnhân vật\nđã lưu", new(() => ShowListChars(TeleportStatus.Delete))));
                    menuItems.Add(new("Xóa tất\ncả", new(() =>
                    {
                        for (int i = listTeleportChars.Count - 1; i >= 0; i--)
                        {
                            if (listTeleportChars[i] != charAutoTeleportTo) listTeleportChars.RemoveAt(i);
                        }
                        SaveData();
                        GameScr.info1.addInfo("Đã xóa toàn bộ nhân vật đã lưu!", 0);
                    })));
                }
            }));
        }

        public void onCancelChat()
        {
            ChatTextField.gI().isShow = false;
            Utilities.ResetTF();
        }

        public void onChatFromMe(string text, string to)
        {
            if (!string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()) && !string.IsNullOrEmpty(text))
            {
                try
                {
                    int charId = int.Parse(text);
                    if (charId < 0)
                    {
                        GameCanvas.startOKDlg("CharID phải lớn hơn hoặc bằng 0!");
                        return;
                    }
                    listTeleportChars.Add(new TeleportChar(charId));
                    SaveData();
                    GameScr.info1.addInfo($"Đã thêm nhân vật với CharID {text}!", 0);

                }
                catch (Exception)
                {
                    GameScr.info1.addInfo("Đã xảy ra lỗi!", 0);
                }
            }
            else ChatTextField.gI().isShow = false;
            Utilities.ResetTF();
            SortList();
        }

        public void perform(int idAction, object p)
        {
            TeleportChar teleportChar = null;
            if (p is TeleportChar c)
            {
                teleportChar = c;
            }
            switch (idAction)
            {
                case 1:
                    ShowListChars(TeleportStatus.TeleportTo);
                    break;
                case 2:
                    listTeleportChars.Insert(0, teleportChar);
                    SaveData();
                    GameScr.info1.addInfo($"Đã thêm nhân vật {teleportChar}!", 0);
                    break;
                case 3:
                    if (isAutoTeleportTo && teleportChar != charAutoTeleportTo)
                    {
                        listTeleportChars.Remove(teleportChar);
                        SaveData();
                        GameScr.info1.addInfo($"Đã xóa nhân vật {teleportChar}!", 0);
                    }
                    else GameScr.info1.addInfo($"Không thể xóa nhân vật đang auto dịch chuyển!", 0);
                    break;
                case 4:
                    if (!isAutoTeleportTo) ShowListChars(TeleportStatus.AutoTeleportTo);
                    else
                    {
                        isAutoTeleportTo = false;
                        charAutoTeleportTo = null;
                        GameScr.info1.addInfo($"Đã dừng auto dịch chuyển đến nhân vật {charAutoTeleportTo.cName}!", 0);
                    }
                    break;
                case 5:
                    ChatTextField.gI().strChat = inputCharID[0];
                    ChatTextField.gI().tfChat.name = inputCharID[1];
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                    ChatTextField.gI().startChat2(getInstance(), string.Empty);
                    break;
                case 6:
                    for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                    {
                        Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                        if (CharExtensions.isNormalChar(@char, false, false))
                        {
                            TeleportChar teleportChar1 = new TeleportChar(@char);
                            if (!listTeleportChars.Contains(teleportChar1)) listTeleportChars.Add(teleportChar1);
                        }
                    }
                    SaveData();
                    GameScr.info1.addInfo("Đã thêm toàn bộ nhân vật trong map!", 0);
                    break;
                case 7:
                    ShowListChars(TeleportStatus.Delete);
                    break;
                case 8:
                    for (int i = listTeleportChars.Count - 1; i >= 0; i--)
                    {
                        if (listTeleportChars[i] != charAutoTeleportTo) listTeleportChars.RemoveAt(i);
                    }
                    SaveData();
                    GameScr.info1.addInfo("Đã xóa toàn bộ nhân vật đã lưu!", 0);
                    break;
                case 9:
                    GameScr.info1.addInfo($"Dịch chuyển đến nhân vật {teleportChar.cName}!", 0);
                    TeleportToPlayer(teleportChar.charID);
                    listTeleportChars[listTeleportChars.FindIndex(tC => tC == teleportChar)].lastTimeTeleportTo = mSystem.currentTimeMillis();
                    break;
                case 10:
                    if (teleportChar != charAutoTeleportTo)
                    {
                        listTeleportChars.Remove(teleportChar);
                        SaveData();
                        GameScr.info1.addInfo($"Đã xóa nhân vật {teleportChar.cName}!", 0);
                        ModMenuPanel.setTypeModMenuMain(1);
                    }
                    else GameCanvas.startOKDlg("Không thể xóa nhân vật đang auto dịch chuyển!");
                    break;
                case 11:
                    isAutoTeleportTo = true;
                    charAutoTeleportTo = teleportChar;
                    GameScr.info1.addInfo($"Auto dịch chuyển đến nhân vật {teleportChar.cName}!", 0);
                    break;
                case 12:
                    currentTeleportStatus = TeleportStatus.TeleportTo;
                    ModMenuPanel.setTypeModMenuMain(1);
                    GameCanvas.panel.show();
                    break;
                case 13:
                    currentTeleportStatus = TeleportStatus.Delete;
                    ModMenuPanel.setTypeModMenuMain(1);
                    GameCanvas.panel.show();
                    break;
                case 14:
                    currentTeleportStatus = TeleportStatus.AutoTeleportTo;
                    ModMenuPanel.setTypeModMenuMain(1);
                    GameCanvas.panel.show();
                    break;
            }
            SortList();
        }

        public static void LoadData()
        {
            try
            {
                if (!isDataLoaded) foreach (string str in Utilities.loadRMSString($"teleportlist_{GameMidlet.IP}_{GameMidlet.PORT}").Split('|'))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                string[] s = str.Split(',');
                                TeleportChar teleportChar = new TeleportChar(s[0], int.Parse(s[1]), long.Parse(s[2]));
                                if (listTeleportChars.Contains(teleportChar)) continue;
                                listTeleportChars.Add(teleportChar);
                            }
                        }
                        catch (Exception) { }
                    }
                isDataLoaded = true;
            }
            catch (Exception) { }
        }

        public static void SaveData()
        {
            string data = "";
            foreach (TeleportChar teleportChar in listTeleportChars)
            {
                data += teleportChar.cName + "," + teleportChar.charID + "," + teleportChar.lastTimeTeleportTo + "|";
            }
            Utilities.saveRMSString($"teleportlist_{GameMidlet.IP}_{GameMidlet.PORT}", data);
        }

        private static void ShowListChars(TeleportStatus status)
        {
            OpenMenu.start(new(menuItems =>
            {
                int i = 0;
                int count = listTeleportChars.Count;
                while (menuItems.Count < (count < 5 ? count : 5))
                {
                    TeleportChar teleportChar = listTeleportChars.ElementAt(i);
                    i++;
                    if (status == TeleportStatus.Delete && teleportChar == charAutoTeleportTo)
                    {
                        count--;
                        continue;
                    }
                    menuItems.Add(new(teleportChar.cName + "\n[" + teleportChar.charID + "]", new(() =>
                    {
                        GameScr.info1.addInfo($"Dịch chuyển đến nhân vật {teleportChar.cName}!", 0);
                        TeleportToPlayer(teleportChar.charID);
                        listTeleportChars[listTeleportChars.FindIndex(tC => tC == teleportChar)].lastTimeTeleportTo = mSystem.currentTimeMillis();
                    })));
                }
                if (listTeleportChars.Count > 5)
                    menuItems.Add(new("Thêm nữa", new(() =>
                    {
                        currentTeleportStatus = status;
                        ModMenuPanel.setTypeModMenuMain(1);
                        GameCanvas.panel.show();
                    })));
                if (menuItems.Count <= 0)
                    GameScr.info1.addInfo("Danh sách nhân vật xóa được trống!", 0);
            }));

        }

        public static void Update()
        {
            if (GameCanvas.gameTick % (60 * Time.timeScale) == 0)
            {
                foreach (TeleportChar teleportChar in listTeleportChars.Where(tC => tC.cName == "Không tên"))
                {
                    Char c = GameScr.findCharInMap(teleportChar.charID);
                    if (c == null) continue;
                    listTeleportChars[listTeleportChars.FindIndex(tC => tC == teleportChar)].cName = c.cName;
                }
            }
            if (isAutoTeleportTo)
            {
                bool isCharInMap = false;
                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    Char c = (Char)GameScr.vCharInMap.elementAt(i);
                    if (c.charID == charAutoTeleportTo.charID)
                    {
                        isCharInMap = true;
                        break;
                    }
                }
                if (GameCanvas.gameTick % 30 * Time.timeScale == 0)
                {
                    if (isCharInMap && previousDisguiseId != -1 && !isChangeDisguise)
                    {
                        new Thread(delegate ()
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
                }
                if (!isCharInMap && isAutoTeleportTo && mSystem.currentTimeMillis() - lastTimeAutoTeleportTo >= 2000)
                {
                    lastTimeAutoTeleportTo = mSystem.currentTimeMillis();
                    if (previousDisguiseId == -1) new Thread(delegate ()
                    {
                        if (Char.myCharz().arrItemBody[5] == null || Char.myCharz().arrItemBody[5] != null && (Char.myCharz().arrItemBody[5].template.id < 592 || Char.myCharz().arrItemBody[5].template.id > 594))
                        {
                            if (Char.myCharz().arrItemBody[5] != null) previousDisguiseId = Char.myCharz().arrItemBody[5].template.id;
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
                        TeleportToPlayer(charAutoTeleportTo.charID, false);
                    }).Start();
                    listTeleportChars[listTeleportChars.FindIndex(tC => tC == charAutoTeleportTo)].lastTimeTeleportTo = mSystem.currentTimeMillis();
                }
            }
        }

        private static void SortList()
        {
            listTeleportChars = listTeleportChars.OrderBy(tC => -tC.lastTimeTeleportTo).ToList();
        }

        public static void setTabTeleportListPanel()
        {
            GameCanvas.panel.ITEM_HEIGHT = 24;
            GameCanvas.panel.currentListLength = listTeleportChars.Count;
            GameCanvas.panel.selected = GameCanvas.isTouch ? -1 : 0;
            GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
            if (GameCanvas.panel.cmyLim < 0) GameCanvas.panel.cmyLim = 0;
            GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex];
            if (GameCanvas.panel.cmy < 0) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = 0;
            if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim;
        }

        public static void doFireTeleportListPanel()
        {
            if (GameCanvas.panel.selected < 0) return;
            string str = "";
            Action action = null;
            var teleportChar = listTeleportChars.OrderBy(tC => tC.cName).ToList()[GameCanvas.panel.selected];

            switch (currentTeleportStatus)
            {
                case TeleportStatus.TeleportTo:
                    str = mResources.den;
                    action = () =>
                    {
                        GameScr.info1.addInfo($"Dịch chuyển đến nhân vật {teleportChar.cName}!", 0);
                        TeleportToPlayer(teleportChar.charID);
                        listTeleportChars[listTeleportChars.FindIndex(tC => tC == teleportChar)].lastTimeTeleportTo = mSystem.currentTimeMillis();
                    };
                    break;
                case TeleportStatus.Delete:
                    str = mResources.DELETE;
                    action = () =>
                    {
                        if (teleportChar != charAutoTeleportTo)
                        {
                            listTeleportChars.Remove(teleportChar);
                            SaveData();
                            GameScr.info1.addInfo($"Đã xóa nhân vật {teleportChar.cName}!", 0);
                            ModMenuPanel.setTypeModMenuMain(1);
                        }
                        else GameCanvas.startOKDlg("Không thể xóa nhân vật đang auto dịch chuyển!");
                    };
                    break;
                case TeleportStatus.AutoTeleportTo:
                    str = "Auto " + mResources.den.ToLower();
                    action = () =>
                    {
                        currentTeleportStatus = TeleportStatus.TeleportTo;
                        ModMenuPanel.setTypeModMenuMain(1);
                        GameCanvas.panel.show();
                    };
                    break;
                default:
                    break;
            }

            OpenMenu.start(
                new(menuItems => { menuItems.Add(new(str, new(action))); }),
                x: GameCanvas.panel.X,
                y: (GameCanvas.panel.selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);

            GameCanvas.panel.cp = new ChatPopup();
            GameCanvas.panel.cp.isClip = false;
            GameCanvas.panel.cp.sayWidth = 180;
            GameCanvas.panel.cp.cx = 3 + GameCanvas.panel.X - (GameCanvas.panel.X != 0 ? Res.abs(GameCanvas.panel.cp.sayWidth - GameCanvas.panel.W) + 8 : 0);
            GameCanvas.panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + listTeleportChars.OrderBy(tC => tC.cName).ToList()[GameCanvas.panel.selected].cName + "\n|6|CharID: " + listTeleportChars.OrderBy(tC => tC.cName).ToList()[GameCanvas.panel.selected].charID, GameCanvas.panel.cp.sayWidth - 10);
            GameCanvas.panel.cp.delay = 10000000;
            GameCanvas.panel.cp.c = null;
            GameCanvas.panel.cp.sayRun = 7;
            GameCanvas.panel.cp.ch = 15 - GameCanvas.panel.cp.sayRun + GameCanvas.panel.cp.says.Length * 12 + 10;
            if (GameCanvas.panel.cp.ch > GameCanvas.h - 80)
            {
                GameCanvas.panel.cp.ch = GameCanvas.h - 80;
                GameCanvas.panel.cp.lim = GameCanvas.panel.cp.says.Length * 12 - GameCanvas.panel.cp.ch + 17;
                if (GameCanvas.panel.cp.lim < 0)
                {
                    GameCanvas.panel.cp.lim = 0;
                }
                ChatPopup.cmyText = 0;
                GameCanvas.panel.cp.isClip = true;
            }
            GameCanvas.panel.cp.cy = GameCanvas.menu.menuY - GameCanvas.panel.cp.ch;
            while (GameCanvas.panel.cp.cy < 10)
            {
                GameCanvas.panel.cp.cy++;
                GameCanvas.menu.menuY++;
            }
            GameCanvas.panel.cp.mH = 0;
            GameCanvas.panel.cp.strY = 10;
        }

        public static void paintTeleportListPanel(mGraphics g)
        {
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
            g.setColor(0);
            if (listTeleportChars == null || listTeleportChars.Count != GameCanvas.panel.currentListLength) return;
            for (int i = 0; i < GameCanvas.panel.currentListLength; i++)
            {
                int num = GameCanvas.panel.xScroll;
                int num2 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int num3 = GameCanvas.panel.wScroll;
                int num4 = GameCanvas.panel.ITEM_HEIGHT - 1;
                g.setColor(i != GameCanvas.panel.selected ? 15196114 : 16383818);
                g.fillRect(num, num2, num3, num4);
                TeleportChar teleportChar = listTeleportChars.OrderBy(tC => tC.cName).ToList()[i];
                if (teleportChar != null)
                {
                    mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + teleportChar.cName, num + 5, num2, 0);
                    mFont.tahoma_7_blue.drawString(g, $"CharID: {teleportChar.charID}", num + 5, num2 + 11, 0);
                }
            }
            GameCanvas.panel.paintScrollArrow(g);
        }

        private static void TeleportToPlayer(int charId, bool isAutoUseYardrat = true)
        {
            Service.gI().gotoPlayer(charId, isAutoUseYardrat);
        }

        public enum TeleportStatus
        {
            TeleportTo,
            Delete,
            AutoTeleportTo
        }
    }
}