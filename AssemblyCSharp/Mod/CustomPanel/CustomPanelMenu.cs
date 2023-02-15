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

        public Panel panel;

        public Action<Panel> setTab;
        public Action<Panel> doFireItem;
        public Action<Panel, mGraphics> paintTab;
        public Action<Panel, mGraphics> paintPanel;

        public static CustomPanelMenu customPanel = new() { panel = GameCanvas.panel };
        public static CustomPanelMenu customPanel2 = new() { panel = GameCanvas.panel2 };

        public static void show(Action<Panel> setTabAction, Action<Panel> doFireItemAction,
            Action<Panel, mGraphics> paintTabAction, Action<Panel, mGraphics> paintPanelAction)
        {
            customPanel.configAndShow(setTabAction, doFireItemAction, paintTabAction, paintPanelAction);
        }

        public static void show(Action<Panel> setTabAction, Action<Panel> doFireItemAction, Action<Panel,
            mGraphics> paintTabAction, Action<Panel, mGraphics> paintPanelAction, Panel panel)
        {
            var customPanel = getCustomPanel(panel);
            customPanel.configAndShow(setTabAction, doFireItemAction, paintTabAction, paintPanelAction);
        }

        public static CustomPanelMenu getCustomPanel(Panel panel)
        {
            if (panel == GameCanvas.panel)
                return customPanel;

            return customPanel2;
        }

        public void configAndShow(Action<Panel> setTabAction, Action<Panel> doFireItemAction, Action<Panel, mGraphics> paintTabAction, Action<Panel, mGraphics> paintPanelAction)
        {
            panel.type = TYPE_CUSTOM_PANEL_MENU;
            setTab = setTabAction;
            doFireItem = doFireItemAction;
            paintPanel = paintPanelAction;
            paintTab = paintTabAction;
            setTypeModMenu();
            panel.show();
        }

        public void setTypeModMenu()
        {
            SoundMn.gI().getSoundOption();
            if (setTab.Method == typeof(ModMenuMain).GetMethod(nameof(ModMenuMain.setTabModMenu))) //Mod menu main
            {
                panel.tabName[TYPE_CUSTOM_PANEL_MENU] = new string[][]
                {
                    new string[]{ "Bật/tắt", "" },
                    new string[]{ "Điều", "chỉnh" },
                    new string[]{ "Chức", "năng" },
                };
                if (ExtensionManager.Extensions.Count > 0)
                    panel.tabName[TYPE_CUSTOM_PANEL_MENU] = new string[][]
                    {
                        new string[]{ "Bật/tắt", "" },
                        new string[]{ "Điều", "chỉnh" },
                        new string[]{ "Chức", "năng" },
                        new string[]{ "Phần", "mở rộng" }
                    };
                panel.currentTabName = panel.tabName[TYPE_CUSTOM_PANEL_MENU];
                panel.currentTabIndex = 0;
                panel.EmulateSetTypePanel(0);
                setTabCustomPanelMenu(panel);
                ModMenuMain.onModMenuValueChanged();
            }
            else
            {
                panel.setType(0);
                setTabCustomPanelMenu(panel);
            }
        }

        public static bool paintTabHeader(Panel panel, mGraphics g)
        {
            var customPanel = getCustomPanel(panel);

            if (customPanel.paintTab == null)
                return false;
            customPanel.paintTab(panel, g);
            return true;
        }

        public static void setTabCustomPanelMenu(Panel panel)
        {
            getCustomPanel(panel).setTab(panel);
        }

        public static void doFireCustomPanelMenu(Panel panel)
        {
            getCustomPanel(panel).doFireItem(panel);
        }

        public static void paintModMenuMain(Panel panel, mGraphics g)
        {
            getCustomPanel(panel).paintPanel(panel, g);
        }
    }
}