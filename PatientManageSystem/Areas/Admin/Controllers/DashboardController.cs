using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PatientManageSystem.FilterConfig;

namespace PatientManageSystem.Areas.Admin.Controllers
{
    [AdminLoginFilter]
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "AdminLogin");
        }

        public ActionResult Error(string err)
        {
            TempData["err"] = err;
            return View();
        }
    }
}