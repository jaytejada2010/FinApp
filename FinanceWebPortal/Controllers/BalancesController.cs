using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinanceWebPortal.Controllers
{
    public class BalancesController : Controller
    {
        // GET: Balances
        FinanceAppDBEntities f = new FinanceAppDBEntities();
        public ActionResult Index()
        {
            user u = (user)Session["curUser"];
            if (u == null)
            {
                return RedirectToAction("Index", "login");

            }
            var data1 = (from t in f.transactions select t).GetEnumerator();
            var data2 = (from t in f.transactions select t).GetEnumerator();
            ViewData["transacts1"] = data1;
            ViewData["transacts2"] = data2;
            return View();
        }
        public ActionResult AddItem(transaction t)
        {
            if (t.transactType.Equals("Income") || t.transactType.Equals("Expense"))
            {
                user u = (user)Session["curUser"];
                DateTime dt = DateTime.Now;

                byte[] bDate = new byte[20];
                bDate = BitConverter.GetBytes(dt.Ticks);
                t.transactDT = bDate;
                t.userID = u.userID;
                f.transactions.Add(t);
                f.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}