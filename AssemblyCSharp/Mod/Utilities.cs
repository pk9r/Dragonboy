using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod
{
    public class Utilities
    {
        [ChatCommand("tdc")]
        public static void editSpeedRun(int speed)
        {
            Char.myCharz().cspeed = speed;

            GameScr.info1.addInfo("Tốc độ chạy: " + speed, 0);
        }

        public static void AddKeyMap(Hashtable h)
        {
            h.Add(KeyCode.Slash, 47);
        }

        public static void AddHotkeys()
        {
            if (GameCanvas.keyAsciiPress == '/')
            {
                ChatTextField.gI().startChat('/', GameScr.gI(), string.Empty);
            }
        }
    }

}
