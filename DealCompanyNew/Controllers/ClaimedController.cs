using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealCompanyNew.Models;
using PagedList;

namespace DealCompanyNew.Controllers
{
    public class ClaimedController : Controller
    {
        private DealsCopmanyEntities db = new DealsCopmanyEntities();
        // GET: Claimed
        public ActionResult Index(string Sorting_Order, string Search_Data, string Filter_Value, int? Page_No)
        {

            ViewBag.DateTime_UTC = String.IsNullOrEmpty(Sorting_Order) ? "DateTime_UTC" : "";
            ViewBag.Name = String.IsNullOrEmpty(Sorting_Order) ? "Name" : "";
            //ViewBag.SortingID = String.IsNullOrEmpty(Sorting_Order) ? "ID_Description" : "";
            // ViewBag.SortingID = Sorting_Order == "Date_Enroll" ? "Date_Description" : "Data";

            //ViewBag.SortingID = Sorting_Order == "ID";

            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;



            var Claimed = from Cal in db.ClaimedDeals  select Cal;
            // ViewBag.SortingDate = Sorting_Order == "Last_Name" ? "name_Description" : "";









            if (!String.IsNullOrEmpty(Search_Data))
            {
                int number;
                bool claimedId = Int32.TryParse(Search_Data,out number);
                if (claimedId)
                {
                    Claimed = Claimed.Where(Cal => Cal.User.ID==number);
                }
                else
                {

                    Claimed = Claimed.Where(Cal => Cal.User.Name.ToUpper().Contains(Search_Data.ToUpper() ));
                }
                   
            }


            switch (Sorting_Order)
            {
                case "Name":
                    Claimed = Claimed.OrderByDescending(x => x.User.Name);
                    break;
                case "DateTime_UTC":
                    Claimed = Claimed.OrderByDescending(x => x.DateTime_UTC);
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
                    Claimed = Claimed.OrderBy(x => x.User.Name);
                    break;
            }

            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(Claimed.ToPagedList(No_Of_Page, Size_Of_Page));

        }

        // GET: Claimed/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Claimed/Create
        [HttpGet]
        public ActionResult Create()
        {
            ClaimedVM model = new ClaimedVM();
            ViewBag.Users = new SelectList(db.Users, "ID", "Name");
            ViewBag.Deals = new SelectList(db.Deals, "ID", "Name");
            return View(model);
         
        }

        // POST: Claimed/Create
        [HttpPost]
        public ActionResult Create(ClaimedVM model)
        {
           try
            {
                ClaimedDeal claimed = new ClaimedDeal();
                claimed.User_ID = model.User_ID;
                claimed.Deal_ID = model.Deal_ID;
                claimed.Amount = model.Amount;
                claimed.Currency = model.Currency;
                claimed.DateTime_UTC = DateTime.UtcNow;
                claimed.Server_DateTime = DateTime.Now;
                db.ClaimedDeals.Add(claimed);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Users = new SelectList(db.Users, "ID", "Name");
                ViewBag.Deals = new SelectList(db.Deals, "ID", "Name");
                return View(model);
            }
        }

        // GET: Claimed/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Claimed/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Claimed/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Claimed/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
