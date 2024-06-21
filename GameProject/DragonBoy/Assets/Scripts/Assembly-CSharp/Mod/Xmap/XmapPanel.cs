using System.Collections.Generic;
using Mod.CustomPanel;

namespace Mod.Xmap
{
    internal static class XmapPanel
    {
        static List<int> currentMaps = new List<int>();

        internal static void Show(List<int> maps)
        {
            currentMaps = maps;
            CustomPanelMenu.Show(new CustomPanelMenuConfig()
            {
                SetTabAction = SetTab, 
                DoFireItemAction = DoFire,
                PaintTabHeaderAction = PaintTabHeader,
                PaintAction = Paint
            });
        }

        static void Paint(Panel panel, mGraphics g)
        {
            PaintPanelTemplates.PaintCollectionCaptionAndDescriptionTemplate(panel, g, currentMaps, mapID => TileMap.mapNames[mapID], mapID => $"ID: {mapID}");
        }

        static void PaintTabHeader(Panel panel, mGraphics g)
        {
            PaintPanelTemplates.PaintTabHeaderTemplate(panel, g, "Xmap by Phucprotein");
        }

        static void SetTab(Panel panel)
        {
            SetTabPanelTemplates.setTabListTemplate(panel, currentMaps);
        }

        static void DoFire(Panel panel)
        {
            InfoDlg.hide();
            panel.hide();
            XmapController.start(currentMaps[panel.selected]);
        }
    }
}
