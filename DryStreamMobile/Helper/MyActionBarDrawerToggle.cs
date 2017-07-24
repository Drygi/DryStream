using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;

namespace DryStreamMobile.Helper
{
    class MyActionBarDrawerToggle : ActionBarDrawerToggle
    {
        private Android.App.Activity mActivity;
        public MyActionBarDrawerToggle(Android.App.Activity activity, DrawerLayout drawerLayout,
            int imageResource, int openDrawerDesc, int closeDrawerDesc)
            : base(activity, drawerLayout, imageResource, openDrawerDesc, closeDrawerDesc)
        {
            mActivity = activity;
        }

        public override void OnDrawerOpened(View drawerView)
        {
            base.OnDrawerOpened(drawerView);
            mActivity.ActionBar.Title = "Menu";
        }

        public override void OnDrawerClosed(View drawerView)
        {
            base.OnDrawerClosed(drawerView);
            mActivity.ActionBar.Title = "";
        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            base.OnDrawerSlide(drawerView, slideOffset);

        }
    }
}