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
    public class Song
    {
        public int SongID { get; set; }
        public int AlbumID { get; set; }
        public int GenreID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public System.TimeSpan Duration { get; set; }
        public int AlbumPosition { get; set; }
        [JsonIgnore]
        public virtual Album Album { get; set; }
        [JsonIgnore]
        public virtual Genre Genre { get; set; }
        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlaylistSong> PlaylistsSongs { get; set; }
    }
}