using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Mapping
{
    public class DailyStock
    {
        //arrow: "↓"
        public string arrow { get; set; }

        //ask1: 47.35
        //ask2: 47.36
        //ask3: 47.37
        //ask4: 47.38
        //ask5: 47.39//卖1 价 47.35
        /// <summary>
        /// 卖1价
        /// </summary>
        public string ask1 { get; set; }
        /// <summary>
        /// 卖2价
        /// </summary>
        public string ask2 { get; set; }
        /// <summary>
        /// 卖3价
        /// </summary>
        public string ask3 { get; set; }
        /// <summary>
        /// 卖4价
        /// </summary>
        public string ask4 { get; set; }
        /// <summary>
        /// 卖5价
        /// </summary>
        public string ask5 { get; set; }

        //askvol1: 700					//卖1 量 700股
        //askvol2: 400
        //askvol3: 400
        //askvol4: 400
        //askvol5: 1871
        /// <summary>
        /// 卖1量（股)
        /// </summary>
        public string askvol1 { get; set; }
        /// <summary>
        /// 卖2量（股)
        /// </summary>
        public string askvol2 { get; set; }
        /// <summary>
        /// 卖3量（股)
        /// </summary>
        public string askvol3 { get; set; }
        /// <summary>
        /// 卖4量（股)
        /// </summary>
        public string askvol4 { get; set; }
        /// <summary>
        /// 卖5量（股)
        /// </summary>
        public string askvol5 { get; set; }

//bid1: 47.32						//买1
//bid2: 47.31						//买2
//bid3: 47.28						//
//bid4: 47.26						//
//bid5: 47.25						//
        /// <summary>
        /// 买1价
        /// </summary>
        public string bid1 { get; set; }
        /// <summary>
        /// 买2价
        /// </summary>
        public string bid2 { get; set; }
        /// <summary>
        /// 买3价
        /// </summary>
        public string bid3 { get; set; }
        /// <summary>
        /// 买4价
        /// </summary>
        public string bid4 { get; set; }
        /// <summary>
        /// 买5价
        /// </summary>
        public string bid5 { get; set; }

        //bidvol1: 8600					//买1 量 8600股
        //bidvol2: 31500
        //bidvol3: 100
        //bidvol4: 1400
        //bidvol5: 7800


        /// <summary>
        /// 买1量（股）
        /// </summary>
        public string bidvol1 { get; set; }
        /// <summary>
        /// 买2量（股）
        /// </summary>
        public string bidvol2 { get; set; }
        /// <summary>
        /// 买3量（股）
        /// </summary>
        public string bidvol3 { get; set; }
        /// <summary>
        /// 买4量（股）
        /// </summary>
        public string bidvol4 { get; set; }
        /// <summary>
        /// 买5量（股）
        /// </summary>
        public string bidvol5 { get; set; }

        /// <summary>
        /// 股票编码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 股票代码
        /// </summary>
        public string symbol { get; set; }
        /// <summary>
        /// 股票名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 上证SH, 深证SZ 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 昨日收盘价
        /// </summary>
        public string yestclose { get; set; }
        /// <summary>
        /// 最高价
        /// </summary>
        public string high { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public string low { get; set; }
        /// <summary>
        /// 开盘价
        /// </summary>
        public string open { get; set; }
        /// <summary>
        /// 当前价， 收盘价
        /// </summary>
        public string price { get; set; }
        /// <summary>
        /// 涨跌幅
        /// </summary>
        public string percent { get; set; }
        /// <summary>
        /// 涨跌额
        /// </summary>
        public string updown { get; set; }
        /// <summary>
        /// 成交量
        /// </summary>
        public string volume { get; set; }
        /// <summary>
        /// 成交额
        /// </summary>
        public string turnover { get; set; }


        public string status { get; set; }
        public string   time { get; set; }
        public string update { get; set; }

    }
}
