using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mod.ModHelper;
using Mod.ModHelper.CommandMod.Chat;
using Mono.CSharp;
using UnityEngine;

namespace Mod.CSharpInteractive
{
    internal class MonoInteractiveCodeExecutor
    {
        internal class LogTextWriter : TextWriter
        {
            public override Encoding Encoding => Encoding.UTF8;

            public override void Write(string value)
            {
                if (CSharpInteractiveForm.instance.Visible)
                    CSharpInteractiveForm.Log(value);
                else
                    GameScr.info1.addInfo(value, 0);
            }
        }

        static MonoInteractiveCodeExecutor()
        {
            Evaluator.Init(new string[] { });
            typeof(Evaluator).GetMethod("Reset", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
            Evaluator.MessageOutput = new LogTextWriter();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name != "Newtonsoft.Json"))
                try
                {
                    Evaluator.ReferenceAssembly(assembly);
                }
                catch (Exception ex)
                {
                    Debug.Log("Error while referencing assembly: " + assembly.FullName);
                    Debug.LogException(ex);
                }
            Evaluator.Run("using System;");
        }

        /// <summary>
        /// Using the Mono Compiler Server (MCS) to evaluate script
        /// 
        /// Code is evaluated on the fly (no assemblies generated)
        /// </summary>
        [ChatCommand("run")]
        public static void RunInteractiveCode(params string[] codes)
        {
            string code = string.Join(" ", codes);
            if (!code.EndsWith(";"))
                code += ";";
            try
            {
                Evaluator.Evaluate(code, out object obj, out bool result_set);
                if (obj != null)
                    GameCanvas.startOKDlg("Giá trị trả về: " + obj.ToString());

            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        public static void RunInteractiveCodeMainThread(string code)
        {
            try
            {
                Evaluator.Compile(code, out CompiledMethod compiledMethod);
                MainThreadDispatcher.dispatcher(() =>
                {
                    object obj = null;
                    compiledMethod(ref obj);
                    if (obj != null)
                        CSharpInteractiveForm.Log("Giá trị trả về: " + obj.ToString());
                });
            }
            catch (Exception ex) 
            {
                CSharpInteractiveForm.Log(ex);
            }
        }
    }
}
