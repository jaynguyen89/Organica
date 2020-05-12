using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class Classification
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Listing Listing { get; set; }
    }
}
