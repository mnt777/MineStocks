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
