using System;
using System.Reflection;

namespace Mod.ModHelper.CommandMod
{
    public abstract class BaseCommand
    {
        public char delimiter;

        [LitJSON.JsonSkip]
        public MethodInfo method;

        [LitJSON.JsonSkip]
        public ParameterInfo[] parameterInfos;

        /// <summary>
        /// Kiểm tra có thể thực hiện command với args không.
        /// </summary>
        /// <param name="args">Chuỗi chứa các đối số cần của command.</param>
        /// <param name="parameters">Danh sách các đối số đã tách và ép kiểu được.</param>
        /// <returns>true nếu có thể thực hiện.</returns>
        public bool canExecute(string args, out object[] parameters)
        {
            parameters = null;
            preprocessingArgs(ref args);

            if (args == "" && parameterInfos.Length == 0)
                return true; // Lệnh không cần tham số

            if (!checkCountArgs(args, out string[] arguments))
                return false; // Số lượng tham số không phù hợp

            if (!checkTypeArgs(arguments, out parameters))
                return false; // Kiểu tham số không phù hợp

            return true;
        }


        /// <summary>
        /// Thực thi command.
        /// </summary>
        /// <param name="args">Các đối số của command.</param>
        /// <returns>true nếu thực thi thành công.</returns>
        public bool execute(string args)
        {
            if (!canExecute(args, out object[] parameters))
                return false;

            method.Invoke(null, parameters);
            return true;
        }

        /// <summary>
        /// Tiền xử lý các đối số.
        /// </summary>
        /// <param name="args">Các đối số của command</param>
        private static void preprocessingArgs(ref string args)
        {
            args = args.Trim();
        }

        /// <summary>
        /// Kiểm tra số lượng đối số.
        /// </summary>
        /// <param name="args">Các đối số của command</param>
        /// <param name="arguments">Danh sách các đối số đã tách được.</param>
        /// <returns>true nếu độ dài phù hợp với command.</returns>
        private bool checkCountArgs(string args, out string[] arguments)
        {
            arguments = args.Split(delimiter);
            return parameterInfos.Length == arguments.Length;
        }

        /// <summary>
        /// Kiểm tra kiểu dữ liệu các đối số.
        /// </summary>
        /// <param name="arguments">Danh sách các đối số chưa ép kiểu.</param>
        /// <param name="parameters">Danh sách các đối số đã ép kiểu được.</param>
        /// <returns></returns>
        private bool checkTypeArgs(string[] arguments, out object[] parameters)
        {
            parameters = new object[arguments.Length];

            try
            {
                for (int i = 0; i < arguments.Length; i++)
                    parameters[i] = Convert.ChangeType(arguments[i],
                        parameterInfos[i].ParameterType);

                return true; // Tất cả đối số đều đúng type
            }
            catch (InvalidCastException)
            {
                return false; // Có đối số không đúng type
            }
        }
    }
}
