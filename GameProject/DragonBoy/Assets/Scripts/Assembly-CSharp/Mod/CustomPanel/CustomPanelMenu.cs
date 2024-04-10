using System;
using Mod.ModMenu;
using Mod.R;

namespace Mod.CustomPanel
{
    internal class CustomPanelMenu
    {
        internal static readonly int TYPE_CUSTOM_PANEL_MENU = 27;

        internal Panel panel;

        internal Action<Panel> setTab;
        internal Action<Panel> doFireItem;
        internal Action<Panel, mGraphics> paintTab;
        internal Action<Panel, mGraphics> paintPanel;

        internal static CustomPanelMenu customPanel = new CustomPanelMenu() { panel = GameCanvas.panel };
        internal static CustomPanelMenu customPanel2 = new CustomPanelMenu() { panel = GameCanvas.panel2 };

        internal static void show(Action<Panel> setTabAction, Action<Panel> doFireItemAction,
            Action<Panel, mGraphics> paintTabAction, Action<Panel, mGraphics> paintPanelAction) => customPanel.configAndShow(setTabAction, doFireItemAction, paintTabAction, paintPanelAction);

        internal static void show(Action<Panel> setTabAction, Action<Panel> doFireItemAction, Action<Panel,
            mGraphics> paintTabAction, Action<Panel, mGraphics> paintPanelAction, Panel panel) => getCustomPanel(panel).configAndShow(setTabAction, doFireItemAction, paintTabAction, paintPanelAction);

        internal static CustomPanelMenu getCustomPanel(Panel panel)
        {
            if (panel == GameCanvas.panel)
            {
                customPanel.panel = panel;
                return customPanel;
            }
            else
            {
                customPanel2.panel = panel;
                return customPanel2;
            }
        }

        internal void configAndShow(Action<Panel> setTabAction, Action<Panel> doFireItemAction, Action<Panel, mGraphics> paintTabAction, Action<Panel, mGraphics> paintPanelAction)
        {
            panel.type = TYPE_CUSTOM_PANEL_MENU;
            setTab = setTabAction;
            doFireItem = doFireItemAction;
            paintPanel = paintPanelAction;
            paintTab = paintTabAction;
            setTypeModMenu();
            panel.show();
        }

        internal void setTypeModMenu()
        {
            SoundMn.gI().getSoundOption();
            if (setTab.Method == new Action<Panel>(ModMenuMain.SetTabModMenu).Method) //Mod menu main
            {
                panel.tabName[TYPE_CUSTOM_PANEL_MENU] = Strings.modMenuPanelTabName;
                panel.currentTabName = panel.tabName[TYPE_CUSTOM_PANEL_MENU];
                panel.currentTabIndex = 0;
                //panel.EmulateSetTypePanel(panel == GameCanvas.panel ? 0 : 1);
                panel.setType(panel == GameCanvas.panel ? 0 : 1);
                setTabCustomPanelMenu(panel);
            }
            else
            {
                panel.setType(panel == GameCanvas.panel ? 0 : 1);
                setTabCustomPanelMenu(panel);
            }
        }

        internal static bool paintTabHeader(Panel panel, mGraphics g)
        {
            var customPanel = getCustomPanel(panel);
            if (customPanel.paintTab == null)
                return false;
            customPanel.paintTab(panel, g);
            return true;
        }

        internal static void setTabCustomPanelMenu(Panel panel) => getCustomPanel(panel).setTab(panel);

        internal static void doFireCustomPanelMenu(Panel panel) => getCustomPanel(panel).doFireItem(panel);

        internal static void paintModMenuMain(Panel panel, mGraphics g) => getCustomPanel(panel).paintPanel(panel, g);
    }
}