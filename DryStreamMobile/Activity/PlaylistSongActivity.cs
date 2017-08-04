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
    [Activity(Label = "PlaylistSongActivity")]
    public class PlaylistSongActivity : Android.App.Activity
    {
        private ListView listView;
        private TextView textView;
        private Button playAll;
        private ProgressBar progressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PlaylistSongsPage);
            initControlos();

            // Create your application here
        }

        private void initControlos()
        {
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            listView = FindViewById<ListView>(Resource.Id.listViewPlaylistSongs);
            listView.ItemClick += ListView_ItemClick;
            listView.ItemLongClick += ListView_ItemLongClick;

            progressBar = FindViewById<ProgressBar>(Resource.Id.PlaylistSongProgressBar);
            progressBar.Visibility = ViewStates.Gone;

            textView = FindViewById<TextView>(Resource.Id.infoEmptyPlaylistSongs);
            textView.Visibility = ViewStates.Gone;

            playAll = FindViewById<Button>(Resource.Id.playyAllPlaylist);
            playAll.Click += PlayAll_Click;


            if (GlobalMemory.ActualPlaylistSongs.Count>0)
            {
               listView.Adapter = new SongAdapter(this, Resource.Layout.model,GlobalMemory.ActualPlaylistSongs);
               textView.Visibility = ViewStates.Gone;
            }
            else
            {
                listView.Visibility = ViewStates.Gone;
                textView.Visibility = ViewStates.Visible;
            }
            this.Title = "Playlista: " + GlobalMemory.ActualPlaylist.Name;

        }

        private void PlayAll_Click(object sender, EventArgs e)
        {
            GlobalMemory.MusicFromPlaylist = true;
            GlobalMemory.actualSong = GlobalMemory.ActualPlaylistSongs.First();
            StartActivity(typeof(PlayerActivity));
            //Toast.MakeText(this, "Play all", ToastLength.Short);
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            GlobalMemory.MusicFromPlaylist = true;
            GlobalMemory.actualSong = GlobalMemory.ActualPlaylistSongs[Convert.ToInt32(e.Position)];
            StartActivity(typeof(PlayerActivity));
            // Toast.MakeText(this, "Play all from this", ToastLength.Short);
        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Czy na pewno chcesz usunąć utwór "+ GlobalMemory.ActualPlaylistSongs[Convert.ToInt32(e.Position)].Name);
            alert.SetPositiveButton("TAK", async (senderAlert, args) =>
            {
                progressBar.Visibility = ViewStates.Visible;
                if (await APIHelper.DeleteSongFromPlaylist(GlobalMemory.ActualPlaylist.PlaylistsSongs.ToList()[Convert.ToInt32(e.Position)].PlaylistsSongsID))
                {
                    GlobalMemory.ActualPlaylistSongs.RemoveAt(Convert.ToInt32(e.Position));
                    listView.Adapter = listView.Adapter = new SongAdapter(this, Resource.Layout.model, GlobalMemory.ActualPlaylistSongs);
                    progressBar.Visibility = ViewStates.Gone;
                    Toast.MakeText(this, "Utwór został usunięty", ToastLength.Short);
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