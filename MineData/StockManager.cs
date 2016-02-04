using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class StockManager
    {
        public string StockCodePosition => "../../../ resource / SH / StockCode_SH.dat";



        public void Download(int day)
        {
            var to = DateTime.Now.ToString("yyyyMMdd");
            var from = DateTime.Now.AddDays(day*-1).ToString("yyyyMMdd");
            Download(from, to, false);
        }

        public void Download(string from, string to, bool isAppend)
        {
            var path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            using (var sr = new StreamReader(StockCodePosition))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var items = line.Split(' ');
                    if (items.Length == 2)
                    {
                        var code = items[1];
                        var aStock = new Stock(code, StockType.SH);
                        aStock.GetBundleStock(from, to, isAppend);
                    }
                }
            }
        }
    }
}
