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
using Plugin.MediaManager;
using Android.Views.InputMethods;

namespace DryStreamMobile.Activity
{
    [Activity(Label = "DryStream",ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainPageActivity : Android.App.Activity
    {

        List<Song> _songs = new List<Song>();
        private ListView lv;
        private DrawerLayout mDrawerLayout;
        private ArrayAdapter mleftAdapter;
        private ListView mLeftDrawer;
        private ActionBarDrawerToggle mDrawerToggle;
        private SearchView searchView;
        private TextView textView;
        private List<SongAlbumArtist> SAAs = new List<SongAlbumArtist>();
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
            lv = FindViewById<ListView>(Resource.Id.LVmainPage);
            textView = FindViewById<TextView>(Resource.Id.infoTxtMP);
            
            mDrawerToggle = new MyActionBarDrawerToggle(this, mDrawerLayout, Resource.Drawable.drawerIcon, Resource.String.open_drawer, Resource.String.close_drawer);
            mleftAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, GlobalHelper.menuList(GlobalMemory._user.Login));

            mLeftDrawer.Adapter = mleftAdapter;
            mLeftDrawer.ItemClick += MLeftDrawer_ItemClick;
            mDrawerLayout.SetDrawerListener(mDrawerToggle);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            lv.ItemClick += Lv_ItemClick;
            if (await APIHelper.GetPlaylists())
            {
                foreach (var item in GlobalMemory._user.Playlists)
                {
                    item.PlaylistsSongs = await APIHelper.GetPlaylistsSong(item.PlaylistID);
                }
            }
            lv.ItemLongClick += Lv_ItemLongClick;
        }

        private void Lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var FM = this.FragmentManager;
            var playlistDialog = new PlaylistsDialog(_songs[Convert.ToInt32(e.Position)].SongID);
            RunOnUiThread(() => {
                playlistDialog.Show(FM, "Playlists");
            });
        }

        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
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
                mDrawerLayout.CloseDrawer(mLeftDrawer);
                if (args.NewText != String.Empty)
                {
                    SAAs = await APIHelper.findSong(args.NewText.Trim());
                    if (SAAs.Count<1)
                    {
                        lv.Adapter = null;
                        _songs = null;
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
                        lv.Adapter = new SongAdapter(this, Resource.Layout.model, _songs);
                        textView.Visibility = Android.Views.ViewStates.Invisible;
                    }
                }
                else
                {
                    lv.Adapter = null;
                    _songs = null;
                    textView.Visibility = Android.Views.ViewStates.Visible;
                }
            };
            return base.OnCreateOptionsMenu(menu);
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
            hideKeyboard();
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
        private void hideKeyboard()
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(mLeftDrawer.WindowToken, 0);
        }    
    }
}