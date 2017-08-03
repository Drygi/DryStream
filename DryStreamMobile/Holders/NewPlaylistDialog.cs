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
using DryStreamMobile.Helper;

namespace DryStreamMobile.Holders
{
    public class NewPlaylistDialog : DialogFragment
    {
        private EditText editText;
        private Button button;

        public NewPlaylistDialog()
        {
         
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View MyView;
            MyView = inflater.Inflate(Resource.Layout.NewPlaylistDialog, container, false);
            this.Dialog.SetTitle("Nowa Playlista");
            editText = MyView.FindViewById<EditText>(Resource.Id.editText);

            button= MyView.FindViewById<Button>(Resource.Id.btnOk);
            button.Click += BtnOkClick;

            return MyView;
        }

        private async void BtnOkClick(object sender, System.EventArgs e)
        {
            Playlist playlist = new Playlist
            {               
                Name = editText.Text.Trim(),
                UserID = GlobalMemory._user.UserID
            };
            if (GlobalMemory._user.Playlists == null)
            {
                if (await APIHelper.PostPlaylist(playlist))
                {
                    if (await APIHelper.GetPlaylists())
                        Toast.MakeText(this.Activity, "Dodano playlistę", ToastLength.Short).Show();
                    else
                        Toast.MakeText(this.Activity, "Coś poszło nie tak", ToastLength.Short).Show();
                }
            }
            else
            {
                if ((from p in GlobalMemory._user.Playlists where p.Name.Trim().ToUpper() == editText.Text.Trim().ToUpper() select p).ToList().Count > 0)
                {
                    Toast.MakeText(this.Activity, "Playlista o podanej nazwie już istnieje", ToastLength.Short).Show();
                    this.Dismiss();
                    return;
                }
                if (await APIHelper.PostPlaylist(playlist))
                {
                    if (await APIHelper.GetPlaylists())
                    {
                        var p = GlobalMemory._user.Playlists.ToList();
                        PlaylistSong PS = new PlaylistSong
                        {
                            PlaylistID = p.Last().PlaylistID,
                            SongID = GlobalMemory.actualSong.SongID
                        };
                        if (await APIHelper.PostSongToPlaylist(PS))
                        {
                            Toast.MakeText(this.Activity, "Dodano utwór i playlistę", ToastLength.Short).Show();
                            this.Dismiss();
                            return;
                        }
                        Toast.MakeText(this.Activity, "Coś poszło nie tak", ToastLength.Short).Show();
                    }
                    else
                        Toast.MakeText(this.Activity, "Coś poszło nie tak", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this.Activity, "Coś poszło nie tak", ToastLength.Short).Show();
                }
            }
            this.Dismiss();
        }

    }
}