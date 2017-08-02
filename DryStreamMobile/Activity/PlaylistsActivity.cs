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

namespace DryStreamMobile.Activity
{
    [Activity(Label = "Playlisty", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PlaylistsActivity : Android.App.Activity
    {
        ListView listView;
        TextView textView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PlaylistsPage);
            initControls();
            // Create your application here
        }

        private void initControls()
        {
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            listView = FindViewById<ListView>(Resource.Id.playlistsListView);
            textView = FindViewById<TextView>(Resource.Id.emptyPlayliststLV);
            if (GlobalMemory._user.Playlists==null)
            {
                listView.Visibility = ViewStates.Gone;
            }
            else
            {
                textView.Visibility = ViewStates.Gone;  
                listView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, GlobalMemory._user.Playlists.ToList());
              //  listView.Click += ListView_Click;
               // listView.LongClick += ListView_LongClick;
            }
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
        private void ListView_LongClick(object sender, View.LongClickEventArgs e)
        {

        }

        private void ListView_Click(object sender, EventArgs e)
        {

        }
    }
}