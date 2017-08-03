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
        private List<Playlist> playlists;
        private List<Genre> genres;
        private int resource;
        private LayoutInflater inflater;

        public ArtistsAdapter(Context context, int resource, List<Artist> artists) : base(context, resource, artists)
        {
            this.c = context;
            this.resource = resource;
            this.artists = artists;
        }
        public ArtistsAdapter(Context context, int resource, List<Playlist> playlists) : base(context, resource, playlists)
        {
            this.c = context;
            this.resource = resource;
            this.playlists = playlists;
        }
        public ArtistsAdapter(Context context, int resource, List<Genre> genres) : base(context, resource, genres)
        {
            this.c = context;
            this.resource = resource;
            this.genres = genres;
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
            if (artists !=null)
            {
                ArtistHolder holder = new ArtistHolder(convertView);
                holder.TitleTxt.Text = artists[position].Name.Trim();
                holder.Img.SetImageBitmap(GlobalHelper.GetImageBitmapFromUrl(GlobalMemory.serverAddressIP + artists[position].CoverLink.Trim()));
            }
            if(playlists!=null)
            {
                ArtistHolder holder = new ArtistHolder(convertView);
                holder.TitleTxt.Text = playlists[position].Name.Trim();
            }
            if (genres != null)
            {
                ArtistHolder holder = new ArtistHolder(convertView);
                holder.TitleTxt.Text = genres[position].NAME.Trim();
                holder.Img.SetImageResource(Resource.Drawable.GenresIcon);
            }




            return convertView;
        }
    }
}