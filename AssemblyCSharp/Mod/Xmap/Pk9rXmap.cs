using Mod.ModHelper;
using System.Collections.Generic;

namespace Mod.Xmap
{
    public class Pk9rXmap : ThreadActionUpdate<Pk9rXmap>
    {
        public static bool IsXmapRunning = false;
        public static bool IsMapTransAsXmap = false;
        public static bool IsShowPanelMapTrans = true;
        public static bool IsUseCapsuleNormal = false;
        public static bool IsUseCapsuleVip = true;
        public static int IdMapCapsuleReturn = -1;

        public override int Interval => 500;

        [ChatCommand("csdb")]
        public static void toggleUseCapsuleVip()
        {
            IsUseCapsuleVip = !IsUseCapsuleVip;
            GameScr.info1.addInfo("Sử dụng capsule đặc biệt Xmap: " + (IsUseCapsuleVip ? "Bật" : "Tắt"), 0);
        }

        [ChatCommand("xmp")]
        public static void toggleXmap(int mapId)
        {
            if (IsXmapRunning)
            {
                XmapController.FinishXmap();
                GameScr.info1.addInfo("Đã huỷ Xmap", 0);
            }
            else
            {
                XmapController.StartRunToMapId(mapId);
            }
        }

        [ChatCommand("xmp"), HotkeyCommand('x')]
        public static void toggleXmap()
        {
            if (IsXmapRunning)
            {
                XmapController.FinishXmap();
                GameScr.info1.addInfo("Đã huỷ Xmap", 0);
            }
            else
            {
                XmapController.ShowXmapMenu();
            }
        }

        protected override void update()
        {
            if (XmapData.Instance().IsLoading) XmapData.Instance().Update();
            if (IsXmapRunning) XmapController.Update();
        }

        public static void Info(string text)
        {
            if (IsXmapRunning)
            {
                var keywords = new List<string>
                {
                    "Bạn chưa thể đến khu vực này",
                    "Bang hội phải có từ 5 thành viên mới được tham gia",
                    "Chỉ tiếp các bang hội, miễn tiếp khách vãng lai",
                    "Gia nhập bang hội trên 2 ngày mới được tham gia",
                };

                if (keywords.Contains(text))
                {
                    XmapController.FinishXmap();
                    GameScr.info1.addInfo("Đã huỷ Xmap", 0);
                }
                else if (text.Equals("Có lỗi xảy ra vui lòng thử lại sau."))
                {
                    XmapController.MoveMyChar(XmapUtils.getX(2), XmapUtils.getY(2));
                }
            }
        }

        public static void SelectMapTrans(int selected)
        {
            if (IsMapTransAsXmap)
            {
                XmapController.HideInfoDlg();
                string mapName = GameCanvas.panel.mapNames[selected];
                int idMap = XmapData.GetIdMapFromPanelXmap(mapName);
                XmapController.StartRunToMapId(idMap);
                return;
            }
            XmapController.SaveIdMapCapsuleReturn();
            Service.gI().requestMapSelect(selected);
        }

        public static void ShowPanelMapTrans()
        {
            IsMapTransAsXmap = false;
            if (IsShowPanelMapTrans)
            {
                GameCanvas.panel.setTypeMapTrans();
                GameCanvas.panel.show();
                return;
            }
            IsShowPanelMapTrans = true;
        }

        public static void FixBlackScreen()
        {
            Controller.gI().loadCurrMap(0);
            Service.gI().finishLoadMap();
            Char.isLoadingMap = false;
        }
    }
}
