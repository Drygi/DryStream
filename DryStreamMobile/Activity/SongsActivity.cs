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
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SongsActivity : Android.App.Activity
    {

        private Album album;
        private List<Song> songs;
        private ListView LV;
        private SongAdapter songAdapter;
        private SearchView searchView;
        private TextView textView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Songs);
            initControls();   
        }
        private async void initControls()
        {
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            LV = FindViewById<ListView>(Resource.Id.LVsongs_ID);
            textView = FindViewById<TextView>(Resource.Id.infoTxtSongs);
            textView.Visibility = Android.Views.ViewStates.Gone;

            songs = new List<Song>();
            album = GlobalMemory.Album;
            album.Artist = GlobalMemory.Artist;
            songs = await APIHelper.getSongs(album.AlbumID);
            songs.ForEach(s => s.Album = album);
            this.Title = album.Artist.Name.Trim() + " - " + album.Title.Trim();

            songAdapter = new SongAdapter(this, Resource.Layout.model, songs);
            LV.Adapter = songAdapter;
            LV.ItemClick += LV_ItemClick;
            LV.ItemLongClick += LV_ItemLongClick;

        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.searchMenu, menu);

            var searchItem = menu.FindItem(Resource.Id.action_search);

            searchView = searchItem.ActionView.JavaCast<Android.Widget.SearchView>();

            searchView.QueryTextChange += (sender, args) =>
            {
                if (args.NewText.Trim() != String.Empty)
                {

                    var _findSong = (from s in songs where s.Name.ToUpper().Contains(args.NewText.Trim().ToUpper()) select s).ToList();
                    if (_findSong.Count<1)
                    {
                        textView.Visibility = Android.Views.ViewStates.Visible;
                        LV.Adapter = null;
                    }
                    else
                    {
                        textView.Visibility = Android.Views.ViewStates.Gone;
                        LV.Adapter =  new SongAdapter(this, Resource.Layout.model, _findSong);
                       
                    }
                }
                else
                {
                    textView.Visibility = Android.Views.ViewStates.Gone;
                    LV.Adapter = new SongAdapter(this, Resource.Layout.model, songs);
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


        private void LV_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Toast.MakeText(this, "LONGclick", ToastLength.Short).Show();
        }

        private void LV_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            GlobalMemory.actualSong = songs[Convert.ToInt16(e.Id)];
            StartActivity(typeof(PlayerActivity));
        }
    }
}