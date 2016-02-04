using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MNT.Utility;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
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
                        var aStockCode = items[1];
                        const string errorMsg = "_ntes_quote_callback({ });";
                         string url = string.Format("http://api.money.126.net/data/feed/0{0},money.api", aStockCode);

                        var response = HttpHelper.CreateGetHttpResponse(url, 3000, null, null);
                        var reader = new StreamReader(response.GetResponseStream());
                        var msg = reader.ReadToEnd();
                        
                        if (msg != errorMsg)
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
    }


}
