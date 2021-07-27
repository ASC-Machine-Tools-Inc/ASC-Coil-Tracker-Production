using ASC_Coil_Tracker_Production.Data_Access_Layer;
using ASC_Coil_Tracker_Production.Models;
using PagedList;
using PrintLogic;
using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ASC_Coil_Tracker_Production.Controllers
{
    public class CoilController : Controller
    {
        private CoilContext db = new CoilContext();

        // GET: Coil
        public ViewResult Index(string sortOrder,
            string searchString, string currentFilter,
            string searchFilter, string currentSearchFilter,
            string lengthFilter, string currentLengthFilter,
            int? page)
        {
            /* Default: sort by ID desc
             * This lets the user sort by ID asc if they choose.
             */
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "id_asc" : "";

            // Reset page to 1 if new search string - otherwise, keep filter
            if (searchString != null || searchFilter != null || lengthFilter != null)
            {
                page = 1;
            }

            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            if (searchFilter == null)
            {
                searchFilter = currentSearchFilter;
            }
            ViewBag.CurrentSearchFilter = searchFilter;

            if (lengthFilter == null)
            {
                if (currentLengthFilter != null)
                {
                    lengthFilter = currentLengthFilter;
                }
                else
                {
                    lengthFilter = "NON-DEPLETED";
                }
            }
            ViewBag.CurrentLengthFilter = lengthFilter;

            // Length list initialization
            var lengthList = new[]
            {
                new SelectListItem { Text = "Non-depleted", Value = "NON-DEPLETED" },
                new SelectListItem { Text = "All", Value = "ALL" }
            };
            ViewBag.LengthList = new SelectList(lengthList, "Value", "Text", lengthFilter);

            // Search select list initialization
            var searchList = new[]
            {
                new SelectListItem { Text = "All Fields", Value = "ALL"},
                new SelectListItem { Text = "ID", Value = "ID"},
                new SelectListItem { Text = "Color", Value = "COLOR"},
                new SelectListItem { Text = "Material", Value = "TYPE"},
                new SelectListItem { Text = "Gauge", Value = "GAUGE"},
                new SelectListItem { Text = "Width", Value = "WIDTH"},
                new SelectListItem { Text = "Thickness", Value = "THICK"},
                new SelectListItem { Text = "Yield", Value = "YIELD"},
                new SelectListItem { Text = "Length", Value = "LENGTH"},
                new SelectListItem { Text = "Weight", Value = "WEIGHT"},
                new SelectListItem { Text = "Customer", Value = "CUSTOMER"},
                new SelectListItem { Text = "Notes", Value = "NOTES"}
            };
            ViewBag.SearchList = new SelectList(searchList, "Value", "Text", searchFilter);

            var coils = from c in db.Coils select c;

            // Let user search by given field.
            if (!String.IsNullOrEmpty(searchString))
            {
                switch (searchFilter)
                {
                    case "ALL":
                        coils = coils.Where(
                            c =>
                                 c.ID.ToString().Contains(searchString) ||
                                 c.COLOR.Contains(searchString) ||
                                 c.TYPE.Contains(searchString) ||
                                 c.GAUGE.Contains(searchString) ||
                                 c.THICK.ToString().Contains(searchString) ||
                                 c.WIDTH.ToString().Contains(searchString) ||
                                 c.YIELD.ToString().Contains(searchString) ||
                                 c.WEIGHT.ToString().Contains(searchString) ||
                                 c.LENGTH.ToString().Contains(searchString) ||
                                 c.NOTES.Contains(searchString) ||
                                 c.CUSTOMER.Contains(searchString)
                        );
                        break;

                    case "ID":
                        coils = coils.Where(c => c.ID.ToString().Contains(searchString));
                        break;

                    case "COLOR":
                        coils = coils.Where(c => c.COLOR.Contains(searchString));
                        break;

                    case "TYPE":
                        coils = coils.Where(c => c.TYPE.Contains(searchString));
                        break;

                    case "GAUGE":
                        coils = coils.Where(c => c.GAUGE.Contains(searchString));
                        break;

                    case "WIDTH":
                        coils = coils.Where(c => c.WIDTH.ToString().Contains(searchString));
                        break;

                    case "THICK":
                        coils = coils.Where(c => c.THICK.ToString().Contains(searchString));
                        break;

                    case "YIELD":
                        coils = coils.Where(c => c.YIELD.ToString().Contains(searchString));
                        break;

                    case "LENGTH":
                        coils = coils.Where(c => c.LENGTH.ToString().Contains(searchString));
                        break;

                    case "WEIGHT":
                        coils = coils.Where(c => c.WEIGHT.ToString().Contains(searchString));
                        break;

                    case "CUSTOMER":
                        coils = coils.Where(c => c.CUSTOMER.Contains(searchString));
                        break;

                    case "NOTES":
                        coils = coils.Where(c => c.NOTES.Contains(searchString));
                        break;
                }
            }

            // Sort by length filter.
            if (lengthFilter.Equals("NON-DEPLETED"))
            {
                coils = coils.Where(c => c.LENGTH > 0.0 || c.LENGTH == null);
            }
            // Else, show all coils.

            switch (sortOrder)
            {
                case "id_asc":
                    coils = coils.OrderBy(c => c.ID);
                    break;

                default:
                    coils = coils.OrderByDescending(c => c.ID);
                    break;
            }

            // Default number of records per page.
            int pageSize = 50;
            // If page not null, set page number to 1, else use current page.
            int pageNumber = page ?? 1;

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

            // Calculate original length
            ViewBag.OriginalLength = CalculateLength(coil);

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
        public ActionResult Create([Bind(Include = "COLOR,TYPE,GAUGE,THICK,WEIGHT,LENGTH,NOTES,YIELD,WIDTH,CUSTOMER")] COILTABLE coil)
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
               new string[] { "COLOR", "TYPE", "GAUGE", "THICK",
                   "WEIGHT", "LENGTH", "NOTES", "YIELD", "WIDTH", "CUSTOMER" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    // Log the error (uncomment dex and add line here to write log
                    ModelState.AddModelError("", "Unable to save changes. " +
                                                 "Try again, and if the problem persists, " +
                                                 "see your system administrator.");
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

            // Calculate original length
            ViewBag.OriginalLength = CalculateLength(coil);
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
            printer.PrintCoil(coil.ID.ToString(), coil.COLOR, coil.TYPE, coil.GAUGE, coil.THICK,
                coil.WEIGHT, coil.LENGTH, coil.WIDTH, coil.YIELD, coil.NOTES);
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

        // Takes a coil and returns its actual length by adding the amount used in its history events.
        private int CalculateLength(COILTABLE coil)
        {
            int currentLength = 0;
            if (coil.LENGTH != null)
            {
                currentLength = (int)coil.LENGTH;
                var history = from h in coil.COILTABLEHISTORY
                              where h.AMOUNTUSED != null
                              select h;

                foreach (var h in history)
                {
                    currentLength += (int)h.AMOUNTUSED;
                }
            }
            return currentLength;
        }
    }
}