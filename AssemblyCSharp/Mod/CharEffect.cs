using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod;
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
            if (focus.charEffectTime.isSleep) strs.Add("Bị thôi miên: khoảng " + focus.charEffectTime.timeSleep + " giây");
            if (focus.charEffectTime.isBlind) strs.Add("Bị DCTT: " + focus.charEffectTime.timeBlind + " giây");
            if (focus.charEffectTime.isTDHS) strs.Add("Bị TDHS: khoảng " + focus.charEffectTime.timeTDHS + " giây");
            if (focus.charEffectTime.isHold) strs.Add("Bị trói: khoảng " + focus.charEffectTime.timeHold + " giây");
        }
        foreach (string str in strs)
        {
            mFont.tahoma_7b_red.drawString(g, str, GameCanvas.w / 2, y, mFont.CENTER);
            y += 10;
        }
    }
}
