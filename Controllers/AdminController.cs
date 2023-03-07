using CarSystem.App_Start;
using CarSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using context = System.Web.HttpContext;
namespace CarSystem.Controllers
{
    
    public class AdminController : Controller
    {
        CarProjectEntities db = new CarProjectEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            return View();
        }
        public ActionResult addNewCar()
        {
            if (Session["u"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult addNewCar(car r)
        {
            
            try
            {
                db.cars.Add(r);
                int rowsaffected = db.SaveChanges();
                if (rowsaffected > 0)
                {
                    ViewData["r"] = "New Car Added Successfully!!";
                }
                else
                {
                    ViewData["r"] = "Error Occured Try again!!";
                }
            }
            catch (Exception ex)
            {
                string uname = Session["u"].ToString();
                string GetClassName = ex.GetType().Name.ToString();
                string URL = context.Current.Request.Url.ToString();
                return View(GlobalException.GetException(ex.Message, ex.StackTrace, uname, ex.Source, GetClassName, URL));

            }
            return View();
        }
        public ActionResult ShowCars()
        {
            if (Session["u"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var res = from t in db.cars select t;
            return View(res.ToList());
        }

       
      
        public ActionResult DeleteCar(string carID)
        {
            if (Session["u"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            try
            {
                var res = (from t in db.cars where t.RegistrationId == carID select t).SingleOrDefault();
                db.cars.Remove(res);
                db.SaveChanges();
                return View();
            }
            catch (Exception ex)
            {
                string uname = Session["u"].ToString();
                string GetClassName = ex.GetType().Name.ToString();
                string URL = context.Current.Request.Url.ToString();
                return View(GlobalException.GetException(ex.Message, ex.StackTrace, uname, ex.Source, GetClassName, URL));

            }
            
        }
        public ActionResult UpdateCar()
        {
            if (Session["u"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }
        [HttpPost]
        public ActionResult UpdateCar(FormCollection f)
        {


            try
            {
                car r = new car();
                r.RegistrationId = f["registration"];
                var res = db.cars.Where(x => x.RegistrationId == r.RegistrationId).FirstOrDefault();
                if (res.RegistrationId == f["registration"])
                {

                    if (res != null)
                    {
                        res.carName = f["carName"];
                        res.Model = f["Model"];
                        res.Image = f["Image"];
                        res.Color = f["Color"];
                        res.Conditioner = bool.Parse(f["Conditioner"]);
                        res.Fuletype = bool.Parse(f["fullType"]);
                       /* res.RentPerDay = int.Parse(f["rentPerday"]);
                        res.capacity = int.Parse(f["capacity"]);*/
                        res.CarStatus = bool.Parse(f["CarStatus"]);
                        db.SaveChanges();
                        return RedirectToAction("ShowCars");
                    }
                    else
                    {
                        ViewData["AddNewCar"] = "Updated succuesfuly";
                        return View("addNewCar");
                        
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                string uname = Session["u"].ToString();
                string GetClassName = ex.GetType().Name.ToString();
                string URL = context.Current.Request.Url.ToString();
                return View(GlobalException.GetException(ex.Message, ex.StackTrace, uname, ex.Source, GetClassName, URL));

            }
        }

        public ActionResult SearchByDate(DateTime fromDate, DateTime toDate)
        {
            if (Session["u"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var res = (from t in db.myBookings where t.bookingDate == fromDate && t.ReturnDate == toDate select t).ToList(); //
            return View(res);

        }
        public ActionResult AddDriver()
        {
            if (Session["u"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddDriver(FormCollection f)
        {

            try
            {
                Driver d = new Driver();
                d.DriverName = f["name"];
                d.DriverPhoneNum = long.Parse(f["PhoneNumber"]);
                d.LicenseDetails = f["license"];
                d.RegistrationId = f["CarID"];
               
                db.Drivers.Add(d);
                int rowsaffected = db.SaveChanges();
                if (rowsaffected > 0)
                {
                    ViewData["r"] = "New Driver Added Successfully!!";
                }
                else
                {
                    ViewData["r"] = "Error Occured Try again!!";
                }
                return View();
            }
            catch (Exception ex)
            {
                string uname = Session["u"].ToString();
                string GetClassName = ex.GetType().Name.ToString();
                string URL = context.Current.Request.Url.ToString();
                return View(GlobalException.GetException(ex.Message, ex.StackTrace, uname, ex.Source, GetClassName, URL));

            }
        }
        public ActionResult ShowFeedbacks()
        {
            var res = db.Feedbacks.ToList();
            return View(res);
        }
    }
}