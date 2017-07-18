using System;
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

namespace DryStream.Controllers.MobileControllers
{
    public class GenresController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/Genres
        public IHttpActionResult GetGenres()
        {
            if (db.Genres.Count() < 0)
                return NotFound();
            else
                return Json(db.Genres);
        }

        // GET: api/Genres/5
        [ResponseType(typeof(Genre))]
        public IHttpActionResult GetGenre(int id)
        {
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Json(genre);
        }

        //// PUT: api/Genres/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutGenre(int id, Genre genre)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != genre.GenreID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(genre).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GenreExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Genres
        //[ResponseType(typeof(Genre))]
        //public IHttpActionResult PostGenre(Genre genre)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Genres.Add(genre);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = genre.GenreID }, genre);
        //}

        //// DELETE: api/Genres/5
        //[ResponseType(typeof(Genre))]
        //public IHttpActionResult DeleteGenre(int id)
        //{
        //    Genre genre = db.Genres.Find(id);
        //    if (genre == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Genres.Remove(genre);
        //    db.SaveChanges();

        //    return Ok(genre);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GenreExists(int id)
        {
            return db.Genres.Count(e => e.GenreID == id) > 0;
        }
    }
}