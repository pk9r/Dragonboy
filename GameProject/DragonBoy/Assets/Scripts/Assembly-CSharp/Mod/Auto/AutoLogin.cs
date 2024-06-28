using Mod.R;

namespace Mod.Auto
{
    internal class AutoLogin
    {
        internal static bool isEnabled;
        static bool is1stTimeLogin = true;
        static long lastTimeAttemptLogin;

        internal static void Update()
        {
            if (!isEnabled)
                return;
            if (is1stTimeLogin)
                return;
            if (GameCanvas.currentScreen != GameCanvas.serverScreen && GameCanvas.currentScreen != GameCanvas.serverScr && GameCanvas.currentScreen != GameCanvas.loginScr)
                return;
            if (mSystem.currentTimeMillis() - lastTimeAttemptLogin < 30000)
                return;
            lastTimeAttemptLogin = mSystem.currentTimeMillis();
            if (GameCanvas.loginScr == null)
                GameCanvas.loginScr = new LoginScr();
            GameCanvas.connect();
            GameCanvas.loginScr.switchToMe();
            Service.gI().login(Rms.loadRMSString("acc"), Rms.loadRMSString("pass"), GameMidlet.VERSION, 0);
            GameCanvas.startWaitDlg();
        }

        internal static void OnServerListScreenLoaded()
        {
            if (!isEnabled)
                return;
            if (is1stTimeLogin)
                return;
            lastTimeAttemptLogin = mSystem.currentTimeMillis();
            GameCanvas.startOKDlg(string.Format(Strings.autoLoginReattemptLoginIn, "30s") + '!');
        }

        internal static void OnGameScrUpdate()
        {
            if (is1stTimeLogin)
            {
                is1stTimeLogin = false;
                lastTimeAttemptLogin = mSystem.currentTimeMillis();
            }
        }

        internal static void SetState(bool state) => isEnabled = state;
    }
}