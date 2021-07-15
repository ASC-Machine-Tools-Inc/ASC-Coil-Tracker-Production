using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ASC_Coil_Tracker_Production.Data_Access_Layer;
using ASC_Coil_Tracker_Production.Models;

namespace ASC_Coil_Tracker_Production.Controllers
{
    public class HomeController : Controller
    {
        private CoilContext db = new CoilContext();

        public ActionResult Guide()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password_hash)
        {
            if (ModelState.IsValid)
            {
                string hashedPw = password_hash.GetHashCode().ToString();
                IQueryable<USERS> result = db.Users.Where(u => u.EMAIL.Equals(email) && u.PASSWORD_HASH.Equals(hashedPw));

                if (result.Any())
                {
                    return RedirectToAction("Index", "Coil");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }

            return View();
        }
    }
}