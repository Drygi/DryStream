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
    public class PlaylistsSongsController : ApiController
    {
        private Entities db = new Entities();

        //// GET: api/PlaylistsSongs
        //public IQueryable<PlaylistsSong> GetPlaylistsSongs()
        //{
        //    return db.PlaylistsSongs;
        //}

        // GET: api/PlaylistsSongs/5
        [ResponseType(typeof(PlaylistsSong))]
        public IHttpActionResult GetPlaylistsSong(int id)
        {
            PlaylistsSong playlistsSong = db.PlaylistsSongs.Find(id);
            if (playlistsSong == null)
            {
                return NotFound();
            }

            return Json(playlistsSong);
        }

        // PUT: api/PlaylistsSongs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlaylistsSong(int id, PlaylistsSong playlistsSong)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playlistsSong.PlaylistsSongsID)
            {
                return BadRequest();
            }

            db.Entry(playlistsSong).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaylistsSongExists(id))
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

        // POST: api/PlaylistsSongs
        [ResponseType(typeof(PlaylistsSong))]
        public IHttpActionResult PostPlaylistsSong(PlaylistsSong playlistsSong)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PlaylistsSongs.Add(playlistsSong);
            db.SaveChanges();

            return Ok();
            //return CreatedAtRoute("DefaultApi", new { id = playlistsSong.PlaylistsSongsID }, playlistsSong);
        }

        // DELETE: api/PlaylistsSongs/5
        [ResponseType(typeof(PlaylistsSong))]
        public IHttpActionResult DeletePlaylistsSong(int id)
        {
            PlaylistsSong playlistsSong = db.PlaylistsSongs.Find(id);
            if (playlistsSong == null)
            {
                return NotFound();
            }

            db.PlaylistsSongs.Remove(playlistsSong);
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

        private bool PlaylistsSongExists(int id)
        {
            return db.PlaylistsSongs.Count(e => e.PlaylistsSongsID == id) > 0;
        }
    }
}