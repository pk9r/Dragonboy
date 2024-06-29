namespace Mod.Constants
{
    /// <summary>
    /// Expresses the meaning of <see cref="MobTemplate.type"/>.
    /// </summary>
    internal struct MonsterType
    {
        /// <summary>
        /// Monster that can't move (Wooden dummy, Destron Gas, etc.).
        /// </summary>
        internal static readonly sbyte Stand = 0;

        /// <summary>
        /// Monster that can walk on the ground (Wild boar, Bulon, Steel Robot, etc.).
        /// </summary>
        internal static readonly sbyte Walk = 1;

        /// <summary>
        /// Monster that can fly (Pterosaurs, Flying dragon, Green lizard, Arbee, etc.).
        /// </summary>
        internal static readonly sbyte Fly = 4;
    }
}