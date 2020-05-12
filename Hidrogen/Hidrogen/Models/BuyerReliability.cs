using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class BuyerReliability
    {
        public int Id { get; set; }
        public int RatingId { get; set; }
        public byte? FairNegotiation { get; set; }
        public byte? FastResponse { get; set; }
        public byte? QuickPayment { get; set; }
        public byte? InTimePickup { get; set; }
        public byte? Politeness { get; set; }
        public DateTime VotedOn { get; set; }

        public virtual BuyerRating Rating { get; set; }
    }
}
