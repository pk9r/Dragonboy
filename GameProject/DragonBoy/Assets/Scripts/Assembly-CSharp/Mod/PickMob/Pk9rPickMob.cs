using System;
using System.Collections.Generic;
using System.Linq;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.Menu;
using Mod.R;

namespace Mod.PickMob
{
    internal class Pk9rPickMob
    {
        const int ID_ITEM_GEM = 77;
        const int ID_ITEM_GEM_LOCK = 861;
        const int DEFAULT_HP_BUFF = 20;
        const int DEFAULT_MP_BUFF = 20;
        static readonly sbyte[] IdSkillsBase = { 0, 2, 17, 4 };
        static readonly short[] IdItemBlockBase =
            { 225, 353, 354, 355, 356, 357, 358, 359, 360, 362 };

        internal static bool IsTanSat = false;
        internal static bool IsNeSieuQuai { get; set; } = true;
        internal static bool IsVuotDiaHinh { get; set; } = true;
        internal static bool IsAutoPickItems { get; set; } = true;
        internal static bool IsItemMe { get; set; } = true;
        internal static bool IsLimitTimesPickItem { get; set; } = true;

        internal static List<int> IdMobsTanSat = new List<int>();
        internal static List<int> TypeMobsTanSat = new List<int>();
        internal static List<sbyte> IdSkillsTanSat = new List<sbyte>(IdSkillsBase);

        internal static int TimesAutoPickItemMax = 7;
        internal static List<short> IdItemPicks = new List<short>();
        internal static List<short> IdItemBlocks = new List<short>(IdItemBlockBase);
        internal static List<sbyte> TypeItemPicks = new List<sbyte>();
        internal static List<sbyte> TypeItemBlocks = new List<sbyte>();

        static Pk9rPickMob _Instance;
        internal static Pk9rPickMob getInstance()
        {
            if (_Instance == null)
                _Instance = new Pk9rPickMob();
            return _Instance;
        }

        internal static void SetSlaughter(bool newState) => IsTanSat = newState;
        internal static void SetAvoidSuperMonster(bool newState) => IsNeSieuQuai = newState;
        internal static void SetCrossTerrain(bool newState) => IsVuotDiaHinh = newState;
        internal static void SetAutoPickItems(bool newState) => IsAutoPickItems = newState;
        internal static void SetAutoPickItemsFromOthers(bool newState) => IsItemMe = newState;
        internal static void SetPickUpLimited(bool newState) => IsLimitTimesPickItem = newState;

        [ChatCommand("ts")]
        internal static void ToggleSlaughter()
        {
            SetSlaughter(!IsTanSat);
            GameScr.info1.addInfo(Strings.pickMobTitle + ": " + Strings.OnOffStatus(IsTanSat), 0);
        }

        [ChatCommand("add")]
        internal static void ToggleSelectedMob()
        {
            Mob mob = Char.myCharz().mobFocus;
            ItemMap itemMap = Char.myCharz().itemFocus;
            if (mob != null)
                AddOrRemoveMonsterAutoAttack(mob.mobId);
            else if (itemMap != null)
                AddOrRemoveItemAutoPick(itemMap.template.id);
            else
                GameScr.info1.addInfo(Strings.pickMobPlsFocusOnMonsterOrItem + '!', 0);
        }

        [ChatCommand("addm")]
        internal static void AddOrRemoveMonsterAutoAttack(int mobId)
        {
            if (IdMobsTanSat.Contains(mobId))
            {
                IdMobsTanSat.Remove(mobId);
                GameScr.info1.addInfo(string.Format(Strings.pickMobMonsterRemoved, mobId) + '!', 0);
            }
            else
            {
                IdMobsTanSat.Add(mobId);
                GameScr.info1.addInfo(string.Format(Strings.pickMobMonsterAdded, mobId) + '!', 0);
            }
        }

        [ChatCommand("addi")]
        internal static void AddOrRemoveItemAutoPick(short id)
        {
            if (IdItemPicks.Contains(id))
            {
                IdItemPicks.Remove(id);
                GameScr.info1.addInfo(string.Format(Strings.pickMobAutoPickItemListRemoved, ItemTemplates.get(id).name, id) + '!', 0);
            }
            else
            {
                IdItemPicks.Add(id);
                GameScr.info1.addInfo(string.Format(Strings.pickMobAutoPickItemListAdded, ItemTemplates.get(id).name, id) + '!', 0);
            }
        }

        [ChatCommand("addt")]
        internal static void ToggleItemOrMobType()
        {
            Mob mob = Char.myCharz().mobFocus;
            ItemMap itemMap = Char.myCharz().itemFocus;
            if (mob != null)
                AddOrRemoveMonsterTypeAutoAttack(mob.getTemplate().mobTemplateId);
            else if (itemMap != null)
                AddOrRemoveItemTypeAutoPick(itemMap.template.type);
            else
                GameScr.info1.addInfo(Strings.pickMobPlsFocusOnMonsterOrItem + '!', 0);
        }

        [ChatCommand("addtm")]
        internal static void AddOrRemoveMonsterTypeAutoAttack(sbyte type)
        {
            if (TypeMobsTanSat.Contains(type))
            {
                TypeMobsTanSat.Remove(type);
                GameScr.info1.addInfo(string.Format(Strings.pickMobMonsterTypeRemoved, Mob.arrMobTemplate[type].name, Mob.arrMobTemplate[type].mobTemplateId) + '!', 0);
            }
            else
            {
                TypeMobsTanSat.Add(type);
                GameScr.info1.addInfo(string.Format(Strings.pickMobMonsterTypeAdded, Mob.arrMobTemplate[type].name, Mob.arrMobTemplate[type].mobTemplateId) + '!', 0);
            }
        }

        [ChatCommand("addti")]
        internal static void AddOrRemoveItemTypeAutoPick(sbyte type)
        {
            if (TypeItemPicks.Contains(type))
            {
                TypeItemPicks.Remove(type);
                GameScr.info1.addInfo(string.Format(Strings.pickMobAutoPickItemTypesListRemoved, type) + '!', 0);
            }
            else
            {
                TypeItemPicks.Add(type);
                GameScr.info1.addInfo(string.Format(Strings.pickMobAutoPickItemTypesListAdded, type) + '!', 0);
            }
        }

        [ChatCommand("clrm")]
        internal static void ClearMonsterToFightList()
        {
            IdMobsTanSat.Clear();
            TypeMobsTanSat.Clear();
            GameScr.info1.addInfo(Strings.pickMobMonsterListCleared + '!', 0);
        }

        [ChatCommand("vdh")]
        internal static void ToggleCrossTerrain()
        {
            SetCrossTerrain(!IsVuotDiaHinh);
            GameScr.info1.addInfo(Strings.vdhTitle + ": " + Strings.OnOffStatus(IsVuotDiaHinh), 0);
        }

        [ChatCommand("nsq")]
        internal static void ToggleAvoidSuperMonsters()
        {
            SetAvoidSuperMonster(!IsNeSieuQuai);
            GameScr.info1.addInfo(Strings.avoidSuperMobTitle + ": " + Strings.OnOffStatus(IsNeSieuQuai), 0);
        }

        [ChatCommand("anhat")]
        internal static void ToggleAutoPickUpItems()
        {
            SetAutoPickItems(IsAutoPickItems);
            GameScr.info1.addInfo(Strings.autoPickItemTitle + ": " + Strings.OnOffStatus(IsAutoPickItems), 0);
        }

        [ChatCommand("itm")]
        internal static void ToggleFilterOtherCharItems()
        {
            SetAutoPickItemsFromOthers(!IsItemMe);
            GameScr.info1.addInfo(Strings.pickMyItemOnlyTitle + ": " + Strings.OnOffStatus(IsItemMe), 0);
        }

        [ChatCommand("sln")]
        internal static void TogglePickUpLimit()
        {
            SetPickUpLimited(!IsLimitTimesPickItem);
            GameScr.info1.addInfo(Strings.limitPickTimesTitle + ": " + Strings.OnOffStatus(IsLimitTimesPickItem) + (IsLimitTimesPickItem ? (", " + TimesAutoPickItemMax) : ""), 0);
        }

        [ChatCommand("sln")]
        internal static void SetPickUpLimit(int limit)
        {
            TimesAutoPickItemMax = limit;
            GameScr.info1.addInfo(Strings.limitPickTimesTitle + ": " + TimesAutoPickItemMax, 0);
        }

        [ChatCommand("clri")]
        internal static void ResetItemFilterToDefault()
        {
            IdItemPicks.Clear();
            TypeItemPicks.Clear();
            TypeItemBlocks.Clear();
            IdItemBlocks.Clear();
            IdItemBlocks.AddRange(IdItemBlockBase);
            GameScr.info1.addInfo(Strings.pickMobItemListResetToDefault + '!', 0);
        }

        [ChatCommand("cnn")]
        internal static void SetToPickOnlyGems()
        {
            IdItemPicks.Clear();
            TypeItemPicks.Clear();
            TypeItemBlocks.Clear();
            IdItemBlocks.Clear();
            IdItemBlocks.AddRange(IdItemBlockBase);
            IdItemPicks.Add(ID_ITEM_GEM);
            IdItemPicks.Add(ID_ITEM_GEM_LOCK);
            GameScr.info1.addInfo(Strings.pickMobConfiguredPickGemsOnly + '!', 0);
        }

        [ChatCommand("skill")]
        internal static void ToggleSelectedSkillForSlaughter()
        {
            SkillTemplate template = Char.myCharz().myskill.template;
            AddOrRemoveSkillAutoAttack(template.id);
        }

        [ChatCommand("skill")]
        internal static void AddOrRemoveSkillAutoAttack(int index)
        {
            SkillTemplate template = Char.myCharz().nClass.skillTemplates[index - 1];
            if (IdSkillsTanSat.Contains(template.id))
            {
                IdSkillsTanSat.Remove(template.id);
                GameScr.info1.addInfo(string.Format(Strings.pickMobSkillListRemoved, template.name, template.id) + '!', 0);
            }
            else
            {
                IdSkillsTanSat.Add(template.id);
                GameScr.info1.addInfo(string.Format(Strings.pickMobSkillListAdded, template.name, template.id) + '!', 0);
            }
        }

        [ChatCommand("skillid")]
        internal static void AddOrRemoveSkillAutoAttack(sbyte id)
        {
            SkillTemplate template = Char.myCharz().nClass.skillTemplates.FirstOrDefault(t => t.id == id);
            if (template == null)
                return;
            if (IdSkillsTanSat.Contains(id))
            {
                IdSkillsTanSat.Remove(id);
                GameScr.info1.addInfo(string.Format(Strings.pickMobSkillListRemoved, template.name, template.id) + '!', 0);
            }
            else
            {
                IdSkillsTanSat.Add(id);
                GameScr.info1.addInfo(string.Format(Strings.pickMobSkillListAdded, template.name, template.id) + '!', 0);
            }
        }

        [ChatCommand("clrs")]
        internal static void SaveCurrentSkillListAsDefault()
        {
            IdSkillsTanSat.Clear();
            IdSkillsTanSat.AddRange(IdSkillsBase);
            GameScr.info1.addInfo(Strings.pickMobSkillListResetToDefault + '!', 0);
        }

        [ChatCommand("blocki")]
        internal static void BlockFocusedItem()
        {
            ItemMap itemMap = Char.myCharz().itemFocus;
            if (itemMap != null)
                BlockItem(itemMap.template.id);
            else
                GameScr.info1.addInfo(Strings.pickMobPlsFocusOnItem + '!', 0);
        }

        [ChatCommand("blockti")]
        internal static void BlockFocusedItemType()
        {
            ItemMap itemMap = Char.myCharz().itemFocus;
            if (itemMap != null)
                BlockItemType(itemMap.template.type);
            else
                GameScr.info1.addInfo(Strings.pickMobPlsFocusOnItem + '!', 0);
        }

        [ChatCommand("blocki")]
        internal static void BlockItem(short id)
        {
            if (IdItemBlocks.Contains(id))
            {
                IdItemBlocks.Remove(id);
                GameScr.info1.addInfo(string.Format(Strings.pickMobDontPickItemListAdded, ItemTemplates.get(id).name, id) + '!', 0);
            }
            else
            {
                IdItemBlocks.Add(id);
                GameScr.info1.addInfo(string.Format(Strings.pickMobDontPickItemListRemoved, ItemTemplates.get(id).name, id) + '!', 0);
            }
        }

        [ChatCommand("blockti")]
        internal static void BlockItemType(sbyte type)
        {
            if (TypeItemBlocks.Contains(type))
            {
                TypeItemBlocks.Remove(type);
                GameScr.info1.addInfo(string.Format(Strings.pickMobDontPickItemTypeListRemoved, type) + '!', 0);
            }
            else
            {
                TypeItemBlocks.Add(type);
                GameScr.info1.addInfo(string.Format(Strings.pickMobDontPickItemTypeListAdded, type) + '!', 0);
            }
        }

        [Obsolete("Không cần dùng")]
        internal static bool Chat(string text)
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
        internal static bool HotKeys()
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

        internal static void Update()
        {
            PickMobController.Update();
        }

        internal static void MobStartDie(object obj)
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

        internal static void UpdateCountDieMob(Mob mob)
        {
            if (mob.levelBoss != 0)
                mob.countDie = 0;
        }

        internal static void ShowMenu()
        {
            string menuDesc = string.Empty;
            Mob mobFocus = Char.myCharz().mobFocus;
            ItemMap itemFocus = Char.myCharz().itemFocus;
            string cpDesc = "PickMobNRO by Phucprotein\n";
            if (mobFocus != null || itemFocus != null)
            {
                if (mobFocus != null)
                    cpDesc += string.Format(Strings.pickMobFocusedMob, mobFocus.getTemplate().name, mobFocus.mobId, mobFocus.getTemplate().mobTemplateId);
                else
                    cpDesc += string.Format(Strings.pickMobFocusedItem, itemFocus.template.name, itemFocus.template.id, itemFocus.template.type);
            }
            MenuBuilder menuBuilder = new MenuBuilder().setChatPopup(cpDesc);
            if (mobFocus != null || itemFocus != null)
            {
                if (mobFocus != null)
                {
                    if (IdMobsTanSat.Contains(mobFocus.mobId))
                        menuDesc = string.Format(Strings.pickMobRemoveMobIdFromList, mobFocus.mobId);
                    else
                        menuDesc = string.Format(Strings.pickMobAddMobIdToList, mobFocus.mobId);
                }
                else if (itemFocus != null)
                {
                    if (IdItemPicks.Contains(itemFocus.template.id))
                        menuDesc = string.Format(Strings.pickMobRemoveFromList, itemFocus.template.name);
                    else
                        menuDesc = string.Format(Strings.pickMobAddToList, itemFocus.template.name);
                }
                menuBuilder.addItem(menuDesc, new MenuAction(ToggleSelectedMob));
                if (mobFocus != null)
                {
                    if (TypeMobsTanSat.Contains(mobFocus.getTemplate().mobTemplateId))
                        menuDesc = string.Format(Strings.pickMobRemoveFromList, mobFocus.getTemplate().name);
                    else
                        menuDesc = string.Format(Strings.pickMobAddToList, mobFocus.getTemplate().name);
                }
                else if (itemFocus != null)
                {
                    if (TypeItemPicks.Contains(itemFocus.template.type))
                        menuDesc = string.Format(Strings.pickMobRemoveItemTypeFromList, itemFocus.template.type);
                    else
                        menuDesc = string.Format(Strings.pickMobAddItemTypeToList, itemFocus.template.type);
                }
                menuBuilder.addItem(menuDesc, new MenuAction(ToggleItemOrMobType));
                if (itemFocus != null)
                {
                    if (IdItemBlocks.Contains(itemFocus.template.id))
                        menuDesc = string.Format(Strings.pickMobRemoveFromDontPickList, itemFocus.template.name);
                    else
                        menuDesc = string.Format(Strings.pickMobAddToDontPickList, itemFocus.template.name);
                    menuBuilder.addItem(menuDesc, new MenuAction(() =>
                    {
                        BlockItem(itemFocus.template.id);
                    }));
                    if (TypeItemBlocks.Contains(itemFocus.template.type))
                        menuDesc = string.Format(Strings.pickMobRemoveItemTypeFromDontPickList, itemFocus.template.type);
                    else
                        menuDesc = string.Format(Strings.pickMobAddItemTypeToDontPickList, itemFocus.template.type);
                    menuBuilder.addItem(menuDesc, new MenuAction(() =>
                    {
                        BlockItemType(itemFocus.template.type);
                    }));
                }
            }
            if (IdMobsTanSat.Count + TypeMobsTanSat.Count > 0)
                menuBuilder.addItem(Strings.pickMobClearMonsterList, new MenuAction(() =>
                {
                    ClearMonsterToFightList();
                }));
            SkillTemplate mySkillTemplate = Char.myCharz().myskill.template;
            if (IdSkillsTanSat.Contains(mySkillTemplate.id))
                menuDesc = string.Format(Strings.pickMobRemoveFromSkillList, mySkillTemplate.name);
            else
                menuDesc = string.Format(Strings.pickMobAddToSkillList, mySkillTemplate.name);
            menuBuilder.addItem(menuDesc, new MenuAction(() =>
            {
                ToggleSelectedSkillForSlaughter();
            }));
            menuBuilder.addItem(Strings.pickMobResetSkillListToDefault, new MenuAction(() =>
            {
                SaveCurrentSkillListAsDefault();
            }));
            menuBuilder.addItem(Strings.pickMobResetItemListToDefault, new MenuAction(() =>
            {
                ResetItemFilterToDefault();
            }));
            if (IdMobsTanSat.Count + TypeMobsTanSat.Count > 0)
                menuBuilder.addItem(Strings.pickMobViewMonsterList, new MenuAction(() =>
                {
                    string infoMob = Strings.pickMobMonsterIdList + ": ";
                    if (IdMobsTanSat.Count > 0)
                    {
                        foreach (int mobId in IdMobsTanSat)
                            infoMob += mobId + ", ";
                        infoMob = infoMob.Remove(infoMob.LastIndexOf(','), 2);
                    }
                    else
                        infoMob += Strings.empty;
                    infoMob += $"\n{Strings.pickMobMonsterTypeList}: ";
                    if (TypeMobsTanSat.Count > 0)
                    {
                        foreach (int mobType in TypeMobsTanSat)
                            infoMob += $"{Mob.arrMobTemplate[mobType].name} [{mobType}], ";
                        infoMob = infoMob.Remove(infoMob.LastIndexOf(','), 2);
                    }
                    else
                        infoMob += Strings.empty;
                    GameCanvas.startOKDlg(infoMob);
                }));
            if (IdItemPicks.Count + TypeItemPicks.Count + IdItemBlocks.Count + TypeItemBlocks.Count > 0)
                menuBuilder.addItem(Strings.pickMobViewItemList, new MenuAction(() =>
                {
                    string infoItem = Strings.pickMobAutoPickItemList + ": ";
                    if (IdItemPicks.Count > 0)
                    {
                        foreach (short itemIdPick in IdItemPicks)
                            infoItem += $"{ItemTemplates.get(itemIdPick).name} [{itemIdPick}], ";
                        infoItem = infoItem.Remove(infoItem.LastIndexOf(','), 2);
                    }
                    else
                        infoItem += Strings.empty;
                    infoItem += $"\n{Strings.pickMobAutoPickItemTypeList}: ";
                    if (TypeItemPicks.Count > 0)
                    {
                        foreach (sbyte itemTypePick in TypeItemPicks)
                            infoItem += itemTypePick + ", ";
                        infoItem = infoItem.Remove(infoItem.LastIndexOf(','), 2);
                    }
                    else
                        infoItem += Strings.empty;
                    infoItem += $"\n{Strings.pickMobDontPickItemList}: ";
                    if (IdItemBlocks.Count > 0)
                    {
                        foreach (short itemIdBlock in IdItemBlocks)
                            infoItem += $"{ItemTemplates.get(itemIdBlock).name} [{itemIdBlock}], ";
                        infoItem = infoItem.Remove(infoItem.LastIndexOf(','), 2);
                    }
                    else
                        infoItem += Strings.empty;
                    infoItem += $"\n{Strings.pickMobDontPickItemTypeList}: ";
                    if (TypeItemBlocks.Count > 0)
                    {
                        foreach (sbyte itemTypeBlock in TypeItemBlocks)
                            infoItem += itemTypeBlock + ", ";
                        infoItem = infoItem.Remove(infoItem.LastIndexOf(','), 2);
                    }
                    else 
                        infoItem += Strings.empty;
                    GameCanvas.startOKDlg(infoItem);
                }));
            menuBuilder.addItem(Strings.pickMobViewSkillList, new MenuAction(() =>
            {
                string infoskill = Strings.pickMobSkillList + ": ";
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
        static bool IsGetInfoChat<T>(string text, string s)
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

        static T GetInfoChat<T>(string text, string s)
        {
            return (T)Convert.ChangeType(text.Substring(s.Length), typeof(T));
        }

        static bool IsGetInfoChat<T>(string text, string s, int n)
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

        static T[] GetInfoChat<T>(string text, string s, int n)
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
