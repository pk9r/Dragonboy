using System;
using System.Collections.Generic;
using System.Linq;
using Mod.R;
using UnityEngine;

namespace Mod
{
    internal class ListCharsInMap
    {
        internal static List<Char> listChars = new List<Char>();
        internal static bool isEnabled;
        internal static bool isShowPet;

        internal static int x = 15 - 9;
        internal static int y = 0;

        static int maxLength = 0;
        static readonly int MAX_CHAR = 6;
        static int distanceBetweenLines = 8;
        static int offset = 0;
        static bool isCollapsed;
        static int titleWidth;
        static int offsetX;

        internal static void Update()
        {
            if (!isEnabled)
                return;
            listChars.Clear();
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char ch = (Char)GameScr.vCharInMap.elementAt(i);
                if (ch.isNormalChar(true))
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
            if (isShowPet)
                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    Char ch = (Char)GameScr.vCharInMap.elementAt(i);
                    if (ch.isNormalChar(false, true) && !listChars.Contains(ch))
                        listChars.Add(ch);
                }
            if (offset >= listChars.Count - MAX_CHAR)
            {
                if (listChars.Count - MAX_CHAR > 0)
                    offset = listChars.Count - MAX_CHAR;
                else
                    offset = 0;
            }
            if (GameCanvas.isMouseFocus(GameCanvas.w - (x + offsetX) - maxLength, y + 1, maxLength, 8 * MAX_CHAR))
            {
                if (GameCanvas.pXYScrollMouse > 0)
                    if (offset < listChars.Count - MAX_CHAR)
                        offset++;
                if (GameCanvas.pXYScrollMouse < 0)
                    if (offset > 0)
                        offset--;
            }
            getScrollBar(out int scrollBarWidth, out _, out _);
            if (listChars.Count > MAX_CHAR)
                offsetX = scrollBarWidth;
            else
                offsetX = 0;
        }

        internal static int Paint(int _y, mGraphics g)
        {
            if (!isEnabled) 
                return getSpaceOccupied();
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
            y = _y;
            maxLength = 0;

            if (!isCollapsed)
            {
                PaintListChars(g);
                PaintScroll(g);
            }
            PaintRect(g);
            return getSpaceOccupied();
        }

        static string formatHP(Char ch)
        {
            int hp = ch.cHP;
            int hpFull = ch.cHPFull;
            float ratio = hp / (float)hpFull;
            Color color = new Color(Mathf.Clamp(2 - ratio * 2, 0, 1), Mathf.Clamp(ratio * 2, 0, 1), 0);
            string hexColor = $"#{(int)(color.r * 255):x2}{(int)(color.g * 255):x2}{(int)(color.b * 255):x2}{(int)(color.a * 255):x2}";
            if (hp == 0)
                hexColor = "black";
            return $"<color=white>[<color={hexColor}>{NinjaUtil.getMoneys(ch.cHP)}</color>/<color=lime>{NinjaUtil.getMoneys(ch.cHPFull)}</color>]</color>";
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
                    alignment = TextAnchor.UpperRight,
                    richText = true
                };
                #region Format
                Char ch = listChars[i];
                string charDesc = $"<color=orange>{ch.getClanTag()}</color>{ch.getNameWithoutClanTag(true)} {formatHP(ch)}";
                if (ch.isNormalChar())
                    charDesc += $" - {ch.getGender(true)} [{ch.charID}]";
                if (ch.IsPet())
                {
                    charDesc += $" - {ch.getGender(true)}";
                    Char chMaster = GameScr.findCharInMap(-ch.charID);
                    if (chMaster != null)
                        charDesc += string.Format(Strings.someonePet, chMaster.getNameWithoutClanTag(true));
                    else
                        charDesc += Strings.petLostMaster;
                    skippedCharCount++;
                }
                else if (!ch.isBoss())
                    charDesc = i + 1 - skippedCharCount + ". " + charDesc;
                else
                    skippedCharCount++;
                //if ((Char.myCharz().isStandAndCharge || (!Char.myCharz().isDie && Char.myCharz().cgender == 2 && Char.myCharz().myskill == Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[4]))) && SuicideRange.mapObjsInMyRange.Contains(ch))
                //{
                //    if (GameCanvas.gameTick % 40 > 20)
                //        charDesc += " - <color=red>Trong tầm</color>";
                //    else
                //        charDesc += " - <color=yellow>Trong tầm</color>";
                //}
                if (ch.charEffectTime.hasBlackStarDragonBall || ch.isBoss())
                    charDesc = $"<color=red>{charDesc}</color>";
                else if (ch.IsPet())
                    charDesc = $"<color=cyan>{charDesc}</color>";
                if (ch.cHP <= 0)
                    charDesc = $"<color=black>{charDesc}</color>";

                charDescriptions.Add(new KeyValuePair<string, GUIStyle>(charDesc, /*mfont*/style));
                maxLength = Math.Max(maxLength, Utils.getWidth(charDescriptions[i - start + offset].Value, charDesc) + (ch.cFlag != 0 ? (distanceBetweenLines + 1) : 0));
                #endregion
            }
            FillBackground(g);
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
                            g.drawString("K", -(x + offsetX), mGraphics.zoomLevel - 3 + y + distanceBetweenLines * (i - start + offset), flagStyle);
                        if (ch.cFlag == 10)
                            g.drawString("M", -(x + offsetX), mGraphics.zoomLevel - 3 + y + distanceBetweenLines * (i - start + offset), flagStyle);
                    }
                    g.setColor(ch.getFlagColor());
                    g.fillRect(GameCanvas.w - (x + offsetX) - distanceBetweenLines + 1, y + 1 + distanceBetweenLines * (i - start + offset), distanceBetweenLines - 1, distanceBetweenLines - 1);
                }
                g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.4f));
                if (GameCanvas.isMouseFocus(GameCanvas.w - (x + offsetX) - maxLength - offsetPaint, y + 1 + distanceBetweenLines * (i - start + offset), maxLength, distanceBetweenLines - 1))
                    g.setColor(new Color(0.2f, 0.2f, 0.2f, 0.7f));
                if (Char.myCharz().charFocus == listChars[i])
                    g.setColor(new Color(1f, .5f, 0f, .5f));
                //if (SuicideRange.isShowSuicideRange && SuicideRange.mapObjsInMyRange.Contains(listChars[i]))
                //{
                //    g.setColor(new Color(0.5f, 0.5f, 0f, 1f));
                //    if (Char.myCharz().isStandAndCharge && GameCanvas.gameTick % 10 >= 5)
                //        g.setColor(new Color(1f, 0f, 0f, 1f));
                //}
                g.fillRect(GameCanvas.w - (x + offsetX) - maxLength, y + 1 + distanceBetweenLines * (i - start + offset), maxLength - offsetPaint, distanceBetweenLines - 1);
                if (GameCanvas.isMouseFocus(GameCanvas.w - (x + offsetX) - maxLength - offsetPaint, y + 1 + distanceBetweenLines * (i - start + offset), maxLength, distanceBetweenLines - 1))
                {
                    int length = Utils.getWidth(charDescriptions[i - start + offset].Value, charDescriptions[i - start + offset].Key);
                    g.setColor(Color.white);
                    g.fillRect(GameCanvas.w - (x + offsetX) - length - offsetPaint + 1, y + distanceBetweenLines * (i - start + offset) + 7, length - 2, 1);
                    int hp = ch.cHP;
                    int hpFull = ch.cHPFull;
                    float ratio = hp / (float)hpFull;
                    Color color = new Color(Mathf.Clamp(2 - ratio * 2, 0, 1), Mathf.Clamp(ratio * 2, 0, 1), 0);
                    g.setColor(color);
                    g.fillRect(GameCanvas.w - (x + offsetX) - length - offsetPaint + 1, y + distanceBetweenLines * (i - start + offset) + 7, (int)(ratio * (length - 2)), 1);
                }
                int offset2 = 0;
                if (ch.isBoss())
                    offset2 = -1;
                g.drawString(charDescriptions[i - start + offset].Key, -(x + offsetX) - offsetPaint, mGraphics.zoomLevel - 3 + y + distanceBetweenLines * (i - start + offset) + offset2, charDescriptions[i - start + offset].Value);
            }
        }

        private static void FillBackground(mGraphics g)
        {
            if (!isCollapsed && listChars.Count > 0)
            {
                g.setColor(new Color(0, 0, 0, .075f));
                getScrollBar(out int scrollBarWidth, out _, out _);
                if (listChars.Count <= MAX_CHAR)
                    scrollBarWidth = 0;
                int w = maxLength + 5 + (scrollBarWidth > 0 ? (scrollBarWidth + 2) : 0);
                int h = distanceBetweenLines * Math.Min(MAX_CHAR, listChars.Count) + 7;
                g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3, y - 5, w, h);
            }
        }

        static void PaintScroll(mGraphics g)
        {
            if (listChars.Count > MAX_CHAR)
            {
                getButtonUp(out int buttonUpX, out int buttonUpY);
                getButtonDown(out int buttonDownX, out int buttonDownY);
                getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight);
                g.setColor(new Color(.2f, .2f, .2f, .4f));
                g.fillRect(buttonUpX, buttonUpY, 9, scrollBarHeight + 6 * 2);
                g.drawRegion(Mob.imgHP, 0, (offset < listChars.Count - MAX_CHAR ? 18 : 54), 9, 6, 1, buttonUpX, buttonUpY, 0);
                g.drawRegion(Mob.imgHP, 0, (offset > 0 ? 18 : 54), 9, 6, 0, buttonDownX, buttonDownY, 0);
                //draw thumb
                g.setColor(new Color(.2f, .2f, .2f, .7f));
                g.fillRect(buttonUpX, buttonUpY + 6 + Mathf.CeilToInt((float)scrollBarHeight / listChars.Count * (listChars.Count - offset - MAX_CHAR)), scrollBarWidth, scrollBarThumbHeight);
                g.setColor(new Color(.7f, .7f, 0f, 1f));
                g.drawRect(buttonUpX, buttonUpY + 6 + Mathf.CeilToInt((float)scrollBarHeight / listChars.Count * (listChars.Count - offset - MAX_CHAR)), scrollBarWidth - 1, scrollBarThumbHeight - 1);
            }
        }

        static void PaintRect(mGraphics g)
        {
            getScrollBar(out int scrollBarWidth, out _, out _);
            if (listChars.Count <= MAX_CHAR)
                scrollBarWidth = 0;
            int w = maxLength + 5 + (scrollBarWidth > 0 ? (scrollBarWidth + 2) : 0);
            int h = distanceBetweenLines * Math.Min(MAX_CHAR, listChars.Count) + 7;
            int count = listChars.Where(c => c.isNormalChar()).Count();
            float ratio = count / (float)GameScr.gI().maxPlayer[TileMap.zoneID];
            Color color = new Color(Mathf.Clamp(ratio * 2, 0, 1), Mathf.Clamp(2 - ratio * 2, 0, 1), 0);
            string hexColor = $"#{(int)(color.r * 255):x2}{(int)(color.g * 255):x2}{(int)(color.b * 255):x2}{(int)(color.a * 255):x2}";
            string str = $"<color=yellow>{TileMap.mapName}</color> {Strings.zone} <color=yellow>{TileMap.zoneID}</color> [<color={hexColor}>{count}</color>/<color=red>{GameScr.gI().maxPlayer[TileMap.zoneID]}</color>]";
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 7 * mGraphics.zoomLevel,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperRight,
                richText = true
            };
            style.normal.textColor = Color.white;
            titleWidth = Utils.getWidth(style, str);
            g.setColor(new Color(.2f, .2f, .2f, .7f));
            g.fillRect(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth, 8);
            if (GameCanvas.isMouseFocus(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth, 8))
            {
                g.setColor(style.normal.textColor);
                g.fillRect(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - 1, titleWidth - 1, 1);
            }
            g.drawString(str, -(x + offsetX) + scrollBarWidth, y - distanceBetweenLines - 2, style);
            getCollapseButton(out int collapseButtonX, out int collapseButtonY);
            g.drawRegion(Mob.imgHP, 0, 18, 9, 6, (isCollapsed ? 5 : 4), collapseButtonX, collapseButtonY, 0);
            if (isCollapsed || listChars.Count <= 0)
                return;
            g.setColor(Color.yellow);
            g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3, y - 5, w - titleWidth - 9 - (scrollBarWidth > 0 ? 2 : 0), 1);
            g.fillRect(GameCanvas.w - (x + offsetX) + scrollBarWidth, y - 5, 3 + (scrollBarWidth > 0 ? 1 : 0), 1);
            g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3, y - 5, 1, h);
            g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3 + w, y - 5, 1, h + 1);
            g.fillRect(GameCanvas.w - (x + offsetX) - maxLength - 3, y - 5 + h, w + 1, 1);
        }

        internal static void updateTouch()
        {
            if (!isEnabled)
                return;
            try
            {
                if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu)
                    return;
                getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight);
                getCollapseButton(out int collapseButtonX, out int collapseButtonY);
                if (GameCanvas.isPointerHoldIn(collapseButtonX, collapseButtonY, 9, 6) || GameCanvas.isPointerHoldIn(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth, 8))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                        isCollapsed = !isCollapsed;
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
                int start = 0;
                if (listChars.Count > MAX_CHAR)
                    start = listChars.Count - MAX_CHAR;
                for (int i = start - offset; i < listChars.Count - offset; i++)
                {
                    if (GameCanvas.isPointerHoldIn(GameCanvas.w - (x + offsetX) - maxLength - (listChars[i].cFlag != 0 ? (distanceBetweenLines + 1) : 0), y + 1 + distanceBetweenLines * (i - start + offset), maxLength, distanceBetweenLines - 1))
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
                            else 
                                Utils.teleportMyChar(listChars[i]);
                        }
                        Char.myCharz().currentMovePoint = null;
                        GameCanvas.clearAllPointerEvent();
                        return;
                    }
                }
                if (listChars.Count > MAX_CHAR)
                {
                    getButtonUp(out int buttonUpX, out int buttonUpY);
                    if (GameCanvas.isPointerMove && GameCanvas.isPointerDown && GameCanvas.isPointerHoldIn(buttonUpX, buttonUpY, scrollBarWidth, scrollBarHeight))
                    {
                        float increment = scrollBarHeight / (float)listChars.Count;
                        float newOffset = (GameCanvas.pyMouse - buttonUpY) / increment;
                        if (float.IsNaN(newOffset))
                            return;
                        offset = Mathf.Clamp(listChars.Count - Mathf.RoundToInt(newOffset), 0, listChars.Count - MAX_CHAR);
                        return;
                    }
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

        static void getButtonUp(out int buttonUpX, out int buttonUpY)
        {
            buttonUpX = GameCanvas.w - (x + offsetX) + 2;
            buttonUpY = y + 1;
        }
        
        static void getButtonDown(out int buttonDownX, out int buttonDownY)
        {
            buttonDownX = GameCanvas.w - (x + offsetX) + 2;
            buttonDownY = y + 2 + distanceBetweenLines * (MAX_CHAR - 1);
        }

        static void getScrollBar(out int scrollBarWidth, out int scrollBarHeight, out int scrollBarThumbHeight)
        {
            scrollBarWidth = 9;
            scrollBarHeight = MAX_CHAR * distanceBetweenLines - 1 - 6 * 2;
            scrollBarThumbHeight = Mathf.CeilToInt((float)MAX_CHAR / listChars.Count * scrollBarHeight);
        }

        static void getCollapseButton(out int collapseButtonX, out int collapseButtonY)
        {
            getScrollBar(out int scrollBarWidth, out _, out _);
            if (listChars.Count <= MAX_CHAR)
            scrollBarWidth = 0;
            collapseButtonX = GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth - 8;
            collapseButtonY = y - distanceBetweenLines + 1;
        }

        internal static void setState(bool value) => isEnabled = value;

        internal static void setStatePet(bool value) => isShowPet = value;

        internal static int getSpaceOccupied()
        {
            if (!isEnabled) 
                return 0;
            byte border = 5;
            byte titleH = 7;
            if (isCollapsed)
                return distanceBetweenLines;
            else
                return titleH + border + distanceBetweenLines * Mathf.Clamp(listChars.Count, 0, MAX_CHAR);
        }
    }
}
