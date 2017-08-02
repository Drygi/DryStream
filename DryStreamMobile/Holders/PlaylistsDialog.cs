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
using DryStreamMobile.Holders;
using DryStreamMobile.Helper;

namespace DryStreamMobile
{
    public class PlaylistsDialog : DialogFragment
    {
        private TextView textview;
        private ListView listView;
        private Button button;
        public PlaylistsDialog()
        {
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.AddToPlaylistDialog,container,false);
            textview = view.FindViewById<TextView>(Resource.Id.emptyLVAddPL);
            textview.Visibility = ViewStates.Gone;
            listView = view.FindViewById<ListView>(Resource.Id.listViewAddTo);
            button = view.FindViewById<Button>(Resource.Id.addToPlaylistButton);
            button.Click += Button_Click;
            this.Dialog.SetTitle("Dodaj do playlisty");
            if (GlobalMemory._user.Playlists == null)
            {
                textview.Visibility = ViewStates.Visible;
                listView.Visibility = ViewStates.Gone;
            }
            else
            {
                listView.Adapter = new ArrayAdapter(this.Activity, Android.Resource.Layout.SimpleListItem1, GlobalMemory._user.Playlists.ToList());
                textview.Visibility = ViewStates.Gone;
                listView.Visibility = ViewStates.Visible;
            }
        listView.ItemClick += ListView_ItemClick;
            return view;
        }

        private void Button_Click(object sender, EventArgs e)
        {
           
            var FM = this.FragmentManager;
            var newPlaylist = new NewPlaylistDialog();
            this.Dismiss();
            newPlaylist.Show(FM, "newPlaylist");

           // Toast.MakeText(this.Activity, "CLICK", ToastLength.Long);
        }

        private async void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var p = GlobalMemory._user.Playlists.ToList();
            
            PlaylistSong PS = new PlaylistSong
            {
                PlaylistID = p[Convert.ToInt32(e.Position)].PlaylistID,
                SongID = GlobalMemory.actualSong.SongID
            };
            
      
           bool isTrue= await APIHelper.PostSongToPlaylist(PS);
            if (isTrue)
                Toast.MakeText(this.Activity, "Dodano do playlisty", ToastLength.Long);
            else
                Toast.MakeText(this.Activity, "Coś poszło nie tak", ToastLength.Long);

            this.Dismiss();
        }
    }
}