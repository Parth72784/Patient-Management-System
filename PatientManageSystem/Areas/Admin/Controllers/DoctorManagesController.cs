using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PatientManageSystem.Models;
using static PatientManageSystem.FilterConfig;

namespace PatientManageSystem.Areas.Admin.Controllers
{
    [AdminLoginFilter]
    public class DoctorManagesController : Controller
    {
        private PMSEntities db = new PMSEntities();

        // GET: Admin/DoctorManages
        public ActionResult Index()
        {
            return View(db.DoctorManages.ToList());
        }

        // GET: Admin/DoctorManages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Id is not Avalible!!" });
            }
            DoctorManage doctorManage = db.DoctorManages.Find(id);
            if (doctorManage == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Data is not Avalible!!" });
            }
            return View(doctorManage);
        }

        // GET: Admin/DoctorManages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DoctorManages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DrId,DrName,DrPassword,Specialzaition,DrEmail,DrContactNumber,DrExperience,DrGender,IsActive,Cretedby,Modifiedby,Creteddate,Modifieddate")] DoctorManage doctorManage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var CretedName = Convert.ToString(Session["AdminName"]);
                    doctorManage.Cretedby = CretedName;
                    doctorManage.Creteddate = DateTime.Now;
                    db.DoctorManages.Add(doctorManage);
                    db.SaveChanges();
                    TempData["msg"] = "Doctor Details Saved!!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    TempData["err"] = ex.Message;
                }
            
              
            }

            return View(doctorManage);
        }

        // GET: Admin/DoctorManages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Id is not Avalible!!" });

            }
            DoctorManage doctorManage = db.DoctorManages.Find(id);
            if (doctorManage == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Data is not Avalible!!" });
            }
            return View(doctorManage);
        }

        // POST: Admin/DoctorManages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DrId,DrName,DrPassword,Specialzaition,DrEmail,DrContactNumber,DrExperience,DrGender,IsActive,Cretedby,Modifiedby,Creteddate,Modifieddate")] DoctorManage doctorManage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ModifiedName = Convert.ToString(Session["AdminName"]);
                    doctorManage.Modifiedby = ModifiedName;

                    if (Session["DrId"] == null)
                    {
                        doctorManage.IsActive = false;
                    }
                    else if (Session["DrId"] != null)
                    {
                        doctorManage.IsActive = true;
                    }
                    doctorManage.Modifieddate = DateTime.Now;
                    db.Entry(doctorManage).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["msg"] = "Doctor Details Updated!!";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    TempData["err"] = ex.Message;
                }
               
                
            }
            return View(doctorManage);
        }

        // GET: Admin/DoctorManages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Id is not Avalible!!" });

            }
            DoctorManage doctorManage = db.DoctorManages.Find(id);
            if (doctorManage == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Data is not Avalible!!" });

            }
            return View(doctorManage);
        }

        // POST: Admin/DoctorManages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                DoctorManage doctorManage = db.DoctorManages.Find(id);
                db.DoctorManages.Remove(doctorManage);
                db.SaveChanges();
                TempData["msg"] = "Doctor Details Deleted!!";
            }
            catch (Exception ex)
            {

                TempData["err"] = ex.Message;
            }
           
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
