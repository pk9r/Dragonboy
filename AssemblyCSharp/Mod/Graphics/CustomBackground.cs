using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Mod.Dialogs;
using System.IO;
using System.Threading;
using System.IO.IsolatedStorage;
using Mod.ModMenu;

namespace Mod.Graphics
{
    public class CustomBackground
    {
        static CustomBackground _Instance;

        public static bool isEnabled;

        public static Dictionary<string, Image> staticBackgroundWallpapers = new Dictionary<string, Image>();

        public static Dictionary<string, Gif> gifBackgroundWallpapers = new Dictionary<string, Gif>();

        public static int inveralChangeBackgroundWallpaper = 30000;

        static int backgroundIndex;

        static bool isAllWallpaperLoaded;

        static long lastTimeChangedWallpaper;

        public static CustomBackground getInstance()
        {
            if (_Instance == null) _Instance = new CustomBackground();
            return _Instance;
        }

        public static void ShowMenu()
        {
            //MyVector myVector = new MyVector();
            //if (backgroundWallpapers.Count > 0) 
            //    myVector.addElement(new Command("Mở danh\nsách ảnh\nđã lưu", getInstance(), 1, null));

            //myVector.addElement(new Command("Thêm ảnh\nvào danh sách", getInstance(), 2, null));

            //if (backgroundWallpapers.Count > 0) 
            //    myVector.addElement(new Command("Xóa hết\nảnh trong\ndanh sách", getInstance(), 3, null));
            //GameCanvas.menu.startAt(myVector, 0);

            var pairs = new List<KeyValuePair<string, Action<int, string, string[]>>>();
            if (staticBackgroundWallpapers.Count + gifBackgroundWallpapers.Count > 0)
                pairs.Add(new("Mở danh\nsách ảnh\nđã lưu", (_, _, _) =>
                    {
                        ModMenuPanel.setTypeModMenuMain(2);
                        GameCanvas.panel.show();
                    }));
            pairs.Add(new("Thêm ảnh\nvào danh sách", (_, _, _) => SelectBackgroundImages()));
            if (staticBackgroundWallpapers.Count + gifBackgroundWallpapers.Count > 0)
                pairs.Add(new("Xóa hết\nảnh trong\ndanh sách", (_, _, _) =>
                    {
                        staticBackgroundWallpapers.Clear();
                        gifBackgroundWallpapers.Clear();
                        GameScr.info1.addInfo("Đã xóa hết ảnh nền trong danh sách!", 0);
                    }));

            Utilities.openMenu(pairs);
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
            //MyVector myVector = new MyVector();
            //myVector.addElement(new Command("Xóa", getInstance(), 4, selected));
            //GameCanvas.menu.startAt(myVector, GameCanvas.panel.X, (selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);
            string fileName;
            if (selected < staticBackgroundWallpapers.Count)
                fileName = Path.GetFileName(staticBackgroundWallpapers.ElementAt(selected).Key);
            else
                fileName = Path.GetFileName(gifBackgroundWallpapers.ElementAt(selected - staticBackgroundWallpapers.Count).Key);
            Utilities.openMenu(
                pairs: new()
                {
                    new("Xóa", (_, _, _) =>
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
                    })
                },
                x: GameCanvas.panel.X,
                y: (selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);

            GameCanvas.panel.cp = new ChatPopup();
            GameCanvas.panel.cp.isClip = false;
            GameCanvas.panel.cp.sayWidth = 180;
            GameCanvas.panel.cp.cx = 3 + GameCanvas.panel.X - ((GameCanvas.panel.X != 0) ? (Res.abs(GameCanvas.panel.cp.sayWidth - GameCanvas.panel.W) + 8) : 0);
            GameCanvas.panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + fileName + "\n--\n|6|Đường dẫn đầy đủ: " + (selected < staticBackgroundWallpapers.Count ? staticBackgroundWallpapers.ElementAt(selected).Key : gifBackgroundWallpapers.ElementAt(selected).Key), GameCanvas.panel.cp.sayWidth - 10);
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

        public static void update()
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
                            gifBackgroundWallpapers[path] = new Gif(path);
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
                        {

                        }
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
        }

        public static void paint(mGraphics g)
        {
            if (!isEnabled || staticBackgroundWallpapers.Count + gifBackgroundWallpapers.Count <= 0) return;
            if (backgroundIndex < staticBackgroundWallpapers.Count)
                g.drawImage(staticBackgroundWallpapers.ElementAt(backgroundIndex).Value, 0, 0);
            else
                gifBackgroundWallpapers.ElementAt(backgroundIndex - staticBackgroundWallpapers.Count).Value.Paint(g, 0, 0);
            if (mSystem.currentTimeMillis() - lastTimeChangedWallpaper > inveralChangeBackgroundWallpaper)
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

        //public void perform(int idAction, object p)
        //{
        //    switch (idAction)
        //    {
        //        case 1:
        //            ModMenuPanel.setTypeModMenuMain(2);
        //            GameCanvas.panel.show();
        //            break;
        //        case 2:
        //            SelectBackgroundImages();
        //            break;
        //        case 3:
        //            backgroundWallpapers.Clear();
        //            GameScr.info1.addInfo("Đã xóa hết ảnh nền trong danh sách!", 0);
        //            break;
        //        case 4:
        //            int index = (int)p;
        //            backgroundWallpapers.Remove(backgroundWallpapers.ElementAt(index).Key);
        //            if (index < backgroundIndex)
        //            {
        //                backgroundIndex--;
        //                lastTimeChangedWallpaper = mSystem.currentTimeMillis();
        //            }
        //            else if (index == backgroundIndex && backgroundWallpapers.Count == backgroundIndex)
        //            {
        //                backgroundIndex = 0;
        //                lastTimeChangedWallpaper = mSystem.currentTimeMillis();
        //            }
        //            GameScr.info1.addInfo("Đã xóa ảnh " + index + "!", 0);
        //            setTabCustomBackgroundPanel();
        //            SaveData();
        //            break;
        //    }
        //}

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
            }
            catch (Exception)
            { }
        }

        public static void SaveData()
        {
            string data = string.Join("|", staticBackgroundWallpapers.Keys.ToArray());
            data += string.Join("|", gifBackgroundWallpapers.Keys.ToArray());
            Utilities.saveRMSString("custombackgroundpath", data);
        }

        public static void setState(bool value) => isEnabled = value;

        public static void setState(int value) => inveralChangeBackgroundWallpaper = value * 1000;
    }
}
