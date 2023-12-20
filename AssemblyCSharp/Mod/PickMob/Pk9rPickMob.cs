using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.Menu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.PickMob
{
    public class Pk9rPickMob
    {
        private const int ID_ITEM_GEM = 77;
        private const int ID_ITEM_GEM_LOCK = 861;
        private const int DEFAULT_HP_BUFF = 20;
        private const int DEFAULT_MP_BUFF = 20;
        private static readonly sbyte[] IdSkillsBase = { 0, 2, 17, 4 };
        private static readonly short[] IdItemBlockBase =
            { 225, 353, 354, 355, 356, 357, 358, 359, 360, 362 };

        public static bool IsTanSat = false;
        public static bool IsNeSieuQuai { get; private set; } = true;
        public static bool IsVuotDiaHinh { get; private set; } = true;
        public static bool IsAutoPickItems { get; private set; } = true;
        public static bool IsItemMe { get; private set; } = true;
        public static bool IsLimitTimesPickItem { get; private set; } = true;

        public static List<int> IdMobsTanSat = new List<int>();
        public static List<int> TypeMobsTanSat = new List<int>();
        public static List<sbyte> IdSkillsTanSat = new List<sbyte>(IdSkillsBase);

        public static int TimesAutoPickItemMax = 7;
        public static List<short> IdItemPicks = new List<short>();
        public static List<short> IdItemBlocks = new List<short>(IdItemBlockBase);
        public static List<sbyte> TypeItemPicks = new List<sbyte>();
        public static List<sbyte> TypeItemBlocks = new List<sbyte>();

        [Obsolete("Chức năng này sẽ làm ở 1 class riêng")]
        public static int HpBuff = 0;
        [Obsolete("Chức năng này sẽ làm ở 1 class riêng")]
        public static int MpBuff = 0;
        private static Pk9rPickMob _Instance;
        public static Pk9rPickMob getInstance()
        {
            if (_Instance == null) _Instance = new Pk9rPickMob();
            return _Instance;
        }

        public static void SetAvoindSuperMonster(bool newState) => IsNeSieuQuai = newState;
        public static void SetCrossTerrain(bool newState) => IsVuotDiaHinh = newState;
        public static void SetAutoPickItems(bool newState) => IsAutoPickItems = newState;
        public static void SetAutoPickItemsFromOthers(bool newState) => IsItemMe = newState;
        public static void SetPickUpLimited(bool newState) => IsLimitTimesPickItem = newState;

        [ChatCommand("ts")]
        public static void ToggleSlaughter() => SetSlaughter(!IsTanSat);

        public static void SetSlaughter(bool newState)
        {
            IsTanSat = newState;
            GameScr.info1.addInfo("Tự động đánh quái: " + (IsTanSat ? "Bật" : "Tắt"), 0);
        }


        // Mobs
        [ChatCommand("add")]
        public static void ToggleSelectedMob()
        {
            Mob mob = Char.myCharz().mobFocus;
            ItemMap itemMap = Char.myCharz().itemFocus;

            if (mob != null)
            {
                if (IdMobsTanSat.Contains(mob.mobId))
                {
                    IdMobsTanSat.Remove(mob.mobId);
                    GameScr.info1.addInfo("Đã xoá mob: " + mob.mobId, 0);
                }
                else
                {
                    IdMobsTanSat.Add(mob.mobId);
                    GameScr.info1.addInfo("Đã thêm mob: " + mob.mobId, 0);
                }
            }
            else if (itemMap != null)
            {
                if (IdItemPicks.Contains(itemMap.template.id))
                {
                    IdItemPicks.Remove(itemMap.template.id);
                    GameScr.info1.addInfo($"Đã xoá khỏi danh sách chỉ tự động nhặt item: {itemMap.template.name}[{itemMap.template.id}]", 0);
                }
                else
                {
                    IdItemPicks.Add(itemMap.template.id);
                    GameScr.info1.addInfo($"Đã thêm vào danh sách chỉ tự động nhặt item: {itemMap.template.name}[{itemMap.template.id}]", 0);
                }
            }
            else
            {
                GameScr.info1.addInfo("Cần trỏ vào quái hay vật phẩm cần thêm vào danh sách", 0);
            }
        }

        [ChatCommand("addt")]
        public static void ToggleItemOrMobType()
        {
            Mob mob = Char.myCharz().mobFocus;
            ItemMap itemMap = Char.myCharz().itemFocus;

            if (mob != null)
            {
                if (TypeMobsTanSat.Contains(mob.templateId))
                {
                    TypeMobsTanSat.Remove(mob.templateId);
                    GameScr.info1.addInfo($"Đã xoá loại mob: {mob.getTemplate().name}[{mob.templateId}]", 0);
                }
                else
                {
                    TypeMobsTanSat.Add(mob.templateId);
                    GameScr.info1.addInfo($"Đã thêm loại mob: {mob.getTemplate().name}[{mob.templateId}]", 0);
                }
            }
            else if (itemMap != null)
            {
                if (TypeItemPicks.Contains(itemMap.template.type))
                {
                    TypeItemPicks.Remove(itemMap.template.type);
                    GameScr.info1.addInfo("Đã xoá khỏi danh sách chỉ tự động nhặt loại item:" + itemMap.template.type, 0);
                }
                else
                {
                    TypeItemPicks.Add(itemMap.template.type);
                    GameScr.info1.addInfo("Đã thêm vào danh sách chỉ tự động nhặt loại item:" + itemMap.template.type, 0);
                }
            }
            else
            {
                GameScr.info1.addInfo("Cần trỏ vào quái hay vật phẩm cần thêm vào danh sách", 0);
            }
        }

        [ChatCommand("clrm")]
        public static void ClearMonsterToFightList()
        {
            IdMobsTanSat.Clear();
            TypeMobsTanSat.Clear();
            GameScr.info1.addInfo("Đã xoá danh sách đánh quái", 0);
        }

        [ChatCommand("vdh")]
        public static void ToggleCrossTerrain()
        {
            SetCrossTerrain(!IsVuotDiaHinh);
            GameScr.info1.addInfo("Tự động đánh quái vượt địa hình: " + (IsVuotDiaHinh ? "Bật" : "Tắt"), 0);
        }

        [ChatCommand("nsq")]
        public static void ToggleAvoidSuperMonsters()
        {
            SetAvoindSuperMonster(!IsNeSieuQuai);
            GameScr.info1.addInfo("Tàn sát né siêu quái: " + (IsNeSieuQuai ? "Bật" : "Tắt"), 0);
        }

        // Items
        [ChatCommand("anhat")]
        public static void ToggleAutoPickUpItems()
        {
            SetAutoPickItems(IsAutoPickItems);
            GameScr.info1.addInfo("Tự động nhặt vật phẩm: " + (IsAutoPickItems ? "Bật" : "Tắt"), 0);
        }

        [ChatCommand("itm")]
        public static void ToggleFilterOtherCharItems()
        {
            SetAutoPickItemsFromOthers(!IsItemMe);
            GameScr.info1.addInfo("Lọc không nhặt vật phẩm của người khác: " + (IsItemMe ? "Bật" : "Tắt"), 0);
        }

        [ChatCommand("sln")]
        public static void TogglePickUpLimit()
        {
            SetPickUpLimited(!IsLimitTimesPickItem);
            StringBuilder builder = new StringBuilder();
            builder.Append($"Giới hạn số lần nhặt là ");
            builder.Append(TimesAutoPickItemMax);
            builder.Append(IsLimitTimesPickItem ? ": Bật" : ": Tắt");
            GameScr.info1.addInfo(builder.ToString(), 0);
        }

        [ChatCommand("clri")]
        public static void ResetItemFilterToDefault()
        {
            IdItemPicks.Clear();
            TypeItemPicks.Clear();
            TypeItemBlocks.Clear();
            IdItemBlocks.Clear();
            IdItemBlocks.AddRange(IdItemBlockBase);
            GameScr.info1.addInfo("Danh sách lọc item đã được đặt lại mặc định", 0);
        }

        [ChatCommand("cnn")]
        public static void SetToPickOnlyGems()
        {
            IdItemPicks.Clear();
            TypeItemPicks.Clear();
            TypeItemBlocks.Clear();
            IdItemBlocks.Clear();
            IdItemBlocks.AddRange(IdItemBlockBase);
            IdItemPicks.Add(ID_ITEM_GEM);
            IdItemPicks.Add(ID_ITEM_GEM_LOCK);
            GameScr.info1.addInfo("Đã cài đặt chỉ nhặt ngọc", 0);
        }

        // Skills
        [ChatCommand("skill")]
        public static void ToggleSelectedSkillForSlaughter()
        {
            SkillTemplate template = Char.myCharz().myskill.template;

            if (IdSkillsTanSat.Contains(template.id))
            {
                IdSkillsTanSat.Remove(template.id);
                GameScr.info1.addInfo($"Đã xoá khỏi danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
            }
            else
            {
                IdSkillsTanSat.Add(template.id);
                GameScr.info1.addInfo($"Đã thêm vào danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
            }
        }

        [ChatCommand("clrs")]
        public static void SaveCurrentSkillListAsDefault()
        {
            IdSkillsTanSat.Clear();
            IdSkillsTanSat.AddRange(IdSkillsBase);
            GameScr.info1.addInfo("Đã đặt danh sách skill sử dụng tự động đánh quái về mặc định", 0);
        }

        [ChatCommand("blocki")]
        public static void BlockFocusedItem()
        {
            ItemMap itemMap = Char.myCharz().itemFocus;

            if (itemMap != null)
            {
                if (IdItemBlocks.Contains(itemMap.template.id))
                {
                    IdItemBlocks.Remove(itemMap.template.id);
                    GameScr.info1.addInfo($"Đã xoá khỏi danh sách không tự động nhặt item: {itemMap.template.name}[{itemMap.template.id}]", 0);
                }
                else
                {
                    IdItemBlocks.Add(itemMap.template.id);
                    GameScr.info1.addInfo($"Đã thêm vào danh sách không tự động nhặt item: {itemMap.template.name}[{itemMap.template.id}]", 0);
                }
            }
            else
            {
                GameScr.info1.addInfo("Cần trỏ vào vật phẩm cần chặn khi auto nhặt", 0);
            }
        }

        public static bool Chat(string text)
        {
            if (IsGetInfoChat<int>(text, "sln"))
            {
                TimesAutoPickItemMax = GetInfoChat<int>(text, "sln");
                GameScr.info1.addInfo("Số lần nhặt giới hạn là: " + TimesAutoPickItemMax, 0);
            }
            else if (IsGetInfoChat<short>(text, "addi"))
            {
                short id = GetInfoChat<short>(text, "addi");
                if (IdItemPicks.Contains(id))
                {
                    IdItemPicks.Remove(id);
                    GameScr.info1.addInfo($"Đã xoá khỏi danh sách chỉ tự động nhặt item: {ItemTemplates.get(id).name}[{id}]", 0);
                }
                else
                {
                    IdItemPicks.Add(id);
                    GameScr.info1.addInfo($"Đã thêm vào danh sách chỉ tự động nhặt item: {ItemTemplates.get(id).name}[{id}]", 0);
                }
            }
            else if (IsGetInfoChat<short>(text, "blocki"))
            {
                short id = GetInfoChat<short>(text, "blocki");
                if (IdItemBlocks.Contains(id))
                {
                    IdItemBlocks.Remove(id);
                    GameScr.info1.addInfo($"Đã thêm vào danh sách không tự động nhặt item: {ItemTemplates.get(id).name}[{id}]", 0);
                }
                else
                {
                    IdItemBlocks.Add(id);
                    GameScr.info1.addInfo($"Đã xoá khỏi danh sách không tự động nhặt item: {ItemTemplates.get(id).name}[{id}]", 0);
                }
            }
            else if (IsGetInfoChat<sbyte>(text, "addti"))
            {
                sbyte type = GetInfoChat<sbyte>(text, "addti");
                if (TypeItemPicks.Contains(type))
                {
                    TypeItemPicks.Remove(type);
                    GameScr.info1.addInfo("Đã xoá khỏi danh sách chỉ tự động nhặt loại item: " + type, 0);
                }
                else
                {
                    TypeItemPicks.Add(type);
                    GameScr.info1.addInfo("Đã thêm vào danh sách chỉ tự động nhặt loại item: " + type, 0);
                }
            }
            else if (IsGetInfoChat<sbyte>(text, "blockti"))
            {
                sbyte type = GetInfoChat<sbyte>(text, "blockti");
                if (TypeItemBlocks.Contains(type))
                {
                    TypeItemBlocks.Remove(type);
                    GameScr.info1.addInfo("Đã xoá khỏi danh sách không tự động nhặt loại item: " + type, 0);
                }
                else
                {
                    TypeItemBlocks.Add(type);
                    GameScr.info1.addInfo("Đã thêm vào danh sách không tự động nhặt loại item: " + type, 0);
                }
            }
            else if (IsGetInfoChat<int>(text, "addm"))
            {
                int id = GetInfoChat<int>(text, "addm");
                if (IdMobsTanSat.Contains(id))
                {
                    IdMobsTanSat.Remove(id);
                    GameScr.info1.addInfo("Đã xoá mob: " + id, 0);
                }
                else
                {
                    IdMobsTanSat.Add(id);
                    GameScr.info1.addInfo("Đã thêm mob: " + id, 0);
                }
            }
            else if (IsGetInfoChat<int>(text, "addtm"))
            {
                int id = GetInfoChat<int>(text, "addtm");
                if (TypeMobsTanSat.Contains(id))
                {
                    TypeMobsTanSat.Remove(id);
                    GameScr.info1.addInfo($"Đã xoá loại mob: {Mob.arrMobTemplate[id].name}[{id}]", 0);
                }
                else
                {
                    TypeMobsTanSat.Add(id);
                    GameScr.info1.addInfo($"Đã thêm loại mob: {Mob.arrMobTemplate[id].name}[{id}]", 0);
                }
            }
            else if (IsGetInfoChat<int>(text, "skill"))
            {
                int index = GetInfoChat<int>(text, "skill") - 1;
                SkillTemplate template = Char.myCharz().nClass.skillTemplates[index];
                if (IdSkillsTanSat.Contains(template.id))
                {
                    IdSkillsTanSat.Remove(template.id);
                    GameScr.info1.addInfo($"Đã xoá khỏi danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
                }
                else
                {
                    IdSkillsTanSat.Add(template.id);
                    GameScr.info1.addInfo($"Đã thêm vào danh sách skill sử dụng tự động đánh quái skill: {template.name}[{template.id}]", 0);
                }
            }
            else if (IsGetInfoChat<sbyte>(text, "skillid"))
            {
                sbyte id = GetInfoChat<sbyte>(text, "skillid");
                if (IdSkillsTanSat.Contains(id))
                {
                    IdSkillsTanSat.Remove(id);
                    GameScr.info1.addInfo("Đã xoá khỏi danh sách skill sử dụng tự động đánh quái skill: " + id, 0);
                }
                else
                {
                    IdSkillsTanSat.Add(id);
                    GameScr.info1.addInfo("Đã thêm vào danh sách skill sử dụng tự động đánh quái skill: " + id, 0);
                }
            }
            //else if (text == "abf")
            //{
            //    if (HpBuff == 0 && MpBuff == 0)
            //    {
            //        GameScr.info1.addInfo("Tự động sử dụng đậu thần: Tắt", 0);
            //    }
            //    else
            //    {
            //        HpBuff = DEFAULT_HP_BUFF;
            //        MpBuff = DEFAULT_MP_BUFF;
            //        GameScr.info1.addInfo($"Tự động sử dụng đậu thần khi HP dưới {HpBuff}%, MP dưới {MpBuff}%", 0);
            //    }    
            //}
            //else if (IsGetInfoChat<int>(text, "abf"))
            //{
            //    HpBuff = GetInfoChat<int>(text, "abf");
            //    MpBuff = 0;
            //    GameScr.info1.addInfo($"Tự động sử dụng đậu thần khi HP dưới {HpBuff}%", 0);
            //}
            //else if (IsGetInfoChat<int>(text, "abf", 2))
            //{
            //    int[] vs = GetInfoChat<int>(text, "abf", 2);
            //    HpBuff = vs[0];
            //    MpBuff = vs[1];
            //    GameScr.info1.addInfo($"Tự động sử dụng đậu thần khi HP dưới {HpBuff}%, MP dưới {MpBuff}%", 0);
            //}
            else return false;
            return true;
        }

        [Obsolete("Không cần dùng")]
        public static bool HotKeys()
        {
            switch (GameCanvas.keyAsciiPress)
            {
                case 't':
                    ToggleSlaughter();
                    break;
                case 'n':
                    ToggleAutoPickUpItems();
                    break;
                case 'a':
                    ToggleSelectedMob();
                    break;
                case 'b':
                    Chat("abf");
                    break;
                default:
                    return false;
            }
            return true;
        }

        public static void Update()
        {
            PickMobController.Update();
        }

        public static void MobStartDie(object obj)
        {
            Mob mob = (Mob)obj;
            if (mob.status != 1 && mob.status != 0)
            {
                mob.lastTimeDie = mSystem.currentTimeMillis();
                mob.countDie++;
                if (mob.countDie > 10)
                    mob.countDie = 0;
            }
        }

        public static void UpdateCountDieMob(Mob mob)
        {
            if (mob.levelBoss != 0)
                mob.countDie = 0;
        }

        public static void ShowMenu()
        {
            string menuDesc = string.Empty;
            Mob mobFocus = Char.myCharz().mobFocus;
            ItemMap itemFocus = Char.myCharz().itemFocus;
            string cpDesc = "PickMobNRO by Phucprotein\n";
            if (mobFocus != null || itemFocus != null)
            {
                if (mobFocus != null)
                {
                    cpDesc += $"Quái đang trỏ vào: {mobFocus.getTemplate().name}, số thứ tự: {mobFocus.mobId}, loại: {mobFocus.getTemplate().mobTemplateId}";
                }
                else cpDesc += $"Item đang trỏ vào: {itemFocus.template.name}, id: {itemFocus.template.id}, loại: {itemFocus.template.type}";
            }

            //OpenMenu.start(new(menuItems =>
            //{
            //    if (mobFocus != null || itemFocus != null)
            //    {
            //        if (mobFocus != null)
            //        {
            //            if (IdMobsTanSat.Contains(mobFocus.mobId)) menuDesc = $"Xóa quái id ({mobFocus.mobId}) khỏi danh sách";
            //            else menuDesc = $"Thêm quái id ({mobFocus.mobId}) vào danh sách";
            //        }
            //        else if (itemFocus != null)
            //        {
            //            if (IdItemPicks.Contains(itemFocus.template.id)) menuDesc = $"Xóa item khỏi danh sách";
            //            else menuDesc = $"Thêm item vào danh sách";
            //        }
            //        menuItems.Add(new(menuDesc, new(() =>
            //        {
            //            Chat("add");
            //        })));
            //        if (mobFocus != null)
            //        {
            //            if (TypeMobsTanSat.Contains(mobFocus.templateId)) menuDesc = $"Xóa {mobFocus.getTemplate().name} khỏi danh sách";
            //            else menuDesc = $"Thêm {mobFocus.getTemplate().name} vào danh sách";
            //        }
            //        else if (itemFocus != null)
            //        {
            //            if (TypeItemPicks.Contains(itemFocus.template.type)) menuDesc = $"Xóa loại item ({itemFocus.template.type}) khỏi danh sách";
            //            else menuDesc = $"Thêm loại item ({itemFocus.template.type}) vào danh sách";
            //        }
            //        menuItems.Add(new(menuDesc, new(() =>
            //        {
            //            Chat("addt");
            //        })));
            //        if (itemFocus != null)
            //        {
            //            if (IdItemBlocks.Contains(itemFocus.template.id)) menuDesc = $"Xóa {itemFocus.template.name} khỏi danh sách không nhặt";
            //            else menuDesc = $"Thêm {itemFocus.template.name} vào danh sách không nhặt";
            //            menuItems.Add(new(menuDesc, new(() =>
            //            {
            //                Chat("blocki" + itemFocus.template.id);
            //            })));
            //            if (TypeItemBlocks.Contains(itemFocus.template.type)) menuDesc = $"Xóa loại item ({itemFocus.template.type}) khỏi danh sách không nhặt";
            //            else menuDesc = $"Thêm loại item ({itemFocus.template.type}) vào danh sách không nhặt";
            //            menuItems.Add(new(menuDesc, new(() =>
            //            {
            //                Chat("blockti" + itemFocus.template.type);
            //            })));
            //        }
            //    }
            //    if (IdMobsTanSat.Count + TypeMobsTanSat.Count > 0)
            //        menuItems.Add(new("Xóa danh sách tàn sát", new(() =>
            //        {
            //            Chat("clrm");
            //        })));
            //    SkillTemplate mySkillTemplate = Char.myCharz().myskill.template;
            //    if (IdSkillsTanSat.Contains(mySkillTemplate.id)) menuDesc = $"Xóa {mySkillTemplate.name} khỏi danh sách skill";
            //    else menuDesc = $"Thêm {mySkillTemplate.name} vào danh sách skill";
            //    menuItems.Add(new(menuDesc, new(() =>
            //    {
            //        Chat("skill");
            //    })));
            //    menuItems.Add(new("Đặt lại danh sách skill", new(() =>
            //    {
            //        Chat("clrs");
            //    })));
            //    menuItems.Add(new("Đặt lại danh sách item", new(() =>
            //    {
            //        Chat("clri");
            //    })));
            //    if (IdMobsTanSat.Count + TypeMobsTanSat.Count > 0)
            //        menuItems.Add(new("Xem danh sách quái", new(() =>
            //        {
            //            string infoMob = "Danh sách id quái: ";
            //            if (IdMobsTanSat.Count > 0)
            //            {
            //                foreach (int mobId in IdMobsTanSat)
            //                {
            //                    infoMob += mobId + ", ";
            //                }
            //                infoMob = infoMob.Remove(infoMob.LastIndexOf(','), 2);
            //            }
            //            else infoMob += "Rỗng";
            //            infoMob += "\nDanh sách loại quái: ";
            //            if (TypeMobsTanSat.Count > 0)
            //            {
            //                foreach (int mobType in TypeMobsTanSat)
            //                {
            //                    infoMob += $"{Mob.arrMobTemplate[mobType].name} [{mobType}], ";
            //                }
            //                infoMob = infoMob.Remove(infoMob.LastIndexOf(','), 2);
            //            }
            //            else infoMob += "Rỗng";
            //            GameCanvas.startOKDlg(infoMob);
            //        })));
            //    if (IdItemPicks.Count + TypeItemPicks.Count + IdItemBlocks.Count + TypeItemBlocks.Count > 0)
            //        menuItems.Add(new("Xem danh sách vật phẩm", new(() =>
            //        {
            //            string infoItem = "Danh sách vật phẩm tự động nhặt: ";
            //            if (IdItemPicks.Count > 0)
            //            {
            //                foreach (short itemIdPick in IdItemPicks)
            //                {
            //                    infoItem += $"{ItemTemplates.get(itemIdPick).name} [{itemIdPick}], ";
            //                }
            //                infoItem = infoItem.Remove(infoItem.LastIndexOf(','), 2);
            //            }
            //            else infoItem += "Rỗng";
            //            infoItem += "\nDanh sách loại vật phẩm tự động nhặt: ";
            //            if (TypeItemPicks.Count > 0)
            //            {
            //                foreach (sbyte itemTypePick in TypeItemPicks)
            //                {
            //                    infoItem += itemTypePick + ", ";
            //                }
            //                infoItem = infoItem.Remove(infoItem.LastIndexOf(','), 2);
            //            }
            //            else infoItem += "Rỗng";
            //            infoItem += "\nDanh sách vật phẩm không nhặt: ";
            //            if (IdItemBlocks.Count > 0)
            //            {
            //                foreach (short itemIdBlock in IdItemBlocks)
            //                {
            //                    infoItem += $"{ItemTemplates.get(itemIdBlock).name} [{itemIdBlock}], ";
            //                }
            //                infoItem = infoItem.Remove(infoItem.LastIndexOf(','), 2);
            //            }
            //            else infoItem += "Rỗng";
            //            infoItem += "\nDanh sách loại vật phẩm không nhặt: ";
            //            if (TypeItemBlocks.Count > 0)
            //            {
            //                foreach (sbyte itemTypeBlock in TypeItemBlocks)
            //                {
            //                    infoItem += itemTypeBlock + ", ";
            //                }
            //                infoItem = infoItem.Remove(infoItem.LastIndexOf(','), 2);
            //            }
            //            else infoItem += "Rỗng";
            //            GameCanvas.startOKDlg(infoItem);
            //        })));
            //    menuItems.Add(new("Xem danh sách skill", new(() =>
            //    {
            //        string infoskill = "Danh sách skill tự động đánh quái: ";
            //        foreach (sbyte skillid in IdSkillsTanSat)
            //        {
            //            for (int i = 0; i < 8; i++)
            //            {
            //                SkillTemplate template = Char.myCharz().nClass.skillTemplates[i];
            //                if (template.id == skillid)
            //                {
            //                    infoskill += $"{template.name} [{skillid}]";
            //                    goto here;
            //                }
            //            }
            //        }
            //    here:;
            //        GameCanvas.startOKDlg(infoskill);
            //    })));
            //}), cpDesc);

            //TODO: Code hack não quá tạm chuyển như này trước
            var menuBuilder = new MenuBuilder().setChatPopup(cpDesc);

            if (mobFocus != null || itemFocus != null)
            {
                if (mobFocus != null)
                {
                    if (IdMobsTanSat.Contains(mobFocus.mobId)) menuDesc = $"Xóa quái id ({mobFocus.mobId}) khỏi danh sách";
                    else menuDesc = $"Thêm quái id ({mobFocus.mobId}) vào danh sách";
                }
                else if (itemFocus != null)
                {
                    if (IdItemPicks.Contains(itemFocus.template.id)) menuDesc = $"Xóa item khỏi danh sách";
                    else menuDesc = $"Thêm item vào danh sách";
                }
                menuBuilder.addItem(menuDesc, new(() => ToggleSelectedMob()));
                if (mobFocus != null)
                {
                    if (TypeMobsTanSat.Contains(mobFocus.templateId)) menuDesc = $"Xóa {mobFocus.getTemplate().name} khỏi danh sách";
                    else menuDesc = $"Thêm {mobFocus.getTemplate().name} vào danh sách";
                }
                else if (itemFocus != null)
                {
                    if (TypeItemPicks.Contains(itemFocus.template.type)) menuDesc = $"Xóa loại item ({itemFocus.template.type}) khỏi danh sách";
                    else menuDesc = $"Thêm loại item ({itemFocus.template.type}) vào danh sách";
                }
                menuBuilder.addItem(menuDesc, new(() => ToggleItemOrMobType()));
                if (itemFocus != null)
                {
                    if (IdItemBlocks.Contains(itemFocus.template.id)) menuDesc = $"Xóa {itemFocus.template.name} khỏi danh sách không nhặt";
                    else menuDesc = $"Thêm {itemFocus.template.name} vào danh sách không nhặt";
                    menuBuilder.addItem(menuDesc, new(() =>
                    {
                        Chat("blocki" + itemFocus.template.id);
                    }));
                    if (TypeItemBlocks.Contains(itemFocus.template.type)) menuDesc = $"Xóa loại item ({itemFocus.template.type}) khỏi danh sách không nhặt";
                    else menuDesc = $"Thêm loại item ({itemFocus.template.type}) vào danh sách không nhặt";
                    menuBuilder.addItem(menuDesc, new(() =>
                    {
                        Chat("blockti" + itemFocus.template.type);
                    }));
                }
            }
            if (IdMobsTanSat.Count + TypeMobsTanSat.Count > 0)
                menuBuilder.addItem("Xóa danh sách tàn sát", new(() =>
                {
                    ClearMonsterToFightList();
                }));
            SkillTemplate mySkillTemplate = Char.myCharz().myskill.template;
            if (IdSkillsTanSat.Contains(mySkillTemplate.id)) menuDesc = $"Xóa {mySkillTemplate.name} khỏi danh sách skill";
            else menuDesc = $"Thêm {mySkillTemplate.name} vào danh sách skill";
            menuBuilder.addItem(menuDesc, new(() =>
            {
                ToggleSelectedSkillForSlaughter();
            }));
            menuBuilder.addItem("Đặt lại danh sách skill", new(() =>
            {
                SaveCurrentSkillListAsDefault();
            }));
            menuBuilder.addItem("Đặt lại danh sách item", new(() =>
            {
                ResetItemFilterToDefault();
            }));
            if (IdMobsTanSat.Count + TypeMobsTanSat.Count > 0)
                menuBuilder.addItem("Xem danh sách quái", new(() =>
                {
                    string infoMob = "Danh sách id quái: ";
                    if (IdMobsTanSat.Count > 0)
                    {
                        foreach (int mobId in IdMobsTanSat)
                        {
                            infoMob += mobId + ", ";
                        }
                        infoMob = infoMob.Remove(infoMob.LastIndexOf(','), 2);
                    }
                    else infoMob += "Rỗng";
                    infoMob += "\nDanh sách loại quái: ";
                    if (TypeMobsTanSat.Count > 0)
                    {
                        foreach (int mobType in TypeMobsTanSat)
                        {
                            infoMob += $"{Mob.arrMobTemplate[mobType].name} [{mobType}], ";
                        }
                        infoMob = infoMob.Remove(infoMob.LastIndexOf(','), 2);
                    }
                    else infoMob += "Rỗng";
                    GameCanvas.startOKDlg(infoMob);
                }));
            if (IdItemPicks.Count + TypeItemPicks.Count + IdItemBlocks.Count + TypeItemBlocks.Count > 0)
                menuBuilder.addItem("Xem danh sách vật phẩm", new(() =>
                {
                    string infoItem = "Danh sách vật phẩm tự động nhặt: ";
                    if (IdItemPicks.Count > 0)
                    {
                        foreach (short itemIdPick in IdItemPicks)
                        {
                            infoItem += $"{ItemTemplates.get(itemIdPick).name} [{itemIdPick}], ";
                        }
                        infoItem = infoItem.Remove(infoItem.LastIndexOf(','), 2);
                    }
                    else infoItem += "Rỗng";
                    infoItem += "\nDanh sách loại vật phẩm tự động nhặt: ";
                    if (TypeItemPicks.Count > 0)
                    {
                        foreach (sbyte itemTypePick in TypeItemPicks)
                        {
                            infoItem += itemTypePick + ", ";
                        }
                        infoItem = infoItem.Remove(infoItem.LastIndexOf(','), 2);
                    }
                    else infoItem += "Rỗng";
                    infoItem += "\nDanh sách vật phẩm không nhặt: ";
                    if (IdItemBlocks.Count > 0)
                    {
                        foreach (short itemIdBlock in IdItemBlocks)
                        {
                            infoItem += $"{ItemTemplates.get(itemIdBlock).name} [{itemIdBlock}], ";
                        }
                        infoItem = infoItem.Remove(infoItem.LastIndexOf(','), 2);
                    }
                    else infoItem += "Rỗng";
                    infoItem += "\nDanh sách loại vật phẩm không nhặt: ";
                    if (TypeItemBlocks.Count > 0)
                    {
                        foreach (sbyte itemTypeBlock in TypeItemBlocks)
                        {
                            infoItem += itemTypeBlock + ", ";
                        }
                        infoItem = infoItem.Remove(infoItem.LastIndexOf(','), 2);
                    }
                    else infoItem += "Rỗng";
                    GameCanvas.startOKDlg(infoItem);
                }));
            menuBuilder.addItem("Xem danh sách skill", new(() =>
            {
                string infoskill = "Danh sách skill tự động đánh quái: ";
                foreach (sbyte skillid in IdSkillsTanSat)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        SkillTemplate template = Char.myCharz().nClass.skillTemplates[i];
                        if (template.id == skillid)
                        {
                            infoskill += $"{template.name} [{skillid}]";
                            goto here;
                        }
                    }
                }
                here:
                    GameCanvas.startOKDlg(infoskill);
            }));
            menuBuilder.start();
        }

    #region Không cần liên kết với game
    private static bool IsGetInfoChat<T>(string text, string s)
    {
        if (!text.StartsWith(s))
        {
            return false;
        }
        try
        {
            Convert.ChangeType(text.Substring(s.Length), typeof(T));
        }
        catch
        {
            return false;
        }
        return true;
    }

    private static T GetInfoChat<T>(string text, string s)
    {
        return (T)Convert.ChangeType(text.Substring(s.Length), typeof(T));
    }

    private static bool IsGetInfoChat<T>(string text, string s, int n)
    {
        if (!text.StartsWith(s))
        {
            return false;
        }
        try
        {
            string[] vs = text.Substring(s.Length).Split(' ');
            for (int i = 0; i < n; i++)
            {
                Convert.ChangeType(vs[i], typeof(T));
            }
        }
        catch
        {
            return false;
        }
        return true;
    }

    private static T[] GetInfoChat<T>(string text, string s, int n)
    {
        T[] ts = new T[n];
        string[] vs = text.Substring(s.Length).Split(' ');
        for (int i = 0; i < n; i++)
        {
            ts[i] = (T)Convert.ChangeType(vs[i], typeof(T));
        }
        return ts;
    }
    #endregion
}
}
