using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class ItemStock
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int? VariationId { get; set; }
        public int? BundleId { get; set; }
        public decimal? StockQuantity { get; set; }

        public virtual ItemBundle Bundle { get; set; }
        public virtual Item Item { get; set; }
        public virtual ItemVariation Variation { get; set; }
    }
}
