using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DealCompanyNew.Models;
using PagedList;
namespace DealCompanyNew.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        DealsCopmanyEntities db = new DealsCopmanyEntities();
        UsersVM usersVM = new UsersVM();
        // GET: Users
        public ActionResult Index(int? Page_No, string Sorting_Order)
        {


            ViewBag.DateTime_UTC = String.IsNullOrEmpty(Sorting_Order) ? "status" : "";
            ViewBag.Name = String.IsNullOrEmpty(Sorting_Order) ? "Name" : "";

            //  return View(db.Deals.ToList());
            var user = from x in db.Users select x;
            switch (Sorting_Order)
            {
                case "Name":
                    user = user.OrderByDescending(x => x.Name);
                    break;
                case "status":
                    user = user.OrderByDescending(x => x.Status);
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
                    user = user.OrderBy(x => x.Name);
                    break;
            }
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(user.ToPagedList(No_Of_Page, Size_Of_Page));

        }
        [HttpGet]
        public ActionResult Create()

        {

            return View(usersVM);
        }

        [HttpPost]
        public ActionResult Create(UsersVM usersVM, HttpPostedFileBase file)
        {
            User recorduser = db.Users.Where(x => x.Email == usersVM.Email && x.UserType == "Normal").FirstOrDefault();
            if (recorduser == null)
            {

                if (ModelState.IsValid)
                {
                    string fileName = "";
                    if (file != null)
                    {
                        fileName = file.FileName;
                        string fileNameFullPath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                        file.SaveAs(fileNameFullPath);
                    }

                    User user = new User();
                    user.Name = usersVM.Name;
                    user.Password = usersVM.Password;
                    user.Gender = usersVM.Gender;
                    user.Email = usersVM.Email.ToLower();
                    user.UserType = "Normal";
                    user.Status = usersVM.Status;
                    user.Phone = usersVM.Phone;
                    user.Date_Of_Birth = Convert.ToDateTime(usersVM.Date_Of_Birth);
                    user.DateTime_UTC = DateTime.UtcNow;
                    user.Server_DateTime = DateTime.Now;
                    //user.Update_DateTime_UTC = DateTime.Now.ToUniversalTime();
                    // user.Last_Login_DateTime_UTC = new DateTime(12 / 55 / 0001);
                    user.Image = fileName;
                    //UsersVM usersVM = new UsersVM();
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            ViewBag.status = "you already registration";
            return View(usersVM);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            User UserFromDataBase = db.Users.Where(x => x.ID == id).FirstOrDefault();
            usersVM.Name = UserFromDataBase.Name;
            usersVM.Email = UserFromDataBase.Email.ToLower();
            usersVM.Email = UserFromDataBase.Email;
            usersVM.Gender = UserFromDataBase.Gender;
            usersVM.Status = UserFromDataBase.Status;
            usersVM.Date_Of_Birth =Convert.ToString( UserFromDataBase.Date_Of_Birth);
            usersVM.Phone = UserFromDataBase.Phone;
            usersVM.Image = UserFromDataBase.Image;
            usersVM.Password = UserFromDataBase.Password;
            usersVM.Gender = UserFromDataBase.Gender;
            return View(usersVM);
        }
        [HttpPost]
        public ActionResult Edit(UsersVM model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                string filename = "";


                if (file != null)
                {
                    filename = file.FileName;
                    string fullpathfilname = Path.Combine(Server.MapPath("~/Content/Images"), filename);
                    file.SaveAs(fullpathfilname);

                }

                User UserFromDataBase = db.Users.Where(x => x.ID == model.ID).FirstOrDefault();

                UserFromDataBase.Name = model.Name;
                UserFromDataBase.Password = model.Password;
                UserFromDataBase.Gender = model.Gender;
                UserFromDataBase.Email =model.Email;
                //UserFromDataBase.UserType = "Admin";
                UserFromDataBase.Status = model.Status;
                UserFromDataBase.Phone = model.Phone;
                 UserFromDataBase.Date_Of_Birth =Convert.ToDateTime(model.Date_Of_Birth);
                
                // user.DateTime_UTC = DateTime.Now.ToUniversalTime();
                // user.Server_DateTime = DateTime.Now;
                UserFromDataBase.Update_DateTime_UTC = DateTime.Now.ToUniversalTime();
                //UserFromDataBase.Last_Login_DateTime_UTC = (DateTime?)Session["LoginTime"];
                UserFromDataBase.Image = filename;
                //UsersVM usersVM = new UsersVM();
                
                db.SaveChanges();
                User userInfo = (User)Session["UserInfo"];
                return RedirectToAction("Index");
                //if (userInfo.UserType == "Admin") {
                //    return RedirectToAction("Index");
                //}
                //else {
                //    return RedirectToAction("Information"); 
                //}


            }
                return View();
        }

        //delete on users
        public ActionResult Delete(int id)
        {
            try
            {
                User usreToDeleted = db.Users.Where(x => x.ID == id).FirstOrDefault();

              List<ClaimedDeal>claimedDeal  = db.ClaimedDeals.Where(x => x.User_ID == id).ToList();
                if (claimedDeal != null)
                {
                    db.ClaimedDeals.RemoveRange(claimedDeal);
                }
                db.Users.Remove(usreToDeleted);
                db.SaveChanges();
                return RedirectToAction("index");

              //  return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }










            ////////////////////////////
          //  User usreToDeleted = db.Users.Where(x => x.ID == id).FirstOrDefault();
            //db.Users.Remove(usreToDeleted);
            //db.SaveChanges();
            //return RedirectToAction("index");
        }

        //public ActionResult DeleteAllselected( IEnumerable<int> userDeleted)
        //{
        //   // db.Users.Where(x => userDeleted.Contains(x.ID)).ToList().ForEach(db.Users.RemoveRange);
        //    return View();
        //}

        //method to delete on user on more one
        public ActionResult DeleteSelected(string[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    //throw error
                    ModelState.AddModelError("", "No item selected to delete");
                    return RedirectToAction("Index");
                }

                //bind the task collection into list
                List<int> TaskIds = ids.Select(x => Int32.Parse(x)).ToList();

              //  var rec = db.ClaimedDeals.Where(x => x.User_ID == TaskIds.Count).ToList();

                //for (var i = 0; i < TaskIds.Count(); i++)
                //{
                //    var claimedDea = db.ClaimedDeals.Where(x => x.User_ID == TaskIds[i]);
                   
                //   db.ClaimedDeals.Remove(claimedDea.FirstOrDefault());
                //}
                //if (claimedDea != null)
                //{
                //    db.ClaimedDeals.RemoveRange(claimedDea);
                //}
                /////////////////////////////
                ///
                //for (var i = 0; i < TaskIds.Count(); i++)
                //{

                //    // List<ClaimedDeal> claimedDeal = db.ClaimedDeals.Where(x => x.User_ID == TaskIds[i]).ToList();
                //    var claimedDeal = db.ClaimedDeals.Where(x=>x.User_ID==TaskIds[i]); // db.ClaimedDeals.Where(x => x.User_ID == TaskIds[i]).ToList();
                //    //                                                                                //  if (claimedDeal > 1) { 
                //      if (claimedDeal != null)
                //       {
                //           db.ClaimedDeals.RemoveRange(claimedDeal);
                //     }
                //    //  //  db.ClaimedDeals.RemoveRange(claimedDeal);

                //}

                for (var i = 0; i < TaskIds.Count(); i++)
                {
                    var todo = db.Users.Find(TaskIds[i]);
                    //remove the record from the database
                    db.Users.Remove(todo);
                    //call save changes action otherwise the table will not be updated
                    db.SaveChanges();
                }

                //redirect to index view once record is deleted
                return RedirectToAction("Index");
            }
            catch
            {
                // return Json(new { status = "the user have more than Claimed" }, JsonRequestBehavior.AllowGet);
               // ViewBag.messageDelet = Json(new { status = "the user have more than Claimed" }, JsonRequestBehavior.AllowGet);
               // Session["msg"] = "the user have more than Claimed";
                  return RedirectToAction("error");
            }

            
        
        }

        [HttpGet]
        public ActionResult Login()
        {
            LoginVM login = new LoginVM();
            return View(login);
        }
        [HttpPost]
        public ActionResult Login(LoginVM model)
        {

            User userInfoAdmin = db.Users.Where(x => x.Email.ToLower() == model.Email.ToLower() && x.Password == model.Password && x.UserType == "Admin").FirstOrDefault();
            User userInfoNormal = db.Users.Where(x => x.Email.ToLower() == model.Email.ToLower() && x.Password == model.Password && x.UserType == "Normal").FirstOrDefault();
            if (userInfoAdmin != null)
            {
                Session["UserInfo"] = userInfoAdmin;
                userInfoAdmin.Last_Login_DateTime_UTC = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Information", "Users");
               // return RedirectToAction("info", "EmployeeAllowance");
            }
            else if (userInfoNormal != null)
            {


                Session["UserInfo"] = userInfoNormal;
                //  return RedirectToAction("info", "EmployeeAllowance");
                userInfoNormal.Last_Login_DateTime_UTC = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Information", "Users");
                
            }
            else
            {
                ViewBag.Errorlogon = "User Name or Passwprd Is Wrong ";
                return View(model);


            }

        }



        [HttpGet]
        public ActionResult Information()
        {

            var userInfo =(User) Session["UserInfo"];
            InformationVM model = new InformationVM();
            User user = new User();

            model.Name = userInfo.Name;
            model.Email = userInfo.Email;
            model.Password = userInfo.Password;
            model.UserType = userInfo.UserType;
            model.Phone = userInfo.Phone;
            model.Last_Login_DateTime_UTC =Convert.ToString( userInfo.Last_Login_DateTime_UTC);
            model.Update_DateTime_UTC =Convert.ToString( userInfo.Update_DateTime_UTC);
            model.DateTime_UTC = Convert.ToString(userInfo.DateTime_UTC);
            model.Date_Of_Birth = Convert.ToString(userInfo.Date_Of_Birth);
            model.Image = userInfo.Image;
            model.Server_DateTime = Convert.ToString(userInfo.Server_DateTime);
            model.Gender = userInfo.Gender;
            model.Status = userInfo.Status;
            model.Update_DateTime_UTC = Convert.ToString(userInfo.Update_DateTime_UTC);

            ////////// info from claimed
            ///
            /// 
            var claimed = db.ClaimedDeals.Where(x => x.User_ID == userInfo.ID).ToList();
            ViewBag.claCount = claimed;
            if (claimed.Count == 0)
            {
                //model.Amount =0;
                model.Total = 0;
            }
            else
            {
                
            // if( claimed)
               // {
                var totalClaimed = db.ClaimedDeals.Where(x => x.User_ID == userInfo.ID).Sum(x => x.Amount);


                    model.Total = totalClaimed;


                    model.Amount = db.ClaimedDeals.Where(x => x.User_ID == userInfo.ID).ToList();
                    ViewBag.amout = db.ClaimedDeals.Where(x => x.User_ID == userInfo.ID).ToList();
                    //foreach (var item in amount)
                    //{

                    //    model.Amount= item.Amount;

                    //    //model.AllowName = Allow.Allowance.AllowName;
                    //}
                    ///

               // }
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult Register()

        {

            return View(usersVM);
        }

        [HttpPost]
        public ActionResult Register(UsersVM usersVM, HttpPostedFileBase file)
        {
            User recorduser = db.Users.Where(x => x.Email.ToLower() == usersVM.Email.ToLower() && x.UserType == "Normal").FirstOrDefault();
            User userInfo = (User)Session["UserInfo"];
            userInfo.UserType = null;
            if (recorduser == null)
            {
               
                if (ModelState.IsValid)
                {
                    string fileName = "";
                    if (file != null)
                    {
                        fileName = file.FileName;
                        string fileNameFullPath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                        file.SaveAs(fileNameFullPath);
                    }
                   Session["UserInfo"] = (User)recorduser;
                    User user = new User();
                    user.Name = usersVM.Name;
                    user.Password = usersVM.Password;
                    user.Gender = usersVM.Gender;
                    user.Email = usersVM.Email;
                    user.UserType = "Normal";
                    user.Status = usersVM.Status;
                    user.Phone = usersVM.Phone;
                    user.Date_Of_Birth = Convert.ToDateTime(usersVM.Date_Of_Birth);
                    user.DateTime_UTC = DateTime.UtcNow;
                    user.Server_DateTime = DateTime.Now;
                    //user.Update_DateTime_UTC = DateTime.Now.ToUniversalTime();
                    // user.Last_Login_DateTime_UTC = new DateTime(12 / 55 / 0001);
                    user.Image = fileName;
                    //UsersVM usersVM = new UsersVM();
                    db.Users.Add(user);
                    db.SaveChanges();
                    Session["UserInfo"] = (User)recorduser;
                    return RedirectToAction("index");

                }
            }
            ViewBag.status = "you already registration";
            return View(usersVM);
        }







        [HttpGet]
        public ActionResult Logout()
        {

            Session["employeeinfo"] = null;
            return RedirectToAction("Login");

        }
        public ActionResult error()
        {
            return View();
        }

    }
}