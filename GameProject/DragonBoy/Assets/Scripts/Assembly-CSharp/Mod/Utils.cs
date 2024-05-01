using System;
using System.IO;
using System.Linq;
using System.Text;
using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mod
{
    internal static class Utils
    {
        internal static readonly string dataPath = Path.Combine(GetRootDataPath(), "CommonModData");

        internal static readonly string PathAutoChat = Path.Combine(dataPath, "autochat.txt");
        internal static readonly string PathChatCommand = Path.Combine(dataPath, "chatCommands.json");
        internal static readonly string PathChatHistory = Path.Combine(dataPath, "chat.txt");
        internal static readonly string PathHotkeyCommand = Path.Combine(dataPath, "hotkeyCommands.json");

        internal static readonly sbyte ID_SKILL_BUFF = 7;
        internal static readonly short ID_ICON_ITEM_TDLT = 4387;
        internal static readonly short ID_NPC_MOD_FACE = 7333;    // Doraemon

        internal static string status = "Đã kết nối";

        internal static int speedRun = 8;

        internal static Waypoint waypointLeft;
        internal static Waypoint waypointMiddle;
        internal static Waypoint waypointRight;

        internal static string username = "";
        internal static string password = "";
        internal static JObject server = null;
        internal static JObject sizeData = null;

        internal static int channelSyncKey = -1;

        internal static System.Random random = new System.Random();

        /// <summary>
        /// Kiểm tra xem game đang chạy trên Android hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên Android, ngược lại trả về false.</returns>
        internal static bool IsAndroidBuild() => Application.platform == RuntimePlatform.Android;

        /// <summary>
        /// Kiểm tra xem game đang chạy trên Linux hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên Linux, ngược lại trả về false.</returns>
        internal static bool IsLinuxBuild() => Application.platform == RuntimePlatform.LinuxPlayer;

        /// <summary>
        /// Kiểm tra xem game đang chạy trên Windows hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên Windows, ngược lại trả về false.</returns>
        internal static bool IsWindowsBuild() => Application.platform == RuntimePlatform.WindowsPlayer;

        /// <summary>
        /// Kiểm tra xem game có đang chạy trên Unity Editor hay không.
        /// </summary>
        /// <returns>Trả về true nếu game đang chạy trên Editor, ngược lại trả về false.</returns>
        internal static bool IsEditor() => Application.isEditor;

        /// <summary>
        /// Kiểm tra xem game đang chạy trên điện thoại hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên điện thoại, ngược lại trả về false.</returns>
        internal static bool IsMobile() => IsAndroidBuild() || Application.platform == RuntimePlatform.IPhonePlayer;

        /// <summary>
        /// Kiểm tra xem game đang chạy trên PC hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên PC, ngược lại trả về false.</returns>
        internal static bool IsPC() => !IsMobile();

        internal static void CheckBackButtonPress()
        {
            if (GameCanvas.panel != null || GameCanvas.panel2 != null)
            {
                if (GameCanvas.panel != null && GameCanvas.panel.isShow)
                {
                    GameCanvas.panel.hide();
                    return;
                }
                if (GameCanvas.panel2 != null && GameCanvas.panel2.isShow)
                {
                    GameCanvas.panel2.hide();
                    return;
                }
            }
            if (InfoDlg.isShow)
                return;
            if (GameCanvas.currentDialog != null && GameCanvas.currentDialog is MsgDlg)
            {
                GameCanvas.endDlg();
                return;
            }
            if (ChatTextField.gI().isShow)
            {
                ChatTextField.gI().close();
                return;
            }
            if (GameCanvas.menu.showMenu)
            {
                GameCanvas.menu.closeMenu();
                return;
            }
            GameCanvas.checkBackButton();
        }

        #region Get info
        /// <summary>
        /// Lấy MyVector chứa nhân vật của người chơi.
        /// </summary>
        /// <returns></returns>
        internal static MyVector getMyVectorMe()
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
        internal static bool canBuffMe(out Skill skillBuff)
        {
            skillBuff = Char.myCharz().
                getSkill(new SkillTemplate { id = ID_SKILL_BUFF });

            if (skillBuff == null)
            {
                return false;
            }

            return true;
        }

        internal static string getTextPopup(PopUp popUp)
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
        /// <returns>true nếu đang sử dụng tự động luyện tập</returns>
        internal static bool isUsingTDLT() =>
            ItemTime.isExistItem(ID_ICON_ITEM_TDLT);

        internal static int getXWayPoint(Waypoint waypoint)
        {
            return waypoint.maxX < 60 ? 15 :
                waypoint.minX > TileMap.pxw - 60 ? TileMap.pxw - 15 :
                waypoint.minX + ((waypoint.maxX - waypoint.minX) / 2);
        }

        internal static int getYWayPoint(Waypoint waypoint)
        {
            return waypoint.maxY;
        }

        /// <summary>
        /// Sử dụng một item có id là một trong số các id truyền vào.
        /// </summary>
        /// <param name="templatesId">Mảng chứa các id của các item muốn sử dụng.</param>
        /// <returns>true nếu có vật phẩm được sử dụng.</returns>
        internal static sbyte getIndexItemBag(params short[] templatesId)
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
        internal static void teleToNpc(Npc npc)
        {
            TeleportMyChar(npc.cx, npc.ySd - npc.ySd % 24);
            Char.myCharz().npcFocus = npc;
        }

        internal static void requestChangeMap(Waypoint waypoint)
        {
            if (waypoint.isOffline)
            {
                Service.gI().getMapOffline();
                return;
            }

            Service.gI().requestChangeMap();
        }

        internal static void setWaypointChangeMap(Waypoint waypoint)
        {
            int cMapID = TileMap.mapID;
            var textPopup = getTextPopup(waypoint.popup);

            if (cMapID == 27 && textPopup == TileMap.mapNames[53])
                return;

            if (cMapID == 70 && textPopup == TileMap.mapNames[69] ||
                cMapID == 73 && textPopup == TileMap.mapNames[67] ||
                cMapID == 110 && textPopup == TileMap.mapNames[106])
            {
                waypointLeft = waypoint;
                return;
            }

            if (((cMapID == 106 || cMapID == 107) && textPopup == TileMap.mapNames[110]) ||
                ((cMapID == 105 || cMapID == 108) && textPopup == TileMap.mapNames[109]) ||
                (cMapID == 109 && textPopup == TileMap.mapNames[105]))
            {
                waypointMiddle = waypoint;
                return;
            }

            if (cMapID == 70 && textPopup == TileMap.mapNames[71])
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

        internal static void UpdateWaypointChangeMap()
        {
            waypointLeft = waypointMiddle = waypointRight = null;

            if (TileMap.mapID == 46)
                waypointRight = new Waypoint(570, 576, 570, 576, true, false, TileMap.mapNames[47]);

            var vGoSize = TileMap.vGo.size();
            if (vGoSize == 0)
            {
                if (TileMap.mapID == 45)
                    waypointMiddle = new Waypoint(570, 576, 570, 576, true, false, TileMap.mapNames[46]);
            }
            for (int i = 0; i < vGoSize; i++)
            {
                Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                setWaypointChangeMap(waypoint);
            }
        }

        [ChatCommand("tdc"), ChatCommand("cspeed")]
        internal static void setSpeedRun(int speed)
        {
            speedRun = speed;

            GameScr.info1.addInfo("Tốc độ chạy: " + speed, 0);
        }

        [ChatCommand("speed")]
        internal static void setSpeedGame(float speed)
        {
            Time.timeScale = speed;
            GameScr.info1.addInfo("Tốc độ game: " + speed, 0);
        }

        /// <summary>
		/// Sử dụng skill Trị thương của namec vào bản thân.
		/// </summary>
		[ChatCommand("hsme"), ChatCommand("buffme"), HotkeyCommand('b')]
        internal static void buffMe()
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
        internal static void TeleportMyChar(int x, int y)
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
        internal static void showMenuTeleNpc()
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
        internal static void useCapsule()
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
        internal static void usePorata()
        {
            var index = getIndexItemBag(921, 454);
            if (index == -1)
            {
                GameScr.info1.addInfo("Không tìm thấy bông tai", 0);
                return;
            }

            Service.gI().useItem(0, 1, index, -1);
        }

        [ChatCommand("skey")]
        internal static void syncKey(int channel)
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
        internal static void ChangeMapLeft()
        {
            if (IsMeInNRDMap() || waypointLeft == null)
                TeleportMyChar(60);
            else 
                ChangeMap(waypointLeft);
        }

        [HotkeyCommand('k')]
        internal static void ChangeMapMiddle()
        {
            if (IsMeInNRDMap())
            {
                if (Char.myCharz().bag >= 0 && ClanImage.idImages.containsKey(Char.myCharz().bag.ToString()))
                {
                    ClanImage clanImage = (ClanImage)ClanImage.idImages.get(Char.myCharz().bag.ToString());
                    if (clanImage.idImage != null)
                    {
                        for (int i = 0; i < clanImage.idImage.Length; i++)
                        {
                            if (clanImage.idImage[i] == 2322)
                            {
                                for (int j = 0; j < GameScr.vNpc.size(); j++)
                                {
                                    Npc npc = (Npc)GameScr.vNpc.elementAt(j);
                                    if (npc.template.npcTemplateId >= 30 && npc.template.npcTemplateId <= 36)
                                    {
                                        Char.myCharz().npcFocus = npc;
                                        TeleportMyChar(npc.cx, npc.cy - 3);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                for (int k = 0; k < GameScr.vItemMap.size(); k++)
                {
                    ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(k);
                    if (itemMap != null && itemMap.IsNRD())
                    {
                        Char.myCharz().itemFocus = itemMap;
                        TeleportMyChar(itemMap.x, itemMap.y);
                        return;
                    }
                }
            }
            else if (waypointMiddle == null)
                TeleportMyChar(TileMap.pxw / 2);
            else 
                ChangeMap(waypointMiddle);
        }

        [HotkeyCommand('l')]
        internal static void ChangeMapRight()
        {
            if (IsMeInNRDMap() || waypointRight == null)
                TeleportMyChar(TileMap.pxw - 60);
            else 
                ChangeMap(waypointRight);
        }

        [HotkeyCommand('g')]
        internal static void sendGiaoDichToCharFocusing()
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
        internal static void changeZone(int zone)
        {
            Service.gI().requestChangeZone(zone, -1);
        }

        [HotkeyCommand('m')]
        internal static void menuZone()
        {
            Service.gI().openUIZone();
            GameCanvas.panel.setTypeZone();
            GameCanvas.panel.show();
        }

        internal static void ChangeMap(Waypoint waypoint)
        {
            if (waypoint != null)
            {
                TeleportMyChar(getXWayPoint(waypoint), getYWayPoint(waypoint));
                requestChangeMap(waypoint);
            }
        }

        internal static bool IsMeInNRDMap() => TileMap.mapID >= 85 && TileMap.mapID <= 91;

        internal static int LoadDataInt(string name, bool isCommon = true)
        {
            string path = dataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Open);
            byte[] array = new byte[4];
            fileStream.Read(array, 0, 4);
            fileStream.Close();
            return BitConverter.ToInt32(array, 0);
        }

        internal static bool LoadDataBool(string name, bool isCommon = true)
        {
            string path = dataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Open);
            byte[] array = new byte[1];
            fileStream.Read(array, 0, 1);
            fileStream.Close();
            return array[0] == 1;
        }

        internal static string LoadDataString(string name, bool isCommon = true)
        {
            string path = dataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Open); 
            StreamReader streamReader = new StreamReader(fileStream);
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            fileStream.Close();
            return result;
        }

        internal static float LoadDataFloat(string name, bool isCommon = true)
        {
            string path = dataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Open); 
            byte[] array = new byte[4];
            fileStream.Read(array, 0, 4);
            fileStream.Close();
            return BitConverter.ToSingle(array, 0);
        }

        internal static bool TryLoadDataInt(string name, out int value, bool isCommon = true)
        {
            value = default;
            try
            {
                value = LoadDataInt(name, isCommon);
                return true;
            }
            catch (Exception ex) { Debug.LogException(ex); }
            return false;
        }

        internal static bool TryLoadDataBool(string name, out bool value, bool isCommon = true)
        {
            value = default;
            try
            {
                value = LoadDataBool(name, isCommon);
                return true;
            }
            catch (Exception ex) { Debug.LogException(ex); }
            return false; 
        }

        internal static bool TryLoadDataString(string name, out string value, bool isCommon = true)
        {
            value = default;
            try
            {
                value = LoadDataString(name, isCommon);
                return true;
            }
            catch (Exception ex) { Debug.LogException(ex); }
            return false;
        }

        internal static bool TryLoadDataFloat(string name, out float value, bool isCommon = true)
        {
            value = default;
            try
            {
                value = LoadDataFloat(name, isCommon);
                return true;
            }
            catch (Exception ex) { Debug.LogException(ex); }
            return false;
        }

        internal static void SaveData(string name, int value, bool isCommon = true)
        {
            string path = dataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Create);
            fileStream.Write(BitConverter.GetBytes(value), 0, 4);
            fileStream.Flush();
            fileStream.Close();
        }

        internal static void SaveData(string name, bool status, bool isCommon = true)
        {
            string path = dataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Create);
            fileStream.Write(new byte[] { (byte)(status ? 1 : 0) }, 0, 1);
            fileStream.Flush();
            fileStream.Close();
        }

        internal static void SaveData(string name, string data, bool isCommon = true)
        {
            string path = dataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Create);
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            fileStream.Write(buffer, 0, buffer.Length);
            fileStream.Flush();
            fileStream.Close();
        }

        internal static void SaveData(string name, float value, bool isCommon = true)
        {
            string path = dataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Create);
            fileStream.Write(BitConverter.GetBytes(value), 0, 4);
            fileStream.Flush();
            fileStream.Close();
        }

        /// <summary>
        /// Dịch chuyển đến đối tượng trong map
        /// </summary>
        /// <param name="obj">Đối tượng cần dịch chuyển tới</param>
        internal static void TeleportMyChar(IMapObject obj)
        {
            TeleportMyChar(obj.getX(), obj.getY());
        }

        /// <summary>
        /// Dịch chuyển đến vị trí trên mặt đất có hoành độ x
        /// </summary>
        /// <param name="x">Hoành độ</param>
        internal static void TeleportMyChar(int x)
        {
            TeleportMyChar(x, GetYGround(x));
        }

        internal static int getWidth(GUIStyle gUIStyle, string s)
        {
            return (int)(gUIStyle.CalcSize(new GUIContent(s)).x * 1.025f / mGraphics.zoomLevel);
        }

        internal static int getHeight(GUIStyle gUIStyle, string content)
        {
            return (int)gUIStyle.CalcSize(new GUIContent(content)).y / mGraphics.zoomLevel;
        }

        /// <summary>
        /// Lấy tung độ mặt đất từ hoành độ
        /// </summary>
        /// <param name="x">Hoành độ x</param>
        /// <returns>Tung độ y thỏa mãn (x, y) là mặt đất</returns>
        internal static int GetYGround(int x)
        {
            int y = 50;
            for (int i = 0; i < 30; i++)
            {
                y += 24;
                if (TileMap.tileTypeAt(x, y, 2))
                {
                    if (y % 24 != 0)
                        y -= y % 24;
                    break;
                }
            }
            return y;
        }

        internal static int getDistance(IMapObject mapObject1, IMapObject mapObject2)
        {
            return Res.distance(mapObject1.getX(), mapObject1.getY(), mapObject2.getX(), mapObject2.getY());
        }

        [HotkeyCommand('w')]
        internal static void KhinhCong()
        {
            Char.myCharz().cy -= 50;
            Service.gI().charMove();
        }

        [HotkeyCommand('s')]
        internal static void DonTho()
        {
            Char.myCharz().cy += 50;
            Service.gI().charMove();
        }

        [HotkeyCommand('a')]
        internal static void DichTrai()
        {
            Char.myCharz().cx -= 50;
            Service.gI().charMove();
        }

        [HotkeyCommand('d')]
        internal static void DichPhai()
        {
            Char.myCharz().cx += 50;
            Service.gI().charMove();
        }

        internal static short getNRSDId()
        {
            if (IsMeInNRDMap()) return (short)(2400 - TileMap.mapID);
            return 0;
        }

        internal static bool isMeWearingActivationSet(int idSet)
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

        internal static bool isMeWearingTXHSet() => Char.myCharz().cgender == 0 && isMeWearingActivationSet(127);

        internal static bool isMeWearingPikkoroDaimaoSet() => Char.myCharz().cgender == 1 && isMeWearingActivationSet(132);

        internal static bool isMeWearingCadicSet() => Char.myCharz().cgender == 2 && isMeWearingActivationSet(134);

        internal static void DoDoubleClickToObj(IMapObject mapObject) => GameScr.gI().doDoubleClickToObj(mapObject);

        internal static bool canNextMap()
        {
            return !Char.isLoadingMap && !Char.ischangingMap && !Controller.isStopReadMessage;
        }

        internal static bool HasStarOption(Item item, out uint star, out uint starE)
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

        internal static long GetLastTimePress()
        {
            return GameCanvas.lastTimePress;
        }

        /// <summary>
        /// Lấy hệ của đệ tử bằng cách kiểm tra skill 1
        /// </summary>
        /// <returns></returns>
        internal static int GetPetGender()
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

        internal static string TrimUntilFit(string str, GUIStyle style, int width)
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

        internal static bool HasActivateOption(Item item)
        {
            if (item.itemOption == null)
                return false;
            for (int i = 0; i < item.itemOption.Length; i++)
            {
                if (item.itemOption[i].optionTemplate.id >= 127 && item.itemOption[i].optionTemplate.id <= 144)
                    return true;
            }
            return false;
        }

        internal static Char FindCharInMap(string name)
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char ch = (Char)GameScr.vCharInMap.elementAt(i);
                if (ch.getNameWithoutClanTag() == name)
                    return ch;
            }
            return null;
        }

        internal static bool IsMyCharHome() => TileMap.mapID == Char.myCharz().cgender + 21;

        internal static string FormatWithSIPrefix(double number)
        {
            string[] prefix = new string[] { "", "k", "M", "B", "T" };
            int degree = Math.Max(0, Math.Min((int)Math.Floor(Math.Log10(Math.Abs(number)) / 3), prefix.Length - 1));
            double scaled = number * Math.Pow(1000, -degree);
            return $"{scaled:0.##}{prefix[degree]}";
        }

        internal static void ResetTextField(ChatTextField chatTextField)
        {
            if (chatTextField == null)
                return;
            chatTextField.left = new Command(mResources.OK, chatTextField, 8000, null, 1, GameCanvas.h - mScreen.cmdH + 1);
            chatTextField.right = new Command(mResources.DELETE, chatTextField, 8001, null, GameCanvas.w - 70, GameCanvas.h - mScreen.cmdH + 1);
            chatTextField.center = null;
            chatTextField.w = chatTextField.tfChat.width + 20;
            chatTextField.h = chatTextField.tfChat.height + 26;
            chatTextField.x = GameCanvas.w / 2 - chatTextField.w / 2;
            chatTextField.tfChat.y = GameCanvas.h - 40 - chatTextField.tfChat.height;
            chatTextField.y = chatTextField.tfChat.y - 18;
            if (Main.isPC && chatTextField.w > 320)
                chatTextField.w = 320;
            chatTextField.left.x = chatTextField.x;
            chatTextField.right.x = chatTextField.x + chatTextField.w - 68;
            if (GameCanvas.isTouch)
            {
                //tfChat.y -= 5;
                chatTextField.y -= 15;
                chatTextField.h += 30;
                chatTextField.left.x = GameCanvas.w / 2 - 68 - 5;
                chatTextField.right.x = GameCanvas.w / 2 + 5;
                chatTextField.left.y = GameCanvas.h - 30;
                chatTextField.right.y = GameCanvas.h - 30;
            }
            chatTextField.yBegin = chatTextField.tfChat.y;
            chatTextField.yUp = GameCanvas.h / 2 - 2 * chatTextField.tfChat.height;
            if (Main.isWindowsPhone)
                chatTextField.tfChat.showSubTextField = false;
            if (Main.isIPhone)
                chatTextField.tfChat.isPaintMouse = false;
            chatTextField.tfChat.name = "chat";
            if (Main.isWindowsPhone)
                chatTextField.tfChat.strInfo = chatTextField.tfChat.name;
            chatTextField.tfChat.width = GameCanvas.w - 6;
            if (Main.isPC && chatTextField.tfChat.width > 250)
                chatTextField.tfChat.width = 250;
            chatTextField.tfChat.height = mScreen.ITEM_HEIGHT + 2;
            chatTextField.tfChat.x = GameCanvas.w / 2 - chatTextField.tfChat.width / 2;
            chatTextField.tfChat.isFocus = true;
            chatTextField.tfChat.setMaxTextLenght(80);
        }

        internal static string GetRootDataPath()
        {
            string result = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Data");
            if (IsEditor() || IsAndroidBuild())
                result = Application.persistentDataPath;
            return result;
        }
    }
}