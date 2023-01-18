using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod
{
    public class ListCharsInMap
    {
        public static List<Char> listChars = new List<Char>();

        public static bool isEnabled;

        public static bool isShowPet;

        static string longestStr = string.Empty;

        static int maxLength = 0;

        static int x = 15;

        static int y = 50;

        static readonly int MAX_CHAR = 6;

        static int distanceBetweenLines = 8;

        static int offset = 0; 

        public static void update()
        {
            if (Boss.isEnabled)
            {
                if (y != 55 + 6 + 8 * Mathf.Clamp(Boss.bosses.Count, 0, 5) + 3)
                    y = 55 + 6 + 8 * Mathf.Clamp(Boss.bosses.Count, 0, 5) + 3;
            }
            else if (y != 50)
                y = 50;
            if (!isEnabled)
                return;
            listChars.Clear();
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char ch = (Char)GameScr.vCharInMap.elementAt(i);
                if (CharExtensions.isNormalChar(ch, true, false))
                {
                    listChars.Add(ch);
                    if (isShowPet && ch.charID > 0)
                    {
                        Char chPet = GameScr.findCharInMap(-ch.charID);
                        if (chPet != null)
                            listChars.Add(chPet);
                    }
                }
            }
            if (offset >= listChars.Count - MAX_CHAR)
            {
                if (listChars.Count - MAX_CHAR > 0)
                    offset = listChars.Count - MAX_CHAR;
                else
                    offset = 0;
            }
            if (GameCanvas.isMouseFocus(GameCanvas.w - x - maxLength, y + 1, maxLength, 8 * MAX_CHAR))
            {
                if (GameCanvas.pXYScrollMouse > 0)
                    if (offset < listChars.Count - MAX_CHAR)
                        offset++;
                if (GameCanvas.pXYScrollMouse < 0)
                    if (offset > 0)
                        offset--;
            }
        }

        public static void paint(mGraphics g)
        {
            if (!isEnabled)
                return;
            g.reset();
            if (offset >= listChars.Count - MAX_CHAR)
            {
                if (listChars.Count - MAX_CHAR > 0)
                    offset = listChars.Count - MAX_CHAR;
                else
                    offset = 0;
            }
            longestStr = string.Empty;
            distanceBetweenLines = 8;
            int skippedCharCount = 0;
            List<KeyValuePair<string, GUIStyle>> charDescriptions = new List<KeyValuePair<string, GUIStyle>>();
            int start = 0;
            if (listChars.Count > MAX_CHAR)
                start = listChars.Count - MAX_CHAR;
            for (int i = start - offset; i < listChars.Count - offset; i++)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 6 * mGraphics.zoomLevel,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.UpperRight
                };
                #region Format
                Char ch = listChars[i];
                if (ch.charEffectTime.hasNRD || CharExtensions.isBoss(ch))
                    style.normal.textColor = Color.red;
                if (ch.cHP <= 0)
                    style.normal.textColor = Color.black;
                string charDesc = ch.cName + " [" + NinjaUtil.getMoneys(ch.cHP) + "/" + NinjaUtil.getMoneys(ch.cHPFull) + "]";
                if (CharExtensions.isNormalChar(ch, false, false))
                    charDesc = ch.cName + " [" + NinjaUtil.getMoneys(ch.cHP) + "/" + NinjaUtil.getMoneys(ch.cHPFull) + "] - " + CharExtensions.getGender(ch) + " [" + ch.charID + "]";
                if (CharExtensions.isPet(ch))
                {
                    style.normal.textColor = Color.blue;
                    if (ch.cHP <= 0)
                        style.normal.textColor = Color.black;
                    charDesc = ch.cName.Replace("$", "").Replace("#", "") + " [" + NinjaUtil.getMoneys(ch.cHP) + "/" + NinjaUtil.getMoneys(ch.cHPFull) + " - " + CharExtensions.getGender(ch) + "]";
                    skippedCharCount++;
                }
                else if (!CharExtensions.isBoss(ch))
                    charDesc = i + 1 - skippedCharCount + ". " + charDesc;
                else
                    skippedCharCount++;
                if ((Char.myCharz().isStandAndCharge || (!Char.myCharz().isDie && Char.myCharz().cgender == 2 && Char.myCharz().myskill == Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]))) && SuicideRange.mapObjsInMyRange.Contains(ch)) 
                    charDesc += " - Trong tầm";
                charDescriptions.Add(new KeyValuePair<string, GUIStyle>(charDesc, /*mfont*/style));
                if (charDesc.Length > longestStr.Length) 
                    longestStr = charDesc;
                #endregion
            }
            for (int i = start - offset; i < listChars.Count - offset; i++)
            {
                maxLength = Utilities.getWidth(charDescriptions[i - start + offset].Value, longestStr);
                g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.4f));
                if (GameCanvas.isMouseFocus(GameCanvas.w - x - maxLength, y + 1 + distanceBetweenLines * (i - start + offset), maxLength, 7))
                    g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.7f));
                if (Char.myCharz().charFocus == listChars[i])
                    g.setColor(new Color(1f, .5f, 0f, .5f));
                if (SuicideRange.isShowSuicideRange && SuicideRange.mapObjsInMyRange.Contains(listChars[i]))
                {
                    g.setColor(new Color(0.5f, 0.5f, 0f, 1f));
                    if (Char.myCharz().isStandAndCharge && GameCanvas.gameTick % 10 >= 5)
                        g.setColor(new Color(1f, 0f, 0f, 1f));
                }
                g.fillRect(GameCanvas.w - x - maxLength, y + 1 + distanceBetweenLines * (i - start + offset), maxLength, 7);
                g.drawString(charDescriptions[i - start + offset].Key, -x, mGraphics.zoomLevel - 3 + y + distanceBetweenLines * (i - start + offset), charDescriptions[i - start + offset].Value);
                Char ch = listChars[i];
                g.setColor(CharExtensions.getFlagColor(ch));
                g.fillRect(GameCanvas.w - x + 2, y + 1 + distanceBetweenLines * (i - start + offset), 7, 7);
                if (ch.cFlag == 9 || ch.cFlag == 10)
                {
                    GUIStyle flagStyle = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.UpperCenter,
                        fontSize = 6 * mGraphics.zoomLevel
                    };
                    flagStyle.normal.textColor = Color.white;
                    if (ch.cFlag == 9)
                        g.drawString("K", -x + 5, mGraphics.zoomLevel - 3 + y + distanceBetweenLines * (i - start + offset), flagStyle);
                    if (ch.cFlag == 10)
                        g.drawString("M", -x + 5, mGraphics.zoomLevel - 3 + y + distanceBetweenLines * (i - start + offset), flagStyle);
                }
            }
            if (listChars.Count > MAX_CHAR)
            {
                if (offset < listChars.Count - MAX_CHAR)
                    g.drawRegion(Mob.imgHP, 0, 24, 9, 6, 1, GameCanvas.w - x - 9, y - 7, 0);
                if (offset > 0)
                    g.drawRegion(Mob.imgHP, 0, 24, 9, 6, 0, GameCanvas.w - x - 9, y + 2 + distanceBetweenLines * MAX_CHAR, 0);
            }
        }

        public static void updateTouch()
        {
            if (!isEnabled)
                return;
            try
            {
                if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
                    return;
                int start = 0;
                if (listChars.Count > MAX_CHAR)
                    start = listChars.Count - MAX_CHAR;
                for (int i = start - offset; i < listChars.Count - offset; i++)
                {
                    if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - maxLength, y + 1 + distanceBetweenLines * (i - start + offset), maxLength, 7))
                    {
                        GameCanvas.isPointerJustDown = false;
                        GameScr.gI().isPointerDowning = false;
                        if (GameCanvas.isPointerClick)
                        {
                            Char.myCharz().mobFocus = null;
                            Char.myCharz().npcFocus = null;
                            Char.myCharz().itemFocus = null;
                            if (Char.myCharz().charFocus != listChars[i])
                                Char.myCharz().charFocus = listChars[i];
                            else Utilities.teleportMyChar(listChars[i]);
                        }
                        Char.myCharz().currentMovePoint = null;
                        GameCanvas.clearAllPointerEvent();
                        return;
                    }
                }
                if (listChars.Count > MAX_CHAR)
                {
                    if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - 9, y - 7, 9, 6))
                    {
                        GameCanvas.isPointerJustDown = false;
                        GameScr.gI().isPointerDowning = false;
                        if (GameCanvas.isPointerClick)
                        {
                            if (offset < listChars.Count - MAX_CHAR)
                                offset++;
                        }
                        GameCanvas.clearAllPointerEvent();
                        return;
                    }
                    if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - 9, y + 2 + distanceBetweenLines * MAX_CHAR, 9, 6))
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
            catch (Exception) { }
        }

        public static void setState(bool value) => isEnabled = value;

        public static void setStatePet(bool value) => isShowPet = value;
    }
}