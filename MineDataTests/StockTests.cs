using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApplication1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Mapping;

namespace ConsoleApplication1.Tests
{
    [TestClass()]
    public class StockTests
    {
        private StockInfo stock603969;
        private StockInfo stock000016;

        [TestInitialize()]
        public void Init()
        {
            stock603969 = new StockInfo();
            stock603969.StockType = StockType.SH;
            stock603969.Symbol = "603969";

            stock000016 = new StockInfo
            {
                StockType = StockType.SZ,
                Symbol = "000016",
            };
        }


        [TestMethod()]
        public void GetRealTimeStockTest()
        {
            var aStock = new Stock(stock603969);
            var da = aStock.GetRealTimeInfo();

            Assert.IsNotNull(da);
            Assert.AreEqual(stock603969.Symbol, da.symbol);
            Assert.AreEqual(StockType.SH.ToString(), da.type);


        }

        [TestMethod()]
        public void DownloadTodaySpecialStockTest()
        {
            var mgr = new StockManager();
            mgr.DownloadTodaySpecialStock(stock603969);

        }

        [TestMethod()]
        public void DownloadSpecialStockTest()
        {
            var mgr = new StockManager();
            mgr.DownloadSpecialStock(stock603969, "20150101", "20160204");

        }

        [TestMethod()]
        public void GetBundleStockSZTest()
        {
            var mgr = new StockManager();
            mgr.Download("20151114", "20160202");
        }

        [TestMethod()]
        public void GetMA5()
        {
            var aStock = new Stock(stock603969);
            var val = aStock.MAForDay(5);
            Assert.AreEqual(12.68m, Math.Round(val[0], 2));
            Assert.AreEqual(12.42M, Math.Round(val[1], 2));
            Assert.AreEqual(12.35m, Math.Round(val[2], 2));
        }

        [TestMethod()]
        public void GetMA20()
        {
            var aStock = new Stock(stock603969);
            var val = aStock.MAForDay(20);
            Assert.AreEqual(13.49m, val);
        }

        [TestMethod()]
        public void GetMA181()
        {
            var aStock = new Stock(stock603969);
            var val = aStock.MAForDay(181);
            Assert.AreEqual(18.53m, val);
        }

        [TestMethod()]
        public void GetMA5ForWeek()
        {
            var aStock = new Stock(stock603969);
            var val = aStock.MAForWeek(5);
            Assert.AreEqual(13.77m, Math.Round(val[0], 2));
            Assert.AreEqual(15.13M, Math.Round(val[1], 2));
            Assert.AreEqual(16.45m, Math.Round(val[2], 2));
        }

    }
}