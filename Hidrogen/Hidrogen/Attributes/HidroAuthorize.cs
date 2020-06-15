using System;
using System.Net;
using System.Threading.Tasks;
using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.ViewModels.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Hidrogen.Attributes {

    public sealed class HidroAuthorize : Attribute, IAsyncAuthorizationFilter {

        private HidroPermissionVM _userPermissions;
        private readonly HidroPermissionVM _requiredPermissions;

        private const int PERM_QTY = 8;

        public HidroAuthorize(HidroEnums.PERMISSIONS policy) {
            _requiredPermissions = new HidroPermissionVM {
                AllowCreate = policy.GetValue() == HidroConstants.PERMISSION_VALS["create"],
                AllowView = policy.GetValue() == HidroConstants.PERMISSION_VALS["view"],
                AllowEditOwn = policy.GetValue() == HidroConstants.PERMISSION_VALS["edit_own"],
                AllowEditOthers = policy.GetValue() == HidroConstants.PERMISSION_VALS["edit_others"],
                AllowDeleteOwn = policy.GetValue() == HidroConstants.PERMISSION_VALS["delete_own"],
                AllowDeleteOthers = policy.GetValue() == HidroConstants.PERMISSION_VALS["delete_others"],
                AllowReviveOwn = policy.GetValue() == HidroConstants.PERMISSION_VALS["revive_own"],
                AllowReviveOthers = policy.GetValue() == HidroConstants.PERMISSION_VALS["revive_others"]
            };
        }

        /*public HidroAuthorize(string policy) {
            policy = policy.Trim().Replace(HidroConstants.WHITE_SPACE, string.Empty);

            if (string.IsNullOrEmpty(policy) || string.IsNullOrWhiteSpace(policy))
                throw new FormatException("HidroAuthorize.Constructor - No permission given.");

            var stringPermissions = policy.Split(",");
            if (stringPermissions.Length == 0 || stringPermissions.Length != PERM_QTY)
                throw new FormatException("HidroAuthorize.Constructor - Permission tokens error.");

            _requiredPermissions = new HidroPermissionVM {
                AllowCreate = stringPermissions[0] == "1",
                AllowView = stringPermissions[1] == "1",
                AllowEditOwn = stringPermissions[2] == "1",
                AllowEditOthers = stringPermissions[3] == "1",
                AllowDeleteOwn = stringPermissions[4] == "1",
                AllowDeleteOthers = stringPermissions[5] == "1",
                AllowReviveOwn = stringPermissions[6] == "1",
                AllowReviveOthers = stringPermissions[7] == "1"
            };
        }*/

        public Task OnAuthorizationAsync(AuthorizationFilterContext context) {
            var sessionPermissions = context.HttpContext.Session.GetString(nameof(HidroAuthorize));
            if (sessionPermissions == null) {
                context.Result = new RedirectToActionResult("LogOutInternal", "Authentication", null);
                return Task.CompletedTask;
            }

            _userPermissions = JsonConvert.DeserializeObject<HidroPermissionVM>(sessionPermissions);

            if (_requiredPermissions.AllowCreate && _userPermissions.AllowCreate == _requiredPermissions.AllowCreate) return Task.CompletedTask;
            if (_requiredPermissions.AllowView && _userPermissions.AllowView == _requiredPermissions.AllowView) return Task.CompletedTask;
            if (_requiredPermissions.AllowEditOwn && _userPermissions.AllowEditOwn == _requiredPermissions.AllowEditOwn) return Task.CompletedTask;
            if (_requiredPermissions.AllowEditOthers && _userPermissions.AllowEditOthers == _requiredPermissions.AllowEditOthers) return Task.CompletedTask;
            if (_requiredPermissions.AllowDeleteOwn && _userPermissions.AllowDeleteOwn == _requiredPermissions.AllowDeleteOwn) return Task.CompletedTask;
            if (_requiredPermissions.AllowDeleteOthers && _userPermissions.AllowDeleteOthers == _requiredPermissions.AllowDeleteOthers) return Task.CompletedTask;
            if (_requiredPermissions.AllowReviveOwn && _userPermissions.AllowReviveOwn == _requiredPermissions.AllowReviveOwn) return Task.CompletedTask;
            if (_requiredPermissions.AllowReviveOthers && _userPermissions.AllowReviveOthers == _requiredPermissions.AllowReviveOthers) return Task.CompletedTask;

            context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            return Task.CompletedTask;
        }
    }
}