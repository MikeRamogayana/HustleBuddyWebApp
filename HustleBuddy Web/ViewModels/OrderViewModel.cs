using HustleBuddy_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.ViewModels
{
    public class OrderViewModel
    {
        public Order order { get; set; }
        public List<OrderedProduct> orderedProducts { get; set; }
        public List<Product> products { get; set; }

        public OrderViewModel(Order order, List<OrderedProduct> orderedProducts, List<Product> products)
        {
            this.order = order;
            this.orderedProducts = orderedProducts;
            this.products = products;
        }

        public OrderViewModel()
        {
        }
    }
}
