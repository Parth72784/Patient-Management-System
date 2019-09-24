using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using PatientManageSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PatientManageSystem.FilterConfig;
using iTextSharp.text.html.simpleparser;

namespace PatientManageSystem.Areas.User.Controllers
{
    [DrLoginFilter]
    public class DrIndexController : Controller
    { 
        private PMSEntities db = new PMSEntities();
        // GET: User/DrIndex
       
           
        
        public ActionResult Index(DateTime? AppointmentStartDate, DateTime? AppointmentEndDate)
        {
            int id = Convert.ToInt32(Session["DrId"]);
            if (AppointmentStartDate != null && AppointmentEndDate != null)
            {
                var appointment = db.AppointmentManages.Where(a => a.RefDid == id && a.AppointmentStartDate >= AppointmentStartDate && a.AppointmentEndDate <= AppointmentEndDate);
                return View(appointment.ToList());
            }
            var appointmentManages = db.AppointmentManages.Where(a=>a.RefDid==id);
            return View(appointmentManages.ToList());
           
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
    
    }
}