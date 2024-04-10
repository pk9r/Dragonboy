namespace Mod.ModHelper.Menu
{
    public class MenuItem
    {
        public string caption;
        public MenuAction action;

        public MenuItem(string caption, MenuAction action)
        {
            this.caption = caption;
            this.action = action;
        }
    }
}
