using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace PatientManageSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public class AdminLoginFilter : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {


                if (HttpContext.Current.Session["Adminid"] == null)
                {
                    FormsAuthentication.SignOut();
                    filterContext.Result =
                   new RedirectToRouteResult(new RouteValueDictionary
                     {
             { "action", "Login" },
            { "controller", "AdminLogin" },
            { "returnUrl", filterContext.HttpContext.Request.RawUrl}
                      });

                    return;
                }
            }
        }
        public class DrLoginFilter : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {


                if (HttpContext.Current.Session["DrId"] == null)
                {
                    FormsAuthentication.SignOut();
                    filterContext.Result =
                   new RedirectToRouteResult(new RouteValueDictionary
                     {
             { "action", "DrLogin" },
            { "controller", "UserLogin" },
            { "returnUrl", filterContext.HttpContext.Request.RawUrl}
                      });

                    return;
                }
            }
        }
        public class StaffLoginFilter : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {


                if (HttpContext.Current.Session["SId"] == null)
                {
                    FormsAuthentication.SignOut();
                    filterContext.Result =
                   new RedirectToRouteResult(new RouteValueDictionary
                     {
             { "action", "StaffLogin" },
            { "controller", "UserLogin" },
            { "returnUrl", filterContext.HttpContext.Request.RawUrl}
                      });

                    return;
                }
            }
        }
    }
}
