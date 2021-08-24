using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOSG_Web_App.Model
{
    public class SaleModel
    {
        public int saleId { set; get; }
        public string productCode { set; get; }
        public int vendorId { set; get; }
        public int quantity { set; get; }
        public string location { set; get; }
        public DateTime date { set; get; }

        public SaleModel(int saleId, string productCode, int vendorId, int quantity, string location, DateTime date)
        {
            this.saleId = saleId;
            this.productCode = productCode;
            this.vendorId = vendorId;
            this.quantity = quantity;
            this.location = location;
            this.date = date;
        }

        public SaleModel()
        {

        }
    }
}