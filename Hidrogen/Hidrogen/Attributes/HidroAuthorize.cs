using System;
using System.Net;
using System.Threading.Tasks;
using HelperLibrary.Common;
using Hidrogen.ViewModels.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Hidrogen.Attributes {

    public sealed class HidroAuthorize : Attribute, IAsyncAuthorizationFilter {

        private HidroPermissionVM UserPermissions;
        private readonly HidroPermissionVM RequiredPermissions;

        private const int PERM_QTY = 8;

        public HidroAuthorize(string policy) {
            policy = policy.Trim().Replace(HidroConstants.WHITE_SPACE, string.Empty);

            if (string.IsNullOrEmpty(policy) || string.IsNullOrWhiteSpace(policy))
                throw new Exception("HidroAuthorize.Constructor - No permission given.");

            var stringPermissions = policy.Split(",");
            if (stringPermissions.Length == 0 || stringPermissions.Length != PERM_QTY)
                throw new Exception("HidroAuthorize.Constructor - Permission tokens error.");

            RequiredPermissions = new HidroPermissionVM {
                AllowCreate = stringPermissions[0] == "1",
                AllowView = stringPermissions[1] == "1",
                AllowEditOwn = stringPermissions[2] == "1",
                AllowEditOthers = stringPermissions[3] == "1",
                AllowDeleteOwn = stringPermissions[4] == "1",
                AllowDeleteOthers = stringPermissions[5] == "1",
                AllowReviveOwn = stringPermissions[6] == "1",
                AllowReviveOthers = stringPermissions[7] == "1"
            };
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context) {
            var sessionPermissions = context.HttpContext.Session.GetString(nameof(HidroAuthorize));
            if (sessionPermissions == null) {
                context.Result = new RedirectToActionResult("LogOutInternal", "Authentication", null);
                return Task.CompletedTask;
            }

            UserPermissions = JsonConvert.DeserializeObject<HidroPermissionVM>(sessionPermissions);

            if (RequiredPermissions.AllowCreate && UserPermissions.AllowCreate == RequiredPermissions.AllowCreate) return Task.CompletedTask;
            if (RequiredPermissions.AllowView && UserPermissions.AllowView == RequiredPermissions.AllowView) return Task.CompletedTask;
            if (RequiredPermissions.AllowEditOwn && UserPermissions.AllowEditOwn == RequiredPermissions.AllowEditOwn) return Task.CompletedTask;
            if (RequiredPermissions.AllowEditOthers && UserPermissions.AllowEditOthers == RequiredPermissions.AllowEditOthers) return Task.CompletedTask;
            if (RequiredPermissions.AllowDeleteOwn && UserPermissions.AllowDeleteOwn == RequiredPermissions.AllowDeleteOwn) return Task.CompletedTask;
            if (RequiredPermissions.AllowDeleteOthers && UserPermissions.AllowDeleteOthers == RequiredPermissions.AllowDeleteOthers) return Task.CompletedTask;
            if (RequiredPermissions.AllowReviveOwn && UserPermissions.AllowReviveOwn == RequiredPermissions.AllowReviveOwn) return Task.CompletedTask;
            if (RequiredPermissions.AllowReviveOthers && UserPermissions.AllowReviveOthers == RequiredPermissions.AllowReviveOthers) return Task.CompletedTask;

            context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            return Task.CompletedTask;
        }
    }
}