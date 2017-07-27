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
using Android.Media;
using static Android.Views.View;
using System.Threading.Tasks;
using Android.Graphics;
using Plugin.MediaManager.Abstractions;
using Plugin.Media.Abstractions;

namespace DryStreamMobile
{
    [  Activity(Label = "Odtwarzanie", LaunchMode =Android.Content.PM.LaunchMode.SingleTop, MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PlayerActivity : Android.App.Activity
    {
        private Button next, previous, startStop;
        ImageView img;
        SeekBar seekBar;
        TextView title, actualTime, allTime;
        Intent intent;
        PendingIntent pendingIntent;
        MediaPlayerService mps;
        Task m;   
       
        bool isPlayed;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Player);
            initControlos();
            this.Title = GlobalMemory.actualSong.Album.Artist.Name;
            this.TitleColor = Android.Graphics.Color.ParseColor("#375a7f");
            myNotification();
            // Create your application herem

        }
        private void myNotification()
        {
            Intent intent = new Intent(this, typeof(PlayerActivity));
            intent.AddFlags(ActivityFlags.FromBackground);
            this.StartActivity(intent);

         
            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.Immutable);
            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(this)
                .SetSubText("Odtwarzanie")
                .SetContentIntent(pendingIntent)
                .SetContentTitle(GlobalMemory.actualSong.Name.Trim())
                .SetContentText(GlobalMemory.actualSong.Album.Artist.Name.Trim())
                .SetSmallIcon(Resource.Drawable.PlayIcon)
              .SetLargeIcon(BitmapFactory.DecodeResource(Resources,Resource.Drawable.playNotification));
               

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
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

            actualTime = FindViewById<TextView>(Resource.Id.PlayerActualTime);
            allTime = FindViewById<TextView>(Resource.Id.PlayerAllTime);
            allTime.Text = GlobalMemory.actualSong.Duration.Minutes + ":" + GlobalMemory.actualSong.Duration.Seconds;

            seekBar = FindViewById<SeekBar>(Resource.Id.PlayerSeekBar);
            seekBar.ProgressChanged += (sender, args) =>
            {
                if (args.FromUser)
                    CrossMediaManager.Current.Seek(TimeSpan.FromSeconds(args.Progress));                
            };

            CrossMediaManager.Current.PlayingChanged += Current_PlayingChanged;
            CrossMediaManager.Current.BufferingChanged += Current_BufferingChanged;
            title = FindViewById<TextView>(Resource.Id.PlayerTitleSong);
            title.Text = GlobalMemory.actualSong.Name;

            img = FindViewById<ImageView>(Resource.Id.PlayerAlbumCover);
            img.SetImageBitmap(GlobalHelper.GetImageBitmapFromUrl(GlobalMemory.serverAddressIP + GlobalMemory.actualSong.Album.CoverLink));
            
            await CrossMediaManager.Current.Play(GlobalMemory.serverAddressIP + GlobalMemory.actualSong.Link.Trim());
            seekBar.Max = Convert.ToInt32(GlobalMemory.actualSong.Duration.TotalSeconds);
        }

        private void Current_BufferingChanged(object sender, Plugin.MediaManager.Abstractions.EventArguments.BufferingChangedEventArgs e)
        {
            seekBar.SecondaryProgress = Convert.ToInt32(e.BufferProgress);
        }

        private void Current_PlayingChanged(object sender, Plugin.MediaManager.Abstractions.EventArguments.PlayingChangedEventArgs e)
        {
            seekBar.Progress = Convert.ToInt32(e.Position.TotalSeconds);
            actualTime.Text = $"{e.Position.Minutes:00}:{e.Position.Seconds:00}";
        }


        private  void StartStop_Click(object sender, EventArgs e)
        {
            
            if (!isPlayed)
            {
              //  await CrossMediaManager.Current.Play(GlobalMemory.serverAddressIP + GlobalMemory.actualSong.Link.Trim());
                startStop.SetBackgroundResource(Resource.Drawable.PauseIcon);

                var z = CrossMediaManager.Current.MediaNotificationManager;
                myNotification();
                CrossMediaManager.Current.Play();
              //  CrossMediaManager.Current.MediaNotificationManager.StartNotification(m.);
                isPlayed = true;
                Toast.MakeText(this, "Start", ToastLength.Short).Show();
               // myNotification();

            }
            else
            {
                isPlayed = false;
                startStop.SetBackgroundResource(Resource.Drawable.PlayIcon);
                Toast.MakeText(this, "Pauza", ToastLength.Short).Show();
                CrossMediaManager.Current.Pause();
                CrossMediaManager.Current.MediaNotificationManager.StopNotifications();
            }
        }
        private void Previous_Click(object sender, EventArgs e)
        {
        }

        private void Next_Click(object sender, EventArgs e)
        {
           
        }
    }
}