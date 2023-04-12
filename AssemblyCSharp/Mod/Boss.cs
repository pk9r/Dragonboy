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

        static int distanceBetweenLines = 8;

        static int offset = 0;

        static int x = 15;

        static int y = 50;

        static int maxLength = 0;

        static int lastBoss = -1;

        static readonly int MAX_BOSS_DISPLAY = 5;

        static readonly int MAX_BOSS = 100;

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

        public static void Paint(mGraphics g)
        {
            if (!isEnabled)
                return;
            PaintListBosses(g);
            PaintScroll(g);
        }

        private static void PaintListBosses(mGraphics g)
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
                };
                Boss boss = listBosses[i];
                styles[i - start + offset].normal.textColor = Color.yellow;
                if (TileMap.mapID == boss.mapId)
                {
                    styles[i - start + offset].normal.textColor = new Color(1f, .5f, 0);
                    for (int j = 0; j < GameScr.vCharInMap.size(); j++)
                        if (((Char)GameScr.vCharInMap.elementAt(j)).cName == boss.name)
                        {
                            styles[i - start + offset].normal.textColor = Color.red;
                            break;
                        }
                }
                int length = Utilities.getWidth(styles[i - start + offset], $"{i + 1}. {boss}");
                maxLength = Math.max(length, maxLength);
            }
            int xDraw = GameCanvas.w - x - maxLength;
            for (int i = start - offset; i < listBosses.Count - offset; i++)
            {
                int yDraw = y + distanceBetweenLines * (i - start + offset);
                Boss boss = listBosses[i];
                g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.4f));
                if (GameCanvas.isMouseFocus(xDraw, yDraw, maxLength, 7))
                    g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.7f));
                g.fillRect(xDraw, yDraw + 1, maxLength, 7);
                g.drawString($"{i + 1}. {boss}", -x, mGraphics.zoomLevel - 3 + yDraw, styles[i - start + offset]);
            }
        }

        static void PaintScroll(mGraphics g)
        {
            if (listBosses.Count > MAX_BOSS_DISPLAY)
            {
                int heightScrollBar = MAX_BOSS_DISPLAY * distanceBetweenLines - 1 - 6 * 2;
                getButtonUp(out int buttonUpX, out int buttonUpY);
                getButtonDown(out int buttonDownX, out int buttonDownY);
                g.setColor(new Color(0, 0, 0, .25f));
                g.fillRect(buttonUpX, buttonUpY, 9, heightScrollBar + 6 * 2);
                g.drawRegion(Mob.imgHP, 0, (offset < listBosses.Count - MAX_BOSS_DISPLAY ? 24 : 54), 9, 6, 1, buttonUpX, buttonUpY, 0);
                g.drawRegion(Mob.imgHP, 0, (offset > 0 ? 24 : 54), 9, 6, 0, buttonDownX, buttonDownY, 0);
                //draw thumb
                int heightScrollBarThumb = Mathf.CeilToInt((float)MAX_BOSS_DISPLAY / listBosses.Count * heightScrollBar);
                g.setColor(new Color(0, 0, 0, .4f));
                g.fillRect(buttonUpX, buttonUpY + 6 + Mathf.CeilToInt((float)heightScrollBar / listBosses.Count * (listBosses.Count - offset - MAX_BOSS_DISPLAY)), 9, heightScrollBarThumb);
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
                if (GameCanvas.isPointerHoldIn(buttonUpX, buttonUpY, 9, 6))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        if (offset < listBosses.Count - MAX_BOSS_DISPLAY)
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
                        if (offset > 0)
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
            if (isEnabled && GameCanvas.isMouseFocus(GameCanvas.w - x - maxLength, y + 1, maxLength, 8 * MAX_BOSS_DISPLAY))
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
            //if (bosses.Count > 5)
            //{
            //    if (offset < bosses.Count - 5)
            //        g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 1, GameCanvas.w - x - 9, y - 7, 0);
            //    if (offset > 0)
            //        g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, GameCanvas.w - x - 9, y + 2 + distanceBetweenLines * 5, 0);
            //}
            buttonUpX = GameCanvas.w - x + 2;
            buttonUpY = y + 1;
        }

        static void getButtonDown(out int buttonDownX, out int buttonDownY)
        {
            buttonDownX = GameCanvas.w - x + 2;
            buttonDownY = y + 2 + distanceBetweenLines * (MAX_BOSS_DISPLAY - 1);
        }

        public static void setState(bool value) => isEnabled = value;
    }
}
