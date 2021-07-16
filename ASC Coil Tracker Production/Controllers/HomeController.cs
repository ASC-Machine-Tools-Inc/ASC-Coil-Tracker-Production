using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
            }

            ModelState.AddModelError("LoginError",
                "There was an error with logging in. Please " +
                "double-check your login and retry.");
            return View();
        }

        public void Logout()
        {
            Session.Abandon();
            Response.Redirect("Login");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, ActionName("ChangePassword")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordPost()
        {
            if (Request.Form["OldPassword"] == null ||
                Request.Form["NewPassword"] == null)
            {
                ModelState.AddModelError("ChangeError",
                    "Please fill out all fields.");
                return ChangePassword();
            }

            string currEmail = Session["Email"].ToString();
            string hashedPw = Request.Form["OldPassword"].GetHashCode().ToString();
            var userQuery = db.Users.Where(u =>
                u.Email.Equals(currEmail) &&
                u.PasswordHash.Equals(hashedPw));

            // Check user exists with the current email and entered password.
            if (userQuery.Any())
            {
                USERS userToUpdate = userQuery.FirstOrDefault();
                userToUpdate.PasswordHash = Request.Form["NewPassword"]
                    .GetHashCode().ToString();

                if (TryUpdateModel(userToUpdate))
                {
                    try
                    {
                        // Save changes and logout on a successful update.
                        db.SaveChanges();
                        return RedirectToAction("Logout");
                    }
                    catch (RetryLimitExceededException /* dex */)
                    {
                        // Log the error (uncomment dex and add line here to write log
                        ModelState.AddModelError("ChangeError",
                            "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                }
            }

            ModelState.AddModelError("ChangeError",
                "Error updating password. Please check your input " +
                "and retry.");
            return ChangePassword();
        }

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost, ActionName("AddUser")]
        [ValidateAntiForgeryToken]
        public ActionResult AddUserPost([Bind(Include = "Email,PasswordHash")] USERS user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check that the email is not already in the database.
                    if (db.Users.Any(u => u.Email.Equals(user.Email)))
                    {
                        ModelState.AddModelError("AddUserError", "Email already in database.");
                        return View();
                    }

                    // Hash the password.
                    user.PasswordHash = user.PasswordHash.GetHashCode().ToString();

                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Coil");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                // Log the error (uncomment dex and add line here to write log)
                ModelState.AddModelError("AddUserError", "Unable to create new user - " +
                                                         "RetryLimitExceededException.");
            }

            return View();
        }
    }
}