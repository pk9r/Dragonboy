using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Mod.ModHelper.CommandMod.Chat
{
    public class ChatCommandHandler
    {
        public static List<ChatCommand> chatCommands = new List<ChatCommand>();

        /// <summary>
        /// Tải lệnh chat mặc định. (các lệnh được định nghĩa trên code)
        /// </summary>
        public static void loadDefault()
        {
            var methods = CommandUtils.GetMethods();

            var length = methods.Length;
            for (int i = 0; i < length; i++)
            {
                var method = methods[i];
                var attributes = Attribute.GetCustomAttributes(method, typeof(ChatCommandAttribute));
                foreach (var attribute in attributes)
                {
                    if (attribute is ChatCommandAttribute cca)
                    {
                        chatCommands.Add(new ChatCommand()
                        {
                            command = cca.command,
                            delimiter = cca.delimiter,
                            fullCommand = method.DeclaringType.FullName + "." + method.Name,
                            method = method,
                            parameterInfos = method.GetParameters()
                        });
                    }
                }
            }

            save();
        }

        /// <summary>
        /// Lưu lệnh chat.
        /// </summary>
        public static void save() => File.WriteAllText(Utils.PathChatCommand, JsonConvert.SerializeObject(chatCommands));

        /// <summary>
        /// Tìm và thực hiện lệnh chat.
        /// </summary>
        /// <param name="command">Nội dung lệnh.</param>
        /// <returns>true nếu lệnh thực hiện thành công.</returns>
        public static bool execute(string command)
        {
            foreach (var chatCommand in chatCommands)
            {
                int commandLength = -1;

                if (!string.IsNullOrEmpty(chatCommand.command) && command.StartsWith(chatCommand.command))
                    commandLength = chatCommand.command.Length;
                else if (command.StartsWith(chatCommand.fullCommand))
                    commandLength = chatCommand.fullCommand.Length;

                if (commandLength != -1)
                {
                    string args = command.Substring(commandLength);

                    if (chatCommand.execute(args))
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
            var match = Regex.Match(command, @"^(([A-Za-z0-9.]+)\.([A-Za-z0-9]+))(.*)$");

            if (!match.Success)
            {
                return false;
            }

            string fullCommand = match.Groups[1].Value;
            string typeFullName = match.Groups[2].Value;
            string methodName = match.Groups[3].Value;
            string args = match.Groups[4].Value;

            var methods = CommandUtils.getMethods(typeFullName);

            if (methods == null)
            {
                return false;
            }

            foreach (var method in methods)
            {
                if (method.Name.ToLower() != methodName.ToLower())
                {
                    continue;
                }

                var c = new ChatCommand()
                {
                    command = null,
                    fullCommand = fullCommand,
                    method = method,
                    parameterInfos = method.GetParameters()
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
                return false;

            return text
                .Substring(1)
                .Split(',')
                .Aggregate(false, (acc, command) => execute(command.ToString()) || acc);
        }
    }
}
