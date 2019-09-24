using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatientManageSystem.Areas.User.Controllers
{
    public class IndexController : Controller
    {
        // GET: User/Index
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Index");
        }

        public ActionResult Error(string err)
        {
            TempData["err"] = err;
            return View();
        }
    }
}