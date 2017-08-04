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

namespace DryStreamMobile.Activity
{
    [Activity(Label = "Brak dostępu", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AccessActivity : Android.App.Activity
    {
        Button button;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AccessValidity);
            initControlos();

            // Create your application here
        }

        private void initControlos()
        {
            button = FindViewById<Button>(Resource.Id.noAccesButton);
            button.Click += Button_Click;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Dodac link do strony",ToastLength.Short);
        }
    }
}