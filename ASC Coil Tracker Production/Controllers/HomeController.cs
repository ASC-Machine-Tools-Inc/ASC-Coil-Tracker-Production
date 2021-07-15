using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Login([Bind(Include = "Email,PasswordHash")] USERS user)
        {
            if (ModelState.IsValid)
            {
                string hashedPw = user.PasswordHash.GetHashCode().ToString();
                IQueryable<USERS> result = db.Users.Where(u => u.Email.Equals(user.Email) &&
                                                               u.PasswordHash.Equals(hashedPw));

                if (result.Any())
                {
                    // If user not added, create new identity.
                    Session["Email"] = user.Email;

                    return RedirectToAction("Index", "Coil");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }

            return View();
        }

        public void Logout()
        {
            Session.Abandon();
            Response.Redirect("Login");
        }
    }
}