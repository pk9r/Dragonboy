using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Mod
{
    public class ChatCommandHandler
    {
        public static List<ChatCommand> chatCommands = new List<ChatCommand>();

        /// <summary>
        /// Tải lệnh chat mặc định. (các lệnh được định nghĩa trên code)
        /// </summary>
        public static void loadDefalut()
        {
            var methods = Utilities.GetMethods();

            foreach (var m in methods)
            {
                var attributes = Attribute.GetCustomAttributes(m, typeof(ChatCommandAttribute));
                foreach (var a in attributes)
                {
                    if (a is ChatCommandAttribute cca)
                    {
                        chatCommands.Add(new ChatCommand()
                        {
                            command = cca.command,
                            delimiter = cca.delimiter,
                            fullCommand = m.DeclaringType.FullName + "." + m.Name,
                            method = m,
                            parameterInfos = m.GetParameters()
                        });
                    }
                }
            }

            save();
        }

        /// <summary>
        /// Lưu lệnh chat.
        /// </summary>
        public static void save()
        {
            File.WriteAllText(Properties.Resources.PathChatCommand,
                LitJson.JsonMapper.ToJson(chatCommands));
        }

        /// <summary>
        /// Tìm và thực hiện lệnh chat.
        /// </summary>
        /// <param name="command">Nội dung lệnh.</param>
        /// <returns>true nếu lệnh thực hiện thành công.</returns>
        public static bool execute(string command)
        {
            foreach (var c in chatCommands)
            {
                string args = null;
                if (c.command != null && command.StartsWith(c.command))
                    args = command.Substring(c.command.Length);
                else if (command.StartsWith(c.fullCommand))
                    args = command.Substring(c.fullCommand.Length);

                if (args != null)
                {
                    if (c.execute(args))
                    {
                        return true;
                    }
                }
            }

            if (executeFull(command))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tìm và thực hiện các lệnh chat dạng đầy đủ (namespace.class.method)
        /// </summary>
        /// <param name="command">Nội dung lệnh.</param>
        /// <returns>true nếu lệnh thực hiện thành công.</returns>
        public static bool executeFull(string command)
        {
            string pattern = @"^(([A-Za-z.]+)\.([A-Za-z]+))(.*)$";
            var match = Regex.Match(command, pattern);

            if (!match.Success)
            {
                return false;
            }

            string fullCommand = match.Groups[1].Value;
            string typeFullName = match.Groups[2].Value;
            string methodName = match.Groups[3].Value;
            string args = match.Groups[4].Value;

            var methods = Utilities.getMethods(typeFullName);

            if (methods == null)
            {
                return false;
            }

            foreach (var m in methods)
            {
                if (m.Name.ToLower() != methodName.ToLower())
                {
                    continue;
                }

                var c = new ChatCommand()
                {
                    command = null,
                    fullCommand = fullCommand,
                    method = m,
                    parameterInfos = m.GetParameters()
                };

                if (c.execute(args))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Xử lý câu chat.
        /// </summary>
        /// <param name="text">Nội dung chat.</param>
        /// <returns>true nếu có lệnh được thực hiện thành công.</returns>
        public static bool handleChatText(string text)
        {
            // Lệnh chat phải bắt đầu bằng / để phân biệt với chat bình thường
            if (!text.StartsWith("/"))
            {
                return false;
            }

            text = text.Substring(1);
            return execute(text);
        }
    }
}