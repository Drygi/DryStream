using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DryStreamMobile.Models
{
    public class SongAlbumArtist
    {
        public Song Song { get; set; }
        public Album Album { get; set; }
        public Artist Artist { get; set; }
    }
}