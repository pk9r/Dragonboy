using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mod.ModHelper
{
    /// <summary>
    /// Kế thừa class này để tạo chức năng sử dụng Thread.
    /// </summary>
    public abstract class ThreadAction
    {
        /// <summary>
        /// Thread sử dụng để thực thi hành động.
        /// </summary>
        public Thread threadAction;

        /// <summary>
        /// Kiểm tra hành động còn thực hiện.
        /// </summary>
        public bool isActing => this.threadAction?.IsAlive == true;

        /// <summary>
        /// Hành động cần thực hiện.
        /// </summary>
        protected abstract void action();

        /// <summary>
        /// Thực thi hành động bằng thread của instance.
        /// </summary>
        public void performAction()
        {
            if (this.isActing)
                this.threadAction.Abort();

            this.executeAction();
        }

        /// <summary>
        /// Sử dụng thread của instance để thực thi hành động.
        /// </summary>
        /// <param name="isReturn">Dùng check return của action khi khác thread.</param>
        protected void executeAction()
        {
            // Không thực hiện hành động trong luồng khác
            if (Thread.CurrentThread != this.threadAction)
            {
                this.threadAction = new Thread(this.executeAction)
                {
                    IsBackground = true
                };
                this.threadAction.Start();
                return;
            }

            this.action();
        }
    }
}
