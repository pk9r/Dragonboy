using System.Threading;

namespace Mod.ModHelper
{
    /// <summary>
    /// Kế thừa class này để tạo chức năng sử dụng Thread.
    /// </summary>
    public abstract class ThreadAction<T>
        where T : ThreadAction<T>, new()
    {
        public static T gI { get; } = new T();

        /// <summary>
        /// Kiểm tra hành động còn thực hiện.
        /// </summary>
        public bool IsActing => threadAction?.IsAlive == true;

        /// <summary>
        /// Thread sử dụng để thực thi hành động.
        /// </summary>
        protected Thread threadAction;

        /// <summary>
        /// Hành động cần thực hiện.
        /// </summary>
        protected abstract void action();

        /// <summary>
        /// Thực thi hành động bằng thread của instance.
        /// </summary>
        public void performAction()
        {
            if (IsActing)
                threadAction.Abort();

            executeAction();
        }

        /// <summary>
        /// Sử dụng thread của instance để thực thi hành động.
        /// </summary>
        protected void executeAction()
        {
            // Không thực hiện hành động trong luồng khác
            if (Thread.CurrentThread != threadAction)
            {
                threadAction = new Thread(executeAction)
                {
                    IsBackground = true
                };
                threadAction.Start();
                return;
            }
            action();
        }
    }
}
