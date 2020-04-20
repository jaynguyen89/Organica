using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class RawLocation
    {
        public int Id { get; set; }
        public string BuildingName { get; set; }
        public string PoBoxNumber { get; set; }
        public string StreetAddress { get; set; }
        public string AlternateAddress { get; set; }
        public string Group { get; set; }
        public string Lane { get; set; }
        public string Quarter { get; set; }
        public string Hamlet { get; set; }
        public string Commute { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Town { get; set; }
        public string Suburb { get; set; }
        public string Postcode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int CountryId { get; set; }
        public bool IsStandard { get; set; }

        public virtual Country Country { get; set; }
    }
}
