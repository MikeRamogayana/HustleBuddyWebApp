using HustleBuddy_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.ViewModels
{
    public class StockViewModel
    {
        public DailyStock dailyStock { get; set; }
        public DailyExpense dailyExpense { get; set; }
        public List<TradingStock> tradingStocks { get; set; }

        public StockViewModel(DailyStock dailyStock, DailyExpense dailyExpense, List<TradingStock> tradingStocks)
        {
            this.dailyStock = dailyStock;
            this.dailyExpense = dailyExpense;
            this.tradingStocks = tradingStocks;
        }

        public StockViewModel()
        {
        }
    }
}
