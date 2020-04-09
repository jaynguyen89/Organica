using Hidrogen.Models;

namespace Hidrogen.ViewModels.Authorization {


    public class HidroPermissionVM {

        public bool AllowCreate { get; set; }

        public bool AllowView { get; set; }

        public bool AllowEditOwn { get; set; }

        public bool AllowEditOthers { get; set; }

        public bool AllowDeleteOwn { get; set; }

        public bool AllowDeleteOthers { get; set; }

        public bool AllowReviveOwn { get; set; }

        public bool AllowReviveOthers { get; set; }

        public static implicit operator HidroPermissionVM(RoleClaimer roleClaimer) {
            return roleClaimer == null ? null : new HidroPermissionVM {
                AllowCreate = roleClaimer.AllowCreate,
                AllowView = roleClaimer.AllowView,
                AllowEditOwn = roleClaimer.AllowEditOwn,
                AllowEditOthers = roleClaimer.AllowEditOthers,
                AllowDeleteOwn = roleClaimer.AllowDeleteOwn,
                AllowDeleteOthers = roleClaimer.AllowDeleteOthers,
                AllowReviveOwn = roleClaimer.AllowReviveOwn,
                AllowReviveOthers = roleClaimer.AllowReviveOthers
            };
        }
    }
}
