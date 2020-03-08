using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using BCrypt;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Hidrogen.Services {

    public sealed class HidroActionFilter : ActionFilterAttribute {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAntiforgery _antiforgery;

        public HidroActionFilter(
            IHttpContextAccessor httpContextAccessor,
            IAntiforgery antiforgery
        ) {
            _httpContextAccessor = httpContextAccessor;
            _antiforgery = antiforgery;
        }

        public override void OnActionExecuting(ActionExecutingContext context) {
            base.OnActionExecuting(context);

            var xsrfToken = _antiforgery.GetTokens(context.HttpContext).RequestToken;
            var sessionXsrfToken = _httpContextAccessor.HttpContext.Session.GetString("XSRF-TOKEN");

            if (xsrfToken != sessionXsrfToken && !string.IsNullOrEmpty(sessionXsrfToken))
                context.Result = new RedirectToActionResult("FilterResult", "Authentication", new { result = "XsrfTokenMismatched" });
        }

        public override void OnActionExecuted(ActionExecutedContext context) {
            base.OnActionExecuted(context);

            var xsrfToken = BCryptHelper.GenerateSalt(15, SaltRevision.Revision2X);

            context.HttpContext.Response.Cookies.Append(
                "XSRF-TOKEN", xsrfToken,
                new CookieOptions { HttpOnly = false }
            );

            _httpContextAccessor.HttpContext.Session.SetString("XSRF-TOKEN", xsrfToken);
        }
    }
}
