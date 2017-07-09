using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DryStream.Models
{
    public class SongsAlbumsArtistsModel
    {
        public DbSet<Song> Songs{ get; set; }
        public DbSet<Album> Albums{ get; set; }
        public DbSet<Artist> Artists{ get; set; }
        public DbSet<Genre> Genres { get; set; }

    }
}