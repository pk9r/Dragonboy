using System.Linq;

namespace Mod.Auto
{
    internal class AutoSkill
    {
        public static TargetMode targetMode { get; private set; } = TargetMode.None;
        public static bool shouldReviveDeadChars => targetMode != TargetMode.None;

        public static void setReviveTargetMode(int target)
        {
            targetMode = (TargetMode)target;

            if (shouldReviveDeadChars && Char.myCharz().cgender != CharRace.Namek)
            {
                targetMode = TargetMode.None;
                GameScr.info1.addInfo("Can't enable since the char is not Namek", 0);
            }
        }

        public static void Update()
        {
            if (!shouldReviveDeadChars || Utilities.isFrameMultipleOf(30)) return;

            var deadChar = getDeadCharInMap();
            var skillRescure = Char.myCharz().getSkill(new SkillTemplate { id = Ability.Rescue });

            if (deadChar == null || !SkillUtilities.canUseSkill(skillRescure))
                return;

            if (canHealChar(deadChar))
                useSkillOn(deadChar, skillRescure);
            else
                Utilities.buffMe();
        }

        private static bool isValidTarget(Char target)
        {
            return targetMode switch
            {
                TargetMode.Everyone => true,
                TargetMode.OnlyClanMembers => ClanUtilities.isFromMyClan(target),
                TargetMode.OnlyPet => target.IsPet(),
                TargetMode.OnlyMyPet => target.IsPet() && MyChar.petId() == target.charID,
                _ => false,
            };
        }

        private static Char getDeadCharInMap()
        {
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                var @char = (Char)GameScr.vCharInMap.elementAt(i);

                if (isValidTarget(@char) && CharUtilities.isCharDead(@char)) return @char;
            }

            return null;
        }

        private static bool canHealChar(Char @char)
        {
            return @char.cFlag == Char.myCharz().cFlag;
        }

        private static void useSkillOn(Char c, Skill skill)
        {
            Service.gI().selectSkill(skill.template.id);
            Service.gI().sendPlayerAttack(new MyVector(), new MyVector([c]), -1);

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
