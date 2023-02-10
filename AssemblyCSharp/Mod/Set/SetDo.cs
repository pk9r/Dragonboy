using LitJson;
using LitJSON;
using Mod.CustomPanel;
using Mod.Graphics;
using Mod.ModHelper;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using Mod.ModMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Analytics;

namespace Mod.Set
{
    internal class SetDo : IChatable, IActionListener
    {
        internal class ItemSet
        {
            public string name = "";
            public string fullName = "";
            public int gender = -1;

            public ItemSet() { }

            public ItemSet(string name, string fullName, int gender)
            {
                this.name = name;
                this.fullName = fullName;
                this.gender = gender;
            }

            public ItemSet(Item item)
            {
                if (item == null)
                    return;
                name = item.template.name;
                fullName = item.GetFullInfo();
                gender = item.template.gender;
            }

            public void Deconstruct()
            {
                name = "";
                fullName = "";
                gender = -1;
            }

            public override string ToString()
            {
                return fullName;
            }
        }

        public ItemSet itemAo;
        public ItemSet itemQuan;
        public ItemSet itemGiay;
        public ItemSet itemGang;
        public ItemSet itemRada;
        public ItemSet itemCaiTrang;
        public ItemSet itemGiapLuyenTap;
        public ItemSet itemThu8;   //không biết loại item
        public ItemSet itemDeoLung;
        public ItemSet itemBay;
        public string Name;

        const int TYPE_ITEM_8 = 0;  //không biết loại item

        [JsonSkip]
        public static int offset;
        [JsonSkip]
        public static List<SetDo> setDos = new List<SetDo>();
        [JsonSkip]
        private static bool isShowMenu;
        [JsonSkip]
        private static long lastTimeSetDisableCloseMenu;
        [JsonSkip]
        static int indexSetToRename;

        [JsonSkip]
        public static bool IsCurrentPanelIsSetDoPanel
        {
            get
            {
                if (!GameCanvas.panel.isShow)
                    return false;
                if (GameCanvas.panel.currentTabName == null)
                    return false;
                if (GameCanvas.panel.currentTabName.Length != Math.min(4, setDos.Count + 1))
                    return false;
                if (!GameCanvas.panel.currentTabName[0][0].Contains("Set") && !GameCanvas.panel.currentTabName[0][1].Contains("Set"))
                    return false;
                Action action = (Action)typeof(CustomPanelMenu).GetField("setTab", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                return action != null && action.Method == typeof(SetDo).GetMethod("setTabSetPanel");
            }
        }

        public SetDo()
        {

        }

        SetDo(string name, Item ao, Item quan, Item giay, Item gang, Item rada, Item caiTrang, Item giapLuyenTap, Item item8, Item deoLung, Item bay)
        {
            Name = name;
            itemAo = new ItemSet(ao);
            itemQuan = new ItemSet(quan);
            itemGang = new ItemSet(gang);
            itemGiay = new ItemSet(giay);
            itemRada = new ItemSet(rada);
            itemCaiTrang = new ItemSet(caiTrang);
            itemGiapLuyenTap = new ItemSet(giapLuyenTap);
            itemThu8 = new ItemSet(item8);
            itemDeoLung = new ItemSet(deoLung);
            itemBay = new ItemSet(bay);
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
                itemAo = new ItemSet(item);
            else if (item.template.type == 1)
                itemQuan = new ItemSet(item);
            else if (item.template.type == 2)
                itemGang = new ItemSet(item);
            else if (item.template.type == 3)
                itemGiay = new ItemSet(item);
            else if (item.template.type == 4)
                itemRada = new ItemSet(item);
            else if (item.template.type == 5)
                itemCaiTrang = new ItemSet(item);
            else if (item.template.type == 32)
                itemGiapLuyenTap = new ItemSet(item);

            else if (item.template.type == TYPE_ITEM_8)
                itemThu8 = new ItemSet(item);

            else if (item.template.type == 11)
                itemDeoLung = new ItemSet(item);
            else if (item.template.type == 23 || item.template.type == 24)
                itemBay = new ItemSet(item);
        }

        public void RemoveItem(Item item)
        {
            if (item == null)
                return;
            if (item.GetFullInfo() == itemAo.fullName)
                itemAo.Deconstruct();
            if (item.GetFullInfo() == itemQuan.fullName)
                itemQuan.Deconstruct();
            if (item.GetFullInfo() == itemGang.fullName)
                itemGang.Deconstruct();
            if (item.GetFullInfo() == itemGiay.fullName)
                itemGiay.Deconstruct();
            if (item.GetFullInfo() == itemRada.fullName)
                itemRada.Deconstruct();
            if (item.GetFullInfo() == itemCaiTrang.fullName)
                itemCaiTrang.Deconstruct();
            if (item.GetFullInfo() == itemGiapLuyenTap.fullName)
                itemGiapLuyenTap.Deconstruct();
            if (item.GetFullInfo() == itemThu8.fullName)
                itemThu8.Deconstruct();
            if (item.GetFullInfo() == itemDeoLung.fullName)
                itemDeoLung.Deconstruct();
            if (item.GetFullInfo() == itemBay.fullName)
                itemBay.Deconstruct();
        }

        public bool HasItem(Item item) => item != null && (itemAo.fullName == item.GetFullInfo() || itemQuan.fullName == item.GetFullInfo() || itemGang.fullName == item.GetFullInfo() || itemGiay.fullName == item.GetFullInfo() || itemRada.fullName == item.GetFullInfo() || itemCaiTrang.fullName == item.GetFullInfo() || itemGiapLuyenTap.fullName == item.GetFullInfo() || itemThu8.fullName == item.GetFullInfo() || itemDeoLung.fullName == item.GetFullInfo() || itemBay.fullName == item.GetFullInfo());

        [JsonSkip]
        public bool HasAnyItem
        {
            get
            {
                return !string.IsNullOrEmpty(itemAo.fullName + itemQuan.fullName + itemGang.fullName + itemGiay.fullName + itemRada.fullName + itemCaiTrang.fullName + itemGiapLuyenTap.fullName + itemThu8.fullName + itemDeoLung.fullName + itemBay.fullName + itemAo.name + itemQuan.name + itemGang.name + itemGiay.name + itemRada.name + itemCaiTrang.name + itemGiapLuyenTap.name + itemThu8.name + itemDeoLung.name + itemBay.name);
            }
        }

        public bool CanWearForMe()
        {
            int myGender = Char.myCharz().cgender;
            if (itemAo.gender != -1 && myGender != itemAo.gender)
                return false;
            if (itemQuan.gender != -1 && myGender != itemQuan.gender)
                return false;
            if (itemGang.gender != -1 && myGender != itemGang.gender)
                return false;
            if (itemGiay.gender != -1 && myGender != itemGiay.gender)
                return false;

            if (itemCaiTrang.gender != -1 && itemCaiTrang.gender != 3 && myGender != itemCaiTrang.gender)
                return false;
            if (itemGiapLuyenTap.gender != -1 && itemGiapLuyenTap.gender != 3 && myGender != itemGiapLuyenTap.gender)
                return false;
            if (itemThu8.gender != -1 && itemThu8.gender != 3 && myGender != itemThu8.gender)
                return false;
            if (itemDeoLung.gender != -1 && itemDeoLung.gender != 3 && myGender != itemDeoLung.gender)
                return false;
            if (itemBay.gender != -1 && itemBay.gender != 3 && myGender != itemBay.gender)
                return false;
            return true;
        }

        public bool CanWearForPet()
        {
            int petGender = Utilities.GetPetGender();
            if (itemAo.gender != -1 && petGender != itemAo.gender)
                return false;
            if (itemQuan.gender != -1 && petGender != itemQuan.gender)
                return false;
            if (itemGang.gender != -1 && petGender != itemGang.gender)
                return false;
            if (itemGiay.gender != -1 && petGender != itemGiay.gender)
                return false;
            if (itemCaiTrang.gender != -1 && itemCaiTrang.gender != 3 && petGender != itemCaiTrang.gender)
                return false;
            return true;
        }

        public void Wear()
        {
            if (!CanWearForMe())
            {
                GameScr.info1.addInfo("Set có chứa đồ khác hệ!", 0);
                return;
            }
            new Thread(() =>
            {
                while (true)
                {
                    DateTime now = DateTime.Now;
                    if (GetItem(itemAo.fullName, 0))
                        Thread.Sleep(100);
                    if (GetItem(itemQuan.fullName, 1))
                        Thread.Sleep(100);
                    if (GetItem(itemGang.fullName, 2))
                        Thread.Sleep(100);
                    if (GetItem(itemGiay.fullName, 3))
                        Thread.Sleep(100);
                    if (GetItem(itemRada.fullName, 4))
                        Thread.Sleep(100);
                    if (GetItem(itemCaiTrang.fullName, 5))
                        Thread.Sleep(100);
                    if (GetItem(itemGiapLuyenTap.fullName, 6))
                        Thread.Sleep(100);
                    if (GetItem(itemThu8.fullName, 7))
                        Thread.Sleep(100);
                    if (GetItem(itemDeoLung.fullName, 8))
                        Thread.Sleep(100);
                    if (GetItem(itemBay.fullName, 9))
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

        public void WearForPet()
        {
            if (!CanWearForPet())
            {
                GameScr.info1.addInfo("Set có chứa đồ khác hệ của đệ tử!", 0);
                return;
            }
            new Thread(() =>
            {
                while (true)
                {
                    DateTime now = DateTime.Now;
                    if (GetItem(itemAo.fullName, 0, 6))
                        Thread.Sleep(100);
                    if (GetItem(itemQuan.fullName, 1, 6))
                        Thread.Sleep(100);
                    if (GetItem(itemGang.fullName, 2, 6))
                        Thread.Sleep(100);
                    if (GetItem(itemGiay.fullName, 3, 6))
                        Thread.Sleep(100);
                    if (GetItem(itemRada.fullName, 4, 6))
                        Thread.Sleep(100);
                    if (GetItem(itemCaiTrang.fullName, 5, 6))
                        Thread.Sleep(100);
                    if (DateTime.Now.Subtract(now).TotalMilliseconds < 50)
                        break;
                }
            })
            {
                IsBackground = true,
                Name = "SetDo.WearForPet"
            }.Start();
        }

        public static bool GetItem(string itemFullName, int index, sbyte type = 4)
        {
            if (string.IsNullOrEmpty(itemFullName))
                return false;
            if (type == 4)
            {
                Char me = Char.myCharz();
                if (me.arrItemBody[index] != null && itemFullName == me.arrItemBody[index].GetFullInfo())
                    return false;
                Item[] arrItemBag = Char.myCharz().arrItemBag.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).ToArray();
                for (int i = 0; i < arrItemBag.Length; i++)
                {
                    Item item = arrItemBag[i];
                    if (itemFullName == item.GetFullInfo())
                    {
                        MainThreadDispatcher.dispatcher(() => Service.gI().getItem(type, (sbyte)item.indexUI));
                        return true;
                    }
                }
            }
            else if (type == 6)
            {
                Char pet = Char.myPetz();
                if (pet.arrItemBody[index] != null && itemFullName == pet.arrItemBody[index].GetFullInfo())
                    return false;
                Item[] arrItemBag = Char.myCharz().arrItemBag.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).ToArray();
                for (int i = 0; i < arrItemBag.Length; i++)
                {
                    Item item = arrItemBag[i];
                    if (itemFullName == item.GetFullInfo())
                    {
                        MainThreadDispatcher.dispatcher(() => Service.gI().getItem(type, (sbyte)item.indexUI));
                        return true;
                    }
                }
            }
            return false;
        }

        public static string GetSetItems(SetDo set)
        {
            string result = "";
            if (set.itemAo != null && !string.IsNullOrEmpty(set.itemAo.name))
                result += set.itemAo.name + ", ";
            if (set.itemQuan != null && !string.IsNullOrEmpty(set.itemQuan.name))
                result += set.itemQuan.name + ", ";
            if (set.itemGang != null && !string.IsNullOrEmpty(set.itemGang.name))
                result += set.itemGang.name + ", ";
            if (set.itemGiay != null && !string.IsNullOrEmpty(set.itemGiay.name))
                result += set.itemGiay.name + ", ";
            if (set.itemRada != null && !string.IsNullOrEmpty(set.itemRada.name))
                result += set.itemRada.name + ", ";
            if (set.itemCaiTrang != null && !string.IsNullOrEmpty(set.itemCaiTrang.name))
                result += set.itemCaiTrang.name + ", ";
            if (set.itemGiapLuyenTap != null && !string.IsNullOrEmpty(set.itemGiapLuyenTap.name))
                result += set.itemGiapLuyenTap.name + ", ";
            if (set.itemThu8 != null && !string.IsNullOrEmpty(set.itemThu8.name))
                result += set.itemThu8.name + ", ";
            if (set.itemDeoLung != null && !string.IsNullOrEmpty(set.itemDeoLung.name))
                result += set.itemDeoLung.name + ", ";
            if (set.itemBay != null && !string.IsNullOrEmpty(set.itemBay.name))
                result += set.itemBay.name + ", ";
            return result.TrimEnd(' ').TrimEnd(',');
        }

        [ChatCommand("set"), HotkeyCommand('`')]
        public static void ShowMenu()
        {
            string description = $"Bạn đã lưu {setDos.Count} set đồ.";
            for (int i = 0; i < Math.min(5, setDos.Count); i++)
                description += $"\nSet {(string.IsNullOrEmpty(setDos[i].Name) ? i + 1 : setDos[i].Name)}: {GetSetItems(setDos[i])}";
            if (setDos.Count > 5)
                description += "\n...";

            new MenuBuilder()
                .setChatPopup(description)
                .map(Enumerable.Range(0, Math.min(5, setDos.Count)), index =>
                {
                    return new($"Mặc set\n{(string.IsNullOrEmpty(setDos[index].Name) ? index + 1 : setDos[index].Name)}", new(() =>
                    {
                        setDos[index].Wear();
                    }));
                })
                .addItem(ifCondition: Char.myCharz().havePet && setDos.Count > 0,
                    "Mặc cho\nđệ tử", new(ShowMenuPetSet))
                .addItem("Mở danh sách set đồ", new(() =>
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
                    CustomPanelMenu.CreateCustomPanelMenu(setTabSetPanel, doFireSetPanel, null, paintSetPanel);
                }))
                .addItem(ifCondition: setDos.Count > 0,
                "Xoá hết\nset đồ\nđã lưu", new(() =>
                {
                    GameCanvas.startYesNoDlg(
                        $"Bạn có chắc chắn muốn xoá hết set đồ đã lưu không?", 
                        new Command(mResources.YES, new SetDo(), 2, null), 
                        new Command(mResources.NO, new SetDo(), 100, null));
                }))
                .start();

            //OpenMenu.start(new MenuItemCollection(menuItems =>
            //{
            //    for (int i = 0; i < Math.min(5, setDos.Count); i++)
            //    {
            //        int index = i;  //bắt buộc phải làm thế này
            //        menuItems.Add(new MenuItem($"Mặc set\n{(string.IsNullOrEmpty(setDos[i].Name) ? i + 1 : setDos[i].Name)}", new MenuAction(() =>
            //        {
            //            setDos[index].Wear();
            //        })));
            //    }
            //    if (Char.myCharz().havePet && setDos.Count > 0)
            //        menuItems.Add(new MenuItem("Mặc cho\nđệ tử", new MenuAction(ShowMenuPetSet)));
            //    menuItems.Add(new MenuItem("Mở danh sách set đồ", new MenuAction(() =>
            //    {
            //        string[][] tabName = new string[Math.min(4, setDos.Count + 1)][];
            //        for (int i = offset; i < tabName.Length + offset; i++)
            //        {
            //            if (i == setDos.Count)
            //                tabName[i - offset] = new string[] { "Thêm", "Set mới" };
            //            else
            //                tabName[i - offset] = new string[] { "Set đồ", string.IsNullOrEmpty(setDos[i].Name) ? (i + 1).ToString() : setDos[i].Name };
            //        }
            //        GameCanvas.panel.tabName[CustomPanelMenu.TYPE_CUSTOM_PANEL_MENU] = tabName;
            //        CustomPanelMenu.CreateCustomPanelMenu(setTabSetPanel, doFireSetPanel, null, paintSetPanel);
            //    })));
            //    if (setDos.Count > 0)
            //        menuItems.Add(new MenuItem("Xoá hết\nset đồ\nđã lưu", new MenuAction(() =>
            //        {
            //            GameCanvas.startYesNoDlg($"Bạn có chắc chắn muốn xoá hết set đồ đã lưu không?", new Command(mResources.YES, new SetDo(), 2, null), new Command(mResources.NO, new SetDo(), 100, null));
            //        })));
            //}), description);
        }

        private static void ShowMenuPetSet()
        {
            string description = $"Bạn đã lưu {setDos.Count} set đồ.";
            for (int i = 0; i < Math.min(5, setDos.Count); i++)
                description += $"\nSet {(string.IsNullOrEmpty(setDos[i].Name) ? i + 1 : setDos[i].Name)}: {GetSetItems(setDos[i])}";
            if (setDos.Count > 5)
                description += "\n...";
            
            //OpenMenu.start(new MenuItemCollection(menuItems =>
            //{
            //    for (int i = 0; i < Math.min(5, setDos.Count); i++)
            //    {
            //        int index = i;
            //        menuItems.Add(new MenuItem($"Mặc cho\nđệ set\n{(string.IsNullOrEmpty(setDos[i].Name) ? i + 1 : setDos[i].Name)}", new MenuAction(() =>
            //        {
            //            setDos[index].WearForPet();
            //        })));
            //    }
            //    menuItems.Add(new MenuItem("Quay lại", new MenuAction(ShowMenu)));
            //}), description);

            new MenuBuilder()
                .setChatPopup(description)
                .map(Enumerable.Range(0, Math.min(5, setDos.Count)), index =>
                {
                    return new($"Mặc cho\nđệ set\n{(string.IsNullOrEmpty(setDos[index].Name) ? index + 1 : setDos[index].Name)}", new(() =>
                    {
                        setDos[index].WearForPet();
                    }));
                })
                .start();
        }

        public static void setTabSetPanel()
        {
            GameCanvas.panel.ITEM_HEIGHT = 24;
            GameCanvas.panel.currentListLength = Char.myCharz().arrItemBody.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).Count() + Char.myCharz().arrItemBag.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).Count() + Char.myPetz().arrItemBody.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).Count();
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
            Item[] arrItemPetBody = Char.myPetz().arrItemBody.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).ToArray();
            for (int i = 0; i < arrItemBody.Length + arrItemBag.Length + arrItemPetBody.Length; i++)
            {
                bool isItemBody = i < arrItemBody.Length;
                bool isItemBag = i >= arrItemBody.Length && i < arrItemBody.Length + arrItemBag.Length;
                bool isItemPetBody = !isItemBody && !isItemBag;
                int x1 = GameCanvas.panel.xScroll + 36;
                int y1 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int w1 = GameCanvas.panel.wScroll - 36;
                int h = GameCanvas.panel.ITEM_HEIGHT - 1;
                int x2 = GameCanvas.panel.xScroll;
                int y2 = GameCanvas.panel.yScroll + i * GameCanvas.panel.ITEM_HEIGHT;
                int w2 = 34;
                int itemHeight = GameCanvas.panel.ITEM_HEIGHT - 1;
                if (y1 - GameCanvas.panel.cmy <= GameCanvas.panel.yScroll + GameCanvas.panel.hScroll)
                {
                    if (y1 - GameCanvas.panel.cmy >= GameCanvas.panel.yScroll - GameCanvas.panel.ITEM_HEIGHT)
                    {
                        Item item = null;
                        if (isItemBody)
                            item = arrItemBody[i];
                        else if (isItemBag)
                            item = arrItemBag[i - arrItemBody.Length];
                        else if (isItemPetBody)
                            item = arrItemPetBody[i - arrItemBody.Length - arrItemBag.Length];
                        if (isItemBody)
                            g.setColor((i != GameCanvas.panel.selected) ? 15196114 : 16383818);
                        else if (isItemPetBody)
                            if (i == GameCanvas.panel.selected)
                                g.setColor(16383818);
                            else
                                g.setColor(new Color(0.9f, 0.87f, 0.72f));
                        else
                            g.setColor((i != GameCanvas.panel.selected) ? 15723751 : 16383818);
                        g.fillRect(x1, y1, w1, h);
                        if (i == GameCanvas.panel.selected)
                            g.setColor(9541120);
                        else
                        {
                            if (isItemBody || isItemPetBody)
                                g.setColor(9993045);
                            else
                                g.setColor(11837316);
                        }
                        if (item.isHaveOption(34))
                            g.setColor((i != GameCanvas.panel.selected) ? Panel.color1[0] : Panel.color2[0]);
                        else if (item.isHaveOption(35))
                            g.setColor((i != GameCanvas.panel.selected) ? Panel.color1[1] : Panel.color2[1]);
                        else if (item.isHaveOption(36))
                            g.setColor((i != GameCanvas.panel.selected) ? Panel.color1[2] : Panel.color2[2]);
                        g.fillRect(x2, y2, w2, itemHeight);
                        if (item.isSelect)
                        {
                            g.setColor((i != GameCanvas.panel.selected) ? 6047789 : 7040779);
                            g.fillRect(x2, y2, w2, itemHeight);
                        }
                        if (GameCanvas.panel.currentTabIndex + offset < setDos.Count && setDos[GameCanvas.panel.currentTabIndex + offset].HasItem(item))
                        {
                            g.setColor(Color.red);
                            g.drawRect(x2, y2, w1 + w2 + 1, h);
                        }
                        string text = string.Empty;
                        if (item.itemOption != null)
                            for (int j = 0; j < item.itemOption.Length; j++)
                                if (item.itemOption[j].optionTemplate.id == 72)
                                {
                                    text = " [+" + item.itemOption[j].param + "]";
                                    CustomGraphics.PaintItemEffectInPanel(g, x2 + 18, y2 + 12, item.itemOption[j].param);
                                }
                        mFont.tahoma_7_green2.drawString(g, item.template.name + text, x1 + 5, y1 + 1, 0);
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
                            mFont.drawString(g, text2, x1 + 5, y1 + 11, mFont.LEFT);
                        }
                        SmallImage.drawSmallImage(g, item.template.iconID, x2 + w2 / 2, y2 + itemHeight / 2, 0, 3);
                        if (item.quantity > 1)
                            mFont.tahoma_7_yellow.drawString(g, "x" + item.quantity, x2 + w2, y2 + itemHeight - mFont.tahoma_7_yellow.getHeight(), 1);
                        CustomGraphics.PaintStar(g, GameCanvas.panel, item, y1);
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
            Item[] arrItemPetBody = Char.myPetz().arrItemBody.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).ToArray();
            if (selected >= arrItemBody.Length + arrItemBag.Length)
                GameCanvas.panel.currItem = arrItemPetBody[selected - arrItemBody.Length - arrItemBag.Length];
            else if (selected >= arrItemBody.Length)
                GameCanvas.panel.currItem = arrItemBag[selected - arrItemBody.Length];
            else
                GameCanvas.panel.currItem = arrItemBody[selected];
            //OpenMenu.start(new MenuItemCollection(menuItems =>
            //    {
            //        string message = "Thêm vào\nset ";
            //        if (GameCanvas.panel.currentTabIndex + offset == setDos.Count)
            //            message += "mới";
            //        else
            //        {
            //            if (setDos[GameCanvas.panel.currentTabIndex + offset].HasItem(GameCanvas.panel.currItem))
            //                message = "Xóa khỏi\nset ";
            //            message += string.IsNullOrEmpty(setDos[GameCanvas.panel.currentTabIndex + offset].Name) ? GameCanvas.panel.currentTabIndex + offset + 1 : ("\n" + setDos[GameCanvas.panel.currentTabIndex + offset].Name);
            //        }
            //        menuItems.Add(new MenuItem(message, new MenuAction(() =>
            //        {
            //            if (GameCanvas.panel.currentTabIndex + offset == setDos.Count)
            //            {
            //                AddSet("");
            //                RefreshPanelTabName();
            //                GameCanvas.panel.EmulateSetTypePanel(0);
            //            }
            //            if (setDos[GameCanvas.panel.currentTabIndex + offset].HasItem(GameCanvas.panel.currItem))
            //            {
            //                setDos[GameCanvas.panel.currentTabIndex + offset].RemoveItem(GameCanvas.panel.currItem);
            //                if (!setDos[GameCanvas.panel.currentTabIndex + offset].HasAnyItem)
            //                {
            //                    string oldName = string.IsNullOrEmpty(setDos[GameCanvas.panel.currentTabIndex + offset].Name) ? (GameCanvas.panel.currentTabIndex + offset + 1).ToString() : ("\"" + setDos[GameCanvas.panel.currentTabIndex + offset].Name + "\"");
            //                    setDos.RemoveAt(GameCanvas.panel.currentTabIndex + offset);
            //                    if (offset >= setDos.Count - 3 && offset > 0)
            //                        offset--;
            //                    if (GameCanvas.panel.currentTabIndex >= Math.min(4, setDos.Count + 1) - 1 && GameCanvas.panel.currentTabIndex > 0)
            //                        GameCanvas.panel.currentTabIndex--;
            //                    RefreshPanelTabName();
            //                    GameCanvas.panel.EmulateSetTypePanel(0);
            //                    GameScr.info1.addInfo($"Đã xóa set {oldName} do set {oldName} không chứa đồ nào!", 0);
            //                }
            //            }
            //            else
            //                setDos[GameCanvas.panel.currentTabIndex + offset].AddOrReplaceItem(GameCanvas.panel.currItem);
            //            SaveData();
            //        })));
            //    }),
            //    GameCanvas.panel.X,
            //    (selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll);

            string message = "Thêm vào\nset ";
            if (GameCanvas.panel.currentTabIndex + offset == setDos.Count)
                message += "mới";
            else
            {
                if (setDos[GameCanvas.panel.currentTabIndex + offset].HasItem(GameCanvas.panel.currItem))
                    message = "Xóa khỏi\nset ";
                message += string.IsNullOrEmpty(setDos[GameCanvas.panel.currentTabIndex + offset].Name) ? GameCanvas.panel.currentTabIndex + offset + 1 : ("\n" + setDos[GameCanvas.panel.currentTabIndex + offset].Name);
            }
            new MenuBuilder()
                .addItem(message, new(() =>
                {
                    if (GameCanvas.panel.currentTabIndex + offset == setDos.Count)
                    {
                        AddSet("");
                        RefreshPanelTabName();
                        GameCanvas.panel.EmulateSetTypePanel(0);
                    }
                    if (setDos[GameCanvas.panel.currentTabIndex + offset].HasItem(GameCanvas.panel.currItem))
                    {
                        setDos[GameCanvas.panel.currentTabIndex + offset].RemoveItem(GameCanvas.panel.currItem);
                        if (!setDos[GameCanvas.panel.currentTabIndex + offset].HasAnyItem)
                        {
                            string oldName = string.IsNullOrEmpty(setDos[GameCanvas.panel.currentTabIndex + offset].Name) ? (GameCanvas.panel.currentTabIndex + offset + 1).ToString() : ("\"" + setDos[GameCanvas.panel.currentTabIndex + offset].Name + "\"");
                            setDos.RemoveAt(GameCanvas.panel.currentTabIndex + offset);
                            if (offset >= setDos.Count - 3 && offset > 0)
                                offset--;
                            if (GameCanvas.panel.currentTabIndex >= Math.min(4, setDos.Count + 1) - 1 && GameCanvas.panel.currentTabIndex > 0)
                                GameCanvas.panel.currentTabIndex--;
                            RefreshPanelTabName();
                            GameCanvas.panel.EmulateSetTypePanel(0);
                            GameScr.info1.addInfo($"Đã xóa set {oldName} do set {oldName} không chứa đồ nào!", 0);
                        }
                    }
                    else
                        setDos[GameCanvas.panel.currentTabIndex + offset].AddOrReplaceItem(GameCanvas.panel.currItem);
                    SaveData();
                }))
                .setPos(GameCanvas.panel.X, (selected + 1) * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.cmy + GameCanvas.panel.yScroll)
                .start();

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
                        GameCanvas.panel.EmulateSetTypePanel(0);
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
                        GameCanvas.panel.EmulateSetTypePanel(0);
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
                        GameCanvas.panel.EmulateSetTypePanel(0);
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
                        GameCanvas.panel.EmulateSetTypePanel(0);
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
                        int index = i;

                        new MenuBuilder()
                            .addItem(ifCondition: GameCanvas.panel.currentTabIndex != index,
                                "Xem set", new(() =>
                                {
                                    GameCanvas.panel.currentTabIndex = index;
                                }))
                            .addItem("Đổi tên set", new(() =>
                            {
                                indexSetToRename = index + offset;
                                GameCanvas.panel.chatTField = new ChatTextField();
                                GameCanvas.panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                                GameCanvas.panel.chatTField.initChatTextField();
                                GameCanvas.panel.chatTField.strChat = string.Empty;
                                GameCanvas.panel.chatTField.tfChat.name = "Tên set";
                                GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
                                GameCanvas.panel.chatTField.startChat2(new SetDo(), "Nhập tên set mới");
                            }))
                            .addItem("Xóa set", new(() =>
                            {
                                GameCanvas.startYesNoDlg(
                                    $"Bạn có chắc chắn muốn xoá set {(string.IsNullOrEmpty(setDos[index + offset].Name) ? index + offset + 1 : ("\"" + setDos[index + offset].Name + "\""))} không?",
                                    new Command(mResources.YES, new SetDo(), 1, index + offset), new Command(mResources.NO, new SetDo(), 100, null));
                            }))
                            .start();
                        //OpenMenu.start(new MenuItemCollection(menuItems =>
                        //{
                        //    int index = i;
                        //    if (GameCanvas.panel.currentTabIndex != i)
                        //        menuItems.Add(new MenuItem("Xem set", new MenuAction(() =>
                        //        {
                        //            GameCanvas.panel.currentTabIndex = index;
                        //        }))); 
                        //    menuItems.Add(new MenuItem("Đổi tên set", new MenuAction(() =>
                        //    {
                        //        indexSetToRename = index + offset;
                        //        GameCanvas.panel.chatTField = new ChatTextField();
                        //        GameCanvas.panel.chatTField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
                        //        GameCanvas.panel.chatTField.initChatTextField();
                        //        GameCanvas.panel.chatTField.strChat = string.Empty;
                        //        GameCanvas.panel.chatTField.tfChat.name = "Tên set";
                        //        GameCanvas.panel.chatTField.tfChat.setIputType(TField.INPUT_TYPE_ANY);
                        //        GameCanvas.panel.chatTField.startChat2(new SetDo(), "Nhập tên set mới");
                        //    })));  
                        //    menuItems.Add(new MenuItem("Xóa set", new MenuAction(() =>
                        //    {
                        //        GameCanvas.startYesNoDlg($"Bạn có chắc chắn muốn xoá set {(string.IsNullOrEmpty(setDos[index + offset].Name) ? index + offset + 1 : ("\"" + setDos[index + offset].Name + "\""))} không?", new Command(mResources.YES, new SetDo(), 1, index + offset), new Command(mResources.NO, new SetDo(), 100, null));
                        //    })));
                        //}), 3, 61);

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

        public static void LoadData()
        {
            try
            {
                setDos = JsonMapper.ToObject<List<SetDo>>(Utilities.loadRMSString($"setdo_{Utilities.username}_{Utilities.server["ip"]}_{Utilities.server["port"]}"));
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        public static void SaveData()
        {
            try
            {
                Utilities.saveRMSString($"setdo_{Utilities.username}_{Utilities.server["ip"]}_{Utilities.server["port"]}", JsonMapper.ToJson(setDos));
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        static string RemoveInvalidCharacter(string str)
        {
            foreach (char c in "ĵĝğġģĥėĕ\u007fĭĳıęĴĜĞĠĢĤĖĔĬĲIĘ")
                str = str.Replace(c.ToString(), "");
            return str;
        }

        public void onChatFromMe(string text, string to)
        {
            text = RemoveInvalidCharacter(text.Trim());
            if (string.IsNullOrEmpty(text))
                GameCanvas.panel.chatTField.isShow = false;
            else if (to == "Nhập tên set mới")
            {
                string oldName = string.IsNullOrEmpty(setDos[indexSetToRename].Name) ? (indexSetToRename + 1).ToString() : ("\"" + setDos[indexSetToRename].Name + "\"");
                setDos[indexSetToRename].Name = text;
                RefreshPanelTabName();
                GameCanvas.panel.EmulateSetTypePanel(0);
                GameScr.info1.addInfo($"Đã đổi tên set {oldName} thành \"{text}\"!", 0);
                SaveData();
            }
            GameCanvas.panel.chatTField.ResetTF();
        }

        public void onCancelChat()
        {
            GameCanvas.panel.chatTField.ResetTF();
        }

        public void perform(int idAction, object p)
        {
            if (idAction == 1)
            {
                string oldName = string.IsNullOrEmpty(setDos[(int)p].Name) ? ((int)p + 1).ToString() : ("\"" + setDos[(int)p].Name + "\"");
                setDos.RemoveAt((int)p);
                if (offset >= setDos.Count - 3 && offset > 0)
                    offset--;
                if (GameCanvas.panel.currentTabIndex >= Math.min(4, setDos.Count + 1) - 1 && GameCanvas.panel.currentTabIndex > 0)
                    GameCanvas.panel.currentTabIndex--;
                RefreshPanelTabName();
                GameCanvas.panel.EmulateSetTypePanel(0);
                GameScr.info1.addInfo($"Đã xoá set {oldName}!", 0);
                SaveData();
            }
            else if (idAction == 2)
            {
                setDos.Clear();
                RefreshPanelTabName();
                GameCanvas.panel.EmulateSetTypePanel(0);
                GameScr.info1.addInfo($"Đã xoá hết set đồ!", 0);
                SaveData();
            }
            else if (idAction == 100)
            {

            }
            InfoDlg.hide();
            GameCanvas.currentDialog = null;
        }
    }
}
