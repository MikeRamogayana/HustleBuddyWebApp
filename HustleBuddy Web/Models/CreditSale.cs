using System;

namespace HustleBuddy_Web.Models
{
    public class CreditSale
    {
        public int creditId { get; set; }
        public int saleId { get; set; }
        public int vendorId { get; set; }
        public string customerName { get; set; }
        public string contactNumber { get; set; }
        public double creditAmount { get; set; }
        public double paidAmount { get; set; }
        public DateTime dueDate { get; set; }
        public DateTime payDate { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }

        public CreditSale(int creditId, int saleId, int vendorId, string customerName, string contactNumber, double creditAmount, double paidAmount, DateTime dueDate, DateTime payDate, DateTime date, string status)
        {
            this.creditId = creditId;
            this.saleId = saleId;
            this.vendorId = vendorId;
            this.customerName = customerName;
            this.contactNumber = contactNumber;
            this.creditAmount = creditAmount;
            this.paidAmount = paidAmount;
            this.dueDate = dueDate;
            this.payDate = payDate;
            this.date = date;
            this.status = status;
        }

        public CreditSale()
        {
        }
    }
}
