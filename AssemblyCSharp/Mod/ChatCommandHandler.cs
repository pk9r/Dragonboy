using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mod
{
    public class ChatCommandHandler
    {
        public static List<ChatCommand> commands = new List<ChatCommand>();

        public static void loadDefalutCommands()
        {
            var methods = AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(x => x.GetTypes())
                                    .Where(x => x.IsClass)
                                    .SelectMany(x => x.GetMethods());

            foreach (var m in methods)
            {
                var attributes = Attribute.GetCustomAttributes(m, typeof(ChatCommandAttribute));
                foreach (var a in attributes)
                {
                    if (a is ChatCommandAttribute cca)
                    {
                        commands.Add(new ChatCommand()
                        {
                            command = cca.command,
                            name = m.Name,
                            type = m.DeclaringType.FullName,
                            method = m,
                            parameterInfos = m.GetParameters()
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Tìm và thực hiện lệnh chat.
        /// </summary>
        /// <param name="text">Nội dung chat.</param>
        /// <returns>true nếu lệnh thực hiện thành công.</returns>
        public static bool executeCommand(string text)
        {
            foreach (var c in commands)
            {
                if (text.StartsWith(c.command))
                {
                    string args = text.Substring(c.command.Length);

                    if (c.canExecute(args))
                    {
                        c.execute();
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Kiểm tra và thực hiện lệnh chat.
        /// </summary>
        /// <param name="text">Nội dung chat.</param>
        /// <returns>true nếu lệnh thực hiện thành công.</returns>
        public static bool checkAndExecuteChatCommand(string text)
        {
            if (text.StartsWith("/"))
            {
                text = text.Substring(1);
                return executeCommand(text);
            }

            return false;
        }
    }
}
