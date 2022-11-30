namespace Mod.ModHelper.CommandMod.Hotkey
{
    public class HotkeyCommand : BaseCommand
    {
        public char key;
        public string fullCommand;

        [LitJSON.JsonSkip]
        public object[] parameters;

        public void execute()
        {
            method.Invoke(null, parameters);
        }
    }
}