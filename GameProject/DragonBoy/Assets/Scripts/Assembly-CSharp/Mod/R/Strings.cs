using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod.R
{
    internal static class Strings
    {
        internal static string communityMod = "";

        internal static string registered = "";

        internal static void LoadLanguage(sbyte newLanguage)
        {
            switch (newLanguage)
            {
                case 0:
                    LoadLanguageVI();
                    break;
                case 1:
                    LoadLanguageEN();
                    break;
                case 2:
                    LoadLanguageID();
                    break;
            }
        }

        static void LoadLanguageVI()
        {
            communityMod = "Mod Cộng đồng";
            registered = "Đã đăng ký";
        }

        static void LoadLanguageEN()
        {
            communityMod = "DBO Community Mod";
            registered = "Registered";
        }

        static void LoadLanguageID()
        {
            communityMod = "Mod Komunitas DBO";
            registered = "Terdaftar";
        }
    }
}
