using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class Country
    {
        public Country()
        {
            FineLocation = new HashSet<FineLocation>();
            RawLocation = new HashSet<RawLocation>();
        }

        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string Continent { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }

        public virtual ICollection<FineLocation> FineLocation { get; set; }
        public virtual ICollection<RawLocation> RawLocation { get; set; }
    }
}
