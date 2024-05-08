using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;

namespace AssemblyCSharpPreprocessor
{
    internal static class Utils
    {
        internal static CustomAttribute GetClosestCompilerGeneratedAttribute(this IMemberDef member)
        {
            try
            {
                if (member.DeclaringType != null)
                {
                    TypeDef declaringType = member.DeclaringType;
                    while (declaringType != null)
                    {
                        CustomAttribute ca = declaringType.GetCompilerGeneratedAttribute();
                        if (ca != null)
                            return ca;
                        declaringType = declaringType.DeclaringType;
                    }
                }
                return member.GetCompilerGeneratedAttribute();
            }
            catch { }
            return null;
        }

        internal static CustomAttribute GetCompilerGeneratedAttribute(this IHasCustomAttribute member)
        {
            try
            {
                return member.CustomAttributes.Count == 0 ? null : member.CustomAttributes.First(ca => ca.TypeFullName == typeof(CompilerGeneratedAttribute).FullName);
            }
            catch { }
            return null;
        }

    }
}
