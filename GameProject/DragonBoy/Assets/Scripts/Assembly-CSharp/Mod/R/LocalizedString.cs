using System.Collections.Generic;
using System.Linq;

namespace Mod.R
{
    /// <summary>
    /// Class chứa các chuỗi của 3 ngôn ngữ: tiếng Việt, tiếng Anh và tiếng Indonesia, dùng để kiểm tra và so sánh với các chuỗi của game do server gửi về
    /// </summary>
    internal class LocalizedString
    {
        internal static LocalizedString[] xmapCantGoHereKeywords = new LocalizedString[]
        {
            new string[3]
            {
                "Bạn chưa thể đến khu vực này",
                "",
                ""
            },
            new string[3]
            {

                "Bang hội phải có từ 5 thành viên mới được tham gia",
                "",
                ""
            },
            new string[3]
            {
                "Chỉ tiếp các bang hội, miễn tiếp khách vãng lai",
                "",
                ""
            },
            new string[3]
            {
                "Gia nhập bang hội trên 2 ngày mới được tham gia",
                "",
                ""
            },
        };
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
            "tantang karin"
        };
        internal static LocalizedString acceptChallenge = new string[3]
        {
            "đồng ý giao đấu",
            "accept fight",
            "accept fight"
        };
        internal static LocalizedString mercenaryTao = new string[3]
        {
            "Tàu Pảy Pảy",
            "Taopaipai",
            "Taopaipai"
        };  
        internal static LocalizedString saoMayLuoiThe = new string[3]
        {
            "sao sư phụ không đánh đi",
            "",  //I don't know what this string is in World server
            ""  //I don't know what this string is in Indonaga server
        };
        internal static LocalizedString errorOccurred = new string[3]
        {
            "Có lỗi xảy ra vui lòng thử lại sau.",
            "",
            ""
        };
        internal static LocalizedString goHome = new string[3]
        {
            "Về nhà",
            "Go home",
            "Pulang"
        };
        internal static LocalizedString spaceshipStation = new string[3]
        {
            "Trạm tàu vũ trụ",
            "Spaceship station",
            "Spaceship station"
        };
        internal static LocalizedString backTo = new string[3]
        {
            "Về chỗ cũ",
            "Back to",
            "Kembali ke"
        };
        internal static LocalizedString stoneForest = new string[3]
        {
            "Rừng đá",
            "Stone forest",
            "Stone forest"
        };
        internal static LocalizedString arbitration = new string[3]
        {
            "Trọng tài",
            "Arbitration",
            "Arbitration"
        };
        internal static LocalizedString cantChangeZoneInThisMap = new string[3]
        {
            "Không thể đổi khu vực trong map này",
            "Can not change zone in this map",
            "Tidak bisa mengganti zone di map ini"
        };
        internal static LocalizedString senzuTreeUpgrading = new string[3]
        {
            "Đang nâng cấp",
            "Upgrading",
            "Sedang mengupgrade"
        };
        internal static LocalizedString senzuBeanHarvested = new string[3]
        {
            "Bạn vừa thu hoạch được",
            "You just harvested",
            "Kamu baru saja memanen"
        };
        internal static LocalizedString free1hCharmReceived = new string[3]
        {
            "Bạn vừa nhận thưởng bùa",
            "You receive award",
            "Kamu menerima hadiah"
        };
        internal static LocalizedString getGift = new string[3]
        {
            "Nhận quà",
            "Get Gift",
            "Menerima Gift"
        };
        internal static LocalizedString rejectGift = new string[3]
        {
            "Từ chối",
            "Reject",
            "Tolak"
        };
        internal static LocalizedString talk = new string[3]
        {
            "Nói chuyện",
            "Talk",
            "Bicara"
        };
        internal static LocalizedString mission = new string[3]
        {
            "Nhiệm vụ",
            "Quest",
            "Misi"
        };
        #region Methods and fields
        string[] strings;

        LocalizedString(string[] strings) => this.strings = strings;

        /// <summary>
        /// Thay thế tất cả <see langword="this"/> xuất hiện trong <paramref name="original"/> thành <paramref name="newValue"/>
        /// </summary>
        /// <param name="original">Chuỗi cần thay thế</param>
        /// <param name="newValue">Chuỗi để thay thế</param>
        internal string Replace(string original, string newValue)
        {
            foreach (string item in strings)
                original = original.Replace(item, newValue);
            return original;
        }

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
        public static implicit operator string(LocalizedString localized) => localized.strings[0];
        public static bool operator ==(LocalizedString localized, string str) => localized.IsEqual(str);
        public static bool operator !=(LocalizedString localized, string str) => !localized.IsEqual(str);
        public static bool operator ==(string str, LocalizedString localized) => localized.IsEqual(str);
        public static bool operator !=(string str, LocalizedString localized) => !localized.IsEqual(str);

        public override bool Equals(object obj)
        { 
            return obj is LocalizedString @string && EqualityComparer<string[]>.Default.Equals(strings, @string.strings);
        }
        public override int GetHashCode() => System.HashCode.Combine(strings);
        #endregion
    }
}
