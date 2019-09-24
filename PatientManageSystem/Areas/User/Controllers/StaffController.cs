using PatientManageSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatientManageSystem.Areas.User.Controllers
{
    public class StaffController : Controller
    {
        private PMSEntities db = new PMSEntities();
        // GET: User/Staff
        public ActionResult Index()
        {
            var StaffManages = db.StaffManages;
            return View(StaffManages.ToList());
            
        }
        // GET: Admin/AppointmentManages
        public ActionResult AIndex(DateTime? AppointmentStartDate, DateTime? AppointmentEndDate)
        {
            if (AppointmentStartDate != null && AppointmentEndDate != null)
            {
                var appointment = db.AppointmentManages;
                return View(appointment.ToList());
            }
            var appointmentManages = db.AppointmentManages;
            return View(appointmentManages.ToList());
        }
        public ActionResult ADetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Index", new { err = "Id is not available!" });
            }
            AppointmentManage appointmentManage = db.AppointmentManages.Find(id);
            if (appointmentManage == null)
            {
                return RedirectToAction("Error", "Index", new { err = "Data is not available!" });
            }
            return View(appointmentManage);
        }
        

        // GET: Admin/AppointmentManages/Create
        public ActionResult ACreate()
        {
            ViewBag.RefDid = new SelectList(db.DoctorManages, "DrId", "DrName");
            ViewBag.RefPid = new SelectList(db.PatientManages, "Pid", "Pname");
            return View();
        }

        // POST: Admin/AppointmentManages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ACreate([Bind(Include = "RefPid,RefDid,AppointmentStartDate,AppointmentEndDate,Purpose,Status,DocumentUpload,Cretedby,Modifiedby,Creteddate,Modifieddate,Aid")] AppointmentManage appointmentManage, HttpPostedFileBase DocumentUpload)
        {
            string dirPath = Server.MapPath("~/Content/Admin/Upload");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string strDateTime = System.DateTime.Now.ToString("ddMMyyyyHHMMss");
            string filename = "Report_" + strDateTime + "." + DocumentUpload.FileName.Split('.')[1];
            string filepath = dirPath + "\\" + filename;
            DocumentUpload.SaveAs(filepath);


            ViewBag.message = "File Uploaded!";
            appointmentManage.DocumentUpload = filename;
            if (ModelState.IsValid)
            {
                try
                {
                    var CretedName = Convert.ToString(Session["AdminName"]);
                    appointmentManage.Cretedby = CretedName;
                    appointmentManage.Creteddate = DateTime.Now;
                    db.AppointmentManages.Add(appointmentManage);
                    db.SaveChanges();
                    TempData["msg"] = "Appointment Set!!";
                    return RedirectToAction("AIndex");
                }
                catch (Exception ex)
                {

                    TempData["err"] = ex.Message;
                }

            }

            ViewBag.RefDid = new SelectList(db.DoctorManages, "DrId", "DrName", appointmentManage.RefDid);
            ViewBag.RefPid = new SelectList(db.PatientManages, "Pid", "Pname", appointmentManage.RefPid);
            return View(appointmentManage);
        }

        // GET: Admin/AppointmentManages/Edit/5
        public ActionResult AEdit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Index", new { err = "Id is not Avalible!!" });
            }
            AppointmentManage appointmentManage = db.AppointmentManages.Find(id);
            if (appointmentManage == null)
            {
                return RedirectToAction("Error", "Index", new { err = "Data is not Avalible!!" });
            }
            ViewBag.RefDid = new SelectList(db.DoctorManages, "DrId", "DrName", appointmentManage.RefDid);
            ViewBag.RefPid = new SelectList(db.PatientManages, "Pid", "Pname", appointmentManage.RefPid);
            return View(appointmentManage);
        }

        // POST: Admin/AppointmentManages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AEdit([Bind(Include = "RefPid,RefDid,AppointmentStartDate,AppointmentEndDate,Purpose,Status,DocumentUpload,Cretedby,Modifiedby,Creteddate,Modifieddate,Aid")] AppointmentManage appointmentManage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ModifiedName = Convert.ToString(Session["AdminName"]);
                    appointmentManage.Modifiedby = ModifiedName;
                    appointmentManage.Modifieddate = DateTime.Now;
                    db.Entry(appointmentManage).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["msg"] = "Appointment Updated!!";
                    return RedirectToAction("AIndex");
                }
                catch (Exception ex)
                {
                    TempData["err"] = ex.Message;
                }

            }
            ViewBag.RefDid = new SelectList(db.DoctorManages, "DrId", "DrName", appointmentManage.RefDid);
            ViewBag.RefPid = new SelectList(db.PatientManages, "Pid", "Pname", appointmentManage.RefPid);
            return View(appointmentManage);
        }
        public ActionResult DIndex()
        {
            return View(db.DoctorManages.ToList());
        }
        public ActionResult PIndex()
        {
            var patientManages = db.PatientManages.Include(p => p.CityMaster);
            return View(patientManages.ToList());
        }

        // GET: Admin/PatientManages/Details/5
        public ActionResult PDetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Index", new { err = "Id is not Avalible!!" });

            }
            PatientManage patientManage = db.PatientManages.Find(id);
            if (patientManage == null)
            {
                return RedirectToAction("Error", "Index", new { err = "Data is not Avalible!!" });


            }
            return View(patientManage);
        }

        // GET: Admin/PatientManages/Create
        public ActionResult PCreate()
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
        public ActionResult PCreate([Bind(Include = "Pid,Pname,Address1,Address2,Gender,DOB,PContactNumber,PEmail,Cretedby,Modifiedby,Creteddate,Modifieddate,RefCityid")] PatientManage patientManage)
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
                    return RedirectToAction("PIndex");
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
        public ActionResult PEdit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Index", new { err = "Id is not Avalible!!" });

            }
            PatientManage patientManage = db.PatientManages.Find(id);
            if (patientManage == null)
            {
                return RedirectToAction("Error", "Index", new { err = "Data is not Avalible!!" });
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
        public ActionResult PEdit([Bind(Include = "Pid,Pname,Address1,Address2,Gender,DOB,PContactNumber,PEmail,Cretedby,Modifiedby,Creteddate,Modifieddate,RefCityid")] PatientManage patientManage)
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

                    return RedirectToAction("PIndex");
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
    }
}