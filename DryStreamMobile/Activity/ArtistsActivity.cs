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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Artists);
            initControls();

            // Create your application here
        }
        private async void initControls()
        {
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            listView = FindViewById<ListView>(Resource.Id.LVartistsPage);
            artists = await APIHelper.getArtists();
            artistsAdapter = new ArtistsAdapter(this, Resource.Layout.artistsModel,artists);
            listView.Adapter = artistsAdapter;
            listView.ItemClick += ListView_ItemClick;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            GlobalMemory.Artist = artists[Convert.ToInt32(e.Id)];
            StartActivity(typeof(AlbumsActivity));
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