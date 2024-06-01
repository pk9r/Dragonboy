using System;
using Mod.R;
using Newtonsoft.Json;

namespace Mod.AccountManager
{
    internal class Account
    {
        [JsonProperty("username")]
        internal string Username { get; set; } = "";
        [JsonProperty("password")]
        internal string Password { get; set; } = "";
        [JsonProperty("server")]
        [JsonConverter(typeof(ServerConverter))]
        internal Server Server { get; set; }
        [JsonProperty("last_time_login")]
        internal DateTime LastTimeLogin { get; set; } = DateTime.MinValue;
        //internal bool IsPinned { get; set; }
        [JsonProperty("vang")]
        internal long Gold { get; set; }
        [JsonProperty("ngoc_xanh")]
        internal long Gem { get; set; }
        [JsonProperty("ruby")]
        internal long Ruby { get; set; }
        [JsonProperty("info")]
        internal CharacterInfo Info { get; set; } = new CharacterInfo();
        [JsonProperty("info_pet")]
        internal CharacterInfo PetInfo { get; set; }

        internal string GetLastTimeLogin()
        {
            if (LastTimeLogin == DateTime.MinValue)
                return Strings.haventLoggedInYet;
            //string result = Strings.lastLogin + ": ";
            string result = "";
            TimeSpan timeSpan = DateTime.Now - LastTimeLogin;
            if (timeSpan.TotalMinutes < 1)
                result += Strings.justNow;
            else if (timeSpan.TotalHours < 1)
                result += string.Format(Strings.minutesAgo, timeSpan.Minutes);
            else if (timeSpan.TotalDays < 1)
                result += string.Format(Strings.hoursAgo, timeSpan.Hours);
            else if (timeSpan.TotalDays < 2)
                result += string.Format(Strings.yesterdayAt, LastTimeLogin.ToString("HH:mm"));
            else
                result += LastTimeLogin.ToString("dd/MM/yyyy HH:mm");
            return result;
        }
    }
}