using Computer_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Web.Razor.Generator;
using System.Text;
using System.Diagnostics;
namespace Computer_Store.Controllers
{
    public class HomeController : Controller
    {

        private ComputerStoreEntities db = new ComputerStoreEntities();
        //GET: ThumbNail
        public ActionResult ThumbNail()
        {
            return View();
        }
        // GET: Home
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        //GET: Register
        public ActionResult Register()
        {
            ViewBag.Breadcrumb = new List<string> { "Home", "Register" };
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    if (string.IsNullOrEmpty(_user.Password))
                    {
                        ViewBag.error = "Password cannot be empty!";
                        return View();
                    }
                    _user.Password = GetMD5(_user.Password);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(_user); 
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email đã tồn tại!";
                    return View();
                }
            }
            return View();
        }
        //Login
        public ActionResult Login()
        {
            ViewBag.Breadcrumb = new List<string> { "Home", "Login" };
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(user.Password))
                {
                    ViewBag.error = "Password cannot be empty";
                    return View();
                }

                var f_password = GetMD5(user.Password);
                Debug.WriteLine("Input Email: " + user.Email);
                Debug.WriteLine("Input Password: " + user.Password);


                var data = db.Users.Where(s => s.Email.Equals(user.Email) && s.Password.Equals(f_password)).ToList();
                
                Debug.WriteLine("Query count: " + data.Count);

                if (data.Count > 0)
                {
                    var loggedInUser = data.FirstOrDefault();
                    Session["UserID"] = loggedInUser.UserID;
                    Session["FullName"] = loggedInUser.FullName;
                    Session["UserName"] = loggedInUser.Username;
                    Session["Email"] = loggedInUser.Email;
                    Session["Role"] = loggedInUser.Role;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Đăng nhập thất bại";
                    return View();
                }
            }

            
            return View();
        }



        //Logout
        public ActionResult Logout() 
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        //MD5
        public static string GetMD5(string password) { 
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(password);
            byte[] target = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < target.Length; i++)
            {
                byte2String += target[i].ToString("x2");
            }
            return byte2String;
        }
    } 
}