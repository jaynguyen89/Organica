using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class BuyerRating
    {
        public BuyerRating()
        {
            BuyerReliability = new HashSet<BuyerReliability>();
        }

        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int TradingId { get; set; }
        public int RatedById { get; set; }
        public bool? RatingSign { get; set; }
        public string Comment { get; set; }
        public string Reliability { get; set; }
        public bool AllowEdit { get; set; }
        public DateTime RatedOn { get; set; }
        public DateTime? RevisedOn { get; set; }

        public virtual HidroTrader Buyer { get; set; }
        public virtual HidroTrader RatedBy { get; set; }
        public virtual Trading Trading { get; set; }
        public virtual ICollection<BuyerReliability> BuyerReliability { get; set; }
    }
}
