using Mod.Dialogs;
using Mod.ModHelper.Menu;
using Mod.ModMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;

namespace Mod.Graphics
{
    public class CustomLogo
    {
        public static Dictionary<string, Image> logos = new Dictionary<string, Image>();
        private static bool isLogoLoaded;

        public static bool isEnabled;

        public static int height = 80, logoIndex;

        public static int inveralChangeLogo = 30000;
        private static long lastTimeChangedLogo;

        public static void SelectLogos()
        {
            new Thread(delegate ()
            {
                foreach (string logopath in FileDialog.OpenSelectFileDialog("Chọn logo", "Tệp ảnh (*.png)|*.png", "png"))
                {
                    logos.Add(logopath, null);
                }
                isLogoLoaded = true;
            }).Start();
        }

        public static void update()
        {
            if (isLogoLoaded)
            {
                isLogoLoaded = false;
                List<string> paths = new List<string>(logos.Keys);
                for (int i = paths.Count - 1; i >= 0; i--)
                {
                    string path = paths[i];
                    try
                    {
                        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                        byte[] imageData = new byte[stream.Length];
                        stream.Read(imageData, 0, imageData.Length);
                        stream.Close();
                        Image image = Utilities.createImage(imageData, -1, height);
                        logos[path] = image;
                    }
                    catch (FileNotFoundException)
                    {
                        logos.Remove(path);
                    }
                    catch (IsolatedStorageException)
                    {
                        logos.Remove(path);
                    }
                    catch (Exception) { }
                    lastTimeChangedLogo = mSystem.currentTimeMillis();
                    SaveData();
                }
            }
        }

        public static void ShowMenu()
        {
            OpenMenu.start(new(menuItems =>
            {
                if (logos.Count > 0)
                    menuItems.Add(new("Mở danh sách logo đã lưu", new(() =>
                    {
                        ModMenuPanel.setTypeModMenuMain(3);
                        GameCanvas.panel.show();
                    })));
                menuItems.Add(new("Thêm logo vào danh sách", new(SelectLogos)));
                if (logos.Count > 0)
                    menuItems.Add(new("Xóa hết logo trong danh sách", new(() =>
                    {
                        logos.Clear();
                        GameScr.info1.addInfo("Đã xóa hết logo trong danh sách!", 0);
                    })));
            }));
        }

        public static void setTabCustomLogoPanel()
        {
            GameCanvas.panel.ITEM_HEIGHT = 24;
            GameCanvas.panel.currentListLength = logos.Count;
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
            string fileName = Path.GetFileName(logos.ElementAt(selected).Key);
            OpenMenu.start(
                menuItemCollection: new(menuItems =>
                {
                    menuItems.Add(new("Xóa", new(() =>
                    {
                        logos.Remove(logos.ElementAt(selected).Key);
                        if (selected < logoIndex)
                        {
                            logoIndex--;
                            lastTimeChangedLogo = mSystem.currentTimeMillis();
                        }
                        else if (selected == logoIndex && logos.Count == logoIndex)
                        {
                            logoIndex = 0;
                            lastTimeChangedLogo = mSystem.currentTimeMillis();
                        }
                        GameScr.info1.addInfo("Đã xóa ảnh " + selected + "!", 0);
                        setTabCustomLogoPanel();
                        SaveData();
                    })));
                }),
                x: GameCanvas.panel.X,
                y: (selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);

            GameCanvas.panel.cp = new ChatPopup();
            GameCanvas.panel.cp.isClip = false;
            GameCanvas.panel.cp.sayWidth = 180;
            GameCanvas.panel.cp.cx = 3 + GameCanvas.panel.X - ((GameCanvas.panel.X != 0) ? (Res.abs(GameCanvas.panel.cp.sayWidth - GameCanvas.panel.W) + 8) : 0);
            GameCanvas.panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + fileName + "\n--\n|6|Đường dẫn đầy đủ: " + logos.ElementAt(selected).Key, GameCanvas.panel.cp.sayWidth - 10);
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

        public static void paint(mGraphics g)
        {
            if (!isEnabled || logos.Count <= 0) return;
            g.drawImage(logos.ElementAt(logoIndex).Value, GameCanvas.w / 2, 0, mGraphics.HCENTER);
            if (mSystem.currentTimeMillis() - lastTimeChangedLogo > inveralChangeLogo)
            {
                lastTimeChangedLogo = mSystem.currentTimeMillis();
                logoIndex++;
                if (logoIndex >= logos.Count)
                {
                    logoIndex = 0;
                }
            }
        }

        public static void paintCustomLogoPanel(mGraphics g)
        {
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
            g.setColor(0);
            if (logos == null || logos.Count != GameCanvas.panel.currentListLength) return;
            for (int i = 0; i < GameCanvas.panel.currentListLength; i++)
            {
                int num = GameCanvas.panel.xScroll;
                int num2 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int num3 = GameCanvas.panel.wScroll;
                int num4 = GameCanvas.panel.ITEM_HEIGHT - 1;
                g.setColor((i != GameCanvas.panel.selected) ? 15196114 : 16383818);
                g.fillRect(num, num2, num3, num4);
                mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + Path.GetFileName(logos.ElementAt(i).Key), num + 5, num2, 0);
                mFont.tahoma_7_blue.drawString(g, $"Đường dẫn đầy đủ: {logos.ElementAt(i).Key}", num + 5, num2 + 11, 0);
            }
            GameCanvas.panel.paintScrollArrow(g);
        }


        public static void LoadData()
        {
            try
            {
                foreach (string path in Utilities.loadRMSString("customlogopath").Split('|'))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(path)) logos.Add(path, null);
                    }
                    catch (ArgumentException)
                    {
                        logos[path] = null;
                    }
                }
                isLogoLoaded = true;
            }
            catch (Exception)
            { }
        }

        public static void SaveData()
        {
            string data = string.Join("|", logos.Keys.ToArray());
            Utilities.saveRMSString("customlogopath", data);
        }

        public static void setState(bool value) => isEnabled = value;

        public static void setState(int value) => inveralChangeLogo = value * 1000;

        public static void setLogoHeight(int value) => height = value;
    }
}
