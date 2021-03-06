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

namespace DryStream.Controllers.MobileControllers
{
    public class AlbumsController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/Albums
        public IHttpActionResult GetAlbums()
        {
            if (db.Albums.Count() < 0)
                return NotFound();
            else
                return Json(db.Albums);
        }
        // GET: api/FindAlbums/{name}
        [Route("api/FindAlbums/{name}"), HttpGet]
        public IHttpActionResult FindArtists(string name)
        {
            try
            {
                var albums = (from a in db.Albums where a.Title.ToUpper().Contains(name.ToUpper()) select a).ToList();
                return Json(albums);
            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        // GET: api/Albums/5
        [ResponseType(typeof(Album))]
        public IHttpActionResult GetAlbum(int id)
        {
            List<Album> albums = (from a in db.Albums where a.ArtistID == id select a).ToList();
            if (albums == null)
            {
                return NotFound();
            }

            return Json(albums);
        }

        /*
        // PUT: api/Albums/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAlbum(int id, Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != album.AlbumID)
            {
                return BadRequest();
            }

            db.Entry(album).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Albums
        [ResponseType(typeof(Album))]
        public IHttpActionResult PostAlbum(Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Albums.Add(album);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = album.AlbumID }, album);
        }

        // DELETE: api/Albums/5
        [ResponseType(typeof(Album))]
        public IHttpActionResult DeleteAlbum(int id)
        {
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return NotFound();
            }

            db.Albums.Remove(album);
            db.SaveChanges();

            return Ok(album);
        }
        */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlbumExists(int id)
        {
            return db.Albums.Count(e => e.AlbumID == id) > 0;
        }
    }
}