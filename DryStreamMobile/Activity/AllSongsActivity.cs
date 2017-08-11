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
using DryStreamMobile.Helper;

namespace DryStreamMobile.Activity
{
    [Activity(Label = "Utwory", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AllSongsActivity : Android.App.Activity
    {
        private ListView listView;
        private TextView textView;
        private List<SongAlbumArtist> SAAs;
        private List<Song> _songs;
        private SearchView searchView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AllSongs);
            initControls();

            // Create your application here
        }

        private void initControls()
        {
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            listView = FindViewById<ListView>(Resource.Id.LVAllSongsID);
            textView = FindViewById<TextView>(Resource.Id.infTxtAllSongs);
            textView.Visibility = Android.Views.ViewStates.Gone;
            SAAs = new List<SongAlbumArtist>();
            _songs = new List<Song>();

            listView.ItemClick += ListView_ItemClick;
            listView.ItemLongClick += ListView_ItemLongClick;
        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var FM = this.FragmentManager;
            var playlistDialog = new PlaylistsDialog(_songs[Convert.ToInt32(e.Position)].SongID);
            RunOnUiThread(() => {
                playlistDialog.Show(FM, "Playlists");
            });
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            GlobalMemory.actualSong = SAAs[Convert.ToInt16(e.Id)].Song;
            GlobalMemory.actualSong.Album = SAAs[Convert.ToInt16(e.Id)].Album;
            GlobalMemory.actualSong.Album.Artist = SAAs[Convert.ToInt16(e.Id)].Album.Artist;
            GlobalMemory.MusicFromPlaylist = false;

            StartActivity(typeof(PlayerActivity));
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.searchMenu, menu);

            var searchItem = menu.FindItem(Resource.Id.action_search);

            searchView = searchItem.ActionView.JavaCast<Android.Widget.SearchView>();

            searchView.QueryTextChange += async (sender, args) =>
            {
                if (args.NewText.Trim() != String.Empty)
                {
                    SAAs = await APIHelper.findSong(args.NewText.Trim());
                    if (SAAs.Count < 1)
                    {
                        listView.Adapter = null;
                        textView.Visibility = Android.Views.ViewStates.Visible;
                    }
                    else
                    {
                        //List<Song>
                        _songs = new List<Song>();
                        foreach (var item in SAAs)
                        {
                            item.Song.Album = item.Album;
                            item.Song.Album.Artist = item.Artist;
                            _songs.Add(item.Song);
                        }
                        listView.Visibility = ViewStates.Visible;
                        listView.Adapter = new SongAdapter(this, Resource.Layout.model, _songs);
                        textView.Visibility = Android.Views.ViewStates.Invisible;
                    }
                }
                else
                {
                    listView.Adapter = null;
                    textView.Visibility = Android.Views.ViewStates.Visible;
                }
            };
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}