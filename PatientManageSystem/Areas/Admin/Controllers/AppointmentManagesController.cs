using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;


using PatientManageSystem.Models;
using static PatientManageSystem.FilterConfig;

namespace PatientManageSystem.Areas.Admin.Controllers
{
    [AdminLoginFilter]
    public class AppointmentManagesController : Controller
    {
        private PMSEntities db = new PMSEntities();

        // GET: Admin/AppointmentManages
        public ActionResult Index(DateTime? AppointmentStartDate,DateTime? AppointmentEndDate)
        {
            if (AppointmentStartDate != null && AppointmentEndDate !=null)
            {
                var appointment = db.AppointmentManages.Include(a => a.DoctorManage).Include(a => a.PatientManage).Where(a => a.AppointmentStartDate >= AppointmentStartDate && a.AppointmentEndDate <= AppointmentEndDate);
                return View(appointment.ToList());
            }
            var appointmentManages = db.AppointmentManages.Include(a => a.DoctorManage).Include(a => a.PatientManage);
            return View(appointmentManages.ToList());
        }

        // GET: Admin/AppointmentManages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Id is not available!" });
            }
            AppointmentManage appointmentManage = db.AppointmentManages.Find(id);
            if (appointmentManage == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Data is not available!" });
            }
            return View(appointmentManage);
        }
        [HttpPost]
        [ValidateInput(false)]
        [Obsolete]
        public FileResult Export(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A2);
          
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }

        // GET: Admin/AppointmentManages/Create
        public ActionResult Create()
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
        public ActionResult Create([Bind(Include = "RefPid,RefDid,AppointmentStartDate,AppointmentEndDate,Purpose,Status,DocumentUpload,Cretedby,Modifiedby,Creteddate,Modifieddate,Aid")] AppointmentManage appointmentManage,HttpPostedFileBase DocumentUpload)
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
                    return RedirectToAction("Index");
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Id is not Avalible!!" });
            }
            AppointmentManage appointmentManage = db.AppointmentManages.Find(id);
            if (appointmentManage == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Data is not Avalible!!" });
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
        public ActionResult Edit([Bind(Include = "RefPid,RefDid,AppointmentStartDate,AppointmentEndDate,Purpose,Status,DocumentUpload,Cretedby,Modifiedby,Creteddate,Modifieddate,Aid")] AppointmentManage appointmentManage)
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
                    return RedirectToAction("Index");
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

        // GET: Admin/AppointmentManages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {

                return RedirectToAction("Error", "Dashboard", new { err = "Id is not Avalible!!" });
            }
            AppointmentManage appointmentManage = db.AppointmentManages.Find(id);
            if (appointmentManage == null)
            {
                return RedirectToAction("Error", "Dashboard", new { err = "Data is not Avalible!!" });
            }
            return View(appointmentManage);
        }

        // POST: Admin/AppointmentManages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                AppointmentManage appointmentManage = db.AppointmentManages.Find(id);
                db.AppointmentManages.Remove(appointmentManage);
                db.SaveChanges();
                TempData["msg"] = "Appointment Cancle!!";
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
