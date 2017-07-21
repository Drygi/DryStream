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
    public class Album
    {
        public int AlbumID { get; set; }
        public int ArtistID { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string CoverLink { get; set; }

        [JsonIgnore]
        public virtual Artist Artist { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<Song> Songs { get; set; }
    }
}