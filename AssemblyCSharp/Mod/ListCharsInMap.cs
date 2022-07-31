using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static UnityEngine.Random;

namespace Mod;
public class ListCharsInMap
{
    public static List<Char> listChars = new List<Char>();

    public static bool isEnabled;

    public static bool isShowPet;

    static int longestStrLength = 0;

    static int paddingRight = 15;

    static int distanceBetweenLines = 8;


    public static void update()
    {
        if (!isEnabled) return;
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
                    {
                        listChars.Add(chPet);
                    }
                }
            }
        }
    }

    public static void paint(mGraphics g)
    {
        if (!isEnabled) return;
        distanceBetweenLines = 8;
        int startY = 50;
        int skippedPetCount = 0;
        longestStrLength = 0;
        Dictionary<string, mFont> charDescriptions = new Dictionary<string, mFont>();
        for (int i = 0; i < listChars.Count; i++)
        {
            Char ch = listChars[i];
            mFont mfont = mFont.tahoma_7_white_tiny;
            if (ch.charEffectTime.hasNRD || CharExtensions.isBoss(ch))
            {
                mfont = mFont.tahoma_7b_red_tiny;
            }
            if (ch.cHP <= 0) mfont = mFont.tahoma_7_tiny;
            string charDesc = ch.cName + " [" + NinjaUtil.getMoneys(ch.cHP) + "/" + NinjaUtil.getMoneys(ch.cHPFull) + "]";
            string petDesc = string.Empty;
            if (CharExtensions.isNormalChar(ch, false, false))
            {
                charDesc = ch.cName + " [" + NinjaUtil.getMoneys(ch.cHP) + "/" + NinjaUtil.getMoneys(ch.cHPFull) + "] - " + CharExtensions.getGender(ch) + " [" + ch.charID + "]";
            }
            if (CharExtensions.isPet(ch))
            {
                mFont mf = mFont.tahoma_7_blue_tiny;
                if (ch.cHP <= 0) mf = mFont.tahoma_7_tiny;
                petDesc = ch.cName.Replace("$", "").Replace("#", "") + " [" + NinjaUtil.getMoneys(ch.cHP) + "/" + NinjaUtil.getMoneys(ch.cHPFull) + " - " + CharExtensions.getGender(ch) + "]";
                //mf.drawString(g, petDesc, GameCanvas.w - paddingRight, startY, mFont.RIGHT);
                charDescriptions.Add(petDesc, mf);
                skippedPetCount++;
            }
            else if (!CharExtensions.isBoss(ch))
            {
                string str = i + 1 - skippedPetCount + ". " + charDesc;
                charDescriptions.Add(str, mfont);
                //mfont.drawString(g, str, GameCanvas.w - paddingRight, startY, mFont.RIGHT);
            }
            else
            {
                //mfont.drawString(g, charDesc, GameCanvas.w - paddingRight, startY, mFont.RIGHT);
                charDescriptions.Add(charDesc, mfont);
            }
            //g.setColor(CharExtensions.getFlagColor(ch));
            //g.fillRect(GameCanvas.w - paddingRight + 2, startY + 1, 6, 6);
            //if (ch.cFlag == 9) mFont.tahoma_7_white_tiny.drawString(g, "K", GameCanvas.w - paddingRight + 5, startY, mFont.CENTER);
            //if (ch.cFlag == 10) mFont.tahoma_7_white_tiny.drawString(g, "M", GameCanvas.w - paddingRight + 5, startY, mFont.CENTER);
            if (charDesc.Length > longestStrLength) longestStrLength = charDesc.Length;
            if (petDesc.Length > longestStrLength) longestStrLength = charDesc.Length;
            //startY += distanceBetweenLines;
        }
        for (int i = 0; i < listChars.Count; i++)
        {
            g.setColor(new Color(0.3f, 0.3f, 0.3f, 0.75f));
            if (Char.myCharz().charFocus == listChars[i]) g.setColor(new Color(0.3f, 0f, 0f, 0.9f));
            g.fillRect(Mathf.RoundToInt(GameCanvas.w - paddingRight - longestStrLength * 3.3f), startY + 1 + distanceBetweenLines * i, Mathf.RoundToInt(longestStrLength * 3.3f), 7);
        }
        for (int i = 0; i < listChars.Count; i++)
        {
            Char ch = listChars[i];

            charDescriptions.ElementAt(i).Value.drawString(g, charDescriptions.ElementAt(i).Key, GameCanvas.w - paddingRight, startY, mFont.RIGHT);
            g.setColor(CharExtensions.getFlagColor(ch));
            g.fillRect(GameCanvas.w - paddingRight + 2, startY + 1, 7, 7);
            if (ch.cFlag == 9) mFont.tahoma_7_white_tiny.drawString(g, "K", GameCanvas.w - paddingRight + 5, startY, mFont.CENTER);
            if (ch.cFlag == 10) mFont.tahoma_7_white_tiny.drawString(g, "M", GameCanvas.w - paddingRight + 5, startY, mFont.CENTER);
            startY += distanceBetweenLines;
        }
    }

    public static void updateTouch()
    {
        if (!GameCanvas.isTouch || ChatTextField.gI().isShow || GameCanvas.menu.showMenu) return;
        int startY = 50;
        for (int i = 0; i < listChars.Count; i++)
        {
            if (GameCanvas.isPointerHoldIn(Mathf.RoundToInt(GameCanvas.w - paddingRight - longestStrLength * 3.3f), startY + 1 + distanceBetweenLines * i, Mathf.RoundToInt(longestStrLength * 3.3f), 7))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                {
                    Char.myCharz().mobFocus = null;
                    Char.myCharz().npcFocus = null;
                    Char.myCharz().itemFocus = null;
                    if (Char.myCharz().charFocus != listChars[i])
                    {
                        Char.myCharz().charFocus = listChars[i];
                    }
                    else Utilities.teleportMyChar(listChars[i].cx, listChars[i].cy);
                }
                Char.myCharz().currentMovePoint = null;
                GameCanvas.clearAllPointerEvent();
                return;
            }
        }
    }
}
