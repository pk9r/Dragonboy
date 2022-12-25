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

        public static Dictionary<string, Image> staticBackgroundWallpapers = new Dictionary<string, Image>();

        public static Dictionary<string, Gif> gifBackgroundWallpapers = new Dictionary<string, Gif>();

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
                if (staticBackgroundWallpapers.Count + gifBackgroundWallpapers.Count > 0)
                    menuItems.Add(new("Mở danh\nsách ảnh\nđã lưu", new(() =>
                    {
                        ModMenuPanel.setTypeModMenuMain(2);
                        GameCanvas.panel.show();
                    })));
                menuItems.Add(new("Thêm ảnh\nvào danh sách", new(SelectBackgroundImages)));
                if (staticBackgroundWallpapers.Count + gifBackgroundWallpapers.Count > 0)
                    menuItems.Add(new("Xóa hết\nảnh trong\ndanh sách", new(() =>
                    {
                        staticBackgroundWallpapers.Clear();
                        gifBackgroundWallpapers.Clear();
                        GameScr.info1.addInfo("Đã xóa hết ảnh nền trong danh sách!", 0);
                    })));
                menuItems.Add(new("Tự động chuyển ảnh nền: " + (isChangeWallpaper ? "Bật" : "Tắt"), new(() =>
                {
                    isChangeWallpaper = !isChangeWallpaper;
                    lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                    GameScr.info1.addInfo("Đã " + (isChangeWallpaper ? "bật" : "tắt") + " tự động chuyển ảnh nền!", 0);
                })));
                menuItems.Add(new("Thay đổi tốc độ ảnh động" , new(() =>
                {
                    ChatTextField.gI().strChat = "Nhập tốc độ ảnh động";
                    ChatTextField.gI().tfChat.name = "Tốc độ";
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
                    ChatTextField.gI().startChat2(instance, string.Empty);
                    ChatTextField.gI().tfChat.setText(Gif.speed.ToString());
                })));
            }));
        }

        public static void setTabCustomBackgroundPanel()
        {
            GameCanvas.panel.ITEM_HEIGHT = 24;
            GameCanvas.panel.currentListLength = staticBackgroundWallpapers.Count + gifBackgroundWallpapers.Count;
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
            string fileName;
            if (selected < staticBackgroundWallpapers.Count)
                fileName = Path.GetFileName(staticBackgroundWallpapers.ElementAt(selected).Key);
            else
                fileName = Path.GetFileName(gifBackgroundWallpapers.ElementAt(selected - staticBackgroundWallpapers.Count).Key);
            OpenMenu.start(
                menuItemCollection: new(menuItems =>
                {
                    menuItems.Add(new("Xóa", new(() =>
                    {
                        if (selected < staticBackgroundWallpapers.Count)
                            staticBackgroundWallpapers.Remove(staticBackgroundWallpapers.ElementAt(selected).Key);
                        else
                            gifBackgroundWallpapers.Remove(gifBackgroundWallpapers.ElementAt(selected - staticBackgroundWallpapers.Count).Key);
                        if (selected < backgroundIndex)
                        {
                            backgroundIndex--;
                            lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                        }
                        else if (selected == backgroundIndex && staticBackgroundWallpapers.Count + gifBackgroundWallpapers.Count == backgroundIndex)
                        {
                            backgroundIndex = 0;
                            lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                        }
                        GameScr.info1.addInfo("Đã xóa ảnh " + selected + "!", 0);
                        setTabCustomBackgroundPanel();
                        SaveData();
                    })));
                    if (backgroundIndex != selected)
                        menuItems.Insert(0, new("Chuyển tới ảnh này", new(() =>
                        {
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
            GameCanvas.panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + fileName + "\n--\n|6|Đường dẫn đầy đủ: " + (selected < staticBackgroundWallpapers.Count ? staticBackgroundWallpapers.ElementAt(selected).Key : gifBackgroundWallpapers.ElementAt(selected - staticBackgroundWallpapers.Count).Key), GameCanvas.panel.cp.sayWidth - 10);
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

        public static void SelectBackgroundImages()
        {
            new Thread(delegate ()
            {
                string[] paths = FileDialog.OpenSelectFileDialog("Chọn tệp ảnh để làm ảnh nền", "Tệp ảnh (*.png)|*.png|Tệp ảnh động (*.gif)|*.gif", "png");
                if (paths != null)
                {
                    foreach (string path in paths)
                    {
                        if (path.EndsWith("gif"))
                            gifBackgroundWallpapers.Add(path, null);
                        else
                            staticBackgroundWallpapers.Add(path, null);
                    }
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
                List<string> paths = new List<string>(staticBackgroundWallpapers.Keys);
                paths.AddRange(new List<string>(gifBackgroundWallpapers.Keys));
                for (int i = paths.Count - 1; i >= 0; i--)
                {
                    string path = paths[i];
                    if (path.EndsWith(".gif"))
                    {
                        try
                        {
                            gifBackgroundWallpapers[path] = new Gif(path, Screen.width, Screen.height);
                        }
                        catch (FileNotFoundException)
                        {
                            gifBackgroundWallpapers.Remove(path);
                        }
                        catch (IsolatedStorageException)
                        {
                            gifBackgroundWallpapers.Remove(path);
                        }
                        catch (Exception)
                        { }
                    }
                    else
                    {
                        try
                        {
                            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                            byte[] imageData = new byte[stream.Length];
                            stream.Read(imageData, 0, imageData.Length);
                            stream.Close();
                            Image image = Utilities.createImage(imageData, Screen.width, Screen.height);
                            staticBackgroundWallpapers[path] = image;
                        }
                        catch (FileNotFoundException)
                        {
                            staticBackgroundWallpapers.Remove(path);
                        }
                        catch (IsolatedStorageException)
                        {
                            staticBackgroundWallpapers.Remove(path);
                        }
                        catch (Exception)
                        { }
                    }
                }
                lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                SaveData();
            }
            ticks++;
            if (ticks > 50)
                ticks = 0;
            if (ticks % 5 != 0)
                return;
            if (updateGifBackgroundIndex >= gifBackgroundWallpapers.Count)
                return;
            Gif gif = gifBackgroundWallpapers.ElementAt(updateGifBackgroundIndex).Value;
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

        public static void paint(mGraphics g)
        {
            if (!isEnabled || staticBackgroundWallpapers.Count + gifBackgroundWallpapers.Count <= 0) return;
            if (backgroundIndex < staticBackgroundWallpapers.Count)
                g.drawImage(staticBackgroundWallpapers.ElementAt(backgroundIndex).Value, 0, 0);
            else
                gifBackgroundWallpapers.ElementAt(backgroundIndex - staticBackgroundWallpapers.Count).Value.Paint(g, 0, 0);
            if (isChangeWallpaper && mSystem.currentTimeMillis() - lastTimeChangedWallpaper > inveralChangeBackgroundWallpaper)
            {
                lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                backgroundIndex++;
                if (backgroundIndex >= staticBackgroundWallpapers.Count + gifBackgroundWallpapers.Count)
                    backgroundIndex = 0;
            }
        }

        public static void paintCustomBackgroundPanel(mGraphics g)
        {
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
            g.setColor(0);
            if (staticBackgroundWallpapers.Count + gifBackgroundWallpapers.Count != GameCanvas.panel.currentListLength) return;
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
                if (i < staticBackgroundWallpapers.Count)
                {
                    mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + Path.GetFileName(staticBackgroundWallpapers.ElementAt(i).Key), num + 5, num2, 0);
                    mFont.tahoma_7_blue.drawString(g, $"Đường dẫn đầy đủ: {staticBackgroundWallpapers.ElementAt(i).Key}", num + 5, num2 + 11, 0);
                }
                else
                {
                    mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + Path.GetFileName(gifBackgroundWallpapers.ElementAt(i - staticBackgroundWallpapers.Count).Key), num + 5, num2, 0);
                    mFont.tahoma_7_blue.drawString(g, $"Đường dẫn đầy đủ: {gifBackgroundWallpapers.ElementAt(i - staticBackgroundWallpapers.Count).Key}", num + 5, num2 + 11, 0);
                }
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
                    {
                        if (path.EndsWith("gif"))
                            gifBackgroundWallpapers.Add(path, null);
                        else
                            staticBackgroundWallpapers.Add(path, null);
                    }
                }
                isAllWallpaperLoaded = true;
                isChangeWallpaper = Utilities.loadRMSBool("ischangewallpaper");
                backgroundIndex = Utilities.loadRMSInt("backgroundindex");
                Gif.speed = Utilities.loadRMSFloat("gifspeed");
                if (backgroundIndex >= staticBackgroundWallpapers.Count + gifBackgroundWallpapers.Count)
                    backgroundIndex = 0;
            }
            catch (Exception)
            { }
        }

        public static void SaveData()
        {
            string data = string.Join("|", staticBackgroundWallpapers.Keys.ToArray());
            data += "|" + string.Join("|", gifBackgroundWallpapers.Keys.ToArray());
            Utilities.saveRMSString("custombackgroundpath", data);
            Utilities.saveRMSBool("ischangewallpaper", isChangeWallpaper);
            Utilities.saveRMSInt("backgroundindex", backgroundIndex);
            Utilities.saveRMSFloat("gifspeed", Gif.speed);
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
                    if (value == Gif.speed)
                        return;
                    Gif.speed = value;
                    GameScr.info1.addInfo($"Thay đổi tốc độ ảnh động thành: {text}!", 0);
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
        }

        public void onCancelChat()
        {
            Utilities.ResetTF();
        }
    }
}
