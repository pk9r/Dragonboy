using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mod
{
    /// <summary>
    /// Định nghĩa các sự kiện của game.
    /// </summary>
    /// <remarks>
    /// - Các hàm bool trả về true thì sự kiện game sẽ không được thực hiện, 
    /// trả về false thì sự kiện sẽ được kích hoạt như bình thường.<br/>
    /// - Các hàm void hỗ trợ thực hiện các lệnh cùng với sự kiện.
    /// </remarks>
    public static class GameEvents
    {
        /// <summary>
        /// Kích hoạt khi người chơi chat.
        /// </summary>
        /// <param name="text">Nội dung chat.</param>
        /// <returns></returns>
        public static bool onSendChat(string text)
        {
            bool result = true;
            if (text == "test")
                GameScr.info1.addInfo("Test OK", 0);
            else
                result = false;
            return result;
        }

        /// <summary>
        /// Kích hoạt sau khi game khởi động.
        /// </summary>
        public static bool onGameStarted()
        {
            bool result = false;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
            GameEventHook.InstallAll();
            //TestHook.Install();
            if (onSetResolution())
                result = true;
            onCheckZoomLevel(Screen.width, Screen.height);
            return result;
        }

        /// <summary>
        /// Kích hoạt khi game đóng
        /// </summary>
        public static void onGameClosing()
        {

        }

        public static void onSaveRMSString(ref string filename, ref string data)
        {

        }

        /// <summary>
        /// Kích hoạt khi cài đăt kích thước màn hình.
        /// </summary>
        /// <returns></returns>
        public static bool onSetResolution()
        {
            return Utilities.IsAndroidBuild();
        }

        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr) chưa được xử lý.
        /// </summary>
        public static void onGameScrPressHotkeysUnassigned()
        {

        }

        /// <summary>
        /// Kích hoạt khi nhấn phím tắt (GameScr).
        /// </summary>
        public static void onGameScrPressHotkeys()
        {

        }

        /// <summary>
        /// Kích hoạt sau khi vẽ khung chat.
        /// </summary>
        public static void onPaintChatTextField(ChatTextField instance, mGraphics g)
        {

        }

        /// <summary>
        /// Kích hoạt khi mở khung chat.
        /// </summary>
        public static bool onStartChatTextField(ChatTextField sender, IChatable parentScreen)
        {
            return false;
        }

        public static bool onLoadRMSInt(string file, out int result)
        {
            result = -1;
            return false;
        }

        internal static bool onGetRMSPath(out string result)
        {
            //result = $"{Application.persistentDataPath}\\{GameMidlet.IP}_{GameMidlet.PORT}_x{mGraphics.zoomLevel}\\";
            string subFolder = "TeaMobi";
            // check ip server lậu, lưu rms riêng
            // ...
            result = Application.persistentDataPath;
            if (Utilities.IsLinuxBuild())
                result = Path.Combine(Application.persistentDataPath, Application.companyName, Application.productName);
            if (Utilities.IsAndroidBuild())
                result = Application.persistentDataPath;
            result = Path.Combine(result, subFolder);
            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);
            return true;
        }

        public static bool onTeleportUpdate(Teleport teleport)
        {
            return false;
        }

        /// <summary>
        /// Kích hoạt khi có ChatTextField update.
        /// </summary>
        public static void onUpdateChatTextField(ChatTextField sender)
        {

        }

        public static bool onClearAllRMS()
        {
            foreach (FileInfo file in new DirectoryInfo(Rms.GetiPhoneDocumentsPath() + "/").GetFiles().Where(f => f.Extension != ".log"))
            {
                try
                {
                    if (file.Name != "isPlaySound")
                        file.Delete();
                }
                catch { }
            }
            foreach (DirectoryInfo directory in new DirectoryInfo(Rms.GetiPhoneDocumentsPath() + "/").EnumerateDirectories())
            {
                try
                {
                    directory.Delete(true);
                }
                catch { }
            }
            return true;
        }

        /// <summary>
        /// Kích hoạt khi GameScr.gI() update.
        /// </summary>
        public static void onUpdateGameScr()
        {

        }

        /// <summary>
        /// Kích hoạt khi gửi yêu cầu đăng nhập.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <param name="type"></param>
        public static void onLogin(ref string username, ref string pass, ref sbyte type)
        {

        }

        /// <summary>
        /// Kích hoạt sau khi màn hình chọn server được load.
        /// </summary>
        public static void onServerListScreenLoaded()
        {

        }

        /// <summary>
        /// Kích hoạt khi Session kết nối đến server.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public static void onSessionConnecting(ref string host, ref int port)
        {

        }

        public static void onScreenDownloadDataShow()
        {

        }

        public static bool onCheckZoomLevel(int w, int h)
        {
            if (Utilities.IsAndroidBuild())
            {
                if (w * h >= 2073600)
                    mGraphics.zoomLevel = 4;
                else if (w * h >= 691200)
                    mGraphics.zoomLevel = 3;
                else if (w * h > 153600)
                    mGraphics.zoomLevel = 2;
                else
                    mGraphics.zoomLevel = 1;
            }
            else
            {
                mGraphics.zoomLevel = 2;
                if (w * h < 480000)
                    mGraphics.zoomLevel = 1;
            }
            return true;
        }

        public static bool onKeyPressedz(int keyCode, bool isFromSync)
        {
            return false;
        }

        public static bool onKeyReleasedz(int keyCode, bool isFromAsync)
        {
            return false;
        }

        public static bool onChatPopupMultiLine(string chat)
        {
            return false;
        }

        public static bool onAddBigMessage(string chat, Npc npc)
        {
            return false;
        }

        public static void onInfoMapLoaded()
        {

        }

        public static void onPaintGameScr(mGraphics g)
        {

        }

        public static bool onUseSkill(Char ch)
        {
            return false;
        }

        public static void onFixedUpdateMain()
        {

        }

        public static void onUpdateMain()
        {

        }

        public static void onAddInfoMe(string str)
        {

        }

        public static void onUpdateTouchGameScr()
        {

        }

        public static void onUpdateTouchPanel()
        {

        }

        public static void onSetPointItemMap(int xEnd, int yEnd)
        {

        }

        public static bool onMenuStartAt(MyVector menuItems)
        {
            return false;
        }

        public static void onAddInfoChar(Char c, string info)
        {

        }

        public static bool onPaintBgGameScr(mGraphics g)
        {
            return false;
        }

        public static void onMobStartDie(Mob instance)
        {

        }

        public static void onUpdateMob(Mob instance)
        {

        }

        public static bool onCreateImage(string filename, out Image image)
        {
            string streamingAssetsPath = Application.streamingAssetsPath;
            if (Utilities.IsAndroidBuild())
                streamingAssetsPath = Path.Combine(Application.persistentDataPath, "StreamingAssets");
            string customAssetsPath = Path.Combine(streamingAssetsPath, "CustomAssets");
            image = new Image();
            Texture2D texture2D;
            if (!Directory.Exists(customAssetsPath))
                Directory.CreateDirectory(customAssetsPath);
            if (File.Exists(Path.Combine(customAssetsPath, filename.Replace('/', '\\') + ".png")))
            {
                texture2D = new Texture2D(1, 1);
                texture2D.LoadImage(File.ReadAllBytes(Path.Combine(customAssetsPath, filename.Replace('/', '\\') + ".png")));
            }
            else
                texture2D = Resources.Load<Texture2D>(filename);
            if (texture2D == null)
                throw new Exception("NULL POINTER EXCEPTION AT Image __createImage " + filename);
            image.texture = texture2D;
            image.w = image.texture.width;
            image.h = image.texture.height;
            image.texture.anisoLevel = 0;
            image.texture.filterMode = FilterMode.Point;
            image.texture.mipMapBias = 0f;
            image.texture.wrapMode = TextureWrapMode.Clamp;
            return true;
        }

        public static void onChatVip(string chatVip)
        {

        }

        public static void onUpdateScrollMousePanel(Panel instance, ref int pXYScrollMouse)
        {

        }

        public static void onPanelHide(Panel instance)
        {

        }

        public static void onUpdateKeyPanel(Panel instance)
        {

        }
    }
}
