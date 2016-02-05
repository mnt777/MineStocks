using System;
using System.Collections.Generic;
using System.IO;
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

        public static string GetUtf32(string unicodeString)
        {
            var utf8 = new UTF32Encoding();
            byte[] encodeBytes = utf8.GetBytes(unicodeString);
            string decodedString = utf8.GetString(encodeBytes);
            return decodedString;
        }
    }

    public class CommonInfo
    {
        public const string fetchURL = "http://api.money.126.net/data/feed/{0},money.api";
        public const string errorMsg = "_ntes_quote_callback({ });";
        //public const string headerFmt = "_ntes_quote_callback({\"0603969\":";
        public const string tailerFmt = " });";

        public const string resourcePath = "../../../resource/";
        public const string SHStockCodePosition = "../../../resource/SH/StockCode_SH.dat";


        public static string FilePath(string stockSymbol)
        {
            var path = resourcePath + "Data/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + stockSymbol + ".dat";

        }

    }
}
