namespace Mod.Constants
{
    /// <summary>
    /// Expresses the meaning of <see cref="SkillTemplate.type"/>.
    /// </summary>
    internal struct SkillType
    {
        internal static readonly int Unknown = 0;

        /// <summary>
        /// Focus on the target to use the skill.
        /// </summary>
        internal static readonly int AttackFocus = 1;

        /// <summary>
        /// Focus on other people to rescue or heal them.
        /// </summary>
        internal static readonly int Rescue = 2;

        /// <summary>
        /// Skill can be used without any target.
        /// </summary>
        internal static readonly int UseWithoutFocus = 3;

        /// <summary>
        /// The 9th skill.
        /// </summary>
        internal static readonly int NinethSkill = 4;
    }
}