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
    class SongAdapter : ArrayAdapter
    {
        private Context c; 
        private List<Song> songs;
        private int resource;
        private LayoutInflater inflater;
        public SongAdapter(Context context, int resource, List<Song> objects) : base(context, resource, objects)
        {
            this.c = context;
            this.resource = resource;
            this.songs = objects;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null)
                inflater = (LayoutInflater) c.GetSystemService(Context.LayoutInflaterService);

            if (convertView==null)
            {
                convertView = inflater.Inflate(resource,parent,false);
            }
            //BIND DATA
            SongHolder holder = new SongHolder(convertView);
            holder.TitleTxt.Text = songs[position].Name.Trim();
            holder.Img.SetImageBitmap(GlobalHelper.GetImageBitmapFromUrl(GlobalMemory.serverAddressIP+songs[position].Album.CoverLink));
            holder.ArtistTxt.Text = songs[position].Album.Artist.Name.Trim() + " - " + songs[position].Album.Title;
            holder.durationTxt.Text = $"{ songs[position].Duration.Minutes:00}:{ songs[position].Duration.Seconds:00}";


            return convertView;
        }
    }
}