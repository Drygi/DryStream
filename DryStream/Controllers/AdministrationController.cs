using DryStream.Models;
using NAudio.Wave;
using PagedList;
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
        public ActionResult Genres(string sorting, Genre genre, int ? page)
        {
            Entities db = new Entities();
            ViewBag.SortedBy = sorting;
            ViewBag.SortByGenre = sorting == null ? "GenreDESC" : "";


            var genres = from i in db.Genres select i;

          

            if (ModelState.IsValid)
            {
                if (genre.NAME != null)
                {
                    genres = from i in db.Genres
                            where i.NAME.Equals(genre.NAME)
                            select i;
                }
            }

            switch (sorting)
            {
                case "GenreDESC":
                    genres = genres.OrderByDescending(u => u.NAME);
                    break;
                default:
                    genres = genres.OrderBy(u => u.NAME);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(genres.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult Genres(Genre genre)
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
        public ActionResult DeleteGenre(int id)
        {
            Entities db = new Entities();

            var genre = (from d in db.Genres where d.GenreID == id select d).Single();
            db.Genres.Remove(genre);
            db.SaveChanges();
            
            return View("Index");
        }
        [HttpGet]
        public ActionResult Artists(string sorting, Artist artist, int? page)
        {
            Entities db = new Entities();

            ViewBag.SortedBy = sorting;
            ViewBag.SortByArtist = sorting == null ? "ArtistDESC" : "";


            var art = from i in db.Artists select i;

            if (ModelState.IsValid)
            {
                if (artist.Name!= null)
                {
                    art = from i in db.Artists
                             where i.Name.Equals(artist.Name)
                             select i;
                }
            }
            switch (sorting)
            {
                case "ArtistDESC":
                    art = art.OrderByDescending(a => a.Name);
                    break;
                default:
                    art = art.OrderBy(a => a.Name);
                    break;
            }

            //zmienic pageSize na wiecej jak bedzie wiecej artystów
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(art.ToPagedList(pageSize,pageNumber));
        }
        [HttpPost]
        public ActionResult Artists(Artist artist, HttpPostedFileBase file)
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
        public ActionResult Albums(string sorting, Album album,int? id, int? page)
        {
            Entities db = new Entities();
            ViewBag.SortedBy = sorting;
            ViewBag.SortByTitle = sorting == null ? "TitleDESC" : "";
            ViewBag.SortByYear = sorting == "YearDESC" ? "YearASC" : "YearDESC";

            var albums = from a in db.Albums where a.ArtistID == id select a;

            if (ModelState.IsValid)
            {
                if (album.Title != null)
                {
                    albums = from a in db.Albums
                          where a.Title.Equals(album.Title) && a.ArtistID ==id
                          select a;
                }
            }
            switch (sorting)
            {
                case "ArtistDESC":
                    albums = albums.OrderByDescending(a => a.Title);
                    break;
                case "YearDESC":
                    albums = albums.OrderByDescending(a => a.Year);
                    break;
                case "YearASC":
                    albums = albums.OrderBy(a => a.Year);
                    break;
                default:
                    albums = albums.OrderBy(a => a.Title);
                    break;
            }

            //zmienic pageSize na wiecej jak bedzie wiecej artystów
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(albums.ToPagedList(pageSize, pageNumber));

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
                    Mp3FileReader reader = new Mp3FileReader(path);
                    var duration = reader.TotalTime;

                    song.Link = Url.Content(("~/Files/Songs/") + fileName);
                    song.AlbumID = Convert.ToInt16(Request.Form["albumID"]);
                    song.GenreID = Convert.ToInt16(Request.Form["genreID"]);
                    song.Duration = duration;
                    //pobrać duration z wstawianego utworu
                    db.Songs.Add(song);
                    db.SaveChanges();
                }
            }
            return View("Index");



          //  return View();
        }

        [HttpGet]
        public ActionResult AlbumSongs(int ? id)
        {
            Entities db = new Entities();

            var album = from a in db.Songs where a.AlbumID == id select a;
            return View(album);
        }

        public ActionResult DeleteAlbum(int? id)
        {
            Entities db = new Entities();

            var album = (from a in db.Albums where a.AlbumID == id select a).Single();

            var songs = (from s in db.Songs where s.AlbumID == id select s).ToList();

            foreach (var item in songs)
            {
                db.Songs.Remove(item);
            }
            db.Albums.Remove(album);
            db.SaveChanges();
            return View("Administration/Artists");
        }

        public ActionResult DeleteSong(int? id)
        {
            Entities db = new Entities();
            var song = (from s in db.Songs where s.SongID == id select s).Single();
            db.Songs.Remove(song);
            db.SaveChanges();

            return View("Index");
        }


    }
}