using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Newtonsoft.Json;
using DryStreamMobile.Models;
using DryStreamMobile.Helper;

namespace DryStreamMobile
{
    [Activity(Label = "DryStream", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            ISharedPreferences pref = Application.Context.GetSharedPreferences("savedUser", FileCreationMode.Private);
            string json = pref.GetString("userJson", "");

            if(json=="")
            {
                this.StartActivity(typeof(LoginActivity));
                this.Finish();
            }
            else
            {
                GlobalMemory._user = JsonConvert.DeserializeObject<User>(json);
                this.StartActivity(typeof(AccountActivity));
                this.Finish();
            }
        }

    }
}

