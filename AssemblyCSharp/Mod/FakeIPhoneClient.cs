using Mod.ModMenu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod
{
    internal class FakeIPhoneClient
    {
        public static bool isEnabled;

        static int clientType = 4;

        internal static void setState(bool value)
        {
            isEnabled = value;
            clientType = value ? 7 : 4;
            if (!new StackFrame(2).GetMethod().Equals(typeof(ModMenuMain).GetMethod(nameof(ModMenuMain.LoadData))))
            {
                Rms.saveRMSInt("clienttype", clientType);
                GameCanvas.startYesNoDlg("Khởi động lại game để thay đổi có hiệu lực!", new Command("Thoát game", 2007, null), new Command("Hủy", 2001, null));
            }
        }

        public static void onExitGame()
        {
            if (!isEnabled)
                return;
            if (clientType != Rms.loadRMSInt("clienttype"))
            {
                Rms.clearAll();
                Rms.saveRMSInt("clienttype", 7);
            }
        }

        public static void onGameStart()
        {
            if (!isEnabled)
                return;
            Rms.saveRMSInt("clienttype", 7);
        }
    }
}
