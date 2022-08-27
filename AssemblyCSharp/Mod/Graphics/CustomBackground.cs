using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Mod.Dialogs;
using System.IO;
using System.Threading;

namespace Mod.Graphics
{
    public class CustomBackground : IActionListener
    {
        static CustomBackground _Instance;

        public static bool isEnabled;

        public static List<Image> listBackgroundImages = new List<Image>();

        public static int inveralDrawImages = 30000;

        static int imageIndex;

        static bool isGotPaths;

        static long lastTimeDrawAnImage;

        static List<string> listImagePaths = new List<string>();

        public const int TYPE_CUSTOM_BACKGROUND = 28;

        public static CustomBackground getInstance()
        {
            if (_Instance == null) _Instance = new CustomBackground();
            return _Instance;
        }

        public static void ShowMenu()
        {
            MyVector myVector = new MyVector();
            if (listImagePaths.Count > 0) myVector.addElement(new Command("Mở danh\nsách ảnh\nđã lưu", getInstance(), 1, null));
            myVector.addElement(new Command("Thêm ảnh\nvào danh sách", getInstance(), 2, null));
            if (listImagePaths.Count > 0) myVector.addElement(new Command("Xóa hết\nảnh trong\ndanh sách", getInstance(), 3, null));
            GameCanvas.menu.startAt(myVector, 0);
        }

        static void setTypeCustomBackgroundPanel()
        {
            GameCanvas.panel.type = TYPE_CUSTOM_BACKGROUND;
            GameCanvas.panel.setType(0);
            SoundMn.gI().getSoundOption();
            setTabCustomBackgroundPanel();
        }

        public static void setTabCustomBackgroundPanel()
        {
            GameCanvas.panel.ITEM_HEIGHT = 24;
            GameCanvas.panel.currentListLength = listImagePaths.Count;
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
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Xóa", getInstance(), 4, selected));
            string fileName = Path.GetFileName(listImagePaths[selected]);
            GameCanvas.menu.startAt(myVector, GameCanvas.panel.X, (selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);
            GameCanvas.panel.cp = new ChatPopup();
            GameCanvas.panel.cp.isClip = false;
            GameCanvas.panel.cp.sayWidth = 180;
            GameCanvas.panel.cp.cx = 3 + GameCanvas.panel.X - ((GameCanvas.panel.X != 0) ? (Res.abs(GameCanvas.panel.cp.sayWidth - GameCanvas.panel.W) + 8) : 0);
            GameCanvas.panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + fileName + "\n--\n|6|Đường dẫn đầy đủ: " + listImagePaths[selected], GameCanvas.panel.cp.sayWidth - 10);
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

        public static void selectImages()
        {
            new Thread(delegate ()
            {
                string[] paths = FileDialog.OpenSelectFileDialog("Chọn tệp ảnh để làm ảnh nền", "Tệp ảnh (*.png)|*.png", "png");
                if (paths != null) 
                {
                    listImagePaths.AddRange(paths);
                    isGotPaths = true;
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        public static void update()
        {
            if (isGotPaths)
            {
                isGotPaths = false;
                foreach (string path in listImagePaths)
                {
                    try
                    {
                        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                        byte[] imageData = new byte[stream.Length];
                        stream.Read(imageData, 0, imageData.Length);
                        stream.Close();
                        Image image = Utilities.createImage(imageData, Screen.width, Screen.height);
                        listBackgroundImages.Add(image);
                    }
                    catch (Exception)
                    { }
                }
                lastTimeDrawAnImage = mSystem.currentTimeMillis();
            }
        }

        public static void paint(mGraphics g)
        {
            if (!isEnabled || listBackgroundImages.Count <= 0) return;
            g.drawImage(listBackgroundImages[imageIndex], 0, 0);
            if (mSystem.currentTimeMillis() - lastTimeDrawAnImage > inveralDrawImages)
            {
                lastTimeDrawAnImage = mSystem.currentTimeMillis();
                imageIndex++;
                if (imageIndex >= listBackgroundImages.Count)
                {
                    imageIndex = 0;
                }
            }
        }

        public static void paintCustomBackgroundPanel(mGraphics g)
        {
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
            g.setColor(0);
            if (listImagePaths == null || listImagePaths.Count != GameCanvas.panel.currentListLength) return;
            for (int i = 0; i < GameCanvas.panel.currentListLength; i++)
            {
                int num = GameCanvas.panel.xScroll;
                int num2 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int num3 = GameCanvas.panel.wScroll;
                int num4 = GameCanvas.panel.ITEM_HEIGHT - 1;
                g.setColor((i != GameCanvas.panel.selected) ? 15196114 : 16383818);
                g.fillRect(num, num2, num3, num4);
                mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + Path.GetFileName(listImagePaths[i]), num + 5, num2, 0);
                mFont.tahoma_7_blue.drawString(g, $"Đường dẫn đầy đủ: {listImagePaths[i]}", num + 5, num2 + 11, 0);
            }
            GameCanvas.panel.paintScrollArrow(g);
        }

        public void perform(int idAction, object p)
        {
            switch (idAction)
            {
                case 1:
                    setTypeCustomBackgroundPanel();
                    GameCanvas.panel.show();
                    break;
                case 2:
                    selectImages();
                    break;
                case 3:
                    listImagePaths.Clear();
                    listBackgroundImages.Clear();
                    GameScr.info1.addInfo("Đã xóa hết danh sách ảnh nền tùy chỉnh!", 0);
                    break;
                case 4:
                    int index = (int)p;
                    listImagePaths.RemoveAt(index);
                    listBackgroundImages.RemoveAt(index);
                    if (index < imageIndex)
                    {
                        imageIndex--;
                        lastTimeDrawAnImage = mSystem.currentTimeMillis();
                    }
                    else if (index == imageIndex && listBackgroundImages.Count == imageIndex)
                    {
                        imageIndex = 0;
                        lastTimeDrawAnImage = mSystem.currentTimeMillis();
                    }
                    GameScr.info1.addInfo("Đã xóa ảnh " + index + "!", 0);
                    setTabCustomBackgroundPanel();
                    SaveData();
                    break;
            }
        }

        public static void LoadData()
        {
            try
            {
                listImagePaths = Utilities.loadRMSString("custombackgroundpath").Split('|').ToList();
                isGotPaths = true;
            }
            catch (Exception)
            { }
        }

        public static void SaveData()
        {
            string data = string.Join("|", listImagePaths.ToArray());
            Utilities.saveRMSString("custombackgroundpath", data);
        }
    }
}
