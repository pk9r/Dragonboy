using LitJson;
using Mod.CustomPanel;
using Mod.Graphics;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using Mod.Set;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;
using Vietpad.InputMethod;

namespace Mod
{
    public static class Utilities
    {
        public static readonly string PathAutoChat = @"ModData\autochat.txt";
        public static readonly string PathChatCommand = @"ModData\chatCommands.json";
        public static readonly string PathChatHistory = @"ModData\chat.txt";
        public static readonly string PathHotkeyCommand = @"ModData\hotkeyCommands.json";


        public static readonly sbyte ID_SKILL_BUFF = Ability.Rescue;
        public static readonly short ID_ICON_ITEM_TDLT = 4387;
        public static readonly short ID_NPC_MOD_FACE = 7333;    // Doraemon

        public static readonly short ID_ITEM_CAPSULE_VIP = 194;
        public static readonly short ID_ITEM_CAPSULE_NORMAL = 193;

        public static readonly int ID_MAP_HOME_BASE = 21;
        public static readonly int ID_MAP_LANG_BASE = 7;
        public static readonly int ID_MAP_TTVT_BASE = 24;

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

        public static int mapCapsuleReturn = -1;

        public static System.Random random = new System.Random();

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

            new MenuBuilder()
                .map<Npc>(GameScr.vNpc, npc =>
                {
                    var npcName = string.IsNullOrEmpty(npc.template.name.Trim()) ? "(no name)" : npc.template.name;
                    return new(npcName, new(() => teleToNpc(npc)));
                }).start();

            //OpenMenu.start(new(menuItems =>
            //{
            //    for (int i = 0; i < vNpcSize; i++)
            //    {
            //        var npc = (Npc)GameScr.vNpc.elementAt(i);
            //        var npcName = string.IsNullOrEmpty(npc.template.name.Trim()) ? "(no name)" : npc.template.name;
            //        menuItems.Add(new(npcName, new(() => teleToNpc(npc))));
            //    }
            //}));
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
        [HotkeyCommand('f')]
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

        [HotkeyCommand('m')]
        public static void menuZone()
        {
            Service.gI().openUIZone();
            GameCanvas.panel.setTypeZone();
            GameCanvas.panel.show();
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

        /// <summary>
        /// Khôi phục trạng thái mặc định của <paramref name="tf"/>
        /// </summary>
        /// <param name="tf">ChatTextField cần khôi phục</param>
        public static void ResetTF(this ChatTextField tf)
        {
            tf.strChat = "Chat";
            tf.tfChat.name = "chat";
            tf.to = "";
            tf.tfChat.setIputType(TField.INPUT_TYPE_ANY);
            tf.isShow = false;
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

        public static float loadRMSFloat(string name)
        {
            string folder = "Data";
            if (new StackFrame(1).GetMethod().Module != typeof(Utilities).Module)
                folder += "\\" + new StackFrame(1).GetMethod().Module.Name.Replace(".dll", "");
            FileStream fileStream = new FileStream(folder + "\\" + name, FileMode.Open);
            byte[] array = new byte[4];
            fileStream.Read(array, 0, 4);
            fileStream.Close();
            return BitConverter.ToSingle(array, 0);
        }

        public static void saveRMSFloat(string name, float value)
        {
            string folder = "Data";
            if (new StackFrame(1).GetMethod().Module != typeof(Utilities).Module)
                folder += "\\" + new StackFrame(1).GetMethod().Module.Name.Replace(".dll", "");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            FileStream fileStream = new FileStream(folder + "\\" + name, FileMode.Create);
            fileStream.Write(BitConverter.GetBytes(value), 0, 4);
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

        public static int getWidth(GUIStyle gUIStyle, string s)
        {
            return (int)(gUIStyle.CalcSize(new GUIContent(s)).x * 1.025f / mGraphics.zoomLevel);
        }

        public static int getHeight(GUIStyle gUIStyle, string content)
        {
            return (int)gUIStyle.CalcSize(new GUIContent(content)).y / mGraphics.zoomLevel;
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

        public static bool isMeWearingTXHSet() => Char.myCharz().cgender == 0 && isMeWearingActivationSet(127);
        
        public static bool isMeWearingPikkoroDaimaoSet() => Char.myCharz().cgender == 1 && isMeWearingActivationSet(132);

        public static bool isMeWearingCadicSet() => Char.myCharz().cgender == 2 && isMeWearingActivationSet(134);

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
                if (TileMap.mapNames[mapCapsuleReturn].Equals(mapName)) return mapCapsuleReturn;
                if (mapName.Equals("Rừng đá")) return -1;
            }
            for (int i = 0; i < TileMap.mapNames.Length; i++) if (mapName.Equals(TileMap.mapNames[i])) return i;
            return -1;
        }

        public static int getIdMapHome(int cgender)
        {
            return ID_MAP_HOME_BASE + cgender;
        }

        public static int getIdMapLang(int cgender)
        {
            return ID_MAP_LANG_BASE * cgender;
        }

        public static bool HasStarOption(Item item, out uint star, out uint starE)
        {
            star = 0;
            starE = 0;
            bool result = false;
            if (item.itemOption == null)
                return result;
            if (item.template.type != 0 && item.template.type != 1 && item.template.type != 2 && item.template.type != 3 && item.template.type != 4 && item.template.type != 32)
                return result;
            for (int i = 0; i < item.itemOption.Length; i++)
            {
                if (item.itemOption[i].optionTemplate.id == 102)
                    star = starE = (uint)item.itemOption[i].param;
                if (item.itemOption[i].optionTemplate.id == 107)
                    starE = (uint)item.itemOption[i].param;
            }
            if (starE != 0)
                result = true;
            starE -= star;
            return result;
        }

        public static long GetLastTimePress()
        {
            return (long)typeof(GameCanvas).GetField("lastTimePress", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        }

        /// <summary>
        /// Mô phỏng <see cref="Panel.setType(int)"/> mà không mở lại panel
        /// </summary>
        public static void EmulateSetTypePanel(this Panel panel, int position)
        {
            panel.typeShop = -1;
            panel.W = Panel.WIDTH_PANEL;
            panel.H = GameCanvas.h;
            panel.X = 0;
            panel.Y = 0;
            panel.ITEM_HEIGHT = 24;
            typeof(Panel).GetField("position", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(panel, position);
            if (position == 0)
            {
                panel.xScroll = 2;
                panel.yScroll = 80;
                panel.wScroll = panel.W - 4;
                panel.hScroll = panel.H - 96;
                //panel.cmx = panel.wScroll;
                panel.cmtoX = 0;
                panel.X = 0;
            }
            else if (position == 1)
            {
                panel.wScroll = panel.W - 4;
                panel.xScroll = GameCanvas.w - panel.wScroll;
                panel.yScroll = 80;
                panel.hScroll = panel.H - 96;
                panel.X = panel.xScroll - 2;
                //panel.cmx = -(GameCanvas.w + panel.W);
                panel.cmtoX = GameCanvas.w - panel.W;
            }
            panel.TAB_W = panel.W / 5 - 1;
            if (panel.currentTabName.Length < 5)
                panel.TAB_W += 5;
            panel.startTabPos = panel.xScroll + panel.wScroll / 2 - panel.currentTabName.Length * panel.TAB_W / 2;
            panel.cmyLast = new int[panel.currentTabName.Length];
            int[] lastSelect = new int[panel.currentTabName.Length];
            for (int i = 0; i < panel.currentTabName.Length; i++)
                lastSelect[i] = GameCanvas.isTouch ? (-1) : 0;
            typeof(Panel).GetField("lastSelect", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(panel, lastSelect);
            typeof(Panel).GetField("scroll", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(panel, null);
            panel.lastTabIndex[CustomPanelMenu.TYPE_CUSTOM_PANEL_MENU] = panel.currentTabIndex;
        }

        /// <summary>
        /// Lấy thông tin đầy đủ (gồm tên, chi tiết, level, ...) của <paramref name="item"/>
        /// </summary>
        /// <param name="item">Item cần lấy tên</param>
        /// <returns></returns>
        public static string GetFullInfo(this Item item)
        {
            string text = item.template.name;
            if (item.itemOption != null)
                for (int i = 0; i < item.itemOption.Length; i++)
                    if (item.itemOption[i].optionTemplate.id == 72)
                    {
                        text = text + " [+" + item.itemOption[i].param.ToString() + "]";
                        break;
                    }
            if (item.itemOption != null)
                for (int j = 0; j < item.itemOption.Length; j++)
                    if (item.itemOption[j].optionTemplate.name.StartsWith("$"))
                    {
                        string optionColor = item.itemOption[j].getOptiongColor();
                        if (item.itemOption[j].param == 1)
                            text = text + "\n" + optionColor;
                        if (item.itemOption[j].param == 0)
                            text = text + "\n" + optionColor;
                    }
                    else
                    {
                        string optionString = item.itemOption[j].getOptionString();
                        if (!optionString.Equals(string.Empty) && item.itemOption[j].optionTemplate.id != 72)
                            text = text + "\n" + optionString;
                    }
            if (item.template.strRequire > 1)
                text += "\n" + mResources.pow_request + ": " + item.template.strRequire.ToString();
            return text + "\n" + item.template.description;
        }

        /// <summary>
        /// Lấy hệ của đệ tử bằng cách kiểm tra skill 1
        /// </summary>
        /// <returns></returns>
        public static int GetPetGender()
        {
            string skill1Pet = Char.myPetz().arrPetSkill[0].template.name;
            if (skill1Pet == GameScr.nClasss[0].skillTemplates[0].name)
                return GameScr.nClasss[0].classId;
            if (skill1Pet == GameScr.nClasss[1].skillTemplates[0].name)
                return GameScr.nClasss[1].classId;
            if (skill1Pet == GameScr.nClasss[2].skillTemplates[0].name)
                return GameScr.nClasss[2].classId;
            return 3;
        }

        public static string TrimUntilFit(string str, GUIStyle style, int width)
        {
            int originalWidth = (int)(getWidth(style, str) / 1.025f);
            if (originalWidth > width)
            {
                while (getWidth(style, str + "...") > width)
                    str = str.Remove(str.Length - 1, 1);
                str = str.Trim() + "...";
            }
            return str;
        }

        public static bool HasActivateOption(Item item)
        {
            for (int i = 0; i < item.itemOption.Length; i++)
            {
                if (item.itemOption[i].optionTemplate.id >= 127 && item.itemOption[i].optionTemplate.id <= 144)
                    return true;
            }
            return false;
        }


        public static bool isFrameMultipleOf(int multiple){
          return GameCanvas.gameTick % (multiple * Time.timeScale) == 0;
        }
    }
}
