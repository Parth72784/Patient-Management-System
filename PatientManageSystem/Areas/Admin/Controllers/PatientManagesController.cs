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
    public class PatientManagesController : Controller
    {
        private PMSEntities db = new PMSEntities();

        // GET: Admin/PatientManages
        public ActionResult Index()
        {
            var patientManages = db.PatientManages.Include(p => p.CityMaster);
            return View(patientManages.ToList());
        }

        // GET: Admin/PatientManages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Id is not Avalible!!" });

            }
            PatientManage patientManage = db.PatientManages.Find(id);
            if (patientManage == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Data is not Avalible!!" });


            }
            return View(patientManage);
        }

        // GET: Admin/PatientManages/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.CountryMasters.ToList(), "CountryId", "CountryName");
            ViewBag.StateId = new List<SelectListItem>(){
                new SelectListItem(){ Text = "", Value="Select State"}
            };
            ViewBag.RefCityid = new List<SelectListItem>(){
                new SelectListItem(){ Text = "", Value="Select city"}
                };
            return View();
        }

        // POST: Admin/PatientManages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Pid,Pname,Address1,Address2,Gender,DOB,PContactNumber,PEmail,Cretedby,Modifiedby,Creteddate,Modifieddate,RefCityid")] PatientManage patientManage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var CretedName = Convert.ToString(Session["AdminName"]);
                    patientManage.Cretedby = CretedName;
                    patientManage.Creteddate = DateTime.Now;
                    db.PatientManages.Add(patientManage);
                    db.SaveChanges();
                    TempData["msg"] = "Patient Details Save!!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    TempData["err"] = ex.Message;
                }
               
            }
            ViewBag.CountryId = new SelectList(db.CountryMasters.ToList(), "CountryId", "CountryName");
            ViewBag.StateId = new List<SelectListItem>(){
                new SelectListItem(){ Text = "", Value="Select State"}
            };
            ViewBag.RefCityid = new List<SelectListItem>(){
                new SelectListItem(){ Text = "", Value="Select city"}
                };
            //ViewBag.RefCityid = new SelectList(db.CityMasters, "CityId", "CityName", patientManage.RefCityid);
            return View(patientManage);
        }
        public JsonResult GetStateByCountry(int CId)
        {
            return Json(db.StateMasters.Where(a => a.RefCountryId == CId).Select(b => new { b.StateId, b.StateName }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCityByState(int CId)
        {
            return Json(db.CityMasters.Where(a => a.RefStateId == CId).Select(b => new { b.CityId, b.CityName }).ToList(), JsonRequestBehavior.AllowGet);
        }
        // GET: Admin/PatientManages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Id is not Avalible!!" });

            }
            PatientManage patientManage = db.PatientManages.Find(id);
            if (patientManage == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Data is not Avalible!!" });
            }
            ViewBag.CountryId = new SelectList(db.CountryMasters.ToList(), "CountryId", "CountryName");
            ViewBag.StateId = new List<SelectListItem>(){
                new SelectListItem(){ Text = "", Value="Select State"}
            };

            ViewBag.RefCityid = new SelectList(db.CityMasters, "CityId", "CityName", patientManage.RefCityid);
            return View(patientManage);
        }

        // POST: Admin/PatientManages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Pid,Pname,Address1,Address2,Gender,DOB,PContactNumber,PEmail,Cretedby,Modifiedby,Creteddate,Modifieddate,RefCityid")] PatientManage patientManage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ModifiedName = Convert.ToString(Session["AdminName"]);
                    patientManage.Modifiedby = ModifiedName;
                    patientManage.Modifieddate = DateTime.Now;
                    db.Entry(patientManage).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["msg"] = "Patient Details Updated!!";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    TempData["err"] = ex.Message;
                }


              
            }
            ViewBag.CountryId = new SelectList(db.CountryMasters.ToList(), "CountryId", "CountryName");
            ViewBag.StateId = new List<SelectListItem>(){
                new SelectListItem(){ Text = "", Value="Select State"}
            };
            ViewBag.RefCityid = new SelectList(db.CityMasters, "CityId", "CityName", patientManage.RefCityid);
            return View(patientManage);
        }

        // GET: Admin/PatientManages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Id is not Avalible!!" });
            }
            PatientManage patientManage = db.PatientManages.Find(id);
            if (patientManage == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Data is not Avalible!!" });
            }
            return View(patientManage);
        }

        // POST: Admin/PatientManages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                PatientManage patientManage = db.PatientManages.Find(id);
                db.PatientManages.Remove(patientManage);
                db.SaveChanges();
                TempData["msg"] = "Patient Details Delete!!";
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
