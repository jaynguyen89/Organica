using HelperLibrary.Common;
using Hidrogen.Models;

namespace Hidrogen.ViewModels {

    public class HidrogenianVM {

        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FamilyName { get; set; }

        public string GivenName { get; set; }

        public string FullName => GivenName + HidroConstants.WHITE_SPACE + FamilyName;

        public string Token { get; set; }

        public static implicit operator HidrogenianVM(Hidrogenian h) {
            return h == null ? null : new HidrogenianVM {
                Id = h.Id,
                UserName = h.UserName,
                Email = h.Email
            };
        }
    }
}
