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


        public StockInfo Info
        {
            get { return stockInfo; }
        }

        public BundleStock LatestPrice
        {
            get
            {
                LoadHistory();
                return History.FirstOrDefault();
            }
        }

        public Stock(StockInfo info)
        {
            this.stockInfo = info;
        }

        public void LoadHistory()
        {
            if (!History.Any())
                History = BundleStock.Load(stockInfo);
        }

        public List<decimal> MAForDay(int day)
        {            
            LoadHistory();            
            return GetMALine(History, day);
        }

        public List<decimal> MAForWeek(int day)
        {
            //LoadHistory();
            //var data = History.Where(it => it.Date.DayOfWeek == DayOfWeek.Friday);
            var data = GenerateWeekLineData();
            return GetMALine(data, day);
        }

        private List<decimal> GetMALine(IEnumerable<BundleStock> historyData, int day)
        {
            var ret = new List<decimal>();
            const int dataLength = 5;
            var loopStop = historyData.Count() / day;

            if (loopStop > dataLength) loopStop = dataLength;

            for (int i = 0; i < loopStop; i++)
            {
                var data = historyData.OrderByDescending(it => it.Date).Skip(i).Take(day);
                //var sum = data.Sum(it => it.Close);
                var sum = 0M;
                foreach (var item in data)
                {
                    sum += item.Close;
                    sum = Math.Round(sum, 2);
                }
                ret.Add(sum/day*1.0M);
            }
            return ret;
        }

        public IList<BundleStock> GenerateWeekLineData()
        {
            LoadHistory();

            //按星期切分时间

            //initial week list
            var weekList = new List<List<BundleStock>> {new List<BundleStock>()};

            if (!History.Any()) return new List<BundleStock>();
            //fill first date
            var pos = 0;
            weekList[pos].Add(History[pos]);
            
            for (int i = 1; i < History.Count; i++)
            {
                var cl = weekList[pos];
                var pWeek = (int)cl.Last().Date.DayOfWeek;
                var cWeek = (int)History[i].Date.DayOfWeek;

                if (cWeek != 0 && cWeek < pWeek && Utility.IsInSameWeek(History[i].Date, cl.Last().Date) || (cl.Count == 1 && pWeek == 0))
                {
                    cl.Add(History[i]);
                }
                else
                {
                    weekList.Add(new List<BundleStock>());
                    pos++;
                    weekList[pos].Add(History[i]);
                }
            }

            return weekList.Select(d => d.First()).ToList();
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
