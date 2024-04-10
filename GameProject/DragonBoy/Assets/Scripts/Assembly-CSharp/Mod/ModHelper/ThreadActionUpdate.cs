using System.Threading;

namespace Mod.ModHelper
{
    /// <summary>
    /// Hỗ trợ tạo thread thực hiện cách hành động lặp đi lặp lại.
    /// </summary>
    internal abstract class ThreadActionUpdate<T> : ThreadAction<T>
        where T : ThreadActionUpdate<T>, new()
    {
        bool isActing;

        internal new bool IsActing => isActing;

        /// <summary>
        /// Thời gian nghỉ giữa các lần thực thi.
        /// </summary>
        internal abstract int Interval { get; }

        protected override void action()
        {
            while (isActing)
            {
                update();
                Thread.Sleep(Interval);
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
        internal void toggle(bool? isActing = null)
        {
            if (isActing == null)
            {
                isActing = !this.isActing;
            }

            if (this.isActing = (bool)isActing)
            {
                performAction();
            }
            else
            {
                if (base.IsActing)
                {
                    threadAction.Abort();
                }
            }
        }

        internal static void toggle(bool value) => gI.toggle(value);
    }
}
