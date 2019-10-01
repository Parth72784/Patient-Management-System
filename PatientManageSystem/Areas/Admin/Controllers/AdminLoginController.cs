using PatientManageSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatientManageSystem.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        PMSEntities db = new PMSEntities();
        // GET: Admin/AdminLogin
        public ActionResult Login()
        {
            if (Request.Cookies["userDetails"] != null)
            {
                HttpCookie cok = Request.Cookies["userDetails"];
                ViewBag.AdminPassword = cok.Values["password"].ToString();
                ViewBag.AdminName = cok.Values["username"].ToString();
            }

            return View();
        }
        [HttpPost]
        public ActionResult Login(SuperAdmin Logindata)
        {
            try
            {
                var login = db.SuperAdmins.SingleOrDefault(a => a.AdminName == Logindata.AdminName && a.AdminPassword == Logindata.AdminPassword);

               // Logindata.isRemember = true;
               
                if (login != null)
                {
                    if (Logindata.isRemember)
                    {
                        HttpCookie cok = new HttpCookie("userDetails");
                        cok.Values["username"] = Logindata.AdminName;
                        cok.Values["password"] = Logindata.AdminPassword;
                        cok.Expires = DateTime.Now.AddSeconds(130);

                        Response.Cookies.Add(cok);
                    }

                    Session["Adminid"] = Logindata.AdminId;
                    Session["AdminName"] = Logindata.AdminName;
                    TempData["msg"] = "Login Done!!";
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    TempData["err"] = "User name or Password is wrong!!";
                }

            }
            catch (Exception ex)
            {
                TempData["err"] = ex.Message;
            }
            return View();
        }
    }
}