using UnityEngine;

public static class Utilities
{
    public static bool IsAndroidBuild() => Application.platform == RuntimePlatform.Android;
    public static bool IsLinuxBuild() => Application.platform == RuntimePlatform.LinuxPlayer;
    public static bool IsWindowsBuild() => Application.platform == RuntimePlatform.WindowsPlayer;
}