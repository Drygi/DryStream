﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DryStreamMobile.Helper
{
    class MyHolder
    {
        public TextView TitleTxt, ArtistTxt, durationTxt;
        public ImageView Img;

        public MyHolder(View itemView)
        {
            Img = itemView.FindViewById<ImageView>(Resource.Id.modelImgID);
            TitleTxt = itemView.FindViewById<TextView>(Resource.Id.modelNameID);
            ArtistTxt = itemView.FindViewById<TextView>(Resource.Id.modelDescriptionID);
            durationTxt = itemView.FindViewById<TextView>(Resource.Id.modelRighttxtID);
        }
    }
}