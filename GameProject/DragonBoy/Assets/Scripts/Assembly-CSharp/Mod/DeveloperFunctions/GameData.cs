using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Mod.ModHelper;
using Mod.ModHelper.Menu;
using Newtonsoft.Json;
using UnityEngine;

namespace Mod.DeveloperFunctions
{
    internal static class GameData
    {
        internal static void ShowMenu()
        {
            new MenuBuilder()
                .setChatPopup("Select the data type you want to extract.")
                .addItem("Maps", new MenuAction(() => WriteJSONToClipboard(GetDataMaps)))
                .addItem("NPC templates", new MenuAction(() => WriteJSONToClipboard(GetDataNPCTemplates)))
                .addItem("Monster templates", new MenuAction(() => WriteJSONToClipboard(GetDataMonsterTemplates)))
                .addItem("Item templates", new MenuAction(() => WriteJSONToClipboard(GetDataItemTemplates)))
                .addItem("ItemOption templates", new MenuAction(() => WriteJSONToClipboard(GetDataItemOptionTemplates)))
                .addItem("SkillOption templates", new MenuAction(() => WriteJSONToClipboard(GetDataSkillOptionTemplates)))
                .addItem("NClasses", new MenuAction(() => WriteJSONToClipboard(GetDataNClasses)))
                .addItem("Parts", new MenuAction(() => WriteJSONToClipboard(GetDataParts)))
                .addItem("EffectCharPaints", new MenuAction(() => WriteJSONToClipboard(GetDataEffectCharPaints)))
                .addItem("Darts", new MenuAction(() => WriteJSONToClipboard(GetDataDarts)))
                .addItem("ArrowPaints", new MenuAction(() => WriteJSONToClipboard(GetDataArrowPaints)))
                .addItem("SkillPaints", new MenuAction(() => WriteJSONToClipboard(GetDataSkillPaints)))
                .addItem("All types", new MenuAction(() => WriteJSONToClipboard(_ =>
                {
                    Service.gI().updateMap();
                    Thread.Sleep(250);
                    Service.gI().updateItem();
                    Thread.Sleep(250);
                    Service.gI().updateSkill();
                    Thread.Sleep(250);
                    Service.gI().updateData();
                    Thread.Sleep(250);
                    return new
                    {
                        maps = GetDataMaps(false),
                        npcTemplates = GetDataNPCTemplates(false),
                        monsterTemplates = GetDataMonsterTemplates(false),
                        itemTemplates = GetDataItemTemplates(false),
                        itemOptionTemplates = GetDataItemOptionTemplates(false),
                        skillOptionTemplates = GetDataSkillOptionTemplates(false),
                        nClasses = GetDataNClasses(false),
                        parts = GetDataParts(false),
                        effectCharPaints = GetDataEffectCharPaints(false),
                        darts = GetDataDarts(false),
                        arrowPaints = GetDataArrowPaints(false),
                        skillPaints = GetDataSkillPaints(false)
                    };
                })))
                .start();
        }

        static void WriteJSONToClipboard(Func<bool, object> getData)
        {
            new Thread(() =>
            {
                object data = getData(true);
                MainThreadDispatcher.Dispatch(() =>
                {
                    GUIUtility.systemCopyBuffer = JsonConvert.SerializeObject(data, Formatting.Indented);
                    GameCanvas.startOKDlg("JSON data has been copied to the clipboard!");
                });
            })
            { IsBackground = true }.Start();
        }

        static Dictionary<int, string> GetDataMaps(bool isUpdate = true)
        {
            if (isUpdate)
            {
                Service.gI().updateMap();
                Thread.Sleep(1000);
            }
            Dictionary<int, string> maps = new Dictionary<int, string>();
            for (int i = 0; i < TileMap.mapNames.Length; i++)
                maps.Add(i, TileMap.mapNames[i]);
            return maps;
        }

        static NpcTemplate[] GetDataNPCTemplates(bool isUpdate = true)
        {
            if (isUpdate)
            {
                Service.gI().updateMap();
                Thread.Sleep(1000);
            }
            return Npc.arrNpcTemplate;
        }

        static MobTemplate[] GetDataMonsterTemplates(bool isUpdate = true)
        {
            if (isUpdate)
            {
                Service.gI().updateMap();
                Thread.Sleep(1000);
            }
            return Mob.arrMobTemplate;
        }

        static ItemTemplate[] GetDataItemTemplates(bool isUpdate = true)
        {
            if (isUpdate)
            {
                Service.gI().updateItem();
                Thread.Sleep(1000);
            }
            Hashtable h = ItemTemplates.itemTemplates.h;
            ItemTemplate[] itemTemplates = new ItemTemplate[h.Count];
            h.Values.CopyTo(itemTemplates, 0);
            return itemTemplates.OrderBy(i => i.id).ToArray();
        }

        static ItemOptionTemplate[] GetDataItemOptionTemplates(bool isUpdate = true)
        {
            if (isUpdate)
            {
                Service.gI().updateItem();
                Thread.Sleep(1000);
            }
            return GameScr.gI().iOptionTemplates;
        }

        static SkillOptionTemplate[] GetDataSkillOptionTemplates(bool isUpdate = true)
        {
            if (isUpdate)
            {
                Service.gI().updateSkill();
                Thread.Sleep(1000);
            }
            return GameScr.gI().sOptionTemplates;
        }

        static NClass[] GetDataNClasses(bool isUpdate = true)
        {
            if (isUpdate)
            {
                Service.gI().updateSkill();
                Thread.Sleep(1000);
            }
            NClass[] nClasses = new NClass[GameScr.nClasss.Length];
            for (int j = 0; j < nClasses.Length; j++)
            {
                nClasses[j] = GameScr.nClasss[j];
                for (int k = 0; k < nClasses[j].skillTemplates.Length; k++)
                {
                    for (int l = 0; l < nClasses[j].skillTemplates[k].skills.Length; l++)
                        nClasses[j].skillTemplates[k].skills[l].template = null;
                }
            }
            return nClasses;
        }

        static Part[] GetDataParts(bool isUpdate = true)
        {
            if (isUpdate)
            {
                Service.gI().updateData();
                Thread.Sleep(1000);
            }
            return GameScr.parts;
        }

        static EffectCharPaint[] GetDataEffectCharPaints(bool isUpdate = true)
        {
            if (isUpdate)
            {
                Service.gI().updateData();
                Thread.Sleep(1000);
            }
            return GameScr.efs;
        }

        static DartInfo[] GetDataDarts(bool isUpdate = true)
        {
            if (isUpdate)
            {
                Service.gI().updateData();
                Thread.Sleep(1000);
            }
            return GameScr.darts;
        }

        static Arrowpaint[] GetDataArrowPaints(bool isUpdate = true)
        {
            if (isUpdate)
            {
                Service.gI().updateData();
                Thread.Sleep(1000);
            }
            return GameScr.arrs;
        }

        static SkillPaint[] GetDataSkillPaints(bool isUpdate = true)
        {
            if (isUpdate)
            {
                Service.gI().updateData();
                Thread.Sleep(1000);
            }
            return GameScr.sks;
        }
    }
}