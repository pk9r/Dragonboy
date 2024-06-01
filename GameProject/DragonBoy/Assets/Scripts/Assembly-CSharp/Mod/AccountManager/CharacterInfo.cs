using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mod.AccountManager
{
    internal class CharacterInfo
    {
        [JsonProperty("name")]
        internal string Name { get; set; } = "unknown";
        [JsonProperty("max_hp")]
        internal long MaxHP { get; set; }
        [JsonProperty("max_mp")]
        internal long MaxMP { get; set; }
        [JsonProperty("exp")]
        internal long EXP { get; set; }
        [JsonProperty("gender")]
        internal sbyte Gender { get; set; } = -1;
        [JsonProperty("icon")]
        internal int Icon { get; set; } = -1;
        [JsonProperty("char_id")]
        internal int CharID { get; set; }

        internal string GetGender()
        {
            if (Gender < 3 && Gender >= 0)
                return mResources.MENUGENDER[Gender];
            return "Unknown";
        }
    }
}
