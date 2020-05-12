using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class SellerRating
    {
        public SellerRating()
        {
            SellerReliability = new HashSet<SellerReliability>();
        }

        public int Id { get; set; }
        public int SellerId { get; set; }
        public int TradingId { get; set; }
        public int RatedById { get; set; }
        public bool? RatingSign { get; set; }
        public string Comment { get; set; }
        public bool AllowEdit { get; set; }
        public DateTime RatedOn { get; set; }
        public DateTime? RevisedOn { get; set; }

        public virtual HidroTrader RatedBy { get; set; }
        public virtual HidroTrader Seller { get; set; }
        public virtual Trading Trading { get; set; }
        public virtual ICollection<SellerReliability> SellerReliability { get; set; }
    }
}
