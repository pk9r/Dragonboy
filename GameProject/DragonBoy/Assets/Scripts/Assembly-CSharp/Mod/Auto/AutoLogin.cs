using Mod.R;
using Mod.Xmap;
using UnityEngine;

namespace Mod.Auto
{
    internal class AutoLogin
    {
        internal static bool isEnabled;
        internal static bool IsRunning => isEnabled && steps > 0;
        static long lastTimeAttemptLogin;
        static long lastTimeUpdate;
        static int lastMapID;
        static int lastZoneID;
        static int lastX;
        static int lastY;
        static int steps;

        internal static void Update()
        {
            if (!isEnabled)
                return;
            switch (steps)
            {
                case 0:
                default:
                    CheckForDisconnected();
                    break;
                case 1:
                    if (mSystem.currentTimeMillis() - lastTimeUpdate <= 750)
                        return;
                    lastTimeUpdate = mSystem.currentTimeMillis();
                    AttemptLogin();
                    break;
                case 2:
                    if (mSystem.currentTimeMillis() - lastTimeUpdate <= 750)
                        return;
                    lastTimeUpdate = mSystem.currentTimeMillis();
                    GotoLastMapAndZone();
                    break;
            }
        }

        static void CheckForDisconnected()
        {
            if (GameCanvas.currentScreen is not GameScr || !Session_ME.gI().isConnected())
            {
                lastTimeAttemptLogin = mSystem.currentTimeMillis();
                GameCanvas.serverScreen.switchToMe();
                GameCanvas.startOKDlg(string.Format(Strings.autoLoginReattemptLoginIn, 30) + '!');
                steps = 1;
            }
        }

        static void AttemptLogin()
        {
            if (GameCanvas.currentScreen is GameScr)
            {
                steps = 2;
                return;
            }
            GameCanvas.startOKDlg(string.Format(Strings.autoLoginReattemptLoginIn, 30 - (mSystem.currentTimeMillis() - lastTimeAttemptLogin) / 1000) + '!');
            if (mSystem.currentTimeMillis() - lastTimeAttemptLogin < 30000)
                return;
            lastTimeAttemptLogin = mSystem.currentTimeMillis();
            if (GameCanvas.currentScreen is LoginScr)
                GameCanvas.serverScreen.switchToMe();
            Session_ME.gI().close();
            Session_ME2.gI().close();
            if (GameCanvas.loginScr == null)
                GameCanvas.loginScr = new LoginScr();
            GameCanvas.connect();
            GameCanvas.loginScr.switchToMe();
            Service.gI().login(Rms.loadRMSString("acc"), Rms.loadRMSString("pass"), GameMidlet.VERSION, 0);
            GameCanvas.startWaitDlg();
        }

        static void GotoLastMapAndZone()
        {
            if (TileMap.mapID != lastMapID)
            {
                if (!XmapController.gI.IsActing)
                    XmapController.start(lastMapID);
            }
            else if (TileMap.zoneID != lastZoneID)
                Service.gI().requestChangeZone(lastZoneID, 0);
            else if (Utils.Distance(Char.myCharz().cx, Char.myCharz().cy, lastX, lastY) > 15)
                Utils.TeleportMyChar(lastX, lastY);
            else
            {
                Char.chatPopup = null;
                ChatPopup.currChatPopup = null;
                steps = 0;
            }
        }

        internal static void OnGameScrUpdate()
        {
            if (!isEnabled)
                return;
            if (steps == 0 && GameCanvas.gameTick % (60f * Time.timeScale) == 0f)
            {
                lastMapID = TileMap.mapID;
                lastZoneID = TileMap.zoneID;
                lastX = Char.myCharz().cx;
                lastY = Utils.GetYGround(Char.myCharz().cx);
            }
        }

        internal static void SetState(bool state) => isEnabled = state;
    }
}