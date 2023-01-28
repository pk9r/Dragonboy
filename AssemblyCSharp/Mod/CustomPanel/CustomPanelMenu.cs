using Mod.Auto;
using Mod.Graphics;
using Mod.ModMenu;
using System;
using System.Reflection;
using UnityEngine;

namespace Mod.CustomPanel
{
    public class CustomPanelMenu
    {
        public static readonly int TYPE_CUSTOM_PANEL_MENU = 26;

        static Action setTab;
        static Action doFireItem;
        static Action<mGraphics> paintTab;
        static Action<mGraphics> paintPanel;

        public static void CreateCustomPanelMenu(Action setTabAction, Action doFireItemAction, Action<mGraphics> paintTabAction, Action<mGraphics> paintPanelAction)
        {
            GameCanvas.panel.type = TYPE_CUSTOM_PANEL_MENU;
            setTab = setTabAction;
            doFireItem = doFireItemAction;
            paintPanel = paintPanelAction;
            paintTab = paintTabAction;
            setTypeModMenu();
            GameCanvas.panel.show();
        }

        public static void setTypeModMenu()
        {
            SoundMn.gI().getSoundOption();
            if (setTab.Method == typeof(ModMenuMain).GetMethod(nameof(ModMenuMain.setTabModMenu))) //Mod menu main
            {
                GameCanvas.panel.tabName[TYPE_CUSTOM_PANEL_MENU] = new string[][]
                {
                    new string[]{ "Bật/tắt", "" },
                    new string[]{ "Điều", "chỉnh" },
                    new string[]{ "Chức", "năng" },
                };
                if (ExtensionManager.Extensions.Count > 0)
                    GameCanvas.panel.tabName[TYPE_CUSTOM_PANEL_MENU] = new string[][]
                    {
                        new string[]{ "Bật/tắt", "" },
                        new string[]{ "Điều", "chỉnh" },
                        new string[]{ "Chức", "năng" },
                        new string[]{ "Phần", "mở rộng" }
                    };
                GameCanvas.panel.currentTabName = GameCanvas.panel.tabName[TYPE_CUSTOM_PANEL_MENU];
                GameCanvas.panel.currentTabIndex = 0;
                Utilities.EmulateSetTypePanel();
                setTabCustomPanelMenu();
                ModMenuMain.onModMenuValueChanged();
            }
            else
            {
                GameCanvas.panel.setType(0);
                setTabCustomPanelMenu();
            }
        }

        public static bool paintTabHeader(mGraphics g)
        {
            if (paintTab == null)
                return false;
            paintTab(g);
            return true;
        }

        public static void setTabCustomPanelMenu() => setTab();

        public static void doFireCustomPanelMenu() => doFireItem();

        public static void paintModMenuMain(mGraphics g) => paintPanel(g);
    }
}