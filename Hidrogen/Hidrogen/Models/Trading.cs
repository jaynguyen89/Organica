using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class Trading
    {
        public Trading()
        {
            BuyerRating = new HashSet<BuyerRating>();
            SellerRating = new HashSet<SellerRating>();
            TradingRating = new HashSet<TradingRating>();
        }

        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int OrderId { get; set; }
        public bool HasItemInWarranty { get; set; }

        public virtual HidroTrader Buyer { get; set; }
        public virtual Order Order { get; set; }
        public virtual HidroTrader Seller { get; set; }
        public virtual ICollection<BuyerRating> BuyerRating { get; set; }
        public virtual ICollection<SellerRating> SellerRating { get; set; }
        public virtual ICollection<TradingRating> TradingRating { get; set; }
    }
}
