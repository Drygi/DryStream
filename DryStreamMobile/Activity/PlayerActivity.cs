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
using Plugin.MediaManager;

namespace DryStreamMobile
{
    [Activity(Label = "Odtwarzanie", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PlayerActivity : Android.App.Activity
    {
        private Button next, previous, startStop;
        ImageView img;
        SeekBar seekBar;
        TextView title, artistName;
        bool isPlayed;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Player);
            initControlos();

            // Create your application here
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
        public async void initControlos()
        {
            isPlayed = true;

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            next = FindViewById<Button>(Resource.Id.PlayerNext);
            next.Click += Next_Click;

            previous = FindViewById<Button>(Resource.Id.PlayerPrevious);
            previous.Click += Previous_Click;

            startStop = FindViewById<Button>(Resource.Id.PlayerPlay_Pause);
            startStop.Click += StartStop_Click;

            seekBar = FindViewById<SeekBar>(Resource.Id.PlayerSeekBar);
            //seekBar.ProgressChanged +=(s,e)=> {

            //}

            title = FindViewById<TextView>(Resource.Id.PlayerTitleSong);
            title.Text = GlobalMemory.actualSong.Name;
            artistName = FindViewById<TextView>(Resource.Id.PlayerArtistName);
            artistName.Text = GlobalMemory.actualSong.Album.Title;

            img = FindViewById<ImageView>(Resource.Id.PlayerAlbumCover);
            img.SetImageBitmap(GlobalHelper.GetImageBitmapFromUrl(GlobalMemory.serverAddressIP + GlobalMemory.actualSong.Album.CoverLink));
            await CrossMediaManager.Current.Play(GlobalMemory.serverAddressIP + GlobalMemory.actualSong.Link.Trim());
        }

        private void StartStop_Click(object sender, EventArgs e)
        {
            if (!isPlayed)
            {
                //startStop.SetBackgroundColor(Android.Graphics.Color.ParseColor("@drawable/StopIcon"));
                CrossMediaManager.Current.Play(GlobalMemory.serverAddressIP + GlobalMemory.actualSong.Link.Trim());
                //startStop.SetBackgroundDrawable(Android.Graphics.Drawables();
                isPlayed = true;
                Toast.MakeText(this, "Start", ToastLength.Long).Show();
            }
            else
            {
                isPlayed = false;
                Toast.MakeText(this, "Pauza", ToastLength.Long).Show();
                //startStop.SetBackgroundColor(Android.Graphics.Color.ParseColor("@drawable/PlayIcon"));
                 CrossMediaManager.Current.Pause();
            }
        }

        private void Previous_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}