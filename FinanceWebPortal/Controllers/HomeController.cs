using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinanceWebPortal.Controllers
{
    public class HomeController : Controller
    {
        FinanceAppDBEntities f = new FinanceAppDBEntities();
        public ActionResult Index()
        {
            user u = (user)Session["curUser"];
            if ( u == null )
            {
                return RedirectToAction("Index", "login");
                
            }
            var data1 = (from t in f.transactions select t).GetEnumerator();
            var data2 = (from l in f.loans select l).GetEnumerator();

            ViewData["transacts"] = data1;
            ViewData["loans"] = data2;
            return View();
        }
    }
}