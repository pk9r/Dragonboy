using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mod
{
    public class Utilities
    {
        [ChatCommand("tdc")]
        public static void editSpeedRun(int speed)
        {
            Char.myCharz().cspeed = speed;

            GameScr.info1.addInfo("Tốc độ chạy: " + speed, 0);
        }

        /// <summary>
		/// Sử dụng skill Trị thương của namec vào bản thân
		/// </summary>
		[ChatCommand("hsme")]
        public static void buffMe()
        {
            sbyte idSkill = Char.myCharz().myskill.template.id;

            SkillTemplate skillTemplate = new();
            skillTemplate.id = 7;
            Skill skill = Char.myCharz().getSkill(skillTemplate);

            Service.gI().selectSkill(skillTemplate.id);

            MyVector vMe = new();
            vMe.addElement(Char.myCharz());

            Service.gI().sendPlayerAttack(new MyVector(), vMe, -1);

            skill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
            Service.gI().selectSkill(idSkill);
        }

        public static void AddKeyMap(Hashtable h)
        {
            h.Add(KeyCode.Slash, 47);
        }

        public static void AddHotkeys()
        {
            if (GameCanvas.keyAsciiPress == '/')
            {
                ChatTextField.gI().startChat('/', GameScr.gI(), string.Empty);
            }
        }
    }

}
