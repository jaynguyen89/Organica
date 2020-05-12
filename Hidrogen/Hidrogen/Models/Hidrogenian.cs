using System;
using System.Collections.Generic;

namespace Hidrogen.Models
{
    public partial class Hidrogenian
    {
        public Hidrogenian()
        {
            ChatParticipant = new HashSet<ChatParticipant>();
            HidroAddress = new HashSet<HidroAddress>();
            HidroProfile = new HashSet<HidroProfile>();
            HidroSetting = new HashSet<HidroSetting>();
            HidroTrader = new HashSet<HidroTrader>();
            PaymentMethod = new HashSet<PaymentMethod>();
            RoleClaimer = new HashSet<RoleClaimer>();
            StoreOwner = new HashSet<StoreOwner>();
            StoreStaff = new HashSet<StoreStaff>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string TwoFaSecretKey { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeactivatedOn { get; set; }
        public DateTime? LastSignin { get; set; }
        public DateTime? LastSignout { get; set; }
        public string RecoveryToken { get; set; }
        public DateTime? TokenSetOn { get; set; }
        public string CookieToken { get; set; }
        public DateTime? CookieSetOn { get; set; }
        public string LastDeviceInfo { get; set; }

        public virtual ICollection<ChatParticipant> ChatParticipant { get; set; }
        public virtual ICollection<HidroAddress> HidroAddress { get; set; }
        public virtual ICollection<HidroProfile> HidroProfile { get; set; }
        public virtual ICollection<HidroSetting> HidroSetting { get; set; }
        public virtual ICollection<HidroTrader> HidroTrader { get; set; }
        public virtual ICollection<PaymentMethod> PaymentMethod { get; set; }
        public virtual ICollection<RoleClaimer> RoleClaimer { get; set; }
        public virtual ICollection<StoreOwner> StoreOwner { get; set; }
        public virtual ICollection<StoreStaff> StoreStaff { get; set; }
    }
}
