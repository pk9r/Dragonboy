using Mod.CustomGroupBox;
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

        static int maxLength = 0;

        static int x = 15;

        static int y = 50;

        static readonly int MAX_CHAR = 6;

        static int distanceBetweenLines = 8;

        static int offset = 0;

        //static GroupBox backgroundGroupBox;

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
            //if (backgroundGroupBox == null)
            //    InitializeGroupBox();
            //UpdateGroupBox();
            //backgroundGroupBox.Paint(g);
            //g.reset();
            if (offset >= listChars.Count - MAX_CHAR)
            {
                if (listChars.Count - MAX_CHAR > 0)
                    offset = listChars.Count - MAX_CHAR;
                else
                    offset = 0;
            }
            maxLength = 0;
            PaintListChars(g);
            PaintScroll(g);
        }

        static void PaintListChars(mGraphics g)
        {
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
                maxLength = Math.max(maxLength, Utilities.getWidth(charDescriptions[i - start + offset].Value, charDesc) + (ch.cFlag != 0 ? (distanceBetweenLines + 1) : 0));
                #endregion
            }
            for (int i = start - offset; i < listChars.Count - offset; i++)
            {
                int offsetPaint = 0;
                Char ch = listChars[i];
                if (ch.cFlag != 0)
                {
                    offsetPaint = distanceBetweenLines + 1;
                    if (ch.cFlag == 9 || ch.cFlag == 10)
                    {
                        GUIStyle flagStyle = new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.UpperCenter,
                            fontSize = 6 * mGraphics.zoomLevel
                        };
                        flagStyle.normal.textColor = Color.white;
                        if (ch.cFlag == 9)
                            g.drawString("K", -x, mGraphics.zoomLevel - 3 + y + distanceBetweenLines * (i - start + offset), flagStyle);
                        if (ch.cFlag == 10)
                            g.drawString("M", -x, mGraphics.zoomLevel - 3 + y + distanceBetweenLines * (i - start + offset), flagStyle);
                    }
                    g.setColor(CharExtensions.getFlagColor(ch));
                    g.fillRect(GameCanvas.w - x - distanceBetweenLines + 1, y + 1 + distanceBetweenLines * (i - start + offset), distanceBetweenLines - 1, distanceBetweenLines - 1);
                }
                g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.4f));
                if (GameCanvas.isMouseFocus(GameCanvas.w - x - maxLength - offsetPaint, y + 1 + distanceBetweenLines * (i - start + offset), maxLength, distanceBetweenLines - 1))
                    g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.7f));
                if (Char.myCharz().charFocus == listChars[i])
                    g.setColor(new Color(1f, .5f, 0f, .5f));
                if (SuicideRange.isShowSuicideRange && SuicideRange.mapObjsInMyRange.Contains(listChars[i]))
                {
                    g.setColor(new Color(0.5f, 0.5f, 0f, 1f));
                    if (Char.myCharz().isStandAndCharge && GameCanvas.gameTick % 10 >= 5)
                        g.setColor(new Color(1f, 0f, 0f, 1f));
                }
                g.fillRect(GameCanvas.w - x - maxLength, y + 1 + distanceBetweenLines * (i - start + offset), maxLength - offsetPaint, distanceBetweenLines - 1);
                g.drawString(charDescriptions[i - start + offset].Key, -x - offsetPaint, mGraphics.zoomLevel - 3 + y + distanceBetweenLines * (i - start + offset), charDescriptions[i - start + offset].Value);
            }
        }

        static void PaintScroll(mGraphics g)
        {
            if (listChars.Count > MAX_CHAR)
            {
                int heightScrollBar = MAX_CHAR * distanceBetweenLines - 1;
                getButtonUp(out int buttonUpX, out int buttonUpY);
                getButtonDown(out int buttonDownX, out int buttonDownY);
                g.setColor(new Color(0, 0, 0, .25f));
                g.fillRect(buttonUpX, buttonUpY, 9, heightScrollBar);
                heightScrollBar -= 6 * 2;
                int heightScrollBarThumb = Mathf.RoundToInt((float)MAX_CHAR / listChars.Count * heightScrollBar);
                g.setColor(new Color(0, 0, 0, .4f));
                g.fillRect(buttonUpX, buttonUpY + 5 + Mathf.RoundToInt((float)(heightScrollBar - heightScrollBarThumb) / (listChars.Count - 5) * (listChars.Count - offset - 5)), 9, heightScrollBarThumb);
                g.drawRegion(Mob.imgHP, 0, (offset < listChars.Count - MAX_CHAR ? 24 : 54), 9, 6, 1, buttonUpX, buttonUpY, 0);
                g.drawRegion(Mob.imgHP, 0, (offset > 0 ? 24 : 54), 9, 6, 0, buttonDownX, buttonDownY, 0);
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
                    if (GameCanvas.isPointerHoldIn(GameCanvas.w - x - maxLength - (listChars[i].cFlag != 0 ? (distanceBetweenLines + 1) : 0), y + 1 + distanceBetweenLines * (i - start + offset), maxLength, distanceBetweenLines - 1))
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
                    getButtonUp(out int buttonUpX, out int buttonUpY);
                    if (GameCanvas.isPointerHoldIn(buttonUpX, buttonUpY, 9, 6))
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
            catch (Exception) { }
        }

        //static void InitializeGroupBox()
        //{
        //    backgroundGroupBox = new GroupBox("Nhân vật trong map", GameCanvas.w - x - (maxLength == 0 ? 100 : maxLength), y - 10, 150, 10)
        //    {
        //        BackColor = new Color(0, 0, 0, .1f),
        //        HasBorder = true,
        //        BorderColor = Color.yellow,
        //        TitleAnchor = TextAnchor.UpperRight,
        //        TitleStyle = new GUIStyle(GUI.skin.label)
        //    };
        //    backgroundGroupBox.TitleStyle.fontSize = 6 * mGraphics.zoomLevel;
        //    backgroundGroupBox.TitleStyle.normal.textColor = Color.white;
        //}

        //static void UpdateGroupBox()
        //{
        //    backgroundGroupBox.X = GameCanvas.w - x - (maxLength == 0 ? 100 : maxLength) - 2;
        //    backgroundGroupBox.Y = y - 10;
        //    backgroundGroupBox.Width = (maxLength == 0 ? 100 : maxLength) + 13;

        //    if (listChars.Count > MAX_CHAR)
        //    {
        //        backgroundGroupBox.X -= 10;
        //        backgroundGroupBox.Width += 10;
        //    }

        //    backgroundGroupBox.Height = Math.min(MAX_CHAR, listChars.Count) * distanceBetweenLines + 12;
        //    if (backgroundGroupBox.Height < 20)
        //        backgroundGroupBox.Height = 20;
        //}

        static void getButtonUp(out int buttonUpX, out int buttonUpY)
        {
            buttonUpX = GameCanvas.w - x + 2;
            buttonUpY = y + 1;
        }
        
        static void getButtonDown(out int buttonDownX, out int buttonDownY)
        {
            buttonDownX = GameCanvas.w - x + 2;
            buttonDownY = y + 2 + distanceBetweenLines * (MAX_CHAR - 1);
        }

        public static void setState(bool value) => isEnabled = value;

        public static void setStatePet(bool value) => isShowPet = value;
    }
}