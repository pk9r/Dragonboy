using Mod.CustomPanel;
using Mod.ModHelper;
using Mod.ModHelper.Menu;
using Mod.ModMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using UnityEngine;
using Mod.R;
using EHVN;

#if UNITY_STANDALONE_WIN
using SFB;
#elif UNITY_ANDROID

#endif

namespace Mod.Graphics
{
    internal class CustomBackground : IChatable
    {
        internal static bool isEnabled;

        internal static Dictionary<string, IImage> backgroundWallpapers = new Dictionary<string, IImage>();

        internal static int intervalChangeBackgroundWallpaper = 30000;
        private static int backgroundIndex;
        private static bool isAllWallpaperLoaded;
        private static long lastTimeChangedWallpaper;
        private static bool isChangeWallpaper = true;
        static float speed = 1f;
        static CustomBackground instance = new CustomBackground();

        internal static void ShowMenu()
        {
            new MenuBuilder()
                .setChatPopup("Loại hình nền được hỗ trợ: ảnh (*.png), ảnh động (*.gif), video (*.mp4).\n" +
                               "Ảnh động và video tiêu tốn nhiều tài nguyên máy, nên cân nhắc trước khi\nsử dụng.")
                .addItem(ifCondition: backgroundWallpapers.Count > 0,
                    "Mở danh sách hình nền đã lưu", new MenuAction(() =>
                        CustomPanelMenu.show(setTabCustomBackgroundPanel,
                            doFireCustomBackgroundListPanel, paintTabHeader, paintCustomBackgroundPanel)))
                .addItem("Thêm hình nền vào danh sách", new MenuAction(SelectBackgrounds))
                .addItem(ifCondition: backgroundWallpapers.Count > 0,
                    "Xóa hết danh sách", new MenuAction(() =>
                    {
                        foreach (BackgroundVideo backgroundVideo in backgroundWallpapers.OfType<BackgroundVideo>())
                            backgroundVideo.Stop();
                        backgroundWallpapers.Clear();
                        GameScr.info1.addInfo("Đã xóa hết hình nền trong danh sách!", 0);
                    }))
                .addItem("Tự động chuyển hình nền: " + Strings.OnOffStatus(isChangeWallpaper), new MenuAction(() => { isChangeWallpaper = !isChangeWallpaper; lastTimeChangedWallpaper = mSystem.currentTimeMillis(); GameScr.info1.addInfo("Đã " + Strings.OnOffStatus(isChangeWallpaper) + " tự động chuyển hình nền!", 0); }))
                .addItem("Thay đổi thời gian chuyển hình nền", new MenuAction(() => { ChatTextField.gI().strChat = "Nhập thời gian thay đổi hình nền"; ChatTextField.gI().tfChat.name = "Thời gian (giây)"; ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC); ChatTextField.gI().startChat2(instance, string.Empty); }))
                .addItem("Thay đổi tốc độ ảnh động", new MenuAction(() => { ChatTextField.gI().strChat = "Nhập tốc độ ảnh động"; ChatTextField.gI().tfChat.name = "Tốc độ"; ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY); ChatTextField.gI().startChat2(instance, string.Empty); ChatTextField.gI().tfChat.setText(speed.ToString()); }))
                .start();
        }

        internal static void setTabCustomBackgroundPanel(Panel panel)
        {
            SetTabPanelTemplates.setTabListTemplate(panel, backgroundWallpapers);
        }

        internal static void doFireCustomBackgroundListPanel(Panel panel)
        {
            int selected = panel.selected;
            if (selected < 0)
                return;
            string fileName = Path.GetFileName(backgroundWallpapers.ElementAt(selected).Key);

            new MenuBuilder()
                .addItem(ifCondition: backgroundIndex != selected,
                    "Chuyển tới hình nền này", new MenuAction(() => { StopAllBackgroundVideo(); backgroundIndex = selected; lastTimeChangedWallpaper = mSystem.currentTimeMillis(); }))
                .addItem("Xóa", new MenuAction(() => { if (backgroundWallpapers.ElementAt(selected).Value is BackgroundVideo videoBackground && videoBackground.isPlaying) videoBackground.Stop(); backgroundWallpapers.Remove(backgroundWallpapers.ElementAt(selected).Key); if (selected < backgroundIndex) { backgroundIndex--; lastTimeChangedWallpaper = mSystem.currentTimeMillis(); } else if (selected == backgroundIndex && backgroundWallpapers.Count == backgroundIndex) { backgroundIndex = 0; lastTimeChangedWallpaper = mSystem.currentTimeMillis(); } GameScr.info1.addInfo("Đã xóa hình nền " + selected + "!", 0); setTabCustomBackgroundPanel(panel); SaveData(); }))
                .setPos(panel.X, (selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll)
                .start();

            panel.cp = new ChatPopup();
            panel.cp.isClip = false;
            panel.cp.sayWidth = 180;
            panel.cp.cx = 3 + panel.X - ((panel.X != 0) ? (Res.abs(panel.cp.sayWidth - panel.W) + 8) : 0);
            panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + fileName + "\n--\n|6|Đường dẫn đầy đủ: " + backgroundWallpapers.ElementAt(selected).Key, panel.cp.sayWidth - 10);
            panel.cp.delay = 10000000;
            panel.cp.c = null;
            panel.cp.sayRun = 7;
            panel.cp.ch = 15 - panel.cp.sayRun + panel.cp.says.Length * 12 + 10;
            if (panel.cp.ch > GameCanvas.h - 80)
            {
                panel.cp.ch = GameCanvas.h - 80;
                panel.cp.lim = panel.cp.says.Length * 12 - panel.cp.ch + 17;
                if (panel.cp.lim < 0)
                {
                    panel.cp.lim = 0;
                }
                ChatPopup.cmyText = 0;
                panel.cp.isClip = true;
            }
            panel.cp.cy = GameCanvas.menu.menuY - panel.cp.ch;
            while (panel.cp.cy < 10)
            {
                panel.cp.cy++;
                GameCanvas.menu.menuY++;
            }
            panel.cp.mH = 0;
            panel.cp.strY = 10;
        }

        internal static void StopAllBackgroundVideo()
        {
            foreach (BackgroundVideo backgroundVideo in backgroundWallpapers.Values.OfType<BackgroundVideo>().Where(v => v.isPlaying))
                backgroundVideo.Stop();
        }

        internal static void SelectBackgrounds()
        {
            string[] paths = null;
            new Thread(delegate ()
            {
#if UNITY_STANDALONE_WIN
                ExtensionFilter[] extensions = new[]
                {
                    new ExtensionFilter("Tệp ảnh/video", "png", "jpg", "jpeg", "gif", "mp4" ),
                    new ExtensionFilter("Tất cả", "*" ),
                };
                paths = StandaloneFileBrowser.OpenFilePanel("Chọn tệp ảnh/video nền", "", extensions, true);
#elif UNITY_ANDROID
                paths = FileChooser.Open(new string[] { "image/*", "video/mp4" });
#endif
                if (paths.Length == 0)
                    return;
                foreach (string path in paths)
                    backgroundWallpapers.Add(path, null);
                isAllWallpaperLoaded = false;
            })
            { IsBackground = true }.Start();
        }

        internal static void FixedUpdate()
        {
            if (!isAllWallpaperLoaded)
            {
                List<string> paths = new List<string>(backgroundWallpapers.Keys);
                for (int i = paths.Count - 1; i >= 0; i--)
                {
                    string path = paths[i];
                    try
                    {
                        if (backgroundWallpapers[path] != null && backgroundWallpapers[path].IsLoaded)
                            continue;
                        if (path.EndsWith(".gif"))
                            backgroundWallpapers[path] = new GifImage(path);
                        else if (path.EndsWith(".mp4"))
                            backgroundWallpapers[path] = new BackgroundVideo(path);
                        else
                            backgroundWallpapers[path] = new StaticImage(path);
                    }
                    catch (FileNotFoundException)
                    {
                        backgroundWallpapers.Remove(path);
                    }
                    catch (IsolatedStorageException)
                    {
                        backgroundWallpapers.Remove(path);
                    }
                    catch (Exception ex) { Debug.LogException(ex); }
                }
                lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                isAllWallpaperLoaded = true;
                SaveData();
            }
        }

        internal static void paint(mGraphics g)
        {
            if (!isEnabled || backgroundWallpapers.Count <= 0)
                return;
            try
            {
                g.setColor(0);
                g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
                if (backgroundIndex >= backgroundWallpapers.Count)
                    backgroundIndex = 0;
                IImage background = backgroundWallpapers.ElementAt(backgroundIndex).Value;
                if (background == null)
                    return;
                if (background is BackgroundVideo backgroundVideo && !backgroundVideo.isPlaying)
                {
                    if (!backgroundVideo.IsLoaded && !backgroundVideo.isPreparing)
                        backgroundVideo.Prepare();
                    backgroundVideo.Play();
                }
                if (background is GifImage gif && gif.speed != speed)
                    gif.speed = speed;
                background.Paint(g, 0, 0);
                if (isChangeWallpaper)
                {
                    if (mSystem.currentTimeMillis() - lastTimeChangedWallpaper > intervalChangeBackgroundWallpaper - 2000)
                    {
                        int index = backgroundIndex + 1;
                        if (index >= backgroundWallpapers.Count)
                            index = 0;
                        if (backgroundWallpapers.ElementAt(index).Value is BackgroundVideo backgroundVideo1 && !backgroundVideo1.isPreparing && !backgroundVideo1.IsLoaded)
                            backgroundVideo1.Prepare();
                    }
                    if (mSystem.currentTimeMillis() - lastTimeChangedWallpaper > intervalChangeBackgroundWallpaper)
                    {
                        lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                        if (background is BackgroundVideo backgroundVideo1 && backgroundVideo1.isPlaying)
                            backgroundVideo1.Stop();
                        backgroundIndex++;
                    }
                }
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        internal static void paintCustomBackgroundPanel(Panel panel, mGraphics g)
        {
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
            g.setColor(0);
            if (backgroundWallpapers.Count != GameCanvas.panel.currentListLength) 
                return;
            for (int i = 0; i < GameCanvas.panel.currentListLength; i++)
            {
                int num = GameCanvas.panel.xScroll;
                int num2 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int num3 = GameCanvas.panel.wScroll;
                int num4 = GameCanvas.panel.ITEM_HEIGHT - 1;
                if (backgroundIndex == i)
                    g.setColor((i != GameCanvas.panel.selected) ? new Color(.5f, 1, 0) : new Color(.375f, .75f, 0));
                else 
                    g.setColor((i != GameCanvas.panel.selected) ? 0xE7DFD2 : 0xF9FF4A);
                g.fillRect(num, num2, num3, num4);
                mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + Path.GetFileName(backgroundWallpapers.ElementAt(i).Key), num + 5, num2, 0);
                mFont.tahoma_7_blue.drawString(g, $"Đường dẫn đầy đủ: {backgroundWallpapers.ElementAt(i).Key}", num + 5, num2 + 11, 0);
            }
            GameCanvas.panel.paintScrollArrow(g);
        }

        internal static void paintTabHeader(Panel panel, mGraphics g)
        {
            PaintPanelTemplates.paintTabHeaderTemplate(panel, g, "Danh sách hình nền tùy chỉnh");
        }

        internal static void LoadData()
        {
            try
            {
                foreach (string path in Utils.LoadDataString("custombackgroundpath").Split('|'))
                {
                    if (!string.IsNullOrEmpty(path))
                        backgroundWallpapers.Add(path, null);
                }
                isAllWallpaperLoaded = false;
                Utils.TryLoadDataBool("ischangewallpaper", out isChangeWallpaper);
                Utils.TryLoadDataInt("backgroundindex", out backgroundIndex);
                Utils.TryLoadDataFloat("gifbackgroundspeed", out float gifbackgroundspeed);
                speed = Mathf.Clamp(gifbackgroundspeed, 0, 100);
                if (backgroundIndex >= backgroundWallpapers.Count)
                    backgroundIndex = 0;
            }
            catch (Exception)
            { }
        }

        internal static void SaveData()
        {
            string data = string.Join("|", backgroundWallpapers.Keys.ToArray());
            Utils.SaveData("custombackgroundpath", data);
            Utils.SaveData("ischangewallpaper", isChangeWallpaper);
            Utils.SaveData("backgroundindex", backgroundIndex);
            Utils.SaveData("gifbackgroundspeed", speed);
        }

        internal static void setState(bool value)
        {
            isEnabled = value;
            if (!value)
                foreach (BackgroundVideo backgroundVideo in backgroundWallpapers.Values.Where((background) => background is BackgroundVideo))
                    backgroundVideo.Stop();
        }

        internal static void setState(int value) => intervalChangeBackgroundWallpaper = value * 1000;

        public void onChatFromMe(string text, string to)
        {
            if (ChatTextField.gI().tfChat.name == "Tốc độ")
            {
                if (string.IsNullOrEmpty(text))
                {
                    GameCanvas.startOKDlg("Bạn chưa nhập số!");
                    return;
                }
                try
                {
                    float value = float.Parse(text);
                    if (value > 10f || value < 0.1f)
                    {
                        GameCanvas.startOKDlg("Số đã nhập phải trong khoảng 0.1 và 10!");
                        return;
                    }
                    if (value == speed)
                        return;
                    speed = value;
                    GameScr.info1.addInfo($"Thay đổi tốc độ ảnh động thành: {value}!", 0);
                    SaveData();
                    ChatTextField.gI().ResetTF();
                }
                catch (FormatException)
                {
                    GameCanvas.startOKDlg("Số đã nhập không hợp lệ!");
                }
                catch (OverflowException)
                {
                    GameCanvas.startOKDlg("Số đã nhập quá lớn, quá nhỏ hoặc quá nhiều số thập phân!");
                }
            }
            else if (ChatTextField.gI().tfChat.name == "Thời gian (giây)")
            {
                if (string.IsNullOrEmpty(text))
                {
                    GameCanvas.startOKDlg("Bạn chưa nhập số!");
                    return;
                }
                try
                {
                    int value = int.Parse(text);
                    if (value < 10)
                    {
                        GameCanvas.startOKDlg("Số đã nhập phải lớn hơn hoặc bằng 10!");
                        return;
                    }
                    ModMenuMain.GetModMenuItem<ModMenuItemValues>("Set_TimeChangeBg").SelectedValue = value;
                    GameScr.info1.addInfo($"Thay đổi thời gian chuyển hình nền thành: {text} giây!", 0);
                    SaveData();
                    ChatTextField.gI().ResetTF();
                }
                catch (FormatException)
                {
                    GameCanvas.startOKDlg("Số đã nhập không hợp lệ!");
                }
                catch (OverflowException)
                {
                    GameCanvas.startOKDlg("Số đã nhập quá lớn hoặc quá nhỏ!");
                }
            }
        }

        public void onCancelChat() => ChatTextField.gI().ResetTF();
    }
}
