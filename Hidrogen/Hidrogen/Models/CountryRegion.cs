using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class CountryRegion
    {
        public CountryRegion()
        {
            InverseResidedIn = new HashSet<CountryRegion>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
        public byte RegionClass { get; set; }
        public int? ResidedInId { get; set; }

        public virtual Country Country { get; set; }
        public virtual CountryRegion ResidedIn { get; set; }
        public virtual ICollection<CountryRegion> InverseResidedIn { get; set; }
    }
}
