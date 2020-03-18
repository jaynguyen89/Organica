using Hidrogen.Models;
using System;

namespace Hidrogen.ViewModels.Authorization {

    public class HidroRoleVM {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //public static implicit operator HidroRoleVM(HidroRole role) {
        //    return new HidroRoleVM {
        //        Id = role.Id,
        //        Name = role.RoleName,
        //        Description = role.RoleDescription
        //    };
        //}
    }
}
