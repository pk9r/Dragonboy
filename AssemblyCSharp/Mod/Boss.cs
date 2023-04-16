using Mod.Graphics;
using Mod.ModHelper.CommandMod.Chat;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod
{
    public class Boss
    {
        public string name;

        public string map;

        public int mapId;

        public int zoneId = -1;

        public DateTime AppearTime;

        public static List<Boss> listBosses = new List<Boss>();

        public static bool isEnabled;

        public static int distanceBetweenLines = 8;

        static int offset = 0;

        public static int x = 15;

        public static int y = 50;

        static int maxLength = 0;

        static int lastBoss = -1;

        public static bool isCollapsed;

        public static readonly int MAX_BOSS_DISPLAY = 5;

        static readonly int MAX_BOSS = 100;

        static readonly string LIST_BOSS = "Danh sách Boss";

        static GUIStyle collapsedStyle;

        static int listBossWidth = 0;

        Boss(string name, string map) 
        {
            this.name = name;
            this.map = map;
            if (map == "Trạm tàu vũ trụ") 
            {
                if (name.StartsWith("Số ") || name.StartsWith("Tiểu đội"))
                    mapId = 25;
                else if (name.Contains("Bojack") || name.StartsWith("Bujin") || name.StartsWith("Bido") || name.StartsWith("Zangya") || name.StartsWith("Bido"))
                    mapId = 24;
            }
            else mapId = GetMapID(map);
            AppearTime = DateTime.Now;
        }

        public static void AddBoss(string chatVip)
        {
            if (!chatVip.StartsWith("BOSS"))
                return;
            chatVip = chatVip.Replace("BOSS ", "").Replace(" vừa xuất hiện tại ", "|").Replace(" appear at ", "|").Replace(" khu vực ", "|").Replace(" zone ", "|");
            string[] array = chatVip.Split('|');
            listBosses.Add(new Boss(array[0].Trim(), array[1].Trim()));
            if (array.Length == 3)
                listBosses.Last().zoneId = int.Parse(array[2].Trim());
            if (listBosses.Count > MAX_BOSS)
                listBosses.RemoveAt(0);
        }

        public override string ToString()
        {
            TimeSpan timeSpan = DateTime.Now.Subtract(AppearTime);
            string result = $"{name} - {map} [{mapId}] - ";
            if (zoneId > -1)
                result += $"khu {zoneId} - "; 
            int hours = (int)System.Math.Floor((decimal)timeSpan.TotalHours);
            if (hours > 0)
                result += $"{hours}h";
            if (timeSpan.Minutes > 0)
                result += $"{timeSpan.Minutes}m";
            result += $"{timeSpan.Seconds}s";
            return result;
        }

        public string ToString(bool enableRichText)
        {
            if (!enableRichText)
                return ToString();
            TimeSpan timeSpan = DateTime.Now.Subtract(AppearTime);
            string colorName = "yellow";
            string colorMap = "yellow";
            if (TileMap.mapID == mapId)
            {
                colorName = "orange";
                colorMap = "red";
            }
            if (CharExtensions.findCharInMap(name) != null)
                colorName = "red";
            string result = $"<color={colorName}>{name}</color> - <color={colorMap}>{map}</color> [<color={colorMap}>{mapId}</color>] - ";
            if (zoneId > -1)
            {
                if (TileMap.mapID == mapId && TileMap.zoneID == zoneId)
                    result += $"<color=yellow>khu</color> <color=red>{zoneId}</color> - ";
                else
                    result += $"khu {zoneId} - ";
            }
            int hours = (int)System.Math.Floor((decimal)timeSpan.TotalHours);
            if (hours > 0)
                result += $"<color=orange>{hours}</color>h";
            if (timeSpan.Minutes > 0)
                result += $"<color=orange>{timeSpan.Minutes}</color>m";
            result += $"<color=orange>{timeSpan.Seconds}</color>s";
            return result;
        }

        public static void Paint(mGraphics g)
        {
            if (!isEnabled)
                return;
            if (listBosses.Count <= 0)
                return;
            getCollapseButton(out int collapseButtonX, out int collapseButtonY);
            g.drawRegion(Mob.imgHP, 0, 24, 9, 6, (isCollapsed ? 5 : 4), collapseButtonX, collapseButtonY, 0);
            if (isCollapsed)
            {
                if (collapsedStyle == null)
                {
                    collapsedStyle = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.UpperRight,
                        fontSize = 6 * mGraphics.zoomLevel,
                        fontStyle = FontStyle.Bold,
                    };
                    collapsedStyle.normal.textColor = Color.white;
                    listBossWidth = Utilities.getWidth(collapsedStyle, LIST_BOSS);
                }
                if (GameCanvas.isMouseFocus(GameCanvas.w - x - listBossWidth, y, listBossWidth, 7))
                    CustomGraphics.fillRect(GameCanvas.w - x - listBossWidth, y + 7, (listBossWidth - 1) * mGraphics.zoomLevel, 1, collapsedStyle.normal.textColor);    
                g.setColor(new Color(.2f, .2f, .2f, .7f));
                g.fillRect(GameCanvas.w - x - listBossWidth, y, listBossWidth, 7);
                g.drawString(LIST_BOSS, GameCanvas.w - x - listBossWidth - 1, y + 1, collapsedStyle, listBossWidth * mGraphics.zoomLevel);
                return;
            }
            maxLength = 0;
            PaintListBosses(g);
            PaintScroll(g);
        }

        static void PaintListBosses(mGraphics g)
        {
            int start = 0;
            if (listBosses.Count > MAX_BOSS_DISPLAY)
                start = listBosses.Count - MAX_BOSS_DISPLAY;
            GUIStyle[] styles = new GUIStyle[MAX_BOSS_DISPLAY];
            for (int i = start - offset; i < listBosses.Count - offset; i++)
            {
                styles[i - start + offset] = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.UpperRight,
                    fontSize = 6 * mGraphics.zoomLevel,
                    fontStyle = FontStyle.Bold,
                    richText = true,
                };
                Boss boss = listBosses[i];
                int length = Utilities.getWidth(styles[i - start + offset], $"{i + 1}. {boss}");
                maxLength = Math.max(length, maxLength);
            }
            int xDraw = GameCanvas.w - x - maxLength;
            for (int i = start - offset; i < listBosses.Count - offset; i++)
            {
                int yDraw = y + distanceBetweenLines * (i - start + offset);
                Boss boss = listBosses[i];
                g.setColor(new Color(.2f, .2f, .2f, .4f));
                if (GameCanvas.isMouseFocus(xDraw, yDraw, maxLength, 7))
                    g.setColor(new Color(.2f, .2f, .2f, .7f));
                g.fillRect(xDraw, yDraw + 1, maxLength, 7);
                if (GameCanvas.isMouseFocus(xDraw, yDraw, maxLength, 7))
                    CustomGraphics.fillRect(xDraw + 1, yDraw + 7, (maxLength - 2) * mGraphics.zoomLevel + 2, 1, Color.white);
                g.drawString($"{i + 1}. {boss.ToString(true)}", -x, mGraphics.zoomLevel - 3 + yDraw, styles[i - start + offset]);
            }
        }

        static void PaintScroll(mGraphics g)
        {
            if (listBosses.Count > MAX_BOSS_DISPLAY)
            {
                getButtonUp(out int buttonUpX, out int buttonUpY);
                getButtonDown(out int buttonDownX, out int buttonDownY);
                getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight);
                g.setColor(new Color(.2f, .2f, .2f, .4f));
                g.fillRect(buttonUpX, buttonUpY, 9, scrollBarHeight + 6 * 2);
                g.drawRegion(Mob.imgHP, 0, (offset < listBosses.Count - MAX_BOSS_DISPLAY ? 24 : 54), 9, 6, 1, buttonUpX, buttonUpY, 0);
                g.drawRegion(Mob.imgHP, 0, (offset > 0 ? 24 : 54), 9, 6, 0, buttonDownX, buttonDownY, 0);
                //draw thumb
                g.setColor(new Color(.2f, .2f, .2f, .7f));
                g.fillRect(buttonUpX, buttonUpY + 6 + Mathf.CeilToInt((float)scrollBarHeight / listBosses.Count * (listBosses.Count - offset - MAX_BOSS_DISPLAY)), scrollBarWidth, scrollBarThumbHeight);
                g.setColor(new Color(.7f, .7f, 0f, 1f));
                g.drawRect(buttonUpX, buttonUpY + 6 + Mathf.CeilToInt((float)scrollBarHeight / listBosses.Count * (listBosses.Count - offset - MAX_BOSS_DISPLAY)), scrollBarWidth - 1, scrollBarThumbHeight - 1);
            }
        }

        static int GetMapID(string mapName)
        {
            for (int i = 0; i < TileMap.mapNames.Length; i++)
            {
                if (TileMap.mapNames[i].Equals(mapName))
                    return i;
            }
            return -1;
        }

        public static void UpdateTouch()
        {
            if (lastBoss != -1 && mSystem.currentTimeMillis() - Utilities.GetLastTimePress() > 200)
                lastBoss = -1;
            if (!isEnabled)
                return;
            if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
                return;
            getCollapseButton(out int collapseButtonX, out int collapseButtonY);
            if (GameCanvas.isPointerHoldIn(collapseButtonX, collapseButtonY, 6, 9) || GameCanvas.isPointerHoldIn(GameCanvas.w - x - listBossWidth, y, listBossWidth, 7))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                    isCollapsed = !isCollapsed;
                GameCanvas.clearAllPointerEvent();
                return;
            }
            if (isCollapsed)
                return;
            int start = 0;
            if (listBosses.Count > MAX_BOSS_DISPLAY)
                start = listBosses.Count - MAX_BOSS_DISPLAY;
            for (int i = start - offset; i < listBosses.Count - offset; i++)
            {
                if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - maxLength, y + 1 + distanceBetweenLines * (i - start + offset), maxLength, 7))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        if (lastBoss == i && mSystem.currentTimeMillis() - Utilities.GetLastTimePress() <= 200)
                        {
                            if (TileMap.mapID != listBosses[i].mapId)
                            {
                                if (XmapController.gI.IsActing)
                                    XmapController.finishXmap();
                                XmapController.start(listBosses[i].mapId);
                                lastBoss = -1;
                                return;
                            }
                            if (listBosses[i].zoneId != -1 && TileMap.zoneID != listBosses[i].zoneId)
                            {
                                Service.gI().requestChangeZone(listBosses[i].zoneId, 0);
                                return;
                            }
                        }
                        else
                            lastBoss = i;
                        if (TileMap.mapID == listBosses[i].mapId)
                        {
                            int j = 0;
                            for (; j < GameScr.vCharInMap.size(); j++)
                            {
                                Char ch = GameScr.vCharInMap.elementAt(j) as Char;
                                if (ch.cName == listBosses[i].name)
                                {
                                    Char.myCharz().deFocusNPC();
                                    Char.myCharz().itemFocus = null;
                                    Char.myCharz().mobFocus = null;
                                    if (Char.myCharz().charFocus != ch)
                                        Char.myCharz().charFocus = ch;
                                    else
                                        Utilities.teleportMyChar(ch);
                                    break;
                                }
                            }
                            if (j == GameScr.vCharInMap.size())
                                GameScr.info1.addInfo("Boss không có trong khu!", 0);
                        }
                    }
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
            }
            if (listBosses.Count > MAX_BOSS_DISPLAY)
            {
                getButtonUp(out int buttonUpX, out int buttonUpY);
                getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight);
                if (GameCanvas.isPointerMove && GameCanvas.isPointerDown && GameCanvas.isPointerHoldIn(buttonUpX, buttonUpY, scrollBarWidth, scrollBarHeight))
                {
                    float increment = scrollBarHeight / (float)listBosses.Count;
                    float newOffset = (GameCanvas.pyMouse - buttonUpY) / increment;
                    if (float.IsNaN(newOffset))
                        return;
                    offset = Mathf.Clamp(listBosses.Count - Mathf.RoundToInt(newOffset), 0, listBosses.Count - MAX_BOSS_DISPLAY);
                    return;
                }
                if (GameCanvas.isPointerHoldIn(buttonUpX, buttonUpY, 9, 6))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        if (offset + MAX_BOSS_DISPLAY <= listBosses.Count - MAX_BOSS_DISPLAY)
                            offset += MAX_BOSS_DISPLAY;
                        else if (offset < listBosses.Count - MAX_BOSS_DISPLAY)
                            offset++;
                    }
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
                getButtonDown(out int buttonDownX, out int buttonDownY);
                if (GameCanvas.isPointerHoldIn(buttonDownX, buttonDownY, 9, 6))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        if (offset - MAX_BOSS_DISPLAY >= 0)
                            offset -= MAX_BOSS_DISPLAY;
                        else if (offset > 0)
                            offset--;
                    }
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
            }
        }

        public static void Update()
        {
            foreach (Boss boss in listBosses)
            {
                if (boss.zoneId != -1)
                    continue;
                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    Char ch = GameScr.vCharInMap.elementAt(i) as Char;
                    if (ch.cName == boss.name)
                    {
                        boss.zoneId = TileMap.zoneID;
                        break;
                    }
                }
                if (boss.zoneId == TileMap.zoneID)
                    break;
            }
            if (isEnabled && !isCollapsed && GameCanvas.isMouseFocus(GameCanvas.w - x - maxLength, y + 1, maxLength, 8 * MAX_BOSS_DISPLAY))
            {
                    if (GameCanvas.pXYScrollMouse > 0)
                        if (offset < listBosses.Count - MAX_BOSS_DISPLAY)
                            offset++;
                    if (GameCanvas.pXYScrollMouse < 0)
                        if (offset > 0)
                            offset--;
                }
        }

        static void getButtonUp(out int buttonUpX, out int buttonUpY)
        {
            buttonUpX = GameCanvas.w - x + 2;
            buttonUpY = y + 1;
        }

        static void getButtonDown(out int buttonDownX, out int buttonDownY)
        {
            buttonDownX = GameCanvas.w - x + 2;
            buttonDownY = y + 2 + distanceBetweenLines * (MAX_BOSS_DISPLAY - 1);
        }

        static void getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight)
        {
            scrollBarWidth = 9;
            scrollBarHeight = MAX_BOSS_DISPLAY * distanceBetweenLines - 1 - 6 * 2;
            scrollBarThumbHeight = Mathf.CeilToInt((float)MAX_BOSS_DISPLAY / listBosses.Count * scrollBarHeight);
        }

        static void getCollapseButton(out int collapseButtonX, out int collapseButtonY)
        {
            if (isCollapsed)
            {
                collapseButtonX = GameCanvas.w - x - listBossWidth - 8;
                collapseButtonY = y;
                return;
            }
            collapseButtonX = GameCanvas.w - x - maxLength - 8;
            collapseButtonY = y + 2 + (distanceBetweenLines * (Math.min(listBosses.Count, MAX_BOSS_DISPLAY) - 1)) / 2;
        }

        public static void setState(bool value) => isEnabled = value;

        //[ChatCommand("testboss")]
        //public static void Test()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        GameEvents.onChatVip("BOSS Vũ Đăng vừa xuất hiện tại Đảo Kamê khu vực 10");
        //    }
        //}
    }
}
