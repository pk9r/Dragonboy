//using Mod.ModHelper.CommandMod.Chat;
//using System.Linq;

//namespace Mod.ModHelper.Menu
//{
//    public class OpenMenu : IActionListener
//    {
//        #region Singleton
//        private OpenMenu() { }
//        static OpenMenu() { }
//        public static OpenMenu gI { get; } = new OpenMenu();
//        #endregion

//        public static void start(MenuItemCollection menuItemCollection, string chatPopup = "")
//        {
//            var myVector = getMyVectorStartMenu(menuItemCollection);
//            if (myVector.size() > 0)
//            {
//                GameCanvas.menu.startAt(myVector, 3);
//            }
//            if (!string.IsNullOrEmpty(chatPopup))
//                ChatPopup.addChatPopup(chatPopup, 100000, new Npc(5, 0, -100, 100, 5, Utils.ID_NPC_MOD_FACE));
//        }

//        public static void start(MenuItemCollection menuItemCollection, int x, int y, string chatPopup = "")
//        {
//            var myVector = getMyVectorStartMenu(menuItemCollection);
//            if (myVector.size() > 0)
//            {
//                GameCanvas.menu.startAt(myVector, x, y);
//            }
//            if (!string.IsNullOrEmpty(chatPopup))
//                ChatPopup.addChatPopup(chatPopup, 100000, new Npc(5, 0, -100, 100, 5, Utils.ID_NPC_MOD_FACE));
//        }

//        //public static void start(List<string> captions, MenuAction action)
//        //{
//        //    start(new(menuItems =>
//        //    {
//        //        for (int i = 0; i < captions.Count; i++)
//        //            menuItems.Add(new(captions[i], action));
//        //    }));
//        //}

//        //public static void start(List<string> captions, MenuAction action, int x, int y)
//        //{
//        //    start(new(menuItems =>
//        //    {
//        //        for (int i = 0; i < captions.Count; i++)
//        //            menuItems.Add(new(captions[i], action));
//        //    }), x, y);
//        //}

//        //public static void start(Action<List<MenuItem>> func)
//        //{
//        //    var menuItems = new List<MenuItem>();
//        //    func.Invoke(menuItems);
//        //    start(menuItems);
//        //}

//        //public static List<KeyValuePair<string, Action<int, string, string[]>>> createPairsMenu(Action<List<KeyValuePair<string, Action<int, string, string[]>>>> action)
//        //{
//        //    var pairs = new List<KeyValuePair<string, Action<int, string, string[]>>>();
//        //    action(pairs);
//        //    return pairs;
//        //}

//        public void perform(int idAction, object p)
//        {
//            IdAction id = (IdAction)idAction;
//            switch (id)
//            {
//                case IdAction.None:
//                    break;
//                case IdAction.MenuSelect:
//                    onMenuSelected(p);
//                    break;
//                default:
//                    break;
//            }
//        }

//        private static void onMenuSelected(object p)
//        {
//            var selected = p.getValueProperty<int>("selected");
//            var action = p.getValueProperty<MenuAction>("action");
//            var captions = p.getValueProperty<string[]>("captions");

//            var caption = captions[selected];
//            if (Char.chatPopup != null && Char.chatPopup.c.avatar == Utils.ID_NPC_MOD_FACE)
//                Char.chatPopup = null;
//            action.Invoke(selected, caption, captions);
//        }

//        private static MyVector getMyVectorStartMenu(MenuItemCollection menuItemCollection)
//        {
//            var captions = from menuItem in menuItemCollection.menuItems select menuItem.caption;

//            var myVector = new MyVector();
//            for (int i = 0; i < menuItemCollection.Count; i++)
//            {
//                var menuItem = menuItemCollection[i];
//                myVector.addElement(new Command(menuItem.caption, gI, (int)IdAction.MenuSelect,
//                    new { selected = i, menuItem.action, captions = captions.ToArray() }));
//            }

//            return myVector;
//        }
//    }
//}
