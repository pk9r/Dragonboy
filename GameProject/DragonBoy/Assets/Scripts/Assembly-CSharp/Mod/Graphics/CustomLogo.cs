//using Mod.CustomPanel;
//using Mod.Dialogs;
//using Mod.ModHelper.Menu;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.IsolatedStorage;
//using System.Linq;
//using System.Threading;

//namespace Mod.Graphics
//{
//    public class CustomLogo
//    {
//        public static Dictionary<string, Image> logos = new Dictionary<string, Image>();
//        private static bool isLogoLoaded;

//        public static bool isEnabled;

//        public static int height = 80, logoIndex;

//        public static int inveralChangeLogo = 30000;
//        private static long lastTimeChangedLogo;

//        public static void SelectLogos()
//        {
//            new Thread(delegate ()
//            {
//                foreach (string logopath in FileDialog.OpenSelectFileDialog("Chọn logo", "Tệp ảnh (*.png)|*.png", "png"))
//                {
//                    logos.Add(logopath, null);
//                }
//                isLogoLoaded = true;
//            })
//            {
//                IsBackground = true,
//                Name = "OpenSelectLogoFileDialog"
//            }.Start();
//        }

//        public static void update()
//        {
//            if (isLogoLoaded)
//            {
//                isLogoLoaded = false;
//                List<string> paths = new List<string>(logos.Keys);
//                for (int i = paths.Count - 1; i >= 0; i--)
//                {
//                    string path = paths[i];
//                    try
//                    {
//                        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
//                        byte[] imageData = new byte[stream.Length];
//                        stream.Read(imageData, 0, imageData.Length);
//                        stream.Close();
//                        Image image = Utils.createImage(imageData, -1, height);
//                        logos[path] = image;
//                    }
//                    catch (FileNotFoundException)
//                    {
//                        logos.Remove(path);
//                    }
//                    catch (IsolatedStorageException)
//                    {
//                        logos.Remove(path);
//                    }
//                    catch (Exception) { }
//                    lastTimeChangedLogo = mSystem.currentTimeMillis();
//                    SaveData();
//                }
//            }
//        }

//        public static void ShowMenu()
//        {
//            new MenuBuilder()
//                .addItem(ifCondition: logos.Count > 0,
//                    "Mở danh sách logo đã lưu", new(() =>
//                    {
//                        CustomPanelMenu.show(setTabCustomLogoPanel, doFireCustomLogoListPanel, paintTabHeader, paintCustomLogoPanel);
//                    }))
//                .addItem("Thêm logo vào danh sách", new(SelectLogos))
//                .addItem(ifCondition: logos.Count > 0,
//                    "Xóa hết logo trong danh sách", new(() =>
//                    {
//                        logos.Clear();
//                        GameScr.info1.addInfo("Đã xóa hết logo trong danh sách!", 0);
//                    }))
//                .start();

//            //OpenMenu.start(new(menuItems =>
//            //{
//            //    if (logos.Count > 0)
//            //        menuItems.Add(new("Mở danh sách logo đã lưu", new(() =>
//            //        {
//            //            CustomPanelMenu.CreateCustomPanelMenu(setTabCustomLogoPanel, doFireCustomLogoListPanel, paintTabHeader, paintCustomLogoPanel);
//            //        })));
//            //    menuItems.Add(new("Thêm logo vào danh sách", new(SelectLogos)));
//            //    if (logos.Count > 0)
//            //        menuItems.Add(new("Xóa hết logo trong danh sách", new(() =>
//            //        {
//            //            logos.Clear();
//            //            GameScr.info1.addInfo("Đã xóa hết logo trong danh sách!", 0);
//            //        })));
//            //}));
//        }

//        private static void paintTabHeader(Panel panel, mGraphics g)
//        {
//            PaintPanelTemplates.paintTabHeaderTemplate(panel, g, "Danh sách logo tùy chỉnh");
//        }

//        public static void setTabCustomLogoPanel(Panel panel)
//        {
//            SetTabPanelTemplates.setTabListTemplate(panel, logos);
//        }

//        public static void doFireCustomLogoListPanel(Panel panel)
//        {
//            int selected = panel.selected;
//            if (selected < 0) return;
//            string fileName = Path.GetFileName(logos.ElementAt(selected).Key);

//            new MenuBuilder()
//                .addItem("Xóa", new(() =>
//                {
//                    logos.Remove(logos.ElementAt(selected).Key);
//                    if (selected < logoIndex)
//                    {
//                        logoIndex--;
//                        lastTimeChangedLogo = mSystem.currentTimeMillis();
//                    }
//                    else if (selected == logoIndex && logos.Count == logoIndex)
//                    {
//                        logoIndex = 0;
//                        lastTimeChangedLogo = mSystem.currentTimeMillis();
//                    }
//                    GameScr.info1.addInfo("Đã xóa ảnh " + selected + "!", 0);
//                    setTabCustomLogoPanel(panel);
//                    SaveData();
//                }))
//                .setPos(panel.X, (selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll)
//                .start();

//            //OpenMenu.start(
//            //    menuItemCollection: new(menuItems =>
//            //    {
//            //        menuItems.Add(new("Xóa", new(() =>
//            //        {
//            //            logos.Remove(logos.ElementAt(selected).Key);
//            //            if (selected < logoIndex)
//            //            {
//            //                logoIndex--;
//            //                lastTimeChangedLogo = mSystem.currentTimeMillis();
//            //            }
//            //            else if (selected == logoIndex && logos.Count == logoIndex)
//            //            {
//            //                logoIndex = 0;
//            //                lastTimeChangedLogo = mSystem.currentTimeMillis();
//            //            }
//            //            GameScr.info1.addInfo("Đã xóa ảnh " + selected + "!", 0);
//            //            setTabCustomLogoPanel();
//            //            SaveData();
//            //        })));
//            //    }),
//            //    x: panel.X,
//            //    y: (selected + 1) * panel.ITEM_HEIGHT - panel.cmy + panel.yScroll);

//            panel.cp = new ChatPopup();
//            panel.cp.isClip = false;
//            panel.cp.sayWidth = 180;
//            panel.cp.cx = 3 + panel.X - ((panel.X != 0) ? (Res.abs(panel.cp.sayWidth - panel.W) + 8) : 0);
//            panel.cp.says = mFont.tahoma_7_red.splitFontArray("|0|2|" + fileName + "\n--\n|6|Đường dẫn đầy đủ: " + logos.ElementAt(selected).Key, panel.cp.sayWidth - 10);
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

//        public static void paint(mGraphics g)
//        {
//            if (!isEnabled || logos.Count <= 0) return;
//            g.drawImage(logos.ElementAt(logoIndex).Value, GameCanvas.w / 2, 0, mGraphics.HCENTER);
//            if (mSystem.currentTimeMillis() - lastTimeChangedLogo > inveralChangeLogo)
//            {
//                lastTimeChangedLogo = mSystem.currentTimeMillis();
//                logoIndex++;
//                if (logoIndex >= logos.Count)
//                {
//                    logoIndex = 0;
//                }
//            }
//        }

//        public static void paintCustomLogoPanel(Panel panel, mGraphics g)
//        {
//            PaintPanelTemplates.paintCollectionCaptionAndDescriptionTemplate(panel, g, logos,
//                w => Path.GetFileName(w.Key), w => $"Đường dẫn đầy đủ: {w.Key}");
//        }


//        public static void LoadData()
//        {
//            try
//            {
//                foreach (string path in Utils.loadRMSString("customlogopath").Split('|'))
//                {
//                    try
//                    {
//                        if (!string.IsNullOrEmpty(path)) logos.Add(path, null);
//                    }
//                    catch (ArgumentException)
//                    {
//                        logos[path] = null;
//                    }
//                }
//                isLogoLoaded = true;
//            }
//            catch (Exception)
//            { }
//        }

//        public static void SaveData()
//        {
//            string data = string.Join("|", logos.Keys.ToArray());
//            Utils.saveRMSString("customlogopath", data);
//        }

//        public static void setState(bool value) => isEnabled = value;

//        public static void setState(int value) => inveralChangeLogo = value * 1000;

//        public static void setLogoHeight(int value) => height = value;
//    }
//}
