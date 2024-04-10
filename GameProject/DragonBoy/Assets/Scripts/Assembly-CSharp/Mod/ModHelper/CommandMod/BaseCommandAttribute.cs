using System;

namespace Mod.ModHelper.CommandMod
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class BaseCommandAttribute : Attribute
    {
        public char delimiter = ' ';
    }
}
