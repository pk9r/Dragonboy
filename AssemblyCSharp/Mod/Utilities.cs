using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Mod
{
    public class Utilities
    {
        public const sbyte ID_SKILL_BUFF = 7;

        private const BindingFlags PUBLIC_STATIC_VOID = BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod;

        public static int speedRun = 8;
        
        [ChatCommand("tdc")]
        [ChatCommand("cspeed")]
        public static void setSpeedRun(int speed)
        {
            speedRun = speed;

            GameScr.info1.addInfo("Tốc độ chạy: " + speed, 0);
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
    }
}