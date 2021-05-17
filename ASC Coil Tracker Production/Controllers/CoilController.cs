using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using ASC_Coil_Tracker_Production.Data_Access_Layer;
using ASC_Coil_Tracker_Production.Models;
using PagedList;
using PrintLogic;

namespace ASC_Coil_Tracker_Production.Controllers
{
    public class CoilController : Controller
    {
        private CoilContext db = new CoilContext();

        // GET: Coil
        public ViewResult Index(string sortOrder, string currentFilter, string searchString,
            string searchFilter, string currentSearchFilter, int? page)
        {
            /* Default: sort by ID desc
             * This lets the user sort by ID asc or job asc, ID desc
             * if they choose.
             */
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "id_asc" : "";
            ViewBag.JobSortParm = sortOrder == "Job #" ? "job_desc" : "Job #";

            // Reset page to 1 if new search string - otherwise, keep filter
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            if (searchFilter != null)
            {
                page = 1;
            }
            else
            {
                searchFilter = currentSearchFilter;
            }

            var coils = from c in db.Coils
                        select c;

            // Let user search by given field
            if (!String.IsNullOrEmpty(searchString))
            {
                switch (searchFilter)
                {
                    case "JOBNUMBER":
                        coils = coils.Where(c => c.JOBNUMBER.Contains(searchString));
                        break;

                    case "GAUGE":
                        coils = coils.Where(c => c.GAUGE.Contains(searchString));
                        break;
                }
            }

            switch (sortOrder)
            {
                case "id_asc":
                    coils = coils.OrderBy(c => c.ID);
                    break;

                case "Job #":
                    coils = coils.OrderBy(c => c.JOBNUMBER);
                    break;

                case "job_desc":
                    coils = coils.OrderByDescending(c => c.JOBNUMBER);
                    break;

                default:
                    coils = coils.OrderByDescending(c => c.ID);
                    break;
            }

            // Default number of records per page
            int pageSize = 50;
            // If page not null, set page number to 1, else use page
            int pageNumber = (page ?? 1);
            return View(coils.ToPagedList(pageNumber, pageSize));
        }

        // GET: Coil/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COILTABLE coil = db.Coils.Find(id);
            if (coil == null)
            {
                return HttpNotFound();
            }
            return View(coil);
        }

        // GET: Coil/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Coil/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "COLOR,TYPE,GAUGE,THICK,WEIGHT,LENGTH,NOTES,JOBNUMBER")] COILTABLE coil)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Coils.Add(coil);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                // Log the error (uncomment dex and add line here to write log
                ModelState.AddModelError("", "Unable to create new coil record. Try again, and report the issue in Contacts if the problem persists.");
            }

            return View(coil);
        }

        // GET: Coil/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COILTABLE coil = db.Coils.Find(id);
            if (coil == null)
            {
                return HttpNotFound();
            }
            return View(coil);
        }

        // POST: Coil/Edit/5
        // Implements security best practice from Microsoft website to prevent overposting.
        // See: Microsoft -> Implementing basic crud functionality
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var coilToUpdate = db.Coils.Find(id);
            if (TryUpdateModel(coilToUpdate, "",
               new string[] { "COLOR", "TYPE", "GAUGE", "THICK", "WEIGHT", "LENGTH", "NOTES", "JOBNUMBER" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    // Log the error (uncomment dex and add line here to write log
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(coilToUpdate);
        }

        // GET: Coil/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COILTABLE coil = db.Coils.Find(id);
            if (coil == null)
            {
                return HttpNotFound();
            }
            return View(coil);
        }

        // POST: Coil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            COILTABLE coil = db.Coils.Find(id);
            db.Coils.Remove(coil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Coil/Print/5
        public ActionResult Print(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COILTABLE coil = db.Coils.Find(id);
            if (coil == null)
            {
                return HttpNotFound();
            }
            return View(coil);
        }

        // POST: Coil/Print/5
        [HttpPost, ActionName("Print")]
        [ValidateAntiForgeryToken]
        public ActionResult PrintConfirmed(int id)
        {
            COILTABLE coil = db.Coils.Find(id);
            var printer = new Print();
            printer.PrintCoil(coil.ID.ToString(), coil.COLOR, coil.TYPE, coil.GAUGE, coil.THICK, coil.WEIGHT, coil.LENGTH);
            return RedirectToAction("Index");
        }

        // GET: Coil/Calculator
        public ActionResult Calculator()
        {
            return View();
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