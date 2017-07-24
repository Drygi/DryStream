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
    public class NameImage
    {
        public string Name { get; set; }
        public string ImgUrl { get; set; }

        public NameImage()
        {
        }
        public NameImage(string Name, string ImgUrl)
        {
            this.Name = Name;
            this.ImgUrl = ImgUrl;
        }
        public List<NameImage> GetNamgImages(User user)
        {
            List<NameImage> nameImages = new List<NameImage>()
            {
                new NameImage(user.Login,user.CoverLink),
                new NameImage("Playlisty","@drawable/playlistIcon"),
                new NameImage("Gatunki","@drawable/musicIcon"),
                new NameImage("Utwory","@drawable/musicIcon"),
                new NameImage("Moje konto","@drawable/userIconMini"),
                new NameImage("Zmiana hasła","@drawable/passwordIcon"),
                new NameImage("Wyloguj","@drawable/logoutIcon")
            };
            return nameImages;
        } 
    }
}