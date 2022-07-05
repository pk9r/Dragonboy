using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK
{
    public class Utilities
    {
        public static string Base64StringEncode(string originalString)
        {
            var bytes = Encoding.UTF8.GetBytes(originalString);

            var encodedString = Convert.ToBase64String(bytes);

            return encodedString;
        }

        public static string Base64StringDecode(string encodedString)
        {
            var bytes = Convert.FromBase64String(encodedString);

            var decodedString = Encoding.UTF8.GetString(bytes);

            return decodedString;
        }
    }
}
