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
using DryStreamMobile.Models;
using Plugin.MediaManager.Abstractions;

namespace DryStreamMobile.Helper
{
    public static class GlobalMemory
    {
        public static string serverAddressIP
        {
            get
            {
                return "http://192.168.100.7:51754";
            }
        }

        public static User _user{ get; set; }
        
        public static Artist Artist { get; set; }
        public static Album Album { get; set; }

        public static Song actualSong { get; set; }

        public static List<Genre> Genres { get; set; }

        public static Playlist ActualPlaylist { get; set; }
        public static List<Song>  ActualPlaylistSongs { get; set; }
        public static bool MusicFromPlaylist { get; set; }



    }
}