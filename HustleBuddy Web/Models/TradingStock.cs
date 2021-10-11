namespace HustleBuddy_Web.Models
{
    public class TradingStock
    {
        public string productCode { get; set; }
        public int stockId { get; set; }
        public int quantity { get; set; }
        public int sold { get; set; }

        public TradingStock(string productCode, int stockId, int quantity, int sold)
        {
            this.productCode = productCode;
            this.stockId = stockId;
            this.quantity = quantity;
            this.sold = sold;
        }

        public TradingStock()
        {
        }
    }
}
