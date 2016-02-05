using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MNT.Utility;

namespace ConsoleApplication1.Mapping
{
    public class StockInfo
    {
        public string StockCode {
            get
            {
                if (StockType == StockType.SH) return "0" + Symbol;
                if (StockType == StockType.SZ) return "1" + Symbol;
                return "";
            }
        }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public StockType StockType { get; set; }


        public static List<StockInfo> GetSHStockCodes()
        {
            return GetStocksFor(CommonInfo.SHStockCodePosition, StockType.SH);
        }

        public static List<StockInfo> GetStocksFor(string stockCodePosition, StockType stockType)
        {
            var stocks = new List<StockInfo>();
            using (var sr = new StreamReader(stockCodePosition))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var items = line.Split(new char[] {' '});
                    var aStockInfo = new StockInfo();

                    aStockInfo.Name = items[0];
                    aStockInfo.Symbol = items[1];
                    aStockInfo.StockType = stockType;

                    stocks.Add(aStockInfo);
                }
            }
            return stocks;
        }        
    }
}
