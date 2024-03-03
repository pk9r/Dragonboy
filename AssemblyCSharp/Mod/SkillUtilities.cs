namespace Mod
{
    internal class SkillUtilities
    {
        public static bool canUseSkill(Skill skill)
        {
            var now = mSystem.currentTimeMillis();
            var isOutOfCooldown = now - skill.lastTimeUseThisSkill > skill.coolDown;

            return isOutOfCooldown && hasManaToUseSkill(skill);
        }

        public static bool hasManaToUseSkill(Skill skill)
        {
            return skill.template.manaUseType switch
            {
                ManaUseType.Percent => Char.myCharz().cMP >= Char.myCharz().cMPFull * (skill.manaUse / 100),
                _ => false,
            };
        }

        public struct ManaUseType
        {
            public const sbyte Percent = 1;
        }
    }
}
