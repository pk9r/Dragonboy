using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.ModHelper
{
    public class MainThreadDispatcher
    {
        private static readonly Queue<Action> Queue = new();

        /// <summary>
        /// Thực hiện các hành động trong Thread chính của game tránh xung đột
        /// </summary>
        /// <param name="action"></param>
        public static void dispatcher(Action action)
        {
            Queue.Enqueue(action);
        }

        public static void update()
        {
            while (Queue.Count > 0)
            {
                Queue.Dequeue().Invoke();
            }
        }
    }
}
