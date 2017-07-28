﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DryStreamMobile.Helper;

namespace DryStreamMobile.Activity
{
    [Activity(Label = "Gatunki", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class GenresPageActivity : Android.App.Activity
    {
        private ListView listView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GenresPage);
            initcontrolos();
            // Create your application here
     
        }

        private  void initcontrolos()
        {
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            listView = FindViewById<ListView>(Resource.Id.genresListView);

                listView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, GlobalMemory.Genres);

            listView.ItemClick += ListView_ItemClick;

        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this,e.Id.ToString(), ToastLength.Long).Show();
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