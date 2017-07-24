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
            // walidacja nie do końca dzialala
            Entities db = new Entities();
            //  var genres = from i in db.Genres select i;
            //int pageSize = 3;
            // int pageNumber = 1;
            genre.NAME = genre.NAME.Trim();
            if (genre.NAME == null)
            {
                ViewBag.Error = "Nie podano nazwy";
                return RedirectToAction("Genres");
            }
            if (db.Genres.Any(g => g.NAME.ToUpper() == genre.NAME.ToUpper()))
            {
                ViewBag.Error = "Podany gatunek już istnieje w bazie";
                return RedirectToAction("Genres");
            }
            if (ModelState.IsValid)
            {
                db.Genres.Add(genre);
                db.SaveChanges();
                ViewBag.Succes = "Pomyślnie dodano gatunek " + genre.NAME + " do bazy";
                return RedirectToAction("Genres");
            }
            else
            {
                ViewBag.Error("Coś poszło nie tak");
                return RedirectToAction("Genres");
            }
        }
        public ActionResult DeleteGenre(int id)
        {
            Entities db = new Entities();

            var genre = (from d in db.Genres where d.GenreID == id select d).Single();
            db.Genres.Remove(genre);
            db.SaveChanges();
            var genres = from i in db.Genres select i;
            return RedirectToAction("Genres");
           // return View("AddGenre", genres.ToPagedList(1, 3));
        }
       
        public ActionResult Artists(string sorting,Artist artist, int? page)
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
                             where i.Name.Contains(artist.Name)
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

            
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(art.ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        public ActionResult AddArtist()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddArtist(Artist artist, HttpPostedFileBase file)
        {
            artist.Name=artist.Name.Trim();
            Entities db = new Entities();
            if (db.Artists.Any(a => a.Name.ToUpper() == artist.Name.ToUpper()))
            {
                ViewBag.Error=("Podany artysta już istnieje w bazie");
                return View("AddArtist", artist);
            }
            if (file == null)
            {
                ViewBag.Error = ("Brak okładki");
                return View("AddArtist", artist);
            }
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Files/Covers/"), fileName);
                file.SaveAs(path);

                artist.CoverLink = Url.Content(("~/Files/Covers/") + fileName);
                db.Artists.Add(artist);
                db.SaveChanges();
                ViewBag.Succes = artist.Name + " został pomyślnie dodany do bazy";
            }
            else
                ViewBag.Error = "Coś poszło nie tak";
;            return View("AddArtist", artist);
        }
        
        public ActionResult AddAlbum(int ? id)
        {
            Entities db = new Entities();
         
            if (id!=null)
            {
                Album album = new Album();
                album.Artist = (from a in db.Artists where a.ArtistID == id select a).Single();
                album.ArtistID = Convert.ToInt32(id);
                return View(album);
            }
            return View();
     
        }

        [HttpPost]
        public ActionResult AddAlbum(Album album, HttpPostedFileBase file)
        {
            Entities db = new Entities();

            if (file==null)
            {
                ViewBag.Error = "Nie wybrano pliku";
                album.Artist = (from a in db.Artists where a.ArtistID == album.ArtistID select a).Single();
                return View("AddAlbum", album);
            }
            if (album.Title == null || album.Year == null)
            {
                ViewBag.Error = "Nie wypełniono wszystkich pól";
                album.Artist = (from a in db.Artists where a.ArtistID == album.ArtistID select a).Single();
                return View("AddAlbum", album);
            }

            if (ModelState.IsValid)
            {
                if (db.Albums.Any(a => a.Title.ToUpper() == album.Title.ToUpper()))
                {
                    ViewBag.Error = "Podany album już istnieje w bazie";
                    album.Artist = (from a in db.Artists where a.ArtistID == album.ArtistID select a).Single();
                    return View("AddAlbum", album);
                }
                
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Files/Covers/"), fileName);
                file.SaveAs(path);

                album.CoverLink = Url.Content(("~/Files/Covers/") + fileName);
                    
                db.Albums.Add(album);
                db.SaveChanges();
                ViewBag.Success = "Album " + album.Title +" pomyślnie dodano do bazy";
                album.Artist = (from a in db.Artists where a.ArtistID == album.ArtistID select a).Single();
                return View("AddAlbum",album);
            }
            else
            {
                album.Artist = (from a in db.Artists where a.ArtistID == album.ArtistID select a).Single();
                return View("AddAlbum",album); 
            }
           
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
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(albums.ToPagedList(pageNumber,pageSize));

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
            var cover = new FileInfo(Path.Combine(Server.MapPath("~"+album.CoverLink)));
            var ID = album.ArtistID;
            cover.Delete();
            foreach (var item in songs)
            {
                var fileSong = new FileInfo(Path.Combine(Server.MapPath("~" + item.Link)));
                fileSong.Delete();
                db.Songs.Remove(item);
            }
            db.Albums.Remove(album);
            db.SaveChanges();
            return RedirectToAction("Albums/" + ID);
           
        }
      
        public ActionResult AddSong(int ?id )
        {
            if (id!=null)
            {
                Entities db = new Entities();
                AlbumSongGenresModel ASG = new AlbumSongGenresModel();
                ASG.Genres = db.Genres;
                ASG.Album = (from a in db.Albums where a.AlbumID == id select a).Single();
                return View(ASG);
            }
            return View(id);
        }
        [HttpPost]
        public ActionResult AddSong(Song song, HttpPostedFileBase file)
        {
            Entities db = new Entities();
            AlbumSongGenresModel ASG = new AlbumSongGenresModel();
            ASG.Genres = db.Genres;
            ASG.Album = (from a in db.Albums where a.AlbumID == song.AlbumID select a).Single();
            ASG.song = song;

            if (file == null)
            {
                ViewBag.Error = "Nie wybrano pliku";
                return View("AddSong", ASG);
            }

            if (
                db.Songs.Any(a => a.Name.ToUpper() == song.Name.ToUpper() ||
                db.Songs.Any(s=> s.AlbumPosition == song.AlbumPosition ))
                )
            {
                ViewBag.Error = ("Piosenka o podanym tytule lub pozycji istnieje w bazie");
                return View("AddSong", ASG);
            }
            if (song.Name == null)
            {
                ViewBag.Error = ("Nie podano imienia");
                return View("AddSong", ASG);
            }
            if ((song.AlbumPosition <= 0 || song.AlbumPosition > 30))
            {
                ViewBag.Error = ("Zła pozycja piosenki");
                return View("AddSong", ASG);
            }
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Files/Songs/"), fileName);
                file.SaveAs(path);
                Mp3FileReader reader = new Mp3FileReader(path);
                var duration = reader.TotalTime;

                song.Link = Url.Content(("~/Files/Songs/") + fileName);
                song.Duration = duration;
                //pobrać duration z wstawianego utworu
                db.Songs.Add(song);
                db.SaveChanges();
                ViewBag.Success = "Piosenka " + song.Name + "została pomyślnie dodana do bazy";
               
            }
            return View("AddSong", ASG);
        }
        public ActionResult DeleteSong(int? id)
        {
            Entities db = new Entities();
            var song = (from s in db.Songs where s.SongID == id select s).Single();
            var fSong = new FileInfo(Path.Combine(Server.MapPath("~" + song.Link)));
            var ID = song.AlbumID; 

            fSong.Delete();
            db.Songs.Remove(song);
            db.SaveChanges();
            
            return View("AlbumSongs",ID);
        }




    }
}