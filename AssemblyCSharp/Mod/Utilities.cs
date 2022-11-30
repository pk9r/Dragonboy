using LitJson;
using Mod.Graphics;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using Mod.Xmap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Vietpad.InputMethod;

namespace Mod
{
    public static class Utilities
    {
        public const string ManifestModuleName = "Assembly-CSharp.dll";
        public const string PathChatCommand = @"ModData\chatCommands.json";
        public const string PathChatHistory = @"ModData\chat.txt";
        public const string PathHotkeyCommand = @"ModData\hotkeyCommands.json";

        public const sbyte ID_SKILL_BUFF = 7;
        public const int ID_ICON_ITEM_TDLT = 4387;
        public const short ID_NPC_MOD_FACE = 7333;// Doraemon, TODO: custom npc avatar

        public const int ID_ITEM_CAPSULE_VIP = 194;
        public const int ID_ITEM_CAPSULE_NORMAL = 193;

        public const int ID_MAP_HOME_BASE = 21;
        public const int ID_MAP_TTVT_BASE = 24;

        private const BindingFlags PUBLIC_STATIC_VOID =
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Static |
            BindingFlags.InvokeMethod;

        public static string status = "Đã kết nối";

        public static int speedRun = 8;

        public static Waypoint waypointLeft;
        public static Waypoint waypointMiddle;
        public static Waypoint waypointRight;

        public static string username = "";
        public static string password = "";
        public static JsonData server = null;
        public static JsonData sizeData = null;

        public static int channelSyncKey = -1;

        public static VietKeyHandler vietKeyHandler = new VietKeyHandler();

        #region Get Methods
        /// <summary>
        /// Lấy danh sách các hàm trong theo tên của class.
        /// </summary>
        /// <remarks> Lưu ý:
        /// <list type="bullet">
        /// <item><description>Chỉ lấy các hàm public static void.</description></item>
        /// <item><description>Tên class phải bao gồm cả namespace.</description></item>
        /// </list>
        /// </remarks>
        /// <param name="typeFullName"></param>
        /// <returns>Danh sách các hàm trong class.</returns>
        public static MethodInfo[] getMethods(string typeFullName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .First(x => x.ManifestModule.Name == Utilities.ManifestModuleName)
                .GetTypes().FirstOrDefault(x => x.FullName.ToLower() == typeFullName.ToLower())
                .GetMethods(PUBLIC_STATIC_VOID);
        }

        /// <summary>
        /// Lấy danh sách tất cả các hàm của tệp Assembly-CSharp.dll.
        /// </summary>
        /// <remarks> Lưu ý:
        /// <list type="bullet">
        /// <item><description>Chỉ lấy các hàm public static void.</description></item>
        /// <item><description>Tên class phải bao gồm cả namespace.</description></item>
        /// </list>
        /// </remarks>
        /// <returns>Danh sách các hàm của tệp Assembly-CSharp.dll.</returns>
        public static IEnumerable<MethodInfo> GetMethods()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .First(x => x.ManifestModule.Name == ManifestModuleName)
                .GetTypes().Where(x => x.IsClass)
                .SelectMany(x => x.GetMethods(PUBLIC_STATIC_VOID));
        }
        #endregion

        #region Get info
        /// <summary>
        /// Lấy MyVector chứa nhân vật của người chơi.
        /// </summary>
        /// <returns></returns>
        public static MyVector getMyVectorMe()
        {
            var vMe = new MyVector();
            vMe.addElement(Char.myCharz());
            return vMe;
        }

        /// <summary>
        /// Kiểm tra khả năng sử dụng skill Trị thương vào bản thân.
        /// </summary>
        /// <param name="skillBuff">Skill trị thương.</param>
        /// <returns>true nếu có thể sử dụng skill trị thương vào bản thân.</returns>
        public static bool canBuffMe(out Skill skillBuff)
        {
            skillBuff = Char.myCharz().
                getSkill(new SkillTemplate { id = ID_SKILL_BUFF });

            if (skillBuff == null)
            {
                return false;
            }

            return true;
        }

        public static string getTextPopup(PopUp popUp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < popUp.says.Length; i++)
            {
                stringBuilder.Append(popUp.says[i]);
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString().Trim();
        }

        /// <summary>
        /// Kiểm tra trạng thái sử dụng TĐLT.
        /// </summary>
        /// <returns>true nếu đang sử dụng tự động luyên tập</returns>
        public static bool isUsingTDLT() =>
            ItemTime.isExistItem(ID_ICON_ITEM_TDLT);

        public static int getXWayPoint(Waypoint waypoint)
        {
            return waypoint.maxX < 60 ? 15 :
                waypoint.minX > TileMap.pxw - 60 ? TileMap.pxw - 15 :
                waypoint.minX + 30;
        }

        public static int getYWayPoint(Waypoint waypoint)
        {
            return waypoint.maxY;
        }

        /// <summary>
        /// Sử dụng một item có id là một trong số các id truyền vào.
        /// </summary>
        /// <param name="templatesId">Mảng chứa các id của các item muốn sử dụng.</param>
        /// <returns>true nếu có vật phẩm được sử dụng.</returns>
        public static sbyte getIndexItemBag(params short[] templatesId)
        {
            var myChar = Char.myCharz();
            int length = myChar.arrItemBag.Length;
            for (sbyte i = 0; i < length; i++)
            {
                var item = myChar.arrItemBag[i];
                if (item != null && templatesId.Contains(item.template.id))
                {
                    return i;
                }
            }

            return -1;
        }
        #endregion

        /// <summary>
        /// Dịch chuyển tới npc trong map.
        /// </summary>
        /// <param name="npc">Npc cần dịch chuyển tới</param>
        public static void teleToNpc(Npc npc)
        {
            teleportMyChar(npc.cx, npc.ySd - npc.ySd % 24);
            Char.myCharz().npcFocus = npc;
        }

        public static void requestChangeMap(Waypoint waypoint)
        {
            if (waypoint.isOffline)
            {
                Service.gI().getMapOffline();
                return;
            }

            Service.gI().requestChangeMap();
        }

        public static Waypoint findWaypoint(int idMap)
        {
            Waypoint waypoint;
            string textPopup;
            for (int i = 0; i < TileMap.vGo.size(); i++)
            {
                waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                textPopup = Utilities.getTextPopup(waypoint.popup);
                if (textPopup.Equals(TileMap.mapNames[idMap]))
                {
                    return waypoint;
                }
            }
            return null;
        }

        public static void setWaypointChangeMap(Waypoint waypoint)
        {
            int cMapID = TileMap.mapID;
            var textPopup = getTextPopup(waypoint.popup);

            if (cMapID == 27 && textPopup == "Tường thành 1")
                return;

            if (cMapID == 70 && textPopup == "Vực cấm" ||
                cMapID == 73 && textPopup == "Vực chết" ||
                cMapID == 110 && textPopup == "Rừng tuyết")
            {
                waypointLeft = waypoint;
                return;
            }

            if (((cMapID == 106 || cMapID == 107) && textPopup == "Hang băng") ||
                ((cMapID == 105 || cMapID == 108) && textPopup == "Rừng băng") ||
                (cMapID == 109 && textPopup == "Cánh đồng tuyết"))
            {
                waypointMiddle = waypoint;
                return;
            }

            if (cMapID == 70 && textPopup == "Căn cứ Raspberry")
            {
                waypointRight = waypoint;
                return;
            }

            if (waypoint.maxX < 60)
            {
                waypointLeft = waypoint;
                return;
            }

            if (waypoint.minX > TileMap.pxw - 60)
            {
                waypointRight = waypoint;
                return;
            }

            waypointMiddle = waypoint;
        }

        public static void updateWaypointChangeMap()
        {
            waypointLeft = waypointMiddle = waypointRight = null;

            var vGoSize = TileMap.vGo.size();
            for (int i = 0; i < vGoSize; i++)
            {
                Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                setWaypointChangeMap(waypoint);
            }
        }

        [ChatCommand("tdc"), ChatCommand("cspeed")]
        public static void setSpeedRun(int speed)
        {
            speedRun = speed;

            GameScr.info1.addInfo("Tốc độ chạy: " + speed, 0);
        }

        [ChatCommand("speed")]
        public static void setSpeedGame(float speed)
        {
            Time.timeScale = speed;
            GameScr.info1.addInfo("Tốc độ game: " + speed, 0);
        }

        /// <summary>
		/// Sử dụng skill Trị thương của namec vào bản thân.
		/// </summary>
		[ChatCommand("hsme"), ChatCommand("buffme"), HotkeyCommand('b')]
        public static void buffMe()
        {
            if (!canBuffMe(out Skill skillBuff))
            {
                GameScr.info1.addInfo("Không tìm thấy kỹ năng Trị thương", 0);
                return;
            }

            // Đổi sang skill hồi sinh
            Service.gI().selectSkill(ID_SKILL_BUFF);

            // Tự tấn công vào bản thân
            Service.gI().sendPlayerAttack(new MyVector(), getMyVectorMe(), -1);

            // Trả về skill cũ
            Service.gI().selectSkill(Char.myCharz().myskill.template.id);

            // Đặt thời gian hồi cho skill
            skillBuff.lastTimeUseThisSkill = mSystem.currentTimeMillis();
        }

        /// <summary>
        /// Dịch chuyển tới một toạ độ cụ thể trong map.
        /// </summary>
        /// <param name="x">Toạ độ x.</param>
        /// <param name="y">Toạ độ y.</param>
        public static void teleportMyChar(int x, int y)
        {
            Char.myCharz().currentMovePoint = null;
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();

            if (isUsingTDLT())
                return;

            Char.myCharz().cx = x;
            Char.myCharz().cy = y + 1;
            Service.gI().charMove();
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();
        }

        [HotkeyCommand('n')]
        public static void showMenuTeleNpc()
        {
            int vNpcSize = GameScr.vNpc.size();
            if (vNpcSize == 0)
            {
                GameScr.info1.addInfo("Không có NPC nào", 0);
                return;
            }

            OpenMenu.start(new(menuItems =>
            {
                for (int i = 0; i < vNpcSize; i++)
                {
                    var npc = (Npc)GameScr.vNpc.elementAt(i);
                    var npcName = string.IsNullOrEmpty(npc.template.name.Trim()) ? "(no name)" : npc.template.name;
                    menuItems.Add(new(npcName, new(() => teleToNpc(npc))));
                }
            }));
        }

        [ChatCommand("csb"), HotkeyCommand('c')]
        public static void useCapsule()
        {
            var index = getIndexItemBag(193, 194);
            if (index == -1)
            {
                GameScr.info1.addInfo("Không tìm thấy capsule", 0);
                return;
            }

            Service.gI().useItem(0, 1, index, -1);
        }

        [ChatCommand("bt")]
        public static void usePorata()
        {
            var index = getIndexItemBag(921, 454);
            if (index == -1)
            {
                GameScr.info1.addInfo("Không tìm thấy bông tai", 0);
                return;
            }

            Service.gI().useItem(0, 1, index, -1);
        }

        [ChatCommand("test")]
        public static void test()
        {

        }

        [ChatCommand("skey")]
        public static void syncKey(int channel)
        {
            channelSyncKey = channel;
            if (channel == -1)
            {
                GameScr.info1.addInfo($"Đã tắt đồng bộ phím", 0);
                return;
            }

            GameScr.info1.addInfo($"Đồng bộ phím với kênh {channel}", 0);
        }

        [HotkeyCommand('j')]
        public static void changeMapLeft() => changeMap(waypointLeft);

        [HotkeyCommand('k')]
        public static void changeMapMiddle() => changeMap(waypointMiddle);

        [HotkeyCommand('l')]
        public static void changeMapRight() => changeMap(waypointRight);

        [HotkeyCommand('g')]
        public static void sendGiaoDichToCharFocusing()
        {
            var charFocus = Char.myCharz().charFocus;
            if (charFocus == null)
            {
                GameScr.info1.addInfo("Trỏ vào nhân vật để giao dịch", 0);
                return;
            }

            Service.gI().giaodich(0, charFocus.charID, -1, -1);
            GameScr.info1.addInfo("Đã gửi lời mời giao dịch đến " + charFocus.cName, 0);
        }

        [ChatCommand("k")]
        public static void changeZone(int zone)
        {
            Service.gI().requestChangeZone(zone, -1);
        }

        public static void changeMap(Waypoint waypoint)
        {
            if (waypoint != null)
            {
                teleportMyChar(getXWayPoint(waypoint), getYWayPoint(waypoint));
                requestChangeMap(waypoint);
            }
        }

        public static bool isMeInNRDMap()
        {
            return TileMap.mapID >= 85 && TileMap.mapID <= 91;
        }

        public static void ResetTF()
        {
            ChatTextField.gI().strChat = "Chat";
            ChatTextField.gI().tfChat.name = "chat";
            ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
            ChatTextField.gI().isShow = false;
        }


        public static void saveRMSInt(string name, int value)
        {
            string folder = "Data";
            if (new StackFrame(1).GetMethod().Module != typeof(Utilities).Module)
            {
                folder += "\\" + new StackFrame(1).GetMethod().Module.Name.Replace(".dll", "");
            }
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            FileStream fileStream = new FileStream(folder + "\\" + name, FileMode.Create);
            fileStream.Write(BitConverter.GetBytes(value), 0, 4);
            fileStream.Flush();
            fileStream.Close();
        }

        public static int loadRMSInt(string name)
        {
            string folder = "Data";
            if (new StackFrame(1).GetMethod().Module != typeof(Utilities).Module)
            {
                folder += "\\" + new StackFrame(1).GetMethod().Module.Name.Replace(".dll", "");
            }
            FileStream fileStream = new FileStream(folder + "\\" + name, FileMode.Open);
            byte[] array = new byte[4];
            fileStream.Read(array, 0, 4);
            fileStream.Close();
            return BitConverter.ToInt32(array, 0);
        }

        public static void saveRMSBool(string name, bool status)
        {
            string folder = "Data";
            if (new StackFrame(1).GetMethod().Module != typeof(Utilities).Module)
            {
                folder += "\\" + new StackFrame(1).GetMethod().Module.Name.Replace(".dll", "");
            }
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            FileStream fileStream = new FileStream(folder + "\\" + name, FileMode.Create);
            fileStream.Write(new byte[] { (byte)(status ? 1 : 0) }, 0, 1);
            fileStream.Flush();
            fileStream.Close();
        }

        public static bool loadRMSBool(string name)
        {
            string folder = "Data";
            if (new StackFrame(1).GetMethod().Module != typeof(Utilities).Module)
            {
                folder += "\\" + new StackFrame(1).GetMethod().Module.Name.Replace(".dll", "");
            }
            FileStream fileStream = new FileStream(folder + "\\" + name, FileMode.Open);
            byte[] array = new byte[1];
            fileStream.Read(array, 0, 1);
            fileStream.Close();
            return array[0] == 1;
        }

        public static string loadRMSString(string name)
        {
            string folder = "Data";
            if (new StackFrame(1).GetMethod().Module != typeof(Utilities).Module)
            {
                folder += "\\" + new StackFrame(1).GetMethod().Module.Name.Replace(".dll", "");
            }
            FileStream fileStream = new FileStream(folder + "\\" + name, FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream);
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            fileStream.Close();
            return result;
        }

        public static void saveRMSString(string name, string data)
        {
            string folder = "Data";
            if (new StackFrame(1).GetMethod().Module != typeof(Utilities).Module)
            {
                folder += "\\" + new StackFrame(1).GetMethod().Module.Name.Replace(".dll", "");
            }
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            FileStream fileStream = new FileStream(folder + "\\" + name, FileMode.Create);
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            fileStream.Write(buffer, 0, buffer.Length);
            fileStream.Flush();
            fileStream.Close();
        }

        public static void toVietnamese(ref string str, int inputType, int caresPos, char keyChar)
        {
            if (inputType == TField.INPUT_TYPE_ANY && !str.StartsWith("/")) str = vietKeyHandler.toVietnamese(str, caresPos);
        }

        /// <summary>
        /// Dịch chuyển đến đối tượng trong map
        /// </summary>
        /// <param name="obj">Đối tượng cần dịch chuyển tới</param>
        public static void teleportMyChar(IMapObject obj)
        {
            teleportMyChar(obj.getX(), obj.getY());
        }

        /// <summary>
        /// Dịch chuyển đến vị trí trên mặt đất có hoành độ x
        /// </summary>
        /// <param name="x">Hoành độ</param>
        public static void teleportMyChar(int x)
        {
            teleportMyChar(x, getYGround(x));
        }
        [Obsolete("Không dùng nữa")]
        internal static int getWidth(mFont mfont, string s)
        {
            return 0;
            //if (mfont == mFont.tahoma_7b_red_tiny || mfont == mFont.tahoma_7b_yellow_tiny || mfont == mFont.tahoma_7_blue_tiny || mfont == mFont.tahoma_7_tiny || mfont == mFont.tahoma_7_white_tiny)
            //{
            //    try
            //    {
            //        GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
            //        gUIStyle.fontSize = 13;
            //        return (int)gUIStyle.CalcSize(new GUIContent(s)).x / mGraphics.zoomLevel + 30;
            //    }
            //    catch (Exception)
            //    {
            //        return mfont.getWidthNotExactOf(s);
            //    }
            //}
            //else throw new ArgumentException();
        }

        internal static int getWidth(GUIStyle gUIStyle, string s)
        {
            return (int)gUIStyle.CalcSize(new GUIContent(s)).x / mGraphics.zoomLevel + 30;
        }

        /// <summary>
        /// Lấy tung độ mặt đất từ hoành độ
        /// </summary>
        /// <param name="x">Hoành độ x</param>
        /// <returns>Tung độ y thỏa mãn (x, y) là mặt đất</returns>
        public static int getYGround(int x)
        {
            int y = 50;
            for (int i = 0; i < 30; i++)
            {
                y += 24;
                if (TileMap.tileTypeAt(x, y, 2))
                {
                    if (y % 24 != 0) y -= y % 24;
                    return y;
                }
            }
            return -1;
        }

        public static int getDistance(IMapObject mapObject1, IMapObject mapObject2)
        {
            return Res.distance(mapObject1.getX(), mapObject1.getY(), mapObject2.getX(), mapObject2.getY());
        }

        [HotkeyCommand('w')]
        public static void KhinhCong()
        {
            Char.myCharz().cy -= 50;
            Service.gI().charMove();
        }

        [HotkeyCommand('s')]
        public static void DonTho()
        {
            Char.myCharz().cy += 50;
            Service.gI().charMove();
        }

        [HotkeyCommand('a')]
        public static void DichTrai()
        {
            Char.myCharz().cx -= 50;
            Service.gI().charMove();
        }

        [HotkeyCommand('d')]
        public static void DichPhai()
        {
            Char.myCharz().cx += 50;
            Service.gI().charMove();
        }

        public static short getNRSDId()
        {
            if (isMeInNRDMap()) return (short)(2400 - TileMap.mapID);
            return 0;
        }

        public static bool isMeWearingActivationSet(int idSet)
        {
            int activateCount = 0;
            for (int i = 0; i < 5; i++)
            {
                Item item = Char.myCharz().arrItemBody[i];
                if (item == null) return false;
                if (item.itemOption == null) return false;
                for (int j = 0; j < item.itemOption.Length; j++)
                {
                    if (item.itemOption[j].optionTemplate.id == idSet)
                    {
                        activateCount++;
                        break;
                    }
                }
            }
            return activateCount == 5;
        }

        public static bool isMeWearingTXHSet()
        {
            return Char.myCharz().cgender == 0 && isMeWearingActivationSet(141);
        }

        public static bool isMeWearingCadicSet()
        {
            return Char.myCharz().cgender == 2 && isMeWearingActivationSet(0);  //TODO: Tìm id set Cadic
        }

        public static bool isMeWearingPikkoroDaimaoSet()
        {
            return Char.myCharz().cgender == 1 && isMeWearingActivationSet(0);  //TODO: Tìm id set Pikkoro Daimao
        }

        public static Image createImage(byte[] imageData, int w = -1, int h = -1)
        {
            if (w == -1 && h == -1) throw new ArgumentException("w or h must be assigned!");
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }
            Image image = new Image();
            try
            {
                image.texture.LoadImage(imageData);
                if (w == -1) w = image.texture.width * h / image.texture.height;
                else if (h == -1) h = image.texture.height * w / image.texture.width;
                if (image.texture.width != w || image.texture.height != h) image.texture = TextureScaler.ScaleTexture(image.texture, w, h);
                image.texture.anisoLevel = 0;
                image.texture.filterMode = FilterMode.Point;
                image.texture.mipMapBias = 0f;
                image.texture.wrapMode = TextureWrapMode.Clamp;
                image.w = image.texture.width;
                image.h = image.texture.height;
                image.texture.Apply();
                return image;
            }
            catch (Exception)
            {
                return image;
            }
        }

        public static void DoDoubleClickToObj(IMapObject mapObject)
        {
            typeof(GameScr).GetMethod("doDoubleClickToObj", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod).Invoke(GameScr.gI(), new object[] { mapObject });
        }

        public static T getValueProperty<T>(this object obj, string name)
        {
            return (T)obj.GetType().GetProperty(name).GetValue(obj, null);
        }

        public static bool isMyCharDied()
        {
            Char myChar = Char.myCharz();
            return myChar.statusMe == 14 || myChar.cHP <= 0;
        }

        public static bool hasItemCapsuleVip()
        {
            Item[] items = Char.myCharz().arrItemBag;
            for (int i = 0; i < items.Length; i++)
                if (items[i] != null && items[i].template.id == ID_ITEM_CAPSULE_VIP)
                    return true;
            return false;
        }

        public static bool hasItemCapsuleNormal()
        {
            Item[] items = Char.myCharz().arrItemBag;
            for (int i = 0; i < items.Length; i++)
                if (items[i] != null && items[i].template.id == ID_ITEM_CAPSULE_NORMAL)
                    return true;
            return false;
        }

        public static bool canNextMap()
        {
            return !Char.isLoadingMap && !Char.ischangingMap && !Controller.isStopReadMessage;
        }

        public static int getMapIdFromName(string mapName)
        {
            int offset = Char.myCharz().cgender;
            if (mapName.Equals("Về nhà")) return ID_MAP_HOME_BASE + offset;
            if (mapName.Equals("Trạm tàu vũ trụ")) return ID_MAP_TTVT_BASE + offset;
            if (mapName.Contains("Về chỗ cũ: "))
            {
                mapName = mapName.Replace("Về chỗ cũ: ", "");
                if (TileMap.mapNames[Pk9rXmap.idMapCapsuleReturn].Equals(mapName)) return Pk9rXmap.idMapCapsuleReturn;
                if (mapName.Equals("Rừng đá")) return -1;
            }
            for (int i = 0; i < TileMap.mapNames.Length; i++) if (mapName.Equals(TileMap.mapNames[i])) return i;
            return -1;
        }
    }
}