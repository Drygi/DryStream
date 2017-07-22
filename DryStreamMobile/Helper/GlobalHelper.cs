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
using System.Security.Cryptography;
using Android.Graphics;
using System.Net;
using DryStreamMobile.Models;
using Newtonsoft.Json;

namespace DryStreamMobile.Helper
{
    public static class GlobalHelper
    {
        public static string GenerateSHA512(string value)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);  
        }
        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        public static Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var client = new WebClient())
            {
                var imageBytes = client.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        //to na pozniej
        public static void switchSavedUser(User user)
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("savedUser", FileCreationMode.Private);
            ISharedPreferencesEditor edit = pref.Edit();
            edit.Clear();  
            edit.PutString("userJson", JsonConvert.SerializeObject(user));
            edit.Apply();
            GlobalMemory._user = user;
        }
        public static bool isSavedUser()
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("savedUser", FileCreationMode.Private);
            string json = pref.GetString("userJson", "");

            if (json == "")
                return false;
            else
                return true;

        }
    }
}