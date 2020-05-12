using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class SellerReliability
    {
        public int Id { get; set; }
        public int RatingId { get; set; }
        public byte? FairPrice { get; set; }
        public byte? FastResponse { get; set; }
        public byte? Politeness { get; set; }
        public byte? Liablility { get; set; }

        public virtual SellerRating Rating { get; set; }
    }
}
