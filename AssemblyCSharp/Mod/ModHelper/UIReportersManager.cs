using System;
using System.Collections.Generic;

namespace Mod.ModHelper
{
    public delegate int Reporter(int y, mGraphics g);

    internal class UIReportersManager
    {
        public const UInt16 STARTER_Y = 60;
        
        private static List<Reporter> reporters = [];

        public static void AddReporter(Reporter reporter)
        {
            reporters.Add(reporter);
        }

        public static void RemoveReporter(Reporter reporter)
        {
            reporters.Remove(reporter);
        }

        public static void ClearReporters()
        {
            reporters.Clear();
        }

        public static void handlePaintGameScr(mGraphics g)
        {
            int lastY = STARTER_Y;
            int itemGap = 10;

            foreach (var reporter in reporters)
            {
                lastY += reporter(lastY, g) + itemGap;
            }
        }
    }
}
