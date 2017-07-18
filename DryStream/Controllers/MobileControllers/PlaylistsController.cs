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

namespace DryStream.Controllers
{
    public class PlaylistsController : ApiController
    {
        private Entities db = new Entities();

        //// GET: api/Playlists
        //public IHttpActionResult GetPlaylists()
        //{
        //    if (db.Playlists.Count() < 0)
        //        return NotFound();
        //    else
        //        return Json(db.Playlists);
        //}

        // GET: api/Playlists/5
        [ResponseType(typeof(Playlist))]
        public IHttpActionResult GetPlaylist(int id)
        {
            //pobranie wszystkich playlist danego usera
            List<Playlist> playlists = (from p in db.Playlists where p.UserID == id select p).ToList();
            if (playlists.Count<0)
            {
                return NotFound();
            }
            return Json(playlists);
        }

        // PUT: api/Playlists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlaylist(int id, Playlist playlist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playlist.PlaylistID)
            {
                return BadRequest();
            }

            db.Entry(playlist).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaylistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Playlists
        [ResponseType(typeof(Playlist))]
        public IHttpActionResult PostPlaylist(Playlist playlist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Playlists.Add(playlist);
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/Playlists/5
        [ResponseType(typeof(Playlist))]
        public IHttpActionResult DeletePlaylist(int id)
        {
            Playlist playlist = db.Playlists.Find(id);
            if (playlist == null)
            {
                return NotFound();
            }

            db.Playlists.Remove(playlist);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlaylistExists(int id)
        {
            return db.Playlists.Count(e => e.PlaylistID == id) > 0;
        }
    }
}