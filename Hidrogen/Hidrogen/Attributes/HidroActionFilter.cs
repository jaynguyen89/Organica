using System;
using BCrypt;
using HelperLibrary;
using HelperLibrary.Common;
using Hidrogen.ViewModels.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace Hidrogen.Attributes {

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class HidroActionFilter : ActionFilterAttribute {

        private readonly string _defaultRole = HidroEnums.ROLES.GUEST.GetValue();

        public HidroActionFilter() {}

        public HidroActionFilter(HidroEnums.ROLES defaultRole) {
            _defaultRole = defaultRole.GetValue();
        }

        public override void OnActionExecuting(ActionExecutingContext context) {
            base.OnActionExecuting(context);

            var session = context.HttpContext.Session;
            var request = context.HttpContext.Request;

            request.Headers.TryGetValue("X-XSRF-TOKEN", out var xsrfToken);
            var sessionXsrfToken = session.GetString("XSRF-TOKEN");

            if (((string)xsrfToken) != sessionXsrfToken && !string.IsNullOrEmpty(sessionXsrfToken)) {
                context.Result = new RedirectToActionResult("FilterResult", "Authentication", new { result = "XsrfTokenMismatched" });
            }
            else {
                request.Headers.TryGetValue("Authorization", out var headerAuth);

                string authToken;
                try {
                    authToken = ((string) headerAuth).Split(HidroConstants.WHITE_SPACE)[1];
                }
                catch (IndexOutOfRangeException) {
                    authToken = string.Empty;
                }

                var userSession = GetUserSession(session);
                //When user has signed in
                if (!string.IsNullOrEmpty(userSession.AuthToken) && userSession.ExpirationTime != 0 &&
                    userSession.UserId != 0 && !string.IsNullOrEmpty(userSession.Role)) {
                    if (userSession.AuthToken == authToken) {
                        var current = ((DateTimeOffset) DateTime.UtcNow).ToUnixTimeSeconds();
                        if (userSession.ExpirationTime <= current)
                            context.Result = new RedirectToActionResult("FilterResult", "Authentication", new {result = HidroEnums.FILTER_RESULT.AUTHENTICATION_EXPIRED});

                        var allowSuperUser = !string.Equals(_defaultRole, HidroEnums.ROLES.CUSTOMER.GetValue(), StringComparison.CurrentCultureIgnoreCase) &&
                                                   HidroConstants.GetRoleHierrachy(userSession.Role.ToLower()) > HidroConstants.GetRoleHierrachy(_defaultRole.ToLower());
                        
                        var allowCustomer = string.Equals(_defaultRole, HidroEnums.ROLES.CUSTOMER.GetValue(), StringComparison.CurrentCultureIgnoreCase) &&
                                                  (
                                                      string.Equals(_defaultRole, userSession.Role, StringComparison.CurrentCultureIgnoreCase) ||
                                                      !string.Equals(_defaultRole, HidroEnums.ROLES.GUEST.GetValue(), StringComparison.CurrentCultureIgnoreCase)
                                                  );

                        if (allowSuperUser == false && allowCustomer == false)
                            context.Result = new RedirectToActionResult("FilterResult", "Authentication", new {result = HidroEnums.FILTER_RESULT.ACCESS_CONTROL_DENIED});
                    }
                    else
                        context.Result = new RedirectToActionResult("FilterResult", "Authentication", new {result = HidroEnums.FILTER_RESULT.INVALID_AUTHENTICATION});
                } //When _defaultRole == "Guest"
                else if (_defaultRole.ToLower() != "guest")
                    context.Result = new RedirectToActionResult("FilterResult", "Authentication", new {result = HidroEnums.FILTER_RESULT.ACCESS_CONTROL_DENIED});
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context) {
            base.OnActionExecuted(context);

            var xsrfToken = BCryptHelper.GenerateSalt(15, SaltRevision.Revision2X);

            context.HttpContext.Response.Cookies.Append(
                "XSRF-TOKEN", xsrfToken,
                new CookieOptions { HttpOnly = false }
            );

            context.HttpContext.Session.SetString("XSRF-TOKEN", xsrfToken);
        }

        private AuthenticatedUser GetUserSession(ISession session) {
            return new AuthenticatedUser {
                AuthToken = session.GetString(nameof(AuthenticatedUser.AuthToken)),
                ExpirationTime = session.GetInt32(nameof(AuthenticatedUser.ExpirationTime)) ?? HidroConstants.EMPTY,
                UserId = session.GetInt32(nameof(AuthenticatedUser.UserId)) ?? HidroConstants.EMPTY,
                Role = session.GetString(nameof(AuthenticatedUser.Role))
            };
        }
    }
}
