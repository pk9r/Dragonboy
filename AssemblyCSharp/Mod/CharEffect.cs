using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod
{
    public class CharEffect
    {
        public static bool isEnabled = true;

        public static List<Char> storedChars = new List<Char>();

        public static void Update()
        {
            for (int i = storedChars.Count - 1; i >= 0; i--)
            {
                Char c = storedChars.ElementAt(i);
                Char @char = GameScr.findCharInMap(c.charID);
                if (!c.charEffectTime.HasAnyEffect()) storedChars.RemoveAt(i);
                else if (@char == null) c.charEffectTime.Update();
                else
                {
                    GameScr.findCharInMap(c.charID).charEffectTime = c.charEffectTime;
                    storedChars[i] = @char;
                }
            }
        }

        public static bool isContains(int charId)
        {
            foreach (Char c in storedChars) if (c.charID == charId) return true;
            return false;
        }

        public static void Paint(mGraphics g)
        {
            if (!isEnabled) return;
            Char focus = Char.myCharz().charFocus;
            if (focus == null) return;
            List<string> strs = new List<string>();
            int y = 72;
            strs.Add(focus.cName + " [" + NinjaUtil.getMoneys(focus.cHP) + "/" + NinjaUtil.getMoneys(focus.cHPFull) + "]");
            if (focus.charEffectTime != null && focus.charEffectTime.HasAnyEffect())
            {
                if (focus.charEffectTime.hasNRD) strs.Add("NRD còn: " + focus.charEffectTime.timeHoldingNRD + " giây");
                if (focus.charEffectTime.hasShield) strs.Add("Khiên còn khoảng: " + focus.charEffectTime.timeShield + " giây");
                if (focus.charEffectTime.hasMonkey) strs.Add("Khỉ còn: " + focus.charEffectTime.timeMonkey + " giây");
                if (focus.charEffectTime.hasHuytSao) strs.Add("Huýt sáo còn: " + focus.charEffectTime.timeHuytSao + " giây");
                if (focus.charEffectTime.hasMobMe) strs.Add("Đẻ trứng còn: " + focus.charEffectTime.timeMobMe + " giây");
                if (focus.charEffectTime.isHypnotized) strs.Add(focus.charEffectTime.isHypnotizedByMe ? "Bị bạn thôi miên: " : "Bị thôi miên: khoảng " + focus.charEffectTime.timeHypnotized + " giây");
                if (focus.charEffectTime.isTeleported) strs.Add("Bị DCTT: " + focus.charEffectTime.timeTeleported + " giây");
                if (focus.charEffectTime.isTDHS) strs.Add("Bị TDHS: khoảng " + focus.charEffectTime.timeTDHS + " giây");
                if (focus.charEffectTime.isTied) strs.Add((focus.charEffectTime.isTiedByMe ? "Bị bạn trói: " : "Bị trói: khoảng ") + focus.charEffectTime.timeTied + " giây");
                if (focus.charEffectTime.isStone) strs.Add("Bị hóa đá: " + focus.charEffectTime.timeStone + " giây");
                if (focus.charEffectTime.isChocolate) strs.Add("Bị biến Sôcôla: " + focus.charEffectTime.timeChocolate + " giây");
            }
            foreach (string str in strs)
            {
                g.setColor(new Color(0f, 0f, 0f, 0.4f));
                g.fillRect(GameCanvas.w / 2 - mFont.tahoma_7b_red.getWidth(str) / 2 - 5, y + 1, mFont.tahoma_7b_red.getWidth(str) + 8, 10);
                mFont.tahoma_7b_red.drawString(g, str, GameCanvas.w / 2, y, mFont.CENTER);
                y += 10;
            }
        }

        public static void AddEffectCreatedByMe(Skill skill)
        {
            if (Char.myCharz().charFocus != null)
            {
                if (skill.template.id == 22) Char.myCharz().charFocus.charEffectTime.isHypnotizedByMe = true;
                if (skill.template.id == 23) Char.myCharz().charFocus.charEffectTime.isTiedByMe = true;
            }
        }
    }
}