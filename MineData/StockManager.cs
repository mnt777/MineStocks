using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Mapping;
using MNT.Utility;

namespace ConsoleApplication1
{
    public class StockManager
    {
        public string StockCodePosition = CommonInfo.SHStockCodePosition;

        private List<StockInfo> stockInfos;

        public StockManager()
        {
            stockInfos = StockInfo.GetSHStockCodes();
        }

        public List<Stock> SelectGold5Fork40()
        {            
            var ret = new List<Stock>();
            foreach (var sinfo in stockInfos)
            {
                var aStock = new Stock(sinfo);

                //取出5点
                var MA5s = aStock.MAForWeek(5);
                var MA40s = aStock.MAForWeek(40);

                //1点
                var ma5 = MA5s.FirstOrDefault();
                var ma40 = MA40s.FirstOrDefault();

                //2点
                var ma5_1 = MA5s.Skip(1).FirstOrDefault();
                var ma40_1 = MA40s.Skip(1).FirstOrDefault();

                if (ma5_1 < ma40_1 && ma5 > ma40)
                    ret.Add(aStock);
            }

            return ret;
        }
        public List<Stock> SelectAbove181()
        {
            var ret = new List<Stock>();
            foreach (var info in stockInfos)
            {
                var aStock = new Stock(info);

                // 181's line
                var ma181s = aStock.MAForDay(181);
                var ma181 = ma181s.FirstOrDefault();

                var T = aStock.GetRealTimeInfo();
                var p = 0M;
                decimal.TryParse(T.price, out p);

                if (ma181 > 0 && p > ma181)
                    ret.Add(aStock);
            }
            return ret;
        }

        public void OutputGoldFork(string fileName)
        {
            var stocks = SelectGold5Fork40();
            OutputToLocal(fileName, stocks);
        }

        public void OutputAbove181(string fileName)
        {
            var stocks = SelectAbove181();
            OutputToLocal(fileName, stocks);
        }

        private static void OutputToLocal(string fileName, IEnumerable<Stock> stocks)
        {
            using (var sw = new StreamWriter(fileName))
            {
                foreach (var stock in stocks)
                {
                    if (!stock.Info.Name.Contains("B") && stock.LatestPrice.Close > 0 && stock.LatestPrice.Close < 20)
                        sw.WriteLine("{0},{1}", stock.Info.Symbol, stock.Info.Name);
                }
            }
        }


        public BundleStock DownloadTodaySpecialStock(StockInfo stockInfo)
        {
            return BundleStock.AppendTodayStock(stockInfo.StockCode, DateTime.Now.ToString("yyyyMMdd"));
        }

        public BundleStock DownloadOneDaySpecialStock(StockInfo stockInfo, string strDate)
        {
            return BundleStock.AppendTodayStock(stockInfo.StockCode, strDate);
        }

        public void DownloadSpecialStock(StockInfo stockInfo, string from, string to)
        {
            BundleStock.SaveTo(stockInfo, from, to);
        }

        /// <summary>
        /// insert today's prices
        /// </summary>
        public void DownloadToday(string strDate = "")
        {
            string currentDate = string.IsNullOrEmpty(strDate) ? DateTime.Now.ToString("yyyyMMdd") : strDate;

            foreach (var aStockCodeInfo in stockInfos)
            {
                BundleStock.AppendTodayStock(aStockCodeInfo.StockCode, currentDate);
            }
        }

        /// <summary>
        /// Download, data will be recovered.
        /// </summary>
        /// <param name="day"></param>
        public void Download(int day)
        {
            var to = DateTime.Now.ToString("yyyyMMdd");
            var from = DateTime.Now.AddDays(day*-1).ToString("yyyyMMdd");
            Download(from, to);
        }

        public void Download(string from, string to)
        {
            foreach (var stock in stockInfos)
            {
                BundleStock.SaveTo(stock, from, to);
            }

        }
    }
}
