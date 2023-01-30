using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mod.ModHelper.CommandMod.Chat;
using Mono.CSharp;
using UnityEngine;

namespace Mod
{
    internal class InteractiveCodeExecute
    {
        internal class LogTextWriter : TextWriter
        {
            public override Encoding Encoding => Encoding.UTF8;

            public override void Write(string value)
            {
                GameScr.info1.addInfo(value, 0);
            }
        }

        static InteractiveCodeExecute()
        {
            Evaluator.Init(new string[] { });
            typeof(Evaluator).GetMethod("Reset", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
            Evaluator.MessageOutput = new LogTextWriter();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                Evaluator.ReferenceAssembly(assembly);
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
            catch(Exception ex) { Debug.LogException(ex); }
        }
    }
}
