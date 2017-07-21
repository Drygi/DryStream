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
using Newtonsoft.Json;

namespace DryStreamMobile.Models
{
    public class PlaylistSong
    {
        public int PlaylistsSongsID { get; set;}
        public int SongID { get; set; }
        public int PlaylistID { get; set; }
        [JsonIgnore]
        public virtual Playlist Playlist { get; set; }
        [JsonIgnore]
        public virtual Song Song { get; set; }
    }
}