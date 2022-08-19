using System;

namespace Mod.Xmap
{
    public class Pk9rXmap
    {
        public static bool IsXmapRunning = false;
        public static bool IsMapTransAsXmap = false;
        public static bool IsShowPanelMapTrans = true;
        public static bool IsUseCapsuleNormal = false;
        public static bool IsUseCapsuleVip = true;
        public static int IdMapCapsuleReturn = -1;

        public static bool Chat(string text)
        {
            if (text == "xmp")
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
            else if (IsGetInfoChat<int>(text, "xmp"))
            {
                if (IsXmapRunning)
                {
                    XmapController.FinishXmap();
                    GameScr.info1.addInfo("Đã huỷ Xmap", 0);
                }
                else
                {
                    int idMap = GetInfoChat<int>(text, "xmp");
                    XmapController.StartRunToMapId(idMap);
                }
            }
            else if (text == "csdb")
            {
                IsUseCapsuleVip = !IsUseCapsuleVip;
                GameScr.info1.addInfo("Sử dụng capsule đặc biệt Xmap: " + (IsUseCapsuleVip ? "Bật" : "Tắt"), 0);
            }
            else return false;
            return true;
        }

        [HotkeyCommand('x'), HotkeyCommand('c')]
        public static bool HotKeys()
        {
            switch (GameCanvas.keyAsciiPress)
            {
                case 'x':
                    Chat("xmp");
                    break;
                case 'c':
                    Chat("csb");
                    break;
                default:
                    return false;
            }
            return true;
        }

        public static void Update()
        {
            if (XmapData.Instance().IsLoading) XmapData.Instance().Update();
            if (IsXmapRunning) XmapController.Update();
        }

        public static void Info(string text)
        {
            if (IsXmapRunning)
            {
                if (text.Equals("Bạn chưa thể đến khu vực này"))
                {
                    XmapController.FinishXmap();
                    GameScr.info1.addInfo("Đã huỷ Xmap", 0);
                }
                else if (text.Equals("Bang hội phải có từ 5 thành viên mới được tham gia"))
                {
                    XmapController.FinishXmap();
                    GameScr.info1.addInfo("Đã huỷ Xmap", 0);
                }
                else if (text.Equals("Chỉ tiếp các bang hội, miễn tiếp khách vãng lai"))
                {
                    XmapController.FinishXmap();
                    GameScr.info1.addInfo("Đã huỷ Xmap", 0);
                }
                else if (text.Equals("Gia nhập bang hội trên 2 ngày mới được tham gia"))
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

        [Obsolete("Đã thêm")]
        public static bool XoaTauBay(object obj)
        {
            Teleport teleport = (Teleport)obj;
            if (teleport.isMe)
            {
                Char.myCharz().isTeleport = false;
                if (teleport.type == 0)
                {
                    Controller.isStopReadMessage = false;
                    Char.ischangingMap = true;
                }
                Teleport.vTeleport.removeElement(teleport);
                return true;
            }
            return false;
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
        #endregion
    }
}
