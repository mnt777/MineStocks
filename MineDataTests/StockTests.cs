using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApplication1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Tests
{
    [TestClass()]
    public class StockTests
    {
        [TestMethod()]
        public void GetRealTimeStockTest()
        {
            var stockCode = "603969";
            var aStock = new Stock(stockCode, StockType.SH);
            var da = aStock.GetRealTimeInfo();

            Assert.IsNotNull(da);
            Assert.AreEqual(stockCode, da.symbol);
            Assert.AreEqual(StockType.SH.ToString(), da.type);


        }

        [TestMethod()]
        public void GetBundleStockSHTest()
        {
            var stockCode = "603969";
            var aStock = new Stock(stockCode, StockType.SH);
            var da = aStock.GetBundleStock("20160114", "20160202", false);
        }

        [TestMethod()]
        public void GetBundleStockSZTest()
        {
            var stockCode = "000016";
            var aStock = new Stock(stockCode, StockType.SZ);
            var da = aStock.GetBundleStock("20160114", "20160202", false);
        }
    }
}