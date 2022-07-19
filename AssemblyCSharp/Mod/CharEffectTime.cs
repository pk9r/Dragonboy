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

    public bool isSleep;

    public int timeSleep;

    public long lastTimeSleep;

    public bool hasMonkey;

    public int timeMonkey;

    public long lastTimeMonkey;

    public bool hasHuytSao;

    public int timeHuytSao;

    public long lastTimeHuytSao;

    public bool hasShield;

    public int timeShield;

    public long lastTimeShield;

    public bool isBlind;

    public int timeBlind;

    public long lastTimeBlind;

    public bool isHold;

    public int timeHold;

    public long lastTimeHold;

    public bool hasMobMe;

    public int timeMobMe;

    public long lastTimeMobMe;

    public bool isTDHS;

    public int timeTDHS;

    public long lastTimeTDHS;

    public void Update()
    {
        if (timeHoldingNRD > 0 && mSystem.currentTimeMillis() - lastTimeHoldNRD >= 1000)
        {
            timeHoldingNRD--;
            lastTimeHoldNRD = mSystem.currentTimeMillis();
        }
        if (timeSleep > 0 && mSystem.currentTimeMillis() - lastTimeSleep >= 1000)
        {
            timeSleep--;
            lastTimeSleep = mSystem.currentTimeMillis();
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
        if (timeBlind > 0 && mSystem.currentTimeMillis() - lastTimeBlind >= 1000)
        {
            timeBlind--;
            lastTimeBlind = mSystem.currentTimeMillis();
        }
        if (timeHold > 0 && mSystem.currentTimeMillis() - lastTimeHold >= 1000)
        {
            timeHold--;
            lastTimeHold = mSystem.currentTimeMillis();
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
        if (timeHuytSao <= 0) hasHuytSao = false;
    }

    public bool HasAnyEffect()
    {
        return timeBlind + timeHold + timeHoldingNRD + timeHuytSao + timeMobMe + timeMonkey + timeShield + timeSleep + timeTDHS > 0;
    }
}
