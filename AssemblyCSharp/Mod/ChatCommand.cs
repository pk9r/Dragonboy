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
        public string fullCommand;
        public char delimiter = ' ';

        [LitJSON.JsonSkip]
        public MethodInfo method;

        [LitJSON.JsonSkip]
        public ParameterInfo[] parameterInfos;

        [LitJSON.JsonSkip]
        public object[] parameters;

        public bool canExecute(string args)
        {
            args = args.Trim();
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
    }
}
