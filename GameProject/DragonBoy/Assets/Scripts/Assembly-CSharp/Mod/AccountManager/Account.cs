using System;

namespace Mod.AccountManager
{
    internal class Account
    {
        internal string Username { get; set; }
        internal string Password { get; set; }
        internal Server Server { get; set; }
        internal string Name { get; set; }
        internal long HP { get; set; }
        internal long MP { get; set; }
        internal long EXP { get; set; }
        internal sbyte Gender { get; set; }
        internal long Gold { get; set; }
        internal long Gem { get; set; }
        internal long Ruby { get; set; }
        internal DateTime LastTimeLogin { get; set; }
        internal bool IsPinned { get; set; }
        internal int Icon { get; set; } = -1;
        internal int PetIcon { get; set; } = -1;
    }
}