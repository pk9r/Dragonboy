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

        static List<Boss> bosses = new List<Boss>();

        public static bool isEnabled;

        static int distanceBetweenLines = 8;

        static int offset = 0;

        static int x = -1;

        static int y = 50;

        static int maxLength = 0;

        static int lastBoss = -1;

        Boss(string name, string map) 
        {
            this.name = name;
            this.map = map;
            mapId = GetMapID(map);
            AppearTime = DateTime.Now;
        }

        public static void AddBoss(string chatVip)
        {
            if (!chatVip.StartsWith("BOSS"))
                return;
            chatVip = chatVip.Replace("BOSS ", "").Replace(" vừa xuất hiện tại ", "|").Replace(" appear at ", "|").Replace(" khu vực ", "|").Replace(" zone ", "|");
            string[] array = chatVip.Split('|');
            bosses.Add(new Boss(array[0].Trim(), array[1].Trim()));
            if (array.Length == 3)
                bosses.Last().zoneId = int.Parse(array[2].Trim());
            if (bosses.Count > 50)
                bosses.RemoveAt(0);
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
            if (x == -1)
                x = (int)(-7.5f * mGraphics.zoomLevel);
            int start = 0;
            if (bosses.Count > 5)
                start = bosses.Count - 5;
            GUIStyle[] styles = new GUIStyle[5];
            for (int i = start - offset; i < bosses.Count - offset; i++)
            {
                styles[i - start + offset] = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.UpperRight,
                    fontSize = 13,
                    fontStyle = FontStyle.Bold
                };
                Boss boss = bosses[i];
                styles[i - start + offset].normal.textColor = Color.yellow;
                if (TileMap.mapID == boss.mapId)
                {
                    styles[i - start + offset].normal.textColor = Color.red;
                    for (int j = 0; j < GameScr.vCharInMap.size(); j++)
                        if (((Char)GameScr.vCharInMap.elementAt(j)).cName.Equals(boss.name))
                        {
                            styles[i - start + offset].fontStyle = FontStyle.Bold;
                            break;
                        }
                }
                maxLength = Math.max(Utilities.getWidth(styles[i - start + offset], boss.ToString()), maxLength);
            }
            for (int i = start - offset; i < bosses.Count - offset; i++)
            {
                Boss boss = bosses[i];
                g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.4f));
                if (TileMap.mapID == boss.mapId)
                {
                    for (int j = 0; j < GameScr.vCharInMap.size(); j++)
                        if (((Char)GameScr.vCharInMap.elementAt(j)).cName.Equals(boss.name))
                        {
                            g.setColor(new Color(1f, 1f, 0f, 0.3f));
                            break;
                        }
                }
                g.fillRect(GameCanvas.w - Math.abs(x) - maxLength, y + 2 + distanceBetweenLines * (i - start + offset), maxLength, 7);
                g.drawString($"{i + 1}. {boss}", x, y + distanceBetweenLines * (i - start + offset), styles[i - start + offset]);
            }
            if (bosses.Count > 5)
            {
                if (offset < bosses.Count - 5)
                    g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 1, GameCanvas.w - Math.abs(x) - 9, y - 5, 0);
                if (offset > 0)
                    g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, GameCanvas.w - Math.abs(x) - 9, y + 2 + distanceBetweenLines * 5, 0);
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
            if (bosses.Count > 5)
                start = bosses.Count - 5;
            for (int i = start - offset; i < bosses.Count - offset; i++)
            {
                if (GameCanvas.isPointerHoldIn(GameCanvas.w - Math.abs(x) - maxLength, y + 2 + distanceBetweenLines * (i - start + offset), maxLength, 7))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        if (lastBoss == i && mSystem.currentTimeMillis() - Utilities.GetLastTimePress() <= 200)
                        {
                            if (TileMap.mapID != bosses[i].mapId)
                            {
                                if (XmapController.gI.IsActing)
                                    XmapController.finishXmap();
                                XmapController.start(bosses[i].mapId);
                                lastBoss = -1;
                            }
                            else if (bosses[i].zoneId != -1)
                            {
                                if (TileMap.zoneID != bosses[i].zoneId)
                                    Service.gI().requestChangeZone(bosses[i].zoneId, 0);
                                else
                                    GameScr.info1.addInfo("Đang trong khu rồi!", 0);
                            }
                            else
                                GameScr.info1.addInfo("Đang trong map rồi!", 0);
                        }
                        else 
                            lastBoss = i;
                    }
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
            }
            if (bosses.Count > 5)
            {
                if (GameCanvas.isPointerHoldIn(GameCanvas.w - Math.abs(x) - 9, y - 5, 9, 6))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        if (offset < bosses.Count - 5)
                            offset++;
                    }
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
                if (GameCanvas.isPointerHoldIn(GameCanvas.w - Math.abs(x) - 9, y + 2 + distanceBetweenLines * 5, 9, 6))
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

        public static void setState(bool value) => isEnabled = value;
    }
}
