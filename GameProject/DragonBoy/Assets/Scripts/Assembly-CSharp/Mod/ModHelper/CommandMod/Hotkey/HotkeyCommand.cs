using Newtonsoft.Json;

namespace Mod.ModHelper.CommandMod.Hotkey
{
    public class HotkeyCommand : BaseCommand
    {
        public char key;
        public string fullCommand;

        [JsonIgnore]
        public object[] parameters;

        public void execute()
        {
            method.Invoke(null, parameters);
        }
    }
}