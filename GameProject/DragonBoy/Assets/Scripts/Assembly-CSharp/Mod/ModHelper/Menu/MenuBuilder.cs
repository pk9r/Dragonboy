using Mod.ModHelper.CommandMod.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.ModHelper.Menu
{
    public class MenuBuilder : IActionListener
    {
        #region Example
        //[ChatCommand("example1OpenMenu")]
        public static void example1OpenMenu()
        {
            new MenuBuilder()
                // selected: vị trí menu được chọn
                // caption: chuỗi được chọn, vd: selected là 1 thì giá trị biến này là "test b"
                // captions: mảng tạo menu, ["test a", "test b", "test c"]
                // thay đổi tên biến tùy ý hoặc thay bằng _ nếu không sử dụng
                .addItem("test", new((selected, caption, captions) =>
                {
                    GameScr.info1.addInfo($"selected {selected}, {caption}", 0);
                }))
                .addItem("Test 1", new(() => // without name params
                {
                    GameScr.info1.addInfo("meow meow", 0);
                }))
                .addItem("Test 2", new(() => GameScr.info1.addInfo("gau gau", 0))) // inline
                .start(); // call start to open menu

            //OpenMenu.start(new(menuItems =>
            //{
            //    // selected: vị trí menu được chọn
            //    // caption: chuỗi được chọn, vd: selected là 1 thì giá trị biến này là "test b"
            //    // captions: mảng tạo menu, ["test a", "test b", "test c"]
            //    // thay đổi tên biến tùy ý hoặc thay bằng _ nếu không sử dụng
            //    menuItems.Add(new("test", new((selected, caption, captions) =>
            //    {
            //        GameScr.info1.addInfo($"selected {selected}, {caption}", 0);
            //    })));
            //    // without name params
            //    menuItems.Add(new("Test 1", new(() =>
            //    {
            //        GameScr.info1.addInfo("meow meow", 0);
            //    })));
            //    // inline
            //    menuItems.Add(new("Test 2", new(() => GameScr.info1.addInfo("gau gau", 0))));
            //}));
        }
        #endregion

        private string chatPopup;

        private bool isPosDefault = true;

        public int x, y;

        public List<MenuItem> menuItems = new();

        public MenuBuilder setChatPopup(string chatPopup)
        {
            this.chatPopup = chatPopup;
            return this;
        }

        //public MenuBuilder setPosDefault()
        //{
        //    isPosDefault = true;
        //    return this;
        //}

        public MenuBuilder setPos(int x, int y)
        {
            isPosDefault = false;
            this.x = x;
            this.y = y;
            return this;
        }

        //public MenuBuilder addItem(string caption, Action action)
        //{
        //    return addItem(caption, new(action));
        //}

        public MenuBuilder addItem(string caption, MenuAction action)
        {
            menuItems.Add(new(caption, action));
            return this;
        }

        public MenuBuilder addItem(bool ifCondition, string caption, MenuAction action)
        {
            if (ifCondition)
                menuItems.Add(new(caption, action));
            return this;
        }

        public MenuBuilder map<T>(MyVector myVector, Func<T ,MenuItem> func)
        {
            for (int i = 0; i < myVector.size(); i++)
            {
                var item = (T)myVector.elementAt(i);
                menuItems.Add(func.Invoke(item));
            }
            return this;
        }

        public MenuBuilder map<T>(IEnumerable<T> values, Func<T, MenuItem> func)
        {
            foreach (var item in values)
            {
                menuItems.Add(func.Invoke(item));
            }
            return this;
        }

        public void start()
        {
            var myVector = getMyVectorStartMenu();
            if (myVector.size() > 0)
            {
                if (isPosDefault)
                    GameCanvas.menu.startAt(myVector, 3);
                else
                    GameCanvas.menu.startAt(myVector, x, y);
            }
            if (!string.IsNullOrEmpty(chatPopup))
                ChatPopup.addChatPopup(chatPopup, 100000, new Npc(5, 0, -100, 100, 5, Utils.ID_NPC_MOD_FACE));
        }

        private MyVector getMyVectorStartMenu()
        {
            var captions = from menuItem in menuItems select menuItem.caption;

            var myVector = new MyVector();
            for (int i = 0; i < menuItems.Count; i++)
            {
                var menuItem = menuItems[i];
                myVector.addElement(new Command(menuItem.caption, this, (int)IdAction.MenuSelect,
                    p: new { selected = i, menuItem.action, captions = captions.ToArray() }));
            }

            return myVector;
        }

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
            if (Char.chatPopup != null && Char.chatPopup.c.avatar == Utils.ID_NPC_MOD_FACE)
                Char.chatPopup = null;
            action.Invoke(selected, caption, captions);
        }
    }
}
