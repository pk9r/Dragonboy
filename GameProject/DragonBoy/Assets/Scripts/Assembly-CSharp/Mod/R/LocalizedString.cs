using System.Collections.Generic;
using System.Linq;

namespace Mod.R
{
    /// <summary>
    /// Class chứa các chuỗi của 3 ngôn ngữ: tiếng Việt, tiếng Anh và tiếng Indonesia, dùng để kiểm tra và so sánh với các chuỗi của game do server gửi về
    /// </summary>
    internal class LocalizedString
    {
        internal static LocalizedString free1hCharm = new string[3]
        {
             "thưởng bùa 1h ngẫu nhiên",
             "random 1 hour charm reward",
             "hadiah 1 jam charm"
        };
        internal static LocalizedString challengeKarin = new string[3]
        {
            "thách đấu thần mèo",
            "challenge with karin",
            ""  //I don't know what this string is in Indonaga server
        };
        internal static LocalizedString acceptChallenge = new string[3]
        {
            "đồng ý giao đấu",
            "accept fight",
            ""  //I don't know what this string is in Indonaga server
        };
        internal static LocalizedString mercenaryTao = new string[3]
        {
            "Tàu Pảy Pảy",
            "Taopaipai",
            ""  //I don't know what this string is in Indonaga server
        };  
        internal static LocalizedString saoMayLuoiThe = new string[3]
        {
            "sao sư phụ không đánh đi",
            "",  //I don't know what this string is in World server
            ""  //I don't know what this string is in Indonaga server
        };

        string[] strings;
        LocalizedString(string[] strings) => this.strings = strings;

        /// <summary>
        /// Kiểm tra xem <see langword="this"/> có chứa chuỗi <paramref name="str"/> không
        /// </summary>
        internal bool Contains(string str) => strings.Any(s => !string.IsNullOrEmpty(s) && s.Contains(str));

        /// <summary>
        /// Kiểm tra xem chuỗi <paramref name="str"/> có chứa <see langword="this"/> không
        /// </summary>
        internal bool ContainsReversed(string str) => strings.Any(s => !string.IsNullOrEmpty(s) && str.Contains(s));

        /// <summary>
        /// Kiểm tra xem <see langword="this"/> có bắt đầu bằng chuỗi <paramref name="str"/> không
        /// </summary>
        internal bool StartsWith(string str) => strings.Any(s => !string.IsNullOrEmpty(s) && s.StartsWith(str));

        /// <summary>
        /// Kiểm tra xem chuỗi <paramref name="str"/> có bắt đầu bằng <see langword="this"/> không
        /// </summary>
        internal bool StartsWithReversed(string str) => strings.Any(s => !string.IsNullOrEmpty(s) && str.StartsWith(s));

        /// <summary>
        /// Kiểm tra xem <see langword="this"/> có kết thúc bằng chuỗi <paramref name="str"/> không
        /// </summary>
        internal bool EndsWith(string str) => strings.Any(s => !string.IsNullOrEmpty(s) && s.EndsWith(str));

        /// <summary>
        /// Kiểm tra xem chuỗi <paramref name="str"/> có kết thúc bằng <see langword="this"/> không
        /// </summary>
        internal bool EndsWithReversed(string str) => strings.Any(s => !string.IsNullOrEmpty(s) && str.EndsWith(s));

        /// <summary>
        /// Kiểm tra xem <see langword="this"/> có bằng chuỗi <paramref name="str"/> không
        /// </summary>
        internal bool IsEqual(string str) => strings.Any(s => !string.IsNullOrEmpty(s) && s == str);

        public static implicit operator LocalizedString(string[] str) => new LocalizedString(str);
        public static bool operator ==(LocalizedString localized, string str) => localized.IsEqual(str);
        public static bool operator !=(LocalizedString localized, string str) => !localized.IsEqual(str);
        public static bool operator ==(string str, LocalizedString localized) => localized.IsEqual(str);
        public static bool operator !=(string str, LocalizedString localized) => !localized.IsEqual(str);

        public override bool Equals(object obj)
        { 
            return obj is LocalizedString @string && EqualityComparer<string[]>.Default.Equals(strings, @string.strings);
        }
        public override int GetHashCode() => System.HashCode.Combine(strings);
    }
}
