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
    public class StaffManagesController : Controller
    {
        private PMSEntities db = new PMSEntities();

        // GET: Admin/StaffManages
        public ActionResult Index()
        {
          
            return View(db.StaffManages.ToList());
        }

        // GET: Admin/StaffManages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Id is not Avalible!!" });


            }
            StaffManage staffManage = db.StaffManages.Find(id);
            if (staffManage == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Data is not Avalible!!" });

            }
            return View(staffManage);
        }

        // GET: Admin/StaffManages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/StaffManages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StaffId,StaffName,StaffPassword,StaffEmail,StaffContactNumber,StaffDesignation,StaffExperience,IsActive,Cretedby,Modifiedby,Creteddate,Modifieddate")] StaffManage staffManage)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var CretedName = Convert.ToString(Session["AdminName"]);
                    staffManage.Cretedby = CretedName;
                    staffManage.Creteddate = DateTime.Now;
                    db.StaffManages.Add(staffManage);
                    db.SaveChanges();
                    TempData["msg"] = "Staff Details Save!!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    TempData["err"] = ex.Message;
                }
            
            }

            return View(staffManage);
        }

        // GET: Admin/StaffManages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Id is not Avalible!!" });
            }
            StaffManage staffManage = db.StaffManages.Find(id);
            if (staffManage == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Data is not Avalible!!" });
            }
            return View(staffManage);
        }

        // POST: Admin/StaffManages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StaffId,StaffName,StaffPassword,StaffEmail,StaffContactNumber,StaffDesignation,StaffExperience,IsActive,Cretedby,Modifiedby,Creteddate,Modifieddate")] StaffManage staffManage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ModifiedName = Convert.ToString(Session["AdminName"]);
                    staffManage.Modifiedby = ModifiedName;
                    staffManage.Modifieddate = DateTime.Now;
                    if (Session["SId"] == null)
                    {
                        staffManage.IsActive = false;
                    }
                    else if (Session["SId"] != null)
                    {
                        staffManage.IsActive = true;
                    }

                    db.Entry(staffManage).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["msg"] = "Staff Details Updated!!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    TempData["err"] = ex.Message;
                }
               
            }
            return View(staffManage);
        }

        // GET: Admin/StaffManages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Id is not Avalible!!" });

            }
            StaffManage staffManage = db.StaffManages.Find(id);
            if (staffManage == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Data is not Avalible!!" });

            }
            return View(staffManage);
        }

        // POST: Admin/StaffManages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                StaffManage staffManage = db.StaffManages.Find(id);
                db.StaffManages.Remove(staffManage);
                db.SaveChanges();
                TempData["msg"] = "Staff Details Delete !!";
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
