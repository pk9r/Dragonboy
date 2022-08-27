using System;

namespace Mod
{
    public class ChatCommandAttribute : BaseCommandAttribute
    {
        public string command;

        public ChatCommandAttribute(string command)
        {
            this.command = command;
        }
    }
}