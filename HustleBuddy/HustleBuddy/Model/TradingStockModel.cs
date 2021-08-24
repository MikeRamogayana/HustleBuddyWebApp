using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOSG_Web_App.Model
{
    public class TradingStockModel
    {
        public string productCode { get; set; }
        public int stockId { get; set; }
        public int quantity { get; set; }
        public int sold { get; set; }

        public TradingStockModel(string productCode, int stockId, int quantity, int sold)
        {
            this.productCode = productCode;
            this.stockId = stockId;
            this.quantity = quantity;
            this.sold = sold;
        }

        public TradingStockModel()
        {

        }
    }
}