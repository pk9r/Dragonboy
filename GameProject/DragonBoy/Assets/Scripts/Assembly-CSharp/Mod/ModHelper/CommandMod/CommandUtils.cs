using System;
using System.Linq;
using System.Reflection;

namespace Mod.ModHelper.CommandMod
{
    internal static class CommandUtils
    {
        private const BindingFlags STATIC_VOID =
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Static |
            BindingFlags.InvokeMethod;

        #region Get Methods
        /// <summary>
        /// Lấy danh sách các hàm trong theo tên của class.
        /// </summary>
        /// <remarks> Lưu ý:
        /// <list type="bullet">
        /// <item><description>Chỉ lấy các hàm public static void.</description></item>
        /// <item><description>Tên class phải bao gồm cả namespace.</description></item>
        /// </list>
        /// </remarks>
        /// <param name="typeFullName"></param>
        /// <returns>Danh sách các hàm trong class.</returns>
        public static MethodInfo[] getMethods(string typeFullName)
        {
            return typeof(CommandUtils).Assembly
                .GetTypes().FirstOrDefault(x => x.FullName.ToLower() == typeFullName.ToLower())
                .GetMethods(STATIC_VOID);
        }

        /// <summary>
        /// Lấy danh sách tất cả các hàm của tệp Assembly-CSharp.dll.
        /// </summary>
        /// <remarks> Lưu ý:
        /// <list type="bullet">
        /// <item><description>Chỉ lấy các hàm public static void.</description></item>
        /// <item><description>Tên class phải bao gồm cả namespace.</description></item>
        /// </list>
        /// </remarks>
        /// <returns>Danh sách các hàm của tệp Assembly-CSharp.dll.</returns>
        public static MethodInfo[] GetMethods()
        {
            return typeof(CommandUtils).Assembly
                .GetTypes().Where(x => x.IsClass)
                .SelectMany(x => x.GetMethods(STATIC_VOID))
                .ToArray();
        }
        #endregion
    }
}
