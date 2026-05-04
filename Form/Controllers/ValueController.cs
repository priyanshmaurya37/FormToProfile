using Form.Models;
using Form.Servies;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Form.Controllers
{
    public class ValueController : Controller
    {
        private readonly DBConnection con;
        private readonly EmailSender emailSender;
        public ValueController(DBConnection con, EmailSender emailSender )
        {
            this.con= con;
            this.emailSender = emailSender;
        }

        // User sign Page
        public IActionResult Index()
        {
            ViewBag.States = con.States.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Index(SignForm data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var already = con.SignForm.FirstOrDefault(x => x.Mobile == data.Mobile);

            if (already != null)
            {
                TempData["Error"] = "User mobile already signed up";
                return View(data);
            }

            TempData["success"] = "User registered successfully!";
            con.SignForm.Add(data);
            con.SaveChanges();

            return RedirectToAction("LoginPage");
        }

        // User sign page end


        public JsonResult GetCities(int stateId)
        {
            var cities = con.Cities
                .Where(x => x.StateId == stateId)
                .Select(x => new {
                    cityId = x.CityId,
                    cityName = x.CityName
                })
                .ToList();

            return Json(cities);
        }
        // State and City Dropdown end


        // Login Page
        public IActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPage(IFormCollection data)
        {
            string Email = data["Email"];
            string Password = data["Password"];

            var Exitdata = con.SignForm.FirstOrDefault(x=>x.Email == Email && x.Password == Password);
            if(Exitdata != null)
            {
                HttpContext.Session.SetString("User", Email);
                TempData["success"] = "User Login successfully!";
                return RedirectToAction("ProfilePage");
            }
            else
            {
                TempData["error"] = "User Password Incorrect ";
                return RedirectToAction("LoginPage");
            }
        }
        // Login Page end

        // email otp verify

        public IActionResult Forget_Password()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult Send_OTP(IFormCollection data)
        {
            string email = data["Email"];
            var emailcheck =con.SignForm.FirstOrDefault(x => x.Email == email);
            if (emailcheck != null)
            {
                HttpContext.Session.SetString("email", email);
                Random random = new Random();
                int OTP = random.Next(1000, 9999);

                // OPT ko stro kare ge esme
                HttpContext.Session.SetString("oldotp", OTP.ToString());

                string sendto = email;
                string PMHepta_Collage = "Admin Login - Forgot Password";
                string body = $"Dear Admin,\n" +
                              $"We received a request to reset your password.\n" +
                              $"Your One-Time Password (OTP) is: {OTP}\n" +
                              $"Please do not share this OTP with anyone.\n";

                emailSender.SendMail(sendto, PMHepta_Collage, body);

                return RedirectToAction("Verify_OTP");

            }
            else
            {
                return RedirectToAction("Forget_Password");
            }
        }


        public IActionResult Verify_OTP()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Check_OTP(IFormCollection data)
        {
            string userotp = data["OTP"];
            string oldotp = HttpContext.Session.GetString("oldotp");
            if (userotp == oldotp)
            {
                return RedirectToAction("Change_Password");
            }
            else
            {
                return RedirectToAction("Verify_OTP");
            }
        }

        public IActionResult Change_Password()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Update_Password(IFormCollection data)
        {
            string newpassword = data["newpassword"];
            string confirmPassword = data["confirmPassword"];
            string email = HttpContext.Session.GetString("email");
            var userdata = con.SignForm.FirstOrDefault(x => x.Email == email);

            userdata.Password = newpassword;
            userdata.ConPassword = confirmPassword;

            con.SignForm.Update(userdata);
           con.SaveChanges();
            return RedirectToAction("LoginPage");
        }



        ///





        public IActionResult ProfilePage()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Login");
            }
            string Email = HttpContext.Session.GetString("User");
            var data = con.SignForm.FirstOrDefault(x => x.Email == Email);

            var stateName = con.States
      .Where(s => s.StateId == data.State)
      .Select(s => s.StateName)
      .FirstOrDefault();

            var cityName = con.Cities
                .Where(c => c.CityId == data.City)
                .Select(c => c.CityName)
                .FirstOrDefault();

            ViewBag.StateName = stateName;
            ViewBag.CityName = cityName;

            return View(data);

        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage");
        }


        [HttpPost]
        public IActionResult DeleteUser()
        {
            string email = HttpContext.Session.GetString("User");

            if (email == null)
            {
                return RedirectToAction("LoginPage");
            }

            var user = con.SignForm.FirstOrDefault(x => x.Email == email);

            if (user != null)
            {
                con.SignForm.Remove(user);
                con.SaveChanges();
            }

            HttpContext.Session.Clear();

            TempData["success"] = "Account deleted successfully!";
            return RedirectToAction("LoginPage");
        }

        //[HttpPost]
        //public IActionResult DeleteUser()
        //{
        //    string email = HttpContext.Session.GetString("User");

        //    if (email == null)
        //    {
        //        return RedirectToAction("LoginPage");
        //    }

        //    var user = con.SignForm.FirstOrDefault(x => x.Email == email);

        //    con.SignForm.Remove(user);
        //    con.SaveChanges();

        //    HttpContext.Session.Clear();

        //    return RedirectToAction("LoginPage");
        //}



        public IActionResult EditData()
        {
            string email = HttpContext.Session.GetString("User");

            if (email == null)
            {
                return RedirectToAction("LoginPage");
            }
            var user = con.SignForm.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                return RedirectToAction("LoginPage");
            }

            // important: dropdown data
            ViewBag.States = con.States.ToList();

            ViewBag.Cities = (from c in con.Cities
                              join s in con.States on c.StateId equals s.StateId
                              where s.StateName == user.State.ToString()
                              select c).ToList();

            return View(user);

        }


        [HttpPost]
public IActionResult EditData(SignForm data)
{
    string email = HttpContext.Session.GetString("User");

    if (email == null)
        return RedirectToAction("LoginPage");

    var user = con.SignForm.FirstOrDefault(x => x.Id == data.Id);

    if (user == null)
        return RedirectToAction("ProfilePage");

    user.UserName = data.UserName;
    user.Email = data.Email;
    user.Mobile = data.Mobile;
    user.Address = data.Address;
    user.State = data.State;
    user.City = data.City;

    con.SaveChanges();

    return RedirectToAction("ProfilePage");
}

        //[HttpPost]
        //public IActionResult EditData(SignForm data)
        //{
        //    string email = HttpContext.Session.GetString("User");
        //    if (email == null)
        //    {
        //        return RedirectToAction("LoginPage");
        //    }

        //    var user = con.SignForm.Find(data.Id);

        //    user.UserName = data.UserName;
        //        user.Email = data.Email;
        //        user.Mobile = data.Mobile;
        //        user.Address = data.Address;
        //        user.State = data.State;
        //        user.City = data.City;
        //        con.SignForm.Update(user);
        //        con.SaveChanges();

        //    return RedirectToAction("ProfilePage");
        //}







    }
}
