using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class Warranty
    {
        public Warranty()
        {
            WarrantyTerm = new HashSet<WarrantyTerm>();
        }

        public int Id { get; set; }
        public int ItemId { get; set; }
        public byte Duration { get; set; }
        public DateTime StartedOn { get; set; }
        public bool IsEditted { get; set; }
        public bool IsActive { get; set; }

        public virtual Item Item { get; set; }
        public virtual ICollection<WarrantyTerm> WarrantyTerm { get; set; }
    }
}
