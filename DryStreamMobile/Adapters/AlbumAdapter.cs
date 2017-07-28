using System;
using System.Collections;
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
    class AlbumAdapter : ArrayAdapter
    {
        private Context c;
        private List<Album> albums;
        private int resource;
        private LayoutInflater inflater;
        public AlbumAdapter(Context context, int resource, List<Album> albums) : base(context, resource, albums)
        {
            this.c = context;
            this.resource = resource;
            this.albums = albums;
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
            SongHolder holder = new SongHolder(convertView);
            holder.TitleTxt.Text = albums[position].Title;
            holder.Img.SetImageBitmap(GlobalHelper.GetImageBitmapFromUrl(GlobalMemory.serverAddressIP + albums[position].CoverLink));
            holder.ArtistTxt.Text = albums[position].Artist.Name;
            holder.durationTxt.Text = albums[position].Year;


            return convertView;
        }
    }
}