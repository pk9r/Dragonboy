using Mod.CustomPanel;
using Mod.Graphics;
using Mod.ModHelper;
using Mod.ModHelper.Menu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Mod.Set
{
    internal class SetDo
    {

        public string itemAo;
        public string itemQuan;
        public string itemGiay;
        public string itemGang;
        public string itemRada;
        public string itemCaiTrang;
        public string itemGiapLuyenTap;
        public string itemThu8;   //không biết loại item
        public string itemDeoLung;
        public string itemBay;
        public string Name;

        const int TYPE_ITEM_8 = 0;  //không biết loại item

        public static int offset;
        public static List<SetDo> setDos = new List<SetDo>();
        private static bool isShowMenu;
        private static long lastTimeSetDisableCloseMenu;

        public static bool IsCurrentPanelIsSetDoPanel
        {
            get
            {
                if (!GameCanvas.panel.isShow)
                    return false;
                Action action = (Action)typeof(CustomPanelMenu).GetField("setTab", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                return action != null && action.Method == typeof(SetDo).GetMethod("setTabSetPanel");
            }
        }

        SetDo(string name, Item ao, Item quan, Item giay, Item gang, Item rada, Item caiTrang, Item giapLuyenTap, Item item8, Item deoLung, Item bay)
        {
            Name = name;
            if (ao != null)
                itemAo = ao.GetFullName();
            if (quan != null)
                itemQuan = quan.GetFullName();
            if (gang != null)
                itemGang = gang.GetFullName();
            if (giay != null)
                itemGiay = giay.GetFullName();    
            if (rada != null)
                itemRada = rada.GetFullName();
            if (caiTrang != null)
                itemCaiTrang = caiTrang.GetFullName();
            if (giapLuyenTap != null)
                itemGiapLuyenTap = giapLuyenTap.GetFullName();
            if (item8 != null)
                itemThu8 = item8.GetFullName();
            if (deoLung != null)
                itemDeoLung = deoLung.GetFullName();
            if (bay != null)
                itemBay = bay.GetFullName();
        }

        public static void AddSet(string name, Item ao = null, Item quan = null, Item giay = null, Item gang = null, Item rada = null, Item caiTrang = null, Item giapLuyenTap = null, Item item8 = null, Item deoLung = null, Item bay = null)
        {
            setDos.Add(new SetDo(name, ao, quan, gang, giay, rada, caiTrang, giapLuyenTap, item8, deoLung, bay));
        }

        public void AddOrReplaceItem(Item item)
        {
            if (item == null)
                return;
            if (item.template.type == 0)
                itemAo = item.GetFullName();
            else if (item.template.type == 1)
                itemQuan = item.GetFullName();
            else if (item.template.type == 2)
                itemGang = item.GetFullName();
            else if (item.template.type == 3)
                itemGiay = item.GetFullName();
            else if (item.template.type == 4)
                itemRada = item.GetFullName();
            else if (item.template.type == 5)
                itemCaiTrang = item.GetFullName();
            else if (item.template.type == 32)
                itemGiapLuyenTap = item.GetFullName();

            else if (item.template.type == TYPE_ITEM_8)
                itemThu8 = item.GetFullName();

            else if (item.template.type == 11)
                itemDeoLung = item.GetFullName();
            else if (item.template.type == 23 || item.template.type == 24)
                itemBay = item.GetFullName();
        }

        public bool HasItem(Item item) => item != null && (itemAo == item.GetFullName() || itemQuan == item.GetFullName() || itemGang == item.GetFullName() || itemGiay == item.GetFullName() || itemRada == item.GetFullName() || itemCaiTrang == item.GetFullName() || itemGiapLuyenTap == item.GetFullName() || itemThu8 == item.GetFullName() || itemDeoLung == item.GetFullName() || itemBay == item.GetFullName());

        public void Wear()
        {
            new Thread(() =>
            {
                while (true)
                {
                    DateTime now = DateTime.Now;
                    if (GetItem(itemAo, 0))
                        Thread.Sleep(100);
                    if (GetItem(itemQuan, 1))
                        Thread.Sleep(100);
                    if (GetItem(itemGang, 2))
                        Thread.Sleep(100);
                    if (GetItem(itemGiay, 3))
                        Thread.Sleep(100);
                    if (GetItem(itemRada, 4))
                        Thread.Sleep(100);
                    if (GetItem(itemCaiTrang, 5))
                        Thread.Sleep(100);
                    if (GetItem(itemGiapLuyenTap, 6))
                        Thread.Sleep(100);
                    if (GetItem(itemThu8, 7))
                        Thread.Sleep(100);
                    if (GetItem(itemDeoLung, 8))
                        Thread.Sleep(100);
                    if (GetItem(itemBay, 9))
                        Thread.Sleep(100);
                    if (DateTime.Now.Subtract(now).TotalMilliseconds < 50)
                        break;
                }
            })
            {
                IsBackground = true,
                Name = "SetDo.Wear"
            }.Start();
        }

        public static bool GetItem(string itemFullName, int index)
        {
            if (string.IsNullOrEmpty(itemFullName))
                return false;
            Char me = Char.myCharz();
            if (me.arrItemBody[index] != null && itemFullName == me.arrItemBody[index].GetFullName())
                return false;
            Item[] arrItemBag = Char.myCharz().arrItemBag.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).ToArray();
            for (int i = 0; i < arrItemBag.Length; i++)
            {
                Item item = arrItemBag[i];
                if (itemFullName == item.GetFullName())
                {
                    MainThreadDispatcher.dispatcher(() => Service.gI().getItem(4, (sbyte)item.indexUI));
                    return true;
                }
            }
            return false;
        }

        public static void ShowMenu()
        {
            OpenMenu.start(new MenuItemCollection(menuItems =>
            {
                for (int i = 0; i < Math.min(5, setDos.Count); i++)
                {
                    int index = i;  //bắt buộc phải làm thế này
                    menuItems.Add(new MenuItem($"Mặc set\n{(string.IsNullOrEmpty(setDos[i].Name) ? i + 1 : setDos[i].Name)}", new MenuAction(() =>
                    {
                        setDos[index].Wear();
                    })));
                }
                menuItems.Add(new MenuItem("Mở danh sách set đồ", new MenuAction(() =>
                {
                    string[][] tabName = new string[Math.min(4, setDos.Count + 1)][];
                    for (int i = offset; i < tabName.Length + offset; i++)
                    {
                        if (i - offset == setDos.Count)
                            tabName[i - offset] = new string[] { "Thêm", "Set mới" };
                        else
                            tabName[i - offset] = new string[] { "Set đồ", string.IsNullOrEmpty(setDos[i].Name) ? (i + 1).ToString() : setDos[i].Name };
                    }
                    GameCanvas.panel.tabName[CustomPanelMenu.TYPE_CUSTOM_PANEL_MENU] = tabName;
                    CustomPanelMenu.CreateCustomPanelMenu(setTabSetPanel, doFireSetPanel, null, paintSetPanel);
                })));
            }));
        }

        public static void setTabSetPanel()
        {
            GameCanvas.panel.ITEM_HEIGHT = 24;
            GameCanvas.panel.currentListLength = Char.myCharz().arrItemBody.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).Count() + Char.myCharz().arrItemBag.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).Count();
            GameCanvas.panel.selected = (GameCanvas.isTouch ? (-1) : 0);
            GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
            if (GameCanvas.panel.cmyLim < 0) GameCanvas.panel.cmyLim = 0;
            GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex];
            if (GameCanvas.panel.cmy < 0) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = 0;
            if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim;
        }

        public static void paintSetPanel(mGraphics g)
        {
            g.setColor(16711680);
            if (offset > 0)
                g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 5, 1, 61, 0);
            if (offset < setDos.Count - 3)
                g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 4, GameCanvas.panel.wScroll - 7, 61, 0);
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
            Item[] arrItemBody = Char.myCharz().arrItemBody.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).ToArray();
            Item[] arrItemBag = Char.myCharz().arrItemBag.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).ToArray();
            for (int i = 0; i < arrItemBody.Length + arrItemBag.Length; i++)
            {
                bool flag = i < arrItemBody.Length;
                int num = i;
                int num2 = i - arrItemBody.Length;
                int num3 = GameCanvas.panel.xScroll + 36;
                int num4 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int num5 = GameCanvas.panel.wScroll - 36;
                int num6 = GameCanvas.panel.ITEM_HEIGHT - 1;
                int num7 = GameCanvas.panel.xScroll;
                int num8 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int num9 = 34;
                int num10 = GameCanvas.panel.ITEM_HEIGHT - 1;
                if (num4 - GameCanvas.panel.cmy <= GameCanvas.panel.yScroll + GameCanvas.panel.hScroll)
                {
                    if (num4 - GameCanvas.panel.cmy >= GameCanvas.panel.yScroll - GameCanvas.panel.ITEM_HEIGHT)
                    {
                        Item item = (!flag) ? arrItemBag[num2] : arrItemBody[num];
                        g.setColor((i != GameCanvas.panel.selected) ? ((!flag) ? 15723751 : 15196114) : 16383818);
                        g.fillRect(num3, num4, num5, num6);
                        g.setColor((i != GameCanvas.panel.selected) ? ((!flag) ? 11837316 : 9993045) : 9541120);
                        if (item.isHaveOption(34))
                            g.setColor((i != GameCanvas.panel.selected) ? Panel.color1[0] : Panel.color2[0]);
                        else if (item.isHaveOption(35))
                            g.setColor((i != GameCanvas.panel.selected) ? Panel.color1[1] : Panel.color2[1]);
                        else if (item.isHaveOption(36))
                            g.setColor((i != GameCanvas.panel.selected) ? Panel.color1[2] : Panel.color2[2]);
                        g.fillRect(num7, num8, num9, num10);
                        if (item.isSelect)
                        {
                            g.setColor((i != GameCanvas.panel.selected) ? 6047789 : 7040779);
                            g.fillRect(num7, num8, num9, num10);
                        }
                        if (GameCanvas.panel.currentTabIndex + offset < setDos.Count && setDos[GameCanvas.panel.currentTabIndex + offset].HasItem(item))
                        {
                            g.setColor(Color.red);
                            g.drawRect(num7, num8, num5 + num9 + 1, num6);
                        }
                        string text = string.Empty;
                        if (item.itemOption != null)
                            for (int j = 0; j < item.itemOption.Length; j++)
                                if (item.itemOption[j].optionTemplate.id == 72)
                                {
                                    text = " [+" + item.itemOption[j].param + "]";
                                    CustomGraphics.PaintItemEffectInPanel(g, num7 + 18, num8 + 12, item.itemOption[j].param);
                                }
                        mFont.tahoma_7_green2.drawString(g, item.template.name + text, num3 + 5, num4 + 1, 0);
                        string text2 = string.Empty;
                        if (item.itemOption != null)
                        {
                            if (item.itemOption.Length > 0 && item.itemOption[0] != null && item.itemOption[0].optionTemplate.id != 102 && item.itemOption[0].optionTemplate.id != 107)
                                text2 += item.itemOption[0].getOptionString();
                            mFont mFont = mFont.tahoma_7_blue;
                            if (item.compare < 0 && item.template.type != 5)
                                mFont = mFont.tahoma_7_red;
                            if (item.itemOption.Length > 1)
                                for (int k = 1; k < 2; k++)
                                    if (item.itemOption[k] != null && item.itemOption[k].optionTemplate.id != 102 && item.itemOption[k].optionTemplate.id != 107)
                                        text2 = text2 + "," + item.itemOption[k].getOptionString();
                            mFont.drawString(g, text2, num3 + 5, num4 + 11, mFont.LEFT);
                        }
                        SmallImage.drawSmallImage(g, item.template.iconID, num7 + num9 / 2, num8 + num10 / 2, 0, 3);
                        if (item.quantity > 1)
                            mFont.tahoma_7_yellow.drawString(g, "x" + item.quantity, num7 + num9, num8 + num10 - mFont.tahoma_7_yellow.getHeight(), 1);
                        CustomGraphics.PaintStar(g, GameCanvas.panel, item, num4);
                    }
                }
            }
            GameCanvas.panel.paintScrollArrow(g);
        }

        public static void doFireSetPanel()
        {
            if (!IsCurrentPanelIsSetDoPanel)
                return;
            int selected = GameCanvas.panel.selected;
            if (selected < 0)
                return;
            GameCanvas.panel.currItem = null;
            Item[] arrItemBody = Char.myCharz().arrItemBody.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).ToArray();
            Item[] arrItemBag = Char.myCharz().arrItemBag.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).ToArray();
            if (selected >= arrItemBody.Length)
                GameCanvas.panel.currItem = arrItemBag[selected - arrItemBody.Length];
            else
                GameCanvas.panel.currItem = arrItemBody[selected];
            OpenMenu.start(new MenuItemCollection(menuItems =>
                {
                    string message = "Thêm vào\nset ";
                    if (GameCanvas.panel.currentTabIndex + offset == setDos.Count)
                        message += "mới";
                    else
                    {
                        if (setDos[GameCanvas.panel.currentTabIndex + offset].HasItem(GameCanvas.panel.currItem))
                            message = "Xóa khỏi\nset ";
                        message += string.IsNullOrEmpty(setDos[GameCanvas.panel.currentTabIndex + offset].Name) ? GameCanvas.panel.currentTabIndex + offset + 1 : ("\n" + setDos[GameCanvas.panel.currentTabIndex + offset].Name);
                    }
                    menuItems.Add(new MenuItem(message, new MenuAction(() =>
                    {
                        if (GameCanvas.panel.currentTabIndex + offset == setDos.Count)
                        {
                            AddSet("");
                            RefreshPanelTabName();
                            Utilities.EmulateSetTypePanel();
                        }
                        setDos[GameCanvas.panel.currentTabIndex + offset].AddOrReplaceItem(GameCanvas.panel.currItem);
                    })));
                }),
                GameCanvas.panel.X,
                (selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);
            if (GameCanvas.panel.currItem != null)
            {
                Char.myCharz().setPartTemp(GameCanvas.panel.currItem.headTemp, GameCanvas.panel.currItem.bodyTemp, GameCanvas.panel.currItem.legTemp, GameCanvas.panel.currItem.bagTemp);
                GameCanvas.panel.addItemDetail(GameCanvas.panel.currItem);
            }
            else
                GameCanvas.panel.cp = null;
        }

        public static void Update()
        {
            if (!IsCurrentPanelIsSetDoPanel)
                return;
            if (GameCanvas.isMouseFocus(GameCanvas.panel.X, GameCanvas.panel.Y + 50, GameCanvas.panel.W, 28))
            {
                if (GameCanvas.pXYScrollMouse > 0)
                {
                    if (GameCanvas.panel.currentTabIndex < Math.min(4, setDos.Count + 1) - 1)
                        GameCanvas.panel.currentTabIndex++;
                    else if (offset < setDos.Count - 3)
                    {
                        offset++;
                        RefreshPanelTabName();
                        Utilities.EmulateSetTypePanel();
                    }
                }
                if (GameCanvas.pXYScrollMouse < 0)
                {
                    if (GameCanvas.panel.currentTabIndex > 0)
                        GameCanvas.panel.currentTabIndex--;
                    else if (offset > 0)
                    {
                        offset--;
                        RefreshPanelTabName();
                        Utilities.EmulateSetTypePanel();
                    }
                }
            }
            if (isShowMenu && mSystem.currentTimeMillis() - lastTimeSetDisableCloseMenu > 500 && GameCanvas.menu.showMenu)
            {
                isShowMenu = false;
                typeof(Menu).GetField("disableClose", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(GameCanvas.menu, false);
                typeof(Menu).GetField("isClose", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(GameCanvas.menu, false);
            }
        }

        public static void UpdateScrollMouse(ref int pXYScrollMouse)
        {
            if (!IsCurrentPanelIsSetDoPanel)
                return;
            if (GameCanvas.isMouseFocus(GameCanvas.panel.X, GameCanvas.panel.Y + 50, GameCanvas.panel.W, 28) && pXYScrollMouse != 0)
                pXYScrollMouse = 0;
        }

        public static void UpdateTouch()
        {
            if (!IsCurrentPanelIsSetDoPanel)
                return;
            if (offset < setDos.Count - 3 && GameCanvas.isPointerHoldIn(GameCanvas.panel.wScroll - 7, 61, 6, 9))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                {
                    if (GameCanvas.panel.currentTabIndex < Math.min(4, setDos.Count + 1) - 1)
                        GameCanvas.panel.currentTabIndex++;
                    else
                    {
                        offset++;
                        RefreshPanelTabName();
                        Utilities.EmulateSetTypePanel();
                    }
                }
                GameCanvas.clearAllPointerEvent();
            }
            else if (offset > 0 && GameCanvas.isPointerHoldIn(2, 61, 6, 9))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                {
                    if (GameCanvas.panel.currentTabIndex > 0)
                        GameCanvas.panel.currentTabIndex--;
                    else
                    {
                        offset--;
                        RefreshPanelTabName();
                        Utilities.EmulateSetTypePanel();
                    }
                }
                GameCanvas.clearAllPointerEvent();
            }
            for (int i = 0; i < GameCanvas.panel.currentTabName.Length; i++)
                if (i + offset < setDos.Count && GameCanvas.isPointer(GameCanvas.panel.startTabPos + i * GameCanvas.panel.TAB_W, 52, GameCanvas.panel.TAB_W - 1, 25))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick)
                    {
                        OpenMenu.start(new MenuItemCollection(menuItems =>
                        {
                            int index = i;
                            if (GameCanvas.panel.currentTabIndex != i)
                                menuItems.Add(new MenuItem("Xem set", new MenuAction(() =>
                                {
                                    GameCanvas.panel.currentTabIndex = index;
                                }))); 
                            menuItems.Add(new MenuItem("Đổi tên set", new MenuAction(() =>
                            {
                                GameCanvas.startOKDlg("Not implemented");
                            })));  
                            menuItems.Add(new MenuItem("Xóa set", new MenuAction(() =>
                            {
                                GameCanvas.startOKDlg("Not implemented");
                            })));
                        }), 3, 61);
                        string str = "Set đồ " + (string.IsNullOrEmpty(setDos[i + offset].Name) ? i + offset + 1 : setDos[i + offset].Name);
                        GameCanvas.panel.popUpDetailInit(GameCanvas.panel.cp = new ChatPopup(), str);
                        GameCanvas.panel.idIcon = -1;
                        GameCanvas.panel.partID = null;
                        GameCanvas.panel.charInfo = null;
                        GameCanvas.panel.currItem = null;
                        GameCanvas.panel.cp.cy = 30;
                        typeof(Menu).GetField("disableClose", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(GameCanvas.menu, true);
                        isShowMenu = true;
                        lastTimeSetDisableCloseMenu = mSystem.currentTimeMillis();
                    }
                    GameCanvas.clearAllPointerEvent();
                }
        }

        private static void RefreshPanelTabName()
        {
            string[][] tabName = new string[Math.min(4, setDos.Count + 1)][];
            for (int i = offset; i < tabName.Length + offset; i++)
            {
                if (i == setDos.Count)
                    tabName[i - offset] = new string[] { "Thêm", "Set mới" };
                else
                    tabName[i - offset] = new string[] { "Set đồ", string.IsNullOrEmpty(setDos[i].Name) ? (i + 1).ToString() : setDos[i].Name };
            }
            GameCanvas.panel.tabName[CustomPanelMenu.TYPE_CUSTOM_PANEL_MENU] = tabName;
            GameCanvas.panel.currentTabName = tabName;
        }
    }
}
