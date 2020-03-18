using System;
using System.Collections.Generic;

namespace Hidrogen.ViewModels.Authorization {

    public class TemporaryPermissionVM {

        public string Title { get; set; }

        public List<string> OverriddenPermissions { get; set; }

        public DateTime EffectFrom { get; set; }

        public DateTime EffectUntil { get; set; }

        public string Description { get; set; }

        public string Notes { get; set; }
    }
}
