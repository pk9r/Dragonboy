using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.CharEffect
{
    internal class CharEffectTime
    {
        internal bool hasBlackStarDragonBall;
        internal int timeHoldingBlackStarDragonBall;
        internal long lastTimeHoldingBlackStarDragonBall;

        internal bool isHypnotized;
        internal int timeHypnotized;
        internal long lastTimeHypnotized;
        internal bool isHypnotizedByMe;

        internal bool hasMonkey;
        internal int timeMonkey;
        internal long lastTimeMonkey;

        internal bool hasHuytSao;
        internal int timeHuytSao;
        internal long lastTimeHuytSao;

        internal bool hasShield;
        internal int timeShield;
        internal long lastTimeShield;

        internal bool isTeleported;
        internal int timeTeleported;
        internal long lastTimeTeleported;

        internal bool isTied;
        internal int timeTied;
        internal long lastTimeTied;
        internal bool isTiedByMe;

        internal bool hasMobMe;
        internal int timeMobMe;
        internal long lastTimeMobMe;

        internal bool isTDHS;
        internal int timeTDHS;
        internal long lastTimeTDHS;

        internal bool isStone;
        internal int timeStone;
        internal long lastTimeStoned;

        internal bool isChocolate;
        internal int timeChocolate;
        internal long lastTimeChocolated;

        internal bool isSelfExplode;
        internal long lastTimeSelfExplode;
        internal int timeSelfExplode;

        internal bool isQCKK;
        internal long lastTimeQCKK;
        internal int timeQCKK;

        internal bool hasNamekianDragonBall;

        internal void Update()
        {
            if (timeHoldingBlackStarDragonBall > 0 && mSystem.currentTimeMillis() - lastTimeHoldingBlackStarDragonBall >= 1000)
            {
                timeHoldingBlackStarDragonBall--;
                lastTimeHoldingBlackStarDragonBall = mSystem.currentTimeMillis();
            }
            if (timeHypnotized > 0 && mSystem.currentTimeMillis() - lastTimeHypnotized >= 1000)
            {
                timeHypnotized--;
                if (timeHypnotized == 0)
                    isHypnotizedByMe = false;
                lastTimeHypnotized = mSystem.currentTimeMillis();
            }
            if (timeMonkey > 0 && mSystem.currentTimeMillis() - lastTimeMonkey >= 1000)
            {
                timeMonkey--;
                lastTimeMonkey = mSystem.currentTimeMillis();
            }
            if (timeHuytSao > 0 && mSystem.currentTimeMillis() - lastTimeHuytSao >= 1000)
            {
                timeHuytSao--;
                if (timeHuytSao <= 0)
                    hasHuytSao = false;
                lastTimeHuytSao = mSystem.currentTimeMillis();
            }
            if (timeShield > 0 && mSystem.currentTimeMillis() - lastTimeShield >= 1000)
            {
                timeShield--;
                lastTimeShield = mSystem.currentTimeMillis();
            }
            if (timeTeleported > 0 && mSystem.currentTimeMillis() - lastTimeTeleported >= 1000)
            {
                timeTeleported--;
                lastTimeTeleported = mSystem.currentTimeMillis();
            }
            if (timeTied > 0 && mSystem.currentTimeMillis() - lastTimeTied >= 1000)
            {
                timeTied--;
                if (timeTied == 0)
                    isTiedByMe = false;
                lastTimeTied = mSystem.currentTimeMillis();
            }
            if (timeMobMe > 0 && mSystem.currentTimeMillis() - lastTimeMobMe >= 1000)
            {
                timeMobMe--;
                lastTimeMobMe = mSystem.currentTimeMillis();
            }
            if (timeTDHS > 0 && mSystem.currentTimeMillis() - lastTimeTDHS >= 1000)
            {
                timeTDHS--;
                lastTimeTDHS = mSystem.currentTimeMillis();
            }
            if (timeStone > 0 && mSystem.currentTimeMillis() - lastTimeStoned >= 1000)
            {
                timeStone--;
                lastTimeStoned = mSystem.currentTimeMillis();
            }
            if (timeChocolate > 0 && mSystem.currentTimeMillis() - lastTimeChocolated >= 1000)
            {
                timeChocolate--;
                lastTimeChocolated = mSystem.currentTimeMillis();
            }

            if (timeSelfExplode > 0 && mSystem.currentTimeMillis() - lastTimeSelfExplode >= 1000)
            {
                timeSelfExplode--;
                lastTimeSelfExplode = mSystem.currentTimeMillis();
            }
            if (timeQCKK > 0 && mSystem.currentTimeMillis() - lastTimeQCKK >= 1000)
            {
                timeQCKK--;
                lastTimeQCKK = mSystem.currentTimeMillis();
            }
        }

        internal bool HasAnyEffect() => timeTeleported + timeTied + timeHoldingBlackStarDragonBall + timeHuytSao + timeMobMe + timeMonkey + timeShield + timeHypnotized + timeTDHS + timeStone + timeChocolate + timeSelfExplode + timeQCKK > 0 || hasNamekianDragonBall;
    }
}