﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DryStream.Models;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Data.Entity.Validation;

namespace DryStream.Controllers
{
    [AllowAnonymous]
    public class MobileUsersController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/MobileUsers
        public IHttpActionResult GetUsers()
        {
            if (db.Users.Count() < 1)
                return NotFound();
            else
                return Json(db.Users.ToList());
        }

        // GET: api/MobileUsers/{login}
        [Route("api/MobileUsers/{login}")]
        public IHttpActionResult GetUser(string login)
        {
            try
            {
                User _user = (from u in db.Users where u.Login == login select u).Single();
                return Ok();
            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        // GET: api/FindEmail/{email}
        [Route("api/FindEmail/{login}")]
        public IHttpActionResult FindEmail(string email)
        {
            try
            {
                User _user = (from u in db.Users where u.Email == email select u).Single();
                return Ok();
            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        // POST: api/LoginUser
        [Route("api/LoginUser")]
        public IHttpActionResult LoginUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                User _user = (from u in db.Users where u.Login == user.Login && user.Password == u.Password select u).Single();
                return Json(_user); 
            }
            catch (Exception)
            {

                return NotFound();
            }
          
            
       

          
        }

        // PUT: api/MobileUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // POST: api/MobileUsers
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            db.Users.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        //POST: api/Files/Upload
        [Route("api/Files/Upload")]
        public async Task<IHttpActionResult>PostPhoto()
        {
            User user = new User();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string item in  httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[item];
                        var fileName = postedFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                       
                        var filePath = HttpContext.Current.Server.MapPath("~/Files/Covers/" + fileName);
                       
                        postedFile.SaveAs(filePath);
                        user.CoverLink = "/Files/Covers/" + fileName;
                        return Json(user);
                    }
                }
            }
            catch (Exception exception)
            {

                return NotFound();
            }
            return NotFound();

        }
        // DELETE: api/MobileUsers/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }
    }
}