using PatientManageSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatientManageSystem.Areas.User.Controllers
{
    public class UserLoginController : Controller
    {
        PMSEntities db = new PMSEntities();
        // GET: User/UserLogin
        public ActionResult Drlogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Drlogin(DoctorManage doctorManage)
        {
            try
            {
                var login = db.DoctorManages.SingleOrDefault(a => a.DrEmail == doctorManage.DrEmail && a.DrPassword == doctorManage.DrPassword);
                if (login != null)
                {
                    Session["DrId"] = doctorManage.DrId;
                    Session["DrName"] = doctorManage.DrName;
                    return RedirectToAction("Index", "DrIndex");
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

        public ActionResult Stafflogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Stafflogin(StaffManage staffManage)
        {
            try
            {
                var login = db.StaffManages.SingleOrDefault(a => a.StaffEmail == staffManage.StaffEmail && a.StaffPassword == staffManage.StaffPassword);
                if (login != null)
                {
                    Session["SId"] = staffManage.StaffId;
                    Session["SName"] = staffManage.StaffName;
                    return RedirectToAction("Index", "Index");
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
