using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mod.CharEffect
{
    internal class CharEffectMain
    {
        internal static bool isEnabled;

        internal static List<Char> storedChars = new List<Char>();

        internal static bool isNRDAdded, isTieAdded, isTDHSAdded, isMobMeAdded, isMonkeyAdded;

        static short NRSDImageId;

        static readonly int PADDING = 5;

        static Image[] charEffectImages;

        internal static void Init()
        {
            charEffectImages = new Image[]
            {
                GameCanvas.loadImage("/CharEffect/nrd1"),
                GameCanvas.loadImage("/CharEffect/nrd2"),
                GameCanvas.loadImage("/CharEffect/nrd3"),
                GameCanvas.loadImage("/CharEffect/nrd4"),
                GameCanvas.loadImage("/CharEffect/nrd5"),
                GameCanvas.loadImage("/CharEffect/nrd6"),
                GameCanvas.loadImage("/CharEffect/nrd7"),
                GameCanvas.loadImage("/CharEffect/shield"),
                GameCanvas.loadImage("/CharEffect/monkey"),
                GameCanvas.loadImage("/CharEffect/whistle"),
                GameCanvas.loadImage("/CharEffect/mobme"),
                GameCanvas.loadImage("/CharEffect/hypnotize"),
                GameCanvas.loadImage("/CharEffect/teleport"),
                GameCanvas.loadImage("/CharEffect/blind"),
                GameCanvas.loadImage("/CharEffect/tie"),
                GameCanvas.loadImage("/CharEffect/stone"),
                GameCanvas.loadImage("/CharEffect/choco"),
                GameCanvas.loadImage("/CharEffect/selfexplode"),
                GameCanvas.loadImage("/CharEffect/nrnm"),
                GameCanvas.loadImage("/CharEffect/qckk"),
            };
        }

        internal static void updateMe()
        {
            CharEffectTime meEffTime = Char.myCharz().charEffectTime;
            if (meEffTime.hasMobMe)
            {
                if (!isMobMeAdded)
                {
                    isMobMeAdded = true;
                    if (Utils.isMeWearingPikkoroDaimaoSet())
                        Char.vItemTime.addElement(new ItemTime(722, true));
                    else
                        Char.vItemTime.addElement(new ItemTime(722, Char.myCharz().getTimeMobMe()));
                }
            }
            else if (isMobMeAdded)
            {
                isMobMeAdded = false;
                if (Utils.isMeWearingPikkoroDaimaoSet())
                    removeElement(new ItemTime(722, true));
                else
                    removeElement(new ItemTime(722, 0));
            }
            if (meEffTime.isTied)
            {
                if (!isTieAdded)
                {
                    isTieAdded = true;
                    Char.vItemTime.addElement(new ItemTime(3779, 35, true));
                }
            }
            else if (isTieAdded)
            {
                isTieAdded = false;
                removeElement(new ItemTime(3779, 0, true));
            }
            if (meEffTime.isTDHS)
            {
                if (!isTDHSAdded)
                {
                    isTDHSAdded = true;
                    Char.vItemTime.addElement(new ItemTime(717, Char.myCharz().freezSeconds));
                }
            }
            else if (isTDHSAdded)
            {
                isTDHSAdded = false;
                removeElement(new ItemTime(717, 0));
            }
            if (meEffTime.hasMonkey)
            {
                if (!isMonkeyAdded)
                {
                    isMonkeyAdded = true;
                    if (Utils.isMeWearingCadicSet())
                        Char.vItemTime.addElement(new ItemTime(718, Char.myCharz().getTimeMonkey() * 5));
                    else
                        Char.vItemTime.addElement(new ItemTime(718, Char.myCharz().getTimeMonkey()));
                }
            }
            else if (isMonkeyAdded)
            {
                isMonkeyAdded = false;
                removeElement(new ItemTime(718, 0));
            }
            if (meEffTime.hasBlackStarDragonBall)
            {
                if (!isNRDAdded)
                {
                    isNRDAdded = true;
                    NRSDImageId = Utils.getNRSDId();
                    Char.vItemTime.addElement(new ItemTime(NRSDImageId, 300));
                }
            }
            else if (isNRDAdded)
            {
                isNRDAdded = false;
                removeElement(new ItemTime(NRSDImageId, 0));
            }

        }

        internal static void removeElement(ItemTime item)
        {
            for (int i = 0; i < Char.vItemTime.size(); i++)
            {
                ItemTime itemTime = Char.vItemTime.elementAt(i) as ItemTime;
                if (itemTime.idIcon == item.idIcon && itemTime.isEquivalence == item.isEquivalence && itemTime.isInfinity == item.isInfinity)
                {
                    Char.vItemTime.removeElementAt(i);
                    return;
                }
            }
        }

        internal static void Update()
        {
            updateMe();
            for (int i = storedChars.Count - 1; i >= 0; i--)
            {
                Char c = storedChars.ElementAt(i);
                Char ch = GameScr.findCharInMap(c.charID);
                if (!c.charEffectTime.HasAnyEffect())
                    storedChars.RemoveAt(i);
                else if (ch == null)
                    c.charEffectTime.Update();
                else
                {
                    GameScr.findCharInMap(c.charID).charEffectTime = c.charEffectTime;
                    storedChars[i] = ch;
                }
            }
        }

        internal static bool isContains(int charId)
        {
            foreach (Char c in storedChars)
                if (c.charID == charId)
                    return true;
            return false;
        }

        internal static void Paint(mGraphics g)
        {
            if (!isEnabled)
                return;
            Char focus = Char.myCharz().charFocus;
            if (focus == null)
                return;
            string str = $"{focus.cName} - {Utils.FormatWithSIPrefix(focus.cHP)}/{Utils.FormatWithSIPrefix(focus.cHPFull)}";
            if (focus.charID > 0)
                str += $" [{focus.charID}]";
            g.setColor(new Color(0f, 0f, 0f, 0.4f));
            g.fillRect(GameCanvas.w / 2 - mFont.tahoma_7b_red.getWidth(str) / 2 - 5, 50 + 1, mFont.tahoma_7b_red.getWidth(str) + 8, 10);
            mFont.tahoma_7b_red.drawString(g, str, GameCanvas.w / 2, 50, mFont.CENTER);
            if (focus.charEffectTime != null && focus.charEffectTime.HasAnyEffect())
            {
                Dictionary<Image, string> effects = new Dictionary<Image, string>();
                if (focus.charEffectTime.hasBlackStarDragonBall)
                    effects.Add(charEffectImages[TileMap.mapID - 85], focus.charEffectTime.timeHoldingBlackStarDragonBall + "s");
                if (focus.charEffectTime.hasShield)
                    effects.Add(charEffectImages[7], "~" + focus.charEffectTime.timeShield + "s");
                if (focus.charEffectTime.hasMonkey)
                    effects.Add(charEffectImages[8], focus.charEffectTime.timeMonkey + "s");
                if (focus.charEffectTime.hasHuytSao)
                    effects.Add(charEffectImages[9], focus.charEffectTime.timeHuytSao + "s");
                if (focus.charEffectTime.hasMobMe)
                    effects.Add(charEffectImages[10], focus.charEffectTime.timeMobMe + "s");
                if (focus.charEffectTime.isHypnotized)
                    effects.Add(charEffectImages[11], focus.charEffectTime.isHypnotizedByMe ? "" : "~" + focus.charEffectTime.timeHypnotized + "s");
                if (focus.charEffectTime.isTeleported)
                    effects.Add(charEffectImages[12], focus.charEffectTime.timeTeleported + "s");
                if (focus.charEffectTime.isTDHS)
                    effects.Add(charEffectImages[13], focus.charEffectTime.timeTDHS + "s");
                if (focus.charEffectTime.isTied)
                    effects.Add(charEffectImages[14], focus.charEffectTime.isTiedByMe ? "" : "~" + focus.charEffectTime.timeTied + "s");
                if (focus.charEffectTime.isStone)
                    effects.Add(charEffectImages[15], focus.charEffectTime.timeStone + "s");
                if (focus.charEffectTime.isChocolate)
                    effects.Add(charEffectImages[16], focus.charEffectTime.timeChocolate + "s");
                if (focus.charEffectTime.isSelfExplode)
                    effects.Add(charEffectImages[17], focus.charEffectTime.timeSelfExplode + "s");
                if (focus.charEffectTime.hasNamekianDragonBall)
                    effects.Add(charEffectImages[18], "???");
                if (focus.charEffectTime.isQCKK)
                    effects.Add(charEffectImages[19], focus.charEffectTime.timeQCKK + "s");

                int totalWidth = charEffectImages[0].w / mGraphics.zoomLevel * effects.Count + PADDING * (effects.Count - 1);
                int xStart = GameCanvas.w / 2 - totalWidth / 2;
                g.fillRect(xStart - 3, 64 - 3, totalWidth + 6, 20 + mFont.tahoma_7b_white.getHeight() + 6);
                foreach (KeyValuePair<Image, string> effect in effects)
                {
                    g.drawImage(effect.Key, xStart, 64);
                    mFont.tahoma_7_white.drawString(g, effect.Value, xStart + 20 / 2, 64 + mFont.tahoma_7b_white.getHeight() + 20 / 2, mFont.CENTER, mFont.tahoma_7b_dark);
                    xStart += 20 + PADDING;
                }
            }
        }

        internal static void AddEffectCreatedByMe(Skill skill)
        {
            if (Char.myCharz().charFocus != null)
            {
                if (skill.template.id == 22)
                    Char.myCharz().charFocus.charEffectTime.isHypnotizedByMe = true;
                if (skill.template.id == 23)
                    Char.myCharz().charFocus.charEffectTime.isTiedByMe = true;
            }
            if (skill.template.id == 6 && mSystem.currentTimeMillis() - skill.lastTimeUseThisSkill > skill.coolDown)
                Char.vItemTime.addElement(new ItemTime(717, Char.myCharz().getTimeTDHS() - 1));
        }

        internal static void setState(bool value) => isEnabled = value;

        internal static void UpdateChar(Char ch)
        {
            if (ch.charEffectTime.HasAnyEffect() && !isContains(ch.charID))
                storedChars.Add(ch);
            ch.charEffectTime.Update();
            if (ch.head == 412 && ch.body == 413 && ch.leg == 414)
            {
                if (!ch.charEffectTime.isChocolate)
                {
                    ch.charEffectTime.isChocolate = true;
                    ch.charEffectTime.lastTimeChocolated = mSystem.currentTimeMillis();
                    ch.charEffectTime.timeChocolate = ch.getTimeChocolate() + 1;
                }
            }
            else
            {
                ch.charEffectTime.isChocolate = false;
                ch.charEffectTime.timeChocolate = 0;
            }
            if (ch.bag >= 0 && ClanImage.idImages.containsKey(ch.bag.ToString()))
            {
                ClanImage clanImage = (ClanImage)ClanImage.idImages.get(ch.bag.ToString());
                bool isResetNRD = true;
                bool hasNamekianDragonBall = false;
                if (clanImage.idImage != null)
                {
                    for (int i = 0; i < clanImage.idImage.Length; i++)
                    {
                        if (clanImage.idImage[i] == 2322 && Utils.IsMeInNRDMap())
                        {
                            ch.charEffectTime.hasBlackStarDragonBall = true;
                            isResetNRD = false;
                            if (ch.charEffectTime.timeHoldingBlackStarDragonBall == 0)
                                ch.charEffectTime.timeHoldingBlackStarDragonBall = 302;
                            break;
                        }
                        if (clanImage.idImage[i] == 2287)
                            hasNamekianDragonBall = true;
                    }
                }
                if (isResetNRD)
                {
                    ch.charEffectTime.hasBlackStarDragonBall = false;
                    ch.charEffectTime.timeHoldingBlackStarDragonBall = 0;
                }
                ch.charEffectTime.hasNamekianDragonBall = hasNamekianDragonBall;
            }

            if (ch.sleepEff)
            {
                if (!ch.charEffectTime.isHypnotized)
                {
                    ch.charEffectTime.isHypnotized = true;
                    ch.charEffectTime.lastTimeHypnotized = mSystem.currentTimeMillis();
                    if (ch.charEffectTime.timeHypnotized <= 0)
                        ch.charEffectTime.timeHypnotized = ch.getTimeHypnotize() + 1;
                }
            }
            else
            {
                ch.charEffectTime.isHypnotized = false;
                ch.charEffectTime.timeHypnotized = 0;
            }
            if (ch.isMonkey == 1)
            {
                if (!ch.charEffectTime.hasMonkey)
                {
                    ch.charEffectTime.hasMonkey = true;
                    ch.charEffectTime.lastTimeMonkey = mSystem.currentTimeMillis();
                    if (ch.charEffectTime.timeMonkey <= 0)
                        ch.charEffectTime.timeMonkey = ch.getTimeMonkey();
                }
            }
            else
            {
                ch.charEffectTime.hasMonkey = false;
                ch.charEffectTime.timeMonkey = 0;
            }
            if (ch.huytSao)
            {
                if (!ch.charEffectTime.hasHuytSao)
                {
                    ch.charEffectTime.hasHuytSao = true;
                    ch.charEffectTime.lastTimeHuytSao = mSystem.currentTimeMillis();
                    if (ch.charEffectTime.timeHuytSao <= 0)
                        ch.charEffectTime.timeHuytSao = ch.getTimeHuytSao() + 1;
                }
            }
            if (ch.blindEff)
            {
                if (!ch.charEffectTime.isTeleported)
                {
                    ch.charEffectTime.isTeleported = true;
                    ch.charEffectTime.lastTimeTeleported = mSystem.currentTimeMillis();
                    if (ch.charEffectTime.timeTeleported <= 0)
                        ch.charEffectTime.timeTeleported = 6 + 1;
                }
            }
            else
            {
                ch.charEffectTime.isTeleported = false;
                ch.charEffectTime.timeTeleported = 0;
            }
            if (ch.protectEff)
            {
                if (!ch.charEffectTime.hasShield)
                {
                    ch.charEffectTime.hasShield = true;
                    ch.charEffectTime.lastTimeShield = mSystem.currentTimeMillis();
                    if (ch.charEffectTime.timeShield <= 0)
                        ch.charEffectTime.timeShield = ch.getTimeShield() + 1;
                }
            }
            else
            {
                ch.charEffectTime.hasShield = false;
                ch.charEffectTime.timeShield = 0;
            }
            if (ch.stone)
            {
                if (!ch.charEffectTime.isStone)
                {
                    ch.charEffectTime.isStone = true;
                    ch.charEffectTime.lastTimeStoned = mSystem.currentTimeMillis();
                    if (ch.charEffectTime.timeStone <= 0)
                        ch.charEffectTime.timeStone = ch.getTimeStone() + 1;
                }
            }
            else
            {
                ch.charEffectTime.isStone = false;
                ch.charEffectTime.timeStone = 0;
            }
            if (ch.isFreez)
            {
                ch.charEffectTime.isTDHS = true;
                if (ch.me && ch.meDead)
                {
                    ch.freezSeconds = 0;
                    ch.charEffectTime.isTDHS = false;
                    ch.charEffectTime.timeTDHS = 0;
                }
                else
                {
                    ch.charEffectTime.lastTimeTDHS = mSystem.currentTimeMillis();
                    ch.charEffectTime.timeTDHS = ch.freezSeconds + 1;
                }
            }
            else
            {
                ch.charEffectTime.isTDHS = false;
                ch.charEffectTime.timeTDHS = 0;
            }
            if (ch.mobMe != null)
            {
                if (ch.mobMe.isDie || ch.mobMe.hp <= 0)
                    ch.charEffectTime.hasMobMe = false;
                else if (!ch.charEffectTime.hasMobMe)
                {
                    ch.charEffectTime.hasMobMe = true;
                    ch.charEffectTime.lastTimeMobMe = mSystem.currentTimeMillis();
                    if (ch.charEffectTime.timeMobMe <= 0)
                        ch.charEffectTime.timeMobMe = ch.getTimeMobMe() + 1;
                }
            }
            else
                ch.charEffectTime.hasMobMe = false;
            if (ch.holdEffID == 0 && !ch.blindEff && !ch.sleepEff && ch.holder && ch.me && ch.statusMe == 2 && ch.currentMovePoint != null)
            {
                ch.charEffectTime.isTied = false;
                if (ch.charHold != null)
                    ch.charHold.charEffectTime.isTiedByMe = false;
            }
            if (ch.isStandAndCharge)
            {
                if (!ch.charEffectTime.isSelfExplode)
                {
                    ch.charEffectTime.isSelfExplode = true;
                    ch.charEffectTime.lastTimeSelfExplode = mSystem.currentTimeMillis();
                    if (ch.charEffectTime.timeSelfExplode <= 0)
                        ch.charEffectTime.timeSelfExplode = ch.getTimeSelfExplode();
                }
            }
            else
            {
                ch.charEffectTime.isSelfExplode = false;
                ch.charEffectTime.timeSelfExplode = 0;
            }
            if (ch.isFlyAndCharge)
            {
                if (!ch.charEffectTime.isQCKK)
                {
                    ch.charEffectTime.isQCKK = true;
                    ch.charEffectTime.lastTimeQCKK = mSystem.currentTimeMillis();
                    if (ch.charEffectTime.timeQCKK <= 0)
                        ch.charEffectTime.timeQCKK = ch.getTimeQCKK();
                }
            }
            else
            {
                ch.charEffectTime.isQCKK = false;
                ch.charEffectTime.timeQCKK = 0;
            }
        }

        internal static void AddCharHoldChar(Char ch, Char r)
        {
            r.charEffectTime.isTied = true;
            r.charEffectTime.timeTied = r.getTimeHold() + 1;
            if (ch.me)
                Char.vItemTime.addElement(new ItemTime(3779, ch.getTimeHold()));
        }

        internal static void AddCharHoldMob(Char ch)
        {
            if (ch.me)
                Char.vItemTime.addElement(new ItemTime(3779, ch.getTimeHold()));
        }

        internal static void RemoveHold(Char ch)
        {
            if (ch.me)
                removeElement(new ItemTime(3779, 0));
        }
    }
}