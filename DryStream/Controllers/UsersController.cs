using DryStream.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DryStream.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index(string sorting, User user, int? page)
        {
            Entities db = new Entities();
            ViewBag.SortedBy = sorting; 
            ViewBag.SortByLogin = sorting == null ? "LoginDESC" : "";
            ViewBag.SortByValidity = sorting == "ValidityDESC" ? "ValidityASC" : "ValidityDESC";

            var users = from i in db.Users select i;


            if (ModelState.IsValid)
            {
                if(user.Login !=null)
                {
                    users = from i in db.Users
                            where i.Login.Contains(user.Login)
                            select i;
                }
            }
            switch (sorting)
            {
                case "LoginDESC":
                    users = users.OrderByDescending(u => u.Login);
                    break;
                case "ValidityDESC":
                    
                    users = users.OrderByDescending(u => u.Validity.Year).ThenByDescending(u=>u.Validity.Month).ThenByDescending(u=>u.Validity.Day);
                    break;
                case "ValidityASC":
                    users = users.OrderBy(u => u.Validity.Year).ThenBy(u => u.Validity.Month).ThenBy(u => u.Validity.Day);
                    break;
                default:
                    users = users.OrderBy(u => u.Login);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1); 
            return View(users.ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        public ActionResult EditUser(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Entities db = new Entities();
                var user = (from u in db.Users where u.UserID == id select u).Single();
                return View(user);
            }
        }
        [HttpPost]
        public ActionResult EditUser(User user)
        {
            Entities db = new Entities();
       
            var u = (from m in db.Users where m.UserID == user.UserID select m).Single();
            var _user = u;
            if (user.Validity>u.Validity && user.Validity>DateTime.Now)
            {
                _user.Validity = user.Validity;
                _user.Access = true;
            }
            db.Entry(u).CurrentValues.SetValues(_user);
            db.SaveChanges();

            return View(_user);
        }

        [HttpGet]
        public ActionResult SendEmail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                Entities db = new Entities();
                var user = (from u in db.Users where u.UserID == id select u).Single();
                return View(user);
            }
        }
        [HttpPost]
        public ActionResult SendEmail(int userID, string messageEmail)
        {
            Entities db = new Entities();
            var user = (from u in db.Users where u.UserID == userID select u).Single();


            using (MailMessage msg = new MailMessage())
            {
                msg.From = new MailAddress("DryStream.Admnistration@gmail.com");
                msg.To.Add(user.Email);
                msg.Body = "Dzień dobry! <br/><br/>" + messageEmail+ "<br /><br />Pozdrawiam <br /> Administracja DryStream";
                msg.Subject = "DryStream Administracja";
                msg.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = "DryStream.Admnistration@gmail.com";
                    NetworkCred.Password = "@Dministration";
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(msg);
                }
            }



            return RedirectToAction("Index");

        }
    }
}