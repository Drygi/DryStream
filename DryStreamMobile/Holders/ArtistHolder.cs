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

namespace DryStreamMobile.Holders
{
    public class ArtistHolder
    {
        public TextView TitleTxt, ArtistTxt;
        public ImageView Img;
        public ArtistHolder(View itemView)
        {
            Img = itemView.FindViewById<ImageView>(Resource.Id.artistsModelImgID);
            TitleTxt = itemView.FindViewById<TextView>(Resource.Id.artistsModelNameID);
           
        }
    }
}