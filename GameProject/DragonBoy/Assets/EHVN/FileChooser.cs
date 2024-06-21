#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace EHVN
{
    /// <summary>
    /// Class chứa các hàm để chọn tệp trên Android.
    /// </summary>
    public static class FileChooser
    {
        static AndroidJavaObject unityActivity;

        static Queue<Action> runOnMainThreadActions = new Queue<Action>();

        /// <summary>
        /// Mở activity chọn tệp trên Android.
        /// </summary>
        /// <param name="mimeTypes">Loại MIME của tệp</param>
        /// <returns>Danh sách đường dẫn các tệp đã chọn hoặc mảng rỗng nếu người dùng hủy chọn tệp.</returns>
        public static string[] Open(string[] mimeTypes)
        {
            if (Application.platform != RuntimePlatform.Android)
                return null;
            InitializeUnityActivity();
            string[] selectedFiles = null;
            AndroidJavaClass fileChooserActivity = null;
            RunOnMainThread(() =>
            {
                fileChooserActivity = new AndroidJavaClass("com.EHVN.FileChooser.FileChooserActivity");
                fileChooserActivity.CallStatic("chooseFiles", unityActivity, mimeTypes);
            });
            Thread.Sleep(2000);
            while (fileChooserActivity == null)
                Thread.Sleep(100);
            new Thread(() =>
            {
                while (GetStaticRunInUIThread<bool>(fileChooserActivity, "isOpening"))
                    Thread.Sleep(200);
                selectedFiles = GetStaticRunInUIThread<string[]>(fileChooserActivity, "selectedFiles");
            })
            { IsBackground = true }.Start();
            while (selectedFiles == null)
                Thread.Sleep(100);
            return selectedFiles;
        }

        static void InitializeUnityActivity()
        {
            if (unityActivity == null)
                RunOnMainThread(() =>
                {
                    AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                });
            while (unityActivity == null)
                Thread.Sleep(100);
        }

        /// <summary>
        /// Gọi hàm này trong hàm Update của <see cref="MonoBehaviour"/> để chạy các hàm java trên thread chính của game.
        /// </summary>
        public static void Update()
        {
            while (runOnMainThreadActions.Count > 0)
                runOnMainThreadActions.Dequeue()();
        }

        static T GetStaticRunInUIThread<T>(AndroidJavaClass javaClass, string name)
        {
            T result = default;
            bool assigned = false;
            Exception ex = null;
            RunOnMainThread(() =>
            {
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    try
                    {
                        result = javaClass.GetStatic<T>(name);
                    }
                    catch (Exception e) { ex = e; }
                    assigned = true;
                }));
            });
            while (!assigned)
                Thread.Sleep(100);
            if (ex != null)
            {
                throw new TargetInvocationException(ex);
            }
            return result;
        }

        static void RunOnMainThread(Action action) => runOnMainThreadActions.Enqueue(action);
    }
}
#endif