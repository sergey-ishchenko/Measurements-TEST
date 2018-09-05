using MeasurementsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeasurementsApp.Controllers
{
    public class HomeController : Controller
    {
        MeasurementsDBEntities db = null;
        public HomeController()
        {
            db = new MeasurementsDBEntities();
        }
        public ActionResult Index(string  dateFromStr, string dateToStr, int deviceId = 1)
        {
            DateTime dateFrom = dateFromStr != null ? DateTime.Parse(dateFromStr) : DateTime.Now.AddMonths(-1);
            DateTime dateTo = dateToStr != null ? DateTime.Parse(dateToStr) : DateTime.Now;

            var filterReadings = db.Readings.Where(i => i.DeviceId == deviceId && i.MeasurementDateTime >= dateFrom && i.MeasurementDateTime <= dateTo).OrderBy(i => i.MeasurementDateTime).ToList();

            ViewBag.Devices = new SelectList(db.Devices.ToList(), "Id", "Name", deviceId);
            ViewBag.DateFrom = dateFrom;
            ViewBag.DateTo = dateTo;
            return View(filterReadings);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}