using System.Linq;

namespace Mod.ModHelper.Menu
{
    public class OpenMenu : IActionListener
    {
        #region Singleton
        private OpenMenu() { }
        static OpenMenu() { }
        public static OpenMenu gI { get; } = new OpenMenu();
        #endregion

        [ChatCommand("example1OpenMenu")]
        public static void example1OpenMenu()
        {
            OpenMenu.start(new(menuItems =>
            {
                // selected: vị trí menu được chọn
                // caption: chuỗi được chọn, vd: selected là 1 thì giá trị biến này là "test b"
                // captions: mảng tạo menu, ["test a", "test b", "test c"]
                // thay đổi tên biến tùy ý (đảm bảo đủ 3 biến) hoặc thay bằng _ nếu không sử dụng
                menuItems.Add(new("test", new((selected, caption, captions) =>
                {
                    GameScr.info1.addInfo($"selected {selected}, {caption}", 0);
                })));
                // without name params
                menuItems.Add(new("Test 1", new(() =>
                {
                    GameScr.info1.addInfo("meow meow", 0);
                })));
                // inline
                menuItems.Add(new("Test 2", new(() => GameScr.info1.addInfo("gau gau", 0))));
            }));
        }

        public static void start(MenuItemCollection menuItemCollection)
        {
            var myVector = getMyVectorStartMenu(menuItemCollection);
            if (myVector.size() > 0)
            {
                GameCanvas.menu.startAt(myVector, 3);
            }
        }

        public static void start(MenuItemCollection menuItemCollection, int x, int y)
        {
            var myVector = getMyVectorStartMenu(menuItemCollection);
            if (myVector.size() > 0)
            {
                GameCanvas.menu.startAt(myVector, x, y);
            }
        }

        //public static void start(List<string> captions, MenuAction action)
        //{
        //    start(new(menuItems =>
        //    {
        //        for (int i = 0; i < captions.Count; i++)
        //            menuItems.Add(new(captions[i], action));
        //    }));
        //}

        //public static void start(List<string> captions, MenuAction action, int x, int y)
        //{
        //    start(new(menuItems =>
        //    {
        //        for (int i = 0; i < captions.Count; i++)
        //            menuItems.Add(new(captions[i], action));
        //    }), x, y);
        //}

        //public static void start(Action<List<MenuItem>> func)
        //{
        //    var menuItems = new List<MenuItem>();
        //    func.Invoke(menuItems);
        //    start(menuItems);
        //}

        //public static List<KeyValuePair<string, Action<int, string, string[]>>> createPairsMenu(Action<List<KeyValuePair<string, Action<int, string, string[]>>>> action)
        //{
        //    var pairs = new List<KeyValuePair<string, Action<int, string, string[]>>>();
        //    action(pairs);
        //    return pairs;
        //}

        public void perform(int idAction, object p)
        {
            IdAction id = (IdAction)idAction;
            switch (id)
            {
                case IdAction.None:
                    break;
                case IdAction.MenuSelect:
                    onMenuSelected(p);
                    break;
                default:
                    break;
            }
        }

        private static void onMenuSelected(object p)
        {
            var selected = p.getValueProperty<int>("selected");
            var action = p.getValueProperty<MenuAction>("action");
            var captions = p.getValueProperty<string[]>("captions");

            var caption = captions[selected];
            action.Invoke(selected, caption, captions);
        }

        private static MyVector getMyVectorStartMenu(MenuItemCollection menuItemCollection)
        {
            var captions = from menuItem in menuItemCollection.menuItems select menuItem.caption;

            var myVector = new MyVector();
            for (int i = 0; i < menuItemCollection.Count; i++)
            {
                var menuItem = menuItemCollection[i];
                myVector.addElement(new Command(menuItem.caption, gI, (int)IdAction.MenuSelect,
                    new { selected = i, menuItem.action, captions = captions.ToArray() }));
            }

            return myVector;
        }
    }
}
