using System;
using System.Collections.Generic;
using System.Linq;

namespace Mod.CustomPanel
{
    internal static class PaintPanelTemplates
    {
        internal static void PaintTabHeaderTemplate(Panel panel, mGraphics g, string header)
        {
            g.setColor(0xCE5E0C);
            g.fillRect(panel.X + 1, 78, panel.W - 2, 1);
            mFont.tahoma_7b_dark.drawString(g, header, panel.xScroll + panel.wScroll / 2, 59, mFont.CENTER);
        }

        internal static void PaintCollectionCaptionAndDescriptionTemplate<T>(Panel panel, mGraphics g, ICollection<T> collection,
            Func<T, string> getCaption, Func<T, string> getDescription, bool captionIndex = true)
        {
            g.setClip(panel.xScroll, panel.yScroll, panel.wScroll, panel.hScroll);
            g.translate(0, -panel.cmy);
            g.setColor(0);
            if (collection == null || collection.Count != panel.currentListLength)
                return;
            for (int i = 0; i < panel.currentListLength; i++)
            {
                int num = panel.xScroll;
                int num2 = panel.yScroll + i * panel.ITEM_HEIGHT;
                int num3 = panel.wScroll;
                int num4 = panel.ITEM_HEIGHT - 1;
                g.setColor((i != panel.selected) ? 0xE7DFD2 : 0xF9FF4A);
                g.fillRect(num, num2, num3, num4);
                string caption = (captionIndex ? $"{i + 1}. " : "") + getCaption(collection.ElementAt(i));
                string description = getDescription(collection.ElementAt(i));
                mFont.tahoma_7_green2.drawString(g, caption, num + 5, num2, 0);
                mFont.tahoma_7_blue.drawString(g, description, num + 5, num2 + 11, 0);
            }
            panel.paintScrollArrow(g);
        }
    }
}
