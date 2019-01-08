using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FirstOfAll.UI.Site.Controllers.Shared
{
    public class IsAuthorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
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
                        
                        filterContext.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                    }
                }
                else{

                    filterContext.Result = new RedirectToActionResult("AccessDenied", "Login", null);
                }

                base.OnActionExecuting(filterContext);
            }
     
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //
        }

    }
}