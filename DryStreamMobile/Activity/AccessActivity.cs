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
using static Android.Bluetooth.BluetoothClass;
using DryStreamMobile.Helper;

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
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            button = FindViewById<Button>(Resource.Id.noAccesButton);
            button.Click += Button_Click;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    GlobalMemory._user = null;
                    GlobalHelper.switchSavedUser(null);
                    StartActivity(typeof(LoginActivity));
                    this.Finish();
                    
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse(GlobalMemory.serverAddressIP+ "/Home/TransferPage");
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);

        }
    }
}