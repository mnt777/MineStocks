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

        /// <summary>
        /// is in same week
        /// </summary>
        /// <param name="d1">start date</param>
        /// <param name="d2">end date</param>
        /// <returns></returns>
        public static bool IsInSameWeek(DateTime d1, DateTime d2)
        {
            var d1Week = (int)d1.DayOfWeek;
            var d2Week = (int)d2.DayOfWeek;
            if (d1Week == 0) d1Week = 7;
            if (d2Week == 0) d2Week = 7;

            var diff = Math.Abs((d1 - d2).Days);

            if (diff > 7)
                return false;
            if (diff == 7 && d1Week == d2Week)
                return false;

            return diff <= (7 - d1Week);
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
        public const string SZStockCodePosition = "../../../resource/SZ/StockCode_SZ.dat";

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
