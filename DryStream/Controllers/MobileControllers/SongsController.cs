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
    public class SongsController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/Songs
        public IHttpActionResult GetSongs()
        {
            try
            {
                List<SongAlbumArtist> SAAs = new List<SongAlbumArtist>();
              //  var songs = (from u in db.Songs where u.Name.ToUpper().Contains(name.ToUpper()) select u).ToList();
                foreach (var item in db.Songs)
                {
                    SAAs.Add(
                        new SongAlbumArtist
                        {
                            Song = item,
                            Album = item.Album,
                            Artist = item.Album.Artist
                        });
                }
                return Json(SAAs);
            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        // GET: api/Songs/5
        [ResponseType(typeof(Song))]
        public IHttpActionResult GetSong(int id)
        {
            List<Song> songs = (from a in db.Songs where a.AlbumID == id select a).ToList();
            if (songs == null)
            {
                return NotFound();
            }

            return Json(songs);
        }

        // GET: api/FindSongs/{name}
        [Route("api/FindSongs/{name}"), HttpGet]
        public IHttpActionResult FindArtists(string name)
        {
            try
            {
                List<SongAlbumArtist> SAAs = new List<SongAlbumArtist>();
                var songs = (from u in db.Songs where u.Name.ToUpper().Contains(name.ToUpper()) select u).ToList();
                foreach (var item in songs)
                {
                    SAAs.Add(
                        new SongAlbumArtist
                        {
                            Song = item,
                            Album = item.Album,
                            Artist = item.Album.Artist
                        });
                }
                return Json(SAAs);
            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        //// PUT: api/Songs/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutSong(int id, Song song)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != song.SongID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(song).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SongExists(id))
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

        //// POST: api/Songs
        //[ResponseType(typeof(Song))]
        //public IHttpActionResult PostSong(Song song)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Songs.Add(song);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = song.SongID }, song);
        //}

        //// DELETE: api/Songs/5
        //[ResponseType(typeof(Song))]
        //public IHttpActionResult DeleteSong(int id)
        //{
        //    Song song = db.Songs.Find(id);
        //    if (song == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Songs.Remove(song);
        //    db.SaveChanges();

        //    return Ok(song);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SongExists(int id)
        {
            return db.Songs.Count(e => e.SongID == id) > 0;
        }
    }
}