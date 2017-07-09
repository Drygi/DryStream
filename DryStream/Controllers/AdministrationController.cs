using DryStream.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DryStream.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
       
        // GET: Administration
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddGenre()
        { 
            Entities db = new Entities();
            return View(db.Genres);
        }
        [HttpPost]
        public ActionResult AddGenre(Genre genre)
        {
            Entities db = new Entities();
            if (!ModelState.IsValid)
            {
                if (db.Genres.Any(g => g.NAME.ToUpper() == genre.NAME.ToUpper()))
                {
                    return Json("Podany gatunek już istnieje w bazie");
                }
                return View("AddGenre", genre);
            }
            else
            {
                db.Genres.Add(genre);
                db.SaveChanges();
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult AddArtist()
        {
            Entities db = new Entities();
            return View(db.Artists);
        }
        [HttpPost]
        public ActionResult AddArtist(Artist artist, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                Entities db = new Entities();
                if (db.Artists.Any(a => a.Name.ToUpper() == artist.Name.ToUpper()))
                {
                    return Json("Podany artysta już istnieje w bazie" );
                }

                if (file!=null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Files/Covers/"),fileName);
                    file.SaveAs(path);

                    artist.CoverLink = Url.Content(("~/Files/Covers/") + fileName);
                    db.Artists.Add(artist);
                    db.SaveChanges();

                }
            }
            
            return View("Index");
        }

        [HttpGet]
        public ActionResult AddAlbum()
        {
            Entities db = new Entities();
          
            return View(db.Albums);
        }
        [HttpPost]
        public ActionResult AddAlbum(Album album, HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {
                Entities db = new Entities();
                if (db.Albums.Any(a => a.Title.ToUpper() == album.Title.ToUpper()))
                {
                    return Json("Podany album już istnieje w bazie");
                }

                if (file != null )
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Files/Covers/"), fileName);
                    file.SaveAs(path);

                    album.CoverLink = Url.Content(("~/Files/Covers/") + fileName);
                    album.ArtistID = Convert.ToInt16(Request.Form["artistName"]);
                    db.Albums.Add(album);
                    db.SaveChanges();
                }
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult AddSong()
        {
            Entities db = new Entities();
            SongsAlbumsArtistsModel SAA = new SongsAlbumsArtistsModel();
            SAA.Songs = db.Songs;
            SAA.Albums = db.Albums;
            SAA.Artists = db.Artists;
            SAA.Genres = db.Genres;
            return View(SAA);
        }
        [HttpPost]
        public ActionResult AddSong(Song song, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                Entities db = new Entities();
                if (db.Songs.Any(a => a.Name.ToUpper() == song.Name.ToUpper()))
                {
                    return Json("Podana piosenka już istnieje w bazie");
                }
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Files/Songs/"), fileName);
                    file.SaveAs(path);
                   
                    song.Link = Url.Content(("~/Files/Songs/") + fileName);
                    song.AlbumID = Convert.ToInt16(Request.Form["albumID"]);
                    song.GenreID = Convert.ToInt16(Request.Form["genreID"]);
                    //pobrać duration z wstawianego utworu
                    db.Songs.Add(song);
                    db.SaveChanges();
                }
            }
            return View("Index");



          //  return View();
        }

    }
}