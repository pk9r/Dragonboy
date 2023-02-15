using Mod.CustomPanel;
using Mod.Dialogs;
using Mod.ModHelper.Menu;
using Mod.ModMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Mod.Graphics
{
    public class CustomBackground : IChatable
    {

        public static bool isEnabled;

        public static Dictionary<string, IImage> backgroundWallpapers = new Dictionary<string, IImage>();

        public static int intervalChangeBackgroundWallpaper = 30000;
        private static int backgroundIndex;
        private static bool isAllWallpaperLoaded;
        private static long lastTimeChangedWallpaper;
        private static bool isChangeWallpaper = true;
        static int updateGifBackgroundIndex;
        static int ticks;
        static float speed;
        static CustomBackground instance = new CustomBackground();

        public static void ShowMenu()
        {
            new MenuBuilder()
                .setChatPopup("Loại background được hỗ trợ: ảnh (*.png), ảnh động (*.gif), video (*.mp4).\n" +
                               "Ảnh động và video tiêu tốn nhiều tài nguyên máy, nên cân nhắc trước khi\nsử dụng.")
                .addItem(ifCondition: backgroundWallpapers.Count > 0,
                    "Mở danh sách nền đã lưu", new(() =>
                        CustomPanelMenu.show(setTabCustomBackgroundPanel, 
                            doFireCustomBackgroundListPanel, paintTabHeader, paintCustomBackgroundPanel)))
                .addItem("Thêm nền vào danh sách", new(SelectBackgrounds))
                .addItem(ifCondition: backgroundWallpapers.Count > 0,
                    "Xóa hết danh sách", new(() =>
                    {
                        backgroundWallpapers.Clear();
                        GameScr.info1.addInfo("Đã xóa hết nền trong danh sách!", 0);
                    }))
                .addItem("Tự động chuyển nền: " + (isChangeWallpaper ? "Bật" : "Tắt"), new(() =>
                {
                    isChangeWallpaper = !isChangeWallpaper;
                    lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                    GameScr.info1.addInfo("Đã " + (isChangeWallpaper ? "bật" : "tắt") + " tự động chuyển nền!", 0);
                }))
                .addItem("Thay đổi thời gian chuyển nền", new(() =>
                {
                    ChatTextField.gI().strChat = "Nhập thời gian thay đổi nền";
                    ChatTextField.gI().tfChat.name = "Thời gian (giây)";
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                    ChatTextField.gI().startChat2(instance, string.Empty);
                }))
                .addItem("Thay đổi tốc độ ảnh động", new(() =>
                {
                    ChatTextField.gI().strChat = "Nhập tốc độ ảnh động";
                    ChatTextField.gI().tfChat.name = "Tốc độ";
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
                    ChatTextField.gI().startChat2(instance, string.Empty);
                    ChatTextField.gI().tfChat.setText(speed.ToString());
                }))
                .start();
            //OpenMenu.start(new(menuItems =>
            //{
            //    if (backgroundWallpapers.Count > 0)
            //        menuItems.Add(new("Mở danh sách nền đã lưu", new(() =>
            //        {
            //            CustomPanelMenu.CreateCustomPanelMenu(setTabCustomBackgroundPanel, doFireCustomBackgroundListPanel, paintTabHeader, paintCustomBackgroundPanel);
            //        })));
            //    menuItems.Add(new("Thêm nền vào danh sách", new(SelectBackgrounds)));
            //    if (backgroundWallpapers.Count > 0)
            //        menuItems.Add(new("Xóa hết danh sách", new(() =>
            //        {
            //            backgroundWallpapers.Clear();
            //            GameScr.info1.addInfo("Đã xóa hết nền trong danh sách!", 0);
            //        })));
            //    menuItems.Add(new("Tự động chuyển nền: " + (isChangeWallpaper ? "Bật" : "Tắt"), new(() =>
            //    {
            //        isChangeWallpaper = !isChangeWallpaper;
            //        lastTimeChangedWallpaper = mSystem.currentTimeMillis();
            //        GameScr.info1.addInfo("Đã " + (isChangeWallpaper ? "bật" : "tắt") + " tự động chuyển nền!", 0);
            //    })));
            //    menuItems.Add(new("Thay đổi thời gian chuyển nền", new(() =>
            //    {
            //        ChatTextField.gI().strChat = "Nhập thời gian thay đổi nền";
            //        ChatTextField.gI().tfChat.name = "Thời gian (giây)";
            //        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
            //        ChatTextField.gI().startChat2(instance, string.Empty);
            //    })));
            //    menuItems.Add(new("Thay đổi tốc độ ảnh động", new(() =>
            //    {
            //        ChatTextField.gI().strChat = "Nhập tốc độ ảnh động";
            //        ChatTextField.gI().tfChat.name = "Tốc độ";
            //        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
            //        ChatTextField.gI().startChat2(instance, string.Empty);
            //        ChatTextField.gI().tfChat.setText(speed.ToString());
            //    })));
            //}), "Loại background được hỗ trợ: ảnh (*.png), ảnh động (*.gif), video (*.mp4).\nẢnh động và video tiêu tốn nhiều tài nguyên máy, nên cân nhắc trước khi\nsử dụng.");
        }

        public static void setTabCustomBackgroundPanel(Panel panel)
        {
            SetTabPanelTemplates.setTabListTemplate(panel, backgroundWallpapers);
        }

        public static void doFireCustomBackgroundListPanel(Panel panel)
        {
            int selected = panel.selected;
            if (selected < 0) return;
            string fileName = Path.GetFileName(backgroundWallpapers.ElementAt(selected).Key);

            new MenuBuilder()
                .addItem(ifCondition: backgroundIndex != selected,
                    "Chuyển tới nền này", new(() =>
                    {
                        if (backgroundWallpapers.ElementAt(backgroundIndex).Value is BackgroundVideo videoBackground && videoBackground.isPlaying)
                            videoBackground.Stop();
                        backgroundIndex = selected;
                        lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                    }))
                .addItem("Xóa", new(() =>
                {
                    if (backgroundWallpapers.ElementAt(selected).Value is BackgroundVideo videoBackground && videoBackground.isPlaying)
                        videoBackground.Stop();
                    backgroundWallpapers.Remove(backgroundWallpapers.ElementAt(selected).Key);
                    if (selected < backgroundIndex)
                    {
                        backgroundIndex--;
                        lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                    }
                    else if (selected == backgroundIndex && backgroundWallpapers.Count == backgroundIndex)
                    {
                        backgroundIndex = 0;
                        lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                    }
                    GameScr.info1.addInfo("Đã xóa nền " + selected + "!", 0);
                    setTabCustomBackgroundPanel(panel);
                    SaveData();
                }))
                .setPos(panel.X, (selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll)
                .start();

            //OpenMenu.start(
            //    menuItemCollection: new(menuItems =>
            //    {
            //        menuItems.Add(new("Xóa", new(() =>
            //        {
            //            if (backgroundWallpapers.ElementAt(selected).Value is BackgroundVideo videoBackground && videoBackground.isPlaying)
            //                videoBackground.Stop();
            //            backgroundWallpapers.Remove(backgroundWallpapers.ElementAt(selected).Key);
            //            if (selected < backgroundIndex)
            //            {
            //                backgroundIndex--;
            //                lastTimeChangedWallpaper = mSystem.currentTimeMillis();
            //            }
            //            else if (selected == backgroundIndex && backgroundWallpapers.Count == backgroundIndex)
            //            {
            //                backgroundIndex = 0;
            //                lastTimeChangedWallpaper = mSystem.currentTimeMillis();
            //            }
            //            GameScr.info1.addInfo("Đã xóa nền " + selected + "!", 0);
            //            setTabCustomBackgroundPanel();
            //            SaveData();
            //        })));
            //        if (backgroundIndex != selected)
            //            menuItems.Insert(0, new("Chuyển tới nền này", new(() =>
            //            {
            //                if (backgroundWallpapers.ElementAt(backgroundIndex).Value is BackgroundVideo videoBackground && videoBackground.isPlaying)
            //                    videoBackground.Stop();
            //                backgroundIndex = selected;
            //                lastTimeChangedWallpaper = mSystem.currentTimeMillis();
            //            })));
            //    }),
            //    x: panel.X,
            //    y: (selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll);

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

        public static void SelectBackgrounds()
        {
            new Thread(delegate ()
            {
                string[] paths = FileDialog.OpenSelectFileDialog("Chọn tệp để làm nền", "Tệp ảnh (*.png)|*.png|Tệp ảnh động (*.gif)|*.gif|Tệp Video (*.mp4)|*.mp4|Tất cả|*.*", "");
                if (paths != null)
                {
                    foreach (string path in paths)
                        backgroundWallpapers.Add(path, null);
                    isAllWallpaperLoaded = false;
                }
            })
            {
                IsBackground = true,
                Name = "OpenSelectBackgroundFileDialog"
            }.Start();
        }

        public static void FixedUpdate()
        {
            if (!isAllWallpaperLoaded)
            {
                isAllWallpaperLoaded = true;
                List<string> paths = new List<string>(backgroundWallpapers.Keys);
                for (int i = paths.Count - 1; i >= 0; i--)
                {
                    string path = paths[i];
                    try
                    {
                        if (path.EndsWith(".gif"))
                            backgroundWallpapers[path] = new GifImage(path, Screen.width, Screen.height);
                        else if (path.EndsWith(".mp4"))
                            backgroundWallpapers[path] = new BackgroundVideo(path);
                        else
                            backgroundWallpapers[path] = new StaticImage(path, Screen.width, Screen.height);
                    }
                    catch (FileNotFoundException)
                    {
                        backgroundWallpapers.Remove(path);
                    }
                    catch (IsolatedStorageException)
                    {
                        backgroundWallpapers.Remove(path);
                    }
                    catch (Exception)
                    { }
                }
                lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                SaveData();
            }
            ticks++;
            if (ticks > 50)
                ticks = 0;
            if (ticks % 5 != 0)
                return;
            if (updateGifBackgroundIndex >= backgroundWallpapers.Count)
                return;
            if (backgroundWallpapers.ElementAt(updateGifBackgroundIndex).Value is GifImage gif)
            {
                gif.FixedUpdate();
                if (!gif.isFullyLoaded)
                {
                    new Thread(gif.LoadFrameGif) { IsBackground = true, Name = "LoadFrameGif" }.Start();
                }
                else
                    updateGifBackgroundIndex++;
            }
            else
                updateGifBackgroundIndex++;
        }

        public static void paint(mGraphics g)
        {
            if (!isEnabled || backgroundWallpapers.Count <= 0)
                return;
            if (backgroundIndex >= backgroundWallpapers.Count)
                backgroundIndex = 0;
            IImage background = backgroundWallpapers.ElementAt(backgroundIndex).Value;
            if (background == null)
                return;
            if (background is BackgroundVideo backgroundVideo && !backgroundVideo.isPlaying)
            {
                if (!backgroundVideo.isPrepared)
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
                    if (backgroundWallpapers.ElementAt(index).Value is BackgroundVideo backgroundVideo1 && !backgroundVideo1.isPreparing && !backgroundVideo1.isPrepared)
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

        public static void paintCustomBackgroundPanel(Panel panel, mGraphics g)
        {
            PaintPanelTemplates.paintCollectionCaptionAndDescriptionTemplate(panel, g, backgroundWallpapers,
                w => Path.GetFileName(w.Key), w => $"Đường dẫn đầy đủ: {w.Key}");
        }

        public static void paintTabHeader(Panel panel, mGraphics g)
        {
            PaintPanelTemplates.paintTabHeaderTemplate(panel, g, "Danh sách nền tùy chỉnh");
        }

        public static void LoadData()
        {
            try
            {
                foreach (string path in Utilities.loadRMSString("custombackgroundpath").Split('|'))
                {
                    if (!string.IsNullOrEmpty(path))
                        backgroundWallpapers.Add(path, null);
                }
                isAllWallpaperLoaded = false;
                isChangeWallpaper = Utilities.loadRMSBool("ischangewallpaper");
                backgroundIndex = Utilities.loadRMSInt("backgroundindex");
                speed = Utilities.loadRMSFloat("gifbackgroundspeed");
                if (backgroundIndex >= backgroundWallpapers.Count)
                    backgroundIndex = 0;
            }
            catch (Exception)
            { }
        }

        public static void SaveData()
        {
            string data = string.Join("|", backgroundWallpapers.Keys.ToArray());
            Utilities.saveRMSString("custombackgroundpath", data);
            Utilities.saveRMSBool("ischangewallpaper", isChangeWallpaper);
            Utilities.saveRMSInt("backgroundindex", backgroundIndex);
            Utilities.saveRMSFloat("gifbackgroundspeed", speed);
        }

        public static void setState(bool value)
        {
            isEnabled = value;
            if (!value)
                foreach (BackgroundVideo backgroundVideo in backgroundWallpapers.Values.Where((background) => background is BackgroundVideo))
                    backgroundVideo.Stop();
        }

        public static void setState(int value) => intervalChangeBackgroundWallpaper = value * 1000;

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
                    ModMenuMain.modMenuItemInts[6].setValue(value);
                    GameScr.info1.addInfo($"Thay đổi thời gian chuyển nền thành: {text} giây!", 0);
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

        public void onCancelChat()
        {
            ChatTextField.gI().ResetTF();
        }
    }
}
