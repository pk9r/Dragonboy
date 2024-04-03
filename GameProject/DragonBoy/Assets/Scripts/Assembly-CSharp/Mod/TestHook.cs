using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MonoHook;
using UnityEngine;

namespace Mod
{
    internal class TestHook
    {
        static MethodHook _hook;

        internal static void Install()
        {
            MethodInfo miAFunc = typeof(GUI).GetMethod("Label", new Type[] {typeof(Rect), typeof(string), typeof(GUIStyle)});
            MethodInfo miBReplace = typeof(TestHook).GetMethod("FuncReplace");
            MethodInfo miBProxy = typeof(TestHook).GetMethod("LabelOriginal");

            _hook = new MethodHook(miAFunc, miBReplace, miBProxy);
            _hook.Install();
        }
        internal static void Uninstall()
        {
            if (_hook != null)
                _hook.Uninstall();
            _hook = null;
        }
        public static void FuncReplace(Rect rect, string text, GUIStyle style)
        {
            if (text == "test1234")
                text = "hooked-test1234";
            LabelOriginal(rect, text, style);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        public static void LabelOriginal(Rect rect, string text, GUIStyle style)
        {
            GUI.Label(rect, text, style);
        }
    }
}
