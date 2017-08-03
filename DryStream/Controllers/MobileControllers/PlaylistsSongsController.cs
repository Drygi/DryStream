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
            List<PlaylistsSong> playlistsSong = (from p in db.PlaylistsSongs where p.PlaylistID == id select p).ToList();
               
            if (playlistsSong.Count <1)
            {
                return NotFound();
            }

            return Json(playlistsSong);
        }

        // GET: api/SongsFromPlaylists/{id}
        [Route("api/SongsFromPlaylists/{id}"), HttpGet]
        public IHttpActionResult getSongsFromPlaylist(int id)
        {
            List<SongAlbumArtist> SAAs = new List<SongAlbumArtist>();
            try
            {
                List<Song> songs = (from S in db.PlaylistsSongs where S.PlaylistID == id select S.Song).ToList();
                foreach (var item in songs)
                {
                    SAAs.Add(
                        new SongAlbumArtist
                        {
                            Song = item,
                            Album = item.Album,
                            Artist = item.Album.Artist
                        }
                        );
                }

                //   User _user = (from u in db.Users where u.Email == email select u).Single();
                return Json(SAAs);
            }
            catch (Exception)
            {

                return NotFound();
            }
        }


        //// PUT: api/PlaylistsSongs/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutPlaylistsSong(int id, PlaylistsSong playlistsSong)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != playlistsSong.PlaylistsSongsID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(playlistsSong).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PlaylistsSongExists(id))
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

        // POST: api/PlaylistsSongs
        [ResponseType(typeof(PlaylistsSong))]
        public IHttpActionResult PostPlaylistsSong(PlaylistsSong playlistsSong)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var find = (from P in db.PlaylistsSongs where P.PlaylistID == playlistsSong.PlaylistID &&
                        P.SongID == playlistsSong.SongID select P).ToList();
            if (find.Count>0)
            {
                return Ok();
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