using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOSG_Web_App.Model
{
    public class OrderedProductModel
    {
        public int orderId { get; set; }
        public string productCode { get; set; }
        public int quantity { get; set; }

        public OrderedProductModel(int orderId, string productCode, int quantity)
        {
            this.orderId = orderId;
            this.productCode = productCode;
            this.quantity = quantity;
        }

        public OrderedProductModel()
        {

        }
    }
}