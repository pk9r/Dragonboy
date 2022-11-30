using System;
using System.Collections.Generic;
using System.IO;

namespace Mod.ModHelper.CommandMod.Hotkey
{
    public class HotkeyCommandHandler
    {
        public static List<HotkeyCommand> hotkeyCommands = new List<HotkeyCommand>();

        /// <summary>
        /// Tải lệnh chat mặc định.
        /// </summary>
        public static void loadDefalut()
        {
            var methods = Utilities.GetMethods();

            foreach (var m in methods)
            {
                var attributes = Attribute.GetCustomAttributes(m, typeof(HotkeyCommandAttribute));
                foreach (var a in attributes)
                {
                    if (a is HotkeyCommandAttribute kca)
                    {
                        var hotkeyCommand = new HotkeyCommand()
                        {
                            key = kca.key,
                            delimiter = kca.delimiter,
                            fullCommand = m.DeclaringType.FullName + "." + m.Name,
                            method = m,
                            parameterInfos = m.GetParameters(),
                        };
                        if (hotkeyCommand.canExecute(kca.agrs, out hotkeyCommand.parameters))
                        {
                            hotkeyCommands.Add(hotkeyCommand);
                        }
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
            File.WriteAllText(Utilities.PathHotkeyCommand,
                LitJson.JsonMapper.ToJson(hotkeyCommands));
        }

        /// <summary>
        /// Xử lý phím nhấn.
        /// </summary>
        /// <param name="key">Mã ASCII phím được nhấn.</param>
        /// <returns>true nếu có lệnh được thực hiện thành công.</returns>
        public static bool handleHotkey(int key)
        {
            foreach (var h in hotkeyCommands)
            {
                if (h.key == key)
                {
                    h.execute();
                    return true;
                }
            }

            return false;
        }
    }
}
