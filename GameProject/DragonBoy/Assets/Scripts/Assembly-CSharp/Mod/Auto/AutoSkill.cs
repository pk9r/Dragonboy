using System.Collections;
using System.Linq;
using Mod.Constants;
using Mod.R;
using UnityEngine;

namespace Mod.Auto
{
    internal class AutoSkill
    {
        internal static TargetMode targetMode { get; set; } = TargetMode.None;
        internal static bool shouldReviveDeadChars => targetMode != TargetMode.None;

        internal static void setReviveTargetMode(int target)
        {
            targetMode = (TargetMode)target;

            if (shouldReviveDeadChars && Char.myCharz().cgender != CharGender.Namekian)
            {
                targetMode = TargetMode.None;
                GameScr.info1.addInfo(Strings.youAreNotNamekian + '!', 0);
            }
        }

        internal static void Update()
        {
            if (!shouldReviveDeadChars || GameCanvas.gameTick % (30 * Time.timeScale) != 0)
                return;
            var deadChar = getDeadCharInMap();
            var skillRescue = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[2]);
            if (deadChar == null || !skillRescue.CanUse())
                return;
            if (canHealChar(deadChar) && skillRescue.point <= 1)
                useSkillOn(deadChar, skillRescue);
            else
                Utils.buffMe();
        }

        static bool isValidTarget(Char target)
        {
            switch (targetMode)
            {
                case TargetMode.Everyone:
                    return true;
                case TargetMode.OnlyClanMembers:
                    return target.IsFromMyClan();
                case TargetMode.OnlyPet:
                    return target.IsPet();
                case TargetMode.OnlyMyPet:
                    return target.IsPet() && Char.myCharz().GetPetId() == target.charID;
                default:
                    return false;
            }
        }

        static Char getDeadCharInMap()
        {
            int i = 0;
            for (; i < GameScr.vCharInMap.size(); i++)
            {
                var ch = (Char)GameScr.vCharInMap.elementAt(i);
                if (isValidTarget(ch) && ch.IsCharDead()) 
                    return ch;
            }
            if (i == GameScr.vCharInMap.size() && isValidTarget(Char.myCharz()) && Char.myCharz().IsCharDead())
                return Char.myCharz();
            return null;
        }

        static bool canHealChar(Char ch) => ch.cFlag == Char.myCharz().cFlag;

        static void useSkillOn(Char c, Skill skill)
        {
            Service.gI().selectSkill(skill.template.id);
            Service.gI().sendPlayerAttack(new MyVector(), new MyVector(new ArrayList() { c }), -1);
            skill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
        }

        internal enum TargetMode
        {
            None,
            Everyone,
            OnlyClanMembers,
            OnlyPet,
            OnlyMyPet,
        }
    }
}
