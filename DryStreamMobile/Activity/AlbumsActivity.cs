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
    [Activity(Label = "Albumy", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AlbumsActivity : Android.App.Activity
    {
        private ListView listView;
        private AlbumAdapter albumAdapter;
        private List<Album> albums;
        private Artist artist;
        private SearchView searchView;
        private TextView textView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Albums);
            initControls();
        }

        private async void initControls()
        {
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            textView = FindViewById<TextView>(Resource.Id.infoTxtAlbums);
            textView.Visibility = Android.Views.ViewStates.Gone;
            artist = GlobalMemory.Artist;
            albums = await APIHelper.getAlbums(artist.ArtistID);
            albums.ForEach(a => a.Artist = artist);
            this.Title = artist.Name;

            listView = FindViewById<ListView>(Resource.Id.LValbumsPage);
            albumAdapter = new AlbumAdapter(this, Resource.Layout.model, albums);
            listView.Adapter = albumAdapter;
            listView.ItemClick += ListView_ItemClick;
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

                    var _findAlbums = (from a in albums where a.Title.ToUpper().Contains(args.NewText.ToUpper().Trim()) select a).ToList();
                    if(_findAlbums.Count<1)
                    {
                        listView.Adapter = null;
                        textView.Visibility = Android.Views.ViewStates.Visible;
                    }
                    else
                    {
                        textView.Visibility = Android.Views.ViewStates.Gone;
                        listView.Adapter = new AlbumAdapter(this, Resource.Layout.model, _findAlbums);
                    }
                }
                else
                {
                    textView.Visibility = Android.Views.ViewStates.Gone;
                    listView.Adapter = new AlbumAdapter(this, Resource.Layout.model, albums);
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

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            GlobalMemory.Album = albums[Convert.ToInt32(e.Id)];
            StartActivity(typeof(SongsActivity));
        }
    }
}