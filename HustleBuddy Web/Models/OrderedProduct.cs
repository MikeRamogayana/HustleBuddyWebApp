using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Models
{
    public class OrderedProduct
    {
        public int orderId { get; set; }
        public string productCode { get; set; }
        public int quantity { get; set; }

        public OrderedProduct(int orderId, string productCode, int quantity)
        {
            this.orderId = orderId;
            this.productCode = productCode;
            this.quantity = quantity;
        }

        public OrderedProduct()
        {
        }
    }
}
