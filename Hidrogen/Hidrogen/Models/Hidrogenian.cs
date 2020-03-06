using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class Hidrogenian
    {
        public Hidrogenian()
        {
            HidroAddress = new HashSet<HidroAddress>();
            HidroProfile = new HashSet<HidroProfile>();
            HidroSetting = new HashSet<HidroSetting>();
            RoleClaimer = new HashSet<RoleClaimer>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int? AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeactivatedOn { get; set; }
        public DateTime? LastSignin { get; set; }
        public DateTime? LastSignout { get; set; }
        public string RecoveryToken { get; set; }
        public DateTime? TokenSetOn { get; set; }

        public virtual ICollection<HidroAddress> HidroAddress { get; set; }
        public virtual ICollection<HidroProfile> HidroProfile { get; set; }
        public virtual ICollection<HidroSetting> HidroSetting { get; set; }
        public virtual ICollection<RoleClaimer> RoleClaimer { get; set; }
    }
}
