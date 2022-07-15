using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod;
public class ModMenuPanel
{
    public static void setTypeModMenu()
    {
        GameCanvas.panel.type = 26;
        GameCanvas.panel.tabName[26] = new string[][]
        {
            new string[]{ "Bật/tắt", "" },
            new string[]{ "Điều", "chỉnh" },
        };
        GameCanvas.panel.setType(0);
        SoundMn.gI().getSoundOption();
        GameCanvas.panel.currentListLength = ModMenu.modMenuItemBools.Length;
        GameCanvas.panel.ITEM_HEIGHT = 24;
        GameCanvas.panel.selected = (GameCanvas.isTouch ? (-1) : 0);
        GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
        if (GameCanvas.panel.cmyLim < 0) GameCanvas.panel.cmyLim = 0;
        GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex]);
        if (GameCanvas.panel.cmy < 0) GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = 0);
        if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim) GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim);
    }

    public static void doFireModMenu()
    {
        if (GameCanvas.panel.currentTabIndex == 0) doFireModMenuBools();
        else doFireModMenuConfigure();
    }

    static void doFireModMenuBools()
    {
        if (GameCanvas.panel.selected < 0) return;
        ModMenu.modMenuItemBools[GameCanvas.panel.selected].Value = !ModMenu.modMenuItemBools[GameCanvas.panel.selected].Value;
    }

    static void doFireModMenuConfigure()
    {
        if (GameCanvas.panel.selected < 0) return;
        ModMenu.modMenuItemConfigs[GameCanvas.panel.selected].SwitchSelection();
    }

    public static void paintModMenu(mGraphics g)
    {
        if (GameCanvas.panel.currentTabIndex == 0) paintModMenuBools(g);
        else paintModMenuConfigure(g);
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
                mFont.tahoma_7_green2.drawString(g, (i + 1) + ". " + modMenuItem.Title, num + 5, num2, 0);
                string description = modMenuItem.Description.Length > 26 ? (modMenuItem.Description.Substring(0, 25) + "...") : modMenuItem.Description;
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

    static void paintModMenuConfigure(mGraphics g)
    {
        g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
        g.translate(0, -GameCanvas.panel.cmy);
        g.setColor(0);
        if (ModMenu.modMenuItemConfigs == null || ModMenu.modMenuItemConfigs.Length != GameCanvas.panel.currentListLength) return;
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
            ModMenuItemConfig modMenuItem = ModMenu.modMenuItemConfigs[i];
            string str = (mResources.status + ": ") == "Trạng thái: " ? "Đang " : mResources.status + ": ";
        if (modMenuItem != null)
            {
                mFont.tahoma_7_green2.drawString(g, (i + 1) + ". " + modMenuItem.Title, num + 5, num2, 0);
                string desc = str + modMenuItem.getSelectedValue();
                string description = desc.Length > 26 ? (desc.Substring(0, 25) + "...") : desc;
                if (i == GameCanvas.panel.selected && desc.Length > 26 && !GameCanvas.panel.isClose)
                {
                    isReset = false;
                    descriptionTextInfo = desc;
                    x = num + 5;
                    y = num2 + 11;
                }
                else mFont.tahoma_7_green.drawString(g, description, num + 5, num2 + 11, 0);
            }
        }
        if (isReset) TextInfo.reset();
        else
        {
            TextInfo.paint(g, descriptionTextInfo, x, y, 118, 15, mFont.tahoma_7_green);
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
        }
        GameCanvas.panel.paintScrollArrow(g);
    }
}
