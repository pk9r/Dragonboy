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
    public class CustomBackground
    {

        public static bool isEnabled;

        public static Dictionary<string, Image> backgroundWallpapers = new Dictionary<string, Image>();

        public static int inveralChangeBackgroundWallpaper = 30000;
        private static int backgroundIndex;
        private static bool isAllWallpaperLoaded;
        private static long lastTimeChangedWallpaper;

        public static void ShowMenu()
        {
            OpenMenu.start(new(menuItems =>
            {
                if (backgroundWallpapers.Count > 0)
                    menuItems.Add(new("Mở danh\nsách ảnh\nđã lưu", new(() =>
                    {
                        ModMenuPanel.setTypeModMenuMain(2);
                        GameCanvas.panel.show();
                    })));
                menuItems.Add(new("Thêm ảnh\nvào danh sách", new(SelectBackgroundImages)));
                if (backgroundWallpapers.Count > 0)
                    menuItems.Add(new("Xóa hết\nảnh trong\ndanh sách", new(() =>
                    {
                        backgroundWallpapers.Clear();
                        GameScr.info1.addInfo("Đã xóa hết ảnh nền trong danh sách!", 0);
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
                        GameScr.info1.addInfo("Đã xóa ảnh " + selected + "!", 0);
                        setTabCustomBackgroundPanel();
                        SaveData();
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

        public static void SelectBackgroundImages()
        {
            new Thread(delegate ()
            {
                string[] paths = FileDialog.OpenSelectFileDialog("Chọn tệp ảnh để làm ảnh nền", "Tệp ảnh (*.png)|*.png", "png");
                if (paths != null)
                {
                    foreach (string path in paths)
                    {
                        backgroundWallpapers.Add(path, null);
                    }
                    isAllWallpaperLoaded = true;
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        public static void update()
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
                        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                        byte[] imageData = new byte[stream.Length];
                        stream.Read(imageData, 0, imageData.Length);
                        stream.Close();
                        Image image = Utilities.createImage(imageData, Screen.width, Screen.height);
                        backgroundWallpapers[path] = image;
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
        }

        public static void paint(mGraphics g)
        {
            if (!isEnabled || backgroundWallpapers.Count <= 0) return;
            g.drawImage(backgroundWallpapers.ElementAt(backgroundIndex).Value, 0, 0);
            if (mSystem.currentTimeMillis() - lastTimeChangedWallpaper > inveralChangeBackgroundWallpaper)
            {
                lastTimeChangedWallpaper = mSystem.currentTimeMillis();
                backgroundIndex++;
                if (backgroundIndex >= backgroundWallpapers.Count)
                {
                    backgroundIndex = 0;
                }
            }
        }

        public static void paintCustomBackgroundPanel(mGraphics g)
        {
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
            g.setColor(0);
            if (backgroundWallpapers == null || backgroundWallpapers.Count != GameCanvas.panel.currentListLength) return;
            for (int i = 0; i < GameCanvas.panel.currentListLength; i++)
            {
                int num = GameCanvas.panel.xScroll;
                int num2 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int num3 = GameCanvas.panel.wScroll;
                int num4 = GameCanvas.panel.ITEM_HEIGHT - 1;
                g.setColor((i != GameCanvas.panel.selected) ? 15196114 : 16383818);
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
                    if (!string.IsNullOrEmpty(path)) backgroundWallpapers.Add(path, null);
                }
                isAllWallpaperLoaded = true;
            }
            catch (Exception)
            { }
        }

        public static void SaveData()
        {
            string data = string.Join("|", backgroundWallpapers.Keys.ToArray());
            Utilities.saveRMSString("custombackgroundpath", data);
        }

        public static void setState(bool value) => isEnabled = value;

        public static void setState(int value) => inveralChangeBackgroundWallpaper = value * 1000;
    }
}
