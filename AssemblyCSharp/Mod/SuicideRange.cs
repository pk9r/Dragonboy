using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod
{
    public class SuicideRange
    {
        public static List<IMapObject> mapObjsInRange { get; private set; } = new List<IMapObject>();

        public static List<IMapObject> mapObjsInMyRange { get; private set; } = new List<IMapObject>();

        public static bool isShowSuicideRange;

        public static void update()
        {
            if (!isShowSuicideRange) return;
            mapObjsInRange.Clear();
            mapObjsInMyRange.Clear();
            if (!Char.myCharz().isDie && Char.myCharz().myskill == Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]) && Char.myCharz().cgender == 2)
            {
                FindMapObjInRange(Char.myCharz());
            }
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char c = GameScr.vCharInMap.elementAt(i) as Char;
                if (c.isStandAndCharge)
                {
                    FindMapObjInRange(c);
                    if (Utilities.getDistance(Char.myCharz(), c) <= CharExtensions.getSuicideRange(c) && !mapObjsInRange.Contains(Char.myCharz())) mapObjsInRange.Add(Char.myCharz());
                }
            }
        }

        public static void paint(mGraphics g)
        {
            if (!isShowSuicideRange) return;
            if (!Char.myCharz().isDie && Char.myCharz().cgender == 2 && Char.myCharz().myskill == Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]))
            {
                g.setColor(Color.yellow);
                if ((Char.myCharz().isStandAndCharge) && GameCanvas.gameTick % 10 >= 5) g.setColor(Color.red);
                CustomGraphics.DrawCircle(g, Char.myCharz(), CharExtensions.getSuicideRange(Char.myCharz()), 1);
            }
            g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
            g.translate(-GameScr.cmx, -GameScr.cmy);
            foreach (IMapObject mapObject in mapObjsInMyRange)
            {
                if (mapObjsInRange.Contains(mapObject)) continue;
                g.setColor(Color.yellow);
                if ((Char.myCharz().isStandAndCharge) && GameCanvas.gameTick % 10 >= 5) g.setColor(Color.red);
                paintMapObjInRange(g, mapObject);
            }
            foreach (IMapObject mapObject in mapObjsInRange)
            {
                g.setColor(Color.yellow);
                if (GameCanvas.gameTick % 10 >= 5) g.setColor(Color.red);
                paintMapObjInRange(g, mapObject);
            }
            GameScr.resetTranslate(g);
            int y = GameCanvas.h - 95;
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char c = GameScr.vCharInMap.elementAt(i) as Char;
                if (c.isStandAndCharge && Utilities.getDistance(c, Char.myCharz()) <= CharExtensions.getSuicideRange(c) && ((c.cFlag != 0 && Char.myCharz().cFlag != 0 && (c.cFlag != Char.myCharz().cFlag || (c.cFlag == 8 && Char.myCharz().cFlag == 8))) || Char.myCharz().cTypePk == 5 || c.cTypePk == 5))
                {
                    g.setColor(Color.yellow);
                    if (GameCanvas.gameTick % 10 >= 5) g.setColor(Color.red);
                    CustomGraphics.drawLine(g, Char.myCharz().cx, Char.myCharz().cy, c.cx, c.cy, 2);
                    string str = $"Đang trong tầm bom của {CharExtensions.getNameWithoutClanTag(c)} [{NinjaUtil.getMoneys(c.cHPFull)}]!";
                    g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.6f));
                    g.fillRect(12, y + 2, mFont.tahoma_7_red.getWidth(str) + 5, 9);
                    mFont.tahoma_7_red.drawString(g, str, 15, y, mFont.LEFT);
                    y -= 10;
                }
            }
        }

        static void paintMapObjInRange(mGraphics g, IMapObject mapObject)
        {
            if (mapObject is Char)
            {
                Char c = mapObject as Char;
                int height = 35;
                int width = 12;
                if (CharExtensions.isPet(c)) height = 30;
                if (c.cTypePk == 5)
                {
                    width = 15;
                    height = 40;
                }
                CustomGraphics.drawRect(g, c.cx - width, c.cy - height, width * 2, height, 2);
            }
            if (mapObject is Mob)
            {
                Mob mob = mapObject as Mob;
                CustomGraphics.drawRect(g, Mathf.RoundToInt(mob.x - mob.w / 2), mob.y - mob.h, mob.w, mob.h, 2);
            }
        }

        static void FindMapObjInRange(Char suicidingChar)
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char c = GameScr.vCharInMap.elementAt(i) as Char;
                if (Utilities.getDistance(c, suicidingChar) <= CharExtensions.getSuicideRange(suicidingChar) && ((suicidingChar.cFlag != 0 && c.cFlag != 0 && (c.cFlag != suicidingChar.cFlag || (c.cFlag == 8 && suicidingChar.cFlag == 8))) || suicidingChar.cTypePk == 5 || c.cTypePk == 5))
                {
                    if (suicidingChar.me)
                    {
                        if (!mapObjsInMyRange.Contains(c)) mapObjsInMyRange.Add(c);
                    }
                    else if (!mapObjsInRange.Contains(c))
                    {
                        mapObjsInRange.Add(c);
                    }
                }
            }
            for (int j = 0; j < GameScr.vMob.size(); j++)
            {
                Mob mob = GameScr.vMob.elementAt(j) as Mob;
                if (Utilities.getDistance(mob, suicidingChar) <= CharExtensions.getSuicideRange(suicidingChar) && !mapObjsInRange.Contains(mob) && !mob.isMobMe)
                {
                    if (suicidingChar.me)
                    {
                        if (!mapObjsInMyRange.Contains(mob)) mapObjsInMyRange.Add(mob);
                    }
                    else if (!mapObjsInRange.Contains(mob))
                    {
                        mapObjsInRange.Add(mob);
                    }
                }
            }
        }
    }
}
