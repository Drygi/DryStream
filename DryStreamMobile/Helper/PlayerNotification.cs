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
using Android.Support.V4.App;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace DryStreamMobile.Helper
{
    [BroadcastReceiver(Enabled =true)]
    public class PlayerNotification : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context);

            builder.SetAutoCancel(true)
                .SetLargeIcon(GlobalHelper.GetImageBitmapFromUrl(GlobalMemory.serverAddressIP+ GlobalMemory.actualSong.Album.CoverLink.Trim()))
                .SetContentText(GlobalMemory.actualSong.Name.Trim())
                .SetColor(Color.Rgb(34,34,34))
                .SetContentInfo(GlobalMemory.actualSong.Album.Artist.Name.Trim());

            NotificationManager manager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            manager.Notify(1, builder.Build());
        }
    }
}