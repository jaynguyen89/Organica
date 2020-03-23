using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class HidroProfile
    {
        public int Id { get; set; }
        public int HidrogenianId { get; set; }
        public string CitizenCardPhoto { get; set; }
        public string CitizenCardNumber { get; set; }
        public DateTime? CitizenCardIssuedDate { get; set; }
        public string AvatarInformation { get; set; }
        public DateTime? DateOfBith { get; set; }
        public bool? Gender { get; set; }
        public string Ethnicity { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public string SelfIntroduction { get; set; }
        public string PersonalWebsite { get; set; }

        public virtual Hidrogenian Hidrogenian { get; set; }
    }
}
