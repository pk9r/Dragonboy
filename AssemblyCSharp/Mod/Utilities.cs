using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Mod
{
    public class Utilities : IActionListener
    {
        public const sbyte ID_SKILL_BUFF = 7;
        public const int ID_ICON_ITEM_TDLT = 4387;
        
        private const BindingFlags PUBLIC_STATIC_VOID = 
            BindingFlags.Public | 
            BindingFlags.Static | 
            BindingFlags.InvokeMethod;

        #region Singleton
        private Utilities() { }
        static Utilities() { }
        public static Utilities gI { get; } = new Utilities(); 
        #endregion

        public static int speedRun = 8;
        
        [ChatCommand("tdc")]
        [ChatCommand("cspeed")]
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
		/// Sử dụng skill Trị thương của namec vào bản thân
		/// </summary>
		[ChatCommand("hsme")]
		[ChatCommand("buffme")]
        [HotkeyCommand('b')]
        public static void buffMe()
        {
            if (!canBuffMe(out Skill skillBuff))
            {
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
                GameScr.info1.addInfo("Không tìm thấy kỹ năng Trị thương", 0);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Dịch chuyển tới một toạ độ cụ thể trong map.
        /// </summary>
        /// <param name="x">Toạ độ x.</param>
        /// <param name="y">Toạ độ y.</param>
        public static void teleportMyChar(int x, int y)
        {
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
            if (GameScr.vNpc.size() == 0)
            {
                GameScr.info1.addInfo("Không có npc nào", 0);
                return;
            }

            MyVector myVector = new MyVector();
            for (int i = 0; i < GameScr.vNpc.size(); i++)
            {
                var npc = (Npc)GameScr.vNpc.elementAt(i);
                myVector.addElement(new Command(npc.template.name,
                    gI, (int)IdAction.MoveToNpc, npc));
            }
            GameCanvas.menu.startAt(myVector, 3);
        }

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
        /// Kiểm tra trạng thái sử dụng TĐLT.
        /// </summary>
        /// <returns>true nếu đang sử dụng tự động luyên tập</returns>
        public static bool isUsingTDLT() =>
            ItemTime.isExistItem(ID_ICON_ITEM_TDLT);

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
                            .First(x => x.ManifestModule.Name == Properties.Resources.ManifestModuleName)
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
                .First           (x => x.ManifestModule.Name == Properties.Resources.ManifestModuleName)
                .GetTypes().Where(x => x.IsClass)
                .SelectMany      (x => x.GetMethods(PUBLIC_STATIC_VOID));
        }

        public void perform(int idAction, object p)
        {
            IdAction id = (IdAction)idAction;
            switch (id)
            {
                case IdAction.None:
                    break;
                case IdAction.MoveToNpc:
                    moveToNpc((Npc)p);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Dịch chuyển tới npc trong map.
        /// </summary>
        /// <param name="npc">Npc cần dịch chuyển tới</param>
        private static void moveToNpc(Npc npc)
        {
            teleportMyChar(npc.cx, npc.ySd - npc.ySd % 24);
            Char.myCharz().npcFocus = npc;
        }
        [ChatCommand("csb")]
        public static void useCapsule()
        {
            try
            {
                for (sbyte b = 0; b < Char.myCharz().arrItemBag.Length; b = (sbyte)(b + 1))
                {
                    if (Char.myCharz().arrItemBag[b].template.id == 193 || Char.myCharz().arrItemBag[b].template.id == 194)
                    {
                        Service.gI().useItem(0, 1, b, -1);
                        break;
                    }
                }
            }
            catch
            {
            }
        }
        [ChatCommand("bt")]
        public static void usePorata()
        {
            try
            {
                for (sbyte b = 0; b < Char.myCharz().arrItemBag.Length; b = (sbyte)(b + 1))
                {
                    if (Char.myCharz().arrItemBag[b].template.id == 921 || Char.myCharz().arrItemBag[b].template.id == 454)
                    {
                        Service.gI().useItem(0, 1, b, -1);
                        //GameScr.info1.addInfo(b.ToString(), 0);
                        break;
                    }
                }
            }
            catch
            {
            }
        }
    }
}