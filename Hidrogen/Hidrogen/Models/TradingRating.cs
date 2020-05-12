using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class TradingRating
    {
        public int Id { get; set; }
        public int TradingId { get; set; }
        public bool RatingSign { get; set; }
        public string Comment { get; set; }
        public byte? MatchingDescription { get; set; }
        public byte? FastDispatch { get; set; }
        public byte? PostageSpeed { get; set; }
        public byte? PackageProtection { get; set; }

        public virtual Trading Trading { get; set; }
    }
}
