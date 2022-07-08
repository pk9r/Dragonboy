using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mod.ModHelper
{
    /// <summary>
    /// Hỗ trợ tạo thread thực hiện cách hành động lặp đi lặp lại.
    /// </summary>
    public abstract class ThreadActionUpdate : ThreadAction
    {
        public new bool isActing;

        /// <summary>
        /// Thời gian nghỉ giữa các lần thực thi.
        /// </summary>
        public int interval;

        protected override void action()
        {
            while (isActing)
            {
                update();
                Thread.Sleep(interval);
            }
        }

        /// <summary>
        /// Hành động thực hiện.
        /// </summary>
        protected abstract void update();

        /// <summary>
        /// Chuyển đổi trạng thái hành động
        /// </summary>
        /// <param name="isActing">Trạng thái hành động muốn chuyển đổi, nếu null thì sẽ đổi qua lại giữa bật và tắt</param>
        public void toggleAction(bool? isActing = null)
        {
            if (isActing == null)
            {
                isActing = !this.isActing;
            }

            if (this.isActing = (bool)isActing)
            {
                this.performAction();
            }
            else
            {
                if (base.isActing)
                {
                    this.threadAction.Abort();
                }
            }
        }
    }
}
