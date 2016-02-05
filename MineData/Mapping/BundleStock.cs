using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MNT.Utility;

namespace ConsoleApplication1.Mapping
{
    public class BundleStock
    {
        
        /// <summary>
        ///日期 
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        ///股票代码 
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        ///名称 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///收盘价 
        /// </summary>
        public decimal Close { get; set; }
        /// <summary>
        ///最高价 
        /// </summary>
        public decimal High  { get; set; }
        /// <summary>
        ///最低价 
        /// </summary>
        public decimal Low { get; set; }
        /// <summary>
        ///开盘价 
        /// </summary>
        public decimal Open { get; set; }        
        /// <summary>
        /// 昨日收盘价
        /// </summary>
        public decimal YesterdayClose { get; set; }
        /// <summary>
        ///涨跌额 
        /// </summary>
        public decimal UpDown { get; set; }
        /// <summary>
        ///涨跌幅 
        /// </summary>
        public decimal Percent { get; set; }
        /// <summary>
        ///换手率 
        /// </summary>
        public decimal ChangeRate { get; set; }
        /// <summary>
        ///成交量 
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        ///成交金额 
        /// </summary>
        public decimal Turnover { get; set; }
        /// <summary>
        ///总市值 
        /// </summary>
        public decimal TotalValue { get; set; }
        /// <summary>
        ///流通市值 
        /// </summary>
        public decimal CirculationValue { get; set; }

        public override string ToString()
        {
            return
                $"{Date},{Symbol},{Name},{Close},{High},{Low},{Open},{YesterdayClose},{UpDown},{Percent},{ChangeRate},{Volume},{Turnover},{TotalValue},{CirculationValue}";
        }

        public static void SaveTo(StockInfo stockInfo, string from, string to)
        {
            var data = GetPriceFromNet(stockInfo.StockCode, from, to);

            using (var sw = new StreamWriter(CommonInfo.FilePath(stockInfo.Symbol)))
            {
                sw.Write(data);
            }
        }

        public static List<BundleStock> Load(StockInfo stockInfo)
        {
            var file = CommonInfo.FilePath(stockInfo.Symbol);
            var ret = new List<BundleStock>();
            using (var sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var aStock = ConvertFrom(line);
                    if (aStock != null)
                        ret.Add(aStock);
                }
            }
            return ret;
        }

        public static BundleStock AppendTodayStock(string stockCode, string date)
        {
            var from = date;
            var to = date;
            var data = BundleStock.GetPriceFromNet(stockCode, from, to);
            var aStockPrice = ConvertFrom(data);
            if (aStockPrice == null) return null;

            var fs = new FileStream(CommonInfo.FilePath(aStockPrice.Symbol), FileMode.Open);
            var buff = new UTF8Encoding().GetBytes(data.ToString());
            fs.Write(buff, 0, buff.Length);
            fs.Flush();
            fs.Close();

            return aStockPrice;
        }


        private static string GetPriceFromNet(string stockCode, string from, string to)
        {
            var url = BuildFetchURL(stockCode, from, to);

            var response = HttpHelper.CreateGetHttpResponse(url, 3000, null, null);
            var stream = response.GetResponseStream();
            if (stream == null) return "";
            var reader = new StreamReader(stream, System.Text.Encoding.Default);
            var title = reader.ReadLine();
            var msg = reader.ReadToEnd();
            return string.IsNullOrEmpty(msg) ? "" : msg;
        }

        public static BundleStock ConvertFrom(string singleData)
        {
            var items = singleData.Split(',');
            if (items.Length != 15) return null;

            var aStock = new BundleStock();
            aStock.Date = DateTime.Parse(items[0]);
            aStock.Symbol = items[1].TrimStart(new char[] {'\''});
            aStock.Name = items[2];
            aStock.Close = Decimal.Parse(items[3]);
            aStock.High = decimal.Parse(items[4]);
            aStock.Low = decimal.Parse(items[5]);
            aStock.Open = decimal.Parse(items[6]);
            aStock.YesterdayClose = decimal.Parse(items[7]);
            aStock.UpDown = decimal.Parse(items[8]);
            aStock.Percent = decimal.Parse(items[9]);
            aStock.ChangeRate = decimal.Parse(items[10]);
            aStock.Volume = decimal.Parse(items[11]);
            aStock.Turnover = decimal.Parse(items[12]);
            aStock.TotalValue = decimal.Parse(items[13]);
            aStock.CirculationValue = decimal.Parse(items[14]);

            return aStock;
        }

        private static string BuildFetchURL(string stockCode, string from, string to)
        {
            return "http://quotes.money.163.com/service/chddata.html?code=" + stockCode
                + "&start=" + from
                + "&end=" + to
                + "&fields=TCLOSE;HIGH;LOW;TOPEN;LCLOSE;CHG;PCHG;TURNOVER;VOTURNOVER;VATURNOVER;TCAP;MCAP";
        }

    }
}
