using Mod.ModHelper.Menu;
using Mod.ModMenu;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using Mod.Dialogs;

namespace Mod.Graphics
{
    internal class CustomCursor : IChatable
    {
        public static bool isEnabled;

        public static Dictionary<string, IImage> customCursors = new Dictionary<string, IImage>();

        private static bool isAllCustomCursorLoaded;
        static long lastTimeChangeFrame;
        static int cursorIndex;
        static int updateGifCursorIndex;
        static int ticks;
        static float speed = 1f;
        static CustomCursor instance = new CustomCursor();

        public static void ShowMenu()
        {
            OpenMenu.start(new(menuItems =>
            {
                if (customCursors.Count > 0)
                    menuItems.Add(new("Mở danh sách con trỏ đã lưu", new(() =>
                    {
                        ModMenuPanel.setTypeModMenuMain(4);
                        GameCanvas.panel.show();
                    })));
                menuItems.Add(new("Thêm con trỏ vào danh sách", new(SelectBackgrounds)));
                if (customCursors.Count > 0)
                    menuItems.Add(new("Xóa hết danh sách", new(() =>
                    {
                        customCursors.Clear();
                        GameScr.info1.addInfo("Đã xóa hết con trỏ trong danh sách!", 0);
                    })));
                menuItems.Add(new("Thay đổi tốc độ ảnh động", new(() =>
                {
                    ChatTextField.gI().strChat = "Nhập tốc độ ảnh động";
                    ChatTextField.gI().tfChat.name = "Tốc độ";
                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
                    ChatTextField.gI().startChat2(instance, string.Empty);
                    ChatTextField.gI().tfChat.setText(speed.ToString());
                })));
            }), "Loại con trỏ chuột được hỗ trợ: ảnh (*.png), ảnh động (*.gif).");
        }

        public static void setTabCustomCursorPanel()
        {
            GameCanvas.panel.ITEM_HEIGHT = 24;
            GameCanvas.panel.currentListLength = customCursors.Count;
            GameCanvas.panel.selected = (GameCanvas.isTouch ? (-1) : 0);
            GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
            if (GameCanvas.panel.cmyLim < 0) GameCanvas.panel.cmyLim = 0;
            GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex];
            if (GameCanvas.panel.cmy < 0) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = 0;
            if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim;
        }

        public static void doFireCustomCursorListPanel()
        {
            int selected = GameCanvas.panel.selected;
            if (selected < 0) return;
            string fileName = Path.GetFileName(customCursors.ElementAt(selected).Key);
            OpenMenu.start(
                menuItemCollection: new(menuItems =>
                {
                    menuItems.Add(new("Xóa", new(() =>
                    {
                        customCursors.Remove(customCursors.ElementAt(selected).Key);
                        GameScr.info1.addInfo("Đã xóa con trỏ " + selected + "!", 0);
                        setTabCustomCursorPanel();
                        SaveData();
                    })));
                    if (cursorIndex != selected)
                        menuItems.Insert(0, new("Chuyển tới con trỏ này", new(() =>
                        {
                            cursorIndex = selected;
                            if (!(customCursors.ElementAt(cursorIndex).Value is GifImage))
                                Cursor.SetCursor(customCursors.ElementAt(cursorIndex).Value.Textures[0], Vector2.zero, CursorMode.Auto);
                        })));
                }),
                x: GameCanvas.panel.X,
                y: (selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);

            GameCanvas.panel.cp = new ChatPopup();
            GameCanvas.panel.cp.isClip = false;
            GameCanvas.panel.cp.sayWidth = 180;
            GameCanvas.panel.cp.cx = 3 + GameCanvas.panel.X - ((GameCanvas.panel.X != 0) ? (Res.abs(GameCanvas.panel.cp.sayWidth - GameCanvas.panel.W) + 8) : 0);
            GameCanvas.panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + fileName + "\n--\n|6|Đường dẫn đầy đủ: " + customCursors.ElementAt(selected).Key, GameCanvas.panel.cp.sayWidth - 10);
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
                string[] paths = FileDialog.OpenSelectFileDialog("Chọn tệp để làm con trỏ", "Tệp ảnh (*.png)|*.png|Tệp ảnh động (*.gif)|*.gif|Tất cả|*.*", "");
                if (paths != null)
                {
                    foreach (string path in paths)
                        customCursors.Add(path, null);
                    isAllCustomCursorLoaded = false;
                }
            })
            {
                IsBackground = true,
                Name = "OpenSelectCursorFileDialog"
            }.Start();
        }

        public static void Update()
        {
            if (!isEnabled || customCursors.Count <= 0)
                return;
            if (!isAllCustomCursorLoaded)
            {
                isAllCustomCursorLoaded = true;
                List<string> paths = new List<string>(customCursors.Keys);
                for (int i = paths.Count - 1; i >= 0; i--)
                {
                    string path = paths[i];
                    try
                    {
                        if (path.EndsWith(".gif"))
                            customCursors[path] = new GifImage(path);
                        else
                            customCursors[path] = new StaticImage(path);
                    }
                    catch (FileNotFoundException)
                    {
                        customCursors.Remove(path);
                    }
                    catch (IsolatedStorageException)
                    {
                        customCursors.Remove(path);
                    }
                    catch (Exception)
                    { }
                }
                SaveData();
                if (customCursors.ElementAt(cursorIndex).Value is StaticImage image)
                    Cursor.SetCursor(image.Textures[0], Vector2.zero, CursorMode.Auto);
            }
            if (customCursors.ElementAt(cursorIndex).Value is GifImage gif && gif.isFullyLoaded && mSystem.currentTimeMillis() - lastTimeChangeFrame > gif.delays[gif.paintFrameIndex] * 1000f / speed)
            {
                lastTimeChangeFrame = mSystem.currentTimeMillis();
                gif.paintFrameIndex++;
                if (gif.paintFrameIndex >= gif.Textures.Length)
                    gif.paintFrameIndex = 0;
                Cursor.SetCursor(gif.Textures[gif.paintFrameIndex], Vector2.zero, CursorMode.Auto);
            }
            ticks++;
            if (ticks > 50)
                ticks = 0;
            if (ticks % 5 != 0)
                return;
            if (updateGifCursorIndex >= customCursors.Count)
                return;
            if (customCursors.ElementAt(updateGifCursorIndex).Value is GifImage gif2)
            {
                gif2.FixedUpdate();
                if (!gif2.isFullyLoaded)
                    new Thread(gif2.LoadFrameGif) { IsBackground = true, Name = "LoadFrameGifCursor" }.Start();
                else
                    updateGifCursorIndex++;
            }
            else
                updateGifCursorIndex++;
        }

        public static void paintCustomCursorPanel(mGraphics g)
        {
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
            g.setColor(0);
            if (customCursors.Count != GameCanvas.panel.currentListLength) return;
            for (int i = 0; i < GameCanvas.panel.currentListLength; i++)
            {
                int num = GameCanvas.panel.xScroll;
                int num2 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int num3 = GameCanvas.panel.wScroll;
                int num4 = GameCanvas.panel.ITEM_HEIGHT - 1;
                g.setColor((i != GameCanvas.panel.selected) ? 15196114 : 16383818);
                if (cursorIndex == i)
                    g.setColor((i != GameCanvas.panel.selected) ? new Color(.5f, 1, 0) : new Color(.375f, .75f, 0));
                g.fillRect(num, num2, num3, num4);
                mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + Path.GetFileName(customCursors.ElementAt(i).Key), num + 5, num2, 0);
                mFont.tahoma_7_blue.drawString(g, $"Đường dẫn đầy đủ: {customCursors.ElementAt(i).Key}", num + 5, num2 + 11, 0);
            }
            GameCanvas.panel.paintScrollArrow(g);
        }

        public static void LoadData()
        {
            try
            {
                foreach (string path in Utilities.loadRMSString("customcursorpath").Split('|'))
                {
                    if (!string.IsNullOrEmpty(path))
                        customCursors.Add(path, null);
                }
                isAllCustomCursorLoaded = false;
                cursorIndex = Utilities.loadRMSInt("cursorindex");
                if (cursorIndex >= customCursors.Count)
                    cursorIndex = 0;
            }
            catch (Exception)
            { }
        }

        public static void SaveData()
        {
            string data = string.Join("|", customCursors.Keys.ToArray());
            Utilities.saveRMSString("customcursorpath", data);
            Utilities.saveRMSInt("cursorindex", cursorIndex);
        }

        public static void setState(bool value)
        {
            isEnabled = value;
            if (!value)
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            else if (!(customCursors.ElementAt(cursorIndex).Value is GifImage))
                Cursor.SetCursor(customCursors.ElementAt(cursorIndex).Value.Textures[0], Vector2.zero, CursorMode.Auto);
        }

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
