using Mod.Auto;
using Mod.CustomPanel;
using Mod.Graphics;
using Mod.PickMob;
using Mod.Set;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using UnityEngine;
using Vietpad.InputMethod;

namespace Mod.ModMenu
{
    public class ModMenuMain : IChatable
    {
        /// <summary>
        /// Thêm bật/tắt chức năng mod ở đây
        /// </summary>
        public static ModMenuItemBoolean[] modMenuItemBools = new ModMenuItemBoolean[]
        {
            new ModMenuItemBoolean("Vsync", "Tắt Vsync nếu bạn muốn điều chỉnh FPS!", value => QualitySettings.vSyncCount = value ? 1 : 0, true, "isvsync"),
            new ModMenuItemBoolean("Hiện thông tin nhân vật", "Hiện gần chính xác thời gian NRD, khiên, khỉ, huýt sáo... của nhân vật đang focus", CharEffect.setState, true, "isshowinfochar"),
            new ModMenuItemBoolean("Tự đánh", "Bật/tắt tự đánh", AutoAttack.toggle),
            new ModMenuItemBoolean("Hiện danh sách nhân vật", "Hiện danh sách nhân vật trong map", ListCharsInMap.setState, false, "isshowlistchar"),
            new ModMenuItemBoolean("Hiện đệ tử trong danh sách", "Hiện đệ tử trong danh sách nhân vật trong map (đệ tử không có sư phụ trong map không được hiển thị)", ListCharsInMap.setStatePet, false, "isshowlistpet", true, "Bạn chưa bật chức năng \"Hiện danh sách nhân vật\"!"),
            new ModMenuItemBoolean("Auto up SS", "Auto up acc sơ sinh đến nhiệm vụ vào bang", AutoSS.setState, false, "", true, "Bạn đã qua nhiệm vụ sơ sinh!"),
            ///new ModMenuItemBoolean("Auto T77", "Auto up Tàu Pảy Pảy", AutoT77.setState, false, "", true, "Bạn không thể vào map Đông Karin!"),
            new ModMenuItemBoolean("Hiện khoảng cách bom", "Hiển thị người, quái, boss... trong tầm bom", SuicideRange.setState, false, "isshowsuiciderange"),
            new ModMenuItemBoolean("Nền tùy chỉnh", "Thay thế nền của game bằng nền tùy chỉnh (tự động điều chỉnh nền cho vừa kích thước màn hình)", CustomBackground.setState, false, "iscustombackground", false, "Bạn cần tắt chức năng \"Giảm đồ họa\"!"),
            new ModMenuItemBoolean("Logo tùy chỉnh", "Bật/tắt hiển thị logo tùy chỉnh trên màn hình game", CustomLogo.setState, false, "isshowlogo"),
            new ModMenuItemBoolean("Thông báo Boss", "Bật/tắt hiển thị thông báo boss", Boss.setState, false, "sanboss"),
            new ModMenuItemBoolean("Con trỏ tùy chỉnh", "Thay con trỏ chuột mặc định thành con trỏ chuột tùy chỉnh", CustomCursor.setState, false, "customcusor"),

            new ModMenuItemBoolean("Tàn sát", "Bật/tắt tự động đánh quái", value => Pk9rPickMob.IsTanSat = value, false, "", false, "Bạn đang bật auto T77 hoặc auto up SS!"),
            new ModMenuItemBoolean("Né siêu quái khi tàn sát", "Tự động né siêu quái khi tàn sát", value => Pk9rPickMob.IsNeSieuQuai = value, true, "isnesieuquaits"),
            new ModMenuItemBoolean("Vượt địa hình khi tàn sát", "Bật/tắt tự động vượt địa hình khi đang tàn sát", value => Pk9rPickMob.IsVuotDiaHinh = value, true, "isvuotdiahinh"),
            new ModMenuItemBoolean("Tự động nhặt vật phẩm", "Bật/tắt tự động nhặt vật phẩm", value => Pk9rPickMob.IsAutoPickItems = value, true, "isautopick", false, "Bạn đang bật auto T77 hoặc auto up SS!"),
            new ModMenuItemBoolean("Không nhặt đồ của người khác", "Bật/tắt lọc không nhặt vật phẩm của người khác", value => Pk9rPickMob.IsItemMe = value, true, "ispickmyitemonly"),
            new ModMenuItemBoolean("Giới hạn số lần nhặt", "Bật/tắt giới hạn số lần tự động nhặt một vật phẩm", value => Pk9rPickMob.IsLimitTimesPickItem = value, true,"islimitpicktimes"),
        };

        /// <summary>
        /// Thêm điều chỉnh chỉ số của chức năng mod ở đây
        /// </summary>
        public static ModMenuItemInt[] modMenuItemInts = new ModMenuItemInt[]
        {
            new ModMenuItemInt("FPS", null, "FPS mục tiêu (cần tắt Vsync để thay đổi có hiệu lực)", 60, delegate(int value)
            {
                if (value > 5 && value <= 60) Application.targetFrameRate = value;
                else throw new ArgumentException();
            }, "targetfps", false, "Bạn chưa tắt Vsync!"),
            new ModMenuItemInt("Giảm đồ họa", new string[]{"Đang tắt", "Đang bật mức 1", "Đang bật mức 2", "Đang bật mức 3"}, "", 0, CustomBackground.StopAllBackgroundVideo, "levelreducegraphics"),
            new ModMenuItemInt("Goback", new string[]{"Đang tắt", "Đang bật (goback tới chỗ cũ khi chết)", "Đang bật (goback tới map cố định)" }, "", 0, AutoGoback.setState),
            new ModMenuItemInt("Gõ tiếng Việt", new string[]{"Đang tắt", "Đang bật kiểu gõ TELEX", "Đang bật kiểu gõ VIQR", "Đang bật kiểu gõ VNI"}, "", 0, delegate(int value)
            {
            if (value == 0) VietKeyHandler.VietModeEnabled = false;
            else
            {
                VietKeyHandler.VietModeEnabled = true;
                VietKeyHandler.InputMethod = (InputMethods)(value - 1);
            }
            }, "vietmode", false, "Bạn không biết gõ tiếng Việt!"),
            new ModMenuItemInt("Auto up đệ tử", new string[]{"Đang tắt", "Đang bật up đệ thường", "Đang bật up đệ né siêu quái", "Đang bật up đệ kaioken"}, "", 0, AutoPet.setState, "", false, "Bạn không có đệ tử!"),
            new ModMenuItemInt("Đánh khi đệ cần", new string[]{"Đánh quái gần nhất", "Đánh đệ (tự động bật cờ xám)", "Đánh bản thân (tự động bật cờ xám)"}, "", 0, AutoPet.setAttackState, "modeautopet", true, "Bạn chưa bật chức năng \"Auto up đệ tử\"!"),
            new ModMenuItemInt("Thời gian đổi nền", null, "Điều chỉnh thời gian thay đổi nền (giây)", 30, CustomBackground.setState, "backgroundinveral", false),
            new ModMenuItemInt("Thời gian đổi logo", null, "Điều chỉnh thời gian thay đổi logo (giây)", 30, CustomLogo.setState, "logoinveral", false),
            new ModMenuItemInt("Chiều cao của logo", null, "Điều chỉnh chiều cao của logo", 80, CustomLogo.setLogoHeight, "logoheight"),
        };

        /// <summary>
        /// Thêm mở chức năng mod ở đây
        /// </summary>
        public static ModMenuItemFunction[] modMenuItemFunctions = new ModMenuItemFunction[]
        {
            new ModMenuItemFunction("Menu Xmap", "Mở menu Xmap (lệnh \"xmp\" hoặc bấm nút x)", Pk9rXmap.showXmapMenu),
            new ModMenuItemFunction("Menu PickMob", "Mở menu PickMob (lệnh \"pickmob\")", Pk9rPickMob.ShowMenu),
            new ModMenuItemFunction("Menu AutoItem", "Mở menu AutoItem (lệnh \"item\" hoặc bấm nút I)", AutoItem.ShowMenu),
            new ModMenuItemFunction("Menu Teleport", "Mở menu dịch chuyển (lệnh \"tele\" hoặc bấm nút z)", TeleportMenu.TeleportMenu.ShowMenu),
            new ModMenuItemFunction("Menu Custom Background", "Mở menu nền tùy chỉnh", CustomBackground.ShowMenu),
            new ModMenuItemFunction("Menu Custom Logo", "Mở menu logo tùy chỉnh", CustomLogo.ShowMenu),
            new ModMenuItemFunction("Menu Custom Cursor", "Mở menu con trỏ tùy chỉnh", CustomCursor.ShowMenu),
            new ModMenuItemFunction("Menu Set đồ", "Mở menu set đồ (lệnh \"set\" hoặc bấm nút \'`\')", SetDo.ShowMenu),
        };

        public static Dictionary<int, string[]> inputModMenuItemInts = new Dictionary<int, string[]>()
        {
            { 0, new string[]{"Nhập mức FPS", "FPS"} },
            { 6, new string[]{"Nhập thời gian thay đổi nền", "Thời gian (giây)"} },
            { 7, new string[]{"Nhập thời gian thay đổi logo", "Thời gian (giây)"} },
            { 8, new string[]{"Nhập chiều cao logo", "Chiều cao logo" } },
        };

        public static void setTabModMenu(Panel panel)
        {
            SetTabPanelTemplates.setTabListTemplate(panel, modMenuItemBools, modMenuItemInts, modMenuItemFunctions, ExtensionManager.Extensions);

            //panel.ITEM_HEIGHT = 24;
            //if (panel.currentTabIndex == 0) panel.currentListLength = modMenuItemBools.Length;
            //else if (panel.currentTabIndex == 1) panel.currentListLength = modMenuItemInts.Length;
            //else if (panel.currentTabIndex == 2) panel.currentListLength = modMenuItemFunctions.Length;
            //else panel.currentListLength = ExtensionManager.Extensions.Count;
            //panel.selected = (GameCanvas.isTouch ? (-1) : 0);
            //panel.cmyLim = panel.currentListLength * panel.ITEM_HEIGHT - panel.hScroll;
            //if (panel.cmyLim < 0) panel.cmyLim = 0;
            //panel.cmy = panel.cmtoY = panel.cmyLast[panel.currentTabIndex];
            //if (panel.cmy < 0) panel.cmy = panel.cmtoY = 0;
            //if (panel.cmy > panel.cmyLim) panel.cmy = panel.cmtoY = panel.cmyLim;
        }

        public static void doFireModMenu(Panel panel)
        {
            if (panel.currentTabIndex == 0)
                doFireModMenuBools(panel);
            else if (panel.currentTabIndex == 1)
                doFireModMenuInts(panel);
            else if (panel.currentTabIndex == 2)
                doFireModMenuFunctions(panel);
            else 
                doFireModMenuExtensions(panel);
            notifySelectDisabledItem(panel);
        }

        private static void doFireModMenuExtensions(Panel panel)
        {
            panel.hideNow();
            ExtensionManager.Extensions[panel.selected].OpenMenu();
        }

        private static void doFireModMenuFunctions(Panel panel)
        {
            panel.hideNow();
            if (modMenuItemFunctions[panel.selected].Action != null) 
                modMenuItemFunctions[panel.selected].Action();
        }

        private static void doFireModMenuBools(Panel panel)
        {
            if (panel.selected < 0) return;
            if (!modMenuItemBools[panel.selected].isDisabled)
            {
                modMenuItemBools[panel.selected].setValue(!modMenuItemBools[panel.selected].Value);
                GameScr.info1.addInfo("Đã " + (modMenuItemBools[panel.selected].Value ? "bật" : "tắt") + " " + modMenuItemBools[panel.selected].Title + "!", 0);
            }
        }

        private static void doFireModMenuInts(Panel panel)
        {
            if (panel.selected < 0) return;
            int selected = panel.selected;
            if (modMenuItemInts[selected].isDisabled) return;
            if (modMenuItemInts[selected].Values != null) modMenuItemInts[selected].SwitchSelection();
            else
            {
                panel.chatTField = new ChatTextField();
                panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                panel.chatTField.initChatTextField();
                panel.chatTField.strChat = string.Empty;
                panel.chatTField.tfChat.name = inputModMenuItemInts[selected][1];
                panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                panel.chatTField.startChat2(new ModMenuMain(), inputModMenuItemInts[selected][0]);
            }
        }

        private static void notifySelectDisabledItem(Panel panel)
        {
            int selected = panel.selected;
            if (panel.currentTabIndex == 0)
            {
                if (!modMenuItemBools[selected].isDisabled) return;
                GameScr.info1.addInfo(modMenuItemBools[selected].DisabledReason, 0);
            }
            else if (panel.currentTabIndex == 1)
            {
                if (!modMenuItemInts[selected].isDisabled) return;
                GameScr.info1.addInfo(modMenuItemInts[selected].DisabledReason, 0);
            }
            else if(panel.currentTabIndex == 2)
            {
                if (!modMenuItemFunctions[selected].isDisabled) return;
                GameScr.info1.addInfo(modMenuItemFunctions[selected].DisabledReason, 0);
            }
        }

        public static void paintModMenu(Panel panel, mGraphics g)
        {
            if (panel.currentTabIndex == 0)
                paintModMenuBools(panel ,g);
            else if (panel.currentTabIndex == 1)
                paintModMenuInts(panel ,g);
            else if (panel.currentTabIndex == 2)
                paintModMenuFunctions(panel ,g);
            else
                paintModMenuExtensions(panel ,g);
        }

        private static void paintModMenuExtensions(Panel panel, mGraphics g)
        {
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            g.setColor(0);
            if (ExtensionManager.Extensions == null || ExtensionManager.Extensions.Count != panel.currentListLength) return;
            bool isReset = true;
            string descriptionTextInfo = string.Empty;
            int x = 0, y = 0;
            for (int i = 0; i < panel.currentListLength; i++)
            {
                int num = panel.xScroll;
                int num2 = panel.yScroll + i * panel.ITEM_HEIGHT;
                int num3 = panel.wScroll;
                int num4 = panel.ITEM_HEIGHT - 1;
                ExtensionManager ext = ExtensionManager.Extensions[i];
                if (ext.HasMenuItems()) g.setColor((i != panel.selected) ? 15196114 : 16383818);
                else g.setColor((i != panel.selected) ? new Color(0.54f, 0.51f, 0.46f) : new Color(0.61f, 0.63f, 0.18f));
                g.fillRect(num, num2, num3, num4);
                if (ext != null)
                {
                    mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + ext.ExtensionName + ' ' + ext.ExtensionVersion, num + 5, num2, 0);
                    string description = Utilities.TrimUntilFit(ext.ExtensionDescription, new GUIStyle() { font = mFont.tahoma_7_blue.myFont }, 160);
                    if (i == panel.selected && mFont.tahoma_7_blue.getWidth(ext.ExtensionDescription) > 160 && !panel.isClose)
                    {
                        isReset = false;
                        descriptionTextInfo = ext.ExtensionDescription;
                        x = num + 5;
                        y = num2 + 11;
                    }
                    else mFont.tahoma_7_blue.drawString(g, description, num + 5, num2 + 11, 0);
                }
            }
            if (isReset) TextInfo.reset();
            else
            {
                TextInfo.paint(g, descriptionTextInfo, x, y, 160, 15, mFont.tahoma_7_blue);
                g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
                g.translate(0, -panel.cmy);
            }
            panel.paintScrollArrow(g);
        }

        private static void paintModMenuFunctions(Panel panel, mGraphics g)
        {
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            g.setColor(0);
            if (modMenuItemFunctions == null || modMenuItemFunctions.Length != panel.currentListLength) return;
            bool isReset = true;
            string descriptionTextInfo = string.Empty;
            int x = 0, y = 0;
            for (int i = 0; i < panel.currentListLength; i++)
            {
                int num = panel.xScroll;
                int num2 = panel.yScroll + i * panel.ITEM_HEIGHT;
                int num3 = panel.wScroll;
                int num4 = panel.ITEM_HEIGHT - 1;
                ModMenuItemFunction modMenuItem = modMenuItemFunctions[i];
                if (!modMenuItem.isDisabled) g.setColor((i != panel.selected) ? 15196114 : 16383818);
                else g.setColor((i != panel.selected) ? new Color(0.54f, 0.51f, 0.46f) : new Color(0.61f, 0.63f, 0.18f));
                g.fillRect(num, num2, num3, num4);
                if (modMenuItem != null)
                {
                    mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, num + 5, num2, 0);
                    string description = Utilities.TrimUntilFit(modMenuItem.Description, new GUIStyle() { font = mFont.tahoma_7_blue.myFont }, 160);
                    if (i == panel.selected && mFont.tahoma_7_blue.getWidth(modMenuItem.Description) > 160 && !panel.isClose)
                    {
                        isReset = false;
                        descriptionTextInfo = modMenuItem.Description;
                        x = num + 5;
                        y = num2 + 11;
                    }
                    else mFont.tahoma_7_blue.drawString(g, description, num + 5, num2 + 11, 0);
                }
            }
            if (isReset) TextInfo.reset();
            else
            {
                TextInfo.paint(g, descriptionTextInfo, x, y, 160, 15, mFont.tahoma_7_blue);
                g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
                g.translate(0, -panel.cmy);
            }
            panel.paintScrollArrow(g);
        }

        private static void paintModMenuBools(Panel panel, mGraphics g)
        {
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            g.setColor(0);
            if (modMenuItemBools == null || modMenuItemBools.Length != panel.currentListLength) return;
            bool isReset = true;
            string descriptionTextInfo = string.Empty;
            int x = 0, y = 0;
            string str = (mResources.status + ": ") == "Trạng thái: " ? "Đang " : (mResources.status + ": ");
            for (int i = 0; i < panel.currentListLength; i++)
            {
                int num = panel.xScroll;
                int num2 = panel.yScroll + i * panel.ITEM_HEIGHT;
                int num3 = panel.wScroll;
                int num4 = panel.ITEM_HEIGHT - 1;
                ModMenuItemBoolean modMenuItem = modMenuItemBools[i];
                if (!modMenuItem.isDisabled) g.setColor((i != panel.selected) ? 15196114 : 16383818);
                else g.setColor((i != panel.selected) ? new Color(0.54f, 0.51f, 0.46f) : new Color(0.61f, 0.63f, 0.18f));
                g.fillRect(num, num2, num3, num4);
                if (modMenuItem != null)
                {
                    mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, num + 5, num2, 0);
                    string description = Utilities.TrimUntilFit(modMenuItem.Description, new GUIStyle() { font = mFont.tahoma_7_blue.myFont }, 145 - mFont.tahoma_7b_red.getWidth(str));
                    if (i == panel.selected && mFont.tahoma_7_blue.getWidth(modMenuItem.Description) > 145 - mFont.tahoma_7b_red.getWidth(str) && !panel.isClose)
                    {
                        isReset = false;
                        descriptionTextInfo = modMenuItem.Description;
                        x = num + 5;
                        y = num2 + 11;
                    }
                    else mFont.tahoma_7_blue.drawString(g, description, num + 5, num2 + 11, 0);
                    mFont mf = mFont.tahoma_7_grey;
                    if (modMenuItem.Value) mf = mFont.tahoma_7b_red;
                    mf.drawString(g, str + (modMenuItem.Value ? mResources.ON.ToLower() : mResources.OFF.ToLower()), num + num3 - 2, num2 + panel.ITEM_HEIGHT - 14, mFont.RIGHT);
                }
            }
            if (isReset) TextInfo.reset();
            else
            {
                TextInfo.paint(g, descriptionTextInfo, x, y, 145 - mFont.tahoma_7b_red.getWidth(str), 15, mFont.tahoma_7_blue);
                g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
                g.translate(0, -panel.cmy);
            }
            panel.paintScrollArrow(g);
        }

        private static void paintModMenuInts(Panel panel, mGraphics g)
        {
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            g.setColor(0);
            if (modMenuItemInts == null || modMenuItemInts.Length != panel.currentListLength) return;
            bool isReset = true;
            string descriptionTextInfo = string.Empty;
            int x = 0, y = 0, currSelectedValue = 0;
            for (int i = 0; i < panel.currentListLength; i++)
            {
                int num = panel.xScroll;
                int num2 = panel.yScroll + i * panel.ITEM_HEIGHT;
                int num3 = panel.wScroll;
                int num4 = panel.ITEM_HEIGHT - 1;
                ModMenuItemInt modMenuItem = modMenuItemInts[i];
                if (!modMenuItem.isDisabled) g.setColor((i != panel.selected) ? 15196114 : 16383818);
                else g.setColor((i != panel.selected) ? new Color(0.54f, 0.51f, 0.46f) : new Color(0.61f, 0.63f, 0.18f));
                g.fillRect(num, num2, num3, num4);
                if (modMenuItem != null)
                {
                    string description, str;
                    mFont.tahoma_7_green2.drawString(g, i + 1 + ". " + modMenuItem.Title, num + 5, num2, 0);
                    if (modMenuItem.Values != null)
                    {
                        str = modMenuItem.getSelectedValue();
                        description = Utilities.TrimUntilFit(str, new GUIStyle() { font = mFont.tahoma_7_blue.myFont}, 160);
                    }
                    else
                    {
                        str = modMenuItem.Description;
                        description = Utilities.TrimUntilFit(str, new GUIStyle() { font = mFont.tahoma_7_blue.myFont }, 160 - mFont.tahoma_7_blue.getWidth(modMenuItem.SelectedValue.ToString()));
                        mFont.tahoma_7b_red.drawString(g, modMenuItem.SelectedValue.ToString(), num + num3 - 2, num2 + panel.ITEM_HEIGHT - 14, mFont.RIGHT);
                    }
                    if (i == panel.selected && mFont.tahoma_7_blue.getWidth(str) > 160 - mFont.tahoma_7_blue.getWidth(modMenuItem.SelectedValue.ToString()) && !panel.isClose)
                    {
                        isReset = false;
                        descriptionTextInfo = modMenuItem.Description;
                        currSelectedValue = modMenuItem.SelectedValue;
                        x = num + 5;
                        y = num2 + 11;
                    }
                    else mFont.tahoma_7_blue.drawString(g, description, num + 5, num2 + 11, 0);
                }
            }
            if (isReset) TextInfo.reset();
            else
            {
                TextInfo.paint(g, descriptionTextInfo, x, y, 160 - mFont.tahoma_7_blue.getWidth(currSelectedValue.ToString()), 15, mFont.tahoma_7_blue);
                g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
                g.translate(0, -panel.cmy);
            }
            panel.paintScrollArrow(g);
        }

        public static void onModMenuValueChanged()
        {
            modMenuItemBools[4].isDisabled = !modMenuItemBools[3].Value;
            if (Char.myCharz().taskMaint != null) modMenuItemBools[5].isDisabled = Char.myCharz().taskMaint.taskId > 11;
            if (Char.myCharz().cPower > 2000000 || (Char.myCharz().cPower > 1500000 && TileMap.mapID != 111) || (Char.myCharz().taskMaint != null && Char.myCharz().taskMaint.taskId < 9)) modMenuItemBools[6].isDisabled = true;
            else modMenuItemBools[6].isDisabled = false;
            modMenuItemBools[8].isDisabled = modMenuItemInts[1].SelectedValue > 0;
            //modMenuItemBools[10].isDisabled = AutoSS.isAutoSS || AutoT77.isAutoT77;
            modMenuItemBools[10].isDisabled = AutoSS.isAutoSS;

            modMenuItemInts[0].isDisabled = modMenuItemBools[0].Value;
            modMenuItemInts[2].isDisabled = modMenuItemBools[5].Value || modMenuItemBools[6].Value;
            if (modMenuItemInts[2].isDisabled) modMenuItemInts[2].SelectedValue = 0;
            modMenuItemInts[4].isDisabled = !Char.myCharz().havePet || modMenuItemBools[5].Value || modMenuItemBools[6].Value;
            if (modMenuItemInts[4].isDisabled) modMenuItemInts[4].SelectedValue = 0;
            modMenuItemInts[5].isDisabled = modMenuItemInts[4].SelectedValue == 0;
        }

        public static void SaveData()
        {
            foreach (ModMenuItemBoolean modMenuItem in modMenuItemBools) if (!string.IsNullOrEmpty(modMenuItem.RMSName)) Utilities.saveRMSBool(modMenuItem.RMSName, modMenuItem.Value);
            foreach (ModMenuItemInt modMenuItem in modMenuItemInts) if (!string.IsNullOrEmpty(modMenuItem.RMSName)) Utilities.saveRMSInt(modMenuItem.RMSName, modMenuItem.SelectedValue);
        }

        public static void LoadData()
        {
            foreach (ModMenuItemBoolean modMenuItem in modMenuItemBools)
            {
                try
                {
                    if (!string.IsNullOrEmpty(modMenuItem.RMSName)) modMenuItem.setValue(Utilities.loadRMSBool(modMenuItem.RMSName));
                }
                catch { }
            }
            foreach (ModMenuItemInt modMenuItem in modMenuItemInts)
            {
                try
                {
                    int data = Utilities.loadRMSInt(modMenuItem.RMSName);
                    modMenuItem.setValue(data == -1 ? 0 : data);
                }
                catch { }
            }
        }

        public static bool getStatusBool(string rmsName)
        {
            foreach (ModMenuItemBoolean modMenuItem in modMenuItemBools)
            {
                if (modMenuItem.RMSName == rmsName) return modMenuItem.Value;
            }
            throw new Exception("Not found any ModMenuItemBoolean with RMSName \"" + rmsName + "\"!");
        }

        public static bool getStatusBool(int index)
        {
            return modMenuItemBools[index].Value;
        }

        public static int getStatusInt(string rmsName)
        {
            foreach (ModMenuItemInt modMenuItem in modMenuItemInts)
            {
                if (modMenuItem.RMSName == rmsName) return modMenuItem.SelectedValue;
            }
            throw new Exception("Not found any ModMenuItemInt with RMSName \"" + rmsName + "\"!");
        }

        public static int getStatusInt(int index)
        {
            return modMenuItemInts[index].SelectedValue;
        }

        public void onChatFromMe(string text, string to)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (to == inputModMenuItemInts[0][0])
                {
                    try
                    {
                        int value = int.Parse(text);
                        if (value > 60 || value < 5) throw new Exception();
                        modMenuItemInts[0].setValue(value);
                        GameScr.info1.addInfo("Đã thay đổi mức FPS!", 0);
                    }
                    catch
                    {
                        GameCanvas.startOKDlg("Mức FPS không hợp lệ!");
                    }
                }
                else if (to == inputModMenuItemInts[6][0])
                {
                    try
                    {
                        int value = int.Parse(text);
                        if (value < 10) throw new Exception();
                        modMenuItemInts[6].setValue(value);
                        GameScr.info1.addInfo("Đã thay đổi thời gian đổi nền!", 0);
                    }
                    catch
                    {
                        GameCanvas.startOKDlg("Thời gian không hợp lệ!");
                    }
                }
                else if (to == inputModMenuItemInts[7][0])
                {
                    try
                    {
                        int value = int.Parse(text);
                        if (value < 10) throw new Exception();
                        modMenuItemInts[7].setValue(value);
                        GameScr.info1.addInfo("Đã thay đổi thời gian đổi logo!", 0);
                    }
                    catch
                    {
                        GameCanvas.startOKDlg("Thời gian không hợp lệ!");
                    }
                }
                else if (to == inputModMenuItemInts[8][0])
                {
                    try
                    {
                        int value = int.Parse(text);
                        if (value < 25 || value > Screen.height * 30 / 100) throw new Exception();
                        modMenuItemInts[8].setValue(value);
                        GameScr.info1.addInfo("Đã thay đổi chiều cao logo!", 0);
                        CustomLogo.LoadData();
                    }
                    catch
                    {
                        GameCanvas.startOKDlg("Chiều cao không hợp lệ!");
                    }
                }
            }
            else 
                GameCanvas.panel.chatTField.isShow = false;
            GameCanvas.panel.chatTField.ResetTF();
        }

        public void onCancelChat()
        {
            GameCanvas.panel.chatTField.ResetTF();
        }
    }
}