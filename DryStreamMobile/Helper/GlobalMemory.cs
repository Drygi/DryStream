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
    public static class GlobalMemory
    {
        public static string serverAddressIP
        {
            get
            {
                return "http://192.168.1.4:51754";
            }
        }

        public static User _user{ get; set; }

    }
}