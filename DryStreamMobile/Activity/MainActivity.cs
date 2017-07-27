using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using DryStreamMobile.Activity;
using Newtonsoft.Json;
using DryStreamMobile.Models;
using DryStreamMobile.Helper;

namespace DryStreamMobile
{
    [Activity(Label = "DryStream", MainLauncher = true, Icon = "@drawable/playNotification", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Android.App.Activity
    {
        private ProgressBar progressBar;
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            progressBar = FindViewById<ProgressBar>(Resource.Id.mainProgressBar);

            var genres = await APIHelper.getGenres();
            if (genres != null)
            {
                GlobalMemory.Genres = genres;
                ISharedPreferences pref = Application.Context.GetSharedPreferences("savedUser", FileCreationMode.Private);
                string json = pref.GetString("userJson", "");

                if (json == "")
                {
                    this.StartActivity(typeof(LoginActivity));
                    this.Finish();
                }
                else
                {
                    GlobalMemory._user = JsonConvert.DeserializeObject<User>(json);
                    this.StartActivity(typeof(MainPageActivity));
                    this.Finish();
                }
            }
            else
            {

            }


        }

    }
}

