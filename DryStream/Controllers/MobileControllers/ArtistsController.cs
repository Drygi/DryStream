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
    public class ArtistsController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/Artists
        public IHttpActionResult GetArtists()
        {
            if (db.Artists.Count() < 0)
                return NotFound();
            else
                return Json(db.Artists);
        }

        // GET: api/FindArtists/{name}
        [Route("api/FindArtists/{name}"),HttpGet]
        public IHttpActionResult FindArtists (string name)
        {
            try
            {
                List<Artist> artists = (from u in db.Artists where u.Name.ToUpper().Contains(name.ToUpper()) select u).ToList();
                return Json(artists);
            }
            catch (Exception)
            {

                return NotFound();
            }
        }


        // GET: api/Artists/5
        [ResponseType(typeof(Artist))]
        public IHttpActionResult GetArtist(int id)
        {
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return NotFound();
            }
            return Json(artist);
        }

        //// PUT: api/Artists/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutArtist(int id, Artist artist)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != artist.ArtistID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(artist).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ArtistExists(id))
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

        //// POST: api/Artists
        //[ResponseType(typeof(Artist))]
        //public IHttpActionResult PostArtist(Artist artist)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Artists.Add(artist);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = artist.ArtistID }, artist);
        //}

        //// DELETE: api/Artists/5
        //[ResponseType(typeof(Artist))]
        //public IHttpActionResult DeleteArtist(int id)
        //{
        //    Artist artist = db.Artists.Find(id);
        //    if (artist == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Artists.Remove(artist);
        //    db.SaveChanges();

        //    return Ok(artist);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArtistExists(int id)
        {
            return db.Artists.Count(e => e.ArtistID == id) > 0;
        }
    }
}