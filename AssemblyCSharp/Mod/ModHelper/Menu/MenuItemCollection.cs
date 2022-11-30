using System;
using System.Collections.Generic;

namespace Mod.ModHelper.Menu
{
    public class MenuItemCollection
    {
        public List<MenuItem> menuItems;

        public MenuItemCollection(Action<List<MenuItem>> action)
        {
            menuItems = new();
            action.Invoke(menuItems);
        }

        public int Count => menuItems.Count;

        public MenuItem this[int index] => menuItems[index];
    }
}
