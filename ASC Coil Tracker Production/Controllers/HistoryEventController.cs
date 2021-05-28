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

namespace ASC_Coil_Tracker_Production.Controllers
{
    public class HistoryEventController : Controller
    {
        private CoilContext db = new CoilContext();

        // GET: HistoryEvent
        public ViewResult Index(string sortOrder, string currentFilter, string searchString,
            string searchFilter, string currentSearchFilter, int? page)
        {
            /* Default: sort by date desc
             * This lets the user sort by date asc or ID desc, date desc
             * if they choose.
             */
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_asc" : "";
            ViewBag.IDSortParm = sortOrder == "Coil ID" ? "id_desc" : "Coil ID";

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
            ViewBag.CurrentSearchFilter = searchFilter;

            var history = from h in db.History
                          select h;

            // Let user search by job number
            if (!String.IsNullOrEmpty(searchString))
            {
                switch (searchFilter)
                {
                    case "COILID":
                        history = history.Where(h => h.COILID.ToString().Contains(searchString));
                        break;

                    // TODO: fix searching by date
                    case "DATE":
                        try
                        {
                            DateTime searchDate = DateTime.ParseExact(searchString, "MM/dd/yyyy", null);
                            history = history.Where(h => h.DATE.Equals(searchDate));
                        }
                        catch (FormatException /* dex */)
                        {
                            // Log the error (uncomment dex and add line here to write log)
                            ModelState.AddModelError("DateFormatError", "Dates must be in the format (MM/dd/yyyy)!");
                        }
                        break;

                    case "JOBNUMBER":
                        history = history.Where(h => h.JOBNUMBER.Contains(searchString));
                        break;

                    case "NOTES":
                        history = history.Where(h => h.NOTES.Contains(searchString));
                        break;
                }
            }

            switch (sortOrder)
            {
                case "date_asc":
                    history = history.OrderBy(h => h.DATE);
                    break;

                case "Coil ID":
                    history = history.OrderBy(h => h.COILID);
                    break;

                case "id_desc":
                    history = history.OrderByDescending(h => h.COILID);
                    break;

                default:
                    history = history.OrderByDescending(h => h.DATE);
                    break;
            }

            // Default number of records per page
            int pageSize = 50;
            // If page not null, set page number to 1, else use page
            int pageNumber = (page ?? 1);
            return View(history.ToPagedList(pageNumber, pageSize));
        }

        // GET: HistoryEvent/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COILTABLEHISTORY historyEvent = db.History.Find(id);
            if (historyEvent == null)
            {
                return HttpNotFound();
            }
            return View(historyEvent);
        }

        // GET: HistoryEvent/Create
        public ActionResult Create(int coilID)
        {
            ViewBag.CoilID = coilID;
            return View();
        }

        // POST: HistoryEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "COILID,DATE,AMOUNTUSED,JOBNUMBER,NOTES")] COILTABLEHISTORY historyEvent)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Update coil's length
                    var coilToUpdate = db.Coils.Find(historyEvent.COILID);
                    if (coilToUpdate != null && historyEvent.AMOUNTUSED != null)
                    {
                        if ((int)historyEvent.AMOUNTUSED > coilToUpdate.LENGTH)
                        {
                            ModelState.AddModelError("AmountUsedGreaterThanLengthError", "Amount used cannot be greater than remaining coil length!");
                            return View(historyEvent);
                        }

                        coilToUpdate.LENGTH -= (int)historyEvent.AMOUNTUSED;
                    }

                    db.History.Add(historyEvent);

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                // Log the error (uncomment dex and add line here to write log)
                ModelState.AddModelError("", "Unable to create new history record. Make sure you're using a valid" +
                    " Coil ID if not autofilled. Try again, and report the issue in Contacts if the problem persists.");
            }

            return View(historyEvent);
        }

        // GET: HistoryEvent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COILTABLEHISTORY historyEvent = db.History.Find(id);
            if (historyEvent == null)
            {
                return HttpNotFound();
            }
            return View(historyEvent);
        }

        // POST: HistoryEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COILTABLEHISTORY historyEvent = db.History.Find(id);
            int oldAmountUsed = (int)historyEvent.AMOUNTUSED;
            if (TryUpdateModel(historyEvent, "",
               new string[] { "DATE", "AMOUNTUSED", "JOBNUMBER", "NOTES" }))
            {
                try
                {
                    // Update coil's length
                    var coilToUpdate = db.Coils.Find(historyEvent.COILID);
                    if (coilToUpdate != null && historyEvent.AMOUNTUSED != null)
                    {
                        if ((int)historyEvent.AMOUNTUSED > coilToUpdate.LENGTH + oldAmountUsed)
                        {
                            ModelState.AddModelError("AmountUsedGreaterThanLengthError", "Amount used cannot be greater than remaining coil length!");
                            return View(historyEvent);
                        }

                        coilToUpdate.LENGTH += oldAmountUsed;
                        coilToUpdate.LENGTH -= (int)historyEvent.AMOUNTUSED;
                    }

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    // Log the error (uncomment dex and add line here to write log
                    ModelState.AddModelError("", "Unable to edit history record. Try again," +
                        " and report the issue in Contacts if the problem persists.");
                }
            }
            return View(historyEvent);
        }

        // GET: HistoryEvent/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COILTABLEHISTORY historyEvent = db.History.Find(id);
            if (historyEvent == null)
            {
                return HttpNotFound();
            }
            return View(historyEvent);
        }

        // POST: HistoryEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            COILTABLEHISTORY historyEvent = db.History.Find(id);
            db.History.Remove(historyEvent);

            // Update coil's length
            var coilToUpdate = db.Coils.Find(historyEvent.COILID);
            if (coilToUpdate != null && historyEvent.AMOUNTUSED != null)
            {
                coilToUpdate.LENGTH += (int)historyEvent.AMOUNTUSED;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
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