using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
            const string update = "update";
            const string downloadAllData = "downloadAllData";
            const string giveMeSomeStocks = "giveMeSomeStocks";
            const string downloadOneDay = "downloadOneDay";

            if (args.Any())
            {
                switch (args.First())
                {
                    case update:
                        ClosingQuotation();
                        break;
                    case downloadAllData:
                        //download SH & SZ stocks in 1 year
                        var mgr = new StockManager();
                        mgr.Download(365);
                        break;
                    case giveMeSomeStocks:
                        SelectStocks();
                        break;
                    case downloadOneDay:
                        DownloadDayOnOneDay(args.Skip(1).Take(1).ToString());
                        break;

                }
                Console.WriteLine("Done.");
            }


        }

        private static void ClosingQuotation()
        {
            var mgr = new StockManager();
            mgr.DownloadToday();
        }

        private static void DownloadSpecialStockonOneDay()
        {
            var aStock = new StockInfo {Symbol = "600004"};
            var oneDay = "20160215";
            var mgr = new StockManager();
            mgr.DownloadOneDaySpecialStock(aStock, oneDay);
        }

        private static void DownloadDayOnOneDay(string oneDay)
        {
            //var oneDay = "20160215";
            var mgr = new StockManager();
            mgr.DownloadToday(oneDay);
        }

        private static void SelectStocks()
        {
            //2016 - 02 - 13
            var mgr = new StockManager();
            mgr.OutputGoldFork(@"D:\output gold fork.dat");
            mgr.OutputAbove181(@"D:\output 181.dat");
        }

        private static void SelectStocks(StockType sType)
        {
            //2016 - 02 - 13
            var mgr = new StockManager(sType);
            mgr.LimitPrice = 30.0M;
            mgr.OutputGoldFork(string.Format(@"D:\output gold fork for {0}.dat", sType.ToString()));
            mgr.OutputAbove181(string.Format(@"D:\output 181 for {0}.dat", sType.ToString()));
        }


        private static void initialStockCode()
        {
            var validCode = new List<string>();
            using (var sr = new StreamReader(@"E:\Source\MineStocks\resource\SZ\stockCode_origin.dat"))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var items = line.Split(new char[] {'(', ')'}, StringSplitOptions.RemoveEmptyEntries);

                    //if (items.Length == 2)
                    //{
                    //    var code = items[1];
                    //    string msg = GetNetStockData(code);
                        
                    //    if (!string.IsNullOrEmpty(msg))
                    //        validCode.Add(items[0] + " " + items[1]);
                    //}

                    
                    using (var sw = new StreamWriter(@"E:\Source\MineStocks\resource\SZ\stockCode.dat"))
                    {
                        for (int i = 0; i < items.Length; i += 2)
                        {
                            sw.WriteLine("{0} {1}", items[i].Trim(), items[i + 1].Trim());
                        }
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

        private static void sendMail()
        {
            var msg = new MailMessage();
            msg.To.Add("manatees@sina.com");
            msg.From = new MailAddress("manatees@126.com");
            msg.Subject = "test mail";

            msg.SubjectEncoding = Encoding.UTF8;
            msg.Body = "some stocks to list above:";
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = false;
            msg.Priority = MailPriority.Normal;

            var client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("manatees@126.com", "nokia.3230");
            client.Host = "smtp.126.com";
            var state = msg;
            try
            {
                client.SendAsync(msg, state);
            }
            catch (Exception ex)
            {
                
            }
        }
    }


}
