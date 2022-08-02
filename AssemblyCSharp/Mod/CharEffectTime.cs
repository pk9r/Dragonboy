using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod;
public class CharEffectTime
{
    public bool hasNRD;

    public int timeHoldingNRD;

    public long lastTimeHoldNRD;

    public bool isHypnotized;

    public int timeHypnotized;

    public long lastTimeHypnotized;

    public bool isHypnotizedByMe;

    public bool hasMonkey;

    public int timeMonkey;

    public long lastTimeMonkey;

    public bool hasHuytSao;

    public int timeHuytSao;

    public long lastTimeHuytSao;

    public bool hasShield;

    public int timeShield;

    public long lastTimeShield;

    public bool isTeleported;

    public int timeTeleported;

    public long lastTimeTeleported;

    public bool isTied;

    public int timeTied;

    public long lastTimeTied;

    public bool isTiedByMe;

    public bool hasMobMe;

    public int timeMobMe;

    public long lastTimeMobMe;

    public bool isTDHS;

    public int timeTDHS;

    public long lastTimeTDHS;

    public bool isStone;

    public int timeStone;

    public long lastTimeStoned; 

    public bool isChocolate;

    public int timeChocolate;

    public long lastTimeChocolated;

    public void Update()
    {
        if (timeHoldingNRD > 0 && mSystem.currentTimeMillis() - lastTimeHoldNRD >= 1000)
        {
            timeHoldingNRD--;
            lastTimeHoldNRD = mSystem.currentTimeMillis();
        }
        if (timeHypnotized > 0 && mSystem.currentTimeMillis() - lastTimeHypnotized >= 1000)
        {
            timeHypnotized--;
            if (timeHypnotized == 0) isHypnotizedByMe = false;
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
            if (timeTied == 0) isTiedByMe = false;
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
        if (timeHuytSao <= 0) hasHuytSao = false;
    }

    public bool HasAnyEffect()
    {
        return timeTeleported + timeTied + timeHoldingNRD + timeHuytSao + timeMobMe + timeMonkey + timeShield + timeHypnotized + timeTDHS + timeStone + timeChocolate > 0;
    }
}
