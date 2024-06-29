namespace Mod.Constants
{
    /// <summary>
    /// Expresses the meaning of <see cref="SkillTemplate.manaUseType"/>.
    /// </summary>
    internal struct SkillManaUseType
    {
        /// <summary>
        /// MP consumed per attack is a fixed value.
        /// </summary>
        internal static readonly int Normal = 0;

        /// <summary>
        /// MP consumed per attack is a percentage of the total MP.
        /// </summary>
        internal static readonly int ByPercent = 1;

        /// <summary>
        /// Consumes all MP for one attack. The level of MP must be 100%.
        /// </summary>
        internal static readonly int Use100Percent = 2;
    }
}