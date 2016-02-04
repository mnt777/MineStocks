using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MNT.Utility;
using System.Text.RegularExpressions;
using ConsoleApplication1.Mapping;
using MNT.JsonHelper;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //var path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;


            //var currentDate = DateTime.Now;
            //var before181 = currentDate.AddDays(-365);

            //var from = before181.ToString("yyyyMMdd");
            //var to = currentDate.ToString("yyyyMMdd");

            //using (var sr = new StreamReader("../../../resource/SH/StockCode_SH.dat"))
            //{
            //    while (!sr.EndOfStream)
            //    {
            //        var line = sr.ReadLine();
            //        var items = line.Split(' ');
            //        if (items.Length == 2)
            //        {
            //            var code = items[1];
            //            var aStock = new Stock(code, StockType.SH);
            //            aStock.GetBundleStock(from, to, false);                        
            //        }
            //    }
            //}


            var mgr = new StockManager();
            mgr.Download(365);

            Console.WriteLine("done.");
            Console.ReadKey();
        }


        

        private static void Fun()
        {
            var validCode = new List<string>();
            using (var sr = new StreamReader(@"D:\SH\StockCode_SH.dat"))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var items = line.Split(new char[] {'(', ')'}, StringSplitOptions.RemoveEmptyEntries);

                    if (items.Length == 2)
                    {
                        var code = items[1];
                        string msg = GetNetStockData(code);
                        
                        if (!string.IsNullOrEmpty(msg))
                            validCode.Add(items[0] + " " + items[1]);
                    }
                }

                using (var sw = new StreamWriter(@"D:\SH\output.dat"))
                {
                    foreach (var code in validCode)
                    {
                        sw.WriteLine(code);
                    }
                }
            }
        }

        private static string GetNetStockData(string stockCode)
        {
            const string fetchURL = "http://api.money.126.net/data/feed/0{0},money.api";
            const string errorMsg = "_ntes_quote_callback({ });";
            string url = string.Format(fetchURL, stockCode);
            
            var response = HttpHelper.CreateGetHttpResponse(url, 3000, null, null);
            var reader = new StreamReader(response.GetResponseStream());
            var msg = reader.ReadToEnd();
            if (msg == errorMsg) msg = "";
            return msg;
        }
    }


}
