using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using DryStreamMobile.Helper;
using DryStreamMobile.Models;

namespace DryStreamMobile.Activity
{
    [Activity(Label = "DryStream",ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainPageActivity : Android.App.Activity
    {

        private CustomAdapter customAdapter;
        private ListView lv;
        List<Song> songs;
        List<Album> albums;
        List<Artist> artists;

        private DrawerLayout mDrawerLayout;
        private ArrayAdapter mleftAdapter;
        private ListView mLeftDrawer;
        private ActionBarDrawerToggle mDrawerToggle;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainPage);
            initControls();
            // Create your application here
        }

        private async void initControls()
        {
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.myDrawer);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.leftListView);
            mDrawerToggle = new MyActionBarDrawerToggle(this, mDrawerLayout, Resource.Drawable.drawerIcon, Resource.String.open_drawer, Resource.String.close_drawer);
            mleftAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, GlobalHelper.menuList(GlobalMemory._user.Login));

            mLeftDrawer.Adapter = mleftAdapter;
            mLeftDrawer.ItemClick += MLeftDrawer_ItemClick;
            mDrawerLayout.SetDrawerListener(mDrawerToggle);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            ///menu glowne
            songs = await APIHelper.getSongs();
            albums = await APIHelper.getAlbums();
            artists = await APIHelper.getArtists();
            foreach (var item in songs)
            {
                item.Album = (from a in albums where item.AlbumID == a.AlbumID select a).Single();   
            }
            foreach (var item in albums)
            {
                item.Artist = (from a in artists where item.ArtistID == a.ArtistID select a).Single();
            }
            ///
            lv = FindViewById<ListView>(Resource.Id.LVmainPage);
            customAdapter = new CustomAdapter(this, Resource.Layout.model, songs);
            lv.Adapter = customAdapter;
            lv.ItemClick += Lv_ItemClick;

        }

        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, e.Id.ToString(), ToastLength.Long).Show();
        }

        private void MLeftDrawer_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            GlobalHelper.switchByIdFromList(Convert.ToInt16(e.Id), this);

        }





        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            mDrawerToggle.SyncState();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (mDrawerToggle.OnOptionsItemSelected(item))
            { return true;
            }
            return base.OnOptionsItemSelected(item);

        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            mDrawerToggle.OnConfigurationChanged(newConfig);
        }
    }
}