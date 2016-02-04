using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Mapping;
using MNT.JsonHelper;

namespace ConsoleApplication1
{

    public enum StockType
    {
        SH,
        SZ,
    }
    public class Stock
    {
        const string fetchURL = "http://api.money.126.net/data/feed/{0},money.api";
        const string errorMsg = "_ntes_quote_callback({ });";
        //const string headerFmt = "_ntes_quote_callback({\"0603969\":";
        const string tailerFmt = " });";

        private string _path => "../../../resource/"; //System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        private string symbol;
        private StockType stockType;

        public string stockCode
        {
            get
            {
                if (stockType == StockType.SH)
                {
                    return "0" + symbol;
                }
                else
                {
                    return "1" + symbol;
                }
            }
        }

        public Stock(string code, StockType stockType)
        {
            symbol = code;
            this.stockType = stockType;
        }

        public DailyStock GetRealTimeInfo()
        {

            var originData = GetNetStockData(stockCode);
            var header = BuildHeader();
            var d = originData.Substring(header.Length, originData.Length - header.Length - tailerFmt.Length);
            var aRealTimeStock = JsonHelper.DeserializeJsonToObject<DailyStock>(d);
            return aRealTimeStock;
        }

        public object GetBundleStock(string from, string to, bool isAppendData)
        {

            var url = BuildFetchURL(stockCode, from, to);

            var response = HttpHelper.CreateGetHttpResponse(url, 3000, null, null);
            var stream = response.GetResponseStream();
            if (stream == null) return null;
            var reader = new StreamReader(stream, System.Text.Encoding.Default);
            var title = reader.ReadLine();
            var msg = reader.ReadToEnd();
            if (string.IsNullOrEmpty(msg)) return null;

            Save(msg, isAppendData);
            return null;
        }

        private void Save(string data, bool isAppend)
        {

            using (var sw = new StreamWriter(FilePath(), isAppend)) 
            {
                sw.Write(data);
            }
        }

        private string FilePath()
        {
            var path = _path + "\\Data\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + symbol + ".dat";
            
        }

        private string BuildHeader()
        {
            return "_ntes_quote_callback({\"" + stockCode + "\":";
        }

        private string BuildFetchURL(string stockCode, string from, string to)
        {
            return "http://quotes.money.163.com/service/chddata.html?code="+stockCode 
                +"&start="+ from 
                +"&end="+ to
                +"&fields=TCLOSE;HIGH;LOW;TOPEN;LCLOSE;CHG;PCHG;TURNOVER;VOTURNOVER;VATURNOVER;TCAP;MCAP";
        }
        private  string GetNetStockData(string stockCode)
        {
            string url = string.Format(fetchURL, stockCode);
            var response = HttpHelper.CreateGetHttpResponse(url, 3000, null, null);
            var reader = new StreamReader(response.GetResponseStream());
            var msg = reader.ReadToEnd();
            if (msg == errorMsg) msg = "";
            return msg;
        }
    }
}
