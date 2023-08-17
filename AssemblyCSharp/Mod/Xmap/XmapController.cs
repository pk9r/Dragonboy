using LitJson;
using Mod.ModHelper;
using Mod.ModHelper.CommandMod.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Mod.Xmap
{
    public class XmapController : ThreadActionUpdate<XmapController>
    {
        public override int Interval => 100;

        private static int mapEnd;
        private static List<MapNext> way;
        private static int indexWay;
        private static bool isNextMapFailed;

        protected override void update()
        {         
            if (GameCanvas.gameTick % Time.timeScale == 0)
            {
                LogMod.writeLine($"[xmap][dbg] update {mapEnd}");
                if (way == null)
                {
                    if (!isNextMapFailed)
                    {
                        string mapName = TileMap.mapNames[mapEnd];
                        MainThreadDispatcher.dispatcher(() =>
                            GameScr.info1.addInfo($"Đi đến: {mapName}", 0));
                    }

                    LogMod.writeLine($"[xmap][dbg] Đang tạo dữ liệu map");
                    XmapAlgorithm.xmapData = new XmapData();
                    XmapAlgorithm.xmapData.loadLinkMapCapsule();
                    try
                    {
                        way = XmapAlgorithm.findWay(TileMap.mapID, mapEnd);
                        if (way != null)
                            LogMod.writeLine($"[xmap][dbg] bestWay: {JsonMapper.ToJson(way.Select(x => x.to).ToArray())}");
                    }
                    catch (Exception ex)
                    {
                        LogMod.writeLine($"[xmap][err] Lỗi tìm đường đi\n{ex}");
                    }
                    indexWay = 0;

                    if (way == null)
                    {
                        MainThreadDispatcher.dispatcher(() =>
                            GameScr.info1.addInfo("Không thể tìm thấy đường đi", 0));
                        finishXmap();
                        return;
                    }
                }

                if (TileMap.mapID == way[way.Count - 1].to && !Utilities.isMyCharDied())
                {
                    MainThreadDispatcher.dispatcher(() =>
                        GameScr.info1.addInfo("Xmap by Phucprotein", 0));
                    finishXmap();
                    return;
                }

                if (TileMap.mapID == way[indexWay].mapStart)
                {
                    if (Utilities.isMyCharDied())
                    {
                        Service.gI().returnTownFromDead();
                        isNextMapFailed = true;
                        way = null;
                    }
                    else if (Utilities.canNextMap())
                    {
                        MainThreadDispatcher.dispatcher(() =>
                            Pk9rXmap.nextMap(way[indexWay]));
                        LogMod.writeLine($"[xmap][dbg] nextMap: {way[indexWay].to}");
                    }
                    Thread.Sleep(500);
                    return;
                }
                else if (TileMap.mapID == way[indexWay].to)
                {
                    indexWay++;
                    return;
                }
                else
                {
                    isNextMapFailed = true;
                    way = null;
                }
            }
        }

        [ChatCommand("xmp")]
        public static void start(int mapId)
        {
            if (gI.IsActing)
            {
                finishXmap();
                LogMod.writeLine($"[xmap][info] Hủy xmap tới {TileMap.mapNames[mapEnd]} để thực hiện xmap mới");
            }

            mapEnd = mapId;
            gI.toggle(true);
            LogMod.writeLine($"[xmap][info] Bắt đầu xmap tới {TileMap.mapNames[mapEnd]}");
        }

        public static void finishXmap()
        {
            LogMod.writeLine($"[xmap][info] Kết thúc xmap");
            way = null;
            isNextMapFailed = false;
            gI.toggle(false);
        }
    }
}
