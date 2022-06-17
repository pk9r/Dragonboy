using System;

namespace Mod
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ChatCommandAttribute : Attribute
    {
        public string command;

        public ChatCommandAttribute(string command)
        {
            this.command = command;
        }
    }
}