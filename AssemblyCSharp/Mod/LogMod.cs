using System;
using System.IO;

namespace Mod
{
    public class LogMod
    {
        public static string pathLog = "ModData\\log.txt";


        public static bool isShowTime = true;
        public static bool logError = true;
        public static bool logDebug = true;
        public static bool logXmap = true;

        private static readonly object locker = new object();

        public static bool filter(string message)
        {
            bool flag = true;
            if (!logError)
                flag &= !message.Contains("[err]");
            if (!logXmap)
                flag &= !message.Contains("[xmap]");
            if (!logDebug)
                flag &= !message.Contains("[dbg]");

            return flag;
        }

        public static void write(string message)
        {
            if (!filter(message))
                return;

            lock (locker)
            {
                using (var writer = new StreamWriter(pathLog, append: true))
                {
                    beforeWriteMessage(writer);
                    writer.Write(message);
                }
            }
        }

        public static void writeLine(string message)
        {
            if (!filter(message))
                return;

            lock (locker)
            {
                using (var writer = new StreamWriter(pathLog, append: true))
                {
                    beforeWriteMessage(writer);
                    writer.WriteLine(message);
                }
            }
        }

        private static void beforeWriteMessage(StreamWriter writer)
        {
            if (isShowTime)
            {
                writer.Write(DateTime.Now.ToString());
                writer.Write(" - ");
            }
        }
    }
}
