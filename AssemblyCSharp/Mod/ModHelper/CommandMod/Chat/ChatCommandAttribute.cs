namespace Mod.ModHelper.CommandMod.Chat
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