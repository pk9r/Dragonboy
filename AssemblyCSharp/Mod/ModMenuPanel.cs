using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod;
public class ModMenuPanel : IChatable
{
    static ModMenuPanel _Instance;

    public static ModMenuPanel getInstance()
    {
        _Instance ??= new ModMenuPanel();
        return _Instance;
    }

    public static void setTypeModMenu()
    {
        GameCanvas.panel.type = ModMenu.TYPE_MOD_MENU;
        GameCanvas.panel.tabName[ModMenu.TYPE_MOD_MENU] = new string[][]
        {
            new string[]{ "Bật/tắt", "" },
            new string[]{ "Điều", "chỉnh" },
        };
        GameCanvas.panel.setType(0);
        SoundMn.gI().getSoundOption();
        setTabModMenu();
    }

    public static void setTabModMenu()
    {
        GameCanvas.panel.ITEM_HEIGHT = 24;
        if (GameCanvas.panel.currentTabIndex == 0) GameCanvas.panel.currentListLength = ModMenu.modMenuItemBools.Length;
        else GameCanvas.panel.currentListLength = ModMenu.modMenuItemInts.Length;
        GameCanvas.panel.selected = (GameCanvas.isTouch ? (-1) : 0);
        GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
        if (GameCanvas.panel.cmyLim < 0) GameCanvas.panel.cmyLim = 0;
        GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex];
        if (GameCanvas.panel.cmy < 0) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = 0;
        if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim;
    }

    public static void doFireModMenu()
    {
        if (GameCanvas.panel.currentTabIndex == 0) doFireModMenuBools();
        else doFireModMenuInts();
    }

    static void doFireModMenuBools()
    {
        if (GameCanvas.panel.selected < 0) return;
        ModMenu.modMenuItemBools[GameCanvas.panel.selected].Value = !ModMenu.modMenuItemBools[GameCanvas.panel.selected].Value;
        onModMenuBoolsValueChanged();
    }

    static void doFireModMenuInts()
    {
        if (GameCanvas.panel.selected < 0) return;
        int selected = GameCanvas.panel.selected;
        if (ModMenu.modMenuItemInts[selected].Values != null) ModMenu.modMenuItemInts[selected].SwitchSelection();
        else
        {
            ChatTextField.gI().strChat = ModMenu.inputModMenuItemInts[selected][0];
            ChatTextField.gI().tfChat.name = ModMenu.inputModMenuItemInts[selected][1];
            ChatTextField.gI().startChat2(getInstance(), string.Empty);
            GameCanvas.panel.hide();
        }     
    }

    public static void paintModMenu(mGraphics g)
    {
        if (GameCanvas.panel.currentTabIndex == 0) paintModMenuBools(g);
        else paintModMenuInt(g);
    }

    static void paintModMenuBools(mGraphics g)
    {
        g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
        g.translate(0, -GameCanvas.panel.cmy);
        g.setColor(0);
        if (ModMenu.modMenuItemBools == null || ModMenu.modMenuItemBools.Length != GameCanvas.panel.currentListLength) return;
        bool isReset = true;
        string descriptionTextInfo = string.Empty;
        int x = 0, y = 0;
        for (int i = 0; i < GameCanvas.panel.currentListLength; i++)
        {
            int num = GameCanvas.panel.xScroll;
            int num2 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
            int num3 = GameCanvas.panel.wScroll;
            int num4 = GameCanvas.panel.ITEM_HEIGHT - 1;
            g.setColor((i != GameCanvas.panel.selected) ? 15196114 : 16383818);
            g.fillRect(num, num2, num3, num4);
            ModMenuItemBoolean modMenuItem = ModMenu.modMenuItemBools[i];
            if (modMenuItem != null)
            {
                mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, num + 5, num2, 0);
                string description = modMenuItem.Description.Length > 28 ? (modMenuItem.Description.Substring(0, 27) + "...") : modMenuItem.Description;
                if (i == GameCanvas.panel.selected && modMenuItem.Description.Length > 26 && !GameCanvas.panel.isClose)
                {
                    isReset = false;
                    descriptionTextInfo = modMenuItem.Description;
                    x = num + 5;
                    y = num2 + 11;
                }
                else mFont.tahoma_7_blue.drawString(g, description, num + 5, num2 + 11, 0);
                mFont mf = mFont.tahoma_7_grey;
                if (modMenuItem.Value) mf = mFont.tahoma_7b_red;
                string str = mResources.status + ": ";
                mf.drawString(g, (str == "Trạng thái: " ? "Đang " : str) + (modMenuItem.Value ? mResources.ON.ToLower() : mResources.OFF.ToLower()), num + num3 - 2, num2 + GameCanvas.panel.ITEM_HEIGHT - 14, mFont.RIGHT);
            }
        }
        if (isReset) TextInfo.reset();
        else
        {
            TextInfo.paint(g, descriptionTextInfo, x, y, 118, 15, mFont.tahoma_7_blue);
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
        }
        GameCanvas.panel.paintScrollArrow(g);
    }

    static void paintModMenuInt(mGraphics g)
    {
        g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
        g.translate(0, -GameCanvas.panel.cmy);
        g.setColor(0);
        if (ModMenu.modMenuItemInts == null || ModMenu.modMenuItemInts.Length != GameCanvas.panel.currentListLength) return;
        bool isReset = true;
        string descriptionTextInfo = string.Empty;
        int x = 0, y = 0;
        for (int i = 0; i < GameCanvas.panel.currentListLength; i++)
        {
            int num = GameCanvas.panel.xScroll;
            int num2 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
            int num3 = GameCanvas.panel.wScroll;
            int num4 = GameCanvas.panel.ITEM_HEIGHT - 1;
            g.setColor((i != GameCanvas.panel.selected) ? 15196114 : 16383818);
            g.fillRect(num, num2, num3, num4);
            ModMenuItemInt modMenuItem = ModMenu.modMenuItemInts[i];
            if (modMenuItem != null)
            {
                string description, str;
                mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, num + 5, num2, 0);
                if (modMenuItem.Values != null)
                {
                    str = modMenuItem.getSelectedValue();
                    description = str.Length > 28 ? (str.Substring(0, 27) + "...") : str;
                }
                else
                {
                    str = modMenuItem.Description;
                    description = str.Length > 35 ? (str.Substring(0, 34) + "...") : str;
                    mFont.tahoma_7b_red.drawString(g, modMenuItem.SelectedValue.ToString(), num + num3 - 2, num2 + GameCanvas.panel.ITEM_HEIGHT - 14, mFont.RIGHT);
                }
                if (i == GameCanvas.panel.selected && str.Length > 26 && !GameCanvas.panel.isClose)
                {
                    isReset = false;
                    descriptionTextInfo = modMenuItem.Description;
                    x = num + 5;
                    y = num2 + 11;
                }
                else mFont.tahoma_7_blue.drawString(g, description, num + 5, num2 + 11, 0);
            }
        }
        if (isReset) TextInfo.reset();
        else
        {
            TextInfo.paint(g, descriptionTextInfo, x, y, 130, 15, mFont.tahoma_7_blue);
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
        }
        GameCanvas.panel.paintScrollArrow(g);
    }

    public void onChatFromMe(string text, string to)
    {
        if (!string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()) && !string.IsNullOrEmpty(text))
        {
            int selected = GameCanvas.panel.selected;
            if (ChatTextField.gI().strChat == ModMenu.inputModMenuItemInts[selected][0])
            {
                try
                {
                    int value = int.Parse(text);
                    if (value > 60 || value < 5) throw new Exception();
                    ModMenu.modMenuItemInts[selected].setValue(value);
                    onModMenuChatFromMe();
                    GameScr.info1.addInfo("Đã thay đổi mức FPS!", 0);
                }
                catch
                {
                    GameCanvas.startOKDlg("Mức FPS không hợp lệ!");
                }
            }
        }
        else ChatTextField.gI().isShow = false;
        Utilities.ResetTF();
    }

    private void onModMenuChatFromMe()
    {
        Application.targetFrameRate = ModMenu.modMenuItemInts[0].SelectedValue;
    }

    public void onCancelChat()
    {
        ChatTextField.gI().isShow = false;
        Utilities.ResetTF();
    }

    static void onModMenuBoolsValueChanged()
    {
        if (ModMenu.modMenuItemBools[0].Value) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
        CharEffect.isEnabled = ModMenu.modMenuItemBools[1].Value;
        AutoAttack.gI.toggle(ModMenu.modMenuItemBools[2].Value);
    }

    static void onModMenuIntsValueChanged()
    {

    }
}
