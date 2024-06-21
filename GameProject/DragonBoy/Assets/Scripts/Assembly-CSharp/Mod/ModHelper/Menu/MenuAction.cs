using System;

namespace Mod.ModHelper.Menu
{
    public class MenuAction
    {
        public Action<int, string, string[]> action;

        public MenuAction(Action<int, string, string[]> action)
        {
            this.action = action;
        }

        public MenuAction(Action<int, string> action)
        {
            this.action = (selected, caption, _) => action.Invoke(selected, caption);
        }

        public MenuAction(Action<int> action)
        {
            this.action = (selected, _, _) => action.Invoke(selected);
        }

        public MenuAction(Action action)
        {
            this.action = (_, _, _) => action.Invoke();
        }

        public void Invoke(int selected, string caption, string[] captions)
        {
            action.Invoke(selected, caption, captions);
        }
    }
}
