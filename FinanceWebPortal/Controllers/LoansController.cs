using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinanceWebPortal.Controllers
{
    public class LoansController : Controller
    {
        // GET: Loans
        FinanceAppDBEntities f = new FinanceAppDBEntities();
        public ActionResult Index()
        {
            user u = (user)Session["curUser"];
            if (u == null)
            {
                return RedirectToAction("Index", "login");

            }
            var data1 = (from l in f.loans select l).GetEnumerator();
            var data2 = (from t in f.transactions select t).GetEnumerator();
            ViewData["transacts"] = data1;
            ViewData["loans"] = data2;
            return View();
        }
        public ActionResult CreateLoan(loan l)
        {
            user u = (user)Session["curUser"];

            string logString = DateTime.Now.ToString("yyyyMMddHHmmss");
            byte[] logEntry;
            int elements = logString.Length / 2;
            logEntry = new byte[elements];

            for (int i = 0; i < elements; i++)
            {
                logEntry[i] = Convert.ToByte(logString.Substring(i * 2, 2));
            }
            l.loanDT = logEntry;
            l.loanInterest /= 100;
            l.loanAmountPaid = 0;
            l.userID = u.userID;
            

            f.loans.Add(l);

            transaction t = new transaction();
            t.transactDT = logEntry;
            t.transactName = "Loaned from " + l.loanCreditor;
            t.transactType = "Income";
            t.transactAmount = l.loanPrincipal;
            t.userID = u.userID;
            f.transactions.Add(t);

            f.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult PayLoan(loan l)
        {
            user u = (user)Session["curUser"];
            loan z;

            z = f.loans.Where<loan>(b => b.loanID.Equals(l.loanID)).First();
            z.loanAmountPaid += l.loanAmountPaid;
            z.loanTerm = (z.loanAmountPaid == z.loanTotal) ? 0 : z.loanTerm - 1;

            transaction t = new transaction();
            string logString = DateTime.Now.ToString("yyyyMMddHHmmss");
            byte[] logEntry;
            int elements = logString.Length / 2;
            logEntry = new byte[elements];

            for (int i = 0; i < elements; i++)
            {
                logEntry[i] = Convert.ToByte(logString.Substring(i * 2, 2));
            }

            t.transactDT = logEntry;
            t.transactName = "Paid loan from " + z.loanCreditor;
            t.transactType = "Expense";
            t.transactAmount = l.loanAmountPaid;
            t.userID = u.userID;
            f.transactions.Add(t);

            f.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}