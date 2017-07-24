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

namespace DryStreamMobile.Helper
{
    public class MenuViewModel
    {
        public List<NameImage> NameImages { get; set; }

        public MenuViewModel(User user)
        {
            NameImages = new NameImage().GetNamgImages(user);
        }
    }
}