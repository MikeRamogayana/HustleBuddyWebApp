using HustleBuddy_Web.Models;

namespace HustleBuddy_Web.ViewModels
{
    public class CreditViewModel
    {
        public Product product { get; set; }
        public Sale sale { get; set; }
        public CreditSale creditSale { get; set; }

        public CreditViewModel(Product product, Sale sale, CreditSale creditSale)
        {
            this.product = product;
            this.sale = sale;
            this.creditSale = creditSale;
        }

        public CreditViewModel()
        {
        }
    }
}
