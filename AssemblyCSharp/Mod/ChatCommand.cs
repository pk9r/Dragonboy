using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mod
{
    public class ChatCommand
    {
        public string command;
        public string type;
        public string name;
        public char delimiter = ' ';

        public MethodInfo method;
        public ParameterInfo[] parameterInfos;
        
        private object[] parameters;

        public bool canExecute(string args)
        {
            if (args == "" && this.parameterInfos.Length == 0)
            {
                return true;
            }
            
            var arguments = args.Split(this.delimiter);

            if (this.parameterInfos.Length != arguments.Length)
            {
                return false;
            }

            var parameters = new object[arguments.Length];

            try
            {
                for (int i = 0; i < arguments.Length; i++)
                {
                    parameters[i] = Convert.ChangeType(arguments[i],
                        this.parameterInfos[i].ParameterType);
                }

                this.parameters = parameters;

                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public void execute()
        {
            method.Invoke(null, parameters);
        }

        public bool findMethod(string text)
        {
            string args = text.Substring(this.command.Length);

            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(x => x.FullName == this.type);

            var methods = type?.GetMethods(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.IgnoreReturn);

            if (methods != null)
            {
                foreach (var m in methods)
                {
                    if (this.canExecute(args))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
