using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Mapping;
using MNT.JsonHelper;
using MNT.Utility;

namespace ConsoleApplication1
{

    public enum StockType
    {
        SH,
        SZ,
    }
    public class Stock
    {

        private StockInfo stockInfo;
        private List<BundleStock> History = new List<BundleStock>();

        public Stock(StockInfo info)
        {
            this.stockInfo = info;
        }

        public void LoadHistory()
        {
            History = BundleStock.Load(stockInfo);
        }

        public List<decimal> MAForDay(int day)
        {            
            if (History.Count() < day) LoadHistory();
            
            return GetMALine(History, day);
        }

        public List<decimal> MAForWeek(int day)
        {
            if (History.Count() == 0) LoadHistory();
            var data = History.Where(it => it.Date.DayOfWeek == DayOfWeek.Friday);
            return GetMALine(data, day);
        }

        private List<decimal> GetMALine(IEnumerable<BundleStock> historyData, int day)
        {
            var ret = new List<decimal>();
            const int dataLength = 5;
            var loopStop = History.Count() / day;

            if (loopStop > dataLength) loopStop = dataLength;

            for (int i = 0; i < loopStop; i++)
            {
                var data = historyData.OrderByDescending(it => it.Date).Skip(i).Take(day);
                var sum = data.Sum(it => it.Close);
                ret.Add(sum/day*1.0M);
            }
            return ret;
        }

        public DailyStock GetRealTimeInfo()
        {
            var originData = GetNetStockData(stockInfo.StockCode);
            var header = BuildHeader();
            var d = originData.Substring(header.Length, originData.Length - header.Length - CommonInfo.tailerFmt.Length);
            var aRealTimeStock = JsonHelper.DeserializeJsonToObject<DailyStock>(d);
            return aRealTimeStock;
        }

        private string BuildHeader()
        {
            return "_ntes_quote_callback({\"" + stockInfo.StockCode + "\":";
        }

        private  string GetNetStockData(string stockCode)
        {
            string url = string.Format(CommonInfo.fetchURL, stockCode);
            var response = HttpHelper.CreateGetHttpResponse(url, 3000, null, null);
            var reader = new StreamReader(response.GetResponseStream());
            var msg = reader.ReadToEnd();
            if (msg == CommonInfo.errorMsg) msg = "";
            return msg;
        }
    }
}
