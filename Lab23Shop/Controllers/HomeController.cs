using Lab23Shop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab23Shop.Controllers
{
    public class HomeController : Controller
    {
        ShopDBEntities2 db = new ShopDBEntities2();

        public ActionResult Index()
        {
            //var user=Session["LoggedInUser"];
            //List<User> p = (List<User>)user;
            //User u = p[0];

            //ViewBag.User = u;

            User u = (User)Session["LoggedInUser"];
            ViewBag.User = u;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult MakeNewUser(User u)
        {
            db.Users.Add(u);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            List<User> Users = db.Users.ToList();
          
            foreach (User u in Users)
            {
                if (u.FirstName == UserName && u.Password == Password)
                {
                    Session["LoggedInUser"] = u; // if not empty in shop view
                    break;
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Shop()
        {
            List<Item> products= db.Items.ToList(); 
            return View(products);
        }
        public ActionResult Buy( int ID)
        {
            if (Session["LoggedInUser"] != null)
            {
                Item purchase = db.Items.Find(ID);
                User buyer = (User)Session["LoggedInUser"];

                if (buyer.Money < purchase.Price)
                {
                    Session["Error"] = $"{buyer.FirstName} cannot afford {purchase.ItemName} at ${purchase.Price}";
                    return RedirectToAction("Login");
                }
                else
                {
                    buyer.Money -= purchase.Price;
                    purchase.Quantity--;
                    db.Users.AddOrUpdate(buyer);
                    db.Items.AddOrUpdate(purchase);
                    db.SaveChanges();

                    //Session["UserFuns"] = buyer.Money;
                    //Session["Price"] = purchase.Price;
                   
                }
            }
            
            return RedirectToAction("Shop");
        }
        public ActionResult Error()
        {
            return View();
        }
    }

}