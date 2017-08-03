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
using DryStreamMobile.Helper;
using DryStreamMobile.Models;

namespace DryStreamMobile.Activity
{
    [Activity(Label = "Playlisty", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PlaylistsActivity : Android.App.Activity
    {
        ListView listView;
        TextView textView;
        ProgressBar progressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PlaylistsPage);
            initControls();
            // Create your application here
        }

        private void initControls()
        {
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            listView = FindViewById<ListView>(Resource.Id.playlistsListView);
            textView = FindViewById<TextView>(Resource.Id.emptyPlayliststLV);
            progressBar = FindViewById<ProgressBar>(Resource.Id.PlaylistProgressBar);
            progressBar.Visibility = ViewStates.Gone;
            if (GlobalMemory._user.Playlists==null)
            {
                listView.Visibility = ViewStates.Gone;
            }
            else
            {
                textView.Visibility = ViewStates.Gone;  
                listView.Adapter = new ArtistsAdapter(this, Resource.Layout.artistsModel, GlobalMemory._user.Playlists.ToList());

                listView.ItemClick += ListView_ItemClick;
                listView.ItemLongClick += ListView_ItemLongClick;
            }
        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Czy na pewno chcesz usunąć playlistę "+ GlobalMemory._user.Playlists.ToList()[Convert.ToInt32(e.Position)].Name);
            alert.SetPositiveButton("TAK", async (senderAlert, args) =>
            {
                progressBar.Visibility = ViewStates.Visible;
                if (await APIHelper.DeletePlaylist(GlobalMemory._user.Playlists.ToList()[Convert.ToInt32(e.Position)].PlaylistID))
                {
                    await APIHelper.GetPlaylists();
                   
                    listView.Adapter = new ArtistsAdapter(this, Resource.Layout.artistsModel, GlobalMemory._user.Playlists.ToList());
                    progressBar.Visibility = ViewStates.Gone;
                    Toast.MakeText(this, "Playlista została usunięta", ToastLength.Short);
                }
                else
                {
                    Toast.MakeText(this, "Coś poszło nie tak", ToastLength.Short);
                    progressBar.Visibility = ViewStates.Gone;
                }
            });
            alert.SetNegativeButton("NIE", (senderAlert, args) => {
                Toast.MakeText(this, "Anulowano", ToastLength.Long).Show();
            });
            RunOnUiThread(() => {
                alert.Show();
            });
        }

        private async void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var SSAs = await APIHelper.GetSongFromPlaylists(GlobalMemory._user.Playlists.ToList()[Convert.ToInt32(e.Position)].PlaylistID);
            GlobalMemory.ActualPlaylist = GlobalMemory._user.Playlists.ToList()[Convert.ToInt32(e.Position)];
            var songs = new List<Song>();
            foreach (var item in SSAs)
            {
                Song song = item.Song;
                song.Album = item.Album;
                song.Album.Artist = item.Artist;
                songs.Add(song);
            }
            GlobalMemory.ActualPlaylistSongs = songs;
            StartActivity(typeof(PlaylistSongActivity));
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