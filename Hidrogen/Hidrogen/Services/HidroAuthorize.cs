using Microsoft.AspNetCore.Mvc.Filters;

namespace Hidrogen.Services {

    public sealed class HidroAuthorize : ActionFilterAttribute {

        private string RequiredRole = "Customer";

        public HidroAuthorize() { }

        public HidroAuthorize(string role) {
            RequiredRole = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context) {
            base.OnActionExecuting(context);

            //Implement custom authorization here
        }
    }
}
