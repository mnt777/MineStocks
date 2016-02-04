using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNT.Utility
{
    public class Utility
    {
        public static string GetUtf8(string unicodeString)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodeBytes = utf8.GetBytes(unicodeString);
            string decodedString = utf8.GetString(encodeBytes);
            return decodedString;
        }
    }
}
