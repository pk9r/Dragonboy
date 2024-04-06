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
        internal static readonly string DEFAULT_IP_SERVERS = "Vũ trụ 1:dragon1.teamobi.com:14445:0:0:0,Vũ trụ 2:dragon2.teamobi.com:14445:0:0:0,Vũ trụ 3:dragon3.teamobi.com:14445:0:0:0,Vũ trụ 4:dragon4.teamobi.com:14445:0:0:0,Vũ trụ 5:dragon5.teamobi.com:14445:0:0:0,Vũ trụ 6:dragon6.teamobi.com:14445:0:0:0,Vũ trụ 7:dragon7.teamobi.com:14445:0:0:0,Vũ trụ 8:dragon10.teamobi.com:14446:0:0:0,Vũ trụ 9:dragon10.teamobi.com:14447:0:0:0,Vũ trụ 10:dragon10.teamobi.com:14445:0:0:0,Vũ trụ 11:dragon11.teamobi.com:14445:0:0:0,Vũ trụ 12:dragon12.teamobi.com:14445:0:0:1,Võ đài liên vũ trụ:dragonwar.teamobi.com:20000:0:0:0,Universe 1:dragon.indonaga.com:14445:1:0:0,Naga:dragon.indonaga.com:14446:2:0:0,Super 1:dragon11.teamobi.com:14446:0:1:1,0,0";

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
