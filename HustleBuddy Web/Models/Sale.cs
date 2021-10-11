using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Models
{
    public class Sale
    {
        public int saleId { get; set; }
        public string productCode { get; set; }
        public int vendorId { get; set; }
        public int quantity { get; set; }
        public string location { get; set; }
        public DateTime date { get; set; }
        public string cashOrCredit { get; set; }

        public Sale(int saleId, string productCode, int vendorId, int quantity, string location, DateTime date, string cashOrCredit)
        {
            this.saleId = saleId;
            this.productCode = productCode;
            this.vendorId = vendorId;
            this.quantity = quantity;
            this.location = location;
            this.date = date;
            this.cashOrCredit = cashOrCredit;
        }

        public Sale()
        {
        }
    }
}
