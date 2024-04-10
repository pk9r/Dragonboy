using System;
using System.Reflection;
using System.Threading;
using Mod;
using Mod.ModHelper;
using UnityEngine;

namespace EHVN
{
    internal static class FileChooser
    {
        static AndroidJavaObject unityActivity;

        internal static string[] Open(string[] mimeTypes)
        {
            if (!Utils.IsAndroidBuild())
                return null;
            string[] selectedFiles = null;
            AndroidJavaClass fileChooserActivity = null;
            MainThreadDispatcher.dispatch(() =>
            {
                fileChooserActivity = new AndroidJavaClass("com.EHVN.FileChooser.FileChooserActivity");
                //fileChooserActivity = new AndroidJavaClass("com.EHVN.FileChooserActivity");
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

        static string[] OpenFilePicker(string[] mimeTypes)
        {
            string[] selectedFiles = null;
            AndroidJavaClass fileChooserActivity = null;
            MainThreadDispatcher.dispatch(() =>
            {
                fileChooserActivity = new AndroidJavaClass("com.EHVN.FileChooserActivity");
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

        internal static void InitializeUnityActivity()
        {
            if (!Utils.IsAndroidBuild())
                return;
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }

        static T GetStaticRunInUIThread<T>(AndroidJavaClass javaClass, string name)
        {
            T result = default;
            if (!Utils.IsAndroidBuild())
                return default;
            bool assigned = false;
            Exception ex = null;
            MainThreadDispatcher.dispatch(() =>
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

    }
}
