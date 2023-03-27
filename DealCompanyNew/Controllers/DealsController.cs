using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DealCompanyNew;
using DealCompanyNew.Models;
using PagedList;
namespace DealCompanyNew.Controllers
{
    public class DealsController : Controller
    {
        private DealsCopmanyEntities db = new DealsCopmanyEntities();

        // GET: Deals
        public ActionResult Index(int? Page_No, string Sorting_Order)
        {



            ViewBag.DateTime_UTC = String.IsNullOrEmpty(Sorting_Order) ? "status" : "";
            ViewBag.Name = String.IsNullOrEmpty(Sorting_Order) ? "Name" : "";

            //  return View(db.Deals.ToList());
            var deal = from del in db.Deals select del;
            switch (Sorting_Order)
            {
                case "Name":
                    deal = deal.OrderByDescending(x => x.Name);
                    break;
                case "status":
                    deal = deal.OrderByDescending(x => x.Status);
                    break;
                //case "ID_Description":
                //    Claimed = Claimed.OrderBy(Emp => Emp.Empid);
                //    break;
                //case "Date_Description":
                //    employees = employees.OrderByDescending(Emp => Emp.Empid);
                //    break;
                //case "ID":
                //    employees = employees.OrderBy(x => x.Empid);
                //    break;



                default:
                    deal = deal.OrderBy(x => x.Name);
                    break;
            }
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(deal.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        // GET: Deals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        // GET: Deals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Deals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Server_DateTime,DateTime_UTC,Name,Description,Status,Amount,Currency")] Deal deal)
        {
            if (ModelState.IsValid)
            {

                deal.Server_DateTime = DateTime.Now;
                deal.DateTime_UTC = DateTime.UtcNow;
                db.Deals.Add(deal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deal);
        }

        // GET: Deals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        // POST: Deals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Server_DateTime,DateTime_UTC,Update_DateTime_UTC,Name,Description,Status,Amount,Currency")] DealsVM deal)
        {
            if (ModelState.IsValid)
            {
                
                Deal updateDeal = db.Deals.Where(x => x.ID == deal.ID).FirstOrDefault();
                updateDeal.Update_DateTime_UTC=DateTime.Now;
                updateDeal.Name = deal.Name;
                updateDeal.Status = deal.Status;
                updateDeal.Currency = deal.Currency;
                updateDeal.Amount = deal.Amount;
                //deal.Update_DateTime_UTC = Convert.ToString(  DateTime.Now);
                db.Entry(updateDeal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deal);
        }

        // GET: Deals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        // POST: Deals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {
                Deal deal = db.Deals.Find(id);

                List<ClaimedDeal> claimedDeal = db.ClaimedDeals.Where(x => x.Deal_ID == id).ToList();
                if (claimedDeal != null)
                {
                    db.ClaimedDeals.RemoveRange(claimedDeal);
                }
                db.Deals.Remove(deal);
                db.SaveChanges();
                return RedirectToAction("index");

                //  return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }


          //  Deal deal = db.Deals.Find(id);
            //db.Deals.Remove(deal);
            //db.SaveChanges();
           
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
