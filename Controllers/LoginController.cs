#region LIBRARIES
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineBookStore.Models;
using System.Net;
using System.Net.Mail;
#endregion

namespace OnlineBookStore.Controllers
{
    public class LoginController : Controller
    {
        private readonly LibraryDbContext db;
        private readonly ISession session;
        public LoginController(LibraryDbContext _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            session = httpContextAccessor.HttpContext.Session;
        }

        #region AUTO REDIRECT
        public class NoDirectAccessAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var canAcess = false;

                // check the refer
                var referer = filterContext.HttpContext.Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(referer))
                {
                    var rUri = new System.UriBuilder(referer).Uri;
                    var req = filterContext.HttpContext.Request;
                    if (req.Host.Host == rUri.Host && req.Host.Port == rUri.Port && req.Scheme == rUri.Scheme)
                    {
                        canAcess = true;
                    }
                }

                // ... check other requirements

                if (!canAcess)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "login", area = "" }));
                }
            }
        }
        #endregion

        #region NEW REGISTRATION
        [HttpGet]
        public IActionResult Registration()            // User Registration 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(NewRegistration r)
        {
            db.NewRegistrations.Add(r);
            db.SaveChanges();

            // Send Mail Confirmation for passsword

            var senderEmail = new MailAddress("librarymanagement13@gmail.com", "pooja");
            var receiverEmail = new MailAddress(r.MailId, "Receiver");
            var password = "kigksgbmzemtqrax";
            var sub = "Hello " + r.UserName + "! Welcome to Pooja Library Managemnet System";
            var body = "Your User Id: " + r.UserId + " And your password is :" + r.Password;
            var smtp = new SmtpClient
            {

                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
                ViewBag.Message = String.Format("Registered Successfully!!\\ Please Check Your Mail to login.");
            }
            return View("login");
        }

        #endregion

        #region LOGIN
        [HttpGet]
        public IActionResult login()               // User Login
        {
            return View();
        }
        [HttpPost]
        public IActionResult login(NewRegistration r)
        {
            var result = (from i in db.NewRegistrations
                          where i.UserId == r.UserId && i.Password == r.Password
                          select i).SingleOrDefault();
            if (result != null)
            {
                if (result.UserName == "Administrator")
                {
                    HttpContext.Session.SetString("UserName", result.UserName);
                    HttpContext.Session.SetString("UserId", result.UserId.ToString());

                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    HttpContext.Session.SetString("UserName", result.UserName);
                    HttpContext.Session.SetString("UserId", result.UserId.ToString());
                    return RedirectToAction("Index", "User");
                }
            }
            else
            {
                ViewBag.Message = String.Format("Try Again!!");
                return View();
            }

        }
        #endregion

        #region LOGOUT
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login", "Login");
        }
        #endregion

    }
}

