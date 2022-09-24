using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FinanceWebPortal.Controllers
{
    public class loginController : Controller
    {
        FinanceAppDBEntities f = new FinanceAppDBEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Verify(user u)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(u.userpass));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            u.userpass = returnValue.ToString();

            user v;
            var users = (from p in f.users select p).GetEnumerator();
            while (users.MoveNext())
            {
                v = users.Current;
                v.username = v.username.Trim();
                v.userpass = v.userpass.Trim();
                if (u.username.Equals(v.username))
                {
                    if (v.userpass == u.userpass)
                    {
                        Session.Add("curUser", v);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult CreateUser(user u)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(u.userpass));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            u.userpass = returnValue.ToString();

            f.users.Add(u);
            f.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult LogOut(user u)
        {
            Session.RemoveAll();
            return RedirectToAction("Index");
        }
    }
}