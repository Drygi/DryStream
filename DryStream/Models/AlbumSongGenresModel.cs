using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DryStream.Models
{
    public class AlbumSongGenresModel
    {
        public Album Album{ get; set; }
        [Required]
        public Song song { get; set; }
        public DbSet<Genre> Genres { get; set; }

    }
}