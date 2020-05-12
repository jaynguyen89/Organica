using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class WarrantyTerm
    {
        public int Id { get; set; }
        public int WarrantyId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public virtual Warranty Warranty { get; set; }
    }
}
