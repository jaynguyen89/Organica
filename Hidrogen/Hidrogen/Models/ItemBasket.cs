using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class ItemBasket
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ListingId { get; set; }
        public int DeliverToId { get; set; }
        public int? VariationId { get; set; }
        public int? BundleId { get; set; }
        public decimal Amount { get; set; }
        public string BasketNote { get; set; }

        public virtual ItemBundle Bundle { get; set; }
        public virtual ShoppingCart Cart { get; set; }
        public virtual HidroAddress DeliverTo { get; set; }
        public virtual Listing Listing { get; set; }
        public virtual ItemVariation Variation { get; set; }
    }
}
