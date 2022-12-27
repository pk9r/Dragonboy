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

        public static Dictionary<string, IBackground> backgroundWallpapers = new Dictionary<string, IBackground>();

        public static int inveralChangeBackgroundWallpaper = 30000;
        private static int backgroundIndex;
        private static bool isAllWallpaperLoaded;
        private static long lastTimeChangedWallpaper;
        private static bool isChangeWallpaper = true;
        static int updateGifBackgroundIndex;
        static int ticks;
        public static int threadCount;
        static CustomBackground instance = new CustomBackground();

        public static void ShowMenu()
        {
            OpenMenu.start(new(menuItems =>
            {
                if (backgroundWallpapers.Count > 0)
                    menuItems.Add(new("Mở danh sách nền đã lưu", new(() =>
                    {
                        ModMenuPanel.setTypeModMenuMain(2);
                        GameCanvas.panel.show();
                    })));
                menuItems.Add(new("Thêm nền vào danh sách", new(SelectBackgrounds)));
                if (backgroundWallpapers.Count > 0)
                    menuItems.Add(new("Xóa hết danh sách", new(() =>
                    {
                        backgroundWallpapers.Clear();
                        GameScr.info1.addInfo("Đã xóa hết nền trong danh sách!", 0);
                    })));
                menuItems.Add(new("Tự động chuyển nền: " + (isChangeWallpaper ? "Bật" : "Tắt"), new(() =>
                {
                    isChangeWallpaper = !isChangeWallpaper;
                    lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                    GameScr.info1.addInfo("Đã " + (isChangeWallpaper ? "bật" : "tắt") + " tự động chuyển nền!", 0);
                })));
                menuItems.Add(new("Thay đổi thời gian chuyển nền", new(() =>
                {
                    ChatTextField.gI().strChat = "Nhập thời gian thay đổi nền";
                    ChatTextField.gI().tfChat.name = "Thời gian (giây)";
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                    ChatTextField.gI().startChat2(instance, string.Empty);
                })));
                menuItems.Add(new("Thay đổi tốc độ ảnh động" , new(() =>
                {
                    ChatTextField.gI().strChat = "Nhập tốc độ ảnh động";
                    ChatTextField.gI().tfChat.name = "Tốc độ";
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
                    ChatTextField.gI().startChat2(instance, string.Empty);
                    ChatTextField.gI().tfChat.setText(BackgroundGif.speed.ToString());
                })));
            }));
        }

        public static void setTabCustomBackgroundPanel()
        {
            GameCanvas.panel.ITEM_HEIGHT = 24;
            GameCanvas.panel.currentListLength = backgroundWallpapers.Count;
            GameCanvas.panel.selected = (GameCanvas.isTouch ? (-1) : 0);
            GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
            if (GameCanvas.panel.cmyLim < 0) GameCanvas.panel.cmyLim = 0;
            GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex];
            if (GameCanvas.panel.cmy < 0) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = 0;
            if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim;
        }

        public static void doFireCustomBackgroundListPanel()
        {
            int selected = GameCanvas.panel.selected;
            if (selected < 0) return;
            string fileName = Path.GetFileName(backgroundWallpapers.ElementAt(selected).Key);
            OpenMenu.start(
                menuItemCollection: new(menuItems =>
                {
                    menuItems.Add(new("Xóa", new(() =>
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
                        setTabCustomBackgroundPanel();
                        SaveData();
                    })));
                    if (backgroundIndex != selected)
                        menuItems.Insert(0, new("Chuyển tới nền này", new(() =>
                        {
                            if (backgroundWallpapers.ElementAt(backgroundIndex).Value is BackgroundVideo videoBackground && videoBackground.isPlaying)
                                videoBackground.Stop();
                            backgroundIndex = selected;
                            lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                        })));
                }),
                x: GameCanvas.panel.X,
                y: (selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);

            GameCanvas.panel.cp = new ChatPopup();
            GameCanvas.panel.cp.isClip = false;
            GameCanvas.panel.cp.sayWidth = 180;
            GameCanvas.panel.cp.cx = 3 + GameCanvas.panel.X - ((GameCanvas.panel.X != 0) ? (Res.abs(GameCanvas.panel.cp.sayWidth - GameCanvas.panel.W) + 8) : 0);
            GameCanvas.panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + fileName + "\n--\n|6|Đường dẫn đầy đủ: " + backgroundWallpapers.ElementAt(selected).Key, GameCanvas.panel.cp.sayWidth - 10);
            GameCanvas.panel.cp.delay = 10000000;
            GameCanvas.panel.cp.c = null;
            GameCanvas.panel.cp.sayRun = 7;
            GameCanvas.panel.cp.ch = 15 - GameCanvas.panel.cp.sayRun + GameCanvas.panel.cp.says.Length * 12 + 10;
            if (GameCanvas.panel.cp.ch > GameCanvas.h - 80)
            {
                GameCanvas.panel.cp.ch = GameCanvas.h - 80;
                GameCanvas.panel.cp.lim = GameCanvas.panel.cp.says.Length * 12 - GameCanvas.panel.cp.ch + 17;
                if (GameCanvas.panel.cp.lim < 0)
                {
                    GameCanvas.panel.cp.lim = 0;
                }
                ChatPopup.cmyText = 0;
                GameCanvas.panel.cp.isClip = true;
            }
            GameCanvas.panel.cp.cy = GameCanvas.menu.menuY - GameCanvas.panel.cp.ch;
            while (GameCanvas.panel.cp.cy < 10)
            {
                GameCanvas.panel.cp.cy++;
                GameCanvas.menu.menuY++;
            }
            GameCanvas.panel.cp.mH = 0;
            GameCanvas.panel.cp.strY = 10;
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
                    isAllWallpaperLoaded = true;
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        public static void FixedUpdate()
        {
            if (isAllWallpaperLoaded)
            {
                isAllWallpaperLoaded = false;
                List<string> paths = new List<string>(backgroundWallpapers.Keys);
                for (int i = paths.Count - 1; i >= 0; i--)
                {
                    string path = paths[i];
                    try
                    {
                        if (path.EndsWith(".gif"))
                            backgroundWallpapers[path] = new BackgroundGif(path, Screen.width, Screen.height);
                        else if (path.EndsWith(".mp4"))
                            backgroundWallpapers[path] = new BackgroundVideo(path);
                        else
                            backgroundWallpapers[path] = new BackgroundStatic(path, Screen.width, Screen.height);
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
            if (backgroundWallpapers.ElementAt(updateGifBackgroundIndex).Value is BackgroundGif gif)
            {
                gif.FixedUpdate();
                if (!gif.isFullyLoaded)
                {
                    if (threadCount <= 50)
                    {
                        new Thread(gif.FixedUpdateDifferentThread).Start();
                        threadCount++;
                    }
                }
                else
                    updateGifBackgroundIndex++;
            }
        }

        public static void paint(mGraphics g)
        {
            if (!isEnabled || backgroundWallpapers.Count <= 0) 
                return;
            if (backgroundIndex >= backgroundWallpapers.Count)
                backgroundIndex = 0;
            IBackground background = backgroundWallpapers.ElementAt(backgroundIndex).Value;
            if (background == null)
                return;
            if (background is BackgroundVideo videoBackground && !videoBackground.isPlaying)
                videoBackground.Play();
            background.Paint(g, 0, 0);
            if (isChangeWallpaper && mSystem.currentTimeMillis() - lastTimeChangedWallpaper > inveralChangeBackgroundWallpaper)
            {
                lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                if (background is BackgroundVideo videoBackground1 && videoBackground1.isPlaying)
                    videoBackground1.Stop();    
                backgroundIndex++;
            }
        }

        public static void paintCustomBackgroundPanel(mGraphics g)
        {
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
            g.setColor(0);
            if (backgroundWallpapers.Count != GameCanvas.panel.currentListLength) return;
            for (int i = 0; i < GameCanvas.panel.currentListLength; i++)
            {
                int num = GameCanvas.panel.xScroll;
                int num2 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int num3 = GameCanvas.panel.wScroll;
                int num4 = GameCanvas.panel.ITEM_HEIGHT - 1;
                g.setColor((i != GameCanvas.panel.selected) ? 15196114 : 16383818);
                if (backgroundIndex == i)
                    g.setColor((i != GameCanvas.panel.selected) ? new Color(.5f, 1, 0) : new Color(.375f, .75f, 0));
                g.fillRect(num, num2, num3, num4);
                mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + Path.GetFileName(backgroundWallpapers.ElementAt(i).Key), num + 5, num2, 0);
                mFont.tahoma_7_blue.drawString(g, $"Đường dẫn đầy đủ: {backgroundWallpapers.ElementAt(i).Key}", num + 5, num2 + 11, 0);
            }
            GameCanvas.panel.paintScrollArrow(g);
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
                isAllWallpaperLoaded = true;
                isChangeWallpaper = Utilities.loadRMSBool("ischangewallpaper");
                backgroundIndex = Utilities.loadRMSInt("backgroundindex");
                BackgroundGif.speed = Utilities.loadRMSFloat("gifspeed");
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
            Utilities.saveRMSFloat("gifspeed", BackgroundGif.speed);
        }

        public static void setState(bool value) => isEnabled = value;

        public static void setState(int value) => inveralChangeBackgroundWallpaper = value * 1000;

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
                    if (value == BackgroundGif.speed)
                        return;
                    BackgroundGif.speed = value;
                    GameScr.info1.addInfo($"Thay đổi tốc độ ảnh động thành: {value}!", 0);
                    SaveData();
                    Utilities.ResetTF();
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
                    Utilities.ResetTF();
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
            Utilities.ResetTF();
        }
    }
}
