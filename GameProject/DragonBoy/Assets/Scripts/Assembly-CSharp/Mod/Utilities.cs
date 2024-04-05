using UnityEngine;

namespace Mod
{
    public static class Utilities
    {
        /// <summary>
        /// Kiểm tra xem ứng dụng đang chạy trên hệ điều hành Android hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên Android, ngược lại trả về false.</returns>
        public static bool IsAndroidBuild() => Application.platform == RuntimePlatform.Android;

        /// <summary>
        /// Kiểm tra xem ứng dụng đang chạy trên hệ điều hành Linux hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên Linux, ngược lại trả về false.</returns>
        public static bool IsLinuxBuild() => Application.platform == RuntimePlatform.LinuxPlayer;

        /// <summary>
        /// Kiểm tra xem ứng dụng đang chạy trên hệ điều hành Windows hay không.
        /// </summary>
        /// <returns>Trả về true nếu đang chạy trên Windows, ngược lại trả về false.</returns>
        public static bool IsWindowsBuild() => Application.platform == RuntimePlatform.WindowsPlayer;
    }
}