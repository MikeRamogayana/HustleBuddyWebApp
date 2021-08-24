using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOSG_Web_App.Model
{
    public class ProductModel
    {
        public string productCode { get; set; }
        public int vendorId { get; set; }
        public string productName { get; set; }
        public string description { get; set; }
        public double costPrice { get; set; }
        public double sellingPrice { get; set; }
        public double discountPrice { get; set; }

        public ProductModel(string productCode, int vendorId, 
            string productName, string description, double costPrice,
            double sellingPrice, double discountPrice)
        {
            this.productCode = productCode;
            this.vendorId = vendorId;
            this.productName = productName;
            this.description = description;
            this.costPrice = costPrice;
            this.sellingPrice = sellingPrice;
            this.discountPrice = discountPrice;
        }

        public ProductModel()
        {

        }
    }
}