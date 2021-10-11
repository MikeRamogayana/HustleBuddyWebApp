using HustleBuddy_Web.Models;

namespace HustleBuddy_Web.ViewModels
{
    public class OrderAddModel
    {
        public Order order { get; set; }
        public string products_json { get; set; }

        public OrderAddModel(Order order, string products_json)
        {
            this.order = order;
            this.products_json = products_json;
        }

        public OrderAddModel()
        {
        }
    }
}
