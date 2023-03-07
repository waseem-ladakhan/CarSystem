using CarSystem.App_Start;
using CarSystem.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using context = System.Web.HttpContext;

namespace CarSystem.Controllers
{
    public class HomeController : Controller
    {
        CarProjectEntities db = new CarProjectEntities();
        public ActionResult Index()
        {
            return View();
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
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult search(string txt1)
        {
            var res = from t in db.cars
                      where t.carName.StartsWith(txt1)
                      select t;
            return View(res.ToList());
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult unauthorized()
        {
            return View();
        }
        public ActionResult SearchByDate(DateTime date)
        {

            var res = (from t in db.cars.ToList()
                       from t1 in db.myBookings.ToList()
                       where t.CarStatus == true && t1.startDate == date
                       select new carJoin
                       {
                           cars = t,
                           myBooking = t1
                       }).ToList();


            return View(res);

        }
        public class carJoin
        {
            public car cars { get; set; }
            public myBooking myBooking { get; set; }
        }
        [HttpPost]
        public ActionResult Login(string uname, string pwd)
        {
            Session["u"] = uname;

            var user = (from t in db.Registrations where t.UserName == uname && t.password == pwd select t).Count();
            if (uname == "waseem" && pwd == "waseem@123")
            {
                return RedirectToAction("ShowCars", "Admin");
            }
            else if (user > 0)
            {
                return RedirectToAction("Vehicles");
            }
            else
            {
                return RedirectToAction("unauthorized");
            }
        }
        public ActionResult Registrations()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrations(Registration reg)
        {
            string errorMessage;
            if (!reg.IsValid(out errorMessage))
            {
                ViewBag.Errors = new List<string> { errorMessage };
                return View();
            }

            if (db.Registrations.Any(r => r.UserName == reg.UserName))
            {
                ViewBag.Errors = new List<string> { "User already exists" };
                return View();
            }
            int rowsaffected = db.SaveChanges();
            if (rowsaffected > 0)
            {
                ViewData["reg"] = "Your Registrations Sucessfully Completed";
            }
            else
            {
                ViewData["reg"] = "Check Your details Once";
            }
            ViewBag.SuccessMessage = "Registration successful!";
            db.Registrations.Add(reg);
            db.SaveChanges();

            return View();
        }
        public ActionResult Logout()
        {
            Session["u"] = null;
            return View();
        }
        public ActionResult Bookings()
        {
            if (Session["u"] == null)
            {
                return View("Login");
            }
            ViewBag.ra = null;
            if (Session["u"] == null)
            {
                return RedirectToAction("Login");
            }
            string st = Request.QueryString["registration"];
            ViewData["registration"] = st;
            TempData["carID"] = st;
            return View();
        }


        public ActionResult showbydate(DateTime? date)
        {
            DateTime date1 = DateTime.Now; if (date == null)
            {
                ViewBag.msg = "Records Not Found ur date";
            }
            var res = from t in db.cars.ToList()
                      from t1 in db.myBookings.ToList()
                      where t.CarStatus == true && t1.startDate == date
                      select new carssjoin
                      {
                          cars = t,
                          myBooking = t1
                      };
            return View(res.ToList());
        }
    
        public class carssjoin
        {
            public car cars { get; set; }
            public myBooking myBooking { get; set; }
        }


    [HttpPost]
        public ActionResult Bookings(myBooking myBooking)
        {
            try
            {
                string Rid = TempData["carID"].ToString();
                myBooking.RegistrationId = Rid;
                myBooking.UserName = Session["u"].ToString();
                myBooking.bookingDate = DateTime.Now;
                db.myBookings.Add(myBooking);
                int rowsaffected = db.SaveChanges();
                if (rowsaffected > 0)
                {
                    return RedirectToAction("myBookings");
                }
                else
                {
                    return View("Error");
                }
               
            }
            catch (Exception ex)
            {
                string uname = Session["u"].ToString();
                string GetClassName = ex.GetType().Name.ToString();
                string URL = context.Current.Request.Url.ToString();
                return View(GlobalException.GetException(ex.Message, ex.StackTrace, uname, ex.Source, GetClassName, URL));

            }

        }
        [HttpGet]
        public ActionResult myBookings()
        {
            if (Session["u"] == null)
            {
                return View("Login");
            }
            var res = (from t in db.myBookings select t).ToList();
            
            return View(res);
        }

        public ActionResult Vehicles()
        {
            var res = from t in db.cars where t.CarStatus == true
                      select t;
            return View(res.ToList());

        }
            
        public ActionResult Feedback()
        {
            if (Session["u"] == null)
            {
                return View("Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Feedback(FormCollection f)
        {
            try
            {
                Feedback ob = new Feedback();
                string username = f["name"].ToString();
                var res = (from t in db.Feedbacks where t.UserName == username select t).Count();
                if(res == 0)
                {
                    ob.UserName = f["name"];
                    ob.Description = f["comments"];
                    ob.Ratings = f["ratings"];
                    db.Feedbacks.Add(ob);
                    int rowsaffected = db.SaveChanges();
                    ViewData["r"] = "Feedback Sumbited suc!!!";
                }
                else
                {
                    ViewData["r"] = "you alredy sumbimted the feedback!";
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
   
        public ActionResult UserProfile()
        {
            if (Session["u"] == null)
            {
                return View("Login");
            }
            string username = Session["u"].ToString();
            Registration user = db.Registrations.FirstOrDefault(u => u.UserName == username);
            
            return View(user);
            
        }
        [HttpPost]
        public ActionResult UserProfile(int amount)
        {
            string username = User.Identity.Name;
            Bank bank = db.Banks.FirstOrDefault(b => b.UserName == username);
            bank.BalanceAmount += amount; 
            db.SaveChanges(); 
            return RedirectToAction("Userprofile");
        }
        public ActionResult Payment()
        {
            if (Session["u"] == null)
            {
                return View("Login", "Home");
            }
            try
            {
                string CarID = Request.QueryString["regID"];
                int BookingId = Convert.ToInt32(Request.QueryString["bid"]);
               
                Bank bank = new Bank();
                myBooking mb = new myBooking();
                var username = Session["u"];
                
                var balanceAmount = (from t in db.Banks where t.UserName == username select t.BalanceAmount).FirstOrDefault();
                var sdate = (from t in db.myBookings where t.BookingId == BookingId select t.startDate).FirstOrDefault();
                var eDate = (from t in db.myBookings where t.BookingId == BookingId select t.ReturnDate).FirstOrDefault();
                int RentPerday = Convert.ToInt32((from t in db.cars where t.RegistrationId == CarID select t.RentPerDay).FirstOrDefault());
                string totalDays = (eDate - sdate).ToString().Substring(0, 1);
                ViewData["CarID"] = CarID;
                ViewData["tDays"] = totalDays;
                ViewData["RentPerday"] = RentPerday;
                ViewData["balanceAmount"] = balanceAmount;
                int totalAmount = RentPerday * Convert.ToInt32(totalDays);
                ViewData["tamount"] = totalAmount;
                TempData["BookingId"] = BookingId;
                TempData["CarID"] = CarID;
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

        [HttpPost]
        public ActionResult Payment(int AcNumber, string Password)
        {
            
            try
            {
                
                Bank bank = new Bank();
                myBooking mb = new myBooking();
                invoice invoiceData = new invoice();
                var username = Session["u"].ToString();
                string carID = TempData["CarID"].ToString();
                int BookingId = Convert.ToInt32(TempData["BookingId"]);
                var balanceAmount = (from t in db.Banks where t.UserName == username select t.BalanceAmount).FirstOrDefault();
                var sdate = (from t in db.myBookings where t.BookingId == BookingId select t.startDate).FirstOrDefault();
                var eDate = (from t in db.myBookings where t.BookingId == BookingId select t.ReturnDate).FirstOrDefault();
                int RentPerday = Convert.ToInt32((from t in db.cars where t.RegistrationId == carID select t.RentPerDay).FirstOrDefault());
                string totalDays = (eDate - sdate).ToString().Substring(0, 1);
                int totalAmount = RentPerday * Convert.ToInt32(totalDays);
                if (AcNumber != 0 || Password != ""  || username != null)
                {
                    var res = (from t in db.Banks 
                               where t.UserName == username 
                               && t.AccountNumber == AcNumber 
                               && t.password == Password select t).FirstOrDefault();
                    res.BalanceAmount = balanceAmount - totalAmount;
                    int i = db.SaveChanges();
                    if(i > 0)
                    {
                        ViewData["res"] = i;
                        invoiceData.bookingDate = DateTime.Now;
                        invoiceData.startDate = sdate;
                        invoiceData.ReturnDate = eDate;
                        invoiceData.RentPerDay = RentPerday;
                        invoiceData.RegistrationId = carID;
                        invoiceData.UserName = username;
                        invoiceData.TotalAmmount = totalAmount;
                        invoiceData.BookingID = BookingId;
                        db.invoices.Add(invoiceData);
                        db.SaveChanges();
                    }

                    car c = new car();
                    
                    var resCars = (from t in db.cars where t.RegistrationId == carID select t).FirstOrDefault();
                    
                    resCars.CarStatus = false;
                    db.SaveChanges();
                    return  RedirectToAction("invoice");
                }
                else
                {
                    return View("Error");
                }
                
            }
            catch (Exception ex)
            {
                string uname = Session["u"].ToString();
                string GetClassName = ex.GetType().Name.ToString();
                string URL = context.Current.Request.Url.ToString();
                return View(GlobalException.GetException(ex.Message, ex.StackTrace, uname, ex.Source, GetClassName, URL));
            }

        }
        public ActionResult invoice()
        {
            if (Session["u"] == null)
            {
                return View("Login");
            }

            var res = db.invoices.ToList();
            return View(res.ToList());
        }

        public ActionResult BookedCars()
        {
            if (Session["u"] == null)
            {
                return View("Login");
            }
            return View();
        }

        private class ViewModel
        {
            public Bank Banks { get; set; }
            public myBooking myBookings { get; set; }
        }
    }
}
