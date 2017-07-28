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
using DryStreamMobile.Models;
using DryStreamMobile.Holders;

namespace DryStreamMobile.Helper
{
    public class ArtistsAdapter : ArrayAdapter
    {
        private Context c;
        private List<Artist> artists;
        private int resource;
        private LayoutInflater inflater;

        public ArtistsAdapter(Context context, int resource, List<Artist> artists) : base(context, resource, artists)
        {
            this.c = context;
            this.resource = resource;
            this.artists = artists;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null)
                inflater = (LayoutInflater)c.GetSystemService(Context.LayoutInflaterService);

            if (convertView == null)
            {
                convertView = inflater.Inflate(resource, parent, false);
            }
            //BIND DATA
            ArtistHolder holder = new ArtistHolder(convertView);
            holder.TitleTxt.Text = artists[position].Name.Trim();
            holder.Img.SetImageBitmap(GlobalHelper.GetImageBitmapFromUrl(GlobalMemory.serverAddressIP + artists[position].CoverLink.Trim()));
          


            return convertView;
        }
    }
}