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
    public class User
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string CoverLink { get; set; }
        public bool Access { get; set; }
        public System.DateTime Validity { get; set; }
        [JsonIgnore]
        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}