using System;
using System.Collections.Generic;

namespace Mod.ModHelper
{
    public delegate int Reporter(int y, mGraphics g);

    internal class UIReportersManager
    {
        public const byte MinY = 60;
        public const byte ItemGap = 5;
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
            int lastY = MinY;

            foreach (var reporter in reporters)
            {
                lastY += reporter(lastY, g) + ItemGap;
            }
        }
    }
}
