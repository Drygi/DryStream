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
    [Activity(Label = "SongActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SongsActivity : Android.App.Activity
    {

        Album album;
        List<Song> songs;
        ListView LV;
        SongAdapter songAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Songs);
            initControls();
            
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


        private async void initControls()
        {
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            LV = FindViewById<ListView>(Resource.Id.LVsongs_ID);
            
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