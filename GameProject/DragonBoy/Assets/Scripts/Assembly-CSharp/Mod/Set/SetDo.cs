using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Mod.CustomPanel;
using Mod.Graphics;
using Mod.ModHelper;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using Newtonsoft.Json;
using UnityEngine;

namespace Mod.Set
{
    internal class SetDo : IChatable, IActionListener
    {
        internal class ItemSet
        {
            internal string name = "";
            internal string fullName = "";
            internal int gender = -1;
            internal int type = -1;

            internal ItemSet() { }
            internal ItemSet(Item item)
            {
                if (item == null)
                    return;
                name = item.template.name;
                fullName = item.GetFullInfo();
                gender = item.template.gender;
                type = item.template.type;
            }

            internal void Deconstruct()
            {
                name = "";
                fullName = "";
                gender = -1;
            }

            public override string ToString() => fullName;
        }

        internal List<ItemSet> items = new List<ItemSet>();
        internal string Name;

        [JsonIgnore]
        internal static int offset;
        [JsonIgnore]
        internal static List<SetDo> setDos = new List<SetDo>();
        [JsonIgnore]
        static bool isShowMenu;
        [JsonIgnore]
        static long lastTimeSetDisableCloseMenu;
        [JsonIgnore]
        static int indexSetToRename;

        [JsonIgnore]
        internal static bool IsCurrentPanelIsSetDoPanel
        {
            get
            {
                if (!GameCanvas.panel.isShow)
                    return false;
                var currentTabName = GameCanvas.panel.currentTabName;
                //HACK: Đoạn dưới currentTabName bị IndexOutOfRangeException, fix tạm bằng check length
                if (currentTabName == null || currentTabName.Length == 0 || currentTabName[0].Length < 2)
                    return false;
                if (currentTabName.Length != Math.Min(4, setDos.Count + 1))
                    return false;
                if (!currentTabName[0][0].Contains("Set") && !currentTabName[0][1].Contains("Set"))
                    return false;

                /*
                var action = CustomPanelMenu.customPanel.setTab;

                return action != null && action.Method == typeof(SetDo).GetMethod("setTabSetPanel");
                */
                return true;
            }
        }

        [JsonIgnore]
        static Item[] arrItemBody = new Item[0];
        [JsonIgnore]
        static Item[] arrItemBag = new Item[0];
        [JsonIgnore]
        static Item[] arrItemBodyPet = new Item[0];

        [JsonIgnore]
        internal bool HasAnyItem => items.Count > 0;

        internal SetDo()
        {

        }

        SetDo(string name, Item[] items)
        {
            Name = name;
            this.items = items.Select(i => new ItemSet(i)).ToList();
        }

        internal static void AddSet(string name, Item[] items = null)
        {
            if (items != null)
                setDos.Add(new SetDo(name, items));
        }

        internal void AddOrReplaceItem(Item item)
        {
            if (item == null)
                return;
            for (int i = 0; i < items.Count; i++)
            {
                if (item.template.type == items[i].type)
                {
                    items[i] = new ItemSet(item);
                    return;
                }
            }
            items.Add(new ItemSet(item));
            //if (item.template.type == 0)
            //    itemAo = new ItemSet(item);
            //else if (item.template.type == 1)
            //    itemQuan = new ItemSet(item);
            //else if (item.template.type == 2)
            //    itemGang = new ItemSet(item);
            //else if (item.template.type == 3)
            //    itemGiay = new ItemSet(item);
            //else if (item.template.type == 4)
            //    itemRada = new ItemSet(item);
            //else if (item.template.type == 5)
            //    itemCaiTrang = new ItemSet(item);
            //else if (item.template.type == 32)
            //    itemGiapLuyenTap = new ItemSet(item);

            //else if (item.template.type == TYPE_ITEM_8)
            //    itemThu8 = new ItemSet(item);

            //else if (item.template.type == 11)
            //    itemDeoLung = new ItemSet(item);
            //else if (item.template.type == 23 || item.template.type == 24)
            //    itemBay = new ItemSet(item);
        }

        internal void RemoveItem(Item item)
        {
            if (item == null)
                return;
            for (int i = 0; i < items.Count; i++)
            {
                if (item.template.type == items[i].type)
                {
                    items.RemoveAt(i);
                    break;
                }
            }
        }

        internal bool HasItem(Item item) => item != null && items.Any(i => i.fullName == item.GetFullInfo());

        internal bool CanWearForMe() => !items.Any(i => i.gender != -1 && i.gender != Char.myCharz().cgender);

        internal bool CanWearForPet() => !items.Where(i => i.type < 6).Any(i => i.gender != -1 && i.gender != Utils.GetPetGender());

        internal void Wear()
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
                    foreach (ItemSet item in items)
                    {
                        if (GetItem(item))
                            Thread.Sleep(100);
                    }
                    //if (GetItem(itemAo.fullName, 0))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemQuan.fullName, 1))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemGang.fullName, 2))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemGiay.fullName, 3))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemRada.fullName, 4))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemCaiTrang.fullName, 5))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemGiapLuyenTap.fullName, 6))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemThu8.fullName, 7))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemDeoLung.fullName, 8))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemBay.fullName, 9))
                    //    Thread.Sleep(100);
                    if (DateTime.Now.Subtract(now).TotalMilliseconds < 50)
                        break;
                }
            })
            {
                IsBackground = true,
            }.Start();
        }

        internal void WearForPet()
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
                    foreach (ItemSet item in items.Where(i => i.type < 6))
                    {
                        if (GetItem(item))
                            Thread.Sleep(100);
                    }
                    //if (GetItem(itemAo.fullName, 0, 6))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemQuan.fullName, 1, 6))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemGang.fullName, 2, 6))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemGiay.fullName, 3, 6))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemRada.fullName, 4, 6))
                    //    Thread.Sleep(100);
                    //if (GetItem(itemCaiTrang.fullName, 5, 6))
                    //    Thread.Sleep(100);
                    if (DateTime.Now.Subtract(now).TotalMilliseconds < 50)
                        break;
                }
            })
            {
                IsBackground = true,
            }.Start();
        }

        internal static bool GetItem(ItemSet itemSet, sbyte type = 4)
        {
            if (string.IsNullOrEmpty(itemSet.fullName))
                return false;
            Char ch;
            if (type == 6)
                ch = Char.myCharz();
            else if (type == 4)
                ch = Char.myPetz();
            else 
                throw new ArgumentOutOfRangeException(nameof(type));
            for (int i = 0; i < ch.arrItemBody.Length; i++)
            {
                if (ch.arrItemBody[i] != null && itemSet.fullName == ch.arrItemBody[i].GetFullInfo())
                    return false;
            }
            Item[] arrItemBag = Char.myCharz().arrItemBag.Where(i => i != null && i.isTypeBody()).ToArray();
            for (int i = 0; i < arrItemBag.Length; i++)
            {
                Item item = arrItemBag[i];
                if (itemSet.fullName == item.GetFullInfo())
                {
                    MainThreadDispatcher.dispatch(() => Service.gI().getItem(type, (sbyte)item.indexUI));
                    return true;
                }
            }
            return false;
        }

        internal static string GetSetItems(SetDo set)
        {
            //string result = "";
            //if (set.itemAo != null && !string.IsNullOrEmpty(set.itemAo.name))
            //    result += set.itemAo.name + ", ";
            //if (set.itemQuan != null && !string.IsNullOrEmpty(set.itemQuan.name))
            //    result += set.itemQuan.name + ", ";
            //if (set.itemGang != null && !string.IsNullOrEmpty(set.itemGang.name))
            //    result += set.itemGang.name + ", ";
            //if (set.itemGiay != null && !string.IsNullOrEmpty(set.itemGiay.name))
            //    result += set.itemGiay.name + ", ";
            //if (set.itemRada != null && !string.IsNullOrEmpty(set.itemRada.name))
            //    result += set.itemRada.name + ", ";
            //if (set.itemCaiTrang != null && !string.IsNullOrEmpty(set.itemCaiTrang.name))
            //    result += set.itemCaiTrang.name + ", ";
            //if (set.itemGiapLuyenTap != null && !string.IsNullOrEmpty(set.itemGiapLuyenTap.name))
            //    result += set.itemGiapLuyenTap.name + ", ";
            //if (set.itemThu8 != null && !string.IsNullOrEmpty(set.itemThu8.name))
            //    result += set.itemThu8.name + ", ";
            //if (set.itemDeoLung != null && !string.IsNullOrEmpty(set.itemDeoLung.name))
            //    result += set.itemDeoLung.name + ", ";
            //if (set.itemBay != null && !string.IsNullOrEmpty(set.itemBay.name))
            //    result += set.itemBay.name + ", ";
            //return result.TrimEnd(' ').TrimEnd(',');
            return string.Join(", ", set.items.Select(i => i.name)).TrimEnd(' ').TrimEnd(',');
        }

        [ChatCommand("set"), HotkeyCommand('`')]
        internal static void ShowMenu()
        {
            string description = $"Bạn đã lưu {setDos.Count} set đồ.";
            for (int i = 0; i < Math.Min(5, setDos.Count); i++)
                description += $"\nSet {(string.IsNullOrEmpty(setDos[i].Name) ? i + 1 : setDos[i].Name)}: {GetSetItems(setDos[i])}";
            if (setDos.Count > 5)
                description += "\n...";

            new MenuBuilder()
                .setChatPopup(description)
                .map(Enumerable.Range(0, Math.Min(5, setDos.Count)), index =>
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
                    string[][] tabName = new string[Math.Min(4, setDos.Count + 1)][];
                    for (int i = offset; i < tabName.Length + offset; i++)
                    {
                        if (i == setDos.Count)
                            tabName[i - offset] = new string[] { "Thêm", "Set mới" };
                        else
                            tabName[i - offset] = new string[] { "Set đồ", string.IsNullOrEmpty(setDos[i].Name) ? (i + 1).ToString() : setDos[i].Name };
                    }
                    GameCanvas.panel.tabName[CustomPanelMenu.TYPE_CUSTOM_PANEL_MENU] = tabName;
                    CustomPanelMenu.Show(new CustomPanelMenuConfig()
                    {
                        SetTabAction = SetTabSetPanel, 
                        DoFireItemAction = DoFireSetPanel, 
                        PaintAction = PaintSetPanel
                    });
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
        }

        static void ShowMenuPetSet()
        {
            string description = $"Bạn đã lưu {setDos.Count} set đồ.";
            for (int i = 0; i < Math.Min(5, setDos.Count); i++)
                description += $"\nSet {(string.IsNullOrEmpty(setDos[i].Name) ? i + 1 : setDos[i].Name)}: {GetSetItems(setDos[i])}";
            if (setDos.Count > 5)
                description += "\n...";

            new MenuBuilder()
                .setChatPopup(description)
                .map(Enumerable.Range(0, Math.Min(5, setDos.Count)), index =>
                {
                    return new($"Mặc cho\nđệ set\n{(string.IsNullOrEmpty(setDos[index].Name) ? index + 1 : setDos[index].Name)}", new(() =>
                    {
                        setDos[index].WearForPet();
                    }));
                })
                .start();
        }

        internal static void SetTabSetPanel(Panel panel)
        {
            SetTabPanelTemplates.setTabListTemplate(panel, getItemCount());

            //GameCanvas.panel.ITEM_HEIGHT = 24;
            //GameCanvas.panel.currentListLength = Char.myCharz().arrItemBody.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).Count() + Char.myCharz().arrItemBag.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).Count() + Char.myPetz().arrItemBody.Where(i => i != null && (i.template.type == TYPE_ITEM_8 || i.isTypeBody())).Count();
            //GameCanvas.panel.selected = (GameCanvas.isTouch ? (-1) : 0);
            //GameCanvas.panel.cmyLim = GameCanvas.panel.currentListLength * GameCanvas.panel.ITEM_HEIGHT - GameCanvas.panel.hScroll;
            //if (GameCanvas.panel.cmyLim < 0) GameCanvas.panel.cmyLim = 0;
            //GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLast[GameCanvas.panel.currentTabIndex];
            //if (GameCanvas.panel.cmy < 0) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = 0;
            //if (GameCanvas.panel.cmy > GameCanvas.panel.cmyLim) GameCanvas.panel.cmy = GameCanvas.panel.cmtoY = GameCanvas.panel.cmyLim;
        }

        internal static void PaintSetPanel(Panel panel, mGraphics g)
        {
            g.setColor(16711680);
            if (offset > 0)
                g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 5, 1, 61, 0);
            if (offset < setDos.Count - 3)
                g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 4, GameCanvas.panel.wScroll - 7, 61, 0);
            g.setClip(GameCanvas.panel.xScroll, GameCanvas.panel.yScroll, GameCanvas.panel.wScroll, GameCanvas.panel.hScroll);
            g.translate(0, -GameCanvas.panel.cmy);
            
            for (int i = 0; i < arrItemBody.Length + arrItemBag.Length + arrItemBodyPet.Length; i++)
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
                            item = arrItemBodyPet[i - arrItemBody.Length - arrItemBag.Length];
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
                            {
                                if (item.itemOption[j].optionTemplate.id == 72)
                                    text = " [+" + item.itemOption[j].param + "]";
                            }
                        CustomGraphics.PaintItemEffectInPanel(g, x2 + 18, y2 + 12, w2, itemHeight, item);
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
                        CustomGraphics.PaintItemOptions(g, GameCanvas.panel, item, y1);
                    }
                }
            }
            GameCanvas.panel.paintScrollArrow(g);
        }

        internal static void DoFireSetPanel(Panel panel)
        {
            if (!IsCurrentPanelIsSetDoPanel)
                return;
            int selected = GameCanvas.panel.selected;
            if (selected < 0)
                return;
            GameCanvas.panel.currItem = null;
            arrItemBody = Char.myCharz().arrItemBody.Where(i => i != null && i.isTypeBody()).ToArray();
            arrItemBag = Char.myCharz().arrItemBag.Where(i => i != null && i.isTypeBody()).ToArray();
            arrItemBodyPet = Char.myPetz().arrItemBody.Where(i => i != null && i.isTypeBody()).ToArray();
            if (selected >= arrItemBody.Length + arrItemBag.Length)
                GameCanvas.panel.currItem = arrItemBodyPet[selected - arrItemBody.Length - arrItemBag.Length];
            else if (selected >= arrItemBody.Length)
                GameCanvas.panel.currItem = arrItemBag[selected - arrItemBody.Length];
            else
                GameCanvas.panel.currItem = arrItemBody[selected];
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
                            if (GameCanvas.panel.currentTabIndex >= Math.Min(4, setDos.Count + 1) - 1 && GameCanvas.panel.currentTabIndex > 0)
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

        internal static void Update()
        {
            if (!IsCurrentPanelIsSetDoPanel)
                return;
            if (GameCanvas.isMouseFocus(GameCanvas.panel.X, GameCanvas.panel.Y + 50, GameCanvas.panel.W, 28))
            {
                if (GameCanvas.pXYScrollMouse > 0)
                {
                    if (GameCanvas.panel.currentTabIndex < Math.Min(4, setDos.Count + 1) - 1)
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
                GameCanvas.menu.disableClose = false;
                GameCanvas.menu.isClose = false;
            }
        }

        internal static void UpdateKey()
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                for (int i = 0; i < Math.Min(9, setDos.Count); i++)
                {
                    int keyPressReal = GameCanvas.keyAsciiPress;
                    switch (keyPressReal)
                    {
                        case '!':
                            keyPressReal = '1';
                            break;
                        case '@':
                            keyPressReal = '2';
                            break;
                        case '#':
                            keyPressReal = '3';
                            break;
                        case '$':
                            keyPressReal = '4';
                            break;
                        case '%':
                            keyPressReal = '5';
                            break;
                        case '^':
                            keyPressReal = '6';
                            break;
                        case '&':
                            keyPressReal = '7';
                            break;
                        case '*':
                            keyPressReal = '8';
                            break;
                        case '(':
                            keyPressReal = '9';
                            break;
                    }
                    if (keyPressReal == i + 49)
                    {
                        GameCanvas.keyAsciiPress = 0;
                        GameCanvas.clearKeyHold();
                        GameCanvas.clearKeyPressed();
                        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                        {
                            if (setDos[i].CanWearForPet())
                                GameScr.info1.addInfo($"Mặc set {(string.IsNullOrEmpty(setDos[i].Name) ? i + 1 : ("\"" + setDos[i].Name + "\""))} cho đệ tử!", 0);
                            setDos[i].WearForPet();
                        }
                        else
                        {
                            if (setDos[i].CanWearForMe())
                                GameScr.info1.addInfo($"Mặc set {(string.IsNullOrEmpty(setDos[i].Name) ? i + 1 : ("\"" + setDos[i].Name + "\""))} cho bản thân!", 0);
                            setDos[i].Wear();
                        }
                        break;
                    }
                }
            }
        }

        internal static void UpdateScrollMouse(Panel panel, ref int pXYScrollMouse)
        {
            if (panel != GameCanvas.panel)
                return;
            if (!IsCurrentPanelIsSetDoPanel)
                return;
            if (GameCanvas.isMouseFocus(GameCanvas.panel.X, GameCanvas.panel.Y + 50, GameCanvas.panel.W, 28) && pXYScrollMouse != 0)
                pXYScrollMouse = 0;
        }

        internal static void UpdateTouch(Panel panel)
        {
            if (panel != GameCanvas.panel)
                return;
            if (!IsCurrentPanelIsSetDoPanel)
                return;
            if (offset < setDos.Count - 3 && GameCanvas.isPointerHoldIn(GameCanvas.panel.wScroll - 7, 61, 6, 9))
            {
                GameCanvas.isPointerJustDown = false;
                GameScr.gI().isPointerDowning = false;
                if (GameCanvas.isPointerClick)
                {
                    if (GameCanvas.panel.currentTabIndex < Math.Min(4, setDos.Count + 1) - 1)
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
                        string str = "Set đồ " + (string.IsNullOrEmpty(setDos[i + offset].Name) ? i + offset + 1 : setDos[i + offset].Name);
                        GameCanvas.panel.popUpDetailInit(GameCanvas.panel.cp = new ChatPopup(), str);
                        GameCanvas.panel.idIcon = -1;
                        GameCanvas.panel.partID = null;
                        GameCanvas.panel.charInfo = null;
                        GameCanvas.panel.currItem = null;
                        GameCanvas.panel.cp.cy = 30;
                        GameCanvas.menu.disableClose = true;
                        isShowMenu = true;
                        lastTimeSetDisableCloseMenu = mSystem.currentTimeMillis();
                    }
                    GameCanvas.clearAllPointerEvent();
                }
        }

        static void RefreshPanelTabName()
        {
            string[][] tabName = new string[Math.Min(4, setDos.Count + 1)][];
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

        internal static void LoadData()
        {
            try
            {
                setDos = JsonConvert.DeserializeObject<List<SetDo>>(Utils.LoadDataString($"setdo_{Utils.username}_{Utils.server["ip"]}_{Utils.server["port"]}"));
            }
            catch { }
        }

        internal static void SaveData()
        {
            try
            {
                Utils.SaveData($"setdo_{Utils.username}_{Utils.server["ip"]}_{Utils.server["port"]}", JsonConvert.SerializeObject(setDos));
            }
            catch { }
        }

        public void onChatFromMe(string text, string to)
        {
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

        static int getItemCount()
        {
            arrItemBody = Char.myCharz().arrItemBody.Where(i => i != null && i.isTypeBody()).ToArray();
            arrItemBag = Char.myCharz().arrItemBag.Where(i => i != null && i.isTypeBody()).ToArray();
            arrItemBodyPet = Char.myPetz().arrItemBody.Where(i => i != null && i.isTypeBody()).ToArray();
            return arrItemBody.Length + arrItemBag.Length + arrItemBodyPet.Length;
        }

        public void onCancelChat() => GameCanvas.panel.chatTField.ResetTF();

        public void perform(int idAction, object p)
        {
            if (idAction == 1)
            {
                string oldName = string.IsNullOrEmpty(setDos[(int)p].Name) ? ((int)p + 1).ToString() : ("\"" + setDos[(int)p].Name + "\"");
                setDos.RemoveAt((int)p);
                if (offset >= setDos.Count - 3 && offset > 0)
                    offset--;
                if (GameCanvas.panel.currentTabIndex >= Math.Min(4, setDos.Count + 1) - 1 && GameCanvas.panel.currentTabIndex > 0)
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
