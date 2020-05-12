using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class HidroTrader
    {
        public HidroTrader()
        {
            BuyerRatingBuyer = new HashSet<BuyerRating>();
            BuyerRatingRatedBy = new HashSet<BuyerRating>();
            SellerRatingRatedBy = new HashSet<SellerRating>();
            SellerRatingSeller = new HashSet<SellerRating>();
            ShoppingCart = new HashSet<ShoppingCart>();
            TradingBuyer = new HashSet<Trading>();
            TradingSeller = new HashSet<Trading>();
        }

        public int Id { get; set; }
        public int HidrogenianId { get; set; }
        public float BuyerRating { get; set; }
        public float SellerRating { get; set; }

        public virtual Hidrogenian Hidrogenian { get; set; }
        public virtual ICollection<BuyerRating> BuyerRatingBuyer { get; set; }
        public virtual ICollection<BuyerRating> BuyerRatingRatedBy { get; set; }
        public virtual ICollection<SellerRating> SellerRatingRatedBy { get; set; }
        public virtual ICollection<SellerRating> SellerRatingSeller { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCart { get; set; }
        public virtual ICollection<Trading> TradingBuyer { get; set; }
        public virtual ICollection<Trading> TradingSeller { get; set; }
    }
}
