using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod;
public class TeleportMenu : IChatable, IActionListener
{
    public static List<TeleportChar> listTeleportChars = new List<TeleportChar>();

    static TeleportMenu _Instance;

    static string[] inputCharID = new string[2] { "Nhập CharID", "CharID" };

    public const int TYPE_TELEPORT_LIST = 27;

    static bool currentState;

    public static bool isDataLoaded;

    public static TeleportMenu getInstance()
    {
        _Instance ??= new TeleportMenu();
        return _Instance;
    }

    [ChatCommand("tele"), HotkeyCommand('z')]
    public static void ShowMenu()
    {
        MyVector myVector = new MyVector();
        if (listTeleportChars.Count > 0) myVector.addElement(new Command("Danh sách\nnhân vật\nđã lưu", getInstance(), 1, null));
        Char c;
        if (Char.myCharz().charFocus != null && CharExtensions.isNormalChar(Char.myCharz().charFocus)) c = Char.myCharz().charFocus;
        else c = CharExtensions.ClosestChar(70, true);
        if (c != null)
        {
            TeleportChar teleportChar = new TeleportChar(c);
            if (!listTeleportChars.Contains(teleportChar)) myVector.addElement(new Command($"Thêm\n{teleportChar.cName}\n[{teleportChar.charID}]", getInstance(), 2, teleportChar));
        }
        if (Char.myCharz().charFocus != null && CharExtensions.isNormalChar(Char.myCharz().charFocus))
        {
            TeleportChar teleportChar = new TeleportChar(Char.myCharz().charFocus);
            if (listTeleportChars.Contains(teleportChar)) myVector.addElement(new Command($"Xóa\n{teleportChar.cName}\n[{teleportChar.charID}]", getInstance(), 3, teleportChar));
        }

        myVector.addElement(new Command("Thêm nhân\nvật bằng\ncharID", getInstance(), 4, null));
        if (GameScr.vCharInMap.size() > 1) myVector.addElement(new Command("Thêm tất\ncả người\ntrong map", getInstance(), 5, null));
        if (listTeleportChars.Count > 0)
        {
            myVector.addElement(new Command("Xóa\nnhân vật\nđã lưu", getInstance(), 6, null));
            myVector.addElement(new Command("Xóa tất\ncả", getInstance(), 7, null));
        }
        GameCanvas.menu.startAt(myVector, 3);
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
                if (charId < 0) throw new Exception();
                listTeleportChars.Add(new TeleportChar(charId));
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
                ShowListChars(false);
                break;
            case 2:
                listTeleportChars.Insert(0, teleportChar);
                GameScr.info1.addInfo($"Đã thêm {teleportChar}!", 0);
                break;
            case 3:
                listTeleportChars.Remove(teleportChar);
                GameScr.info1.addInfo($"Đã xóa nhân vật {teleportChar}!", 0);
                break;
            case 4:
                ChatTextField.gI().strChat = inputCharID[0];
                ChatTextField.gI().tfChat.name = inputCharID[1];
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
                break;
            case 5:
                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                    if (CharExtensions.isNormalChar(@char))
                    {
                        TeleportChar teleportChar1 = new TeleportChar(@char);
                        if (!listTeleportChars.Contains(teleportChar1)) listTeleportChars.Add(teleportChar1);
                    }
                }
                GameScr.info1.addInfo("Đã thêm toàn bộ nhân vật trong map!", 0);
                break;
            case 6:
                ShowListChars(true);
                break;
            case 7:
                listTeleportChars.Clear();
                GameScr.info1.addInfo("Đã xóa toàn bộ nhân vật đã lưu!", 0);
                break;
            case 8:
                GameScr.info1.addInfo($"Dịch chuyển đến nhân vật {teleportChar.cName}!", 0);
                Service.gI().gotoPlayer(teleportChar.charID);
                listTeleportChars[listTeleportChars.FindIndex(tC => tC == teleportChar)].lastTimeTeleportTo = mSystem.currentTimeMillis();
                break;
            case 9:
                listTeleportChars.Remove(teleportChar);
                GameScr.info1.addInfo($"Đã xóa nhân vật {teleportChar.cName}!", 0);
                setTypeTeleportListPanel();
                break;
            case 10:
                currentState = false;
                setTypeTeleportListPanel();
                break;
            case 11:
                currentState = true;
                setTypeTeleportListPanel();
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
                        TeleportChar teleportChar = new TeleportChar(s[0], int.Parse(s[1]), long.Parse(s[3]));
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
        string data = string.Empty;
        foreach (TeleportChar teleportChar in listTeleportChars)
        {
            data += teleportChar.cName + "," + teleportChar.charID + "," + teleportChar.lastTimeTeleportTo + "|"; 
        }
        Utilities.saveRMSString($"teleportlist_{GameMidlet.IP}_{GameMidlet.PORT}", data);
    }

    static void ShowListChars(bool isDelete)
    {
        MyVector myVector = new MyVector();
        for (int i = 0; i < (listTeleportChars.Count > 5 ? 5 : listTeleportChars.Count); i++)
        {
            TeleportChar teleportChar = listTeleportChars.ElementAt(i);
            myVector.addElement(new Command(teleportChar.cName + "\n[" + teleportChar.charID + "]", getInstance(), 8 + (isDelete ? 1 : 0), teleportChar));
        }
        if (listTeleportChars.Count > 5) myVector.addElement(new Command("Thêm nữa", getInstance(), 10 + (isDelete ? 1 : 0), null));
        GameCanvas.menu.startAt(myVector, 3);
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
    }

    static void SortList()
    {
        listTeleportChars = listTeleportChars.OrderBy(tC => -tC.lastTimeTeleportTo).ToList();
    }

    static void setTypeTeleportListPanel()
    {
        GameCanvas.panel.type = TYPE_TELEPORT_LIST;
        GameCanvas.panel.setType(0);
        SoundMn.gI().getSoundOption();
        setTabTeleportListPanel();
        GameCanvas.panel.show();
    }

    public static void setTabTeleportListPanel()
    {
        GameCanvas.panel.ITEM_HEIGHT = 24;
        GameCanvas.panel.currentListLength = listTeleportChars.Count;
        GameCanvas.panel.selected = (GameCanvas.isTouch ? (-1) : 0);
        GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
        if (GameCanvas.panel.cmyLim < 0) GameCanvas.panel.cmyLim = 0;
        GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex];
        if (GameCanvas.panel.cmy < 0) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = 0;
        if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim;
    }

    public static void doFireTeleportListPanel()
    {
        if (GameCanvas.panel.selected < 0) return;
        MyVector myVector = new MyVector();
        string str = "";
        int cmd = 0;
        if (currentState == true)
        {
            str = mResources.DELETE;
            cmd = 9;
        }
        if (currentState == false)
        {
            str = mResources.den;
            cmd = 8;
        }
        myVector.addElement(new Command(str, getInstance(), cmd, listTeleportChars.OrderBy(tC => tC.cName).ToList()[GameCanvas.panel.selected]));
        GameCanvas.menu.startAt(myVector, GameCanvas.panel.X, (GameCanvas.panel.selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);
        GameCanvas.panel.cp = new ChatPopup();
        GameCanvas.panel.cp.isClip = false;
        GameCanvas.panel.cp.sayWidth = 180;
        GameCanvas.panel.cp.cx = 3 + GameCanvas.panel.X - ((GameCanvas.panel.X != 0) ? (Res.abs(GameCanvas.panel.cp.sayWidth - GameCanvas.panel.W) + 8) : 0);
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
            g.setColor((i != GameCanvas.panel.selected) ? 15196114 : 16383818);
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
}
