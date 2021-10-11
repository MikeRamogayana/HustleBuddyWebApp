using HustleBuddy_Web.Models;

namespace HustleBuddy_Web.ViewModels
{
    public class StockAddModel
    {
        public DailyStock dailyStock { get; set; }
        public DailyExpense dailyExpense { get; set; }
        public string product_json { get; set; }

        public StockAddModel(DailyStock dailyStock, DailyExpense dailyExpense, string product_json)
        {
            this.dailyStock = dailyStock;
            this.dailyExpense = dailyExpense;
            this.product_json = product_json;
        }

        public StockAddModel()
        {
        }
    }
}
