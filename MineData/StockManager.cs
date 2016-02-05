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

        public string Select()
        {
            var sb = new StringBuilder();
            foreach (var sinfo in stockInfos)
            {
                var aStock = new Stock(sinfo);
                var MA5s = aStock.MAForWeek(5);
                var MA40s = aStock.MAForWeek(40);

                var ma5 = MA5s.FirstOrDefault();
                var ma40 = MA40s.FirstOrDefault();

                var ma5_1 = MA5s.Skip(1).FirstOrDefault();
                var ma40_1 = MA40s.Skip(1).FirstOrDefault();
                

                if (ma5_1 < ma40_1 && ma5 > ma40)
                    sb.AppendLine(sinfo.Symbol);
            }
            return sb.ToString();
        }


        public string Select2()
        {
            var sb = new StringBuilder();
            foreach (var info in stockInfos)
            {
                var aStock = new Stock(info);
                var ma181s = aStock.MAForDay(181);
                var ma181 = ma181s.FirstOrDefault();
                
                var T = aStock.GetRealTimeInfo();
                var p = 0M;
                decimal.TryParse(T.price, out p);
                if (ma181 > 0 && p > ma181)
                    sb.AppendLine(info.Symbol);

            }
            return sb.ToString();
        }

        public BundleStock DownloadTodaySpecialStock(StockInfo stockInfo)
        {
            return BundleStock.AppendTodayStock(stockInfo.StockCode, DateTime.Now.ToString("yyyyMMdd"));
        }

        public void DownloadSpecialStock(StockInfo stockInfo, string from, string to)
        {
            BundleStock.SaveTo(stockInfo, from, to);
        }

        /// <summary>
        /// insert today's prices
        /// </summary>
        public void DownloadToday()
        {
            var currentDate = DateTime.Now.ToString("yyyyMMdd");

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
