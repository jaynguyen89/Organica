using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class ShoppingCart
    {
        public ShoppingCart()
        {
            ItemBasket = new HashSet<ItemBasket>();
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int BuyerId { get; set; }
        public string CartNote { get; set; }

        public virtual HidroTrader Buyer { get; set; }
        public virtual ICollection<ItemBasket> ItemBasket { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
