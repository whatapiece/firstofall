using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FirstOfAll.UI.Site.Controllers.Shared
{

    public class IsAuthorizeAttribute : TypeFilterAttribute
    {
        public IsAuthorizeAttribute(string claimType = "", string claimValue = "") : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;

                var accessType = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                    .FirstOrDefault(a => a.GetType().Equals(typeof(ActionType)));

                if (accessType != null)
                {
                    switch ((AccessType)accessType.GetType().GetProperty("AccessType").GetValue(accessType))
                    {
                        case AccessType.Create:
                            {
                                if (filterContext.HttpContext.User.Claims.Any(c => c.Type == controllerActionDescriptor.ControllerName && c.Value == "Create")) return;
                                break;
                            }
                        case AccessType.Read:
                            {
                                if (filterContext.HttpContext.User.Claims.Any(c => c.Type == controllerActionDescriptor.ControllerName && c.Value == "Read")) return;
                                break;
                            }
                        case AccessType.Update:
                            {
                                if (filterContext.HttpContext.User.Claims.Any(c => c.Type == controllerActionDescriptor.ControllerName && c.Value == "Update")) return;
                                break;
                            }
                        case AccessType.Delete:
                            {
                                if (filterContext.HttpContext.User.Claims.Any(c => c.Type == controllerActionDescriptor.ControllerName && c.Value == "Delete")) return;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }

                    filterContext.Result = new UnauthorizedResult();
                }
            }
            else
            {
                filterContext.Result = new UnauthorizedResult();
            }
        }
    }
}