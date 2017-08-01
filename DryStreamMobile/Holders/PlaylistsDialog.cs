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

namespace DryStreamMobile
{
    public class PlaylistsDialog : DialogFragment
    {
        private ListView listView;
        private Button button;
        private List<Playlist> playlists;
        public PlaylistsDialog(List<Playlist> playlists)
        {
            this.playlists = playlists;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            
            View view = inflater.Inflate(Resource.Layout.AddToPlaylistDialog,container,false);
            listView = view.FindViewById<ListView>(Resource.Id.listViewAddTo);
            button =  view.FindViewById<Button>(Resource.Id.newPlaylistButton);
            button.Click += Button_Click;
            this.Dialog.SetTitle("Dodaj do playlisty");
            listView.Adapter = new ArrayAdapter(this.Activity, Android.Resource.Layout.SimpleListItem1, playlists);
            listView.ItemClick += ListView_ItemClick;
            return view;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            Toast.MakeText(this.Activity, e.Id.ToString(), ToastLength.Long);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this.Activity, "Nowa", ToastLength.Long);
        }
    }
}