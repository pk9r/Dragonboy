using System.Reflection;

namespace Mod
{
    public class HotkeyCommand : BaseCommand
    {
        public char key;
        public string fullCommand;

        [LitJSON.JsonSkip]
        public object[] parameters;

        public void execute()
        {
            this.method.Invoke(null, this.parameters);
        }
    }
}