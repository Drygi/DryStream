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
    [Activity(Label = "Artyści", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ArtistsActivity : Android.App.Activity
    {
        private ListView listView;
        private ArtistsAdapter artistsAdapter;
        private List<Artist> artists;
        private SearchView searchView;
        private TextView textView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Artists);
            initControls();

            // Create your application here
        }
        private void initControls()
        {
            GlobalMemory.MusicFromPlaylist = false;
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true); 
            listView = FindViewById<ListView>(Resource.Id.LVartistsPage);
            textView = FindViewById<TextView>(Resource.Id.infoTxtA);
            listView.ItemClick += ListView_ItemClick;
            textView.Visibility = Android.Views.ViewStates.Visible;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            GlobalMemory.Artist = artists[Convert.ToInt32(e.Id)];
            StartActivity(typeof(AlbumsActivity));

        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.searchMenu, menu);

            var searchItem = menu.FindItem(Resource.Id.action_search);

            searchView = searchItem.ActionView.JavaCast<Android.Widget.SearchView>();

            searchView.QueryTextChange += async (sender, args) =>
            {
                if(args.NewText.Trim()!=String.Empty)
                {
                    
                    artists = await APIHelper.findArtists(args.NewText.Trim());
                    if (artists.Count<1)
                    {
                        listView.Adapter = null;
                        textView.Visibility = Android.Views.ViewStates.Visible;
                    }
                    else
                    {
                        artistsAdapter = new ArtistsAdapter(this, Resource.Layout.artistsModel, artists);
                        listView.Adapter = artistsAdapter;
                        textView.Visibility = Android.Views.ViewStates.Gone;
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