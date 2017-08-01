using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DryStream.Models
{
    public class SongAlbumArtist
    {
        public Song Song { get; set; }
        public Album Album { get; set; }
        public Artist Artist { get; set; }
    }
}