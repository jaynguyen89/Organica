using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class Location
    {
        public Location()
        {
            HidroAddress = new HashSet<HidroAddress>();
        }

        public int Id { get; set; }
        public string BuildingName { get; set; }
        public string StreetAddress { get; set; }
        public string AlternateAddress { get; set; }
        public string Lane { get; set; }
        public string Quarter { get; set; }
        public string Hamlet { get; set; }
        public string Commute { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Suburb { get; set; }
        public string Postcode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Note { get; set; }

        public virtual ICollection<HidroAddress> HidroAddress { get; set; }
    }
}
