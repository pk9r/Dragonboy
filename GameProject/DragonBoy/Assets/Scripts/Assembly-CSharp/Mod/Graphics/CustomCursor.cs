//using Mod.ModHelper.Menu;
//using System;
//using System.Collections.Generic;
//using System.IO.IsolatedStorage;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using UnityEngine;
//using Mod.Dialogs;
//using Mod.CustomPanel;

//namespace Mod.Graphics
//{
//    public class CustomCursor : IChatable
//    {
//        public static bool isEnabled;

//        public static Dictionary<string, IImage> customCursors = new Dictionary<string, IImage>();

//        private static bool isAllCustomCursorLoaded;
//        static long lastTimeChangeFrame;
//        static int cursorIndex;
//        static int updateGifCursorIndex;
//        static int ticks;
//        static float speed = 1f;
//        static CustomCursor instance = new CustomCursor();

//        public static void ShowMenu()
//        {
//            new MenuBuilder()
//                .setChatPopup("Loại con trỏ chuột được hỗ trợ: ảnh (*.png), ảnh động (*.gif).")
//                .addItem(ifCondition: customCursors.Count > 0,
//                    "Mở danh sách con trỏ đã lưu", new(() =>
//                    {
//                        CustomPanelMenu.show(setTabCustomCursorPanel, doFireCustomCursorListPanel, paintTabHeader, paintCustomCursorPanel);
//                    }))
//                .addItem("Thêm con trỏ vào danh sách", new(SelectBackgrounds))
//                .addItem(ifCondition: customCursors.Count > 0,
//                    "Xóa hết danh sách", new(() =>
//                    {
//                        customCursors.Clear();
//                        GameScr.info1.addInfo("Đã xóa hết con trỏ trong danh sách!", 0);
//                    }))
//                .addItem("Thay đổi tốc độ ảnh động", new(() =>
//                {
//                    ChatTextField.gI().strChat = "Nhập tốc độ ảnh động";
//                    ChatTextField.gI().tfChat.name = "Tốc độ";
//                    ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
//                    ChatTextField.gI().startChat2(instance, string.Empty);
//                    ChatTextField.gI().tfChat.setText(speed.ToString());
//                }))
//                .start();
            
//            //OpenMenu.start(new(menuItems =>
//            //{
//            //    if (customCursors.Count > 0)
//            //        menuItems.Add(new("Mở danh sách con trỏ đã lưu", new(() =>
//            //        {
//            //            CustomPanelMenu.CreateCustomPanelMenu(setTabCustomCursorPanel, doFireCustomCursorListPanel, paintTabHeader, paintCustomCursorPanel);
//            //        })));
//            //    menuItems.Add(new("Thêm con trỏ vào danh sách", new(SelectBackgrounds)));
//            //    if (customCursors.Count > 0)
//            //        menuItems.Add(new("Xóa hết danh sách", new(() =>
//            //        {
//            //            customCursors.Clear();
//            //            GameScr.info1.addInfo("Đã xóa hết con trỏ trong danh sách!", 0);
//            //        })));
//            //    menuItems.Add(new("Thay đổi tốc độ ảnh động", new(() =>
//            //    {
//            //        ChatTextField.gI().strChat = "Nhập tốc độ ảnh động";
//            //        ChatTextField.gI().tfChat.name = "Tốc độ";
//            //        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
//            //        ChatTextField.gI().startChat2(instance, string.Empty);
//            //        ChatTextField.gI().tfChat.setText(speed.ToString());
//            //    })));
//            //}), "Loại con trỏ chuột được hỗ trợ: ảnh (*.png), ảnh động (*.gif).");
//        }

//        private static void paintTabHeader(Panel panel, mGraphics g)
//        {
//            PaintPanelTemplates.paintTabHeaderTemplate(panel, g, "Danh sách con trỏ chuột tùy chỉnh");
//        }

//        public static void setTabCustomCursorPanel(Panel panel)
//        {
//            SetTabPanelTemplates.setTabListTemplate(panel, customCursors);
//        }

//        public static void doFireCustomCursorListPanel(Panel panel)
//        {
//            int selected = panel.selected;
//            if (selected < 0) return;
//            string fileName = Path.GetFileName(customCursors.ElementAt(selected).Key);

//            new MenuBuilder()
//                .addItem(ifCondition: cursorIndex != selected,
//                    "Chuyển tới con trỏ này", new(() =>
//                    {
//                        cursorIndex = selected;
//                        if (!(customCursors.ElementAt(cursorIndex).Value is GifImage))
//                            Cursor.SetCursor(customCursors.ElementAt(cursorIndex).Value.Textures[0], Vector2.zero, CursorMode.Auto);
//                    }))
//                .addItem("Xóa", new(() =>
//                {
//                    customCursors.Remove(customCursors.ElementAt(selected).Key);
//                    GameScr.info1.addInfo("Đã xóa con trỏ " + selected + "!", 0);
//                    setTabCustomCursorPanel(panel);
//                    SaveData();
//                }))
//                .setPos(panel.X, (selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll)
//                .start();

//            //OpenMenu.start(
//            //    menuItemCollection: new(menuItems =>
//            //    {
//            //        menuItems.Add(new("Xóa", new(() =>
//            //        {
//            //            customCursors.Remove(customCursors.ElementAt(selected).Key);
//            //            GameScr.info1.addInfo("Đã xóa con trỏ " + selected + "!", 0);
//            //            setTabCustomCursorPanel();
//            //            SaveData();
//            //        })));
//            //        if (cursorIndex != selected)
//            //            menuItems.Insert(0, new("Chuyển tới con trỏ này", new(() =>
//            //            {
//            //                cursorIndex = selected;
//            //                if (!(customCursors.ElementAt(cursorIndex).Value is GifImage))
//            //                    Cursor.SetCursor(customCursors.ElementAt(cursorIndex).Value.Textures[0], Vector2.zero, CursorMode.Auto);
//            //            })));
//            //    }),
//            //    x: panel.X,
//            //    y: (selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll);

//            panel.cp = new ChatPopup();
//            panel.cp.isClip = false;
//            panel.cp.sayWidth = 180;
//            panel.cp.cx = 3 + panel.X - ((panel.X != 0) ? (Res.abs(panel.cp.sayWidth - panel.W) + 8) : 0);
//            panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + fileName + "\n--\n|6|Đường dẫn đầy đủ: " + customCursors.ElementAt(selected).Key, panel.cp.sayWidth - 10);
//            panel.cp.delay = 10000000;
//            panel.cp.c = null;
//            panel.cp.sayRun = 7;
//            panel.cp.ch = 15 - panel.cp.sayRun + panel.cp.says.Length * 12 + 10;
//            if (panel.cp.ch > GameCanvas.h - 80)
//            {
//                panel.cp.ch = GameCanvas.h - 80;
//                panel.cp.lim = panel.cp.says.Length * 12 - panel.cp.ch + 17;
//                if (panel.cp.lim < 0)
//                {
//                    panel.cp.lim = 0;
//                }
//                ChatPopup.cmyText = 0;
//                panel.cp.isClip = true;
//            }
//            panel.cp.cy = GameCanvas.menu.menuY - panel.cp.ch;
//            while (panel.cp.cy < 10)
//            {
//                panel.cp.cy++;
//                GameCanvas.menu.menuY++;
//            }
//            panel.cp.mH = 0;
//            panel.cp.strY = 10;
//        }

//        public static void SelectBackgrounds()
//        {
//            new Thread(delegate ()
//            {
//                string[] paths = FileDialog.OpenSelectFileDialog("Chọn tệp để làm con trỏ", "Tệp ảnh (*.png)|*.png|Tệp ảnh động (*.gif)|*.gif|Tất cả|*.*", "");
//                if (paths != null)
//                {
//                    foreach (string path in paths)
//                        customCursors.Add(path, null);
//                    isAllCustomCursorLoaded = false;
//                }
//            })
//            {
//                IsBackground = true
//            }.Start();
//        }

//        public static void Update()
//        {
//            if (!isEnabled || customCursors.Count <= 0)
//                return;
//            if (!isAllCustomCursorLoaded)
//            {
//                isAllCustomCursorLoaded = true;
//                List<string> paths = new List<string>(customCursors.Keys);
//                for (int i = paths.Count - 1; i >= 0; i--)
//                {
//                    string path = paths[i];
//                    try
//                    {
//                        if (path.EndsWith(".gif"))
//                            customCursors[path] = new GifImage(path);
//                        else
//                            customCursors[path] = new StaticImage(path);
//                    }
//                    catch (FileNotFoundException)
//                    {
//                        customCursors.Remove(path);
//                    }
//                    catch (IsolatedStorageException)
//                    {
//                        customCursors.Remove(path);
//                    }
//                    catch (Exception)
//                    { }
//                }
//                SaveData();
//                if (customCursors.ElementAt(cursorIndex).Value is StaticImage image)
//                    Cursor.SetCursor(image.Textures[0], Vector2.zero, CursorMode.Auto);
//            }
//            if (customCursors.ElementAt(cursorIndex).Value is GifImage gif && gif.isFullyLoaded && mSystem.currentTimeMillis() - lastTimeChangeFrame > gif.delays[gif.paintFrameIndex] * 1000f / speed)
//            {
//                lastTimeChangeFrame = mSystem.currentTimeMillis();
//                gif.paintFrameIndex++;
//                if (gif.paintFrameIndex >= gif.Textures.Length)
//                    gif.paintFrameIndex = 0;
//                Cursor.SetCursor(gif.Textures[gif.paintFrameIndex], Vector2.zero, CursorMode.Auto);
//            }
//            ticks++;
//            if (ticks > 50)
//                ticks = 0;
//            if (ticks % 5 != 0)
//                return;
//            if (updateGifCursorIndex >= customCursors.Count)
//                return;
//            if (customCursors.ElementAt(updateGifCursorIndex).Value is GifImage gif2)
//            {
//                gif2.FixedUpdate();
//                if (!gif2.isFullyLoaded)
//                    new Thread(gif2.LoadFrameGif) { IsBackground = true, Name = "LoadFrameGifCursor" }.Start();
//                else
//                    updateGifCursorIndex++;
//            }
//            else
//                updateGifCursorIndex++;
//        }

//        public static void paintCustomCursorPanel(Panel panel, mGraphics g)
//        {
//            PaintPanelTemplates.paintCollectionCaptionAndDescriptionTemplate(panel, g, customCursors,
//                w => Path.GetFileName(w.Key), w => $"Đường dẫn đầy đủ: {w.Key}");
//        }

//        public static void LoadData()
//        {
//            try
//            {
//                foreach (string path in Utils.loadRMSString("customcursorpath").Split('|'))
//                {
//                    if (!string.IsNullOrEmpty(path))
//                        customCursors.Add(path, null);
//                }
//                isAllCustomCursorLoaded = false;
//                cursorIndex = Utils.loadRMSInt("cursorindex");
//                if (cursorIndex >= customCursors.Count)
//                    cursorIndex = 0;
//            }
//            catch (Exception)
//            { }
//        }

//        public static void SaveData()
//        {
//            string data = string.Join("|", customCursors.Keys.ToArray());
//            Utils.saveRMSString("customcursorpath", data);
//            Utils.saveRMSInt("cursorindex", cursorIndex);
//        }

//        public static void setState(bool value)
//        {
//            isEnabled = value;
//            if (!value)
//                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
//            else if (!(customCursors.ElementAt(cursorIndex).Value is GifImage))
//                Cursor.SetCursor(customCursors.ElementAt(cursorIndex).Value.Textures[0], Vector2.zero, CursorMode.Auto);
//        }

//        public void onChatFromMe(string text, string to)
//        {
//            if (ChatTextField.gI().tfChat.name == "Tốc độ")
//            {
//                if (string.IsNullOrEmpty(text))
//                {
//                    GameCanvas.startOKDlg("Bạn chưa nhập số!");
//                    return;
//                }
//                try
//                {
//                    float value = float.Parse(text);
//                    if (value > 10f || value < 0.1f)
//                    {
//                        GameCanvas.startOKDlg("Số đã nhập phải trong khoảng 0.1 và 10!");
//                        return;
//                    }
//                    if (value == speed)
//                        return;
//                    speed = value;
//                    GameScr.info1.addInfo($"Thay đổi tốc độ ảnh động thành: {value}!", 0);
//                    SaveData();
//                    ChatTextField.gI().ResetTF();
//                }
//                catch (FormatException)
//                {
//                    GameCanvas.startOKDlg("Số đã nhập không hợp lệ!");
//                }
//                catch (OverflowException)
//                {
//                    GameCanvas.startOKDlg("Số đã nhập quá lớn, quá nhỏ hoặc quá nhiều số thập phân!");
//                }
//            }
//        }

//        public void onCancelChat()
//        {
//            ChatTextField.gI().ResetTF();
//        }
//    }
//}
